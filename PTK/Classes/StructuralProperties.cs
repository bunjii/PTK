using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace PTK
{ 
    class StructuralProperties
    {





        public double Length;
        public double forceNx;
        public double momentMy;
        public double momentMz;
        public double grain_angle;



        public double EffectiveLength(int direction)
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

        public double SlendernessRatio(Section cs1, int direction)
        {

            double effective_lenght = EffectiveLength(direction);
            double slenderness = 0;

            if (direction == 1)
            {
                slenderness = effective_lenght / cs1.RadiusOfGyrationRectangle(1);
            }
            if (direction == 2)
            {
                slenderness = effective_lenght / cs1.RadiusOfGyrationRectangle(2);
            }

            return slenderness;
        }

        public double EulerForce(Section cs1, MatProps md1, int direction)
        {
            double euler_force = Math.Pow(Math.PI, 2) * md1.e_0_05 / SlendernessRatio(cs1, direction);
            return euler_force;
        }

        public double SlendernessRelative(Section cs1, MatProps md1, int direction)
        {
            // EC5 6.21 , 6.22 relative slenderness
            double slenderness_relative = (SlendernessRatio(cs1, direction) / Math.PI) * (Math.Sqrt(md1.f_c_0_k / md1.e_0_05));
            return slenderness_relative;
        }

        public double InstabilityFactor(Section cs1, MatProps md1, int direction)
        {
            double instability_faktor_kc;
            double lambda_rel = SlendernessRelative(cs1, md1, direction);

            double betaC = 0.1;                     // for the beams with curvature smaller than L/300 for Timber, and L/500 for glulam 
            if (md1.material_type == "glulam")
                betaC = 0.1;                        // EC5 equation 6.29 , straightness measured midway between supports should be lower than L/500
            if (md1.material_type == "timber")
                betaC = 0.2;                        // EC5 equation 6.29 , straightness measured midway between supports should be lower than L/300

            double ky = 0.5 * (1 + betaC * (lambda_rel - 0.3) + Math.Pow(lambda_rel, 2));                 //EC5 equation 6.27
            instability_faktor_kc = 1 / (ky + Math.Sqrt(Math.Pow(ky, 2) - Math.Pow(lambda_rel, 2)));    //EC5 equation 6.25

            return instability_faktor_kc;
        }

        public double BucklingStrength(Section cs1, MatProps md1, int direction)
        {
            double buckling_strength;
            buckling_strength = md1.f_c_0_k * InstabilityFactor(cs1, md1, direction);
            return buckling_strength;
        }

        public double CompressionUtilization(Section cs1, MatProps md1)
        {
            double utilization = 0;
            double utilization_dir1 = 0;
            double utilization_dir2 = 0;
            double stress = forceNx / cs1.AreaOfRectangle();                        // design compressive stress
            double strength = md1.k_mod * md1.k_sys * md1.f_c_0_k / md1.gamma_m;          // design compressive strength parallel to the grain

            double relative_slenderness_dir1 = SlendernessRelative(cs1, md1, 1);
            double relative_slenderness_dir2 = SlendernessRelative(cs1, md1, 2);

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

        public double CompressionUtilizationAngle(Section cs1, MatProps md1)
        {
            double utilization = 0;
            double utilization_dir1 = 0;
            double utilization_dir2 = 0;
            double k_c_90 = 1;                   // recommended value [1] page 172

            double stress = forceNx * Math.Cos(grain_angle) / cs1.AreaOfRectangle();                        // design compressive stress
            double stressangle = stress * Math.Sin(grain_angle);                                            // design compressive stress according to grain angle
            double strength = md1.k_mod * md1.k_sys * md1.f_c_0_k / md1.gamma_m;          // design compressive strength parallel to the grain
            // design compressive strength considering grain angle
            double strengthangle = strength / (strength / (k_c_90 * md1.f_c_90_k) * Math.Pow(Math.Sin(grain_angle), 2) + Math.Pow(Math.Cos(grain_angle), 2));
            double relative_slenderness_dir1 = SlendernessRelative(cs1, md1, 1);
            double relative_slenderness_dir2 = SlendernessRelative(cs1, md1, 2);

            utilization = stressangle / strengthangle;

            return utilization;

        }

        public double TensionUtilization(Section cs1, MatProps md1)
        {
            double utilization = 0;

            double stress = forceNx / cs1.AreaOfRectangle();                                                                            // design tension stress
            double strength = md1.k_mod * md1.k_sys * md1.f_t_0_k * cs1.CoefficientOfSize_kh() / md1.gamma_m;                            // design tension strength

            utilization = stress / strength;

            return utilization;
        }

        public double BendingUtilization(Section cs1, MatProps md1)
        {
            double utilization = 0;
            double utilization1 = 0;
            double utilization2 = 0;

            double stress_1 = momentMy * (cs1.height / 2) / cs1.MomentOfInertiaRectangle(1);
            double stress_2 = momentMy * (cs1.width / 2) / cs1.MomentOfInertiaRectangle(2);            // design bending stress

            double strength1 = md1.k_mod * md1.k_sys * md1.f_m_k * cs1.CoefficientOfSize_kh() / md1.gamma_m;                            // design tension strength
            double strength2 = md1.k_mod * md1.k_sys * md1.f_m_k * cs1.CoefficientOfSize_kh() / md1.gamma_m;                            // design tension strength



            utilization1 = stress_1 / strength1 + md1.k_m * stress_2 / strength2;
            utilization2 = md1.k_m * stress_1 / strength1 + stress_2 / strength2;

            utilization = utilization1;
            if (utilization2 > utilization1)
            {
                utilization = utilization2;
            }

            return utilization;

        }

        public double CombinedBendingAndAxial(Section cs1, MatProps md1)
        {
            double utilization = 0;
            double utilization1 = 0;
            double utilization2 = 0;

            double stress_1 = momentMy * (cs1.height / 2) / cs1.MomentOfInertiaRectangle(1);
            double stress_2 = momentMy * (cs1.width / 2) / cs1.MomentOfInertiaRectangle(2);            // design bending stress

            double strength1 = md1.k_mod * md1.k_sys * md1.f_m_k * cs1.CoefficientOfSize_kh() / md1.gamma_m;                            // design tension strength
            double strength2 = md1.k_mod * md1.k_sys * md1.f_m_k * cs1.CoefficientOfSize_kh() / md1.gamma_m;                            // design tension strength


            double stress = forceNx / cs1.AreaOfRectangle();                        // design compressive stress
            double strength = md1.k_mod * md1.k_sys * md1.f_c_0_k / md1.gamma_m;          // design compressive strength parallel to the grain
            if (forceNx >= 0)
            {
                stress = forceNx / cs1.AreaOfRectangle();                        // design compressive stress
                strength = md1.k_mod * md1.k_sys * md1.f_t_0_k * cs1.CoefficientOfSize_kh() / md1.gamma_m;
            }

            double relative_slenderness_dir1 = SlendernessRelative(cs1, md1, 1);
            double relative_slenderness_dir2 = SlendernessRelative(cs1, md1, 2);



            utilization1 = stress / (strength * InstabilityFactor(cs1, md1, 1)) + stress_1 / strength1 + md1.k_m * stress_2 / strength2;
            utilization2 = stress / (strength * InstabilityFactor(cs1, md1, 2)) + md1.k_m * stress_1 / strength1 + stress_2 / strength2;

            utilization = utilization1;
            if (utilization2 > utilization1)
            {
                utilization = utilization2;
            }

            if (relative_slenderness_dir1 <= 0.3 && relative_slenderness_dir2 <= 0.3)
            {
                utilization1 = Math.Pow(stress / strength, 2) + stress_1 / strength1 + md1.k_m * stress_2 / strength2;
                utilization2 = Math.Pow(stress / strength, 2) + md1.k_m * stress_1 / strength1 + stress_2 / strength2;

                utilization = utilization1;
                if (utilization2 > utilization1)
                {
                    utilization = utilization2;
                }

            }


            return utilization;


        }

    }
}
*/