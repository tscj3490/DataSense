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
    public class SearchController : Controller
    {
        
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(int id)
        {
            SearchViewModel view = new SearchViewModel();
            view.searchPost = new Models.DTO.SearchPost();
            await view.GenerateSearchView(Session, id);

            view.searchPost.dataSetIndexId = id;
            return View(view);
        }

        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Search(SearchViewModel view)
        {
            //view.searchPost.Likelihood = 30;
            await view.GenerateSearchView(Session, view.dataSetIndexView.id);
            
            if (view.searchPost.pageNo == 0)
            {
                view.searchPost.pageNo = 1;
            }
           
            ViewBag.Nextpage = view.searchPost.pageNo + 1;
            ViewBag.Preview = view.searchPost.pageNo - 1;
            view.searchPost.noOfRecords = 10;

            await view.PostSearch(Session, view.dataSetIndexView.id);

            return View(view);
        }
    }
}