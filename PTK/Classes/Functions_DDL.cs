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


        public static string ConvertCommaToPeriodDecimal(string _txt, bool _reverse = false)
        {
            if (!_reverse)
            {
                return _txt.Replace(',', '.');  //Comma to Period
            }
            else
            {
                return _txt.Replace('.', ',');  //Period to Comma
            }
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

        //public static List<Brep> OperatePriority(List<Node> _nodes, List<Element> _elems, ref List<Brep> _breps)
        //{
        //    List<Brep> _outBreps = new List<Brep>(_breps);

        //    // looping through Node by Node
        //    for (int i = 0; i < _nodes.Count; i++)
        //    {
        //        List<int> _nElemIds = _nodes[i].ElemIds.ToList();
        //        List<int> _priority = new List<int>();
        //        List<Brep> _relvBrep = new List<Brep>();    // "relevant Breps"
        //        List<int> _brepElemId = new List<int>();

        //        // for each element
        //        for (int j = 0; j < _nElemIds.Count; j++)
        //        {
        //            _priority.Add(Element.FindElemById(_elems, _nElemIds[j]).Priority);

        //            _brepElemId.Add(_nElemIds[j]);
        //            _relvBrep.Add(_outBreps[_nElemIds[j]]);
        //        }

        //        // looping through each element at the node for cutting
        //        for (int j = 0; j < _relvBrep.Count; j++)
        //        {
        //            for (int k = 0; k < _relvBrep.Count; k++)
        //            {
        //                // I am a Brep named _relvBrep[j]. I will be cut when _relvBrep[k] is more prioritized.
        //                if (j == k) continue;

        //                if (_priority[j] <= _priority[k]) continue;

        //                Brep[] _slashedBreps = Brep.CreateBooleanDifference(_relvBrep[j], _relvBrep[k], CommonProps.tolerances);
        //                if (_slashedBreps == null) continue;
        //                if (_slashedBreps.Count() == 0) continue;

        //                List<double> _totalEdgeLength = new List<double>();
        //                for (int l = 0; l < _slashedBreps.Count(); l++)
        //                {
        //                    Curve[] _edgeCrvs = _slashedBreps[l].DuplicateEdgeCurves();

        //                    double _length = 0.0;
        //                    foreach (Curve _c in _edgeCrvs) _length += _c.GetLength();

        //                    _totalEdgeLength.Add(_length);

        //                }

        //                double[] _totalEdgeLengthArray = _totalEdgeLength.ToArray();
        //                Array.Sort(_totalEdgeLengthArray, _slashedBreps);

        //                Brep _slashed = _slashedBreps[_slashedBreps.Count() - 1];
        //                _relvBrep[j] = _slashed;
        //                _outBreps[_brepElemId[j]] = _slashed;

        //            } // end for (int k = 0; k < _relvBrep.Count; k++)
        //        } // end for (int j = 0; j < _relvBrep.Count; j++)
        //    } // end for (int i = 0; i < _nodes.Count; i++)

        //    return _outBreps;
        //}

        //public static void RegisterSupports(ref List<Support> _sups)
        //{
        //    for (int i = 0; i < _sups.Count; i++)
        //    {
        //        _sups[i].AssignID();
        //    }
        //}

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

        //private static void RegisterElemToNode(Node _node, Element _elem, double _param)
        //{
        //    // check if the elem id is already registered, 
        //    // and if not, register elem and elemparam to node.
        //    if (_node.ElemIds.Contains(_elem.Id) == false)
        //    {
        //        _node.AddElemId(_elem.Id);
        //        _node.AddElemParams(_param);
        //    }
        //}

        //private static void RegisterNodeToElem(ref List<Element> _elems, Node _node, int _i, double _param)
        //{
        //    // check if the node id is already registered, 
        //    // and if not, register node and nodeparam to elem
        //    if (_elems[_i].NodeIds.Contains(_node.Id) == false)
        //    {
        //        _elems[_i].AddNodeId(_node.Id);
        //        _elems[_i].AddNodeParams(_param);
        //    }
        //}

        private static Line CurveToLine(Curve _crv)
        {
            Point3d pt0 = _crv.PointAtStart;
            Point3d pt1 = _crv.PointAtEnd;
            Line result = new Line(pt0, pt1);
            return result;
        }
    }
}