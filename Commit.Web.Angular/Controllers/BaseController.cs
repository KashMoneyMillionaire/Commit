using System.Web.Mvc;

namespace Commit.Web.Angular.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}