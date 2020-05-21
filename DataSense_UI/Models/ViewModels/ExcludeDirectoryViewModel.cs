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
    public class ExcludeDirectoryViewModel
    {
        public bool errorOccurred { get; set; }
        public string DataSetName { get; set; }
        public string ErrorMessage { get; set; }
        public List<DataSetIndexCredExcludeView> objDataSetIndexCredExcludeViewlist { get; set; }
        public DataSetIndexCredExcludeView objDataSetIndexCredExcludeView { get; set; }
        public DataSetIndexCredExcludeDirPost objDataSetIndexCredExcludeDirPost { get; set; }

        public DataSetIndexCredExcludeDirPatch objDataSetIndexCredExcludeDirPatch { get; set; }

        private string EndPointExcludeDirectory = Configuration.APIPath() + "/exclude/directories/cred/{id}";
        private string EndPointPatchDeleteExcludeDirectory = Configuration.APIPath() + "/exclude/directory/{id}";
        
        private string endpointUserProfile = Configuration.APIPath() + "/users/{id}/profile";
        public async Task GetAllExcludeDirectory(HttpSessionStateBase currentsession, string credid)
        {
            objDataSetIndexCredExcludeViewlist = new List<DataSetIndexCredExcludeView>();
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = EndPointExcludeDirectory;
            objHttpObject.id = credid;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            objDataSetIndexCredExcludeViewlist = JsonConvert.DeserializeObject<List<DataSetIndexCredExcludeView>>(val);

            foreach (DataSetIndexCredExcludeView resp in objDataSetIndexCredExcludeViewlist)
            {
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
            }
        }

        public async Task AddExcludeDirectory(HttpSessionStateBase currentSession, int credid)
        {
            string accessToken = UserSession.accessToken(currentSession);

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = EndPointExcludeDirectory;
            objHttpObject.id = Convert.ToString(credid);

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objDataSetIndexCredExcludeDirPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task GetExcludeDirectory(HttpSessionStateBase currentSession, string id, int credid)
        {
            objDataSetIndexCredExcludeView = new DataSetIndexCredExcludeView();

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = Convert.ToString(credid);
            objHttpObject.endPoint = EndPointExcludeDirectory;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            else
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                objDataSetIndexCredExcludeViewlist = JsonConvert.DeserializeObject<List<DataSetIndexCredExcludeView>>(val);
                objDataSetIndexCredExcludeDirPatch = new DataSetIndexCredExcludeDirPatch();
                foreach (DataSetIndexCredExcludeView resp in objDataSetIndexCredExcludeViewlist)
                {
                    if (resp.id == Convert.ToInt32(id))
                    {
                        objDataSetIndexCredExcludeDirPatch.directoryExclude = resp.directoryExclude;
                        break;
                    }
                }
            }
        }

        public async Task EditExcludeDirectory(HttpSessionStateBase currentSession, string directoryExcludeId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = EndPointPatchDeleteExcludeDirectory;
            objHttpObject.id = directoryExcludeId;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objDataSetIndexCredExcludeDirPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task DeleteExcludeDirectory(HttpSessionStateBase currentSession, string directoryExcludeId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = EndPointPatchDeleteExcludeDirectory;
            objHttpObject.id = directoryExcludeId; //directoryExcludeId
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task dataSetName(HttpSessionStateBase currentSession, string dataSetId)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(currentSession, dataSetId);
            DataSetName = dsview.dataSetPatch.dataSetName;
        }
    }
}