using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PTK
{
    public class BuildingProject
    {
        public ProjectType BTLProject{ get; private set; }
        public List<BuildingElement> BuildingElements{ get; private set; }
        public List<BuildingNode> BuildingNodes { get; private set; }

        public BuildingProject(ProjectType _btlProject)
        {
            BTLProject = _btlProject;

        }


        public void PrepairElements(List<Element1D> _elements, List<OrderedTimberProcess> _orderedTimberProcesses)  //PHASE1: PREPAIR
        {
            foreach (Element1D element in _elements)
            {
                //Find all correct _ordereded Timberprocesses. Store in list
                List<PerformTimberProcessDelegate> processDelegateInElement = new List<PerformTimberProcessDelegate>();

                BuildingElements.Add(new BuildingElement(element, processDelegateInElement));
            }
        }


    }

    

    public class BuildingElement
    {
        public Element1D Element { get; private set; }
        public List<SubElement3d> Subelement3Ds { get; private set; }
        public List<PartType> BTLParts;
        public List<OrderedTimberProcess> OrderedTimberProcesseses { get; private set; }
        private bool ready = false;

        public BuildingElement(Element1D _element, List<PerformTimberProcessDelegate> _processDelegate)      //PHASE1: PREPAIR
        {
            ready = true;
            Element = _element;
            
            foreach (SubElement subElement in Element.SubElement)
            {
                //ConstructSubElement3D
                Subelement3Ds.Add(new SubElement3d(Element.BaseCurve, subElement, _processDelegate));

            }

        }

  

        public void ManufactureElement(ManufactureMode _mode)      //PHASE 2: Manufacture
        {
            if (ready)
            {
                foreach(SubElement3d subelem3D in Subelement3Ds)
                {
                    subelem3D.ManufactureSubElement(_mode);
                    if (_mode == ManufactureMode.BTL || _mode == ManufactureMode.BOTH)
                    {
                        BTLParts.Add(subelem3D.BTLPart);
                    }

                }
            }

        }

    }


    public class SubElement3d
    {
        public PartType BTLPart { get; private set; }                                           //Needed
        public Brep Stock { get; private set; }  
        public List<Brep> ProcessedStock { get; private set; }
        public List<Brep> VoidProcess { get; private set; }

        public List<PerformTimberProcessDelegate> PerformTimberProcesses { get; private set; }  //Needed
        public BTLPartGeometry BTLPartGeometry { get; private set; }
        private bool ready = false;
        

        public SubElement3d(Curve _baseCurve, SubElement _subElement2D, List<PerformTimberProcessDelegate> _processDelegate)     //PHASE1: PREPAIR
        {
            ready = true;
            PerformTimberProcesses = _processDelegate;
            //Create BTL-part
            // CREATE BTLPARTGEOMETRY
            //Assign
        }

        public void ManufactureSubElement(ManufactureMode _mode)               //PHASE 2: Manufacture
        {
            if (ready)
            {
                
                foreach (PerformTimberProcessDelegate Perform in PerformTimberProcesses)
                {
                    
                    PerformedProcess PerformedProcess = Perform(BTLPartGeometry, _mode);

                    if (_mode == ManufactureMode.BTL || _mode==ManufactureMode.BOTH)
                    {
                        BTLPart.Processings.Items.Add(PerformedProcess.BTLProcess);
                    }

                    if (_mode == ManufactureMode.NURBS || _mode == ManufactureMode.BOTH)
                    {
                        BTLPart.Processings.Items.Add(PerformedProcess.BTLProcess);
                    }
                 
                }

                if (_mode == ManufactureMode.NURBS || _mode == ManufactureMode.BOTH)
                {
                    // ProcessedStock = BrepOperation....
                }

            }
            
        }
        

    }

    public class BTLPartGeometry
    {
        public List<Refside> Refsides { get; private set; }
    } 


    public class PerformedProcess
    {
        public ProcessingType BTLProcess { get; private set; }
        public Brep VoidProcess { get; private set; }
    }
     

    public class OrderedTimberProcess
    {
        public int ElementID { get; private set; }
        public PerformTimberProcessDelegate PerformTimberProcess { get; private set; }
    }


    public class BuildingNode
    {
        // In the future, the geometrical node should be stored here. 
    }



}


       



        




    







    

