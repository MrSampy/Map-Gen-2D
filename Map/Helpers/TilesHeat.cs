namespace Course_work.Map.Helpers;

public class TilesHeat:TilesProperty
{
    public Constants.HeatType THeat { get; set; }
    public TilesHeat(Constants.HeatType heat, RgbColor color):base(color)
        {
            THeat = heat;
        }
}