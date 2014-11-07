using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commit.Web.Controllers.Web
{
    public partial class BackgroundController : Controller
    {
        // GET: Background
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}