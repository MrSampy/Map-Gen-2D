namespace Course_work.Map.Helpers;

public class TilesBiome
{
    public Constants.Biome TBiome { get; init; }
    public RgbColor _Color;
    public TilesBiome(Constants.Biome bi, RgbColor color)
    {
        TBiome = bi;
        _Color = color;
    }
    public void Darkify(double k)
    { 
        _Color = new RgbColor((int) (_Color.Red * k), (int) (_Color.Green * k), (int) (_Color.Blue * k));
    }
}