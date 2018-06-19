// alphanumerical order for namespaces please
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    public class Section
    {
        #region fields
        public string Name { get; private set; }
        public double Width { get; private set; } = 100;
        public double Height { get; private set; } = 100;
        public Material Material { get; private set; }
        #endregion

        #region constructors
        public Section()
        {
            Name = "N/A";
            Material = new Material();
        }
       public Section(string _name, double _width, double _height)
        {
            Name = _name;
            Width = _width;
            Height = _height;
            Material = new Material();
        }
        public Section(string _name, double _width, double _height, Material _material)
        {
            Name = _name;
            Width = _width;
            Height = _height;
            Material = _material;
        }
        #endregion

        #region properties
        #endregion

        #region methods
        public Section DeepCopy()
        {
            return (Section)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<Section> Name:" + Name + 
                " Width:" + Width.ToString() + 
                " Height:" + Height.ToString() + 
                " Material:" + Material.Name;
            return info; 
        }
        public bool IsValid()
        {
            return Name != "N/A";
        }
        #endregion
    }

    public class GH_Section : GH_Goo<Section>
    {
        public GH_Section() { }
        public GH_Section(GH_Section other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_Section(Section sec) : base(sec) { this.Value = sec; }
        public override bool IsValid => base.m_value.IsValid();

        public override string TypeName => "Section";

        public override string TypeDescription => "Cross sectional shape of Element and its material";

        public override IGH_Goo Duplicate()
        {
            return new GH_Section(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_Section : GH_PersistentParam<GH_Section>
    {
        public Param_Section() : base(new GH_InstanceDescription("Section", "Sec", "Cross sectional shape of Element and its material", CommonProps.category, CommonProps.subcat0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("480DDCC7-02FB-497D-BF9F-4FAE3CE0687A");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Section> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Section value)
        {
            return GH_GetterResult.success;
        }
    }

}
