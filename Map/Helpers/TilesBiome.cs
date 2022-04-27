namespace Course_work.Map.Helpers;

public class TilesBiome:TilesProperty
{
    public Constants.Biome TBiome { get; init; }
    public TilesBiome(Constants.Biome biome, RgbColor color):base(color)
    {
        TBiome = biome;
    }
}