using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Grasshopper.Kernel.Types;

namespace PTK
{
    public class MaterialStructuralProp
    {
        #region fields
        public string Name { get; set; }
        public double Fmgk { get; set; }
        public double Ft0gk { get; set; }
        public double Ft90gk { get; set; }
        public double Fc0gk { get; set; }
        public double Fc90gk { get; set; }
        public double Fvgk { get; set; }
        public double Frgk { get; set; }
        public double EE0gmean { get; set; }
        public double EE0g05 { get; set; }
        public double EE90gmean { get; set; }
        public double EE90g05 { get; set; }
        public double GGgmean { get; set; }
        public double GGg05 { get; set; }
        public double GGrgmean { get; set; }
        public double GGrg05 { get; set; }
        public double Rhogk { get; set; }
        public double Rhogmean { get; set; }
        #endregion

        #region constructors
        public MaterialStructuralProp() { }
        public MaterialStructuralProp(
            string _materialName,
            double _fmgk,
            double _ft0gk,
            double _ft90gk,
            double _fc0gk,
            double _fc90gk,
            double _fvgk,
            double _frgk,
            double _e0gmean,
            double _e0g05,
            double _e90gmean,
            double _e90g05,
            double _ggmean,
            double _gg05,
            double _grgmean,
            double _grg05,
            double _rhogk,      // density
            double _rhogmean    // density
        )
        {
            Name = _materialName; 
            Fmgk = _fmgk;
            Ft0gk = _ft0gk;
            Ft90gk = _ft90gk;
            Fc0gk = _fc0gk;
            Fc90gk = _fc90gk;
            Fvgk = _fvgk;
            Frgk = _frgk;
            EE0gmean = _e0gmean;
            EE0g05 = _e0g05;
            EE90gmean = _e90gmean;
            EE90g05 = _e90g05;
            GGgmean = _ggmean;
            GGg05 = _gg05;
            GGrgmean = _grgmean;
            GGrg05 = _grg05;
            Rhogk = _rhogk;          // density
            Rhogmean = _rhogmean;    // density
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public MaterialStructuralProp DeepCopy()
        {
            return (MaterialStructuralProp)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "Name:" + Name;
            return info;
        }

        public bool IsValid()
        {
            return Name != "N/A";
        }
        #endregion
    }

    public class GH_MaterialStructuralProp : GH_Goo<MaterialStructuralProp>
    {
        public GH_MaterialStructuralProp() { }
        public GH_MaterialStructuralProp(GH_MaterialStructuralProp other) : base(other.Value)
        {
            this.Value = other.Value.DeepCopy();
        }
        public GH_MaterialStructuralProp(MaterialStructuralProp MSProp) : base(MSProp)
        {
            this.Value = MSProp;
        }

        public override bool IsValid => base.m_value.IsValid();

        public override string TypeName => "MaterialStructuralProp";

        public override string TypeDescription => "MaterialStructuralProp";

        public override IGH_Goo Duplicate()
        {
            return new GH_MaterialStructuralProp(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
