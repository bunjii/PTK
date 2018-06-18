using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Karamba;

namespace PTK.Classes
{
    class KarambaExport
    {
        public class MatCroPair {
            private PTK_Material _mat;
            private PTK_Section _sec;

            public PTK_Material mat { get { return _mat; } }
            public PTK_Section sec { get { return _sec; } }

            public MatCroPair(PTK_Material mat, PTK_Section sec) { _mat = mat; _sec = sec; }

            public override int GetHashCode()
            {
                int hash = 269;
                hash = (hash * 47) + _mat.GetHashCode();
                hash = (hash * 47) + _sec.GetHashCode();
                return hash;
            }

            public override bool Equals(object obj) {
                if (obj == null)
                {
                    return false;
                }

                var item = obj as MatCroPair;
                if (item == null)
                {
                    return false;
                }

                if (item._mat != _mat) return false;
                if (item._sec != _sec) return false;
                return true;
            }

        }
        /// <summary>
        ///  public class, which is pairing material and section
        /// </summary>
        Assembly ptkassembly;

        public KarambaExport( Assembly _ptkassembly)
        {
            ptkassembly = _ptkassembly;
        }

        #region buildmodel
        public Karamba.Models.Model BuildModel()
        {
            /// 
            var mat_cros = new Dictionary<MatCroPair, int>();
            var mats = new Dictionary<PTK_Material, int>();
            int ind = 0;

            foreach (var e in ptkassembly.Elems)
            {
                if (!mat_cros.TryGetValue(new MatCroPair(e.Material, e.Section), out ind)) {
                    mat_cros[new MatCroPair(e.Material, e.Section)] = ind++;

                }
            }

            ind = 0;
            foreach (var mat_cro in mat_cros.Keys) {
                if (!mats.TryGetValue(mat_cro.mat, out ind))
                {
                    mats[mat_cro.mat] = ind++;
                }
            }

            var krmb_mats = new List<Karamba.Materials.FemMaterial>();
            foreach (var mat in mats.Keys) {
                krmb_mats.Add(krmb_material(mat));
            }

            var krmb_cros = new List<Karamba.CrossSections.CroSec>();
            foreach (var mat_cro in mat_cros.Keys)
            {
                krmb_cros.Add(krmb_crosec(mat_cro.sec, krmb_mats[mats[mat_cro.mat]]));
            }

            var krmb_elems = new List<Karamba.Elements.GrassElement>();
            foreach (var e in ptkassembly.Elems) {
                var c = krmb_cros[mat_cros[new MatCroPair(e.Material, e.Section)]];
                var krmb_elem = new Karamba.Elements.GrassBeam(e.PointAtStart, e.PointAtEnd);
                krmb_elem.crosec = c;
                krmb_elems.Add(krmb_elem);
            }
            
            var supps = new List<Karamba.Supports.Support>();
            var loads = new List<Karamba.Loads.Load>();
            var add_points = new List<Rhino.Geometry.Point3d>();
            var add_crosecs = new List<Karamba.CrossSections.CroSec>();
            var add_beamsets = new List<Karamba.Utilities.ElemSet>();
            var add_mats = new List<Karamba.Materials.FemMaterial>();

            /// supports
            foreach (PTK_Support s in ptkassembly.Sups)
            {
                supps.Add( s.Krmb_GH_support.Value );
            }

            /// loads
            foreach (PTK_Load l in ptkassembly.Loads)
            {
                loads.Add( l.Krmb_GH_load.Value );
            }


            double limit_dist = 0.005;
            var model_builder = new Karamba.Models.ModelBuilder(limit_dist);
            var model = model_builder.build(
                add_points,
                add_mats,
                add_crosecs,
                supps,
                loads,
                krmb_elems,
                add_beamsets);

            return model;

            #endregion
        }

        protected Karamba.Materials.FemMaterial krmb_material(PTK_Material mat) {
            var n = new Karamba.Materials.FemMaterial_Isotrop(
                "family",
                mat.MatName,
                mat.Properties.EE0gmean,
                mat.Properties.EE0gmean * 0.5,
                mat.Properties.GGgmean,
                mat.Properties.Rhogk * 10,
                mat.Properties.Ft0gk,
                0.0000001,
                null
                );

            return n;
        }

        protected Karamba.CrossSections.CroSec krmb_crosec(PTK_Section sec, Karamba.Materials.FemMaterial mat)
        {
            var c = new Karamba.CrossSections.CroSec_Trapezoid(
                "family",
                sec.SectionName,
                "norway",
                null,
                mat,
                sec.Height,
                sec.Width,
                sec.Width
                );
            return c;
        }
    }
}
