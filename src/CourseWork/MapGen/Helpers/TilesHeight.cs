namespace CourseWork.MapGen.Helpers;

public sealed class TilesHeight : TilesProperty
{
    public Constants.Biomes THeight { get; }

    public double HeightValue { get; set; }
    public TilesHeight(Constants.Biomes height, RgbColor color) : base(color) => THeight = height;
}