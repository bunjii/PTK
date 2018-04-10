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
        // This is the main job! And can not be done in parallel. 
        // In other words: everything that happens internally for each member happens in materializer. 
        // Everything that interacts happens in this functions.
        // Making nodes, making relations, etc etc
        public static void Assemble(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeElems, ref RTree _rTreeNodes)
        {
            
            // Give Id and Make RTree for elements
            for (int i = 0; i < _elems.Count; i++)
            {
                _elems[i].AssignID();
                _rTreeElems.Insert(_elems[i].BoundingBox, i);
            }
            
            for (int i = 0; i < _elems.Count; i++)
            {
                List<Point3d> endPts =
                    new List<Point3d>() { _elems[i].PointAtStart, _elems[i].PointAtEnd };
                
                for (int j = 0; j < 2; j++) // j < 2 as spt & ept
                {
                    // check if the node exists. 
                    // if yes it returns nId, else it makes node, register to rtree, then it returns nid.
                    int nId = DetectExistingNode(ref _nodes, ref _rTreeNodes, endPts[j]);

                    // register elemId & its parameter to node
                    RegisterElemToNode(Node.FindNodeById(_nodes, nId), _elems[i], (double) j);

                    // register nodeId & parameter at node to elem
                    RegisterNodeToElem(ref _elems, Node.FindNodeById(_nodes, nId), i, (double) j);

                } // end for (int j = 0; j < 2; j++)
            } // for (int i = 0; i < _elems.Count; i++)
        }
        
        public static void Intersect(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeElems, ref RTree _rTreeNodes)
        {
            // check if the elements are potentially colliding by checking curves' boundary boxes.
            for (int i = 0; i < _elems.Count; i++)
            {
                Curve targetCrv = _elems[i].Crv;
                // reparameterize targetCrv
                targetCrv.Domain = new Interval(0, 1);
                List<int> eNumBBoxClash = new List<int>();

                // event handler of bbox clash
                EventHandler<RTreeEventArgs> elementExisting = (object sender, RTreeEventArgs args) =>
                {
                    eNumBBoxClash.Add(args.Id);
                };

                // search for bbox clashes
                _rTreeElems.Search(new Sphere(targetCrv.PointAt(0.5), targetCrv.GetLength() / 2), elementExisting);

                // search for real clashes out of bbox clashes 
                // and register to elems and nodes list if any detected.
                for (int j = 0; j < eNumBBoxClash.Count; j++)
                {
                    Curve clashingCrv = _elems[eNumBBoxClash[j]].Crv;
                    Line target = CurveToLine(targetCrv);
                    Line clash = CurveToLine(clashingCrv);

                    // case 1: curves are linear -> LLXIntersect
                    double paramA, paramB;
                    int nId = new int();
                    Point3d intersectPt = new Point3d();
                    bool registerFlag = false;
                    // line-line intersect operation.
                    // be aware of & and && here.
                    if (targetCrv.IsLinear() && clashingCrv.IsLinear() && Rhino.Geometry.Intersect.Intersection.LineLine
                        (target, clash, out paramA, out paramB, ProjectProperties.tolerances, true)
                        & (ProjectProperties.tolerances < paramA && paramA < 1 - ProjectProperties.tolerances))
                    {
                        intersectPt = target.PointAt(paramA);

                        // check if the node exists. 
                        // if yes it returns nId, else it makes node, register to rtree, then it returns nid.
                        
                        nId = DetectExistingNode(ref _nodes, ref _rTreeNodes, intersectPt);

                        registerFlag = true;

                    }
                    // else: at least one of the curves are not linear -> curve-curve intersect
                    else 
                    {
                        var intersect = Rhino.Geometry.Intersect.Intersection.CurveCurve
                        (targetCrv, clashingCrv, ProjectProperties.tolerances, ProjectProperties.tolerances);

                        // in case there's no intersect, go on with the next loop.
                        // in case intersect happens at either end of targetCrv, go on with the next loop. 
                        if (intersect == null || intersect.Count == 0 || 
                            intersect[0].ParameterA == 0 || intersect[0].ParameterA == 1) continue;

                        // check if the node exists. 
                        // if yes it returns nId, else it makes node, register to rtree, then it returns nid.
                        intersectPt = intersect[0].PointA;
                        paramA = intersect[0].ParameterA;

                        nId = DetectExistingNode(ref _nodes, ref _rTreeNodes, intersectPt);

                        registerFlag = true;

                    }

                    if (registerFlag == false) continue;

                    // register elemId & its parameter to node
                    RegisterElemToNode(Node.FindNodeById(_nodes, nId), _elems[i], paramA);

                    // register nodeId & parameter at node to elem
                    RegisterNodeToElem(ref _elems, Node.FindNodeById(_nodes, nId), i, paramA);
                }

            }
        }

        public static void GenerateStructuralLines(ref List<Element> _elems, List<Node> _nodes)
        {
            for (int i = 0; i < _elems.Count; i++) //Element index i       
            {
                List<Point3d> pts = new List<Point3d>();
                List<double> paramList = _elems[i].ParameterConnectedNodes;
                
                for (int j = 0; j < _elems[i].NodeIds.Count; j++)
                {
                    pts.Add(Node.FindNodeById(_nodes, _elems[i].NodeIds[j]).Pt3d);
                }
                
                var key = paramList.ToArray();
                var ptsArray = pts.ToArray();
                
                Array.Sort(key,ptsArray);

                // reset substructural id count
                Element.SubElementStructural.ResetSubStrIdCnt();
                for (int j = 1; j < ptsArray.Count(); j++)
                {
                    Line segment = new Line(ptsArray[j - 1], ptsArray[j]);
                    // Element.AddStrctline gives subid as well as segment.
                    _elems[i].AddStrctline(segment);
                }

            }

        }

        private static void RegisterElemToNode(Node _node, Element _elem, double _param)
        {
            _node.AddElemId(_elem.ID);
            _node.AddParameterOfConnectedElements(_param);
        }

        private static void RegisterNodeToElem(ref List<Element> _elems, Node _node, int _i, double _param)
        {
            // check if the node id is already registered.
            if (_elems[_i].NodeIds.Contains(_node.ID) == false)
            {
                _elems[_i].AddNodeId(_node.ID);
                _elems[_i].AddParameterConnectedNodes(_param);
            }
        }
        
        private static Line CurveToLine(Curve _crv)
        {
            Point3d pt0 = _crv.PointAtStart;
            Point3d pt1 = _crv.PointAtEnd;
            Line result = new Line(pt0, pt1);
            return result;
        }

        private static int DetectExistingNode(ref List<Node> _nodes, ref RTree _rTreeNodes, Point3d _sPt)
        {
            // check if the node exists.
            int _nId = new int();
            bool _nodeExists = false;

            // "nodeExisting" will be performed, when items are found.
            EventHandler<RTreeEventArgs> _nodeExisting =
                (object sender, RTreeEventArgs args) =>
                {
                    _nodeExists = true;
                    _nId = args.Id;
                };

            // BoundingBox _spotBBox = new BoundingBox(_samplePt, _samplePt); 
            // Above code didn't work out, needing of considering tolerance for BBox. comment by DDL 9th Apr.
            double tol = ProjectProperties.tolerances; 
            BoundingBox _spotBBox = new BoundingBox
                (_sPt.X-tol,_sPt.Y-tol, _sPt.Z-tol,_sPt.X+tol,_sPt.Y+tol,_sPt.Z+tol);

            // node search
            _rTreeNodes.Search(_spotBBox, _nodeExisting);
            
            if (!_nodeExists)
            {
                Node _newNode = new Node(_sPt);
                _nodes.Add(_newNode);
                // register the node to _rTreeNodes
                _rTreeNodes.Insert(_newNode.BoundingBox, _newNode.ID);
                // obtain nId
                _nId = _newNode.ID;
            }

            return _nId;
        }
    }
}
