using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoIssueLA301UserObjects
{
   public class DyeDetails
    {
        public int DyeLabItemID { get; set; }
        public decimal DyeQuantity { get; set; }
        public string DyeName { get; set; }
        public DateTime WeighedDate { get; set; }
        public int AdditionCount { get; set; }
    }
}
