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
    
    public class ViewRunViewModel
    {
        public List<DataSetIndexRunView> listdataSetIndexRunView { get; set; }
        public DataSetIndexRunDetailView DataSetIndexRunDetailView { get; set; }
        public PagingHeaders pagingHeaders { get; set; }
        public string DataSetName { get; set; }
        public bool errorOccurred { get; set; }
        private string endpointrunhistory = Configuration.APIPath() + "/dataset/index/{datasetindexid}/runhistory"; //Get runhistory 
        private string endpointrundetail = Configuration.APIPath() + "/dataset/index/{id}/rundetail"; //Get runhistory 

        private string endpointrundetailLog = Configuration.APIPath() + "/dataset/index/rundetail?indexrunid={id}&pageNumber={pageNumber}&pageSize={pageSize}"; //Get runhistory Details log

        private string endpointForceComplete = Configuration.APIPath() + "/run/end/{id}"; //Post Force Complete

        public bool IsNext { get; set; }
        public bool IsPrev { get; set; }
        public string dataSetId { get; set; }
        public string dataSetIndexId { get; set; }
        public string id { get; set; }
        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public string Search { get; set; }
        public string sortby { get; set; }
        public async Task GenerateViewRunView(HttpSessionStateBase currentsession, string id, string DatasetId)
        {

            listdataSetIndexRunView = new List<DataSetIndexRunView>();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointrunhistory;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }

            string val = await dsResp.Content.ReadAsStringAsync();
               listdataSetIndexRunView = JsonConvert.DeserializeObject<List<DataSetIndexRunView>>(val);

            DataSetIndexViewModel objDataSetIndexViewModel = new DataSetIndexViewModel();
            await objDataSetIndexViewModel.dataSetName(currentsession, DatasetId);
            DataSetName = objDataSetIndexViewModel.DataSetName;
        }
        public async Task GenerateViewRunStatusLog(HttpSessionStateBase currentsession, string id, 
                                                    string DatasetId, PageViewPost objPageViewPost, string pageName)
        {

            DataSetIndexRunDetailView = new DataSetIndexRunDetailView();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointrundetail;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            //HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objPageViewPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }

            string val = await dsResp.Content.ReadAsStringAsync();
            DataSetIndexRunDetailView = JsonConvert.DeserializeObject<DataSetIndexRunDetailView>(val);

            IsNext = false;
            IsPrev = false;
            if (objPageViewPost.pageNo != 1)
            {
                IsPrev = true;
            }

            objPageViewPost.pageNo = objPageViewPost.pageNo + 1;

            dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objPageViewPost));
            val = await dsResp.Content.ReadAsStringAsync();
            DataSetIndexRunDetailView RunLogDetailSvcNext = JsonConvert.DeserializeObject<DataSetIndexRunDetailView>(val);
            if (RunLogDetailSvcNext.status.Count > 0 && pageName == "STATUS")
            {
                IsNext = true;
            }
            else if (RunLogDetailSvcNext.indexLog.Count > 0 && pageName == "DETAIL")
            {
                IsNext = true;
            }


            DataSetIndexViewModel objDataSetIndexViewModel = new DataSetIndexViewModel();
            await objDataSetIndexViewModel.dataSetName(currentsession, DatasetId);
            DataSetName = objDataSetIndexViewModel.DataSetName;
        }
        public async Task GenerateViewRunDetailsLog(HttpSessionStateBase currentsession, string id,
                                                    string DatasetId, string pageNumber, string pageSize, string Search, string sortby)
        {

            DataSetIndexRunDetailView = new DataSetIndexRunDetailView();

            endpointrundetailLog = endpointrundetailLog.Replace("{pageNumber}", pageNumber);
            endpointrundetailLog = endpointrundetailLog.Replace("{pageSize}", pageSize);
            if (!string.IsNullOrEmpty(Search))
            {
                endpointrundetailLog = endpointrundetailLog + "&q=" + Search;
            }
            endpointrundetailLog = endpointrundetailLog + "&sortBy=" + sortby;
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointrundetailLog;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }

            string val = await dsResp.Content.ReadAsStringAsync();
            DataSetIndexRunDetailView = JsonConvert.DeserializeObject<DataSetIndexRunDetailView>(val);

            var headerValues = dsResp.Headers.GetValues("paging-headers").FirstOrDefault();
            pagingHeaders = JsonConvert.DeserializeObject<PagingHeaders>(headerValues);

            DataSetIndexViewModel objDataSetIndexViewModel = new DataSetIndexViewModel();
            await objDataSetIndexViewModel.dataSetName(currentsession, DatasetId);
            DataSetName = objDataSetIndexViewModel.DataSetName;
        }
        public async Task ForceComplete(HttpSessionStateBase currentsession, string id)
        {

            DataSetIndexRunDetailView = new DataSetIndexRunDetailView();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointForceComplete;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(""));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}