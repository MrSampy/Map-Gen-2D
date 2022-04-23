namespace Course_work.Map.Helpers;

public class TilesBiomes
{
    public Enums.Biome TBiome { get; set; }
    public int Red { get; set; }
    public int Green { get; set; }
    public int Blue { get; set; }

    public TilesBiomes(Enums.Biome bi, int red, int green, int blue)
    {
        TBiome = bi;
        Red = red;
        Blue = blue;
        Green = green;
    }
}