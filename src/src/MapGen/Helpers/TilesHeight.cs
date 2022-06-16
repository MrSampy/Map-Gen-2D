namespace CourseWork.MapGen.Helpers;

public sealed class TilesHeight : TilesProperty
{
    public Constants.Biomes Height { get; }
    public TilesHeight(Constants.Biomes height, RgbColor color) : base(color) => Height = height;
}