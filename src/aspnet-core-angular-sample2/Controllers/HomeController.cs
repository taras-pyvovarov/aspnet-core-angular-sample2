using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_angular_sample2.Controllers
{
    [Route("")]
    [Route("Home")]
    public class HomeController : Controller
    {
        [Route("")]
        [Route(nameof(Index))]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Redirect here if exception occurred in code due to bad input request or code bug.
        /// Will hit if any crash in .net code occurs.
        /// </summary>
        [Route(nameof(Error))]
        public IActionResult Error()
        {
            //Maybe log something.
            return View();
        }
    }
}