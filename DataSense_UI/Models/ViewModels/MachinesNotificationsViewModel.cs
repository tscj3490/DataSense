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
    public class MachinesNotificationsViewModel
    {
        MachineNotificationView machineNotificationViewInfo;
        public bool errorOccurred { get; set; }
        public string DataSetName { get; set; }
        public List<MachineNotificationView> machineNotificationView { get; set; }
        public MachineNotificationPost notificationPost { get; set; }
        public MachineNotificationPatch notificationPatch { get; set; }

        private string getMachineNotificationList = Configuration.APIPath() + "/machine/credentials/{id}/notifications";
        private string postMachineNotificationEmail = Configuration.APIPath() + "/machine/credentials/{id}/notifications";
        private string patchMachineNotificationRecord = Configuration.APIPath() + "/machine/credentials/notifications/{id}";
        private string deleteMachineNotificationRecord = Configuration.APIPath() + "/machine/credentials/notifications/{id}";

        public async Task GetAllMachineEmailRecipients(HttpSessionStateBase currentsession, int credid)
        {
            machineNotificationView = new List<MachineNotificationView>();
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = getMachineNotificationList;
            objHttpObject.id = Convert.ToString(credid);
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await dsResp.Content.ReadAsStringAsync();
            machineNotificationView = JsonConvert.DeserializeObject<List<MachineNotificationView>>(val);

        }

        public async Task AddMachineNotification(HttpSessionStateBase currentSession, int credid)
        {
            string accessToken = UserSession.accessToken(currentSession);

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = postMachineNotificationEmail;
            objHttpObject.id = Convert.ToString(credid);

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(notificationPost));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task GetViewMachineNotification(HttpSessionStateBase currentSession,string id,  int credid)
        {
            machineNotificationViewInfo = new MachineNotificationView();

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.id = Convert.ToString(credid);
            objHttpObject.endPoint = getMachineNotificationList;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.getAsync(objHttpObject);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            else
            {
                string val = await dsResp.Content.ReadAsStringAsync();
                machineNotificationView = JsonConvert.DeserializeObject<List<MachineNotificationView>>(val);
                notificationPatch = new MachineNotificationPatch();
                foreach (MachineNotificationView resp in machineNotificationView)
                {
                    if (resp.id == Convert.ToInt32(id))
                    {
                        notificationPatch.emailTo = resp.emailTo;
                        
                        break;
                    }
                }
            }
        }

        public async Task EditMachineNotifyRecord(HttpSessionStateBase currentSession, string notificationId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = patchMachineNotificationRecord;
            objHttpObject.id = notificationId;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(notificationPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task DeleteViewMachine(HttpSessionStateBase currentSession, string notificationId)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = deleteMachineNotificationRecord;
            objHttpObject.id = notificationId; // dataSetIndex Cred Id
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