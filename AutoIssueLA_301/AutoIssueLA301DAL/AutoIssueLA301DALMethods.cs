using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace AutoIssueLA301DAL
{
    public class AutoIssueLA301DALMethods
    {

        public bool SendSMSForDyeWeighingAutoIssueError(DataTable dt)
        {
            bool res = false;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    Execute objExecute = new Execute();
                    SqlParameter[] param = new SqlParameter[]
                {
                    Execute.AddParameter("@tblSMSDetails",dt)                   
                
                };
                    objExecute.Executes("spSendSMSForDyeWeighingAutoIssueError", param, CommandType.StoredProcedure);
                    ts.Complete();
                    res = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return res;
        }

        public bool SaveChemicalRequisitionsNew(DataTable dtRecipeReqQty, int recipeID, bool IsMaual,bool isChemical)
        {
            bool blSuccess = false;
            try
            {
                Execute objExecute = new Execute();
                SqlParameter[] param = new SqlParameter[]
            {     
                Execute.AddParameter("@intRecipeID", recipeID),                
                Execute.AddParameter("@tblRequestChemicalsQtys",dtRecipeReqQty),
                   Execute.AddParameter("@IsMaual",IsMaual),
                   Execute.AddParameter("@bIsChemical",isChemical)
            };       
                DataRow dr = (DataRow)objExecute.Executes("spInsertPendingChemicalRequisitionNew", ReturnType.DataRow, param, CommandType.StoredProcedure);
                if (dr != null)
                {
                    blSuccess = true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return blSuccess;
        }

        public bool SaveManualIssueChemicals(string manualIsuueNo, DataTable dtManualRequestChemicalDetails)
        {
            bool blSuccess = false;
            try
            {
                Execute objExecute = new Execute();
                SqlParameter[] param = new SqlParameter[]
            {   
                 Execute.AddParameter("@vcManualIssueNo", manualIsuueNo),
                Execute.AddParameter("@tblManualRequestChemicalDetails",dtManualRequestChemicalDetails)
            };
                objExecute.Executes("spInsertManualIsseChemicalDetails", param, CommandType.StoredProcedure);
                blSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return blSuccess;
        }

        public void SaveCaugthOKFiles(string fileName, DateTime caughtDate)
        {
            try
            {
                Execute objExecute = new Execute();
                SqlParameter[] param = new SqlParameter[]
            {   
                 Execute.AddParameter("@vcFileName", fileName),
                Execute.AddParameter("@dtCaughtDate",caughtDate)
            };

                objExecute.Executes("spSaveCaugthOKFiles", param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
