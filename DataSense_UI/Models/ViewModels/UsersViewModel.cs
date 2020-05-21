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
    public class UsersViewModel
    {
        public List<UserProfileResp> users;

        private string endPointUsers = Configuration.APIPath() + "/users";
        private string endpointEditUser = Configuration.APIPath() + "/users/{id}";
        private string endpointAddDataSet = Configuration.APIPath() + "/dataset";
        private string endpointUserProfile = Configuration.APIPath() + "/users/{id}/profile";
        private string endpointUserActiveAPI = Configuration.APIPath() + "/users/{id}/apikey";
        public bool errorOccurred { get; set; }

        public UserProfilePatchView userPatchView { get; set; }
        public UserProfilePatch userPatch { get; set;  }
        public UserAPIActivePatch userAPIPatch { get; set; }
        public UserProfileAdd userAdd { get; set;  }
        public async Task GenerateUsersView(HttpSessionStateBase currentsession)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endPointUsers;

            APIClient apiclient = new APIClient();
            HttpResponseMessage usersResp = await apiclient.getAsync(objHttpObject);
            if (!usersResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await usersResp.Content.ReadAsStringAsync();
            users = JsonConvert.DeserializeObject<List<UserProfileResp>>(val);

            foreach (UserProfileResp u in users)
            {
                if (u.createdBy != null)
                {
                    objHttpObject.id = Convert.ToString(u.createdBy);
                    objHttpObject.endPoint = endpointUserProfile;
                    HttpResponseMessage profileResp = await apiclient.getAsync(objHttpObject);
                    if (!profileResp.IsSuccessStatusCode)
                    {
                        errorOccurred = true;
                    }
                    string profileVal = await profileResp.Content.ReadAsStringAsync();
                    UserProfileResp userprof = JsonConvert.DeserializeObject<UserProfileResp>(profileVal);
                    u.createdByUser = userprof.emailAddress;
                }
            }

        }
        public async Task EditView(HttpSessionStateBase currentsession, int uid)
        {
            string accessToken = UserSession.accessToken(currentsession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointUserProfile;
            objHttpObject.id = Convert.ToString(uid);
            APIClient apiclient = new APIClient();
            HttpResponseMessage usersResp = await apiclient.getAsync(objHttpObject);
            if (!usersResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await usersResp.Content.ReadAsStringAsync();
            userPatchView = JsonConvert.DeserializeObject<UserProfilePatchView>(val);
            if (userPatchView.modifiedBy != null)
            {
                objHttpObject.id = Convert.ToString(userPatchView.modifiedBy);
                objHttpObject.endPoint = endpointUserProfile;
                HttpResponseMessage profileResp = await apiclient.getAsync(objHttpObject);
                if (!profileResp.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
                string profileVal = await profileResp.Content.ReadAsStringAsync();
                UserProfileResp userprof = JsonConvert.DeserializeObject<UserProfileResp>(profileVal);
                userPatchView.modifiedByUser = userprof.emailAddress;
            }

        }
        public async Task PatchUsers(HttpSessionStateBase currentSession)
        {
            
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointEditUser;
            objHttpObject.id = Convert.ToString(userPatchView.id);

            userPatch = new UserProfilePatch();
            userPatch.firstName = userPatchView.firstName;
            userPatch.lastName = userPatchView.lastName;
            userPatch.active = userPatchView.active;

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(userPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            
        }
        public async Task AddUser(HttpSessionStateBase currentSession)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endPointUsers;
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(userAdd), false);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task ActiveAPI(HttpSessionStateBase currentSession, int UserId, string APIKey, bool? APIActive)
        {

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointUserActiveAPI;
            objHttpObject.id = Convert.ToString(UserId);

            if(APIActive == null)
            {
                APIActive = false;
            }

            userAPIPatch = new UserAPIActivePatch();
            //userAPIPatch.apiKey = APIKey;
            userAPIPatch.active = APIActive;

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(userAPIPatch), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }

        }
    }
}