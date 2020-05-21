using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataSense_UI.Helpers;
using System.Threading.Tasks;
using DataSense_UI.Models.DTO;
using System.Net.Http;
using Newtonsoft.Json;

namespace DataSense_UI.Models.ViewModels
{
    public class DataTypeViewModel
    {
        public bool errorOccurred { get; set; }
        public string DataSetName { get; set; }
        public int dataSetIndexRegExId { get; set; }
        public List<RegDataType> regdataTypeViewList { get; set; }
        public RegDataTypePost regDataTypePost { get; set; }
        public RegDataTypePatch regDataTypePatch { get; set; }

        public List<predefinedDataTypeView> predefinedDataTypeViewList { get; set; }
        
        private string getDataTypeEndPoint = Configuration.APIPath() + "/expression/machine/credentials/{id}";  //get
        private string postDataTypeEndPoint = Configuration.APIPath() + "/expression/machine/credentials/{id}"; //post
        private string patchDataTypeEndPoint = Configuration.APIPath() + "/expression/{id}"; //patch
        private string deleteDataTypeEndPoint = Configuration.APIPath() + "/expression/{id}"; //delete

        private string getpredefinedDataTypeEndPoint = Configuration.APIPath() + "/expression/machine/credentials/{id}/predefined";  //get- Post

        public async Task dataSetName(HttpSessionStateBase currentSession, string dataSetId)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(currentSession, dataSetId);
            DataSetName = dsview.dataSetPatch.dataSetName;
        }
        public async Task GetAllDataType(HttpSessionStateBase currentsession, string CredId)
        {
            regdataTypeViewList = new List<RegDataType>();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = getDataTypeEndPoint;
            objHttpObject.id = CredId;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            regdataTypeViewList = JsonConvert.DeserializeObject<List<RegDataType>>(val);


            predefinedDataTypeViewList = new List<predefinedDataTypeView>();
            objHttpObject.endPoint = getpredefinedDataTypeEndPoint;
            objHttpObject.id = CredId;
            
            dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            val = await dsResp.Content.ReadAsStringAsync();
            predefinedDataTypeViewList = JsonConvert.DeserializeObject<List<predefinedDataTypeView>>(val);
        }


        public async Task AddDataType(HttpSessionStateBase currentSession, string CredId)
        {
            string accessToken = UserSession.accessToken(currentSession);

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = postDataTypeEndPoint;
            objHttpObject.id = CredId;

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(regDataTypePost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task GetDataType(HttpSessionStateBase currentSession, string id, string CredId)
        {
            regDataTypePatch = new RegDataTypePatch();


            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = CredId;
            objHttpObject.endPoint = getDataTypeEndPoint;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            else
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                regdataTypeViewList = JsonConvert.DeserializeObject<List<RegDataType>>(val);

                foreach (RegDataType resp in regdataTypeViewList)
                {
                    if (resp.id == Convert.ToInt32(id))
                    {
                        dataSetIndexRegExId = resp.id;
                        regDataTypePatch.regularExpression = resp.regularExpression;
                        regDataTypePatch.dataTypeDesc = resp.dataTypeDesc;
                        regDataTypePatch.active = resp.active;
                        break;
                    }
                }
            }
        }

        public async Task EditDataType(HttpSessionStateBase currentSession, string RegExId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = patchDataTypeEndPoint;
            objHttpObject.id = RegExId; // RegEx Id
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(regDataTypePatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task DeleteDataType(HttpSessionStateBase currentSession, string RegExId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = deleteDataTypeEndPoint;
            objHttpObject.id = RegExId; // RegExId
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task PredefineddatatypesPost(HttpSessionStateBase currentSession,string CredId)
        {
            string accessToken = UserSession.accessToken(currentSession);

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = getpredefinedDataTypeEndPoint;
            objHttpObject.id = CredId;

            APIClient apiclient = new APIClient();
            foreach (var item in predefinedDataTypeViewList)
            {
                DataSetIndexCredRegPredPost obj = new DataSetIndexCredRegPredPost();
                obj.active = item.active;
                obj.typeDesc = item.dataTypeDesc;

                HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(obj));
                if (!dsResp.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
            }
           
        }
    }
}