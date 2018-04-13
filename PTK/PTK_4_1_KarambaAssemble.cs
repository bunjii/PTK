using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Windows.Forms;

using Karamba.Algorithms;
using Karamba.Utilities.UIWidgets;
using Karamba.Algorithms.GUI;
using Karamba.Algorithms.BESOShell;
using Karamba.Utilities.Geometry.Mesh;
using Karamba.CrossSections;
using Karamba.Elements;
using Karamba.Algorithms.Deprecated;
using Karamba.CrossSections.GUI;
using Karamba.CrossSections.Deprecated;
using Karamba.Loads.GUI;
using Karamba.Elements.GUI;
using Karamba.Elements.Deprecated;
using Karamba.Loads;
using Karamba.Loads.Deprecated;
using Karamba.Materials.GUI;
using Karamba.Materials.Deprecated;
using Karamba.Models.GUI;
using Karamba;
using Karamba.Models;
using Karamba.Models.Deprecated;
using Karamba.Results;
using Karamba.Results.GUI;
using Karamba.Results.Deprecated;
using Karamba.Supports.GUI;
using Karamba.Supports;
using Karamba.Exporters;
using Karamba.Licenses;
using Karamba.Exporters.GUI;
using Karamba.Exporters.Deprecated;
using Karamba.Utilities;
using feb;
using Karamba.Utilities.GUI;
using Karamba.Utilities.Deprecated;
using Karamba.Utilities.Components;
using Karamba.Utilities.Mappings.GUI;
using Karamba.Utilities.Mappings;
using Karamba.Materials;
using Karamba.Supports.Deprecated;
using Karamba.Utilities.AABBTrees;
using Karamba.Nodes;
using Karamba.Utilities.UIWidgets.switcher;

