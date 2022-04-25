using Course_work.Map.Helpers;
namespace Course_work.Map;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public double HeightValue { get; set; }
    public TilesBiomes _Biome;
    public Enums.TileGroupType Type;
    public Tile? LeftTile, RightTile, TopTile, BottomTile;
    public int Bitmask;
    public bool IsBorder;

    public Tile(int x, int y, double heightvalue)
    {   Colors color = new Colors();
        X = x;
        Y = y;
        HeightValue = heightvalue;
        
        if (heightvalue < 0.48)
        {
            _Biome = new TilesBiomes(Enums.Biome.DEEPWATER, color.DeepWater);
            Type = Enums.TileGroupType.Water;
        }
        else if (heightvalue <= 0.54)
        {
            _Biome = new TilesBiomes(Enums.Biome.SHALLOWWATER, color.ShallowWater);
            Type = Enums.TileGroupType.Water;
        }
        else if (heightvalue <= 0.55)
        {
            _Biome = new TilesBiomes(Enums.Biome.SAND, color.Sand);
            Type = Enums.TileGroupType.Land;
        }
        else if (heightvalue <= 0.58)
        {
            _Biome = new TilesBiomes(Enums.Biome.GRASS, color.Grass);
            Type = Enums.TileGroupType.Land;
        }
        else if (heightvalue <= 0.67)
        {
            _Biome = new TilesBiomes(Enums.Biome.FOREST, color.Forest);
            Type = Enums.TileGroupType.Land;
        }
        else if (heightvalue <= 0.7)
        {
            _Biome = new TilesBiomes(Enums.Biome.ROCK, color.Rock);
            Type = Enums.TileGroupType.Land;
        }
        else
        {
            _Biome = new TilesBiomes(Enums.Biome.SNOW, color.Snow);
            Type = Enums.TileGroupType.Land;
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

            }

        }
        
}