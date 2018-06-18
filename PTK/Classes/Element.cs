using Rhino.Geometry;
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
        public Section Section { get; private set; }
        public Align Align { get; private set; }
        public bool IsIntersectWithOther { get; private set; } = true;
        //public BoundingBox BoundingBox { get; private set; }
        #endregion

        #region constructors
        public Element1D() : base()
        {
            BaseCurve = null;
            PointAtStart = new Point3d();
            PointAtEnd = new Point3d();
            Section = new Section();
            Align = new Align();
            InitializeCentricPlanes();
        }
        public Element1D(string _tag) : base(_tag)
        {
            BaseCurve = null;
            PointAtStart = new Point3d();
            PointAtEnd = new Point3d();
            Section = new Section();
            Align = new Align();
            InitializeCentricPlanes();
        }
        public Element1D(string _tag, Curve _curve, Section _section, Align _align, bool _intersect = true) : base(_tag)
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
        public Element1D Clone()
        {
            return (Element1D)MemberwiseClone();
        }
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
        #endregion
    }

}
