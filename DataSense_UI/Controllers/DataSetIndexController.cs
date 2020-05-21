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
    public class DataSetIndexController : Controller
    {
        // GET: DataSetIndex
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string id)
        {
            DataSetIndexViewModel dsidxviewmodel = new DataSetIndexViewModel();
            await dsidxviewmodel.GenerateDataSetIndexes(Session, id);
            
            if (dsidxviewmodel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
                //return View(dsidxviewmodel);
            }
            else
                return View(dsidxviewmodel);

           
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Add(string id)
        {
            DataSetIndexViewModel dsview = new DataSetIndexViewModel();

            await dsview.dataSetName(Session, id);

            dsview.dataSetIndexPostView = new DataSetIndexPostView();
            
            if (!string.IsNullOrEmpty(id))
            {
                dsview.dataSetIndexPostView.dataSetId =Convert.ToInt32(id);
            }

            return View(dsview);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(DataSetIndexViewModel viewmodel)
        {
            await viewmodel.AddDataSetIndex(Session);

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/DataSetIndex/Index?id=" + Convert.ToString(viewmodel.dataSetIndexPostView.dataSetId));
                //return RedirectToAction("Add", "DataSetIndex");
            }
            return Redirect("/DataSetIndex/Index?id=" + Convert.ToString(viewmodel.dataSetIndexPostView.dataSetId));
            //return RedirectToAction("Index", "DataSetIndex");
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id,string dataSetId)
        {
            DataSetIndexViewModel dsviewIndex = new DataSetIndexViewModel();
            await dsviewIndex.GetDataSet(Session, id, dataSetId);
            await dsviewIndex.dataSetName(Session, dataSetId);
            if (dsviewIndex.errorOccurred == true)
            {
                return RedirectToAction("Index", new { id = dataSetId });
            }
            return View(dsviewIndex);
        }
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(DataSetIndexViewModel viewmodel)
        {
            await viewmodel.EditDataSet(Session, viewmodel.dataSetIndexPatchView.id);

            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return Redirect("/DataSetIndex/edit?id="+ Convert.ToString(viewmodel.dataSetIndexPatchView.id) + "&dataSetId="+ Convert.ToString(viewmodel.dataSetIndexPatch.dataSetId));
            //, new { id = Convert.ToString(viewmodel.dataSetIndexPatchView.id), dataSetId = Convert.ToString(viewmodel.dataSetIndexPatch.dataSetId) });
        }
    }
   
}