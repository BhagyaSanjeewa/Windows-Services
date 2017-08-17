using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AutoIssueLA301UserObjects
{
    public class Logger
    {
        private static string sLogFormat;
        private static string sErrorTime;
        private static string fileName;

        static Logger()
        {
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + "\n \n";
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = "-" + sYear + "-" + sMonth + "-" + sDay;

            fileName = sErrorTime;


        }

        public static void LoggError(Exception ex, string MethodName)
        {
            string path = @"D:\DyeStuff\DyeERPIssueErrors";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                path = System.IO.Path.Combine(path, sErrorTime + ".txt");
                StreamWriter sw = new StreamWriter(path, true);
                sw.WriteLine("------------" + sLogFormat + "-------------");
                sw.WriteLine(" " + ex.ToString());
                sw.WriteLine(" " + MethodName);
                sw.WriteLine("-------------------------------------------");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            if (Directory.Exists(path))
            {
                path = System.IO.Path.Combine(path, sErrorTime + ".txt");
                StreamWriter sw = new StreamWriter(path, true);
                sw.WriteLine("------------" + sLogFormat + "-------------");
                sw.WriteLine(" " + ex.ToString());
                sw.WriteLine(" " + MethodName);
                sw.WriteLine("-------------------------------------------");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }

        }

        public static void LoggIssueItems(string recipeID)
        {
            string path = @"D:\ChemicalStuff\LA301IssueDetails";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                path = System.IO.Path.Combine(path, "DailyIssueDetails" + ".txt");
                StreamWriter sw = new StreamWriter(path, true);
                sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + "\n \n";
                sw.WriteLine("------------" + sLogFormat + "-------------");
                sw.WriteLine("-------------------------------------------");
                sw.WriteLine("" + recipeID + "");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
            if (Directory.Exists(path))
            {
                path = System.IO.Path.Combine(path, "DailyIssueDetails" + ".txt");
                StreamWriter sw = new StreamWriter(path, true);
                sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + "\n \n";
                sw.WriteLine("------------" + sLogFormat + "-------------");
                sw.WriteLine("-------------------------------------------");
                sw.WriteLine("" + recipeID + "");
                sw.WriteLine("");
                sw.Flush();
                sw.Close();
            }
        }
    }
}
