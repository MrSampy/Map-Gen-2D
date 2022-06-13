namespace CourseWork.MapGen.Helpers;

public sealed class TilesMoisture : TilesProperty
{
    public Constants.MoistureType TMoisture { get; }
    public double MoistureValue { set; get; }

    public TilesMoisture(Constants.MoistureType moisture, RgbColor color) : base(color) => TMoisture = moisture;
}