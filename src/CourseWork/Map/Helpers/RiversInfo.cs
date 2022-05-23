namespace CourseWork.Map.Helpers;

public class RiversInfo
{
    public int MaxRiverCount { get; }
    public int MaxRiverWidth { get; }

    public RiversInfo(double width, double length)
    {
        var riverCoef = 0.000015;
        Random rnd = Random.Shared;
        MaxRiverCount = rnd.Next(0,5);
        MaxRiverWidth = (int)Math.Ceiling(width * length * riverCoef);
    }
}