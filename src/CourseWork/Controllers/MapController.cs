using Microsoft.AspNetCore.Mvc;
using CourseWork.MapGen;
namespace CourseWork.Controllers;

public class MapController:Controller
{
    private static Map Map;
    [HttpGet]
    public IActionResult Index() => View();
    
    [HttpPost]
    public IActionResult Index(int? sizeId, int? lenOfPix, bool? hasRiver=false, bool? hasCastles=false, bool? hasParticles=false)
    {
        int[] numbers = {200, 300, 400, 500, 600, 700, 800};
        int side = numbers[sizeId.GetValueOrDefault(0)];
        MapBuilder mapBuilder = new MapBuilder(side, side, lenOfPix.GetValueOrDefault(1));
        Map = mapBuilder.BuildMap(hasRiver.GetValueOrDefault(false),hasCastles.GetValueOrDefault(false),hasParticles.GetValueOrDefault(false));
        ViewBag.Map = Map;
        return View("One");
    }

    
    public IActionResult One()
    {
        ViewBag.Map = Map;
        return View();
    }
    
    public IActionResult Two()
    { 
        ViewBag.Map = Map;
       return View();
    }
    
    public IActionResult Three()
    {
        ViewBag.Map = Map;
        return View();
    }
}