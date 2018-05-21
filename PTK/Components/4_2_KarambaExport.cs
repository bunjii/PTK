using feb;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Karamba;
using Karamba.Algorithms;
using Karamba.Algorithms.BESOShell;
using Karamba.Algorithms.Deprecated;
using Karamba.Algorithms.GUI;
using Karamba.CrossSections;
using Karamba.CrossSections.Deprecated;
using Karamba.CrossSections.GUI;
using Karamba.Elements;
using Karamba.Elements.Deprecated;
using Karamba.Elements.GUI;
using Karamba.Exporters;
using Karamba.Exporters.Deprecated;
using Karamba.Exporters.GUI;
using Karamba.Licenses;
using Karamba.Loads;
using Karamba.Loads.Deprecated;
using Karamba.Loads.GUI;
using Karamba.Materials;
using Karamba.Materials.Deprecated;
using Karamba.Materials.GUI;
using Karamba.Models;
using Karamba.Models.Deprecated;
using Karamba.Models.GUI;
using Karamba.Nodes;
using Karamba.Results;
using Karamba.Results.Deprecated;
using Karamba.Results.GUI;
using Karamba.Supports;
using Karamba.Supports.Deprecated;
using Karamba.Supports.GUI;
using Karamba.Utilities;
using Karamba.Utilities.AABBTrees;
using Karamba.Utilities.Components;
using Karamba.Utilities.Deprecated;
using Karamba.Utilities.Geometry.Mesh;
using Karamba.Utilities.GUI;
using Karamba.Utilities.Mappings;
using Karamba.Utilities.Mappings.GUI;
using Karamba.Utilities.UIWidgets;
using Karamba.Utilities.UIWidgets.switcher;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PTK
{
    public class PTK_4_2_KarambaExport : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_4_1_KarambaAssemble class.
        /// </summary>
        public PTK_4_2_KarambaExport()
          : base("Karamba Export (PTK)", "Karamba Export",
              "Creates Model information of Karamba",
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
            pManager.AddParameter(new Param_Load(), "Load", "Load", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_Element(), "Elem", "Elem", "", GH_ParamAccess.list);
            pManager.AddParameter(new Param_Support(), "Support", "Support", "", GH_ParamAccess.list);
            pManager.AddParameter(new Param_Load(), "Load", "Load", "", GH_ParamAccess.list);
            pManager.AddParameter(new Param_CrossSection(), "Cross section", "CroSec", "", GH_ParamAccess.list);
            pManager.AddParameter(new Param_FemMaterial(), "Material", "Material", "", GH_ParamAccess.list);

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
            List<Material> mats = new List<Material>();
            List<Section> secs = new List<Section>();
            List<Support> sups = new List<Support>();

            List<Karamba.Loads.Load> kload = new List<Karamba.Loads.Load>();
            List<GH_Load> gInLoad = new List<GH_Load>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; }
            DA.GetDataList(1, gInLoad);
            #endregion

            #region solve

            wrapAssembly.CastTo<Assembly>(out assemble);

            nodes = assemble.Nodes;
            elems = assemble.Elems;
            mats = assemble.Mats;
            secs = assemble.Secs;
            sups = assemble.Sups;

            // Support for Karamba Assemble
            List<Karamba.Supports.Support> cSups = new List<Karamba.Supports.Support>();
            for (int i = 0; i < sups.Count; i++)
            {
                int id = sups[i].Id;
                List<bool> cond = sups[i].Conditions.ToList();
                Plane pln = sups[i].Pln;
                Point3d pt = pln.Origin;
                cSups.Add(new Karamba.Supports.Support(pt, cond, pln));

            }

            // Material for Karamba Assemble
            // Unit conversion:
            // E,G,f: N/mm2 -> kN/cm2 (x 1000)
            // rho: kg/m3 -> kN/m3 (x 0.1) 
            List<FemMaterial> kMat = new List<FemMaterial>();
            for (int i = 0; i < mats.Count; i++)
            {
                if (mats[i].ElemIds.Count == 0)
                {
                    continue;
                }

                FemMaterial fMat = new FemMaterial();

                string kMFamily = "Wood";       // tentatively "Wood"
                string kMName = mats[i].MatName;
                double kME = 1000 * mats[i].Properties.EE0gmean;
                double kMG = 1000 * mats[i].Properties.GGgmean;
                double kMGamma = 0.1 * mats[i].Properties.Rhogmean;
                double kMFy = 1000 * mats[i].Properties.Fmgk;
                double kMAlphaT = 5.00E-06;     // temporal value for general wood. might need be confirmed.

                fMat.setMaterialProperties(kMFamily, kMName, kME, kMG, kMGamma, kMFy, kMAlphaT);

                // associating Element Tag to a karamba Material
                // be aware of duplication 
                List<string> tagLst = new List<string>();
                for (int j = 0; j < mats[i].ElemIds.Count; j++)
                {
                    string elemTag = Element.FindElemById(elems, mats[i].ElemIds[j]).Tag;

                    if (tagLst.Contains(elemTag)) continue;

                    tagLst.Add(elemTag);
                    // associate element Tag (PTK) to beam Id (Karamba)
                    fMat.AddBeamId(elemTag);
                }
                kMat.Add(fMat);
            }

            // cross-sections for Karamba Assemble
            List<CroSec> kCroSec = new List<CroSec>();
            for (int i = 0; i < secs.Count; i++)
            {
                // in case there is no element attached to the section, continue to next loop.
                if (secs[i].ElemIds.Count == 0) continue;

                CroSec cSec = new CroSec_Trapezoid("Trapezoid", secs[i].SectionName, "",
                    secs[i].Height * 100, secs[i].Width * 100, secs[i].Width * 100); // or CroSec_Trapezoid()?

                List<string> tagLst = new List<string>();
                for (int j = 0; j < secs[i].ElemIds.Count; j++)
                {
                    string elemTag = Element.FindElemById(elems, secs[i].ElemIds[j]).Tag;

                    if (tagLst.Contains(elemTag)) continue;

                    tagLst.Add(elemTag);
                    cSec.AddElemId(elemTag);
                }
                kCroSec.Add(cSec);
            }

            // Elem for Karamba Assemble
            List<GrassElement> grElems = new List<GrassElement>();
            for (int i = 0; i < elems.Count; i++)
            {
                for (int j = 0; j < elems[i].SubStructural.Count; j++)
                {
                    // making of a grass beam
                    Point3d sPt = elems[i].SubStructural[j].StrctrLine.From;
                    Point3d ePt = elems[i].SubStructural[j].StrctrLine.To;
                    GrassBeam gb = new GrassBeam(sPt, ePt);
                    gb.id = elems[i].Tag;

                    //  // sets the orientation of the element:
                    gb.x_ori = elems[i].localYZPlane.ZAxis;
                    gb.z_ori = elems[i].localYZPlane.YAxis;

                    grElems.Add(gb);

                    // making of a model beam
                    int ind = elems[i].Id;
                    //  // gb will be used
                    FemMaterial fmat = kMat[elems[i].MatId];
                    CroSec csec = kCroSec[elems[i].SecId];

                    // elems[i].SubStructural.
                    // in need of substructural's node ids

                    // ModelBeam mb = new ModelBeam(ind, gb, fmat, csec, , );

                }
            }

            // preparation for output
            List<GH_Element> gElemLst = new List<GH_Element>();
            foreach (GrassElement ge in grElems)
            {
                GH_Element ghe = new GH_Element(ge);
                gElemLst.Add(ghe);
            }

            List<GH_Support> gSupLst = new List<GH_Support>();
            foreach (Karamba.Supports.Support cs in cSups)
            {
                GH_Support gsup = new GH_Support(cs);
                gSupLst.Add(gsup);
            }

            List<GH_Load> gLoadLst = new List<GH_Load>(gInLoad);

            List<GH_CrossSection> gCSLst = new List<GH_CrossSection>();
            foreach (CroSec cs in kCroSec)
            {
                GH_CrossSection gcs = new GH_CrossSection(cs);
                gCSLst.Add(gcs);
            }

            List<GH_FemMaterial> gMatLst = new List<GH_FemMaterial>();
            foreach (FemMaterial fm in kMat)
            {
                GH_FemMaterial ghm = new GH_FemMaterial(fm);
                gMatLst.Add(ghm);
            }

            Karamba.Models.Model kmodel = new Karamba.Models.Model();
            // Functions_DDL.CreateKarambaModelElement(grElems); // , ref kmodel);




            #endregion

            #region output
            DA.SetDataList(0, gElemLst);
            DA.SetDataList(1, gSupLst);
            DA.SetDataList(2, gLoadLst);
            DA.SetDataList(3, gCSLst);
            DA.SetDataList(4, gMatLst);
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