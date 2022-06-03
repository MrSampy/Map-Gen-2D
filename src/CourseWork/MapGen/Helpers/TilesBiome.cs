namespace CourseWork.MapGen.Helpers;

public class TilesBiome : TilesProperty
{
    public Constants.Biomes TBiome { get; }

    public TilesBiome(Constants.Biomes biome, RgbColor color) : base(color)
    {
        TBiome = biome;
    }
}