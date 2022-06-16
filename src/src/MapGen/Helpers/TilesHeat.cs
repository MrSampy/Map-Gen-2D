namespace CourseWork.MapGen.Helpers;

public sealed class TilesHeat : TilesProperty
{
    public Constants.HeatType Heat { get; }
    public TilesHeat(Constants.HeatType heat, RgbColor color) : base(color) => Heat = heat;
}