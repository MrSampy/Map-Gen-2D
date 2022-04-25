using Course_work.Map.Helpers;
namespace Course_work.Map;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public double HeightValue { get; set; }
    public TilesBiomes _Biome;
    public TilesHeat Heat;
    public Enums.TileGroupType Type;
    public Tile? LeftTile, RightTile, TopTile, BottomTile;
    public int Bitmask;
    public bool IsBorder;

    public Tile(int x, int y, double heightvalue)
    {   X = x;
        Y = y;
        HeightValue = heightvalue;
        
        if (heightvalue < 0.48)
        {
            _Biome = new TilesBiomes(Enums.Biome.DEEPWATER, new Colors().DeepWater);
            Type = Enums.TileGroupType.Water;
            Heat = new TilesHeat(Enums.HeatType.COLD, new Colors().Cold);
        }
        else if (heightvalue <= 0.54)
        {
            _Biome = new TilesBiomes(Enums.Biome.SHALLOWWATER, new Colors().ShallowWater);
            Type = Enums.TileGroupType.Water;
            Heat = new TilesHeat(Enums.HeatType.WARM, new Colors().Warm);
        }
        else if (heightvalue <= 0.55)
        {
            _Biome = new TilesBiomes(Enums.Biome.SAND, new Colors().Sand);
            Type = Enums.TileGroupType.Land;
            Heat = new TilesHeat(Enums.HeatType.WARMER, new Colors().Warmer);
        }
        else if (heightvalue <= 0.58)
        {
            _Biome = new TilesBiomes(Enums.Biome.GRASS, new Colors().Grass);
            Type = Enums.TileGroupType.Land;
            Heat = new TilesHeat(Enums.HeatType.WAMEST, new Colors().Warmest);
        }
        else if (heightvalue <= 0.67)
        {
            _Biome = new TilesBiomes(Enums.Biome.FOREST, new Colors().Forest);
            Type = Enums.TileGroupType.Land;
            Heat = new TilesHeat(Enums.HeatType.WARM, new Colors().Warm);
        }
        else if (heightvalue <= 0.7)
        {
            _Biome = new TilesBiomes(Enums.Biome.ROCK, new Colors().Rock);
            Type = Enums.TileGroupType.Land;
            Heat = new TilesHeat(Enums.HeatType.COLDER, new Colors().Colder);
        }
        else
        {
            _Biome = new TilesBiomes(Enums.Biome.SNOW, new Colors().Snow);
            Type = Enums.TileGroupType.Land;
            Heat = new TilesHeat(Enums.HeatType.COLDEST, new Colors().Coldest);
        }

    }   
        public void UpdateBitmask()
        {
            int counter = 0;
            if (TopTile != null && TopTile._Biome.TBiome==_Biome.TBiome)
                counter += 1;
            if (RightTile != null && RightTile._Biome.TBiome==_Biome.TBiome)
                counter += 2;
            if (BottomTile != null && BottomTile._Biome.TBiome==_Biome.TBiome)
                counter += 4;
            if (LeftTile != null && LeftTile._Biome.TBiome==_Biome.TBiome)
                counter += 8;
            Bitmask = counter;
            IsBorder = (Bitmask != 15);
           if (IsBorder)
            {
                _Biome._Color.Blue = (int) (_Biome._Color.Blue * 0.8);
                _Biome._Color.Red = (int) (_Biome._Color.Red * 0.8);
                _Biome._Color.Green = (int) (_Biome._Color.Green * 0.8);
                Heat._Color = new Colors().Border;
            }

        }
        
}