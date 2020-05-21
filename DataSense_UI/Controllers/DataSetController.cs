using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataSense_UI.Helpers;
using DataSense_UI.Models.ViewModels;
using System.Threading.Tasks;

namespace DataSense_UI.Controllers
{
    public class DataSetController : Controller
    {
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index()
        {
            DataSetViewModel dsviewmodel = new DataSetViewModel();
            await dsviewmodel.GenerateDataSetView(Session);
            if (dsviewmodel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(dsviewmodel);
        }

        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Edit(string id)
        {
            DataSetViewModel dsview = new DataSetViewModel();
            await dsview.GetDataSet(Session, id);
            return View(dsview);
        }
        [HttpGet]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Add()
        {
            DataSetViewModel dsview = new DataSetViewModel();
            return View(dsview);
        }
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(DataSetViewModel viewmodel)
        {
            await viewmodel.AddDataSet(Session);

            if (viewmodel.errorOccurred == true)
            {
                return RedirectToAction("Add", "DataSet");
            }

            return RedirectToAction("Index", "DataSet");
        }
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Patch(DataSetViewModel viewmodel)
        {            
            await viewmodel.EditDataSet(Session);

            if (viewmodel.errorOccurred == true)
            {
                return View(viewmodel);
            }

            return RedirectToAction("Edit", new { id = Convert.ToString(viewmodel.dataSetPatchView.id) });           
        }

    }
}