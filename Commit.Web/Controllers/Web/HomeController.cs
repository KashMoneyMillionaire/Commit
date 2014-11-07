using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Domain;

namespace Commit.Web.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly OperationalDataContext _ctx = new OperationalDataContext();

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public virtual ActionResult Demo()
        {
            ViewBag.Message = "Demo";
            
            var model = new DemoViewModel
            {
                Demographics =
                    _ctx.Demographics.Select(d => new CascadingDropDownListViewModel { Text = d.Name, DropDownId = d.Id, MyId = d.Id }).ToList(),
                DemographicDetails =
                    _ctx.DemographicDetails.Select(
                        d => new CascadingDropDownListViewModel { Text = d.Description, DropDownId = d.Demographic.Id, MyId = d.Id }).ToList(),
                Categories =
                    _ctx.Categories.Select(c => new CascadingDropDownListViewModel { DropDownId = c.Id, Text = c.Name, MyId = c.Id }).ToList(),
                CategoryDetails =
                    _ctx.CategoryDetails.Where(c => c.CategoryType == CategoryType.Count)
                        .Select(c => new CascadingDropDownListViewModel { DropDownId = c.Category.Id, Text = c.Description, MyId = c.Id })
                        .ToList(),
                Subjects = _ctx.Subjects.Select(s => new CascadingDropDownListViewModel { MyId = s.Id, DropDownId = s.Id, Text = s.Description }).ToList(),
                Campuses = _ctx.Campuses.Select(s => new CascadingDropDownListViewModel { MyId = s.Id, DropDownId = s.Id, Text = s.Name }).ToList(),
            };

            return View(model);
        }
    }

    public class DemoViewModel
    {
        public List<CascadingDropDownListViewModel> Demographics { get; set; }
        public List<CascadingDropDownListViewModel> Categories { get; set; }
        public List<CascadingDropDownListViewModel> DemographicDetails { get; set; }
        public List<CascadingDropDownListViewModel> CategoryDetails { get; set; }
        public List<CascadingDropDownListViewModel> Subjects { get; set; }
        public List<CascadingDropDownListViewModel> Campuses { get; set; }
    }

    public class CascadingDropDownListViewModel
    {
        public long DropDownId { get; set; }
        public long MyId { get; set; }
        public string Text { get; set; }
    }
}