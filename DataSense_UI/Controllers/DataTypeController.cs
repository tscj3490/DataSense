using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataSense_UI.Helpers;
using DataSense_UI.Models.ViewModels;
using System.Threading.Tasks;
using DataSense_UI.Models.DTO;

namespace DataSense_UI.Controllers
{
    public class DataTypeController : Controller
    {
        // GET: DataType
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string CredId, string dataSetIndexId, string dataSetId)
        {
            DataTypeViewModel viewModel = new DataTypeViewModel();
            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetAllDataType(Session, CredId);
            
            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.dataSetIndexCredId = CredId;

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
        public async Task<ActionResult> Add(string CredId, string dataSetIndexId, string dataSetId)
        {
            DataTypeViewModel vmview = new DataTypeViewModel();
            await vmview.dataSetName(Session, dataSetId);
            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.dataSetIndexCredId = CredId;
            vmview.regDataTypePost = new RegDataTypePost();
            
            return View(vmview);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(DataTypeViewModel viewmodel, string CredId, string dataSetIndexId, string dataSetId)
        {
            await viewmodel.AddDataType(Session, CredId);

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }


        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id, string CredId, string dataSetIndexId, string dataSetId)
        {
            DataTypeViewModel vmview = new DataTypeViewModel();
            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.dataSetIndexCredId = CredId;

            await vmview.dataSetName(Session, dataSetId);
            await vmview.GetDataType(Session, id, CredId);
            if (vmview.errorOccurred == true)
            {
                return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return View(vmview);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(DataTypeViewModel viewmodel, string RegExId, string CredId, string dataSetIndexId, string dataSetId)
        {
            await viewmodel.EditDataType(Session, RegExId);

            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return Redirect("/DataType/Edit?id=" + RegExId + "&CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }


        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Delete(string id, string CredId, string dataSetIndexId, string dataSetId)
        {
            DataTypeViewModel vmview = new DataTypeViewModel();

            await vmview.DeleteDataType(Session, id);
            if (vmview.errorOccurred == true)
            {
                return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> PredefineddatatypesPost(DataTypeViewModel viewmodel, string CredId, string dataSetIndexId, string dataSetId)
        {
            await viewmodel.PredefineddatatypesPost(Session, CredId);

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return Redirect("/DataType/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }
    }
}