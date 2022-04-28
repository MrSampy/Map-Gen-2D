using Course_work.Map.Helpers;
namespace Course_work.Map;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public double HeightValue { get; set; }
    public readonly TilesBiome _Biome;
    public TilesHeat Heat;
    public Constants.TileGroupType Type;
    public Tile? LeftTile, RightTile, TopTile, BottomTile;
    public bool IsBorder;
    public int Bitmask;
 
    public Tile(int x, int y, double heightvalue)
    {   X = x;
        Y = y;
        HeightValue = heightvalue;
        
        if (heightvalue < Constants.HeightValDeepWat)
            _Biome = new TilesBiome(Constants.Biome.DeepWater, Constants.DeepWater);
        else if (heightvalue <= Constants.HeightValShalWat)
            _Biome = new TilesBiome(Constants.Biome.ShallowWater, Constants.ShallowWater);
        else if (heightvalue <= Constants.HeightValSand)
            _Biome = new TilesBiome(Constants.Biome.Sand, Constants.Sand);
        else if (heightvalue <= Constants.HeightValGrass)
            _Biome = new TilesBiome(Constants.Biome.Grass, Constants.Grass);
        else if (heightvalue <= Constants.HeightValForest)
            _Biome = new TilesBiome(Constants.Biome.Forest, Constants.Forest);
        else if (heightvalue <= Constants.HeightValRock)
            _Biome = new TilesBiome(Constants.Biome.Rock, Constants.Rock);
        else
            _Biome = new TilesBiome(Constants.Biome.Snow, Constants.Snow);
        
        if(heightvalue < Constants.HeatValColder1)
            Heat = new TilesHeat(Constants.HeatType.Colder, Constants.Colder);
        else if(heightvalue <= Constants.HeatValCold1)
            Heat = new TilesHeat(Constants.HeatType.Cold, Constants.Cold);
        else if(heightvalue <= Constants.HeatValWarm1)
            Heat = new TilesHeat(Constants.HeatType.Warm, Constants.Warm);
        else if(heightvalue <= Constants.HeatValWarmer1)
            Heat = new TilesHeat(Constants.HeatType.Warmer, Constants.Warmer);
        else if(heightvalue <= Constants.HeatValWarmest)
            Heat = new TilesHeat(Constants.HeatType.Warmest, Constants.Warmest);
        else if(heightvalue <= Constants.HeatValWarmer2)
            Heat = new TilesHeat(Constants.HeatType.Warmer, Constants.Warmer);
        else if(heightvalue <= Constants.HeatValWarm2)
            Heat = new TilesHeat(Constants.HeatType.Warm, Constants.Warm);
        else if(heightvalue <= Constants.HeatValCold2)
            Heat = new TilesHeat(Constants.HeatType.Cold, Constants.Cold);
        else if(heightvalue < Constants.HeatValColder2)
            Heat = new TilesHeat(Constants.HeatType.Colder, Constants.Colder);
        else
            Heat = new TilesHeat(Constants.HeatType.Coldest, Constants.Coldest);

        Type = (_Biome.TBiome == Constants.Biome.DeepWater || _Biome.TBiome == Constants.Biome.ShallowWater)
            ? (Constants.TileGroupType.Water)
            : (Constants.TileGroupType.Land);

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
            IsBorder = (counter != 15);
            
           if (IsBorder)
           {
               double shadFactor = 0.8;
                _Biome.Darkify(shadFactor);
                Heat.Darkify(0);
           }

        }
        

}