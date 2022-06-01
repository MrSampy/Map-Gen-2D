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
        const double wallCof = 0.00004;
        const double castleCof = 0.0003;
        const double widthCof = 0.000015;
        var rnd = Random.Shared;
        MaxCastleNumber = rnd.Next(1, 4);
        var square = width * height;
        WallLength = (int) Math.Ceiling(square * castleCof);
        WallWidth = (int) Math.Ceiling(square * wallCof);
        RoadWidth = (int) Math.Ceiling(square * widthCof);
        MinPathLength = height / 3;
    }
}