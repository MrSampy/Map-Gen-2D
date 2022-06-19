namespace CourseWork.MapGen.Helpers;

public class TilesBiome : TilesProperty
{
    public Constants.BiomeType Biome { get; }

    public TilesBiome(Constants.BiomeType biome, RgbColor color) : base(color) => Biome = biome;
}