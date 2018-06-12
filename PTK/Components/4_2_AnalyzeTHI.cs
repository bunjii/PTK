using feb;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Karamba.Models;
using Karamba.Elements;

namespace PTK
{
    public class PTK_4_2_AnalyzeTHI : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_4_1_KarambaAssemble class.
        /// </summary>
        public PTK_4_2_AnalyzeTHI()
          : base("Analyze THI (PTK)", "Analyze THI (PTK)",
              "Adding calculated model to the PTK assembly",
              CommonProps.category, CommonProps.subcat4)
        {
            Message = CommonProps.initialMessage;

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "PTK Assembly", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "Assembled project data", GH_ParamAccess.item);
            pManager.RegisterParam(new Karamba.Models.Param_Model(), "Analyzed Model", "Analyzed Model", "", GH_ParamAccess.item);
            
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            Assembly assemble;
            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            
            Karamba.Models.GH_Model in_gh_model = null;
            string singular_system_msg = "singular stiffness matrix";
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; }
            wrapAssembly.CastTo<Assembly>(out assemble);
            List<Karamba.Models.GH_Model> wrapModelList = new List<Karamba.Models.GH_Model>();
            #endregion

            #region solve
            foreach (var m in assemble.Krmb_GH_model)
            {
                wrapModelList.Add(m);
            }

            // right now the nalysis will be justt for the firdt GH_model in the list
            // we hve to rethink if one assembly can have many GH_models
            in_gh_model = wrapModelList[0];
            Karamba.Models.Model model = in_gh_model.Value;

            //clone model to avoid side effects
            model = (Karamba.Models.Model)model.Clone() ;

            //clone its elements to avoid side effects
            model.cloneElements() ;

            //clone the feb model
            model.deepCloneFEModel();


            feb.Deform deform = new feb.Deform(model.febmodel);
            feb.Response response = new feb.Response(deform);

            try
            {
                //calculate displacements
                response.updateNodalDisplacements() ;
                //calculate the member forces
                response.updateMemberForces() ;
            }
            catch
            {
                 // s end an e r r o r mes sage i n c a s e some thing went wrong
                 throw new Exception(singular_system_msg);
            }


            Karamba.Models.GH_Model outKrmbModel = new Karamba.Models.GH_Model(model);
            var gm_list = new List<Karamba.Models.GH_Model> { outKrmbModel };

            Assembly Assembly = new Assembly(assemble.Nodes, assemble.Elems,
                assemble.Mats, assemble.Secs, assemble.Sups,
                assemble.Loads, gm_list);


            #endregion

            #region output
            DA.SetData(0, Assembly);
            DA.SetData(1, outKrmbModel);
            

            #endregion
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return PTK.Properties.Resources.ico_exportKaramba;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7c6860fa-ee7b-4580-9f04-7bab9e325d8b"); }
        }
    }
}