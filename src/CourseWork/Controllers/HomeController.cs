using System.Diagnostics;
using CourseWork.Models;
using Microsoft.AspNetCore.Mvc;
using CourseWork.Map;
namespace CourseWork.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public static MapBuilder Map;
    private int Side;
    private int LenofPix;
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    
    [HttpGet]
    public IActionResult Index() => View();
    [HttpPost]
    public IActionResult Index(int? sizeId, int? lenOfPix)

    {   
        int[] numbers = {200,300,400,500,600};
        Side = numbers[sizeId.GetValueOrDefault(0)];
        LenofPix = lenOfPix.GetValueOrDefault(1);
        Map = new MapBuilder(Side,Side,LenofPix);
        return View("One");

    }
    public IActionResult Finish()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult One()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Two()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Three()
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
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}