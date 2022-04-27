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
        {
            _Biome = new TilesBiome(Constants.Biome.DeepWater, Constants.DeepWater);
            Heat = new TilesHeat(Constants.HeatType.Cold, Constants.Cold);
        }
        else if (heightvalue <= Constants.HeightValShalWat)
        {
            _Biome = new TilesBiome(Constants.Biome.ShallowWater, Constants.ShallowWater);
            Heat = new TilesHeat(Constants.HeatType.Warm, Constants.Warm);
        }
        else if (heightvalue <= Constants.HeightValSand)
        {
            _Biome = new TilesBiome(Constants.Biome.Sand, Constants.Sand);
            Heat = new TilesHeat(Constants.HeatType.Warmer, Constants.Warmer);
        }
        else if (heightvalue <= Constants.HeightValGrass)
        {
            _Biome = new TilesBiome(Constants.Biome.Grass, Constants.Grass);
            Heat = new TilesHeat(Constants.HeatType.Warmest, Constants.Warmest);
        }
        else if (heightvalue <= Constants.HeightValForest)
        {
            _Biome = new TilesBiome(Constants.Biome.Forest, Constants.Forest);
            Heat = new TilesHeat(Constants.HeatType.Warm, Constants.Warm);
        }
        else if (heightvalue <= Constants.HeightValRock)
        {
            _Biome = new TilesBiome(Constants.Biome.Rock, Constants.Rock);
            Heat = new TilesHeat(Constants.HeatType.Colder, Constants.Colder);
        }
        else
        {
            _Biome = new TilesBiome(Constants.Biome.Snow, Constants.Snow);
            Heat = new TilesHeat(Constants.HeatType.Coldest, Constants.Coldest);
        }

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
           }

        }

        public void UpdateHeatTile(Constants.HeatType type)
        {
            if (IsBorder)
            {   Heat.Darkify(0);
                return;
            }
            int currHeatype = (int)Heat.THeat, tempHeattype=(int)type;
            if (tempHeattype != currHeatype)
                tempHeattype = (tempHeattype > currHeatype) ? (tempHeattype - 1) : (tempHeattype + 1);
            
            Heat.THeat = (Constants.HeatType) tempHeattype;
            if (Heat.THeat == Constants.HeatType.Coldest)
                Heat._Color = Constants.Coldest;
            else if (Heat.THeat == Constants.HeatType.Colder)
                Heat._Color = Constants.Colder;
            else if (Heat.THeat == Constants.HeatType.Cold)
                Heat._Color = Constants.Cold;
            else if (Heat.THeat == Constants.HeatType.Warm)
                Heat._Color = Constants.Warm;
            else if (Heat.THeat == Constants.HeatType.Warmer)
                Heat._Color = Constants.Warmer;
            else if (Heat.THeat == Constants.HeatType.Warmest)
                Heat._Color = Constants.Warmest;
        }

}