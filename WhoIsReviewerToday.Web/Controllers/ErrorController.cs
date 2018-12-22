using Microsoft.AspNetCore.Mvc;

namespace WhoIsReviewerToday.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index() => View();
    }
}