using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitHelper
{
    public static class Configurations
    {
        //Connection string determined by Startup.IEnvironment and used privately in dbContext
        //private static string localDbConnectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DataSummitDB;Integrated Security=True;Pooling=False;Connect Timeout=30";
        //private static DataSummitDbContext dataSummitDbContext = new DataSummitDbContext(localDbConnectionString);
        private static DataSummitDbContext dataSummitDbContext = new DataSummitDbContext();

        public static string GetFunctionURL(string functionName)
        {
            try
            {
                //Connect to database
                //22- retrieve URL for named function from configurations tables
                //Disconnect
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return "";
        }
    }
}
