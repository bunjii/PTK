using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace PTK
{
    public class PTKInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "PTK";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
                
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("c6a3d4ab-76f8-40b7-8be8-1168a27dd763");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
