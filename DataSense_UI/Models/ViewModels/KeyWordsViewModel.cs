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
    public class KeyWordsViewModel
    {
        public DataSetKeyWord dataSetKeyWord { get; set; }
        public DataSetKeyWordPatch dataSetKeyWordPatch { get; set; }
        public List<DataSetKeyWord> dataSetKeyWordViewList { get; set; }
        public DataSetKeywordPost dataSetKeywordPost { get; set; }
        public string DataSetName { get; set; }
        private string endpointKeyWords = Configuration.APIPath() + "/datasetindex/keyword/{id}";
        private string endpointEditKeyWords = Configuration.APIPath() + "/datasetindex/keyword/{id}";
        private string endpointAddKeyWord = Configuration.APIPath() + "/datasetindex/keyword";
        private string endpointDeleteKeyWord = Configuration.APIPath() + "/datasetindex/keyword/{id}";
        private string endpointUpdateKeyWords = Configuration.APIPath() + "/datasetindex/keyword/{id}"; //PATCH 
        public bool errorOccurred { get; set; }
        public async Task GenerateKeyWordsView(HttpSessionStateBase currentsession, string id,string DatasetId)
        {

            dataSetKeyWordViewList = new List<DataSetKeyWord>();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointKeyWords;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            
            string val = await dsResp.Content.ReadAsStringAsync();
            dataSetKeyWordViewList = JsonConvert.DeserializeObject<List<DataSetKeyWord>>(val);

            DataSetIndexViewModel objDataSetIndexViewModel = new DataSetIndexViewModel();
            await objDataSetIndexViewModel.dataSetName(currentsession, DatasetId);
            DataSetName = objDataSetIndexViewModel.DataSetName;
        }
        public async Task GetDataSet(HttpSessionStateBase currentSession, string dsid, string datasetindexid)
        {
            dataSetKeyWord = new DataSetKeyWord();
            dataSetKeyWordPatch = new DataSetKeyWordPatch();
            dataSetKeyWordPatch.keyWordId = Convert.ToInt32(dsid);
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = datasetindexid;
            objHttpObject.endPoint = endpointEditKeyWords;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            dataSetKeyWordViewList = JsonConvert.DeserializeObject<List<DataSetKeyWord>>(val);

            foreach (DataSetKeyWord resp in dataSetKeyWordViewList)
            {
                if (resp.id == dataSetKeyWordPatch.keyWordId)
                {
                    dataSetKeyWord.id = resp.id;
                    dataSetKeyWord.dataSetIndexId = resp.dataSetIndexId;
                    dataSetKeyWord.keyWord = resp.keyWord;
                    dataSetKeyWord.createdBy = resp.createdBy;
                    dataSetKeyWord.createdOn = resp.createdOn;
                    dataSetKeyWord.modifiedBy = resp.modifiedBy;
                    dataSetKeyWord.modifiedOn = resp.modifiedOn;

                    dataSetKeyWordPatch.keyWordId = dataSetKeyWord.id;
                    dataSetKeyWordPatch.keyWord = dataSetKeyWord.keyWord;
                    dataSetKeyWordPatch.dataSetIndexId = dataSetKeyWord.dataSetIndexId;
                }
            }

            DataSetIndexViewModel objDataSetIndexViewModel = new DataSetIndexViewModel();
            await objDataSetIndexViewModel.dataSetName(currentSession, datasetindexid);
            DataSetName = objDataSetIndexViewModel.DataSetName;
        }
        public async Task EditDataSet(HttpSessionStateBase currentSession, int id)
        {
            dataSetKeyWord = new DataSetKeyWord();
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointUpdateKeyWords;
            objHttpObject.id = Convert.ToString(id);
            dataSetKeyWordPatch.keyWordId = 0;
            //dataSetIndexPatch.id = 0;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(dataSetKeyWordPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
        public async Task AddDataSetIndex(HttpSessionStateBase currentSession)
        {
            dataSetKeyWordPatch = new DataSetKeyWordPatch();
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointAddKeyWord;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(dataSetKeywordPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task DeleteKeyWord(HttpSessionStateBase currentSession, string id)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointDeleteKeyWord;
            objHttpObject.id = id.ToString();
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}