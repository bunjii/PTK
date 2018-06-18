using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace PTK
{
    public class PTK_9_BtlExport : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PTK_C_06 class.
        /// </summary>
        public PTK_9_BtlExport()
          : base("BTL EXPORTER (PTK)", "Export BTL",
              "Exporting BTL file to the designated location",
              CommonProps.category, CommonProps.subcat5)
        {
            Message = CommonProps.initialMessage;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PTK INPUT", "PTK IN", "PTK DATA INPUT", GH_ParamAccess.item);
            pManager.AddGenericParameter("BTL-processes", "", "", GH_ParamAccess.list);
            pManager.AddTextParameter("FILE LOCATION", "Folder", "Folder LOCATION OF EXPORTED BTL FILE", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ENABLE?", "ENABLE?", "ENABLE EXPORTING?", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Result", "", "", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool enable = false;
            Assembly assembly = new Assembly();
            List<BTLprocess> Processes = new List<BTLprocess>();

            string filepath = "";


            DA.GetData(0, ref assembly);
            DA.GetDataList(1, Processes);
            DA.GetData(2, ref filepath);
            DA.GetData(3, ref enable);
            filepath += @"\Test.btlx";

            if (enable)
            {
                //Initializing the parts
                ProjectTypeParts Parts = new ProjectTypeParts();



                foreach (BTLprocess process in Processes)
                {

                    assembly.Elems.Find(t => t.Id == Convert.ToInt16(process.ElemId)).SubElementBTL[0].BTLPart.Processings.Items.Add(process.Process);  //Adding processess in correct btl part
                    assembly.Elems.Find(t => t.Id == Convert.ToInt16(process.ElemId)).SubElementBTL[0].BTLProcesses.Add(process);

                }


                List<Brep> allBreps = new List<Brep>();

                for (int i = 0; i < assembly.Elems.Count; i++)
                {
                    Parts.Part.Add(assembly.Elems[i].SubElementBTL[0].BTLPart);  //Adding part to parts for each element. Line 73 have included all processess

                    List<BTLprocess> BTLProcessess = assembly.Elems[i].SubElementBTL[0].BTLProcesses;
                    List<Brep> voids = new List<Brep>();
                    for (int j = 0; j< BTLProcessess.Count; j++)
                    {
                        voids.Add(BTLProcessess[j].Voidgeometry);
                    }
                    List<Brep> keep = new List<Brep>();
                    keep.Add(assembly.Elems[i].ElementGeometry);
                    double tolerance = 0.01;
                    Rhino.Geometry.Brep[] breps = Rhino.Geometry.Brep.CreateBooleanDifference(keep, voids, tolerance);
                    if (breps != null)
                    {
                        allBreps.Add(breps[0]);
                    }
                    else
                    {
                        allBreps.AddRange(keep);
                    }
                    

                }

                //Initializing the project
                ProjectType Project = new ProjectType();
                Project.Parts = Parts;
                Project.Name = "PTK";
                Project.Architect = "JOHNBUNJIMarcin";
                Project.Comment = "YeaaaahhH! Finally. ";




                //Initializing the file;

                BTLx BTLx = new BTLx();

                BTLx.Project = Project;
                BTLx.Language = "Norsk";


                // Create a new XmlSerializer instance with the type of the test class


                XmlSerializer SerializerObj = new XmlSerializer(typeof(BTLx));


                // Create a new file stream to write the serialized object to a file
                TextWriter WriteFileStream = new StreamWriter(filepath);

                SerializerObj.Serialize(WriteFileStream, BTLx);
                WriteFileStream.Close();

                DA.SetDataList(0, allBreps);

            }


        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return PTK.Properties.Resources.ico_BTL;

            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a638c80f-bcd6-4ecd-a075-9dc9a9c73a98"); }
        }
    }
}