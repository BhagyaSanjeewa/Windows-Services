using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoIssueLA301UserObjects
{
    public class Requests
    {
        public int RecipeID { get; set; }
        public List<Requests> lstRequests { get; set; }
        public int ChemicalReqID { get; set; }
        public ChemicalDetails ObjChemicalDetails { get; set; }
        public DyeDetails ObjDyeDetials { get; set; }
    }
}