namespace PTK
{
    public class PTK_4_1_KarambaAssemble : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_4_1_KarambaAssemble class.
        /// </summary>
        public PTK_4_1_KarambaAssemble()
          : base("KarambaAssemble", "Karamba A (PTK)",
              "Creates Model information of Karamba",
              "PTK", "Assemble")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "PTK A", "PTK Assembly", GH_ParamAccess.item);
            pManager.AddParameter(new Param_FemMaterial(), "karamMat", "karamMat", "", GH_ParamAccess.item);
            pManager.AddParameter(new Param_Model(), "karamMdl", "karamMdl", "", GH_ParamAccess.item);
            // pManager.AddLineParameter("lines", "lines", "", GH_ParamAccess.list);
            // pManager.AddParameter(new Param_Model(), "karamM", "karamM", "", GH_ParamAccess.item);

            pManager[2].Optional = true;

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_Model(), "outModel", 
                "outModel", "Assembled Karamba Model", GH_ParamAccess.item);
            pManager.AddParameter(new Param_Element(), "element", "element", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            GH_Model in_gh_model = null;

            #region variables
            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            Assembly assemble;
            List<Node> nodes = new List<Node>();
            List<Element> elems = new List<Element>();
            // List<Karamba.Elements.GH_Element> karamBeam = new List<GH_Element>();
            Karamba.Materials.GH_FemMaterial karamMat = new Karamba.Materials.GH_FemMaterial();
            List<Line> lines = new List<Line>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; }
            if (!DA.GetData(1, ref karamMat)) { return; }
            if (!DA.GetData(2, ref in_gh_model)) { return; }
            #endregion

            #region solve

            wrapAssembly.CastTo<Assembly>(out assemble);

            nodes = assemble.Nodes;
            elems = assemble.Elems;

            Karamba.Models.Model model = in_gh_model.Value;

            List<GrassElement> gElems = new List<GrassElement>();

            for (int i = 0; i < elems.Count; i++)
            {
                Point3d sPt = elems[i].PointAtStart;
                Point3d ePt = elems[i].PointAtEnd;
                gElems.Add(new GrassBeam(sPt, ePt));
            }

            List<FemMaterial> kMat = new List<FemMaterial>();

            model = (Karamba.Models.Model)model.Clone();
            model.cloneElements();
            model.cloneNodes();
            model.clonePointLoads();
            model.cloneSupportPosition();
            model.cloneVertexPosition();
            model.deepCloneFEModel();

            List<Karamba.Nodes.Node> vertices = model.nodes;
            List<FemMaterial> materials = model.materials;
            List<CroSec> crosecs = model.crosecs;
            List<Karamba.Elements.ModelElement> elements = model.elems;
            List<PointLoad> ploads = model.ploads;
            List<PointMass> pmass = model.pmass;
            List<MeshLoad> mloads = model.mloads;
            List<ElementLoad> eloads = model.eloads;
            List<Karamba.Supports.Support> supports = model.supports;
            List<ElemSet> bsets = model.beamsets;
            Dictionary<int, GravityLoad> gl = model.gravities;

            // ???
            // model.
            ItemSelector sel = new ItemSelector();
            IdManager iman = new IdManager();
            ///

            int numLC = model.numLC;

            Karamba.Models.Model newModel = new Karamba.Models.Model();
            // feb.Model febmodel = new feb.Model();
            // add nodes
            foreach (Karamba.Nodes.Node v in vertices)
            {
                newModel.add(v);
                // newmodel.nodes.Add(v);
            }
            
            // add material
            /*
            MessageBox.Show(materials.Count.ToString());
            foreach (FemMaterial m in materials)
            {
                MessageBox.Show(m.E.ToString());
                newmodel.materials.Add(m);
            }
            
            // add cross-sections
            foreach (CroSec s in crosecs)
            {
                newmodel.crosecs.Add(s);
            }
            */

            // add elements
            foreach (Karamba.Elements.ModelElement e in elements)
            {

                newModel.elems.Add(e);
            }
            
            
            // add boundary conditions
            foreach (Karamba.Supports.Support s in supports)
            {
                // newmodel.supports.Add(s);
                // newmodel.
                // newmodel.supports.Add(s);
                // s.addTo(newmodel, newmodel.febmodel);
            }


            newModel.gravities = gl;

            // newmodel.buildFEModel();

            Karamba.Models.Model nM =
                new Karamba.Models.Model(vertices, materials, crosecs,
               elements, ploads, pmass, mloads, eloads, supports, bsets, gl, sel, iman, numLC);

            /*
                = new Karamba.Models.Model(vertices, materials, crosecs,
               elements, ploads, pmass, mloads, eloads, supports, bsets, gl, sel, iman, numLC);
            */

            // string tmp = "";
            /*
            for (int i = 0; i < model.gravities.Count; i++)
            {
                tmp += model.gravities[i].ToString() + ', ';
            }
            */
            // tmp += model.gravities[0].toString(); // + ", " + model.gravities[1].toString();
            // MessageBox.Show(model.gravities.Count.ToString());

            /*
            // 

            foreach (KeyValuePair<int, Karamba.Loads.GravityLoad> item in model.gravities)
            {
                
                MessageBox.Show($"{item.Key} {item.Value.force.ToString()}, "); 
            }
            */

            // MessageBox.Show(model.element_selector..ToString());



            // Karamba.Elements.ModelBeam 
            /*
             * 
            public Model(

	        List<Node> _vertices,               // model.nodes;
	        List<FemMaterial> _materials,       // model.materials;
	        List<CroSec> _crosecs,              // model.crosecs;
	            List<ModelElement> _elems,      // model.elems;
	        List<PointLoad> _point_loads,       // model.ploads;
	        List<PointMass> _point_masses,      // model.pmass;
	        List<MeshLoad> _mesh_loads,         // model.mloads;
	        List<ElementLoad> _element_loads,   // model.eloads;
	        List<Support> _supports,            // model.supports;
	        List<ElemSet> bsets,                // model.beamsets;
	        Dictionary<int, GravityLoad> g,     // model.gravities;

	        ItemSelector elem_id2elem_ind,      // model.element_selector// ?? 
	        IdManager id_map,                   // ??
	        int numLC                           // model.numLC;
            )
 
            */


            #endregion

            #region output
            GH_Model outModel = new GH_Model(nM);
            List<GH_Element> gElem = new List<GH_Element>();
            foreach (GrassElement ge in gElems)
            {
                GH_Element ghe = new GH_Element(ge);
                gElem.Add(ghe);
            }
            
            DA.SetData(0, outModel);
            DA.SetDataList(1, gElem);
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
                return null;
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