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
    public class ViewRunController : Controller
    {
        // GET: ViewRun
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string id,string dataSetId)
        {
            ViewRunViewModel view = new ViewRunViewModel();
            await view.GenerateViewRunView(Session, id, dataSetId);
            return View(view);
        }
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> statuslog(string id, string dataSetId, string dataSetIndexId, string pageNo)
        {
            PageViewPost objPageViewPost = new PageViewPost();
            if (string.IsNullOrEmpty(pageNo))
            {
                objPageViewPost.pageNo = 1;
            }
            else
            {
                objPageViewPost.pageNo = Convert.ToInt32(pageNo);
            }
            ViewBag.Nextpage = objPageViewPost.pageNo + 1;
            ViewBag.Preview = objPageViewPost.pageNo - 1;
            objPageViewPost.noOfRecords = 50;

            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = dataSetIndexId;
            ViewBag.id = id;

            ViewRunViewModel view = new ViewRunViewModel();
            await view.GenerateViewRunStatusLog(Session, id, dataSetId, objPageViewPost, "STATUS");
            return View(view);
        }

        // GET: Detailed Log
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> DetailedLog(string id, string dataSetId, string dataSetIndexId,string pageNumber, string pageSize, string Search, string sortby)
        {
            if (string.IsNullOrEmpty(pageNumber))
            {
                pageNumber = "1";
            }
            if (string.IsNullOrEmpty(pageSize))
            {
                pageSize = "10";
            }
            if (string.IsNullOrWhiteSpace(Search))
            {
                Search = string.Empty;
            }
            if (string.IsNullOrEmpty(sortby))
            {
                sortby = "description_desc";
            }

            ViewRunViewModel view = new ViewRunViewModel();
            await view.GenerateViewRunDetailsLog(Session, id, dataSetId, pageNumber, pageSize, Search, sortby);

            view.dataSetId = dataSetId;
            view.dataSetIndexId = dataSetIndexId;
            view.id = id;

            view.dataSetId = dataSetId;
            view.dataSetIndexId = dataSetIndexId;
            view.id = id;

            view.pageNumber = pageNumber;
            view.pageSize = pageSize;
            view.Search = Search;
            view.sortby = sortby;


            if (view.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(view);
        }



        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> ForceComplete(string id, string dataSetId,string dataSetIndexId)
        {
            ViewRunViewModel view = new ViewRunViewModel();
            await view.ForceComplete(Session, id);
            if (view.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return Redirect("/ViewRun/Index?id="+ dataSetIndexId + "&dataSetId="+ dataSetId);
        }
    }
}