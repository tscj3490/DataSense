using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataSense_UI.Models.DTO;
using System.Threading.Tasks;
using DataSense_UI.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
namespace DataSense_UI.Models.ViewModels
{
    public class AssetsInventoryViewModel
    {
        public List<AssetsInventoryResp> assetsInventories;

        private string endPointAssets = Configuration.APIPath() + "/reports/assetinventory";
        public bool errorOccurred { get; set; }

        public async Task GenerateAssetsView(HttpSessionStateBase currentsession)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endPointAssets;

            APIClient apiclient = new APIClient();
            HttpResponseMessage assetsResp = await apiclient.getAsync(objHttpObject);
            if (!assetsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await assetsResp.Content.ReadAsStringAsync();
            assetsInventories = JsonConvert.DeserializeObject<List<AssetsInventoryResp>>(val);
        }
    }
}