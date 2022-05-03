namespace CourseWork.Map.Helpers;

public class TilesBiome : TilesProperty
{
    public Constants.Biomes TBiome { get; init; }

    public TilesBiome(Constants.Biomes biome, RgbColor color) : base(color)
    {
        TBiome = biome;
    }
}