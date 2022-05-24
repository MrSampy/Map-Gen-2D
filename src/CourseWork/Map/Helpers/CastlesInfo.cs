namespace CourseWork.Map.Helpers;

public class CastlesInfo
{
    public int MaxCastleNumber { get; }
    public int WallLength { get; }
    public int WallWidth { get; }
    public int RoadNumber { get; }

    public CastlesInfo(int width, int height)
    {   
        var wallCoef = 0.00004;
        var castleCoef = 0.0003;
        Random rnd = Random.Shared;
        MaxCastleNumber = rnd.Next(2, 4);
        RoadNumber = --MaxCastleNumber;
        int square = width * height;
        WallLength = (int) Math.Ceiling(square * castleCoef);
        WallWidth = (int)Math.Ceiling(square * wallCoef);
    }

}