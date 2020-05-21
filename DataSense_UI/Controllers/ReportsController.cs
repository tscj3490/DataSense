using DataSense_UI.Helpers;
using DataSense_UI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataSense_UI.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Dashboard()
        {
            return View();
        }
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> AssetsInventoryReport()
        {
            AssetsInventoryViewModel assets = new AssetsInventoryViewModel();
            await assets.GenerateAssetsView(Session);
            return View(assets);
        } 
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> PIISummaryReport()
        {
            PIISummaryViewModel pii = new PIISummaryViewModel();
            await pii.PiiSummaryView(Session);
            await pii.GetDatasetList(Session);
            return View(pii);
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> GetDataset(string id)
        {
            PIISummaryViewModel pii = new PIISummaryViewModel();

            await pii.GetDatasetByDatasetIndex(Session, id);

            return View(pii);
        }
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult PIIAccumulationReport()
        {
            return View();
        }
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult HighRiskAssetsReport()
        {
            return View();
        }
    }
}