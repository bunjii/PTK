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
                        if (Node[e].Pt3d.DistanceTo(element[e].Crv.PointAtEnd) < ProjectProperties.tolerances)
                        {
                            element[e].N1id = Node[n].ID;
                        }
                        if (Node[n].Pt3d.DistanceTo(element[e].Crv.PointAtStart) < ProjectProperties.tolerances)
                        {
                            element[e].N0id = Node[n].ID;
                        }

                    }

                    List<double> a1 = new List<double> { 1, 3, 5, 6 };
                    a1.Sort();
                    

                     
                    parameterTemp.Sort();

                    var cloned = parameterTemp.ToArray();
                    var sourceArr =  pointOnCurves.ToArray();
                    Array.Sort(cloned, sourceArr);
                    

                    


                    

                }

                for (int i = 1; i < pointOnCurves.Count; i++)
                {
                    Line segment = new Line(pointOnCurves[i - 1], pointOnCurves[i]);
                    element[e].AddStrctline(segment);


                }

            }


        }




    }
}
