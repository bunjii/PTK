using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;


namespace PTK
{
    public class SubElement
    {
        /////////////////////////////////////////////////////////////////////////////////
        // fields
        /////////////////////////////////////////////////////////////////////////////////
        public string Name { get; private set; }
        public List<CrossSection> CrossSections { get; private set; }
        public List<MaterialProperty> MaterialProperties { get; private set; }
        public List<Alignment> Alignments { get; private set; }
        /////////////////////////////////////////////////////////////////////////////////
        // constructors
        /////////////////////////////////////////////////////////////////////////////////
        public SubElement()
        {
            Name = null;
            MaterialProperties = new List<MaterialProperty>();
            CrossSections = new List<CrossSection>();
            Alignments = new List<Alignment>();
        }
        public SubElement(string _name, List<MaterialProperty> _materialProperties, List<CrossSection> _crossSections, List<Alignment> _alignments)
        {
            Name = _name;
            MaterialProperties = _materialProperties;
            CrossSections = _crossSections;
            Alignments = _alignments;
        }
        /////////////////////////////////////////////////////////////////////////////////
        // properties
        /////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////
        // methods
        /////////////////////////////////////////////////////////////////////////////////

        public SubElement DeepCopy()
        {
            return (SubElement)base.MemberwiseClone();
        }

        public override string ToString()
        {
            string info;
            info = "<SubElement> Name:" + Name;
            // plus matprops, plus crossSecs,
            return info;
        }

        public bool IsValid()
        {
            return Name != "N/A";
        }
        
    }

    public class GH_SubElement : GH_Goo<SubElement>
    {
        /////////////////////////////////////////////////////////////////////////////////
        // fields
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        // constructors
        /////////////////////////////////////////////////////////////////////////////////
        public GH_SubElement() { }
        public GH_SubElement(GH_SubElement other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_SubElement(SubElement subelem) : base(subelem) { this.Value = subelem; }

        /////////////////////////////////////////////////////////////////////////////////
        // properties
        /////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////
        // methods
        /////////////////////////////////////////////////////////////////////////////////
        public override IGH_Goo Duplicate()
        {
            return new GH_SubElement(this);
        }
        public override bool IsValid => base.m_value.IsValid();
        public override string TypeName => "SubElement";
        public override string TypeDescription => "A part of an Element";
        public override string ToString()
        {
            return Value.ToString();
        }
        
        
    }

    public class Param_SubElement : GH_PersistentParam<GH_SubElement>
    {

        /////////////////////////////////////////////////////////////////////////////////
        // fields
        /////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////
        // constructors
        /////////////////////////////////////////////////////////////////////////////////
        public Param_SubElement() : base(new GH_InstanceDescription("SubElement", "SubElem",
            "A part of an Element", CommonProps.category, CommonProps.subcate0))
        { }
        /////////////////////////////////////////////////////////////////////////////////
        // properties
        /////////////////////////////////////////////////////////////////////////////////

        
        // set icon here
        protected override System.Drawing.Bitmap Icon { get { return null; } }

        public override Guid ComponentGuid => new Guid("8a12f26a-532b-4da0-80a0-d775f5648123");

        /////////////////////////////////////////////////////////////////////////////////
        // methods
        /////////////////////////////////////////////////////////////////////////////////
        protected override GH_GetterResult Prompt_Plural(ref List<GH_SubElement> values)
        {
            return GH_GetterResult.success;
        }
        protected override GH_GetterResult Prompt_Singular(ref GH_SubElement value)
        {
            return GH_GetterResult.success;
        }
    }
}
