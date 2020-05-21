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

    public class SearchPatternViewModel
    {
        public bool errorOccurred { get; set; }
        public string errormsg { get; set; }
        public string DataSetName { get; set; }
        public List<PatternRecViewSvc> PatternRecViewSvc { get; set; }
        public List<Reviewed> objreviewed { get; set; }
        public ResponseGetLatestFile objResponseGetLatestFile { get; set; }
        public GenericResp objGenericResp { get; set; }
        public DownloadLatestFileResult objDownloadLatestFileResult { get; set; }
        public PagingHeaders pagingHeaders { get; set; }
        public string dataSetId { get; set; }
        public string dataSetIndexId { get; set; }
        public string dataSetIndexCredId { get; set; }
        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public string Search { get; set; }
        public string sortby { get; set; }
        private string getSearchPatternEndPoint = Configuration.APIPath() + "/patternrecord/machine/credentials?id={id}&pageNumber={pageNumber}&pageSize={pageSize}";  //get
        private string postMarkReviewedEndPoint = Configuration.APIPath() + "/quarantine/machine/{id}";  //post
        private string getReviewedEndPoint = Configuration.APIPath() + "/quarantine/machine?id={id}&pageNumber={pageNumber}&pageSize={pageSize}";  //get
        private string deleteMarkSensitiveEndPoint = Configuration.APIPath() + "/quarantine/{id}";  //delete
        private string endpointUserProfile = Configuration.APIPath() + "/users/{id}/profile";
        private string postProcessFileEndPoint= Configuration.APIPath() + "/process/machine/{id}/resultsfile";
        private string postlatestfileEndPoint = Configuration.APIPath() + "/process/machine/{id}/latestfile";
        private string GetDownloadLatestFileEndPoint = Configuration.APIPath() + "/process/machine/{id}/DownloadLatestFile";
        private string ChecklatestfileEndPoint = Configuration.APIPath() + "/process/machine/{id}/checklatestfile";

        public async Task dataSetName(HttpSessionStateBase currentSession, string dataSetId)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(currentSession, dataSetId);
            DataSetName = dsview.dataSetPatch.dataSetName;
        }
        public async Task GetAllSearchPattern(HttpSessionStateBase currentsession, string CredId, string pageNumber, string pageSize, string Search, string sortby)
        {
            PatternRecViewSvc = new List<PatternRecViewSvc>();

            getSearchPatternEndPoint = getSearchPatternEndPoint.Replace("{pageNumber}", pageNumber);
            getSearchPatternEndPoint = getSearchPatternEndPoint.Replace("{pageSize}", pageSize);
            if (!string.IsNullOrEmpty(Search))
            {
                getSearchPatternEndPoint = getSearchPatternEndPoint + "&q=" + Search;
            }
            getSearchPatternEndPoint = getSearchPatternEndPoint + "&sortBy=" + sortby;
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = getSearchPatternEndPoint;
            objHttpObject.id = CredId;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            
            string val = await dsResp.Content.ReadAsStringAsync();
            PatternRecViewSvc = JsonConvert.DeserializeObject<List<PatternRecViewSvc>>(val);

            var headerValues = dsResp.Headers.GetValues("paging-headers").FirstOrDefault();
            pagingHeaders = JsonConvert.DeserializeObject<PagingHeaders>(headerValues);
        }

        public async Task MarkReviewed(HttpSessionStateBase currentsession, string CredId, List<MarkReviewsPost> objMarkReviewsPost)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = postMarkReviewedEndPoint;
            objHttpObject.id = CredId;

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objMarkReviewsPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task GetAllReviewed(HttpSessionStateBase currentsession, string CredId, string pageNumber, string pageSize, string Search, string sortby)
        {
            objreviewed = new List<Reviewed>();

            getReviewedEndPoint = getReviewedEndPoint.Replace("{pageNumber}", pageNumber);
            getReviewedEndPoint = getReviewedEndPoint.Replace("{pageSize}", pageSize);
            if (!string.IsNullOrEmpty(Search))
            {
                getReviewedEndPoint = getReviewedEndPoint + "&q=" + Search;
            }
            getReviewedEndPoint = getReviewedEndPoint + "&sortBy=" + sortby;
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = getReviewedEndPoint;
            objHttpObject.id = CredId;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }

            string val = await dsResp.Content.ReadAsStringAsync();
            objreviewed = JsonConvert.DeserializeObject<List<Reviewed>>(val);

            var headerValues = dsResp.Headers.GetValues("paging-headers").FirstOrDefault();
            pagingHeaders = JsonConvert.DeserializeObject<PagingHeaders>(headerValues);

            foreach (Reviewed resp in objreviewed)
            {
                objHttpObject.endPoint = endpointUserProfile;
                if (resp.createdby != 0)
                {
                    objHttpObject.id = Convert.ToString(resp.createdby);
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

        public async Task MarkSensitive(HttpSessionStateBase currentsession, int[] chkMarkSensitive)
        {
            for (int i = 0; i < chkMarkSensitive.Length; i++)
            {
                string accessToken = UserSession.accessToken(currentsession);
                HttpGetObject objHttpObject = new HttpGetObject();
                objHttpObject.accessToken = accessToken;
                objHttpObject.endPoint = deleteMarkSensitiveEndPoint;
                objHttpObject.id = chkMarkSensitive[i].ToString();

                APIClient apiclient = new APIClient();
                HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
                if (!dsResp.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
            }
           
        }

        public async Task ProcessFile(HttpSessionStateBase currentsession, string CredId)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = postProcessFileEndPoint;
            objHttpObject.id = CredId;
            APIClient apiclient = new APIClient();

            ResultsFilePost objResultsFilePost = new ResultsFilePost();
            objResultsFilePost.includeAllFiles = true;

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objResultsFilePost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            objGenericResp = new GenericResp();
            objGenericResp = JsonConvert.DeserializeObject<GenericResp>(val);
        }
        public async Task<string> FileChekingProcess(HttpSessionStateBase currentsession, string FileId)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = ChecklatestfileEndPoint;
            objHttpObject.id = FileId;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
           
            string val = await dsResp.Content.ReadAsStringAsync();
            objResponseGetLatestFile = JsonConvert.DeserializeObject<ResponseGetLatestFile>(val);
            return objResponseGetLatestFile.fileName;
        }
        public async Task GetLatestFile(HttpSessionStateBase currentsession, string CredId)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = postlatestfileEndPoint;
            objHttpObject.id = CredId;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            objResponseGetLatestFile = JsonConvert.DeserializeObject<ResponseGetLatestFile>(val);

        }
        public async Task DownloadFile(HttpSessionStateBase currentsession, string CredId)
        {
            objDownloadLatestFileResult = new DownloadLatestFileResult();

            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = GetDownloadLatestFileEndPoint;
            objHttpObject.id = CredId;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
                if (dsResp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    errorOccurred = true;
                    errormsg = "File not found.";
                }
            }
            else
            {
                byte[] bytes = dsResp.Content.ReadAsByteArrayAsync().Result;

                objDownloadLatestFileResult.FileName = dsResp.Content.Headers.ContentDisposition.FileName;
                objDownloadLatestFileResult.Stuff = bytes;
            }
        }
    }
}