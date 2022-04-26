using Course_work.Map.Helpers;
using Course_work.Map.Helpers.Storages;
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
        
        if (heightvalue < new MagicNumbers().HeightValDeepWat)
        {
            _Biome = new TilesBiomes(Enums.Biome.DEEPWATER, new Colors().DeepWater);
            Heat = new TilesHeat(Enums.HeatType.COLD, new Colors().Cold);
        }
        else if (heightvalue <= new MagicNumbers().HeightValShalWat)
        {
            _Biome = new TilesBiomes(Enums.Biome.SHALLOWWATER, new Colors().ShallowWater);
            Heat = new TilesHeat(Enums.HeatType.WARM, new Colors().Warm);
        }
        else if (heightvalue <= new MagicNumbers().HeightValSand)
        {
            _Biome = new TilesBiomes(Enums.Biome.SAND, new Colors().Sand);
            Heat = new TilesHeat(Enums.HeatType.WARMER, new Colors().Warmer);
        }
        else if (heightvalue <= new MagicNumbers().HeightValGrass)
        {
            _Biome = new TilesBiomes(Enums.Biome.GRASS, new Colors().Grass);
            Heat = new TilesHeat(Enums.HeatType.WAMEST, new Colors().Warmest);
        }
        else if (heightvalue <= new MagicNumbers().HeightValForest)
        {
            _Biome = new TilesBiomes(Enums.Biome.FOREST, new Colors().Forest);
            Heat = new TilesHeat(Enums.HeatType.WARM, new Colors().Warm);
        }
        else if (heightvalue <= new MagicNumbers().HeightValRock)
        {
            _Biome = new TilesBiomes(Enums.Biome.ROCK, new Colors().Rock);
            Heat = new TilesHeat(Enums.HeatType.COLDER, new Colors().Colder);
        }
        else
        {
            _Biome = new TilesBiomes(Enums.Biome.SNOW, new Colors().Snow);
            Heat = new TilesHeat(Enums.HeatType.COLDEST, new Colors().Coldest);
        }

        Type = (_Biome.TBiome == Enums.Biome.DEEPWATER || _Biome.TBiome == Enums.Biome.SHALLOWWATER)
            ? (Enums.TileGroupType.Water)
            : (Enums.TileGroupType.Land);

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
               double shadFactor = 0.8;
                _Biome._Color.Blue = (int) (_Biome._Color.Blue * shadFactor);
                _Biome._Color.Red = (int) (_Biome._Color.Red * shadFactor);
                _Biome._Color.Green = (int) (_Biome._Color.Green * shadFactor);
                Heat._Color = new Colors().Border;
            }

        }
    
}