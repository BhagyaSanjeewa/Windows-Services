using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AutoIssueLA301UserObjects;
using AutoIssueLA301DAL;

namespace AutoIssueLA301BLL
{
   public class AutoIssueLA301Manager
    {
        private DataTable createSMSDetails()
        {
            DataTable dt = new DataTable();

            DataColumn clmPhoneNo = new DataColumn("PhoneNo", typeof(string));
            dt.Columns.Add(clmPhoneNo);

            DataColumn clmstrMessage = new DataColumn("strMessage", typeof(string));
            dt.Columns.Add(clmstrMessage);

            DataColumn clmIsSMS = new DataColumn("IsSMS", typeof(bool));
            dt.Columns.Add(clmIsSMS);

            return dt;
        }

        private DataTable CreateTableForRequsetDetails()
        {
            DataTable dt = new DataTable();
            DataColumn clmDyeQuantity = new DataColumn("DyeQuantity", typeof(decimal));
            dt.Columns.Add(clmDyeQuantity);
            DataColumn clmLabItemCode = new DataColumn("LabItemCode", typeof(int));
            dt.Columns.Add(clmLabItemCode);
            DataColumn clmRecipeID = new DataColumn("RecipeID", typeof(int));
            dt.Columns.Add(clmRecipeID);
            DataColumn clmAdditionCount = new DataColumn("AdditionCount", typeof(int));
            dt.Columns.Add(clmAdditionCount);
            DataColumn clmEnterDate = new DataColumn("EnterDate", typeof(DateTime));
            dt.Columns.Add(clmEnterDate);

            return dt;
        }

        public bool SendSMSForDyeWeighingAutoIssueError(List<SMS> lstSMS)
        {
            DataTable dt = createSMSDetails();
            DataRow dr;
            foreach (var item in lstSMS)
            {
                dr = dt.NewRow();
                dr["PhoneNo"] = item.PhoneNo;
                dr["strMessage"] = item.Message;
                dr["IsSMS"] = item.IsSMS;
                dt.Rows.Add(dr);
            }
            AutoIssueLA301DALMethods objAutoIssueLA301DALMethods = new AutoIssueLA301DALMethods();
            return objAutoIssueLA301DALMethods.SendSMSForDyeWeighingAutoIssueError(dt);
        }

        public bool SaveChemicalRequisitionsNew(List<Requests> lstTemp, int intRecipeID, bool IsMaual, bool isChemical)
        {
            AutoIssueLA301DALMethods objAutoIssueLA301DALMethods = new AutoIssueLA301DALMethods();
            DataTable dt = CreateTableForRequsetDetails();
            foreach (var item in lstTemp)
            {
                DataRow dr = dt.NewRow();
                dr["DyeQuantity"] = item.ObjDyeDetials.DyeQuantity;
                dr["LabItemCode"] = item.ObjDyeDetials.DyeLabItemID;
                dr["RecipeID"] = item.RecipeID;
                dr["AdditionCount"] = item.ObjDyeDetials.AdditionCount;
                dr["EnterDate"] = item.ObjDyeDetials.WeighedDate;
                dt.Rows.Add(dr);
            }
            return objAutoIssueLA301DALMethods.SaveChemicalRequisitionsNew(dt, intRecipeID, IsMaual,isChemical);
        }

        public bool SaveManualIssueChemicals(string manualIsuueNo, List<Requests> lstRequest)
        {
            AutoIssueLA301DALMethods objAutoIssueLA301DALMethods = new AutoIssueLA301DALMethods();
            DataTable dt = CreateTableForRequsetDetails();

            foreach (var item in lstRequest)
            {
                DataRow dr = dt.NewRow();
                dr["DyeQuantity"] = item.ObjDyeDetials.DyeQuantity;
                dr["LabItemCode"] = item.ObjDyeDetials.DyeLabItemID;
                dr["AdditionCount"] = item.ObjDyeDetials.AdditionCount;
                dr["EnterDate"] = item.ObjDyeDetials.WeighedDate;
                dt.Rows.Add(dr);
            }
            return objAutoIssueLA301DALMethods.SaveManualIssueChemicals(manualIsuueNo, dt);
        }

        public void SaveCaugthOKFiles(string fileName, DateTime caughtDate)
        {
            AutoIssueLA301DALMethods objAutoIssueLA301DALMethods = new AutoIssueLA301DALMethods();
            objAutoIssueLA301DALMethods.SaveCaugthOKFiles(fileName, caughtDate);
        }
    }
}
