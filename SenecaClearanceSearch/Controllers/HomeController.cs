using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SenecaClearanceSearch.Models;

namespace SenecaClearanceSearch.Controllers
{
    public class HomeController : Controller
    {
        protected List<string> ExistingApplicantsInfo = new List<string>();
        public ActionResult Index()
        {
            ViewBag.Message = false;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string item)
        {
            if (item.Length > 0)
            {
                string[] ApplicantInfo = item.Trim().Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
                this.ExistingApplicantsInfo = ApplicantInfo.ToList();
                ShowApplicantsDetail applicants = new ShowApplicantsDetail();
                List<ApplicantDetail> collection = applicants.ApplicantsSearch(this.ExistingApplicantsInfo);
                return View(collection);
            }
            else
            {
                ViewBag.Message = true;
                return View();
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Seneca Insured Clearance process.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}