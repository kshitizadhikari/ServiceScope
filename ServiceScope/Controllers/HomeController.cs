using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceScope.Models;
using ServiceScope.Services;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ServiceScope.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ISingletonService _singletonService;
        private IScopedService _scopedService;
        private ITransientService _transientService;
        private IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IScopedService scopedService, ITransientService transientService, ISingletonService singletonService, IConfiguration configuration)
        {
            _logger = logger;
            _scopedService = scopedService;
            _transientService = transientService;
            _singletonService = singletonService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Singleton()
        {
            return View("Singleton", _singletonService.GetGuid());
        }

        public IActionResult Scoped()
        {
            return View("Scoped", _scopedService.GetGuid());
        }

        public IActionResult Transient()
        {
            return View("Transient", _scopedService.GetGuid());
        }

        //public async Task<IActionResult> Weather()
        //{
        //    string? apiKey = _configuration.GetConnectionString("API_ID");
        //    string url = $"https://api.openweathermap.org/data/2.5/weather?lat=44.34&lon=10.99&appid={apiKey}";
        //    HttpClient httpClient = new HttpClient();

        //    try
        //    {
        //        var httpResponse = await httpClient.GetAsync(url);
        //        string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

        //        var weatherData = JsonConvert.DeserializeObject<JSONData.Root>(jsonResponse);
        //        return View("Weather", weatherData);

        //    }
        //    catch
        //    {
        //        return View("Weather", null);
        //    }
        //}
        
        public IActionResult Weather()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CityWeather(FormInput formObj)
        {
            string city = formObj.City;
            string? apiKey = _configuration.GetSection("API_Settings")["API_ID"];
            string? apiBaseUrl = _configuration.GetSection("API_Settings")["API_BaseUrl"];
            string url = $"{apiBaseUrl}?q={city}&units=metric&appid={apiKey}";
            HttpClient httpClient = new HttpClient();

            try
            {
                var httpResponse = await httpClient.GetAsync(url);
                string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

                var cityWeatherData = JsonConvert.DeserializeObject<JSONData.Root>(jsonResponse);
                return View("CityWeather", cityWeatherData);

            }
            catch
            {
                return View("CityWeather", null);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}