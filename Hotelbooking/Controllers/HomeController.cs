using Hotelbooking_System.Models;
using Hotelbooking_System.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotelbooking.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Hotel information.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "If you have any problems, you can always find us!";

            return View();
        }
        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string path = Server.MapPath("~/Utils/Uploads");
                    string fileName = Path.GetFileName(model.AttachedFile.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    model.AttachedFile.SaveAs(fullPath);

                    EmailSender es = new EmailSender();
                    es.Send(model, fullPath);

                    ViewBag.Result = "Email has been send.";


                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }
    }
}