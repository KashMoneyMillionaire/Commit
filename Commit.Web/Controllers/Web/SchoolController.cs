using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commit.Web.Models;
using Infrastructure.Data;
using Microsoft.Ajax.Utilities;

namespace Commit.Web.Controllers.Web
{
    public partial class SchoolController : Controller
    {
        private readonly AzureDataContext _ctx = new AzureDataContext();
        // GET: School
        public virtual ActionResult Info(string name)
        {

            if (name.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Index", "Home");
            }

            var school = _ctx.Campuses.First(c => c.Name == name);
            var model = new SchoolViewModel
            {
                Name = school.Name
            };

            return View(model);
        }
    }
}