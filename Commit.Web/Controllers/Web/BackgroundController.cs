using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Commit.Web.Models.ApiViewModels;
using Commit.Web.Models.BasicViewModels;
using Commit.Web.Models.WebViewModels;
using Infrastructure.Data;

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

    }
}

