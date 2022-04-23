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
    public int Bitmask;
    public Tile(int x, int y, double heightvalue)
    {
        X = x;
        Y = y;
        HeightValue = heightvalue;
 
        if (heightvalue < 0.48)
            _Biome = new TilesBiomes(Enums.Biome.DEEPWATER,0,60,255);
        else if (heightvalue <= 0.54)
            _Biome = new TilesBiomes(Enums.Biome.SHALLOWWATER,53,195,255);
        else if (heightvalue <= 0.55)
            _Biome = new TilesBiomes(Enums.Biome.SAND,249,221,84);
        else if (heightvalue <= 0.58)
            _Biome = new TilesBiomes(Enums.Biome.GRASS,57,205,72);
        else if (heightvalue <= 0.67)
            _Biome = new TilesBiomes(Enums.Biome.FOREST,2,137,15);
        else if (heightvalue <= 0.7)
            _Biome = new TilesBiomes(Enums.Biome.ROCK,191,185,183);
        else
            _Biome = new TilesBiomes(Enums.Biome.SNOW,236,256,238);
        
    }   
        public void UpdateBitmask()
        {
            int count = 0;
            if (TopTile._Biome == _Biome)
                count += 1;
            else if (RightTile._Biome == _Biome)
                count += 2;
            else if (BottomTile._Biome == _Biome)
                count += 4;
            else if (LeftTile._Biome == _Biome)
                count += 8;
            Bitmask = count;
        }

}