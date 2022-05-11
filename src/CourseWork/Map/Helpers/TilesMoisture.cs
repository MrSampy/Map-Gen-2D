namespace CourseWork.Map.Helpers;

public class TilesMoisture : TilesProperty
{
    public Constants.MoistureType TMoisture { get; }

    public TilesMoisture(Constants.MoistureType moisture, RgbColor color) : base(color)
    {
        TMoisture = moisture;
    }
}