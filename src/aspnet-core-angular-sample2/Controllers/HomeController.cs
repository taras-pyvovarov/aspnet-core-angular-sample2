using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_angular_sample2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirect here if exception occurred in code due to bad input request or code bug.
        /// Will hit if any crash in .net code occurs.
        /// </summary>
        public IActionResult Error()
        {
            //Maybe log something.
            return View();
        }
    }
}