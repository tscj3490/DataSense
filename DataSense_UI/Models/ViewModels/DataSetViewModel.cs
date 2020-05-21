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
    public class DataSetViewModel
    {
        public List<DataSetResp> dataSets { get; set;  }
        public DataSetPatch dataSetPatch { get; set;  }
        public DataSetPost dataSetPost { get; set; }
        public DataSetPostView dataSetPostView { get; set; }
        public DataSetPatchView dataSetPatchView { get; set;  }
        private string endpointDataSets = Configuration.APIPath() + "/dataset";
        private string endpointEditDataSet = Configuration.APIPath() + "/dataset/{id}";
        private string endpointAddDataSet = Configuration.APIPath() + "/dataset";
        private string endpointUserProfile = Configuration.APIPath() + "/users/{id}/profile";
        public bool errorOccurred { get; set;  }
        public async Task GenerateDataSetView(HttpSessionStateBase currentsession)
        {
            
            dataSets = new List<DataSetResp>();
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointDataSets;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            dataSets = JsonConvert.DeserializeObject<List<DataSetResp>>(val);

            foreach(DataSetResp resp in dataSets)
            {
                objHttpObject.endPoint = endpointUserProfile;
                if (resp.createdBy != null)
                {
                    objHttpObject.id = Convert.ToString(resp.createdBy);
                    objHttpObject.endPoint = endpointUserProfile;
                    HttpResponseMessage profileResp = await apiclient.getAsync(objHttpObject);
                    if (!profileResp.IsSuccessStatusCode)
                    {
                        errorOccurred = true;
                    }
                    string profileVal = await profileResp.Content.ReadAsStringAsync();
                    UserProfileResp userprof = JsonConvert.DeserializeObject<UserProfileResp>(profileVal);
                    resp.createdByUser = userprof.emailAddress;
                }
                
                if (resp.modifiedBy != null)
                {
                    objHttpObject.id = Convert.ToString(resp.modifiedBy);
                    objHttpObject.endPoint = endpointUserProfile;
                    HttpResponseMessage profileResp = await apiclient.getAsync(objHttpObject);
                    if (!profileResp.IsSuccessStatusCode)
                    {
                        errorOccurred = true;
                    }
                    string profileVal = await profileResp.Content.ReadAsStringAsync();
                    UserProfileResp userprof = JsonConvert.DeserializeObject<UserProfileResp>(profileVal);
                    resp.modifiedByUser = userprof.emailAddress;
                }



            }

        }
        public async Task GetDataSet(HttpSessionStateBase currentSession, string dsid)
        {
            dataSetPatchView = new DataSetPatchView();
            dataSetPatch = new DataSetPatch();
            dataSetPatchView.id = Convert.ToInt32(dsid);
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointDataSets;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            dataSets = JsonConvert.DeserializeObject<List<DataSetResp>>(val);

            foreach (DataSetResp resp in dataSets)
            {
                if (resp.id == dataSetPatchView.id)
                {
                    dataSetPatchView.dataSetName = resp.dataSetName;
                    dataSetPatchView.dataSetLookUpTable = resp.dataSetLookUpTable;
                    dataSetPatchView.active = resp.active;
                    dataSetPatchView.modifiedOn = resp.modifiedOn;

                    dataSetPatch.datasetId = dataSetPatchView.id;
                    dataSetPatch.dataSetName = dataSetPatchView.dataSetName;
                    dataSetPatch.dataSetLookUpTable = dataSetPatchView.dataSetLookUpTable;
                    dataSetPatch.active = dataSetPatchView.active;
                    if (resp.modifiedBy != null)
                    {
                        objHttpObject.id = Convert.ToString(resp.modifiedBy);
                        objHttpObject.endPoint = endpointUserProfile;
                        HttpResponseMessage profileResp = await apiclient.getAsync(objHttpObject);
                        if (!profileResp.IsSuccessStatusCode)
                        {
                            errorOccurred = true;
                        }
                        string profileVal = await profileResp.Content.ReadAsStringAsync();
                        UserProfileResp userprof = JsonConvert.DeserializeObject<UserProfileResp>(profileVal);
                        resp.modifiedByUser = userprof.emailAddress;
                        dataSetPatchView.modifiedByUser = resp.modifiedByUser;
                    }
                }
            }
        }
        public async Task EditDataSet(HttpSessionStateBase currentSession)
        {

            dataSetPatchView = new DataSetPatchView();
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointEditDataSet;
            objHttpObject.id = Convert.ToString(dataSetPatch.datasetId);
            dataSetPatchView.id = Convert.ToInt32(objHttpObject.id);
            dataSetPatch.datasetId = 0;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(dataSetPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
        public async Task AddDataSet(HttpSessionStateBase currentSession)
        {

            dataSetPatchView = new DataSetPatchView();
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointAddDataSet;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(dataSetPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}