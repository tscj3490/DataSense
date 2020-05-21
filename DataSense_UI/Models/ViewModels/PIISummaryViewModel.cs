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
    public class PIISummaryViewModel
    {
        public List<PIISummaryResp> pIISummaries;
        public List<DataSetResp> datasetLists;
        public List<DataSetIndexView> dataSetIndexViewList { get; set; }

        private string endPointPiis = Configuration.APIPath() + "/reports/piisummary";
        private string endPointDatasets = Configuration.APIPath() + "/dataset";
        private string dataSetIndexesEndPoint = Configuration.APIPath() + "/dataset/index/{id}";
        public bool errorOccurred { get; set; }

        public async Task PiiSummaryView(HttpSessionStateBase currentsession)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endPointPiis;

            APIClient apiclient = new APIClient();
            HttpResponseMessage piiSummaryResp = await apiclient.getAsync(objHttpObject);
            if (!piiSummaryResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await piiSummaryResp.Content.ReadAsStringAsync();
            pIISummaries = JsonConvert.DeserializeObject<List<PIISummaryResp>>(val);
        }

        public async Task GetDatasetList(HttpSessionStateBase currentsession)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endPointDatasets;

            APIClient apiclient = new APIClient();
            HttpResponseMessage datasetListResp = await apiclient.getAsync(objHttpObject);
            if (!datasetListResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await datasetListResp.Content.ReadAsStringAsync();
            datasetLists = JsonConvert.DeserializeObject<List<DataSetResp>>(val);
        }

        public async Task GetDatasetByDatasetIndex(HttpSessionStateBase currentSession, string dataSetId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = dataSetId;
            objHttpObject.endPoint = endPointDatasets;

            APIClient apiclient = new APIClient();
            HttpResponseMessage datasetListResp = await apiclient.getAsync(objHttpObject);
            if (!datasetListResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await datasetListResp.Content.ReadAsStringAsync();
            dataSetIndexViewList = JsonConvert.DeserializeObject<List<DataSetIndexView>>(val);
        }
    }
}