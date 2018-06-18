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

            // right now the analysis will be just for the first GH_model in the list
            // we have to rethink if one assembly can have many GH_models
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

            Assembly OutAssembly = Add_Forces_From_Karamba(assemble, model);
            Karamba.Models.GH_Model outKrmbModel = new Karamba.Models.GH_Model(model);
            var gm_list = new List<Karamba.Models.GH_Model> { outKrmbModel };

            OutAssembly = new Assembly(assemble.Nodes, assemble.Elems,
                assemble.Mats, assemble.Secs, assemble.Sups,
                assemble.Loads, gm_list);

            #endregion

            #region output
            DA.SetData(0, OutAssembly);
            DA.SetData(1, gm_list[0] );
           

            #endregion
        }

        public Assembly Add_Forces_From_Karamba(Assembly assembly, Karamba.Models.Model model)
        {
            // this method is adding the results from Karamba analysis to thr PTK_Force class of the class PTK_Element

            Assembly assemble = assembly;  //assembly from input

            /// exporting forces to elements   
            // creating the list of ids which will be calculated
            List<string> id_list = new List<string>();
            foreach (var e in model.elems)
            {
                id_list.Add(e.id);      // loop over !!karamba!! elements, to take their id
            }
            
            #region result sorting
            /// The result sorting algorithm
            /// The alghorithm is using results reading in points
            /// problem description!: 

            double maximum_distance_bt_points = 1;
            int maximum_num_points = 20;

            List<List<List<List<Double>>>> results = new List<List<List<List<double>>>>();
            Karamba.Results.Component_BeamForces.solve(
                model,
                id_list,
                -1,
                maximum_distance_bt_points,
                maximum_num_points,
                out results
                );

            // Lists for storing forces

            int load_case_id = -1;
            int element_id;

            foreach (var result_element_list in results)
            {
                load_case_id++;
                /// This loop is over load cases
                /// Please be aware of that:
                /// if you have more than 1 load case the maximum values will be simplified to one case
                /// so if you have 3 loadcases, 
                /// in PTK_element should be PTK_Force from only one case which give the biggest values
                element_id = -1;

                foreach (var result_point_list in result_element_list)
                {
                    /// this loop is over elements in the model

                    double N1_tension_max = 0;
                    double N1_compression_max = 0;
                    double N2_max = 0;
                    double N3_max = 0;

                    double M1_max = 0;
                    double M2_max = 0;
                    double M3_max = 0;

                    element_id++;
                    
                    var N1_compression_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };
                    var N1_tension_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };

                    var N2_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };
                    var N3_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };

                    var M1_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };
                    var M2_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };
                    var M3_list_element = new List<double>() { 0, 0, 0, 0, 0, 0 };

                    int id_points = -1;
                    PTK_Forces tmpForces = new PTK_Forces(
                        N1_compression_list_element,
                        N1_tension_list_element,
                        N2_list_element,
                        N3_list_element,
                        M1_list_element,
                        M2_list_element,
                        M3_list_element
                        );

                    foreach (var force_component_list in result_point_list)
                    {
                        /// Loop over result points
                        id_points++;

                        if (N1_compression_max >= force_component_list[0])
                        {
                            N1_compression_max = force_component_list[0];
                            N1_compression_list_element = force_component_list;

                            tmpForces.Loadcase_max_Fx_compression = load_case_id;
                            tmpForces.position_Fx_maxCompression = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));

                        }
                        if (N1_tension_max <= force_component_list[0])
                        {
                            N1_tension_max = force_component_list[0];
                            N1_tension_list_element = force_component_list;

                            tmpForces.loadcase_Fx_maxTension = load_case_id;
                            tmpForces.position_Fx_position_maxTension = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));
                        }
                        if (N2_max <= Math.Abs(force_component_list[1]))
                        {
                            N2_max = force_component_list[1];
                            N2_list_element = force_component_list;

                            tmpForces.loadcase_Fy_maxShearY = load_case_id;
                            tmpForces.position_Fy_maxShearY = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));
                        }
                        if (N3_max <= Math.Abs(force_component_list[2]))
                        {
                            N3_max = force_component_list[2];
                            N3_list_element = force_component_list;

                            tmpForces.loadcase_Fz_maxShearZ = load_case_id;
                            tmpForces.position_Fz_maxShearZ = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));
                        }
                        if (M1_max <= Math.Abs(force_component_list[3]))
                        {
                            M1_max = force_component_list[3];
                            M1_list_element = force_component_list;

                            tmpForces.loadcase_Mx_maxTorsion = load_case_id;
                            tmpForces.position_Mx_maxTorsion = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));
                        }
                        if (M2_max <= Math.Abs(force_component_list[4]))
                        {
                            M2_max = force_component_list[4];
                            M2_list_element = force_component_list;

                            tmpForces.loadcase_My_maxBendingY = load_case_id;
                            tmpForces.position_My_maxBendingY = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));
                        }
                        if (M3_max <= Math.Abs(force_component_list[5]))
                        {
                            M3_max = force_component_list[5];
                            M3_list_element = force_component_list;

                            tmpForces.loadcase_Mz_maxBendingZ = load_case_id;
                            tmpForces.position_Mz_maxBendingZ = assemble.Elems[element_id].Crv.PointAt(Convert.ToDouble(id_points) / Convert.ToDouble(maximum_num_points));
                        }

                    }

                    tmpForces.FXc = N1_compression_list_element;
                    tmpForces.FXt = N1_tension_list_element;
                    tmpForces.FY = N2_list_element;
                    tmpForces.FZ = N3_list_element;
                    tmpForces.MX = M1_list_element;
                    tmpForces.MY = M2_list_element;
                    tmpForces.MZ = M3_list_element;
                    
                    assemble.Elems[element_id].Forces = tmpForces;

                    assemble.Elems[element_id].Forces.maxCompression=Math.Abs(N1_compression_max);
                    assemble.Elems[element_id].Forces.maxTension = Math.Abs(N1_tension_max);
                    assemble.Elems[element_id].Forces.maxShearY = Math.Abs(N2_max);
                    assemble.Elems[element_id].Forces.maxShearZ = Math.Abs(N3_max);
                    assemble.Elems[element_id].Forces.maxTorsion = Math.Abs(M1_max);
                    assemble.Elems[element_id].Forces.maxBendingY = Math.Abs(M2_max);
                    assemble.Elems[element_id].Forces.maxBendingZ = Math.Abs(M3_max);
                }

            }

            List<Point3d> nlist1 = new List<Point3d>();

            foreach (var e2 in assemble.Elems)
            {
                nlist1.Add(e2.Forces.position_My_maxBendingY);

            };




            /// Then end of result sorting algorithm
            #endregion

            Assembly OutAssembly = new Assembly(assemble.Nodes, assemble.Elems,
                assemble.Mats, assemble.Secs, assemble.Sups,
                assemble.Loads);

            return OutAssembly;
        }

        public List<GH_Number> DoubleToGHNumber(List<double> ListOfDoubles)
        {
            var list1 = new List<GH_Number>();

            foreach (var i1 in ListOfDoubles)
            {
                list1.Add(new GH_Number(i1));
            }

            return list1;
        }


        public override BoundingBox ClippingBox
        {
            get
            {
                return new BoundingBox( new Point3d(0,0,0) , new Point3d(1,1,1));
            }

        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            args.Display.DrawPoint(
                new Point3d(1, 1, 1),
                Rhino.Display.PointStyle.X,
                10,
                System.Drawing.Color.FromArgb(100, 200, 0)

                );
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