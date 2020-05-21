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
    public class ScheduleController : Controller
    {
        // GET: Schedule
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Index(string id,string dataSetId)
        {
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            await scheduleViewModel.GenerateScheduleView(Session, id, dataSetId);

            ViewBag.dataSetId = dataSetId;
            ViewBag.dataSetIndexId = id;
            if (scheduleViewModel.errorOccurred)
            {
                return RedirectToAction("Index", "Authenticate");
            }
            else
                return View(scheduleViewModel);
            
        }
        [HttpPost]
        [DataSenseAuthorized]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public async Task<ActionResult> Post(ScheduleViewModel viewmodel,string radio,string dataSetId)
        {
            
            if(radio== "Hourly")
            {
                if (viewmodel.indexHourlyView.id == 0)
                {
                    await viewmodel.AddHourlyPost(Session);
                }
                else
                {
                    await viewmodel.EditHourlyPost(Session);
                }
                
            }
            else if(radio== "weekly")
            {
                if (viewmodel.indexWeeklyView.id == 0)
                {
                    await viewmodel.AddWeeklyPost(Session);
                }
                else
                {
                    await viewmodel.EditWeeklyPost(Session);
                }
            }
            else if (radio == "Daily")
            {
                if (viewmodel.indexDailyView.id == 0)
                {
                    await viewmodel.AddDailyPost(Session);
                }
                else
                {
                    await viewmodel.EditDailyPost(Session);
                }
            }
            else if (radio == "Monthly")
            {
                if (viewmodel.indexMonthlyView != null && viewmodel.indexMonthlyView.Count > 0)
                {
                    if (viewmodel.indexMonthlyView.Where(n=> n.id != 0).ToList().Count > 0) 
                    {
                        await viewmodel.DeleteMonthlyData(Session);
                    }
                    if (viewmodel.errorOccurred == true)
                    {
                        return Redirect("/Schedule?id=" + Convert.ToString(viewmodel.indexHourlyView.dataSetIndexId) + "&dataSetId=" + dataSetId);
                    }
                    await viewmodel.AddMonthlyPost(Session);
                }
            }

            if (viewmodel.errorOccurred == true)
            {
                return Redirect("/Schedule?id=" + Convert.ToString(viewmodel.indexHourlyView.dataSetIndexId)+ "&dataSetId="+ dataSetId);
            }
            return Redirect("/Schedule?id=" + Convert.ToString(viewmodel.indexHourlyView.dataSetIndexId) + "&dataSetId="+ dataSetId);
        }
       
    }
}