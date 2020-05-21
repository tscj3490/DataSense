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
    public class WebhooksController : Controller
    {
        // GET: Webhooks
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string CredId, string dataSetIndexId, string dataSetId, string DataSetIndexExpID)
        {
            WebhookViewModel viewModel = new WebhookViewModel();
            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetAllWebhook(Session, DataSetIndexExpID);
            viewModel.objwebhookPost = new WebhookPost();

            if (!string.IsNullOrEmpty(Convert.ToString(TempData["errormsg"])))
            {
                viewModel.errormsg = Convert.ToString(TempData["errormsg"]);
            }

            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.dataSetIndexCredId = CredId;
            ViewBag.DataSetIndexExpID = DataSetIndexExpID;
            if (viewModel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(viewModel);
        }
        
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(WebhookViewModel viewmodel, string CredId, string dataSetIndexId, string dataSetId, string DataSetIndexExpID)
        {
            await viewmodel.AddWebhook(Session, DataSetIndexExpID);

            if (viewmodel.errorOccurred == true)
            {
                TempData["errormsg"] = viewmodel.errormsg;
                return Redirect("/Webhooks/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "&DataSetIndexExpID="+ DataSetIndexExpID);
            }
            return Redirect("/Webhooks/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "&DataSetIndexExpID=" + DataSetIndexExpID);
        }
       
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(WebhookViewModel viewmodel, string Id, string CredId, string dataSetIndexId, string dataSetId, string DataSetIndexExpID)
        {
            await viewmodel.EditWebhook(Session, Id);

            if (viewmodel.errorOccurred == true)
            {
                TempData["errormsg"] = viewmodel.errormsg;
                return Redirect("/Webhooks/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "&DataSetIndexExpID=" + DataSetIndexExpID);
            }

            return Redirect("/Webhooks/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "&DataSetIndexExpID=" + DataSetIndexExpID);
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Delete(string id, string CredId, string dataSetIndexId, string dataSetId, string DataSetIndexExpID)
        {
            WebhookViewModel vmview = new WebhookViewModel();
            await vmview.DeleteWebhook(Session, id);
            if (vmview.errorOccurred == true)
            {
                return Redirect("/Webhooks/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "&DataSetIndexExpID=" + DataSetIndexExpID);
            }
            return Redirect("/Webhooks/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "&DataSetIndexExpID=" + DataSetIndexExpID);
        }
    }
}