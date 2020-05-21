using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using DataSense_UI.Helpers;
using System.Threading.Tasks;
using DataSense_UI.Models.DTO;
using System.Net.Http;
using Newtonsoft.Json;
namespace DataSense_UI.Models.ViewModels
{
    public class DashboardViewModel
    {
        public DashboardView dashboardView;
        public bool errorOccurred { get; set; }
        private string dashboardEndPoint = Configuration.APIPath() + "/dashboard";
        public async Task GenerateDashboard(HttpSessionStateBase currentsession)
        {
            dashboardView = new DashboardView();
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = dashboardEndPoint;
            
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            dashboardView = JsonConvert.DeserializeObject<DashboardView>(val);

            

        }
    }
}