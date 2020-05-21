using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataSense_UI.Helpers;
using DataSense_UI.Models.ViewModels;
using System.Threading.Tasks;


namespace DataSense_UI.Controllers
{
    public class ManageUsersController : Controller
    {
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index()
        {
            UsersViewModel users = new UsersViewModel();
            await users.GenerateUsersView(Session);
            return View(users);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id)
        {
            UsersViewModel userviewedit = new UsersViewModel();
            await userviewedit.EditView(Session, Convert.ToInt32(id));
            return View(userviewedit);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(UsersViewModel viewmodel)
        {
            await viewmodel.PatchUsers(Session);
            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return RedirectToAction("Edit", new { id = Convert.ToString(viewmodel.userPatchView.id) });
        }

        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> PostUser(UsersViewModel viewmodel)
        {
            await viewmodel.AddUser(Session);
            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> ActiveAPI(int UserId, string APIKey, bool? APIActive)
        {
            UsersViewModel viewmodel = new UsersViewModel();
            await viewmodel.ActiveAPI(Session, UserId, APIKey, APIActive);
            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return RedirectToAction("Index", "ManageUsers");
        }

    }
}