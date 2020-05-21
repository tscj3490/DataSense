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
    public class ViewMachinesController : Controller
    {
        // GET: ViewMachines
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string dataSetIndexId, string dataSetId,string pageNumber,string pageSize, string pageNumberdb, string pageSizedb,string Search,string Searchdb,string isActive, string isActivedb,string sortby,string sortbydb)
        {
            
            if (string.IsNullOrEmpty(pageNumber))
            {
                pageNumber = "1";
            }
            if (string.IsNullOrEmpty(pageSize))
            {
                pageSize = "10";
            }
            if (string.IsNullOrEmpty(pageNumberdb))
            {
                pageNumberdb = "1";
            }
            if (string.IsNullOrEmpty(pageSizedb))
            {
                pageSizedb = "10";
            }
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(Searchdb))
            {
                Searchdb = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(isActive))
            {
                isActive = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(isActivedb))
            {
                isActivedb = string.Empty;
            }
            if (string.IsNullOrEmpty(sortby))
            {
                sortby = "server";
            }
            if (string.IsNullOrEmpty(sortbydb))
            {
                sortbydb = "server";
            }

            Response.AddHeader("Refresh", "30");
            ViewMachinesViewModel viewModel = new ViewMachinesViewModel();
            await viewModel.dataSetName(Session, dataSetId);
            await viewModel.GetAllViewMachines(Session, dataSetIndexId, pageNumber, pageSize, pageNumberdb, pageSizedb, Search, Searchdb, isActive, isActivedb,sortby,sortbydb);
            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;

            viewModel.pageNumber = pageNumber;
            viewModel.pageSize = pageSize;
            viewModel.Search = Search;
            viewModel.isActive = isActive;
            viewModel.sortby = sortby;

            viewModel.pageNumberdb = pageNumberdb;
            viewModel.pageSizedb = pageSizedb;
            viewModel.Searchdb = Searchdb;
            viewModel.isActivedb = isActivedb;
            viewModel.sortbydb = sortbydb;

            if (viewModel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
                //return View(dsidxviewmodel);
            }
            else
                return View(viewModel);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Add(string dataSetIndexId, string dataSetId)
        {
            ViewMachinesViewModel vmview = new ViewMachinesViewModel();
            await vmview.dataSetName(Session, dataSetId);
            ViewBag.dataSetId = dataSetId;
            vmview.viewMachinesPost = new ViewMachinesPost();
            vmview.viewMachinesPost.databaseType = "MSSQL";
            if (!string.IsNullOrEmpty(dataSetIndexId))
            {
                ViewBag.dataSetIndexId = Convert.ToInt32(dataSetIndexId);
            }

            return View(vmview);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(ViewMachinesViewModel viewmodel, string dataSetId,string dataSetIndexId)
        {
           
            await viewmodel.AddViewMachine(Session, dataSetIndexId);
            
            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/ViewMachines/Index?dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return Redirect("/ViewMachines/Index?dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }


        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id, string dataSetIndexId, string dataSetId)
        {
            ViewMachinesViewModel vmview = new ViewMachinesViewModel();
            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
           
            await vmview.dataSetName(Session, dataSetId);
            await vmview.GetViewMachine(Session, id, dataSetIndexId);

            //Make sure if the value is null, just make it false
          
            if (vmview.viewMachinesPatch.databaseType == null || vmview.viewMachinesPatch.databaseType == "")
                vmview.viewMachinesPatch.databaseType = "MSSQL";

            if (vmview.errorOccurred == true)
            {
                return Redirect("/ViewMachines/Index?dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return View(vmview);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(ViewMachinesViewModel viewmodel, string dataSetIndexCredId, string dataSetIndexId, string dataSetId)
        {
            await viewmodel.EditViewMachine(Session, dataSetIndexCredId);

            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return Redirect("/ViewMachines/Edit?id=" + dataSetIndexCredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }


        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Delete(string id, string dataSetIndexId, string dataSetId)
        {
            ViewMachinesViewModel vmview = new ViewMachinesViewModel();

            await vmview.DeleteViewMachine(Session, id);
            if (vmview.errorOccurred == true)
            {
                return Redirect("/ViewMachines/Index?dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
            }
            return Redirect("/ViewMachines/Index?dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId);
        }
    }
}