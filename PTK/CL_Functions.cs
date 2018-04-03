using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;



namespace PTK
{
    public class Functions
    {
        //Function that removes duplicate points
        public static List<Point3d> RemoveDuplicates(List<Point3d> ptList, double tolerance)
        {
            List<int> indexes = new List<int>();
            List<Point3d> newPointList = new List<Point3d>();
            for (int i = 0; i < ptList.Count; i++)
            {
                for (int k = i+1; k<ptList.Count;k++)
                {
                    double distance = ptList[i].DistanceTo(ptList[k]);

                    if (distance < tolerance)
                    {
                        indexes.Add(k);
                    }
                }
            }

            indexes.Sort();
            indexes.Reverse();

            //removing duplicates
            List<int> singleindex = new List<int>();
            singleindex = indexes.Distinct().ToList();
            
            if (singleindex.Count > 0)
            {
                for (int j = 0; j < singleindex.Count; j++)
                {
                    ptList.RemoveAt(singleindex[j]);
                }
            }


            return ptList;
        }

        //Function that assign elements its nodes and nodes its elements
        //inputing elements, nodes and tolerance
        //Looping through nodes, looping through members. Checking distance to members
        //if smaller than projectPropertyTolerances, memberID is added to node, nodeID is added to member
        public static void GenerateStructuralLines(List<Element> element)
        {
            for (int e = 0; e < element.Count; e++) //Element index e       
            {
                List<Point3d> pointOnCurves = new List<Point3d>();

                List<Point3d> pointtemp = new List<Point3d>();
                List<double> parameterTemp = element[e].ParameterConnectedNodes;
                
                List<Point3d> points = new List<Point3d>();


                for (int i = 0; i < element[e].Nodes.Count; i++)
                {
                    points.Add(element[e].Nodes[i].Pt3d);
                    
                }


                var key = parameterTemp.ToArray();
                var elements = points.ToArray();




                Array.Sort(elements, key);


                List<Point3d> pt = elements.ToList();
                





                for (int i = 1; i < pt.Count; i++)
                {
                    Line segment = new Line(pt[i - 1], pt[i]);
                    element[e].AddStrctline(segment);
                }

            }


        }

