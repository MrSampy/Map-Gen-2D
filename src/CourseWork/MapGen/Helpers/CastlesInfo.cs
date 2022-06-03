namespace CourseWork.MapGen.Helpers;

public sealed class CastlesInfo
{
    public int WallLength { get; }
    public int WallWidth { get; }
    public int MinPathLength { get; }
    public int RoadWidth { get; }

    public CastlesInfo(int width, int height)
    {
        const double wallCof = 0.00002;
        const double castleCof = 0.0002;
        const double widthCof = 0.000012;
        var rnd = Random.Shared;
        var square = width * height;
        WallLength = (int) Math.Ceiling(square * castleCof);
        WallWidth = (int) Math.Ceiling(square * wallCof);
        RoadWidth = (int) Math.Ceiling(square * widthCof);
        MinPathLength = height / 3;
    }
    
}