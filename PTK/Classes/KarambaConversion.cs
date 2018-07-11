using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Karamba;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;


namespace PTK
{
    public static class KarambaConversion
    {
        public static Karamba.Models.Model BuildModel(StructuralAssembly _strAss)
        {
            var points = new List<Rhino.Geometry.Point3d>();
            var materialMap = new Dictionary<Material, Karamba.Materials.FemMaterial>();
            var crosecMap = new Dictionary<CrossSection, Karamba.CrossSections.CroSec>();
            var supports = new List<Karamba.Supports.Support>();
            var loads = new List<Karamba.Loads.Load>();
            var elems = new List<Karamba.Elements.GrassElement>();
            var elemset = new List<Karamba.Utilities.ElemSet>();


            points = _strAss.Assembly.Nodes.ConvertAll(n => n.Point);// * CommonProps.ConversionUnit(Rhino.UnitSystem.Meters));
            
            foreach(KeyValuePair<CrossSection,Material> kvp in _strAss.Assembly.CrossSectionMap)
            {
                if (!materialMap.ContainsKey(kvp.Value))
                {
                    materialMap.Add(kvp.Value, MakeFemMaterial(kvp.Value));
                }
                crosecMap.Add(kvp.Key, MakeCrossSection(kvp.Key, materialMap[kvp.Value]));
            }

            foreach(Support s in _strAss.Supports)
            {
                var sup = new Karamba.Supports.Support(s.FixingPlane.Origin, s.Conditions, s.FixingPlane);
                sup.loadcase = s.LoadCase;
                supports.Add(sup);
            }

            foreach(Load l in _strAss.Loads)
            {
                if(l is PointLoad pl)
                {
                    var load = new Karamba.Loads.PointLoad(pl.Point, pl.ForceVector, pl.MomentVector, pl.LoadCase, true);
                    loads.Add(load);
                }
                else if(l is GravityLoad gl)
                {
                    var load = new Karamba.Loads.GravityLoad(gl.GravityVector, gl.LoadCase);
                    loads.Add(load);
                }
            }


            foreach(StructuralElement e in _strAss.SElements)
            {
                var paramList = _strAss.Assembly.SearchNodeParamAtElement(e.Element);
                for (int i = 0; i <= paramList.Count-2; i++ )
                {
                    var elem = new Karamba.Elements.GrassBeam(e.Element.BaseCurve.PointAt(paramList[i]), e.Element.BaseCurve.PointAt(paramList[i + 1]));
                    //var s = crosecMap[e.Element.Sections[0]];
                    //s.ecce_loc = new Vector3d(e.Element.Align.OffsetY, e.Element.Align.OffsetZ,0);
                    elem.crosec = crosecMap[e.Element.Sections[0]]; //現在は断面材1つで想定
                    //elem.z_ori
                    elems.Add(elem);
                }
            }


            double limitDist = 0.5;
            var modelBuilder = new Karamba.Models.ModelBuilder(limitDist);
            var model = modelBuilder.build(
                points,
                materialMap.Values.ToList(),
                crosecMap.Values.ToList(),
                supports,
                loads,
                elems,
                elemset
            );

            return model;
        }

        private static Karamba.Materials.FemMaterial MakeFemMaterial(Material _mat)
        {
            var fm = new Karamba.Materials.FemMaterial_Isotrop(
                "familyName",
                _mat.Name,
                _mat.StructuralProp.EE0gmean,
                _mat.StructuralProp.EE0gmean*0.5,
                _mat.StructuralProp.GGgmean,
                _mat.StructuralProp.Rhogk*10,
                _mat.StructuralProp.Ft0gk,
                0.0000001,
                null/*color*/
                );

            return fm;
        }

        private static Karamba.CrossSections.CroSec MakeCrossSection(CrossSection _sec, Karamba.Materials.FemMaterial _mat)
        {
            Karamba.CrossSections.CroSec sec;
            if (_sec is RectangleCroSec rectSec)
            {
                sec = new Karamba.CrossSections.CroSec_Trapezoid(
                    "familyName",
                    _sec.Name,
                    "Country",
                    null/*color*/,
                    _mat,
                    rectSec.GetHeight() * CommonProps.ConversionUnit(Rhino.UnitSystem.Centimeters),
                    rectSec.GetWidth() * CommonProps.ConversionUnit(Rhino.UnitSystem.Centimeters),
                    rectSec.GetWidth() * CommonProps.ConversionUnit(Rhino.UnitSystem.Centimeters)
                    );
                //Karamba.Utilities.UnitsConversionFactory unitsConversionFactory = Karamba.Utilities.UnitsConversionFactories.Conv();
                //sec.ecce_loc = unitsConversionFactory.cm();
            }
            else
            {
                sec = null;
            }
            return sec;
        }
    }
}