        public static void AsignNeighbour(List<Element> element, List<Node> Node)
        {
            for (int e = 0; e < element.Count; e++) //Element index e       
            {
                List<Point3d> pointOnCurves = new List<Point3d>();
                List<double> parameterTemp = new List<double>();

                for (int n = 0; n < Node.Count; n++)    //Node index: n
                {
                    double t;
                    var temp = element[e].Crv.ClosestPoint(Node[n].Pt3d, out t);
                    Point3d tmppt = element[e].Crv.PointAt(t);
                    

                    if (tmppt.DistanceTo(Node[n].Pt3d)<.1)
                    {
                        pointOnCurves.Add(tmppt);
                        parameterTemp.Add(t);



                        element[e].AddNeighbour(Node[n].ID);
                        Node[n].AddNeighbour(element[e].ID);
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



                
                Array.Sort(elements,key);


                List<Point3d> pt =  elements.ToList();
                List<double> test = key.ToList();

                for (int i = 1; i < pt.Count; i++)
                {
                    Line segment = new Line(pt[i - 1], pt[i]);
                    element[e].AddStrctline(segment);


                }

            }


        }



        // This is the main job! And can not be done in parallel. 
        // In other words: everything that happens internally for each member happens in materializer. 
        // Everything that interacts happens in this functions.
        // Making nodes, making relations, etc etc
        public static void Assemble(List<Element> _elems, out List<Node> _nodes)
        {
            List<Node> nodes = new List<Node>();
            
            RTree rTreeElements;
            rTreeElements = new RTree();

            //Step 1: GIVE ID and Make rTree
            //The following loop will assign ID to each element and add the Rtree
            for (int i = 0; i < _elems.Count; i++)
            {
                _elems[i].AsignID();
                rTreeElements.Insert(_elems[i].BoundingBox, i);
            }


            
            List < Point3d > tempPoint = new List<Point3d>();
            RTree rTreeNodes = new RTree();

            //Step 2. Create Adding points
            for (int i = 0; i < _elems.Count; i++)
            {
                Curve tempCrv= _elems[i].Crv;
                List<Point3d> testPt = new List<Point3d>();
                testPt.Add(_elems[i].PointAtStart);
                testPt.Add(_elems[i].PointAtEnd);
                List<Double> tempparameter = new List<double>();
                tempparameter.Add(0.0);
                tempparameter.Add(1.0);
                bool exists = false;

                //The r.TreeNodes.Search will perform this function if it finds existing points(boundingboxes)
                EventHandler<RTreeEventArgs> nodehappening = (object sender, RTreeEventArgs args) => 
                {
                    exists = true;
                };

                //Looping through startpoint and endpoint
                for (int pt =0; pt<testPt.Count; pt++)
                {
                    
                    
                    exists = false;
                    BoundingBox tempBoundingBox = new BoundingBox(testPt[pt], testPt[pt]);

                    rTreeNodes.Search(tempBoundingBox, nodehappening);


                    // If point does not exists, the point is added to rtreeNodes and a new node is made
                    // The node is also added to the the node list 
                    // Important: The rTreeNodes ID is the ID of the node
                    if (!exists)
                    {

                        Node tempNode = new Node(testPt[pt]);     //Creating new node
                        
                        tempNode.AddElements(_elems[i]);     //Adding current element to the node
                        tempNode.ParameterOfConnectedElements.Add(tempparameter[pt]); //adding the parameter for the connected element 
                        nodes.Add(tempNode);                 //Adding the node to the Global NodeList That will be outed
                        _elems[i].AddNode(tempNode);        //Adding the node to the current element
                        _elems[i].ParameterConnectedNodes.Add(tempparameter[pt]);  //Adding parameter on the element where the connected node is placed. (This will be used for dividing into substructural lines
                        rTreeNodes.Insert(tempNode.BoundingBox, tempNode.ID);       //Adding a new boundingbox witht he ID of the node
                        
                    }
                }
                


                //The next thing is to check wether the elements are colliding.


                tempCrv.Domain = new Interval(0, 1);

 
                List<int> tempCurvesMaybeCollidingID = new List<int>();


                //The elementHappening add the ID of the element to the tempCurvesMaybeColliding. These are tested to proper collision detection
                EventHandler<RTreeEventArgs> elementhappening = (object sender, RTreeEventArgs args) =>
                {
                    tempCurvesMaybeCollidingID.Add(args.Id);
                };

                //Finding elements that are close to the current element [i]. if found: See ElementHapping
                rTreeElements.Search(new Sphere(tempCrv.PointAt(0.5), tempCrv.GetLength()), elementhappening);

                for (int j = 0; j < tempCurvesMaybeCollidingID.Count; j++)
                {
                    Curve testForCollisionCrv = _elems[tempCurvesMaybeCollidingID[j]].Crv; 

                    // if both curves are linear, a simple mathematical intersection is performed. If not a more CPU-consuming geometrical intersection is performed
                    if (tempCrv.IsLinear() && testForCollisionCrv.IsLinear())   
                    {
                        //Curves to line
                        Line a = new Line(tempCrv.PointAtEnd, tempCrv.PointAtStart);
                        
                        Line b = new Line(testForCollisionCrv.PointAtStart, testForCollisionCrv.PointAtEnd); 
                        double numba;   //elems[i]
                        double numbb;   //elems[tempCurvesMaybeCollidingID[j]]

                        if (Rhino.Geometry.Intersect.Intersection.LineLine(a, b, out numba, out numbb))  //Colliding vectors?
                        {
                            if (-0.001 <= numba && numba <= 1.001)  //Colliding within parameter for line a?
                                if (0.0 < numbb && numbb < 1.0) //Colliding within parameter for line b?
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
                                    
                                    tempNode.AddElements(_elems[i]);                                    //Adding current element to the node
                                    tempNode.ParameterOfConnectedElements.Add(numba);
                                    tempNode.ParameterOfConnectedElements.Add(numbb);

                                    tempNode.AddElements(_elems[tempCurvesMaybeCollidingID[j]]);        //adding testelement to the node

                                                                                                          
                                    _elems[i].AddNode(tempNode);                                                            //Adding the node to the current element
                                    _elems[i].ParameterConnectedNodes.Add(numba);                                          //Adding parameter of the node
                                    _elems[tempCurvesMaybeCollidingID[j]].AddNode(tempNode);                             //Adding the node to the testelement
                                    _elems[tempCurvesMaybeCollidingID[j]].ParameterConnectedNodes.Add(numbb);           //Adding parameter of the node
                                    rTreeNodes.Insert(tempNode.BoundingBox, tempNode.ID);

                                    if (!exists)
                                    {
                                        nodes.Add(tempNode);                                             //Adding the node to the Global NodeList That will be outed
                                    }

                                    

                                    //Adding a new boundingbox witht he ID of the node
                                }
                        }
                    }
                    else
                    {

                        
                        var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(tempCrv, testForCollisionCrv, ProjectProperties.tolerances, ProjectProperties.tolerances);
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



        public static bool CheckInputValidity (List <int> counts)
        {
            counts.Sort();
            bool valid = true;
            int Max = counts.Last();
            

            for (int i = 0; i<counts.Count; i++)
            {
                if ( counts[i] != 1 || counts[i] != Max)
                {
                    valid = false;
                    break; 
                }
              
            }
            return valid; 
            
        }


    }
}
