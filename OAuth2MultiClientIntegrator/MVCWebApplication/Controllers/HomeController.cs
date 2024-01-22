using Microsoft.AspNetCore.Mvc;
using MVCWebApplication.Models;
using OAuth2MultiClientIntegrator;
using System.Diagnostics;

namespace MVCWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOAuth2AccessTokenProvider _oauth2AccessTokenProvider;

        public HomeController(
            IOAuth2AccessTokenProvider oAuth2AccessTokenProvider)
        {
            _oauth2AccessTokenProvider = oAuth2AccessTokenProvider;
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = await _oauth2AccessTokenProvider
                .GetAccessToken("ARegularClient");

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
