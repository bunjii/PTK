// alphanumerical order for namespaces please
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public abstract class CrossSection
    {
        #region fields
        public string Name { get; private set; }
        #endregion

        #region constructors
        public CrossSection()
        {
            Name = "N/A";
        }
        public CrossSection(string _name)
        {
            Name = _name;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public abstract CrossSection DeepCopy();
        public override string ToString()
        {
            string info;
            info = "<CrossSection> Name:" + Name;
            return info;
        }
        public bool IsValid()
        {
            return true;
        }
        #endregion
    }

    public class RectangleCroSec : CrossSection
    {
        #region fields
        public double Width { get; private set; } = 100;
        public double Height { get; private set; } = 100;
        public Material Material { get; private set; }
        #endregion

        #region constructors
        public RectangleCroSec() : base()
        {
            Material = new Material();
        }
       public RectangleCroSec(string _name, double _width, double _height) : base(_name)
        {
            Width = _width;
            Height = _height;
            Material = new Material();
        }
        public RectangleCroSec(string _name, double _width, double _height, Material _material) : base(_name)
        {
            Width = _width;
            Height = _height;
            Material = _material;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public override CrossSection DeepCopy()
        {
            return (CrossSection)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<RectangleCroSec> Name:" + Name + 
                " Width:" + Width.ToString() + 
                " Height:" + Height.ToString() + 
                " Material:" + Material.Name;
            return info; 
        }
        #endregion
    }

    public class GH_CroSec : GH_Goo<CrossSection>
    {
        public GH_CroSec() { }
        public GH_CroSec(GH_CroSec other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_CroSec(CrossSection sec) : base(sec) { this.Value = sec; }
        public override bool IsValid => base.m_value.IsValid();

        public override string TypeName => "CrossSection";

        public override string TypeDescription => "Cross Sectional shape of Element and its material";

        public override IGH_Goo Duplicate()
        {
            return new GH_CroSec(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_CroSec : GH_PersistentParam<GH_CroSec>
    {
        public Param_CroSec() : base(new GH_InstanceDescription("CrossSection", "CroSec", "Cross Sectional shape of Element and its material", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }

        public override Guid ComponentGuid => new Guid("480DDCC7-02FB-497D-BF9F-4FAE3CE0687A");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_CroSec> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_CroSec value)
        {
            return GH_GetterResult.success;
        }
    }

}
