using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PTK
{
    public abstract class Element
    {
        #region fields
        public string Tag { get; private set; }
        #endregion

        #region constructors
        public Element()
        {
            Tag = "N/A";
        }
        public Element(string _tag)
        {
            Tag = _tag;
        }
        #endregion
    }

    public class Element1D : Element
    {
        #region fields
        public Curve BaseCurve { get; private set; }
        public Point3d PointAtStart { get; private set; }
        public Point3d PointAtEnd { get; private set; }
        public Plane YZPlane { get; private set; }
        public CrossSection Section { get; private set; }
        public Alignment Align { get; private set; }
        public bool IsIntersectWithOther { get; private set; } = true;
        //public BoundingBox BoundingBox { get; private set; }
        #endregion

        #region constructors
        public Element1D() : base()
        {
            BaseCurve = null;
            PointAtStart = new Point3d();
            PointAtEnd = new Point3d();
            Section = new CrossSection();
            Align = new Alignment();
            InitializeCentricPlanes();
        }
        public Element1D(string _tag) : base(_tag)
        {
            BaseCurve = null;
            PointAtStart = new Point3d();
            PointAtEnd = new Point3d();
            Section = new CrossSection();
            Align = new Alignment();
            InitializeCentricPlanes();
        }
        public Element1D(string _tag, Curve _curve, CrossSection _section, Alignment _align, bool _intersect = true) : base(_tag)
        {
            BaseCurve = _curve;
            PointAtStart = _curve.PointAtStart;
            PointAtEnd = _curve.PointAtEnd;
            Section = _section;
            Align = _align;
            IsIntersectWithOther = _intersect;
            InitializeCentricPlanes();
        }

        #endregion

        #region properties
        #endregion

        #region methods

        private void InitializeCentricPlanes()
        {
            if (BaseCurve != null)
            {
                Vector3d localX = BaseCurve.TangentAtStart;
                Vector3d globalZ = Vector3d.ZAxis;

                // determination of local-y direction
                // case A: where local X is parallel to global Z.
                // (such as most of the columns)

                Vector3d localY = Vector3d.YAxis; // case A default

                // case B: other than case A. (such as beams or inclined columns)
                if (Vector3d.Multiply(globalZ, localX) != globalZ.Length * localX.Length)
                {
                    // localY direction is obtained by the cross product of globalZ and localX.
                    localY = Vector3d.CrossProduct(globalZ, localX);
                }

                Plane localXY = new Plane(BaseCurve.PointAtStart, localX, localY);
                Plane localYZ = new Plane(localXY.Origin, localY, localXY.ZAxis);

                // rotation
                if (Align.RotationAngle != 0.0)
                {
                    double rad = Align.RotationAngle * Math.PI / 180; // degree to radian
                    Vector3d axis = localYZ.ZAxis;
                    Point3d origin = localYZ.Origin;
                    Transform transR = Transform.Rotation(rad, axis, origin);

                    localXY.Transform(transR);
                    localYZ.Transform(transR);

                }

                // translation
                if (Align.OffsetY != 0.0 || Align.OffsetZ != 0.0)
                {
                    Point3d origin = new Point3d(BaseCurve.PointAtStart);
                    Transform transL = Transform.Translation((-1.0) * Align.OffsetY * localYZ.XAxis + Align.OffsetZ * localYZ.YAxis);

                    localXY.Transform(transL);
                    localYZ.Transform(transL);
                }

                YZPlane = localYZ;
            }
            else
            {
                YZPlane = new Plane();
            }
        }
        public Element1D DeepCopy()
        {
            return (Element1D)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<Element1D> Tag:" + Tag +
                " PointAtStart:" + PointAtStart.ToString() +
                " PointAtEnd:" + PointAtEnd.ToString();
            return info;
        }
        public bool IsValid()
        {
            return Tag != "N/A";
        }
        #endregion
        // has moved to PTK_UTIL1_GenerateGeometry
        // Generating extrusion/SweepIntervals
        //private void GenerateIntervals()
        //{
        //    //double[] parameter = { 0.0, 2.2 };

        //    double HalfWidth = Section.Width / 2;
        //    double HalfHeight = Section.Height / 2;

        //    iz = new Interval(-HalfHeight, HalfHeight);
        //    iy = new Interval(-HalfWidth, HalfWidth);
        //    ix = new Interval(0, Crv.GetLength());
        //    crossSectionRectangle = new Rectangle3d(yzPlane, iy, iz);
        //}

        //private void GenerateElementGeometry()
        //{
        //    Brep tempgeometry = new Brep();

        //    if (Crv.IsLinear())
        //    {
        //        Box boxen = new Box(yzPlane, iy, iz, ix);
        //        tempgeometry = Brep.CreateFromBox(boxen);
        //        BoundingBox = boxen.BoundingBox;

        //    }
        //    else
        //    {
        //        SweepOneRail tempsweep = new SweepOneRail();

        //        var sweep = tempsweep.PerformSweep(Crv, crossSectionRectangle.ToNurbsCurve());
        //        tempgeometry = sweep[0];
        //        BoundingBox = tempgeometry.GetBoundingBox(Rhino.Geometry.Plane.WorldXY);
        //        int test = 0;
        //    }
        //    ElementGeometry = tempgeometry;
        //}

    }

    public class ElementForStructural
    {
        #region fields
        public Element1D Element { get; private set; }
        public List<Force> Forces { get; private set; }
        #endregion
        #region constructors
        public ElementForStructural()
        {
            Element = new Element1D();
            Forces = new List<Force>();
        }
        public ElementForStructural(Element1D _element,List<Force> _forces)
        {
            Element = _element;
            Forces = _forces;
        }
        #endregion
        #region properties
        #endregion
        #region methods
        public int AddForce(Force _force)
        {
            Forces.Add(_force);
            return Forces.Count;
        }
        public ElementForStructural DeepCopy()
        {
            return (ElementForStructural)base.MemberwiseClone();
        }
        public override string ToString()
        {
            string info;
            info = "<ElementForStructural> Element1D:" + Element.Tag +
                " Forces:" + Forces.Count.ToString();
            return info;
        }
        public bool IsValid()
        {
            return Element != null;
        }
        #endregion
    }

    public class GH_Element1D : GH_Goo<Element1D>
    {
        public GH_Element1D() { }
        public GH_Element1D(GH_Element1D other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_Element1D(Element1D str) : base(str) { this.Value = str; }
        public override IGH_Goo Duplicate()
        {
            return new GH_Element1D(this);
        }
        public override bool IsValid => base.m_value.IsValid();
        public override string TypeName => "Element1D";
        public override string TypeDescription => "A linear Element";
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_Element1D : GH_PersistentParam<GH_Element1D>
    {
        public Param_Element1D() : base(new GH_InstanceDescription("Element1D", "Elem1D", "A linear Element", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("76479A6F-4C3D-43E0-B85E-FF2C6A99FEA5");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_Element1D> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_Element1D value)
        {
            return GH_GetterResult.success;
        }
    }

    public class GH_ElementForStructural : GH_Goo<ElementForStructural>
    {
        public GH_ElementForStructural() { }
        public GH_ElementForStructural(GH_ElementForStructural other) : base(other.Value) { this.Value = other.Value.DeepCopy(); }
        public GH_ElementForStructural(ElementForStructural elem) : base(elem) { this.Value = elem; }
        public override IGH_Goo Duplicate()
        {
            return new GH_ElementForStructural(this);
        }
        public override bool IsValid => base.m_value.IsValid();
        public override string TypeName => "ElementForStructural";
        public override string TypeDescription => "Element with structural information added";
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class Param_ElementForStructural : GH_PersistentParam<GH_ElementForStructural>
    {
        public Param_ElementForStructural() : base(new GH_InstanceDescription("ElementForStructural", "SElem", "Element with structural information added", CommonProps.category, CommonProps.subcate0)) { }

        protected override System.Drawing.Bitmap Icon { get { return null; } }  //クラスにアイコンを付けたい場合はここ

        public override Guid ComponentGuid => new Guid("B29BCA28-9108-48C1-B4F5-F808644F015A");

        protected override GH_GetterResult Prompt_Plural(ref List<GH_ElementForStructural> values)
        {
            return GH_GetterResult.success;
        }

        protected override GH_GetterResult Prompt_Singular(ref GH_ElementForStructural value)
        {
            return GH_GetterResult.success;
        }
    }

}
