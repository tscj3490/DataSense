using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Helpers
{
    public class Configuration
    {
        public static string APIPath()
        {
            string retVal = "";     //make an empty string default
            try
            {
                retVal = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["apiPath"]);
            }
            catch { }
            return retVal;
        }

        public static string APIBaseURLPath()
        {
            string retVal = "";     //make an empty string default
            try
            {
                retVal = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["apiBaseURLPath"]);
            }
            catch { }
            return retVal;
        }

      
    }
}