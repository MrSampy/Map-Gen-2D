namespace CourseWork.MapGen.Helpers;

public class TilesBiome:TilesProperty
{
    public Constants.BiomeType TBiome { get; }

    public TilesBiome(Constants.BiomeType biome, RgbColor color) : base(color)
    {
        TBiome = biome;
    }
    
}