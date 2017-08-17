using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace AutoIssueLA301DAL
{
    public class SqlConn
    {
        public static SqlConnection OpenConnectiion()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings.Get("ConnStr").ToString());
                // string c = ConfigurationManager.ConnectionStrings["conString"].ConnectionString.ToString();
                // c = c.Replace("NaturubSystemUsers", "DyeERP");


                if (con.State == ConnectionState.Closed)
                {
                    con.Open();

                }
                else
                    con.Close();
                return con;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings.Get("ConnStr").ToString());
                // string c = ConfigurationManager.ConnectionStrings["conString"].ConnectionString.ToString();
                // c = c.Replace("NaturubSystemUsers", "DyeERP");
                return con;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
