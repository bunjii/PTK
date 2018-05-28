using feb;
using GH_IO.Serialization;
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
using System.Diagnostics;
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
            pManager.AddParameter(new Param_Model(), "Assembled Model", "Assembled Model", "", GH_ParamAccess.item);
            pManager.AddParameter(new Param_Model(), "Analyzed Model", "Analyzed Model", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Displacement", "Disp", "Maximum displacement in [m]", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gravity force", "G", "Resulting force of gravity [kN] of each load-case of the model", GH_ParamAccess.list);
            pManager.AddNumberParameter("Strain Energy", "Energy", "Internal elastic energy in [kNm of each load cases of the model", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            Assembly assemble;
            List<Node> nodes;
            List<Element> elems;
            List<Material> mats;
            List<Section> secs;
            List<Support> sups;

            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            List<Karamba.Loads.Load> kload;
            List<GH_Load> gInLoad = new List<GH_Load>();
            bool flgNewSolution = false;

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

            // Create Pt List
            List<Point3d> pts = new List<Point3d>();
            for (int i = 0; i < nodes.Count; i++)
            {
                pts.Add(nodes[i].Pt3d);
            }

            // Support Information for Karamba Assemble
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

                // tentatively "Wood"
                string kMFamily = "Wood";
                string kMName = mats[i].MatName;
                double kME = 1000 * mats[i].Properties.EE0gmean;
                double kMG = 1000 * mats[i].Properties.GGgmean;
                double kMGamma = 0.1 * mats[i].Properties.Rhogmean;
                double kMFy = 1000 * mats[i].Properties.Fmgk;
                double kMAlphaT = 5.00E-06;
                // kMAlphaT: temporal value for general wood. might need be confirmed.

                fMat.setMaterialProperties(kMFamily, kMName, kME, kMG, kMGamma, kMFy, kMAlphaT);

                // associating Element Tag to a karamba Material
                // be aware of duplication 
                List<string> tagLst = new List<string>();
                for (int j = 0; j < mats[i].ElemIds.Count; j++)
                {
                    string elemTag = "N/A";
                    try
                    {
                        elemTag = Element.FindElemById(elems, mats[i].ElemIds[j]).Tag;
                    }
                    catch (NullReferenceException e)
                    {
                        flgNewSolution = true;
                    }
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
                    secs[i].Height * 100, secs[i].Width * 100, secs[i].Width * 100);

                List<string> tagLst = new List<string>();
                for (int j = 0; j < secs[i].ElemIds.Count; j++)
                {
                    string elemTag = "N/A";
                    try
                    {
                        elemTag = Element.FindElemById(elems, secs[i].ElemIds[j]).Tag;
                    }
                    catch (NullReferenceException e)
                    {
                        flgNewSolution = true;
                    }

                    if (tagLst.Contains(elemTag)) continue;

                    tagLst.Add(elemTag);
                    cSec.AddElemId(elemTag);
                }
                kCroSec.Add(cSec);
            }

            // Elem for Karamba Assemble
            // Create modelbeam element, too.
            List<GrassElement> grElems = new List<GrassElement>();
            List<ModelBeam> mbs = new List<ModelBeam>();
            for (int i = 0; i < elems.Count; i++)
            {
                for (int j = 0; j < elems[i].SubElem.Count; j++)
                {
                    // making of a grass beam
                    Point3d _sPt = elems[i].SubElem[j].StrLn.From;
                    Point3d _ePt = elems[i].SubElem[j].StrLn.To;
                    GrassBeam _gb = new GrassBeam(_sPt, _ePt);
                    _gb.id = elems[i].Tag;

                    // sets the orientation of the element:
                    _gb.x_ori = elems[i].localYZPlane.ZAxis;
                    _gb.z_ori = elems[i].localYZPlane.YAxis;

                    grElems.Add(_gb);
                }
            }


            List<ElemSet> _esets = new List<ElemSet>();
            Karamba.Models.Model kmodel = new Karamba.Models.Model();
            string aInfo; string aMsg; double aMass; Point3d aCoG; GH_RuntimeMessageLevel aLvl;

            List<Karamba.Loads.Load> modelLoads = new List<Karamba.Loads.Load>();
            modelLoads.Add(new Karamba.Loads.GravityLoad(new Vector3d(0, 0, -1.0), 0));

            // Equivalent to karamba assemble component
            try
            {
                Karamba.Models.Component_AssembleModel.solve(pts, grElems, cSups, modelLoads,
                                kCroSec, kMat, _esets, 0.005, out kmodel, out aInfo, out aMass,
                                out aCoG, out aMsg, out aLvl);
            }
            catch (Exception e)
            {
                flgNewSolution = true;
                Debug.WriteLine(e.ToString());
            }

            // trial for analysis = inside of component 
            List<double> lc_max_disp = new List<double>();
            List<double> lc_gravity_force = new List<double>();
            List<double> lc_elastic_energy = new List<double>();
            Karamba.Models.Model outModel = new Karamba.Models.Model();

            try
            {
                outModel = (Karamba.Models.Model)kmodel.Clone();
                feb.Model febModel = outModel.febmodel;
                // febModel = outModel.
                Deform deform = new Deform(febModel); // error
                Response response = new Response(deform);
                Utils.handleError(response.updateMemberForces(), deform);
                outModel.maxDisp = 0.0;

                for (int i = 0; i < outModel.numLC; i++)
                {
                    double num = response.maxDisplacement(i);
                    outModel.maxDisp = Math.Max(outModel.maxDisp, num);
                    lc_max_disp.Add(num);

                    double item = deform.deadWeight((uint)i);
                    lc_gravity_force.Add(item);

                    EnergyVisitor energyVisitor = new EnergyVisitor(outModel.febmodel,
                        outModel.febmodel.state((uint)i), (uint)i);
                    energyVisitor.visit(outModel.febmodel);

                    double item2 = energyVisitor.elasticEnergy();
                    lc_elastic_energy.Add(item2);
                }

                deform.Dispose();
                response.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                flgNewSolution = true;
            }

            GH_Model gma = new GH_Model(kmodel);
            GH_Model gm = new GH_Model(outModel);

            if (flgNewSolution == true)
            {
                // ExpireSolution(true);
                GH_Document doc = this.OnPingDocument();
                Debug.WriteLine("### new solution ###");
                doc.NewSolution(true);
            }

            #endregion

            #region output
            DA.SetData(0, gma);
            DA.SetData(1, gm);
            DA.SetDataList(2, lc_max_disp);
            DA.SetDataList(3, lc_gravity_force);
            DA.SetDataList(4, lc_elastic_energy);

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