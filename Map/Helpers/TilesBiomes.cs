using Course_work.Map.Helpers.Storages;
namespace Course_work.Map.Helpers;

public class TilesBiomes
{
    public Enums.Biome TBiome { get; set; }
    public Color _Color;
    public TilesBiomes(Enums.Biome bi, Color color)
    {
        TBiome = bi;
        _Color = color;
    }
}