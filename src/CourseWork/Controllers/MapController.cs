using Microsoft.AspNetCore.Mvc;
using CourseWork.Map;
namespace CourseWork.Controllers;

public class MapController:Controller
{
    public static MapBuilder Map;
    private int Side;
    private int LenofPix;
    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(int? sizeId, int? lenOfPix)

    {
        int[] numbers = {200, 300, 400, 500, 600, 700, 800};
        Side = numbers[sizeId.GetValueOrDefault(0)];
        LenofPix = lenOfPix.GetValueOrDefault(1);
        Map= new MapBuilder(Side, Side, LenofPix);
        return View("One");
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
    public IActionResult Finish()
    {
        return View();
    }
}