using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoIssueLA301UserObjects
{
    public class SMS
    {
        public int SMSID { get; set; }
        public string PhoneNo { get; set; }
        public string Message { get; set; }
        public bool IsSMS { get; set; }
    }
}
