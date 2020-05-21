using DataSense_UI.Helpers;
using DataSense_UI.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace DataSense_UI.Models.ViewModels
{
    public class WebhookViewModel
    {
        public bool errorOccurred { get; set; }
        public string errormsg { get; set; }
        public string DataSetName { get; set; }
        public WebhookPost objwebhookPost { get; set; }
        public WebhookPatch objWebhookPatch { get; set; }
        public List<DataSetIndexExpHook> objDataSetIndexExpHook { get; set; }

        private string getWebhookEndPoint = Configuration.APIPath() + "/hook/expression/{id}/predefined/all";  //get
        private string getWebhookByIDEndPoint = Configuration.APIPath() + "/hook/expression/{id}/predefined";  //get By ID
        private string PostWebhookEndPoint = Configuration.APIPath() + "/hook/expression/{id}";  //POST
        private string PatchWebhookEndPoint = Configuration.APIPath() + "/hook/expression/{id}";  //Patch
        private string DeleteWebhookEndPoint = Configuration.APIPath() + "/hook/expression/{id}";  //Delete
        public async Task dataSetName(HttpSessionStateBase currentSession, string dataSetId)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(currentSession, dataSetId);
            DataSetName = dsview.dataSetPatch.dataSetName;
        }
        public async Task GetAllWebhook(HttpSessionStateBase currentsession, string DataSetIndexExpID)
        {
            objDataSetIndexExpHook = new List<DataSetIndexExpHook>();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = getWebhookEndPoint;
            objHttpObject.id = DataSetIndexExpID;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            objDataSetIndexExpHook = JsonConvert.DeserializeObject<List<DataSetIndexExpHook>>(val);
                       
        }
        public async Task AddWebhook(HttpSessionStateBase currentSession, string DataSetIndexExpID)
        {
            string accessToken = UserSession.accessToken(currentSession);

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = PostWebhookEndPoint;
            objHttpObject.id = DataSetIndexExpID;
            objwebhookPost.DataSetIndexExpID =Convert.ToInt32(DataSetIndexExpID);
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objwebhookPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                ResponseErrorView objResponseErrorView = JsonConvert.DeserializeObject<ResponseErrorView>(val);
                if (objResponseErrorView.errorCount != 0)
                {
                    errormsg = objResponseErrorView.responseErrors[0].Message;
                }
                else
                {
                    ResponseErrormessage objResponseErrormessage = JsonConvert.DeserializeObject<ResponseErrormessage>(val);
                    errormsg = objResponseErrormessage.message;
                }
                errorOccurred = true;
            }
        }
        public async Task GetWebhook(HttpSessionStateBase currentSession, string id)
        {
            objWebhookPatch = new WebhookPatch();

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = id; // webhook Id
            objHttpObject.endPoint = getWebhookByIDEndPoint;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            else
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                objDataSetIndexExpHook = JsonConvert.DeserializeObject<List<DataSetIndexExpHook>>(val);

                foreach (DataSetIndexExpHook resp in objDataSetIndexExpHook)
                {
                    if (resp.id == Convert.ToInt32(id))
                    {
                        objWebhookPatch.id = resp.id;
                        objWebhookPatch.HookURL = resp.HookURL;
                        objWebhookPatch.HookName = resp.HookName;
                        objWebhookPatch.HookAuthorization = resp.HookAuthorization;
                        break;
                    }
                }
            }
        }
        public async Task EditWebhook(HttpSessionStateBase currentSession, string Id)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = PatchWebhookEndPoint;
            objHttpObject.id = Id; // webhook Id
            objWebhookPatch.id = 0;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objWebhookPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                ResponseErrorView objResponseErrorView = JsonConvert.DeserializeObject<ResponseErrorView>(val);
                if (objResponseErrorView.errorCount != 0)
                {
                    errormsg = objResponseErrorView.responseErrors[0].Message;
                }
                else
                {
                    ResponseErrormessage objResponseErrormessage = JsonConvert.DeserializeObject<ResponseErrormessage>(val);
                    errormsg = objResponseErrormessage.message;
                }
                errorOccurred = true;
            }
        }
        public async Task DeleteWebhook(HttpSessionStateBase currentSession, string Id)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = DeleteWebhookEndPoint;
            objHttpObject.id = Id; // webhook Id
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}