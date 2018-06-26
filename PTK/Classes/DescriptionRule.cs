using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK
{
    public class Rule
    {
        private List<MethodDelegate> rules;

        public Rule(List<MethodDelegate> _rules)
        {
            rules = _rules;
        }


        public List<MethodDelegate> Rules { get { return rules; } }


    }

}
