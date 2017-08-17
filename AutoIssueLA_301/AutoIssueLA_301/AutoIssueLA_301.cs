using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using AutoIssueLA301BLL;
using System.Configuration;
using System.Text.RegularExpressions;
using AutoIssueLA301UserObjects;
using System.IO;
using System.Timers;
using System.Net.NetworkInformation;
using System.Threading;

namespace AutoIssueLA_301
{
    public partial class AutoIssueLA_301 : ServiceBase
    {
        #region Private Variables
        string sourcepath = @"\\la301\ERP-Export";
        public int FileCount { get; set; }
        AutoIssueLA301Manager objAutoIssueManager = null;

        string xx = ConfigurationSettings.AppSettings["smsRecipients"].ToString();
        string[] arrSMSRecpients = ConfigurationSettings.AppSettings["smsRecipients"].ToString().Split(';').ToArray();
        protected System.Timers.Timer timer = new System.Timers.Timer(Convert.ToInt32(ConfigurationSettings.AppSettings["timerInterval"]));
        private static string ChemicalMachinePath = ConfigurationSettings.AppSettings.Get("ChemicalMachinePath").ToString();
        private static string ChemicalBackUpDestination = ConfigurationSettings.AppSettings.Get("ChemicalBackUpDestination").ToString();
        private static string ChemicalIntermidiateDestination = ConfigurationSettings.AppSettings.Get("ChemicalIntermidiateDestination").ToString();
        #endregion

        #region Private Methods
        public static List<Requests> FillFileContent(string fileName, int intRecipeID)
        {
            try
            {
                const Int32 BufferSize = 128;
                List<Requests> lstRequests = new List<Requests>();
                Requests objRequests = null;
                char[] ch = { '.' };

                using (var fileStream = File.OpenRead(ChemicalMachinePath + fileName))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            objRequests = new Requests();
                            objRequests.ObjDyeDetials = new DyeDetails();

                            string addCount = line.Substring(0, 2);
                            int addiCount = Convert.ToInt32(addCount);
                            string LabItemIDPart = line.Substring(2, 8);
                            Regex regexObj1 = new Regex(@"[^\d]");
                            Regex regexObj2 = new Regex(@"\s+");
                            LabItemIDPart = regexObj1.Replace(LabItemIDPart, "");

                            string qtyPart = line.Substring(8, 12);
                            string[] qtyParts = qtyPart.Split(ch);

                            string intPart = regexObj2.Replace(qtyParts[0].ToString(), "");

                            if (intPart == String.Empty || Convert.ToInt32(intPart) == 0)
                            {
                                intPart = "0";
                            }
                            string decimalPart = regexObj2.Replace(qtyParts[1].ToString(), "");

                            if (decimalPart == String.Empty || Convert.ToInt32(decimalPart) == 0)
                            {
                                decimalPart = "0000";
                            }

                            string fullQtyPart = intPart + "." + decimalPart;
                            decimal dyeReqQty = Math.Round(Convert.ToDecimal(fullQtyPart), 4);
                            string dateAndTimepart = line.Substring(20, 16);
                            string datePart = dateAndTimepart.Substring(0, 8);

                            string newDatePart = datePart.Substring(0, 4) + "/" + datePart.Substring(4, 2) + "/" + datePart.Substring(6, 2);

                            string timePart = dateAndTimepart.Substring(8, 8);
                            string fullDatePart = newDatePart + " " + timePart;
                            DateTime dtWeighedDate = DateTime.Now;
                            if (fullDatePart != String.Empty)
                            {
                                dtWeighedDate = Convert.ToDateTime(fullDatePart);
                            }

                            if (LabItemIDPart != String.Empty || fullQtyPart != String.Empty || fullDatePart != String.Empty)
                            {
                                int LabItemID = Convert.ToInt32(LabItemIDPart);
                                objRequests.ObjDyeDetials.DyeLabItemID = LabItemID;
                                objRequests.ObjDyeDetials.DyeQuantity = dyeReqQty;
                                objRequests.RecipeID = intRecipeID;
                                objRequests.ObjDyeDetials.AdditionCount = addiCount;
                                objRequests.ObjDyeDetials.WeighedDate = dtWeighedDate;
                                lstRequests.Add(objRequests);
                            }
                        }
                    }
                }

