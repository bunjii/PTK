using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTK.Classes
{
    public delegate bool NodeRuleDelegate(Detail _detail);

    class DetailingGroupRulesDefinition
    {

        public string Name { get; private set; }
        public List<NodeRuleDelegate> ValidProperties { get; private set; }
        public List<NodeRuleDelegate> InValidProperties { get; private set; }
        //V1: Public NodeProperty NodeProperty { get; private set; }

        public DetailingGroupRulesDefinition(string _name, List<NodeRuleDelegate> _validProperties, List<NodeRuleDelegate> _inValidProperties)
        {
            Name = _name;
            ValidProperties = _validProperties;
            InValidProperties = _inValidProperties;
        }
        
        public DetailingGroup GetDetailingGroup(List<Detail> _details)
        {
            List<Detail> ApprovedDetails = new List<Detail>();
            bool ValidDetail = true;

            foreach (Detail detail in _details)
            {  
                foreach(NodeRuleDelegate TrueProp in ValidProperties)  
                {
                    if (!TrueProp(detail)) //Testing for false. If false, the detail does not contain in the group
                    {
                        ValidDetail = false;
                        break;
                    }
                }
                foreach(NodeRuleDelegate FalsePrope in InValidProperties) 
                {
                    if (FalsePrope(detail))  //Testing for true. If true, the detail does not contain in the group
                    {
                        ValidDetail = false;
                        break;
                    }
                }
                if (!ValidDetail)
                {
                    break;
                }

                if (ValidDetail)
                {
                    ApprovedDetails.Add(detail);
                }

            }
            if (ValidDetail)
            {
                return  new DetailingGroup(Name, ApprovedDetails);
            }
            else
            {
                return new DetailingGroup(Name);
            }

            



        }


    }
}
