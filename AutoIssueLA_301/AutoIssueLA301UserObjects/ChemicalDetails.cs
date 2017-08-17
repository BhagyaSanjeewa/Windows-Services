using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoIssueLA301UserObjects
{
   public class ChemicalDetails
    {
        public int ChemicalLabItemID { get; set; }
        public decimal ChemicalQuantity { get; set; }
        public string ChemicalName { get; set; }
        public DateTime WeighedDate { get; set; }
        public int AdditionCount { get; set; }
    }
}
