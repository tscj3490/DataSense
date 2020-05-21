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
    public class ScheduleViewModel
    {
        public IndexDailyView indexDailyView { get; set; }
        public IndexDailySchedulePost indexDailySchedulePost { get; set; }
        public IndexDailySchedulePatch indexDailySchedulePatch { get; set; }

        public IndexHourlyView indexHourlyView { get; set; }
        public IndexHourlySchedulePost indexHourlySchedulePost { get; set; }
        public IndexHourlySchedulePatch indexHourlySchedulePatch { get; set; }

        public IndexWeeklyView indexWeeklyView { get; set; }
        public IndexWeeklySchedulePost indexWeeklySchedulePost { get; set; }
        public IndexWeeklySchedulePatch indexWeeklySchedulePatch { get; set; }

        public List<IndexMonthlyView> indexMonthlyView { get; set; }
        public IndexMonthlySchedulePost indexMonthlySchedulePost { get; set; }
        public bool errorOccurred { get; set; }
        public string DataSetName { get; set; }
        private string endpointdailyscheduleGet = Configuration.APIPath() + "/dailyschedule/index/{id}"; // Get dailyschedule by DatasetID
        private string endpointweeklyschedulescheduleGet = Configuration.APIPath() + "/weeklyschedule/index/{id}"; // Get weeklyschedule by DatasetID
        private string endpointhourlyschedulescheduleGet = Configuration.APIPath() + "/hourlyschedule/index/{id}"; // Get hourlyschedule by DatasetID
        private string endpointmonthlyschedulescheduleGet = Configuration.APIPath() + "/monthlyschedule/index/{id}"; // Get monthlyschedule by dataSetIndexId


        public async Task GenerateScheduleView(HttpSessionStateBase currentsession, string id,string dataSetId)
        {
            indexDailyView = new IndexDailyView();
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointdailyscheduleGet;
            objHttpObject.id = id;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            indexDailyView = JsonConvert.DeserializeObject<IndexDailyView>(val);
            if (indexDailyView.id == 0)
            {
                indexDailyView.dataSetIndexId = Convert.ToInt32(id);
            }

            indexHourlyView = new IndexHourlyView();
            objHttpObject.endPoint = endpointhourlyschedulescheduleGet;
            objHttpObject.id = id;
            APIClient apiclient1 = new APIClient();
            HttpResponseMessage dsResp1 = await apiclient1.getAsync(objHttpObject);
            if (!dsResp1.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val1 = await dsResp1.Content.ReadAsStringAsync();
            indexHourlyView = JsonConvert.DeserializeObject<IndexHourlyView>(val1);
            if (indexHourlyView.id == 0)
            {
                indexHourlyView.dataSetIndexId = Convert.ToInt32(id);
            }

            indexWeeklyView = new IndexWeeklyView();
            objHttpObject.endPoint = endpointweeklyschedulescheduleGet;
            objHttpObject.id = id;
            APIClient apiclient2 = new APIClient();
            HttpResponseMessage dsResp2 = await apiclient1.getAsync(objHttpObject);
            if (!dsResp2.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val2 = await dsResp2.Content.ReadAsStringAsync();
            indexWeeklyView = JsonConvert.DeserializeObject<IndexWeeklyView>(val2);
            if (indexWeeklyView.id == 0)
            {
                indexWeeklyView.dataSetIndexId = Convert.ToInt32(id);
            }

            indexMonthlyView = new List<IndexMonthlyView>();
            objHttpObject.endPoint = endpointmonthlyschedulescheduleGet;
            objHttpObject.id = id;
            APIClient apiclient3 = new APIClient();
            HttpResponseMessage dsResp3 = await apiclient3.getAsync(objHttpObject);
            if (!dsResp3.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val3 = await dsResp3.Content.ReadAsStringAsync();
            indexMonthlyView = JsonConvert.DeserializeObject<List<IndexMonthlyView>>(val3);
            //if(indexMonthlyView !=null && indexMonthlyView.Count > 0)
            //{
            //    indexMonthlyView.ForEach(n => n.dataSetIndexId = Convert.ToInt32(id));
            //}

            DataSetIndexViewModel objDataSetIndexViewModel = new DataSetIndexViewModel();
            await objDataSetIndexViewModel.dataSetName(currentsession, dataSetId);
            DataSetName = objDataSetIndexViewModel.DataSetName;
        }

        public async Task EditHourlyPost(HttpSessionStateBase currentSession)
        {
            indexHourlySchedulePatch = new IndexHourlySchedulePatch();
            indexHourlySchedulePatch.hourlyOccurrence = indexHourlyView.hourlyOccurrence;
            

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointhourlyschedulescheduleGet;
            objHttpObject.id = Convert.ToString(indexHourlyView.id);
            
            //dataSetIndexPatch.id = 0;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexHourlySchedulePatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task AddHourlyPost(HttpSessionStateBase currentSession)
        {
            indexHourlySchedulePost = new IndexHourlySchedulePost();
            indexHourlySchedulePost.hourlyOccurrence = indexHourlyView.hourlyOccurrence;

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = Convert.ToString(indexHourlyView.dataSetIndexId);
            objHttpObject.endPoint = endpointhourlyschedulescheduleGet;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexHourlySchedulePost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task EditWeeklyPost(HttpSessionStateBase currentSession)
        {
            indexWeeklySchedulePatch = new IndexWeeklySchedulePatch();
            indexWeeklySchedulePatch.daysToRun = indexWeeklyView.daysToRun;
            indexWeeklySchedulePatch.hourToRun = indexWeeklyView.hourToRun;
            indexWeeklySchedulePatch.dataSetIndexId = indexWeeklyView.dataSetIndexId;

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointweeklyschedulescheduleGet;
            objHttpObject.id = Convert.ToString(indexWeeklyView.id);

            //dataSetIndexPatch.id = 0;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexWeeklySchedulePatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
        public async Task AddWeeklyPost(HttpSessionStateBase currentSession)
        {
            indexWeeklySchedulePost = new IndexWeeklySchedulePost();
            indexWeeklySchedulePost.daysToRun = indexWeeklyView.daysToRun;
            indexWeeklySchedulePost.hourToRun = indexWeeklyView.hourToRun;
            //indexWeeklySchedulePost.dataSetIndexId = indexWeeklyView.dataSetIndexId;

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = Convert.ToString(indexWeeklyView.dataSetIndexId);
            objHttpObject.endPoint = endpointweeklyschedulescheduleGet;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexWeeklySchedulePost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task EditDailyPost(HttpSessionStateBase currentSession)
        {
            indexDailySchedulePatch = new IndexDailySchedulePatch();
            indexDailySchedulePatch.hourToRun = indexDailyView.hourToRun;
           
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointdailyscheduleGet;
            objHttpObject.id = Convert.ToString(indexDailyView.id);
            
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexDailySchedulePatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
        public async Task AddDailyPost(HttpSessionStateBase currentSession)
        {
            indexDailySchedulePost = new IndexDailySchedulePost();
            indexDailySchedulePost.hourToRun = indexDailyView.hourToRun;
            
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = Convert.ToString(indexDailyView.dataSetIndexId);
            objHttpObject.endPoint = endpointdailyscheduleGet;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexDailySchedulePost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
        public async Task AddMonthlyPost(HttpSessionStateBase currentSession)
        {
            indexMonthlyView = indexMonthlyView.Where(n => n.monthDay.Year == 1900).ToList();
            foreach (var item in indexMonthlyView)
            {
                indexMonthlySchedulePost = new IndexMonthlySchedulePost();
                indexMonthlySchedulePost.monthDay = item.monthDay;

                string accessToken = UserSession.accessToken(currentSession);
                HttpGetObject objHttpObject = new HttpGetObject();
                objHttpObject.accessToken = accessToken;
                objHttpObject.id = Convert.ToString(item.dataSetIndexId);
                objHttpObject.endPoint = endpointmonthlyschedulescheduleGet;
                APIClient apiclient = new APIClient();

                HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(indexMonthlySchedulePost));
                if (!dsResp.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
            }
        }
        public async Task DeleteMonthlyData(HttpSessionStateBase currentSession)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = Convert.ToString(indexMonthlyView[0].dataSetIndexId);
            objHttpObject.endPoint = endpointmonthlyschedulescheduleGet;
            APIClient apiclient = new APIClient();

            HttpResponseMessage dsResp = await apiclient.deleteAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }
    }
}