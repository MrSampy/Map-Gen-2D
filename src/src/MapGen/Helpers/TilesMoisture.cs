namespace CourseWork.MapGen.Helpers;

public sealed class TilesMoisture : TilesProperty
{
    public Constants.MoistureType Moisture { get; }
    public TilesMoisture(Constants.MoistureType moisture, RgbColor color) : base(color) => Moisture = moisture;
}