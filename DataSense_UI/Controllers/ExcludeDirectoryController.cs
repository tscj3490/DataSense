using DataSense_UI.Helpers;
using DataSense_UI.Models.DTO;
using DataSense_UI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataSense_UI.Controllers
{
    public class ExcludeDirectoryController : Controller
    {
        // GET: ExcludeDirectory
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string credId, string dataSetIndexId, string dataSetId)
        {
            ExcludeDirectoryViewModel viewModel = new ExcludeDirectoryViewModel();

            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetAllExcludeDirectory(Session, credId);
            if (viewModel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(viewModel);
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Add(int credId, string dataSetIndexId, string dataSetId)
        {
            ExcludeDirectoryViewModel viewModel = new ExcludeDirectoryViewModel();
            ViewBag.dataSetId = dataSetId;
            ViewBag.credId = credId;
            ViewBag.dataSetIndexId = dataSetIndexId;

            await viewModel.dataSetName(Session, dataSetId);
            viewModel.objDataSetIndexCredExcludeDirPost = new DataSetIndexCredExcludeDirPost();

            return View(viewModel);
        }
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(DataSetIndexCredExcludeDirPost objpost, int credId, string dataSetId, string dataSetIndexId)
        {
            ExcludeDirectoryViewModel viewModel = new ExcludeDirectoryViewModel();

            viewModel.objDataSetIndexCredExcludeDirPost = new DataSetIndexCredExcludeDirPost();
            viewModel.objDataSetIndexCredExcludeDirPost.credId = objpost.credId;
            viewModel.objDataSetIndexCredExcludeDirPost.directoryExclude = objpost.directoryExclude;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            await viewModel.AddExcludeDirectory(Session, credId);

            if (viewModel.errorOccurred == true)
            {
                viewModel.ErrorMessage = "Something wrong please try again.";
                return View(viewModel);
            }
            viewModel.ErrorMessage = "Data save successfully.";
            return Redirect("/ExcludeDirectory/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id, int credId, string dataSetIndexId, string dataSetId)
        {
            ExcludeDirectoryViewModel viewModel = new ExcludeDirectoryViewModel();
            ViewBag.dataSetId = dataSetId;
            ViewBag.credId = credId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.id = id;

            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetExcludeDirectory(Session, id, credId);

            if (viewModel.errorOccurred == true)
            {
                return Redirect("/ExcludeDirectory/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return View(viewModel);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(DataSetIndexCredExcludeDirPatch obj, string id, string dataSetIndexId, string dataSetId, int credId)
        {

            ExcludeDirectoryViewModel viewmodel = new ExcludeDirectoryViewModel();
            viewmodel.objDataSetIndexCredExcludeDirPatch = new DataSetIndexCredExcludeDirPatch();
            viewmodel.objDataSetIndexCredExcludeDirPatch.directoryExclude = obj.directoryExclude;

            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            await viewmodel.EditExcludeDirectory(Session, id);

            if (viewmodel.errorOccurred == true)
            {
                viewmodel.ErrorMessage = "Something wrong please try again.";
                return View(viewmodel);
            }
            viewmodel.ErrorMessage = "Data save successfully.";
            return Redirect("/ExcludeDirectory/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Delete(string id, string dataSetIndexId, string dataSetId, int credId)
        {
            ExcludeDirectoryViewModel vmview = new ExcludeDirectoryViewModel();

            await vmview.DeleteExcludeDirectory(Session, id);
            if (vmview.errorOccurred == true)
            {
                vmview.ErrorMessage = "Something wrong please try again.";
                return View(vmview);
            }
            vmview.ErrorMessage = "Record deleted successfully.";
            return Redirect("/ExcludeDirectory/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }
    }
}