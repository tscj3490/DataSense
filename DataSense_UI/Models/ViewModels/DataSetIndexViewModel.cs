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
    public class DataSetIndexViewModel
    {
            
        public List<DataSetIndexView> dataSetIndexViewList { get; set; }

        public DataSetIndexPatchView dataSetIndexPatchView { get; set; }
        public DataSetIndexPatch dataSetIndexPatch { get; set; }

        public DataSetIndexPostView dataSetIndexPostView { get; set; }
        public bool errorOccurred { get; set; }
        public string DataSetName { get; set; }
        private string dataSetIndexesEndPoint = Configuration.APIPath() + "/dataset/index/{id}";
        private string dataSetIndexesdatasetidEndPoint = Configuration.APIPath() + "/dataset/index/{datasetid}";
        private string userProfileEndPoint = Configuration.APIPath() + "/users/myprofile";
        private string addDataSetIndexEndpoint = Configuration.APIPath() + "/dataset/index"; //POST NEW INDEX
        private string updateDataSetIndexEndPoint = Configuration.APIPath() + "/dataset/index/{id}"; //PATCH NEW INDEX
        private string dataSetIndexesStatusEndPoint = Configuration.APIPath() + "/dataset/index/{id}/status"; //dataSetIndexesStatus
        public async Task GenerateDataSetIndexes(HttpSessionStateBase currentsession, string id)
        {
            dataSetIndexViewList = new List<DataSetIndexView>();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = dataSetIndexesEndPoint;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            dataSetIndexViewList = JsonConvert.DeserializeObject<List<DataSetIndexView>>(val);

            foreach (DataSetIndexView resp in dataSetIndexViewList)
            {
                objHttpObject.endPoint = dataSetIndexesStatusEndPoint;
                if (resp.status == null)
                {
                    objHttpObject.id = Convert.ToString(resp.id);
                    HttpResponseMessage profileResp = await apiclient.getAsync(objHttpObject);
                    if (!profileResp.IsSuccessStatusCode)
                    {
                        errorOccurred = true;
                    }
                    else
                    {
                        string profileVal = await profileResp.Content.ReadAsStringAsync();
                        DataSetIndexStatusView objDataSetIndexStatusView = JsonConvert.DeserializeObject<DataSetIndexStatusView>(profileVal);
                        if(objDataSetIndexStatusView.status.id==0)
                        {
                            resp.status = "N/A";
                        }
                        else
                        {
                            resp.status = objDataSetIndexStatusView.status.name;
                        }
                        
                    }

                }
                await dataSetName(currentsession, id);
            }
            
        }
        public async Task dataSetName(HttpSessionStateBase currentSession,string id)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(currentSession, id);
            DataSetName = dsview.dataSetPatch.dataSetName;
        }
        public async Task GetDataSet(HttpSessionStateBase currentSession, string dsid,string dataSetId)
        {
            dataSetIndexPatchView = new DataSetIndexPatchView();
            dataSetIndexPatch = new DataSetIndexPatch();
            dataSetIndexPatchView.id = Convert.ToInt32(dsid);
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = dataSetId;
            objHttpObject.endPoint = dataSetIndexesdatasetidEndPoint;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            else
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                dataSetIndexViewList = JsonConvert.DeserializeObject<List<DataSetIndexView>>(val);

                foreach (DataSetIndexView resp in dataSetIndexViewList)
                {
                    if (resp.id == dataSetIndexPatchView.id)
                    {
                        dataSetIndexPatchView.id = resp.id;
                        dataSetIndexPatchView.dataSetId = resp.dataSetId;
                        dataSetIndexPatchView.dataSetColumns = resp.dataSetColumns;
                        dataSetIndexPatchView.limitRecords = (resp.limitRecords == null ? 0 : resp.limitRecords);
                        dataSetIndexPatchView.shingleSize = resp.shingleSize;
                        dataSetIndexPatchView.rowsPerBand = resp.rowsPerBand;
                        dataSetIndexPatchView.procType = resp.procType;
                        dataSetIndexPatchView.nodeUrl = resp.nodeUrl;
                        dataSetIndexPatchView.active = resp.active;
                        dataSetIndexPatchView.status = resp.status;

                        dataSetIndexPatch.dataSetId = dataSetIndexPatchView.dataSetId;
                        dataSetIndexPatch.dataSetColumns = dataSetIndexPatchView.dataSetColumns;
                        dataSetIndexPatch.limitRecords = (dataSetIndexPatchView.limitRecords == null ? 0 : dataSetIndexPatchView.limitRecords);
                        dataSetIndexPatch.shingleSize = dataSetIndexPatchView.shingleSize;
                        dataSetIndexPatch.rowsPerBand = dataSetIndexPatchView.rowsPerBand;
                        dataSetIndexPatch.procType = dataSetIndexPatchView.procType;
                        dataSetIndexPatch.nodeUrl = dataSetIndexPatchView.nodeUrl;
                        dataSetIndexPatch.active = dataSetIndexPatchView.active;

                    }
                }
            }
        }

        public async Task EditDataSet(HttpSessionStateBase currentSession,int id)
        {
            dataSetIndexPatchView = new DataSetIndexPatchView();
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = updateDataSetIndexEndPoint;
            objHttpObject.id = Convert.ToString(id);
            dataSetIndexPatchView.id = Convert.ToInt32(objHttpObject.id);
            //dataSetIndexPatch.id = 0;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(dataSetIndexPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task AddDataSetIndex(HttpSessionStateBase currentSession)
        {
            dataSetIndexPatchView = new DataSetIndexPatchView();
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = addDataSetIndexEndpoint;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(dataSetIndexPostView));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}