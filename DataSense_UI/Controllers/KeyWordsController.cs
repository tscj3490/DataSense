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
    public class KeyWordsController : Controller
    {
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        // GET: KeyWords
        public async Task<ActionResult> Index(string id,string DatasetId)
        {
            KeyWordsViewModel KWviewmodel = new KeyWordsViewModel();
            await KWviewmodel.GenerateKeyWordsView(Session, id, DatasetId);
            ViewBag.DatasetId = DatasetId;
            ViewBag.DatasetIndexId = id;
            if (KWviewmodel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(KWviewmodel);
            
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Add(string id,string Datasetid)
        {
            KeyWordsViewModel dsview = new KeyWordsViewModel();

            dsview.dataSetKeywordPost = new DataSetKeywordPost();

            DataSetIndexViewModel dsview1 = new DataSetIndexViewModel();

            await dsview1.dataSetName(Session, Datasetid);
            dsview.DataSetName =dsview1.DataSetName;

            ViewBag.DatasetId = Datasetid;
            if (!string.IsNullOrEmpty(id))
            {
                dsview.dataSetKeywordPost.dataSetIndexId = Convert.ToInt32(id);
            }
            return View(dsview);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(KeyWordsViewModel viewmodel,string Datasetid)
        {
            await viewmodel.AddDataSetIndex(Session);

            if (viewmodel.errorOccurred == true)
            {
                return RedirectToAction("Add", "KeyWords");
            }
            return Redirect("/KeyWords/Index?id=" + Convert.ToString(viewmodel.dataSetKeywordPost.dataSetIndexId) + "&dataSetId=" + Datasetid);
            //return RedirectToAction("Index", "KeyWords", new { id = Convert.ToString(viewmodel.dataSetKeywordPost.dataSetIndexId)+ "&dataSetId=" + Datasetid });
           
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id, string datasetindexid,string Datasetid)
        {
            KeyWordsViewModel KWviewmodel = new KeyWordsViewModel();
            await KWviewmodel.GetDataSet(Session, id, datasetindexid);
            ViewBag.DatasetId = Datasetid;
            return View(KWviewmodel);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(KeyWordsViewModel viewmodel, string Datasetid)
        {
            await viewmodel.EditDataSet(Session, viewmodel.dataSetKeyWord.id);

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/KeyWords/Index?id=" + Convert.ToString(viewmodel.dataSetKeyWordPatch.dataSetIndexId) + "&dataSetId=" + Datasetid);
            }
            
            return Redirect("/KeyWords/Index?id="+Convert.ToString(viewmodel.dataSetKeyWordPatch.dataSetIndexId)+ "&dataSetId=" + Datasetid);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Delete(string id, string datasetindexid,string Datasetid)
        {
            KeyWordsViewModel KWviewmodel = new KeyWordsViewModel();
            await KWviewmodel.DeleteKeyWord(Session, id);

            if (KWviewmodel.errorOccurred == true)
            {
                return View(KWviewmodel);
            }
            return Redirect("/KeyWords/Index?id=" + datasetindexid + "&dataSetId=" + Datasetid);
            //return RedirectToAction("Index", "KeyWords", new { id = datasetindexid });
        }
    }
}