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

    public class MachineNotificationController : Controller
    {
        // GET: MachineNotification
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(int credId,string dataSetIndexId,string dataSetId)
        {
            MachinesNotificationsViewModel viewModel = new MachinesNotificationsViewModel();
            
            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetAllMachineEmailRecipients(Session, credId);
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
            MachinesNotificationsViewModel viewModel = new MachinesNotificationsViewModel();
            ViewBag.dataSetId = dataSetId;
            ViewBag.credId = credId;
            ViewBag.dataSetIndexId = dataSetIndexId;

            await viewModel.dataSetName(Session, dataSetId);
            //await viewModel.AddMachineNotification(Session, credId);
            viewModel.notificationPost = new MachineNotificationPost();
            return View(viewModel);
        }
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(MachinesNotificationsViewModel viewmodel, int credId, string dataSetId, string dataSetIndexId)
        {
            await viewmodel.AddMachineNotification(Session, credId);

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/MachineNotification/Index?credId="+ credId + "&dataSetIndexId="+ dataSetIndexId + "&dataSetId="+ dataSetId);
            }
            return Redirect("/MachineNotification/Index?credId="+ credId + "&dataSetIndexId="+ dataSetIndexId + "&dataSetId="+ dataSetId);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id,int credId, string dataSetIndexId, string dataSetId)
        {
            MachinesNotificationsViewModel viewModel = new MachinesNotificationsViewModel();
            ViewBag.dataSetId = dataSetId;
            ViewBag.credId = credId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.id = id;
            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetViewMachineNotification(Session, id, credId);
            if (viewModel.errorOccurred == true)
            {
                return Redirect("/MachineNotification/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return View(viewModel);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(MachinesNotificationsViewModel viewmodel, string id, string dataSetIndexId, string dataSetId,int credId)
        {
            await viewmodel.EditMachineNotifyRecord(Session, id);

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/MachineNotification/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }

            return Redirect("/MachineNotification/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Delete(string id, string dataSetIndexId, string dataSetId,int credId)
        {
            MachinesNotificationsViewModel vmview = new MachinesNotificationsViewModel();

            await vmview.DeleteViewMachine(Session, id);
            if (vmview.errorOccurred == true)
            {
                return Redirect("/MachineNotification/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return Redirect("/MachineNotification/Index?credId=" + credId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }
    }
}