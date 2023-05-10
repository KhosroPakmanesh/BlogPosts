using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UsingHCaptchaWithActionAndPageFilters.Models;
using UsingHCaptchaWithActionAndPageFilters.Utilities.HCaptcha.ActionFilters;

namespace UsingHCaptchaWithActionAndPageFilters.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ConfigureHCaptchaActionFilter]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ProtectByHCaptchaActionFilter]
        public IActionResult ContactUs(ContactUsModel contactUsModel)
        {
            //Here you can store the contact message.
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}