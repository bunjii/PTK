using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK
{

    public class PTK_4_Assemble : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PTK_4_Assemble()
          : base("Assemble (PTK)", "Assemble",
              "Assemble",
              CommonProps.category, CommonProps.subcat1)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Element", "E (PTK)", "Add elements here", GH_ParamAccess.list);
            pManager.AddGenericParameter("Supports", "Sup (PTK)", "Add Supports here", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "L (PTK)", "Add Loads here", GH_ParamAccess.list);
            pManager.AddTextParameter("Priority txt", "Priority", "Priority", GH_ParamAccess.item, "");

            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK Assembly", "A (PTK)", "Assembled project data", GH_ParamAccess.item);

            #region obsolete
            // outputs below just checking purposes. should be removed before release. 
            /*
            pManager.AddPointParameter("Point", "", "", GH_ParamAccess.list);
            pManager.AddBrepParameter("Breptest", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("ID", "", "", GH_ParamAccess.list);
            pManager.AddCurveParameter("CenterCurve", "", "", GH_ParamAccess.list);
            pManager.AddNumberParameter("CenterCurve", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Neighbours", "", "", GH_ParamAccess.list);
            pManager.AddLineParameter("Lines", "", "", GH_ParamAccess.list);
            */
            // pManager.AddTextParameter("SubID", "", "", GH_ParamAccess.list);
            #endregion
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Node.ResetIDCount();
            Element.ResetIDCount();
            Support.ResetIDCount();
            // ExpireSolution(true);
            #region variables

            // Assigning lists of objects
            string priorityTxt = "";
            List<Node> nodes = new List<Node>();
            List<Element> elems = new List<Element>();
            List<Material> mats = new List<Material>();
            List<Section> secs = new List<Section>();
            List<Support> sups = new List<Support>();
            RTree rTreeNodes = new RTree();
            RTree rTreeElems = new RTree();

            List<GH_ObjectWrapper> wrapElemList = new List<GH_ObjectWrapper>();
            List<GH_ObjectWrapper> wrapSupList = new List<GH_ObjectWrapper>();
            #endregion

            #region input
            if (!DA.GetDataList(0, wrapElemList)) { return; }
            DA.GetDataList(1, wrapSupList);
            DA.GetData(3, ref priorityTxt);
            #endregion

            #region solve
            // DDL "unwrap wrapped element class" and 
            // "merge multiple element class instance lists"
            for (int i = 0; i < wrapElemList.Count; i++)
            {
                List<Element> tempElemList = new List<Element>();
                wrapElemList[i].CastTo<List<Element>>(out tempElemList);
                elems.AddRange(tempElemList);
            }

            // DDL "unwrap wrapped support class" and 
            // "merge multiple support class instance lists"
            if (wrapSupList.Count != 0)
            {
                for (int i = 0; i < wrapSupList.Count; i++)
                {
                    List<Support> tempSupList = new List<Support>();
                    wrapSupList[i].CastTo<List<Support>>(out tempSupList);
                    sups.AddRange(tempSupList);
                }
            }

            // main functions #1
            // Functions.Assemble returns "nodes"
            try
            {
                Assemble(ref elems, ref nodes, ref rTreeElems, ref rTreeNodes);
                //Functions_DDL.Assemble(ref elems, ref nodes, ref rTreeElems, ref rTreeNodes);
            }
            catch (Exception e)
            {
                Debug.WriteLine("assemble fails.");
                ExpireSolution(true);
            }
            // main functions #2
            // Functions.Intersect returns nodes
            try
            {
                // Functions_DDL.SolveIntersection(ref elems, ref nodes, ref rTreeElems, ref rTreeNodes);
                SolveIntersection(ref elems, ref nodes, ref rTreeElems, ref rTreeNodes);
            }
            catch (Exception e)
            {
                Debug.WriteLine("intersection fails.");
                ExpireSolution(true);
            }
            // main functions #3
            // Functions.GenerateStructuralLines returns nodes
            try
            {
                GenerateStructuralLines(ref elems, nodes);
                //Functions_DDL.GenerateStructuralLines(ref elems, nodes);
            }
            catch (Exception e)
            {
                Debug.WriteLine("structural line fails");
                ExpireSolution(true);
            }
            // main functions #4
            // extract material information from elements
            Functions_DDL.RegisterMaterials(ref elems, ref mats);

            // main functions #5
            // extract cross-section informations from elements
            Functions_DDL.RegisterSections(ref elems, ref secs);

            // main function #6
            // register priorities to element
            Functions_DDL.RegisterPriority(ref elems, priorityTxt);

            // main function #7 
            // register 
            Functions_DDL.RegisterSupports(ref sups);

            #region obsolete

            /* has moved to PTK_UTIL_1_GenerateGeometry 
             * & PTK_UTIL_5_DisassembleElement
            List<Brep> BokseTest = new List<Brep>();
            List<Curve> elementCurves = new List<Curve>();
            List<int> elementid = new List<int>();
            List<int> ConnectedNodes = new List<int>();
            
            List<Line> strLine = new List<Line>();
            List<String> SubID = new List<String>();

            // Testing, making breps
            for (int i = 0; i < elems.Count; i++)
            {
                has moved to PTK_UTIL_1_GenerateGeometry
                BokseTest.Add(elems[i].ElementGeometry);
                elementCurves.Add(elems[i].Crv);
                ConnectedNodes.Add(elems[i].ConnectedNodes);
                elementid.Add(elems[i].Id);
                

                string tempID = Convert.ToString(elems[i].Id);
                
                // making SubID output
                for (int j = 0; j < elems[i].SubStructural.Count; j++)
                {
                    strLine.Add(elems[i].SubStructural[j].StrctrLine);
                    SubID.Add(tempID + "_" + Convert.ToString(elems[i].SubStructural[j].StrctrlLineID));
                }
            }
            List<string> IDs = new List<string>();
            List<Point3d> PointNodes = new List<Point3d>();
            List<string> NeighbourList = new List<string>();
            for (int i = 0; i < nodes.Count; i++)
            {
                IDs.Add(Convert.ToString( nodes[i].ID));
                PointNodes.Add(nodes[i].Pt3d);
                string text ="N"+ Convert.ToString(nodes[i].ID)+"_";
                for (int j = 0; j < nodes[i].ElemIds.Count; j++)
                {
                    text += "E:" +Convert.ToString(nodes[i].ElemIds[j])+" ";
                }
                NeighbourList.Add(text);
            }
            */
            #endregion
            #endregion

            #region output
            Assembly Assembly = new Assembly(new List<Node>(nodes), new List<Element>(elems),
                new List<Material>(mats), new List<Section>(secs), new List<Support>(sups));

            // Assembly Assembly = new Assembly(nodes, elems, mats, secs, sups);
            nodes.Clear();
            elems.Clear();
            mats.Clear();
            secs.Clear();
            sups.Clear();

            DA.SetData(0, Assembly);
            #region obsolete
            // DA.SetDataList(1, PointNodes);
            // DA.SetDataList(2, BokseTest);
            // DA.SetDataList(3, IDs);
            // DA.SetDataList(4, elementCurves);
            // DA.SetDataList(5, elementid);
            // DA.SetDataList(6, ConnectedNodes);
            // DA.SetDataList(7, strLine);
            // DA.SetDataList(1, SubID);
            #endregion

            #endregion

        }

        internal void Assemble(ref List<Element> _elems, ref List<Node> _nodes, ref RTree _rTreeElems, ref RTree _rTreeNodes)
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
                    Node targetNode = _nodes[nId];
                    RegisterElemToNode(ref targetNode, _elems[i], (double)j);

                    // register nodeId & parameter at node to elem
                    RegisterNodeToElem(ref _elems, Node.FindNodeById(_nodes, nId), i, (double)j);

                } // end for (int j = 0; j < 2; j++)
            } // end for (int i = 0; i < _elems.Count; i++)
        }

        internal void SolveIntersection(ref List<Element> _elems,
            ref List<Node> _nodes, ref RTree _rTreeElems, ref RTree _rTreeNodes)
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
                _rTreeElems.Search(new Sphere(targetCrv.PointAt(0.5),
                    targetCrv.GetLength() / 2), elementExisting);

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
                    Node targetNode = _nodes[nId];
                    RegisterElemToNode(ref targetNode, _elems[i], (double)j);

                    // register nodeId & parameter at node to elem
                    RegisterNodeToElem(ref _elems, Node.FindNodeById(_nodes, nId), i, paramA);
                }
            }
        }

        internal void GenerateStructuralLines(ref List<Element> _elems, List<Node> _nodes)
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

        internal int DetectOrCreateNode(ref List<Node> _nodes, ref RTree _rTreeNodes, Point3d _sPt)
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

        internal void RegisterElemToNode(ref Node _node, Element _elem, double _param)
        {
            // check if the elem id is already registered, 
            // and if not, register elem and elemparam to node.
            if (_node.ElemIds.Contains(_elem.Id) == false)
            {
                _node.AddElemId(_elem.Id);
                _node.AddElemParams(_param);
            }
        }

        internal void RegisterNodeToElem(ref List<Element> _elems, Node _node, int _i, double _param)
        {
            // check if the node id is already registered, 
            // and if not, register node and nodeparam to elem
            if (_elems[_i].NodeIds.Contains(_node.Id) == false)
            {
                _elems[_i].AddNodeId(_node.Id);
                _elems[_i].AddNodeParams(_param);
            }
        }

        internal static Line CurveToLine(Curve _crv)
        {
            Point3d pt0 = _crv.PointAtStart;
            Point3d pt1 = _crv.PointAtEnd;
            Line result = new Line(pt0, pt1);
            return result;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                // return null;
                return PTK.Properties.Resources.ico_assemble;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d16b2f49-a170-4d47-ae63-f17a4907fed1"); }
        }
    }
}
