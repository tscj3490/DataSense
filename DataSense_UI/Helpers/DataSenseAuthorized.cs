using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using DataSense_UI;
using System.Threading.Tasks;
using System.Web.Routing;

namespace DataSense_UI.Helpers
{
    public class DataSenseAuthorized : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            
            string userProfileEndpoint = Configuration.APIPath() + "/users/{:id}/profile";
            bool isValid = true;
            string strToken = UserSession.accessToken(httpContext.Session);
            int userId = UserSession.UserId(httpContext.Session);
            HttpGetObject httpget = new HttpGetObject();
            httpget.id = Convert.ToString(userId);
            httpget.accessToken = strToken;
            userProfileEndpoint = userProfileEndpoint.Replace("{:id}", userId.ToString());
            httpget.endPoint = userProfileEndpoint;
            APIClient api = APIClient.Instance;
            try
            {
                string response = api.getSync(httpget);
            }
            catch (WebException webex)
            {
                HttpWebResponse webResp = (HttpWebResponse)webex.Response;
                if (webResp.StatusCode != HttpStatusCode.OK)
                {
                    isValid = false;
                    return isValid;
                }
            }

            return isValid;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Authenticate/Index");
        }

    }
}