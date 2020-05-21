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
    public class AuthenticateViewModel
    {
        [Required]
        public string emailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string passWord { get; set; }
        public string ErrorMessage { get; set; }
        public bool errorOccurred { get; set; }

        public ResetPassword objResetPassword { get; set; }
        public ChangePassword objchangePassword { get; set; }

        private string loginEndPoint = Configuration.APIPath() + "/login-key";
        private string tokenEndPoint = Configuration.APIPath() + "/login-token";
        private string userProfileEndPoint = Configuration.APIPath() + "/users/myprofile";

        private string logoutendpoint = Configuration.APIPath() + "/logout"; // Get dailyschedule by DatasetID
        private string forgotpasswordEndPoint = Configuration.APIPath() + "/reset-password"; //Get
        private string resetpasswordEndPoint = Configuration.APIPath() + "/reset-password"; // Patch
        private string endpointUserProfile = Configuration.APIPath() + "/users/{id}/profile";
        private string endpointchangepass = Configuration.APIPath() + "/users/{id}";

        public async Task<bool> AttempLogin(HttpSessionStateBase currentSession)
        {
            
            bool isSuccess = true;
            LogIn userlogin = new LogIn() { emailAddress = this.emailAddress, passWord = this.passWord };
            HttpGetObject httpreq = new HttpGetObject();
            httpreq.endPoint = loginEndPoint;

            APIClient api = new APIClient();
            HttpResponseMessage resp = await api.postAsync(httpreq, api.convertToContent(userlogin));
            if (!resp.IsSuccessStatusCode)
            {
                isSuccess = false;
                return isSuccess;
            }
            string val = await resp.Content.ReadAsStringAsync();
            APIKeyResp apikey = JsonConvert.DeserializeObject<APIKeyResp>(val);
            //use API Key to request token

            httpreq.endPoint = tokenEndPoint;
            resp = await api.postAsync(httpreq, api.convertToContent(apikey));
            if (!resp.IsSuccessStatusCode)
            {
                isSuccess = false;
                return isSuccess;
            }
            val = await resp.Content.ReadAsStringAsync();
            TokenResp token = JsonConvert.DeserializeObject<TokenResp>(val);

            httpreq.accessToken = token.token;
            httpreq.endPoint = userProfileEndPoint;
            //httpreq.

            resp = await api.getAsync(httpreq);
            if (!resp.IsSuccessStatusCode)
            {
                isSuccess = false;
                return isSuccess;
            }

            val = await resp.Content.ReadAsStringAsync();
            GenericResp genId = JsonConvert.DeserializeObject<GenericResp>(val);

            UserSession.SetLoginValues(currentSession, apikey.apiKey, token.token, Convert.ToInt32(genId.id));


            return isSuccess;
        }

        public async Task LogOut(HttpSessionStateBase currentSession)
        {
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = logoutendpoint;
            APIClient apiclient = new APIClient();
            HttpContent obj = null;
            
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, obj);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
        }

        public async Task ForgotPassword(HttpSessionStateBase currentSession)
        {
            ForgotPassword objforgotPassword = new ForgotPassword();
            objforgotPassword.emailAddress = this.emailAddress;

            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.endPoint = forgotpasswordEndPoint;
            
            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objforgotPassword));
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }            

        }

        public async Task ResetPassword(HttpSessionStateBase currentSession, ResetPassword objResetPassword)
        {
            UpdatePasswordPost objUpdatePasswordPost = new UpdatePasswordPost();
            objUpdatePasswordPost.updatedPassword = objResetPassword.newPassWord;
            objUpdatePasswordPost.emailAddress = objResetPassword.emailAddress;

            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.endPoint = resetpasswordEndPoint;
            objHttpObject.accessToken = objResetPassword.resetCode;

            APIClient apiclient = new APIClient();
            HttpResponseMessage dsResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objUpdatePasswordPost), true);
            if (!dsResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }

        }

        public async Task ChangePassword(HttpSessionStateBase currentSession)
        {
           
            string accessToken = UserSession.accessToken(currentSession);
            HttpGetObject objHttpObject = new HttpGetObject();
            objHttpObject.accessToken = accessToken;
            objHttpObject.endPoint = endpointUserProfile;
            objHttpObject.id = Convert.ToString(UserSession.UserId(currentSession));
            APIClient apiclient = new APIClient();
            HttpResponseMessage usersResp = await apiclient.getAsync(objHttpObject);
            if (!usersResp.IsSuccessStatusCode)
            {
                errorOccurred = true;
            }
            string val = await usersResp.Content.ReadAsStringAsync();
            UserProfilePatchView userPatchView = JsonConvert.DeserializeObject<UserProfilePatchView>(val);

            if (!string.IsNullOrEmpty(userPatchView.emailAddress))
            {
                LogIn userlogin = new LogIn() { emailAddress = userPatchView.emailAddress, passWord = objchangePassword.currentPassword };
                HttpGetObject httpreq = new HttpGetObject();
                httpreq.endPoint = loginEndPoint;

                APIClient api = new APIClient();
                HttpResponseMessage resp = await api.postAsync(httpreq, api.convertToContent(userlogin));
                if (!resp.IsSuccessStatusCode)
                {
                    errorOccurred = true;
                }
                else
                {
                    ChangePasswordPatch objChangePasswordPatch = new ChangePasswordPatch();
                    objChangePasswordPatch.passWord = objchangePassword.newPassWord;

                    objHttpObject.accessToken = accessToken;
                    objHttpObject.endPoint = endpointchangepass;
                    objHttpObject.id = Convert.ToString(UserSession.UserId(currentSession));
                    usersResp = await apiclient.postAsync(objHttpObject, apiclient.convertToContent(objChangePasswordPatch),true);
                    if (!usersResp.IsSuccessStatusCode)
                    {
                        errorOccurred = true;
                    }
                }
            }
            else
            {
                errorOccurred = true;
            }
        }
    }
}