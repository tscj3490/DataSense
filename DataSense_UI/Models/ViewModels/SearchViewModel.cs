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
    public class SearchViewModel
    {
        public bool errorOccurred;
        public string errormsg { get; set; }
        private string endPointDataSetIdxDetails = Configuration.APIPath() + "/dataset/index/{id}/profile";
        private string endPointSearchDataSet = Configuration.APIPath() + "/search/dataset";
        private string endPointSearchPattern = Configuration.APIPath() + "/search/pattern";
        public DataSetIndexView dataSetIndexView { get; set; }
        public SearchPost searchPost { get; set; }
        public PatternSearchPost patternSearchPost { get; set;  }
        public List<MatchPairView> searchResults { get; set; }
        public List<LocationView> patternSearchResults { get; set; }
        public string searchType { get; set;  }
        public string nodeUrl { get; set;  }
        public bool IsNext { get; set; }
        public bool IsPrev { get; set; }
        public async Task GenerateSearchView(HttpSessionStateBase currentsession, int dataSetIndexId)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endPointDataSetIdxDetails;
            objHttpObject.id = Convert.ToString(dataSetIndexId);
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsIdxProfResp = await apiclient.getAsync(objHttpObject);
            if (!dsIdxProfResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsIdxProfResp.Content.ReadAsStringAsync();
            dataSetIndexView = JsonConvert.DeserializeObject<DataSetIndexView>(val);
            nodeUrl = dataSetIndexView.nodeUrl;

            
        }
        public async Task PostSearch(HttpSessionStateBase currentsession, int dataSetIndexId)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            searchPost.dataSetIndexId = dataSetIndexId;

            objHttpObject.endPoint = endPointSearchDataSet;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsSearchResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(searchPost), false, nodeUrl);
            if (!dsSearchResp.IsSuccessStatusCode)
            {
                string val = await dsSearchResp.Content.ReadAsStringAsync();
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
                if (dsSearchResp.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    errormsg = "No matches were found";
                }
                errorOccurred = true;
            }
            else
            {
                string val = await dsSearchResp.Content.ReadAsStringAsync();
                searchResults = JsonConvert.DeserializeObject<List<MatchPairView>>(val);
                searchResults = searchResults.OrderByDescending(t => t.JaccardIndex).ToList();

                IsNext = false;
                IsPrev = false;
                if (searchPost.pageNo != 1)
                {
                    IsPrev = true;
                }
                searchPost.pageNo = searchPost.pageNo + 1;

                HttpResponseMessage dsSearchResp1 = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(searchPost), false, nodeUrl);
                if (dsSearchResp1.IsSuccessStatusCode)
                {
                    string val1 = await dsSearchResp1.Content.ReadAsStringAsync();
                    List<MatchPairView> searchResults1 = JsonConvert.DeserializeObject<List<MatchPairView>>(val1);
                    if (searchResults1.Count > 0)
                    {
                        IsNext = true;
                    }
                }
            }
            
            searchPost.pageNo = searchPost.pageNo - 1;
            
        }



    }
}