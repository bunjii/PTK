using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

namespace PTK.Components
{
    public class PTK_2_SubElement : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _2_SubElement class.
        /// </summary>
        public PTK_2_SubElement()
          : base("SubElement", "SubElem",
              "creates a sub element",
              CommonProps.category, CommonProps.subcate2)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Add name to the sub-element.", GH_ParamAccess.item);
            pManager.AddParameter(new Param_MaterialProperty(), "Material properties", "M", "Add material properties", GH_ParamAccess.list);
            pManager.AddParameter(new Param_CroSec(), "Cross sections", "S", "Add cross sections", GH_ParamAccess.list);
            pManager.AddParameter(new Param_Alignment(), "Alignments", "A", "Add alignments", GH_ParamAccess.list);
            
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_SubElement(), "SubElement", "SE", "PTK Sub-elements", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            /////////////////////////////////////////////////////////////////////////////////
            // variables
            /////////////////////////////////////////////////////////////////////////////////

            string name = null;
            List<GH_MaterialProperty> gMaterialProperties = new List<GH_MaterialProperty>();
            List<MaterialProperty> materialProperties = null;
            List<GH_CroSec> gCrossSections = new List<GH_CroSec>();
            List<CrossSection> crossSections = null;
            List<GH_Alignment> gAlignments = new List<GH_Alignment>();
            List<Alignment> alignments = null;

            /////////////////////////////////////////////////////////////////////////////////
            // input
            /////////////////////////////////////////////////////////////////////////////////

            if(!DA.GetData(0, ref name)) { return; }
        
            if (!DA.GetDataList(1, gMaterialProperties))
            {
                materialProperties = new List<MaterialProperty>();
            }
            else
            {
                materialProperties = gMaterialProperties.ConvertAll(m => m.Value);
            }

            if (!DA.GetDataList(2, gCrossSections))
            {
                crossSections = new List<CrossSection>();
            }
            else
            {
                crossSections = gCrossSections.ConvertAll(c => c.Value);
            }

            if (!DA.GetDataList(3, gAlignments))
            {
                alignments = new List<Alignment>();
            }
            else
            {
                alignments = gAlignments.ConvertAll(a => a.Value);
            }

            /////////////////////////////////////////////////////////////////////////////////
            // solve
            /////////////////////////////////////////////////////////////////////////////////

            GH_SubElement subElement = new GH_SubElement(
                                            new SubElement(
                                                            name, 
                                                            materialProperties,
                                                            crossSections,
                                                            alignments
                                                           ));

            /////////////////////////////////////////////////////////////////////////////////
            // output
            /////////////////////////////////////////////////////////////////////////////////
            DA.SetData(0, subElement);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9c4880e6-f925-484b-9ec1-cf5cf466d417"); }
        }
    }
}