                return lstRequests;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<Requests> FillManualIsuueDetails(string fileName)
        {
            List<Requests> lstRequests = new List<Requests>();
            try
            {
                const Int32 BufferSize = 128;
                char[] ch = { '.' };

                Requests objRequests = null;
                using (var fileStream = File.OpenRead(ChemicalMachinePath + fileName))
                {
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                    {
                        String line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            objRequests = new Requests();
                            objRequests.ObjDyeDetials = new DyeDetails();

                            string addCount = line.Substring(0, 2);
                            int addiCount = Convert.ToInt32(addCount);
                            //if (addiCount < 10)
                            //{
                            //    addiCount = Convert.ToInt32(addiCount.ToString().Substring(1, 1));
                            //}

                            string LabItemIDPart = line.Substring(2, 8);
                            Regex regexObj1 = new Regex(@"[^\d]");
                            Regex regexObj2 = new Regex(@"\s+");
                            LabItemIDPart = regexObj1.Replace(LabItemIDPart, "");

                            string qtyPart = line.Substring(8, 12);
                            string[] qtyParts = qtyPart.Split(ch);

                            string intPart = regexObj2.Replace(qtyParts[0].ToString(), "");

                            if (intPart == String.Empty || Convert.ToInt32(intPart) == 0)
                            {
                                intPart = "0";
                            }
                            string decimalPart = regexObj2.Replace(qtyParts[1].ToString(), "");

                            if (decimalPart == String.Empty || Convert.ToInt32(decimalPart) == 0)
                            {
                                decimalPart = "0000";
                            }
                            string fullQtyPart = intPart + "." + decimalPart;
                            decimal dyeReqQty = Math.Round(Convert.ToDecimal(fullQtyPart), 4);

                            string dateAndTimepart = line.Substring(20, 16);
                            string datePart = dateAndTimepart.Substring(0, 8);

                            string newDatePart = datePart.Substring(0, 4) + "/" + datePart.Substring(4, 2) + "/" + datePart.Substring(6, 2);

                            string timePart = dateAndTimepart.Substring(8, 8);
                            string fullDatePart = newDatePart + " " + timePart;

                            DateTime dtWeighedDate = DateTime.Now;
                            if (fullDatePart != String.Empty)
                            {
                                dtWeighedDate = Convert.ToDateTime(fullDatePart);
                            }

                            if (LabItemIDPart != String.Empty || fullQtyPart != String.Empty || fullDatePart != String.Empty)
                            {
                                int LabItemID = Convert.ToInt32(LabItemIDPart);
                                objRequests.ObjDyeDetials.DyeLabItemID = LabItemID;
                                objRequests.ObjDyeDetials.DyeQuantity = dyeReqQty;
                                objRequests.ObjDyeDetials.AdditionCount = addiCount;
                                objRequests.ObjDyeDetials.WeighedDate = dtWeighedDate;
                                lstRequests.Add(objRequests);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return lstRequests;
        }

        public bool CheckFileCountExceed()
        {
            bool result = false;
            char[] ch = { '.' };
            try
            {
                if (System.IO.Directory.Exists(sourcepath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcepath);
                    FileCount = files.Count();

                    if (FileCount >= 5)
                    {
                        result = true;
                        //Here Send the Message to the Responsible Parties
                        objAutoIssueManager = new AutoIssueLA301Manager();
                        SMS objSMS = null;
                        List<SMS> lstSMS = new List<SMS>();
                        //foreach (string SMSRecpients in arrSMSRecpients)
                        //{
                        //    objSMS = new SMS();
                        //    objSMS.PhoneNo = SMSRecpients.ToString();
                        //    objSMS.Message = "Dye Weighing Auto Issue System Has Stoped";
                        //    objSMS.IsSMS = true;
                        //    lstSMS.Add(objSMS);
                        //}
                        if (lstSMS.Count() > 0)
                        {
                            objAutoIssueManager = new AutoIssueLA301Manager();
                            //bool res = objAutoIssueManager.SendSMSForDyeWeighingAutoIssueError(lstSMS);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                EventLog.WriteEntry(ex.Message);
                this.Stop();
            }
            return result;
        }

        private static void SaveCaugthOKFiles(string fileName, DateTime caughtDate)
        {
            AutoIssueLA301Manager objAutoIssueManager = new AutoIssueLA301Manager();
            objAutoIssueManager.SaveCaugthOKFiles(fileName, caughtDate);
        }

        public void IsuuePendings()
        {
            Ping ping = null;

            try
            {
                char[] ch = { '.' };
                if (System.IO.Directory.Exists(sourcepath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcepath);

                    DirectoryInfo dir = new DirectoryInfo(sourcepath);

                    FileInfo[] fileinfo = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();
                    Array.Reverse(fileinfo);
                    FileCount = fileinfo.Count();
                    foreach (FileInfo filesinfos in fileinfo)
                    {
                        ping = new Ping();
                        PingReply pingReply = ping.Send("LA301", 1);
                        if (pingReply.Status == IPStatus.Success)
                        {
                            //string fileName = System.IO.Path.GetFileName(fileNames);
                            string fileName = filesinfos.Name;
                            List<Requests> lstTemp = new List<Requests>();

                            SaveCaugthOKFiles(fileName, DateTime.Now);
                            if (fileName.Substring(0, 3).ToString() != "00_")
                            {
                                string strrecipeID = fileName.Split(ch)[0];
                                int intRecipeID = Convert.ToInt32(strrecipeID);
                                lstTemp = FillFileContent(fileName, intRecipeID);

                                if (lstTemp.Count > 0)
                                {
                                    objAutoIssueManager = new AutoIssueLA301Manager();
                                    bool blIsComplete = objAutoIssueManager.SaveChemicalRequisitionsNew(lstTemp, intRecipeID, false,true);
                                    if (blIsComplete == true)
                                    {
                                        Logger.LoggIssueItems(strrecipeID);
                                        if (File.Exists(ChemicalBackUpDestination + fileName))
                                        {
                                            System.IO.File.Delete(ChemicalBackUpDestination + fileName);
                                        }
                                        System.IO.File.Move(ChemicalMachinePath + fileName, ChemicalBackUpDestination + fileName);
                                        FileCount--;
                                        Thread.Sleep(2000);
                                    }
                                }
                            }

                            else
                            {
                                string manualIsuueNo = fileName.Split(ch)[0];
                                List<Requests> lstRequest = new List<Requests>();
                                lstRequest = FillManualIsuueDetails(fileName);
                                if (lstRequest.Count > 0)
                                {
                                    objAutoIssueManager = new AutoIssueLA301Manager();
                                    bool res = objAutoIssueManager.SaveManualIssueChemicals(manualIsuueNo, lstRequest);
                                    if (res)
                                    {
                                        Logger.LoggIssueItems(manualIsuueNo);


                                        if (File.Exists(ChemicalBackUpDestination + fileName))
                                        {
                                            System.IO.File.Delete(ChemicalBackUpDestination + fileName);
                                        }
                                        System.IO.File.Move(ChemicalMachinePath + fileName, ChemicalBackUpDestination + fileName);
                                        FileCount--;
                                        Thread.Sleep(2000);
                                    }

                                }
                            }

                            if (FileCount == 2)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(ex.Message);
                this.Stop();

            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //IssuePendingsNew();

            if (CheckFileCountExceed())
            {
                IsuuePendings();
            }
        }     
        #endregion

        public AutoIssueLA_301()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            IsuuePendings();
            
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Enabled = true;
            }
            catch (Exception ex)
            {

                Logger.LoggError(ex, "CheckFileCountExceed()");
                EventLog.WriteEntry(ex.Message);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }
    }
}
