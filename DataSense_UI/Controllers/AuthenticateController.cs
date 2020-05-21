using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataSense_UI.Models.ViewModels;
using System.Threading.Tasks;
using DataSense_UI.Models.DTO;

namespace DataSense_UI.Controllers
{
    public class AuthenticateController : Controller
    {
        // GET: Authenticate
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(AuthenticateViewModel model,string Rememberme)
        {
            if(Rememberme=="on")
            {
                Session.Timeout = 360;
            }
            else
            {
                Session.Timeout = 20;
            }
            bool successfullogin = await model.AttempLogin(Session);
            if (successfullogin == false)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                model.ErrorMessage = "Invalid username or password";
                return View("Index", model);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> LogOut()
        {
            AuthenticateViewModel obj = new AuthenticateViewModel();

            await obj.LogOut(Session);
            if (obj.errorOccurred == true)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            return RedirectToAction("Index", "Authenticate");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(AuthenticateViewModel model)
        {
            await model.ForgotPassword(Session);
            if (model.errorOccurred == true)
            {
                return RedirectToAction("ForgotPassword", "Authenticate");
            }
            model.ErrorMessage = "Email Sent.";
            return View("ForgotPassword", model);
           
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string t, string e)
        {
            AuthenticateViewModel model = new AuthenticateViewModel();
            model.objResetPassword = new ResetPassword();
            model.objResetPassword.emailAddress = e;
            model.objResetPassword.resetCode = t;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPassword objResetPassword)
        {
            AuthenticateViewModel model = new AuthenticateViewModel();

            if (string.IsNullOrEmpty(objResetPassword.emailAddress) || string.IsNullOrEmpty(objResetPassword.resetCode))
            {
                model.ErrorMessage = "Some value is missing. Please click reset password link in email.";
                return View(model);
            }
            
            model.objResetPassword = new ResetPassword();
            model.objResetPassword.emailAddress = objResetPassword.emailAddress;
            model.objResetPassword.resetCode = objResetPassword.resetCode;
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await model.ResetPassword(Session, objResetPassword);
            if (model.errorOccurred == true)
            {
                model.ErrorMessage = "Email Address or Reset Code is not valid.";
                return View(model);
            }

            model.ErrorMessage = "Your New Pasword successfully saved.";
            return View("Index", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            AuthenticateViewModel model = new AuthenticateViewModel();
            model.objchangePassword = new ChangePassword();

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword(ChangePassword objChangePassword)
        {
            AuthenticateViewModel model = new AuthenticateViewModel();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.objchangePassword = new ChangePassword();
            model.objchangePassword.confirmNewPassWord = objChangePassword.confirmNewPassWord;
            model.objchangePassword.currentPassword = objChangePassword.currentPassword;
            model.objchangePassword.newPassWord = objChangePassword.newPassWord;

            await model.ChangePassword(Session);

            if (model.errorOccurred == true)
            {
                model.ErrorMessage = "Pasword is not valid.";
                return View(model);
            }

            model.ErrorMessage = "Your New Pasword successfully saved.";
            return View("ChangePassword", model);
        }
    }
}