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
        public static void Assemble(List<Element> _elems, out List<Node> _nodes)
        {
            List<Node> nodes = new List<Node>();

            // RTree for Elements
            RTree rTreeElements = new RTree();

            //Step 1: GIVE ID and Make rTree
            //The following loop will assign ID to each element and add the Rtree
            for (int i = 0; i < _elems.Count; i++)
            {
                _elems[i].AssignID();
                rTreeElements.Insert(_elems[i].BoundingBox, i);
            }

            // RTree for Nodes
            RTree rTreeNodes = new RTree();

            //Step 2. Create Adding points
            for (int i = 0; i < _elems.Count; i++)
            {
                Curve targetCrv = _elems[i].Crv;

                List<Point3d> endPts = new List<Point3d>();

                endPts.Add(_elems[i].PointAtStart);
                endPts.Add(_elems[i].PointAtEnd);

                List<Double> tempParam = new List<double>();

                tempParam.Add(0.0);
                tempParam.Add(1.0);

                bool exists = false;

                // The rTreeNodes.Search will perform this function 
                // if it finds existing points (boundingboxes)
                EventHandler<RTreeEventArgs> nodehappening =
                    (object sender, RTreeEventArgs args) => exists = true;

                // Looping through startpoint and endpoint
                // j = 0,1
                for (int j = 0; j < endPts.Count; j++)
                {
                    exists = false;
                    // BoundingBox is one single point in this case.
                    BoundingBox tempBoundingBox = new BoundingBox(endPts[j], endPts[j]);

                    // from SDK document:
                    // " Searches for items in a bounding box.
                    // The bounding box can be singular and contain exactly one single point."
                    // 
                    // "nodehappening" will be performed, when items are found.
                    // when detected: exists -> true
                    rTreeNodes.Search(tempBoundingBox, nodehappening);

                    // If point does not exists, the point is added to rtreeNodes and a new node is made
                    // The node is also added to the the node list 
                    // Important: The rTreeNodes ID is the ID of the node

                    if (!exists)
                    {
                        // Creating new node
                        Node tempNode = new Node(endPts[j]);

                        // Adding current element to the node
                        tempNode.AddElements(_elems[i]);

                        //adding the parameter for the connected element 
                        tempNode.ParameterOfConnectedElements.Add(tempParam[j]);

                        //Adding the node to the Global NodeList That will be outed
                        nodes.Add(tempNode);

                        //Adding the node to the current element
                        _elems[i].AddNode(tempNode);

                        // Adding parameter on the element where the connected node is placed. 
                        // (This will be used for dividing into substructural lines
                        _elems[i].ParameterConnectedNodes.Add(tempParam[j]);

                        //Adding a new boundingbox with the ID of the node
                        rTreeNodes.Insert(tempNode.BoundingBox, tempNode.ID);
                        // MessageBox.Show(tempNode.ID.ToString());      
                    }

                }



                // The next thing is to check wether the elements are colliding.
                targetCrv.Domain = new Interval(0, 1);
                List<int> tempCurvesMaybeCollidingID = new List<int>();

                // The elementHappening add the ID of the element to the tempCurvesMaybeColliding. 
                // These are tested to proper collision detection
                EventHandler<RTreeEventArgs> elementhappening = (object sender, RTreeEventArgs args) =>
                {
                    tempCurvesMaybeCollidingID.Add(args.Id);
                };

                // Finding elements that are close to the current element [i]. 
                // if found: See Elementhappening
                rTreeElements.Search(new Sphere(targetCrv.PointAt(0.5), targetCrv.GetLength()), elementhappening);

                for (int j = 0; j < tempCurvesMaybeCollidingID.Count; j++)
                {
                    Curve testForCollisionCrv = _elems[tempCurvesMaybeCollidingID[j]].Crv;

                    // If both curves are linear, a simple mathematical intersection is performed. 
                    // If not a more CPU-consuming geometrical intersection is performed.
                    if (targetCrv.IsLinear() && testForCollisionCrv.IsLinear())
                    {
                        //Curves to line
                        Line a = new Line(targetCrv.PointAtEnd, targetCrv.PointAtStart);

                        Line b = new Line(testForCollisionCrv.PointAtStart, testForCollisionCrv.PointAtEnd);
                        double numba;   //elems[i]
                        double numbb;   //elems[tempCurvesMaybeCollidingID[j]]

                        if (Rhino.Geometry.Intersect.Intersection.LineLine(a, b, out numba, out numbb))  //Colliding vectors?
                        {
                            // Colliding within parameter for line a?
                            if (-0.001 <= numba && numba <= 1.001)

                                // Colliding within parameter for line b?
                                if (0.0 < numbb && numbb < 1.0)
                                {
                                    Point3d intersectpt = a.PointAt(numba);

                                    int existingnodeID = new int();

                                    //The r.TreeNodes.Search will perform this function if it finds existing points(boundingboxes)
                                    nodehappening = (object sender, RTreeEventArgs args) =>
                                    {
                                        if (intersectpt.DistanceTo(nodes[args.Id].Pt3d) < ProjectProperties.tolerances)
                                        {
                                            exists = true;
                                            existingnodeID = args.Id;
                                        }
                                        else
                                        {
                                            exists = false;
                                        }

                                    };
                                    exists = false;

                                    rTreeNodes.Search(new Sphere(intersectpt, ProjectProperties.tolerances), nodehappening);

                                    Node tempNode;
                                    if (!exists)
                                    {
                                        tempNode = new Node(intersectpt);
                                    }
                                    else
                                    {
                                        tempNode = nodes[existingnodeID];
                                    }

                                    // Adding current element to the node
                                    tempNode.AddElements(_elems[i]);

                                    tempNode.ParameterOfConnectedElements.Add(numba);
                                    tempNode.ParameterOfConnectedElements.Add(numbb);

                                    // Adding testelement to the node
                                    tempNode.AddElements(_elems[tempCurvesMaybeCollidingID[j]]);

                                    // Adding the node to the current element                                                         
                                    _elems[i].AddNode(tempNode);

                                    // Adding parameter of the node
                                    // DDL: need checking if "numba" val is already in the _elem[i]'s PCN list. commented on 3rd Apr.
                                    _elems[i].ParameterConnectedNodes.Add(numba);

                                    // Adding the node to the testelement                                    
                                    _elems[tempCurvesMaybeCollidingID[j]].AddNode(tempNode);

                                    // Adding parameter of the node 
                                    _elems[tempCurvesMaybeCollidingID[j]].ParameterConnectedNodes.Add(numbb);


                                    rTreeNodes.Insert(tempNode.BoundingBox, tempNode.ID);

                                    if (!exists)
                                    {
                                        // Adding the node to the Global NodeList That will be outed
                                        nodes.Add(tempNode);
                                    }

                                    // Adding a new boundingbox with the ID of the node
                                }
                        }
                    }
                    else
                    {


                        var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(targetCrv, testForCollisionCrv, ProjectProperties.tolerances, ProjectProperties.tolerances);
                        if (events != null)
                        {
                            for (int k = 0; k < events.Count; k++)
                            {
                                var evente = events[k];
                                Point3d intersectpt = evente.PointA;      // elems[i]: A, elems[k]  _elems[tempCurvesMaybeCollidingID[j]] : B

                                int existingnodeID = new int();

                                //The r.TreeNodes.Search will perform this function if it finds existing points(boundingboxes)
                                nodehappening = (object sender, RTreeEventArgs args) =>
                                {
                                    if (intersectpt.DistanceTo(nodes[args.Id].Pt3d) < ProjectProperties.tolerances)
                                    {
                                        exists = true;
                                        existingnodeID = args.Id;
                                    }
                                    else
                                    {
                                        exists = false;
                                    }

                                };
                                exists = false;

                                rTreeNodes.Search(new Sphere(intersectpt, ProjectProperties.tolerances), nodehappening);
                                Node tempNode;
                                if (!exists)
                                {
                                    tempNode = new Node(intersectpt);

                                }
                                else
                                {
                                    tempNode = nodes[existingnodeID];
                                }

                                tempNode.AddElements(_elems[i]);                                    //Adding current element to the node
                                tempNode.ParameterOfConnectedElements.Add(evente.ParameterA);
                                tempNode.ParameterOfConnectedElements.Add(evente.ParameterB);

                                tempNode.AddElements(_elems[tempCurvesMaybeCollidingID[j]]);        //adding testelement to the node


                                _elems[i].AddNode(tempNode);                                                            //Adding the node to the current element
                                _elems[i].ParameterConnectedNodes.Add(evente.ParameterA);                                          //Adding parameter of the node
                                _elems[tempCurvesMaybeCollidingID[j]].AddNode(tempNode);                             //Adding the node to the testelement
                                _elems[tempCurvesMaybeCollidingID[j]].ParameterConnectedNodes.Add(evente.ParameterB);           //Adding parameter of the node
                                rTreeNodes.Insert(tempNode.BoundingBox, tempNode.ID);

                                if (!exists)
                                {
                                    nodes.Add(tempNode);                                             //Adding the node to the Global NodeList That will be outed
                                }


                            }
                        }
                    }


                }
            }
            _nodes = nodes;
        }

    }
}
