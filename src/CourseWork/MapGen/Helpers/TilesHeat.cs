namespace CourseWork.MapGen.Helpers;

public class TilesHeat : TilesProperty
{
    public Constants.HeatType THeat { get; }

    public TilesHeat(Constants.HeatType heat, RgbColor color) : base(color)
    {
        THeat = heat;
    }
}