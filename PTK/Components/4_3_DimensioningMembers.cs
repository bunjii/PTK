   
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using System.Xml.Serialization;
using System.IO;

using Karamba.Models;
using Karamba.Elements;

namespace PTK.Components
{
    public class PTK_4_3_DimensioningMembers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the _4_3_DimensioningMembers class.
        /// </summary>
        public PTK_4_3_DimensioningMembers()
          : base("Dimensioning Members", "Dimensioning",
              "Description",
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
            pManager.AddGenericParameter("PTK Report", "R (PTK)", "Structural analysis report", GH_ParamAccess.item);
            pManager.AddTextParameter("OUT information", "info", "temporary information from analysis", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            #region variables
            Assembly inassembly;
            GH_ObjectWrapper wrapAssembly = new GH_ObjectWrapper();
            List<string> infolist = new List<string>();
            #endregion

            #region input
            if (!DA.GetData(0, ref wrapAssembly)) { return; }
            wrapAssembly.CastTo<Assembly>(out inassembly);
            #endregion

            #region solve
            /// Generating the cross section data needed for calculation
            /// 

            foreach (var e1 in inassembly.Elems)
            {

                e1.Section.Structural_Area = e1.Section.Width * e1.Section.Height;
                e1.Section.Structural_Moment_of_inertia = new List<double>()
                {
                    e1.Section.Width * Math.Pow(e1.Section.Height, 3) / 12,
                    e1.Section.Height * Math.Pow(e1.Section.Width, 3) / 12
                };
                e1.Section.Structural_Radius_of_gyration = new List<double>()
                {
                    Math.Sqrt( (e1.Section.Width * Math.Pow(e1.Section.Height, 3) / 12 )/(e1.Section.Width * e1.Section.Height)),
                    Math.Sqrt( (e1.Section.Height * Math.Pow(e1.Section.Width, 3) / 12 )/(e1.Section.Width * e1.Section.Height))
                };

            }


            // effective length variables
            double tmpefflength_dir1;
            double tmpefflength_dir2;

            // slenderness variables
            double tmpslender_dir1;
            double tmpslender_dir2;

            // euler force variables
            double tmp_euler_dir1;
            double tmp_euler_dir2;

            //relative  slenderness variables
            double tmp_rel_slender_dir1;
            double tmp_rel_slender_dir2;

            //instability factor variables
           double tmp_ins_factor_dir1;
            double tmp_ins_factor_dir2;

            //instability factor variables
            double tmp_buc_strength_dir1;
            double tmp_buc_strength_dir2;

            //instability factor variables
            double tmp_util_compression;

            List<PTK_StructuralAnalysis> report_list=new List<PTK_StructuralAnalysis>();
            
            /// Loop over elements
            foreach (var e1 in inassembly.Elems)
            {
                PTK_StructuralAnalysis element_report = new PTK_StructuralAnalysis(e1.Id);
                element_report.elementLength = e1.Crv.GetLength();
                element_report.elementWidth = e1.Section.Width;
                element_report.elementHeight = e1.Section.Height;
                
                
                element_report.elementEffectiveLengthDir1 = EffectiveLength(1, e1.Crv.GetLength());
                element_report.elementEffectiveLengthDir2 = EffectiveLength(2, e1.Crv.GetLength());

                
                element_report.elementSlendernessRatioDir1 = SlendernessRatio(e1.Section, 1, e1.Crv.GetLength());
                element_report.elementSlendernessRatioDir2 = SlendernessRatio(e1.Section, 2, e1.Crv.GetLength());

                element_report.elementEulerForceDir1 = EulerForce(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                element_report.elementEulerForceDir2 = EulerForce(e1.Section, e1.Material, 2, e1.Crv.GetLength());

                
                element_report.elementSlendernessRatioDir1 = SlendernessRelative(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                element_report.elementSlendernessRatioDir2 = SlendernessRelative(e1.Section, e1.Material, 2, e1.Crv.GetLength());

                element_report.elementInstabilityFactorDir1 = InstabilityFactor(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                element_report.elementInstabilityFactorDir2 = InstabilityFactor(e1.Section, e1.Material, 2, e1.Crv.GetLength());

                element_report.elementBucklingStrengthDir1 = BucklingStrength(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                element_report.elementBucklingStrengthDir2 = BucklingStrength(e1.Section, e1.Material, 2, e1.Crv.GetLength());

                
                element_report.elementCompressionUtilization = CompressionUtilization(e1.Section, e1.Material, e1.Forces, e1.Crv.GetLength());
                element_report.elementCompressionUtilizationAngle = CompressionUtilizationAngle(e1.Section, e1.Material, e1.Forces, e1.Crv.GetLength());
                element_report.elementTensionUtilization = TensionUtilization(e1.Section, e1.Material, e1.Forces);
                element_report.elementBendingUtilization = BendingUtilization(e1.Section, e1.Material, e1.Forces);
                element_report.elementCombinedBendingAndAxial = CombinedBendingAndAxial(e1.Section, e1.Material, e1.Forces, e1.Crv.GetLength());

                var list_of_utilizations = new List<double>() {
                    element_report.elementCompressionUtilization,
                    element_report.elementTensionUtilization,
                    element_report.elementBendingUtilization,
                    element_report.elementCombinedBendingAndAxial
                };

               
                report_list.Add(element_report);

                #region temporary solution
                /*
                tmpefflength_dir1 = EffectiveLength(1, e1.Crv.GetLength());
                tmpefflength_dir2 = EffectiveLength(2, e1.Crv.GetLength());
                tmpslender_dir1 = SlendernessRatio( e1.Section, 1, e1.Crv.GetLength());
                tmpslender_dir2 = SlendernessRatio( e1.Section, 2, e1.Crv.GetLength());
                                
                tmp_euler_dir1 = EulerForce(e1.Section, e1.Material , 1, e1.Crv.GetLength());
                tmp_euler_dir2 = EulerForce(e1.Section, e1.Material , 2, e1.Crv.GetLength());

                tmp_rel_slender_dir1 = SlendernessRelative(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                tmp_rel_slender_dir2 = SlendernessRelative(e1.Section, e1.Material, 2, e1.Crv.GetLength());
                
                tmp_ins_factor_dir1 = InstabilityFactor(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                tmp_ins_factor_dir2 = InstabilityFactor(e1.Section, e1.Material, 2, e1.Crv.GetLength());
                
                tmp_buc_strength_dir1 = BucklingStrength(e1.Section, e1.Material, 1, e1.Crv.GetLength());
                tmp_buc_strength_dir2 = BucklingStrength(e1.Section, e1.Material, 2, e1.Crv.GetLength());
                tmp_util_compression = CompressionUtilization(e1.Section, e1.Material, e1.Forces, e1.Crv.GetLength());
                infolist.Add("length=" + e1.Crv.GetLength().ToString());
                infolist.Add("element number =" + e1.Id.ToString());
                infolist.Add("element width =" + e1.Section.Width.ToString());
                infolist.Add("element height =" + e1.Section.Height.ToString());
                infolist.Add("element area  =" + e1.Section.Structural_Area.ToString());
                infolist.Add("element moment of inertia dir1  =" + e1.Section.Structural_Moment_of_inertia[0].ToString());
                infolist.Add("element moment of inertia dir2  =" + e1.Section.Structural_Moment_of_inertia[1].ToString());
                infolist.Add("element radius of gyration dir1  =" + e1.Section.Structural_Radius_of_gyration[0].ToString());
                infolist.Add("element radius of gyration dir2  =" + e1.Section.Structural_Radius_of_gyration[1].ToString());

                infolist.Add("max compression force  =" + e1.Forces.maxCompression.ToString());
                
                infolist.Add("effective length dir1 =" + tmpefflength_dir1.ToString());
                infolist.Add("effective length dir2 =" + tmpefflength_dir2.ToString());
                infolist.Add("slenderness dir1 =" + tmpslender_dir1.ToString());
                infolist.Add("slenderness dir2 =" + tmpslender_dir2.ToString());
                infolist.Add("euler force dir1 =" + tmp_euler_dir1.ToString());
                infolist.Add("euler force dir2 =" + tmp_euler_dir2.ToString());
                infolist.Add("relative slenderness dir1 =" + tmp_rel_slender_dir1.ToString());
                infolist.Add("relative slenderness dir2 =" + tmp_rel_slender_dir2.ToString());
                infolist.Add("instability factor dir1 =" + tmp_ins_factor_dir1.ToString());
                infolist.Add("instability factor dir2 =" + tmp_ins_factor_dir2.ToString());
                infolist.Add("buckling strength dir1=" + tmp_buc_strength_dir1.ToString());
                infolist.Add("buckling strength dir2 =" + tmp_buc_strength_dir2.ToString());
                infolist.Add("utilization =" + tmp_util_compression.ToString());
                */

                #endregion

            }

           
           

            #endregion

            #region output
            Assembly outassembly = inassembly;
            DA.SetData(0, outassembly);
            DA.SetData(1, report_list);
            DA.SetDataList(2, infolist);
            #endregion
        }

        #region methods
        private double EffectiveLength(int direction , double Length)
        {
            double buckling_coefficient = 1;
            double effective_length = Length * buckling_coefficient;

            if (direction == 1)
            {
                buckling_coefficient = 1;
                effective_length = Length * buckling_coefficient;
            }
            if (direction == 2)
            {
                buckling_coefficient = 1;
                effective_length = Length * buckling_coefficient;
            }

            return effective_length;
        }

        private double SlendernessRatio(PTK_Section cs1, int direction, double length)
        {

            double effective_lenght = EffectiveLength(direction, length);
            double slenderness = 0;

            if (direction == 1)
            {
                slenderness = effective_lenght / cs1.Structural_Radius_of_gyration[0];
            }
            if (direction == 2)
            {
                slenderness = effective_lenght / cs1.Structural_Radius_of_gyration[1];
            }

            return slenderness;
        }

        private double EulerForce(PTK_Section cs1, PTK_Material md1, int direction, double length)
        {
            double euler_force = Math.Pow(Math.PI, 2) * md1.Properties.EE0g05 / SlendernessRatio(cs1, direction, length);
            return euler_force;
        }

        private double SlendernessRelative(PTK_Section cs1, PTK_Material md1, int direction, double length)
        {
            // EC5 6.21 , 6.22 relative slenderness
            double slenderness_relative = (SlendernessRatio(cs1, direction,length) / Math.PI) * (Math.Sqrt(md1.Properties.Fc0gk / md1.Properties.EE0g05));
            return slenderness_relative;
        }

        private double InstabilityFactor(PTK_Section cs1, PTK_Material md1, int direction, double length)
        {
            double instability_faktor_kc;
            double lambda_rel = SlendernessRelative(cs1, md1, direction, length);

            double betaC = 0.1;                     // for the beams with curvature smaller than L/300 for Timber, and L/500 for glulam 
            if (md1.Properties.MaterialName == "glulam")
                betaC = 0.1;                        // EC5 equation 6.29 , straightness measured midway between supports should be lower than L/500
            if (md1.Properties.MaterialName == "timber")
                betaC = 0.2;                        // EC5 equation 6.29 , straightness measured midway between supports should be lower than L/300

            double ky = 0.5 * (1 + betaC * (lambda_rel - 0.3) + Math.Pow(lambda_rel, 2));                 //EC5 equation 6.27
            instability_faktor_kc = 1 / (ky + Math.Sqrt(Math.Pow(ky, 2) - Math.Pow(lambda_rel, 2)));    //EC5 equation 6.25

            return instability_faktor_kc;
        }

        private double BucklingStrength(PTK_Section cs1, PTK_Material md1, int direction, double length)
        {
            double buckling_strength;
            buckling_strength = md1.Properties.Fc0gk * InstabilityFactor(cs1, md1, direction,length);
            return buckling_strength;
        }

        private double CompressionUtilization(PTK_Section cs1, PTK_Material md1, PTK_Forces force, double length)
        {
            double utilization = 0;
            double utilization_dir1 = 0;
            double utilization_dir2 = 0;
            double stress = force.maxCompression / cs1.Structural_Area;                        // design compressive stress
            double strength = md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fc0gk / md1.Properties.GammaM;          // design compressive strength parallel to the grain

            double relative_slenderness_dir1 = SlendernessRelative(cs1, md1, 1, length);
            double relative_slenderness_dir2 = SlendernessRelative(cs1, md1, 2, length);

            if (relative_slenderness_dir1 <= 0.3 && relative_slenderness_dir2 <= 0.3)
            {
                utilization = stress / strength;
            }
            if (relative_slenderness_dir1 > 0.3 && relative_slenderness_dir2 <= 0.3)
            {
                utilization_dir1 = stress / (relative_slenderness_dir1 * strength);
                utilization_dir2 = stress / strength;

                //choose the bigger utilization to be the element utilization
                utilization = utilization_dir1;
                if (utilization_dir1 < utilization_dir2)
                {
                    utilization = utilization_dir2;
                }

            }
            if (relative_slenderness_dir1 <= 0.3 && relative_slenderness_dir2 > 0.3)
            {
                utilization_dir1 = stress / strength;
                utilization_dir2 = stress / (relative_slenderness_dir2 * strength);

                //choose the bigger utilization to be the element utilization
                utilization = utilization_dir1;
                if (utilization_dir1 < utilization_dir2)
                {
                    utilization = utilization_dir2;
                }
            }
            if (relative_slenderness_dir1 > 0.3 && relative_slenderness_dir2 > 0.3)
            {
                utilization_dir1 = stress / (relative_slenderness_dir1 * strength);
                utilization_dir2 = stress / (relative_slenderness_dir1 * strength);

                //choose the bigger utilization to be the element utilization
                utilization = utilization_dir1;
                if (utilization_dir1 < utilization_dir2)
                {
                    utilization = utilization_dir2;
                }
            }
            return utilization;

        }

        private double CompressionUtilizationAngle(PTK_Section cs1, PTK_Material md1, PTK_Forces force, double length)
        {
            double utilization = 0;
            double utilization_dir1 = 0;
            double utilization_dir2 = 0;
            double k_c_90 = 1;                   // recommended value [1] page 172

            double stress = force.maxCompression * Math.Cos(md1.Properties.GrainAngle) / cs1.Structural_Area;                        // design compressive stress
            double stressangle = stress * Math.Sin(md1.Properties.GrainAngle);                                            // design compressive stress according to grain angle
            double strength = md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fc0gk / md1.Properties.GammaM;         // design compressive strength parallel to the grain
            // design compressive strength considering grain angle
            double strengthangle = strength / (strength / (k_c_90 * md1.Properties.Fc90gk) * Math.Pow(Math.Sin(md1.Properties.GrainAngle), 2) + Math.Pow(Math.Cos(md1.Properties.GrainAngle), 2));
            double relative_slenderness_dir1 = SlendernessRelative(cs1, md1, 1, length);
            double relative_slenderness_dir2 = SlendernessRelative(cs1, md1, 2, length);

            utilization = stressangle / strengthangle;

            return utilization;
        }
        
        private double TensionUtilization(PTK_Section cs1, PTK_Material md1, PTK_Forces force)
        {
            double utilization = 0;

            #region kh coefficient
            double var1;
            double kh=1.0;
            

            double h = cs1.Height;
            if (cs1.Height < cs1.Width)
            {
                h = cs1.Width;
            }

            if (md1.Properties.MaterialName == "Timber")
            {
                var1 = Math.Pow(150 / h, 0.2);
                kh = 1.3;
                if (var1 < 1.3)
                {
                    kh = var1;
                }
            }
            else if (md1.Properties.MaterialName == "Glulam")
            {
                var1 = Math.Pow(600 /h, 0.1);
                kh = 1.1;
                if (var1 < 1.1)
                {
                    kh = var1;
                }
            }
            #endregion

            double stress = force.maxTension / cs1.Structural_Area;                                                                            // design tension stress
            double strength = md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Ft0gk * kh / md1.Properties.GammaM;
            // design tension strength

            utilization = stress / strength;

            return utilization;
        }

        public double BendingUtilization(PTK_Section cs1, PTK_Material md1, PTK_Forces force)
        {
            double utilization = 0;
            double utilization1 = 0;
            double utilization2 = 0;

            #region kh coefficient
            double var1;
            double kh = 1.0;


            double h = cs1.Height;
            if (cs1.Height < cs1.Width)
            {
                h = cs1.Width;
            }

            if (md1.Properties.MaterialName == "Timber")
            {
                var1 = Math.Pow(150 / h, 0.2);
                kh = 1.3;
                if (var1 < 1.3)
                {
                    kh = var1;
                }
            }
            else if (md1.Properties.MaterialName == "Glulam")
            {
                var1 = Math.Pow(600 / h, 0.1);
                kh = 1.1;
                if (var1 < 1.1)
                {
                    kh = var1;
                }
            }
            #endregion
            
            double stress_1 = force.maxBendingY * (cs1.Height / 2) / cs1.Structural_Moment_of_inertia[0] ;
            double stress_2 = force.maxBendingZ * (cs1.Width / 2) / cs1.Structural_Moment_of_inertia[1] ;            // design bending stress
            
            double strength1 = Math.Abs(md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fmgk * kh / md1.Properties.GammaM);                            // design tension strength
            double strength2 = Math.Abs(md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fmgk * kh / md1.Properties.GammaM);                            // design tension strength

            double km = 0.7; // for rectangular timber cross section

            utilization1 = stress_1 / strength1 + km * stress_2 / strength2;
            utilization2 = km * stress_1 / strength1 + stress_2 / strength2;

            utilization = utilization1;
            if (utilization2 > utilization1)
            {
                utilization = utilization2;
            }

            return utilization;
        }

        public double CombinedBendingAndAxial(PTK_Section cs1, PTK_Material md1, PTK_Forces force, double length)
        {
            double utilization = 0;
            double utilization1 = 0;
            double utilization2 = 0;
            double km = 0.7; // for rectangular timber cross section

            #region kh coefficient
            double var1;
            double kh = 1.0;


            double h = cs1.Height;
            if (cs1.Height < cs1.Width)
            {
                h = cs1.Width;
            }

            if (md1.Properties.MaterialName == "Timber")
            {
                var1 = Math.Pow(150 / h, 0.2);
                kh = 1.3;
                if (var1 < 1.3)
                {
                    kh = var1;
                }
            }
            else if (md1.Properties.MaterialName == "Glulam")
            {
                var1 = Math.Pow(600 / h, 0.1);
                kh = 1.1;
                if (var1 < 1.1)
                {
                    kh = var1;
                }
            }
            #endregion

            double forceNx = force.maxCompression;
            if (force.maxCompression < Math.Abs(force.maxTension))
            {
                forceNx = force.maxTension;
            }

            double stress_1 = force.maxBendingY * (cs1.Height / 2) / cs1.Structural_Moment_of_inertia[0];
            double stress_2 = force.maxBendingZ * (cs1.Width / 2) / cs1.Structural_Moment_of_inertia[1];            // design bending stress

            double strength1 = Math.Abs(md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fmgk * kh / md1.Properties.GammaM);                            // design tension strength
            double strength2 = Math.Abs(md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fmgk * kh / md1.Properties.GammaM);                             // design tension strength
            
            double stress = forceNx / cs1.Structural_Area;                        // design compressive stress
            double strength = md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Fc0gk / md1.Properties.GammaM ;          // design compressive strength parallel to the grain
            if (forceNx >= 0)
            {
                stress = forceNx / cs1.Structural_Area ;                        // design compressive stress
                strength = Math.Abs(md1.Properties.Kmod * md1.Properties.Ksys * md1.Properties.Ft0gk * kh / md1.Properties.GammaM ) ;
            }

            double relative_slenderness_dir1 = SlendernessRelative(cs1, md1, 1,length);
            double relative_slenderness_dir2 = SlendernessRelative(cs1, md1, 2,length);
            
            utilization1 = stress / (strength * InstabilityFactor(cs1, md1, 1, length)) + stress_1 / strength1 + km * stress_2 / strength2;
            utilization2 = stress / (strength * InstabilityFactor(cs1, md1, 2, length)) + km * stress_1 / strength1 + stress_2 / strength2;

            utilization = utilization1;
            if (utilization2 > utilization1)
            {
                utilization = utilization2;
            }

            if (relative_slenderness_dir1 <= 0.3 && relative_slenderness_dir2 <= 0.3)
            {
                utilization1 = Math.Pow(stress / strength, 2) + stress_1 / strength1 + km * stress_2 / strength2;
                utilization2 = Math.Pow(stress / strength, 2) + km * stress_1 / strength1 + stress_2 / strength2;

                utilization = utilization1;
                if (utilization2 > utilization1)
                {
                    utilization = utilization2;
                }

            }

            return utilization;
        }
        #endregion
        

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return PTK.Properties.Resources.ico_dimensioning;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6639ee92-b0e1-4005-8df8-94d01ab78237"); }
        }
    }
}