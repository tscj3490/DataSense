using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Helpers
{
    public static class UserSession
    {
        public static void SetLoginValues(HttpSessionStateBase session, string apiKey, string accessToken, int userId)
        {
            //set the session values for logged in user
            session["userid"] = userId.ToString();
            session["apikey"] = apiKey;
            session["accessToken"] = accessToken;
        }

        public static void SetLogoutValues(HttpSessionStateBase session)
        {
            //set the session values for logged in user
            session["userid"] = "";
            session["apikey"] = "";
            session["accessToken"] = "";
        }

        public static string accessToken(HttpSessionStateBase session)
        {
            string retVal = "";
            //read the access token from the session
            try
            {
                retVal = session["accessToken"].ToString();
            }
            catch (Exception)
            {
            }
            return retVal;
        }

        public static string UserAPIKey(HttpSessionStateBase session)
        {
            string retVal = "";
            //read the access token from the session
            try
            {
                retVal = session["apikey"].ToString();
            }
            catch (Exception)
            {
            }
            return retVal;
        }

        public static int UserId(HttpSessionStateBase session)
        {
            int retVal = 0;
            //read the userid value from the session object
            try
            {
                if (session["userid"].ToString() != "")
                {
                    retVal = Convert.ToInt32(session["userid"]);
                }
            }
            catch (Exception)
            {
                //
            }
            return retVal;
        }
    }

}