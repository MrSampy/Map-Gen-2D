namespace CourseWork.Map.Helpers;

public class CastlesInfo
{
    public int MaxCastleNumber { get; }
    public int WallLength { get; }
    public int WallWidth { get; }
    public int MinPathLength { get; }
    public int RoadWidth { get; }

    public CastlesInfo(int width, int height)
    {
        var wallCoef = 0.00004;
        var castleCoef = 0.0003;
        var widthCoef = 0.000015;
        Random rnd = Random.Shared;
        MaxCastleNumber = rnd.Next(1, 4);
        int square = width * height;
        WallLength = (int) Math.Ceiling(square * castleCoef);
        WallWidth = (int) Math.Ceiling(square * wallCoef);
        RoadWidth = (int) Math.Ceiling(square * widthCoef);
        MinPathLength = height / 3;
    }
}