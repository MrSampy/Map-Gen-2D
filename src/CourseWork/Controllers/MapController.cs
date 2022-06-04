using Microsoft.AspNetCore.Mvc;
using CourseWork.MapGen;
namespace CourseWork.Controllers;

public class MapController:Controller
{
    private static Map _map = null!;
    [HttpGet]
    public IActionResult Index() => View();
    
    [HttpPost]
    public IActionResult Index(int? sizeId, int? lenOfPix, bool? hasRiver=false, bool? hasCastles=false, bool? hasParticles=false)
    {
        int[] numbers = {200, 300, 400, 500, 600, 700, 800};
        int side = numbers[sizeId.GetValueOrDefault(0)];
        MapBuilder mapBuilder = new MapBuilder(side, side, lenOfPix.GetValueOrDefault(1));
        _map = mapBuilder.BuildMap(hasRiver.GetValueOrDefault(false),hasCastles.GetValueOrDefault(false),hasParticles.GetValueOrDefault(false));
        ViewBag.Map = _map;
        return View("One");
    }

    
    public IActionResult One()
    {
        ViewBag.Map = _map;
        return View();
    }
    
    public IActionResult Two()
    { 
        ViewBag.Map = _map;
       return View();
    }
    
    public IActionResult Three()
    {
        ViewBag.Map = _map;
        return View();
    }
}