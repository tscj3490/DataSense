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
    public class SearchPatternController : Controller
    {
        // GET: SearchPattern
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string CredId, string dataSetIndexId, string dataSetId, string pageNumber, string pageSize, string Search, string sortby)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["msg"])))
            {
                ViewBag.msg = Convert.ToString(TempData["msg"]);
            }
            else
            {
                ViewBag.msg = "";
            }
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
                sortby = "datecreated";
            }
            SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();
            await objSearchPatternViewModel.dataSetName(Session, dataSetId);
            await objSearchPatternViewModel.GetAllSearchPattern(Session, CredId, pageNumber, pageSize,Search,sortby);
            await objSearchPatternViewModel.GetLatestFile(Session, CredId);

            objSearchPatternViewModel.dataSetId = dataSetId;
            objSearchPatternViewModel.dataSetIndexId = dataSetIndexId;
            objSearchPatternViewModel.dataSetIndexCredId = CredId;

            objSearchPatternViewModel.pageNumber = pageNumber;
            objSearchPatternViewModel.pageSize = pageSize;
            objSearchPatternViewModel.Search = Search;
            objSearchPatternViewModel.sortby = sortby;

            if (objSearchPatternViewModel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(objSearchPatternViewModel);
        }

        // post: Mark as Reviewed
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> MarkReviewed(string CredId, string dataSetIndexId, string dataSetId, string[] chkMarkReview)
        {
            if (chkMarkReview.Length > 0)
            {
                List<MarkReviewsPost> objMarkReviewsPostList = new List<MarkReviewsPost>();
                foreach (var item in chkMarkReview)
                {
                    MarkReviewsPost objMarkReviewsPost = new MarkReviewsPost();
                    objMarkReviewsPost.fileName = item;
                    objMarkReviewsPostList.Add(objMarkReviewsPost);
                }
                SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();
                await objSearchPatternViewModel.MarkReviewed(Session, CredId, objMarkReviewsPostList);
            }
            
            return Redirect("/SearchPattern/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "");
        }

        // GET: Reviewed
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Reviewed(string CredId, string dataSetIndexId, string dataSetId, string pageNumber, string pageSize, string Search, string sortby)
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
                sortby = "filename";
            }
            SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();
            
            await objSearchPatternViewModel.dataSetName(Session, dataSetId);

            objSearchPatternViewModel.objreviewed = new List<Reviewed>();
            await objSearchPatternViewModel.GetAllReviewed(Session, CredId, pageNumber, pageSize,Search,sortby);

            objSearchPatternViewModel.dataSetId = dataSetId;
            objSearchPatternViewModel.dataSetIndexId = dataSetIndexId;
            objSearchPatternViewModel.dataSetIndexCredId = CredId;

            objSearchPatternViewModel.pageNumber = pageNumber;
            objSearchPatternViewModel.pageSize = pageSize;
            objSearchPatternViewModel.Search = Search;
            objSearchPatternViewModel.sortby = sortby;

            if (objSearchPatternViewModel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(objSearchPatternViewModel);
        }

        // post: Mark as Sensitive
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> MarkSensitive(string CredId, string dataSetIndexId, string dataSetId, int[] chkMarkSensitive)
        {
            SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();

            await objSearchPatternViewModel.MarkSensitive(Session, chkMarkSensitive);

            return Redirect("/SearchPattern/Reviewed?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "");
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> DownloadFile(string CredId, string dataSetIndexId, string dataSetId)
        {
            SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();

            await objSearchPatternViewModel.DownloadFile(Session, CredId);

            if (objSearchPatternViewModel.errorOccurred == true)
            {
                if (!string.IsNullOrEmpty(objSearchPatternViewModel.errormsg))
                {
                    TempData["msg"] = objSearchPatternViewModel.errormsg;
                }
            }
            else
            {
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                Response.ContentType = "application/octet-stream";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + objSearchPatternViewModel.objDownloadLatestFileResult.FileName);
                Response.BinaryWrite(objSearchPatternViewModel.objDownloadLatestFileResult.Stuff);
                Response.Flush();
                Response.End();
            }

            return Redirect("/SearchPattern/Index?CredId=" + CredId + "&dataSetIndexId=" + dataSetIndexId + "&dataSetId=" + dataSetId + "");
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<JsonResult> ProcessFile(string CredId, string dataSetIndexId, string dataSetId)
        {
            SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();

            await objSearchPatternViewModel.ProcessFile(Session, CredId);
            return new JsonResult { Data = objSearchPatternViewModel.objGenericResp.id, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<JsonResult> FileChekingProcess(string FileId)
        {
            SearchPatternViewModel objSearchPatternViewModel = new SearchPatternViewModel();
            string Filename = string.Empty;
            Filename = await objSearchPatternViewModel.FileChekingProcess(Session, FileId);

            return new JsonResult { Data = Filename, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}