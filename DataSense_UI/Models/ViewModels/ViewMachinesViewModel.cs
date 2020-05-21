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
    public class ViewMachinesViewModel
    {
        public bool errorOccurred { get; set; }
        public string DataSetName { get; set; }
        public int dataSetIndexCredId { get; set; }
        public List<ViewMachines> ViewMachinesViewList { get; set; }
        public List<ViewMachines> ViewMachinesViewListdb { get; set; }
        public ViewMachinesPost viewMachinesPost { get; set; }
        public ViewMachinesPatch viewMachinesPatch { get; set; }
        public PagingHeaders pagingHeaders { get; set; }
        public PagingHeaders pagingHeadersdb { get; set; }

        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public string Search { get; set; }
        public string isActive { get; set; }
        public string sortby { get; set; }

        public string pageNumberdb { get; set; }
        public string pageSizedb { get; set; }
        public string Searchdb { get; set; }
        public string isActivedb { get; set; }
        public string sortbydb { get; set; }

        private string getViewMachinesEndPoint = Configuration.APIPath() + "/machine/credentials/datasetindex?id={id}&isDatabase={isDatabase}&pageNumber={pageNumber}&pageSize={pageSize}";  //get Machine
        private string getViewMachinesEndPointdb = Configuration.APIPath() + "/machine/credentials/datasetindex?id={id}&isDatabase={isDatabase}&pageNumber={pageNumber}&pageSize={pageSize}";  //get db
        private string postViewMachinesEndPoint = Configuration.APIPath() + "/machine/credentials/datasetindex/{id}"; //post
        private string patchViewMachinesEndPoint = Configuration.APIPath() + "/machine/credentials/{id}"; //patch
        private string deleteViewMachinesEndPoint = Configuration.APIPath() + "/machine/credentials/{id}"; //delete
        public async Task GetAllViewMachines(HttpSessionStateBase currentsession, string dataSetIndexId, string pageNumber, string pageSize, string pageNumberdb, string pageSizedb,string Search,string Searchdb,string isActive,string isActivedb, string sortby, string sortbydb)
        {
            ViewMachinesViewList = new List<ViewMachines>();
            
            try
            {
                string accessToken = UserSession.accessToken(currentsession);
                HttpGetObject objHttpObject = new HttpGetObject();

                getViewMachinesEndPoint = getViewMachinesEndPoint.Replace("{pageNumber}", pageNumber);
                getViewMachinesEndPoint = getViewMachinesEndPoint.Replace("{pageSize}", pageSize);
                getViewMachinesEndPoint = getViewMachinesEndPoint.Replace("{isDatabase}", "false");
                if (!string.IsNullOrEmpty(Search))
                {
                    getViewMachinesEndPoint = getViewMachinesEndPoint + "&q=" + Search;
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    getViewMachinesEndPoint = getViewMachinesEndPoint + "&isActive=" + isActive;
                }
                getViewMachinesEndPoint = getViewMachinesEndPoint + "&sortBy=" + sortby;
                objHttpObject.accessToken = accessToken;
                objHttpObject.endPoint = getViewMachinesEndPoint;
                objHttpObject.id = dataSetIndexId;
                APIClient apiclient = new APIClient();
                HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
                if (!dsResp.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
                string val = await dsResp.Content.ReadAsStringAsync();

                var headerValues = dsResp.Headers.GetValues("paging-headers").FirstOrDefault();
                pagingHeaders = JsonConvert.DeserializeObject<PagingHeaders> (headerValues);
                ViewMachinesViewList = JsonConvert.DeserializeObject<List<ViewMachines>>(val);

                //Database List Code
                getViewMachinesEndPointdb = getViewMachinesEndPointdb.Replace("{pageNumber}", pageNumberdb);
                getViewMachinesEndPointdb = getViewMachinesEndPointdb.Replace("{pageSize}", pageSizedb);
                getViewMachinesEndPointdb = getViewMachinesEndPointdb.Replace("{isDatabase}", "true");
                if (!string.IsNullOrEmpty(Searchdb))
                {
                    getViewMachinesEndPointdb = getViewMachinesEndPointdb + "&q=" + Searchdb;
                }
                if (!string.IsNullOrEmpty(isActivedb))
                {
                    getViewMachinesEndPointdb = getViewMachinesEndPointdb + "&isActive=" + isActivedb;
                }
                getViewMachinesEndPointdb = getViewMachinesEndPointdb + "&sortBy=" + sortbydb;
                objHttpObject.accessToken = accessToken;
                objHttpObject.endPoint = getViewMachinesEndPointdb;
                objHttpObject.id = dataSetIndexId;
                APIClient apiclient1 = new APIClient();
                HttpResponseMessage dsResp1 = await apiclient1.getAsync(objHttpObject);
                if (!dsResp1.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
                string val1 = await dsResp1.Content.ReadAsStringAsync();

                var headerValues1 = dsResp1.Headers.GetValues("paging-headers").FirstOrDefault();
                pagingHeadersdb = JsonConvert.DeserializeObject<PagingHeaders>(headerValues1);
                ViewMachinesViewListdb = JsonConvert.DeserializeObject<List<ViewMachines>>(val1);

            }
            catch(Exception ex)
            {
                //errorOccurred = true;
            }

        }

        public async Task dataSetName(HttpSessionStateBase currentSession, string dataSetId)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(currentSession, dataSetId);
            DataSetName = dsview.dataSetPatch.dataSetName;
        }

        public async Task AddViewMachine(HttpSessionStateBase currentSession, string dataSetIndexId)
        {
            string accessToken = UserSession.accessToken(currentSession);

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = postViewMachinesEndPoint;
            objHttpObject.id = dataSetIndexId;
            

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(viewMachinesPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task GetViewMachine(HttpSessionStateBase currentSession, string id, string dataSetIndexId)
        {
            viewMachinesPatch = new ViewMachinesPatch();
            

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = dataSetIndexId;
            objHttpObject.endPoint = getViewMachinesEndPoint;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            else
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                ViewMachinesViewList = JsonConvert.DeserializeObject<List<ViewMachines>>(val);

                foreach (ViewMachines resp in ViewMachinesViewList)
                {
                    if (resp.id == Convert.ToInt32(id))
                    {

                        dataSetIndexCredId = resp.id;
                        viewMachinesPatch.computerName = resp.computerName;
                        viewMachinesPatch.domainName = resp.domainName;
                        viewMachinesPatch.userName = resp.userName;
                        viewMachinesPatch.passWord = resp.passWord;
                        viewMachinesPatch.isWmi = resp.isWmi;
                        viewMachinesPatch.isDatabase = resp.isDatabase;
                        viewMachinesPatch.active = resp.active;
                        if (viewMachinesPatch.isDatabase == true)
                        {
                            if (resp.databaseType == null || resp.databaseType == "")
                                viewMachinesPatch.databaseType = "MSSQL";
                            else
                                viewMachinesPatch.databaseType = resp.databaseType;

                        }

                        break;
                    }
                }
            }
        }

        public async Task EditViewMachine(HttpSessionStateBase currentSession, string dataSetIndexCredId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = patchViewMachinesEndPoint;
            objHttpObject.id = dataSetIndexCredId; // dataSetIndex Cred Id
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(viewMachinesPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task DeleteViewMachine(HttpSessionStateBase currentSession, string dataSetIndexCredId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = deleteViewMachinesEndPoint;
            objHttpObject.id = dataSetIndexCredId; // dataSetIndex Cred Id
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}