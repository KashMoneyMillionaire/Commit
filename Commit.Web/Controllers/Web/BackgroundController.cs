using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Commit.Web.Models.ApiViewModels;
using Commit.Web.Models.BasicViewModels;
using Commit.Web.Models.WebViewModels;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Services;

namespace Commit.Web.Controllers.Web
{
    public partial class BackgroundController : Controller
    {
        private readonly AzureDataContext _ctx = ApplicationFactory.RetrieveContext();

        [HttpGet]
        public virtual ActionResult Index()
        {
            var dvm = _ctx.Demographics.Select(d => new DropDownViewModel
            {
                Value = d.Id,
                Text = d.Name
            }).ToList();

            var cvm = _ctx.Categories.Select(c => new DropDownViewModel
            {
                Value = c.Id,
                Text = c.Name,
            }).ToList();

            var model = new BackgroundViewModel
            {
                Demographics = dvm,
                Categories = cvm,
            };

            return View(model);
        }

        [HttpGet]
        public virtual ActionResult ImportStaarTests(string message = null, string outputPath = null)
        {
            return View(new ImportViewModel
            {
                DownloadPath = outputPath,
                Message = message
            });
        }

        [HttpPost]
        public virtual ActionResult ImportStaarTests(HttpPostedFileBase file)
        {
            var backgroundService = ApplicationFactory.RetrieveService<BackgroundService>() as BackgroundService ?? new BackgroundService();
            var unpivotorService = ApplicationFactory.RetrieveService<UnpivotorService>() as UnpivotorService ?? new UnpivotorService();
            Directory.CreateDirectory(Server.MapPath("~/App_Data/uploadlogs"));
            var logPath = Path.Combine(Server.MapPath("~/App_Data/uploadlogs"), "log.txt");
            var guid = Guid.NewGuid();

            // Verify that the user selected a file
            if (file == null || file.ContentLength <= 0)
                return RedirectToAction(MVC.Background.ImportStaarTests("File contains nothing."));

            if (Path.GetExtension(file.FileName) != ".zip")
                return RedirectToAction(MVC.Background.ImportStaarTests("Please upload zip with parsed files inside"));

            try
            {

                // save file to path for use later
                var fileName = Path.GetFileName(file.FileName);
                var outPath =
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/App_Data/uploads"), guid.ToString()));
                var path = Path.Combine(outPath.FullName, fileName ?? "sample.csv");
                file.SaveAs(path);

                var outputPath = backgroundService.UnzipFile(path);

                unpivotorService.PopulateDatabaseFromUnpivotedStaarTestDirectory(outputPath, logPath);
            }
            catch (Exception ex)
            {
                unpivotorService.WriteToLogPath(logPath, ex.Message);
                return RedirectToAction(MVC.Background.ImportStaarTests("Error occured while importing", logPath));
            }
            finally
            {
                Directory.Delete(Path.Combine(Server.MapPath("~/App_Data/uploads"), guid.ToString()), true);
            }


            // redirect back to the index action to show the form once again
            return RedirectToAction(MVC.Background.ImportStaarTests("Success", logPath));
        }

        [HttpPost]
        public virtual ActionResult DownloadLogs(string path)
        {
            return File(path, "text/plain", "Upload logs.txt");
        }
    }
}

