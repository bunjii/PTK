using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PTK
{
    public class BTLref
    {

        #region fields
        private Plane btlplane;
        private Plane refSide1;
        private Plane refSide2;
        private Plane refSide3;
        private Plane refSide4;

        private Line refEdge1;
        private Line refEdge2;
        private Line refEdge3;
        private Line refEdge4;



        #endregion
        #region constructors

        public BTLref(Plane yzPlane, double _height, double _width )
        {
            //Move half height, half width

            
            yzPlane.Translate(yzPlane.XAxis * _width / 2 + (yzPlane.YAxis * _height/2));

            //Making same plane as in manual
            btlplane = new Plane(yzPlane.Origin, yzPlane.ZAxis, yzPlane.YAxis);

            refSide1 = new Plane(btlplane.Origin, btlplane.XAxis, btlplane.ZAxis);
            refSide2 = btlplane;
            refSide2.Translate(btlplane.YAxis * _height);
            refSide2 = new Plane(refSide2.Origin, btlplane.XAxis, -btlplane.YAxis);
            refSide3 = refSide2;
            refSide3.Translate(btlplane.ZAxis * _width);
            refSide3 = new Plane(refSide3.Origin, btlplane.XAxis, -btlplane.ZAxis);
            refSide4 = btlplane;
            refSide4.Translate(btlplane.ZAxis * _width);
            refSide4 = new Plane(refSide4.Origin, btlplane.XAxis, btlplane.YAxis);

            refEdge1 = new Line(refSide1.Origin, refSide1.XAxis, 5000);
            refEdge2 = new Line(refSide2.Origin, refSide2.XAxis, 5000);
            refEdge3 = new Line(refSide3.Origin, refSide3.XAxis, 5000);
            refEdge4 = new Line(refSide4.Origin, refSide4.XAxis, 5000);





        }

        #endregion
        #region properties
        public Plane BTLplane { get { return btlplane; } }
        public Plane RefSide1 { get { return refSide1; } }
        public Plane RefSide2 { get { return refSide2; } }
        public Plane RefSide3 { get { return refSide3; } }
        public Plane RefSide4 { get { return refSide4; } }

        public Line RefEdge1 { get { return refEdge1; } }
        public Line RefEdge2 { get { return refEdge2; } }
        public Line RefEdge3 { get { return refEdge3; } }
        public Line RefEdge4 { get { return refEdge4; } }

        #endregion
        #region methods
        #endregion



    }

    

}


