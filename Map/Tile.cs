using Course_work.Map.Helpers;
namespace Course_work.Map;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public double HeightValue { get; set; }
    public TilesBiomes _Biome;
    public Tile? LeftTile;
    public Tile? RightTile;
    public Tile? TopTile;
    public Tile? BottomTile;
    public Tile(int x, int y, double heightvalue)
    {
        X = x;
        Y = y;
        HeightValue = heightvalue;
        if (heightvalue < 0.42)
            _Biome = new TilesBiomes(Enums.Biome.DEEPWATER,0,60,255);
        else if (heightvalue <= 0.52)
            _Biome = new TilesBiomes(Enums.Biome.SHALLOWWATER,53,195,255);
        else if (heightvalue <= 0.53)
            _Biome = new TilesBiomes(Enums.Biome.SAND,249,221,84);
        else if (heightvalue <= 0.59)
            _Biome = new TilesBiomes(Enums.Biome.GRASS,57,205,72);
        else if (heightvalue <= 0.64)
            _Biome = new TilesBiomes(Enums.Biome.FOREST,2,137,15);
        else if (heightvalue <= 0.69)
            _Biome = new TilesBiomes(Enums.Biome.ROCK,191,185,183);
        else
            _Biome = new TilesBiomes(Enums.Biome.SNOW,236,256,238);
        
    }   

}