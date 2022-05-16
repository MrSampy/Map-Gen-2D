using Microsoft.AspNetCore.Mvc;
using CourseWork.Map;
namespace CourseWork.Controllers;

public class MapController:Controller
{
    public MapBuilder Map;
    public int Side;
    public int LenofPix;
    public MapController(int side, int lenofpix)
    {
        //Map = new MapBuilder(width,height,lenofpix);
        Side = side;
        LenofPix = lenofpix;

    }

    public IActionResult Index()
    {
        return View();
    }
    
   
}