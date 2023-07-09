using Microsoft.AspNetCore.Mvc;
using MySampleApp.Web.Models;
using System.Diagnostics;
using MySampleApp.Core.Interfaces;
using MySampleApp.Core.Entities;
using static System.Net.Mime.MediaTypeNames;


namespace MySampleApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUrlAnalyzer _urlAnalyzer;
        public HomeController(ILogger<HomeController> logger,IUrlAnalyzer urlAnalyzer)
        {
            _logger = logger;
            _urlAnalyzer = urlAnalyzer;
        }

        public IActionResult Index()
        {
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
        public IActionResult Analyze()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(string url)
        {
            var images = await _urlAnalyzer.GetImagesFromUrl(url);
            var wordCount = await _urlAnalyzer.GetWordCount(url);

            var viewModel = new AnalyzeViewModel
            {
                ImagesXP = images,
                TotalWords = wordCount.TotalCount,
                TopWords = wordCount.WordCounts.Take(10).ToList()
            };

            return View(viewModel);
        }
    }
    
}
