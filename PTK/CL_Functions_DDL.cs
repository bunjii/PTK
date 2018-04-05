using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PTK
{
    class Functions_DDL
    {

        public static void AssignNeighbour(List<Element> _elems, List<Node> _nodes)
        {
            for (int i = 0; i < _elems.Count; i++) //Element index e       
            {
                List<Point3d> pointOnCurves = new List<Point3d>();
                List<double> parameterTemp = new List<double>();

                for (int j = 0; j < _nodes.Count; j++)    //Node index: n
                {
                    double t;
                    var temp = _elems[i].Crv.ClosestPoint(_nodes[j].Pt3d, out t);
                    Point3d tmpPt = _elems[i].Crv.PointAt(t);


                    if (tmpPt.DistanceTo(_nodes[j].Pt3d) < .1)
                    {
                        pointOnCurves.Add(tmpPt);
                        parameterTemp.Add(t);

                        _elems[i].AddNeighbour(_nodes[j].ID);
                        _nodes[j].AddNeighbour(_elems[i].ID);
                        /*
                        if (Node[e].Pt3d.DistanceTo(element[e].Crv.PointAtEnd) < ProjectProperties.tolerances)
                        {
                            element[e].N1id = Node[n].ID;
                        }
                        if (Node[n].Pt3d.DistanceTo(element[e].Crv.PointAtStart) < ProjectProperties.tolerances)
                        {
                            element[e].N0id = Node[n].ID;
                        }
                        */
                    }
                }
                var key = parameterTemp.ToArray();
                var elements = pointOnCurves.ToArray();

                Array.Sort(elements, key);

                List<Point3d> pt = elements.ToList();
                List<double> test = key.ToList();

                for (int j = 1; j < pt.Count; j++)
                {
                    Line segment = new Line(pt[j - 1], pt[j]);
                    _elems[i].AddStrctline(segment);

                }
            }
        }

        // This is the main job! And can not be done in parallel. 
        // In other words: everything that happens internally for each member happens in materializer. 
        // Everything that interacts happens in this functions.
        // Making nodes, making relations, etc etc
        public static void Assemble(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeE, ref RTree _rTreeN)
        {
            
            // Give Id and Make RTree for elements
            for (int i = 0; i < _elems.Count; i++)
            {
                _elems[i].AssignID();
                _rTreeE.Insert(_elems[i].BoundingBox, i);
            }
            
            for (int i = 0; i < _elems.Count; i++)
            {
                List<Point3d> endPts =
                    new List<Point3d>() { _elems[i].PointAtStart, _elems[i].PointAtEnd };
                
                // The rTreeNodes.Search will perform this function 
                // if it finds existing points (BoundingBoxes)
                bool nodeExists;
                int numId = new int();
                EventHandler<RTreeEventArgs> nodeExisting =
                    (object sender, RTreeEventArgs args) => 
                    {
                        nodeExists = true;
                        numId = args.Id;
                    };

                for (int j = 0; j < 2; j++) // j<2 as spt & ept
                {
                    nodeExists = false;
                    // BoundingBox is one single point in this case.
                    BoundingBox pointBBox = new BoundingBox(endPts[j], endPts[j]);
                    // "nodeExisting" will be performed, when items are found. nodeExists -> true
                    _rTreeN.Search(pointBBox, nodeExisting);

                    int targetNodeNum; // assemblingNode[targetNodeNum] is the node instance to be manipulated.
                    if (!nodeExists) 
                    {
                        Node newNode = new Node(endPts[j]);
                        _nodes.Add(newNode);
                        _rTreeN.Insert(newNode.BoundingBox, newNode.ID);

                        targetNodeNum = _nodes.Count - 1;
                    }
                    else // nodeExists
                    {
                        targetNodeNum = numId;
                    }

                    // register elemId to node
                    _nodes[targetNodeNum].AddElemId(_elems[i].ID);
                    // register element's parameter to node
                    _nodes[targetNodeNum].AddParameterOfConnectedElements((double) j);
                    // register nodeId to elem
                    _elems[i].AddNodeId(_nodes[targetNodeNum].ID);
                    // register parameter at node to elem
                    _elems[i].AddParameterConnectedNodes((double) j);

                } // end for (int j = 0; j < 2; j++)

            } // for (int i = 0; i < _elems.Count; i++)

            
        }

        public static void Intersect(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeE, ref RTree _rTreeN)
        {
            // check if the elements are colliding
            for (int i = 0; i < _elems.Count; i++)
            {
                Curve targetCrv = _elems[i].Crv;
                List<int> clashElemIdByBBox = new List<int>();
            }


        }


    }
}
