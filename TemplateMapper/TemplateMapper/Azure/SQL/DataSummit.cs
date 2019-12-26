using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateMapper.Azure.SQL
{
    public static class DataSummit
    {
        public static SqlConnection conDS = null;
        public static Boolean Connect()
        {
            try
            {
                if(conDS == null)
                {
                    conDS = new SqlConnection(Properties.Settings.Default.csDataSummit);
                    if(conDS.State != ConnectionState.Open)
                    { conDS.Open(); }
                    if (conDS.State != ConnectionState.Open) return false;
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
                return false;
            }
            return true;
        }
        public static Boolean Disconnect()
        {
            try
            {
                if (conDS == null)
                {
                    conDS = new SqlConnection(Properties.Settings.Default.csDataSummit);
                    if (conDS.State != ConnectionState.Closed)
                    { conDS.Close(); }
                    if (conDS.State != ConnectionState.Closed) return false;
                }
            }
            catch (Exception ae)
            {
                String strError = ae.ToString();
                return false;
            }
            return true;
        }
    }
}
