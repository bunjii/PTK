using feb;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Karamba.Elements;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{
    class Functions_DDL
    {

        // ### public functions ###

        #region obsolete
        /*
        public static void Assemble(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeElems, ref RTree _rTreeNodes)
        {
            // Give Id and Make RTree for elements
            for (int i = 0; i < _elems.Count; i++)
            {
                _elems[i].AssignID(); // assigning element id
                _rTreeElems.Insert(_elems[i].BoundingBox, i);
            }

            for (int i = 0; i < _elems.Count; i++)
            {
                List<Point3d> endPts =
                    new List<Point3d>() { _elems[i].PointAtStart, _elems[i].PointAtEnd };

                for (int j = 0; j < 2; j++) // j < 2 as spt & ept
                {
                    // check if the node exists. 
                    // if yes it returns nId, 
                    // else it makes node, register to rtree, then it returns nid.
                    int nId = DetectOrCreateNode(ref _nodes, ref _rTreeNodes, endPts[j]);

                    // register elemId & its parameter to node
                    RegisterElemToNode(Node.FindNodeById(_nodes, nId), _elems[i], (double)j);

                    // register nodeId & parameter at node to elem
                    RegisterNodeToElem(ref _elems, Node.FindNodeById(_nodes, nId), i, (double)j);

                } // end for (int j = 0; j < 2; j++)
            } // end for (int i = 0; i < _elems.Count; i++)
        }

        public static void SolveIntersection(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeElems, ref RTree _rTreeNodes)
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


                    double paramA, paramB;
                    int nId = new int();
                    Point3d intersectPt = new Point3d();
                    bool registerFlag = false;

                    // case 1: curves are linear -> LLXIntersect
                    if (targetCrv.IsLinear() && clashingCrv.IsLinear() && Rhino.Geometry.Intersect.Intersection.LineLine
                        (target, clash, out paramA, out paramB, CommonProps.tolerances, true)
                        & (CommonProps.tolerances < paramA && paramA < 1 - CommonProps.tolerances))
                    {
                        intersectPt = target.PointAt(paramA);

                        // check if the node exists. 
                        // if yes it returns nId, else it makes node, register to rtree, then it returns nid.
                        nId = DetectOrCreateNode(ref _nodes, ref _rTreeNodes, intersectPt);

                        registerFlag = true;

                    }
                    // case 2: at least one of the curves are not linear -> curve-curve intersect
                    else
                    {
                        var intersect = Rhino.Geometry.Intersect.Intersection.CurveCurve
                        (targetCrv, clashingCrv, CommonProps.tolerances, CommonProps.tolerances);

                        // in case there's no intersect, go on with the next loop.
                        // in case intersect happens at either end of targetCrv, go on with the next loop. 
                        if (intersect == null || intersect.Count == 0 ||
                            intersect[0].ParameterA == 0 || intersect[0].ParameterA == 1) continue;

                        // check if the node exists. 
                        // if yes it returns nId, else it makes node, register to rtree, then it returns nid.
                        intersectPt = intersect[0].PointA;
                        paramA = intersect[0].ParameterA;

                        nId = DetectOrCreateNode(ref _nodes, ref _rTreeNodes, intersectPt);

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
                List<Point3d> _segmentPts = new List<Point3d>();
                List<int> _nids = new List<int>();
                List<double> _paramList = _elems[i].NodeParams.ToList();

                for (int j = 0; j < _elems[i].NodeIds.Count; j++)
                {
                    Node _tempNode = Node.FindNodeById(_nodes, _elems[i].NodeIds[j]);
                    _nids.Add(_tempNode.Id);
                    _segmentPts.Add(_tempNode.Pt3d);
                }

                // sort points in a line from start pt to end pt
                var key = _paramList.ToArray();
                var ptsArray = _segmentPts.ToArray();
                var nidArray = _nids.ToArray();

                Array.Sort(key, ptsArray);
                Array.Sort(key, nidArray);

                // reset substructural id count and structural lines
                Element.Subelement.ResetSubStrIdCnt();
                _elems[i].ClrStrLn();

                for (int j = 1; j < ptsArray.Count(); j++) // j starting with #1
                {
                    Line _segment = new Line(ptsArray[j - 1], ptsArray[j]);
                    // be aware that Element.AddStrctline gives subid as well as segment.
                    _elems[i].AddStrctLine(_segment);
                    _elems[i].SubElem[_elems[i].SubElem.Count - 1].SNId = nidArray[j - 1];
                    _elems[i].SubElem[_elems[i].SubElem.Count - 1].ENId = nidArray[j];
                }

            }
        }
        

        public static void RegisterMaterials(ref List<Element> _elems, ref List<Material> _mats)
        {
            List<string> _hashList = new List<string>();
            int _matIdCnt = 0;

            foreach (Element e in _elems)
            {
                Material _elemMat = e.Material;
                string _hash = _elemMat.Properties.TxtHash;

                // if the hash values coincide, add existent matId. 
                if (_hashList.Contains(_hash))
                {
                    int _numLst = _hashList.IndexOf(_hash);
                    e.MatId = _mats[_numLst].Id;

                    // add element id into material instance
                    _mats[_numLst].AddElemId(e.Id);
                }
                // else: register material and assign matId.
                else
                {
                    // assign id to material
                    e.Material.Id = _matIdCnt;
                    // assign material id to element
                    e.MatId = _matIdCnt;
                    // register material and its hash value
                    _mats.Add(e.Material);
                    _hashList.Add(_hash);

                    // add element id into material instance
                    _mats[_mats.Count - 1].AddElemId(e.Id);
                    _matIdCnt++;
                }
            }
        }

        public static void RegisterSections(ref List<Element> _elems, ref List<Section> _secs)
        {
            List<string> _hashList = new List<string>();
            int _secIdCnt = 0;

            foreach (Element e in _elems)
            {
                Section _elemSec = e.Section;
                string _hash = _elemSec.TxtHash;

                // if the hash values coincides, add existent secId.
                if (_hashList.Contains(_hash))
                {
                    int _numLst = _hashList.IndexOf(_hash);
                    e.SecId = _secs[_numLst].Id;

                    // add element id into section instance
                    _secs[_numLst].AddElemId(e.Id);
                }
                // else: register section and assign secId.
                else
                {
                    // assign id to section
                    e.Section.Id = _secIdCnt;
                    // assign section id to element
                    e.SecId = _secIdCnt;
                    // register section and its hash value
                    _secs.Add(e.Section);
                    _hashList.Add(_hash);

                    // add element id into material instance
                    _secs[_secs.Count - 1].AddElemId(e.Id);
                    _secIdCnt++;
                }
            }

        }

        public static void RegisterPriority(ref List<Element> _elems, string _priorityTxt)
        {
            // priority: 0 (highest priority) to bigger integer (lower priority)

            if (_priorityTxt != "")
            {
                // make a priority list out of the text input
                List<string> _prLst = new List<string>();
                _prLst = _priorityTxt.Split(',').ToList();
                for (int i = 0; i < _prLst.Count; i++)
                {
                    _prLst[i] = _prLst[i].Trim();
                }

                for (int i = 0; i < _elems.Count; i++)
                {
                    _elems[i].Priority = _prLst.IndexOf(_elems[i].Tag);
                }
            }
            else // in case the list is disconnected.
            {
                for (int i = 0; i < _elems.Count; i++)
                {
                    _elems[i].Priority = -999;
                }
            }

        }

        */
        #endregion

        public static string ConvertCommaToPeriodDecimal(string _txt, bool _reverse = false)
        {
            char[] _charList;
            string _resultString = "";

            _charList = _txt.ToCharArray();
            foreach (char _c in _charList)
            {
                char tmpC;
                if (_reverse == false && _c == ',') tmpC = '.';
                else if (_reverse == true && _c == '.') tmpC = ',';
                else tmpC = _c;
                _resultString += tmpC;
            }

            return _resultString;
        }

        public static string CreateHash(string _str)
        {

            byte[] _byteVal = Encoding.UTF8.GetBytes(_str);

            // create SHA256 value
            SHA256 _sha256val = new SHA256CryptoServiceProvider();
            byte[] _hashVal = _sha256val.ComputeHash(_byteVal);

            // byte -> string
            StringBuilder _hashedTxt = new StringBuilder();
            for (int i = 0; i < _hashVal.Length; i++)
            {
                _hashedTxt.Append(_hashVal[i].ToString("X2"));
            }

            return _hashedTxt.ToString();
        }

        public static List<Brep> OperatePriority(List<Node> _nodes, List<PTK_Element> _elems, ref List<Brep> _breps)
        {
            List<Brep> _outBreps = new List<Brep>(_breps);

            // looping through Node by Node
            for (int i = 0; i < _nodes.Count; i++)
            {
                List<int> _nElemIds = _nodes[i].ElemIds.ToList();
                List<int> _priority = new List<int>();
                List<Brep> _relvBrep = new List<Brep>();    // "relevant Breps"
                List<int> _brepElemId = new List<int>();

                // for each element
                for (int j = 0; j < _nElemIds.Count; j++)
                {
                    _priority.Add(PTK_Element.FindElemById(_elems, _nElemIds[j]).Priority);

                    _brepElemId.Add(_nElemIds[j]);
                    _relvBrep.Add(_outBreps[_nElemIds[j]]);
                }

                // looping through each element at the node for cutting
                for (int j = 0; j < _relvBrep.Count; j++)
                {
                    for (int k = 0; k < _relvBrep.Count; k++)
                    {
                        // I am a Brep named _relvBrep[j]. I will be cut when _relvBrep[k] is more prioritized.
                        if (j == k) continue;

                        if (_priority[j] <= _priority[k]) continue;

                        Brep[] _slashedBreps = Brep.CreateBooleanDifference(_relvBrep[j], _relvBrep[k], CommonProps.tolerances);
                        if (_slashedBreps == null) continue;
                        if (_slashedBreps.Count() == 0) continue;

                        List<double> _totalEdgeLength = new List<double>();
                        for (int l = 0; l < _slashedBreps.Count(); l++)
                        {
                            Curve[] _edgeCrvs = _slashedBreps[l].DuplicateEdgeCurves();

                            double _length = 0.0;
                            foreach (Curve _c in _edgeCrvs) _length += _c.GetLength();

                            _totalEdgeLength.Add(_length);

                        }

                        double[] _totalEdgeLengthArray = _totalEdgeLength.ToArray();
                        Array.Sort(_totalEdgeLengthArray, _slashedBreps);

                        Brep _slashed = _slashedBreps[_slashedBreps.Count() - 1];
                        _relvBrep[j] = _slashed;
                        _outBreps[_brepElemId[j]] = _slashed;

                    } // end for (int k = 0; k < _relvBrep.Count; k++)
                } // end for (int j = 0; j < _relvBrep.Count; j++)
            } // end for (int i = 0; i < _nodes.Count; i++)

            return _outBreps;
        }

        public static void RegisterSupports(ref List<PTK_Support> _sups)
        {
            for (int i = 0; i < _sups.Count; i++)
            {
                _sups[i].AssignID();
            }
        }

        public static void CloneKarambaModel(ref Karamba.Models.Model _model)
        {
            // clone model to avoid side effects
            _model = (Karamba.Models.Model)_model.Clone();

            // clone its elements to avoid side effects
            _model.cloneElements();

            // clone the feb-model to avoid side effects
            _model.deepCloneFEModel();
        }

        // ### private functions ###

        private static void RegisterElemToNode(Node _node, PTK_Element _elem, double _param)
        {
            // check if the elem id is already registered, 
            // and if not, register elem and elemparam to node.
            if (_node.ElemIds.Contains(_elem.Id) == false)
            {
                _node.AddElemId(_elem.Id);
                _node.AddElemParams(_param);
            }
        }

        private static void RegisterNodeToElem(ref List<PTK_Element> _elems, Node _node, int _i, double _param)
        {
            // check if the node id is already registered, 
            // and if not, register node and nodeparam to elem
            if (_elems[_i].NodeIds.Contains(_node.Id) == false)
            {
                _elems[_i].AddNodeId(_node.Id);
                _elems[_i].AddNodeParams(_param);
            }
        }

        private static Line CurveToLine(Curve _crv)
        {
            Point3d pt0 = _crv.PointAtStart;
            Point3d pt1 = _crv.PointAtEnd;
            Line result = new Line(pt0, pt1);
            return result;
        }

        /*
        private static int DetectOrCreateNode(ref List<Node> _nodes, ref RTree _rTreeNodes, Point3d _sPt)
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
            // Above code didn't work out, needing of considering tolerance for BBox as below. comment by DDL 9th Apr.
            double tol = CommonProps.tolerances;
            BoundingBox _spotBBox = new BoundingBox
                (_sPt.X - tol, _sPt.Y - tol, _sPt.Z - tol, _sPt.X + tol, _sPt.Y + tol, _sPt.Z + tol);

            // node search
            _rTreeNodes.Search(_spotBBox, _nodeExisting);

            if (!_nodeExists)
            {
                Node _newNode = new Node(_sPt);
                _nodes.Add(_newNode);
                // register the node to _rTreeNodes
                _rTreeNodes.Insert(_newNode.BoundingBox, _newNode.Id);
                // obtain nId
                _nId = _newNode.Id;
            }

            return _nId;
        }
        */

    }
}