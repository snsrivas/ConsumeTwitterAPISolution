using ConsumeTwitterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TwitterApiBorker;
using Microsoft.Extensions.Configuration;

namespace ConsumeTwitterAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public string ApiMessage { get; set; }
        public string TotalCount { get; set; }
        public string AverageTweetValue { get; set; }

        public HomeController(ILogger<HomeController> logger, IConfiguration iConfiguration)
        {
            _logger = logger;
            _configuration = iConfiguration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        
        public IActionResult GetTotalCount()
        {
            TwitterBroker twitterApiBroker = new TwitterBroker();
            var twitterDomain = _configuration["ApiDomainName"];
            var bearerToken = _configuration["APiBearerToken"];
            var totalTweets = twitterApiBroker.GetTotalTweets(twitterDomain, bearerToken);
            if (totalTweets != null)
            {
                ViewBag.TotalCount = totalTweets.Result;
                return View("Index");
            }
            else
            {
                ApiMessage = "Not Successful";
                return NotFound();
            }
        }
      
       
        public IActionResult AverageNoOfTweets()
        {
            TwitterBroker twitterApiBroker = new TwitterBroker();
            int averageNoOfTweets = twitterApiBroker.AverageNoOfTweets(_configuration["ApiDomainName"], _configuration["APiBearerToken"]);
            if (averageNoOfTweets != 0)
            {
                ViewBag.AverageTweetValue = averageNoOfTweets;
                return View("Index");
            }
            else
            {
                ApiMessage = "Not Successful";
                return NotFound();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
