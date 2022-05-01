using CourseWork.Map.Helpers;

namespace CourseWork.Map;

public class Tile
{
    public int X { get; }
    public int Y { get; }
    public double HeightValue { get; }
    public readonly TilesBiome _Biome;
    public TilesHeat Heat;
    public bool IsLand;
    public Tile? LeftTile, RightTile, TopTile, BottomTile;
    public bool IsBorder, HasRiver;
    public int Bitmask;
    
    public Tile(int x, int y, double heightvalue)
    {
        X = x;
        Y = y;
        HasRiver = false;
        HeightValue = heightvalue;
        _Biome = new TilesBiome(Constants.Biome.Snow,Constants.Snow);
        foreach (var elem in Constants.HeightVals)
        {
            if (heightvalue < elem.Key)
            {
                _Biome = new TilesBiome(elem.Value.TBiome, elem.Value._Color);
                break;
            }
        }

        Heat = new TilesHeat(Constants.HeatType.Coldest,Constants.Coldest);
        foreach (var elem in Constants.HeatVals)
        {
            if (heightvalue < elem.Key)
            {
                Heat = new TilesHeat(elem.Value.THeat, elem.Value._Color);
                break;
            }
        }
        IsLand = (_Biome.TBiome != Constants.Biome.DeepWater && _Biome.TBiome != Constants.Biome.ShallowWater);

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

        public Tile GetNextPixRiver(Tile skipble)
        {
            Tile temptile = new Tile(-1,-1,10);
            if (LeftTile != null && temptile.HeightValue > LeftTile.HeightValue && LeftTile.X != skipble.X && LeftTile.Y != skipble.Y)
                temptile = LeftTile;    
            if (RightTile != null && temptile.HeightValue > RightTile.HeightValue && RightTile.X != skipble.X && RightTile.Y != skipble.Y)
                temptile = RightTile;
            if (TopTile != null && temptile.HeightValue > TopTile.HeightValue && TopTile.X != skipble.X && TopTile.Y != skipble.Y)
                temptile = TopTile;
            if (BottomTile != null && temptile.HeightValue > BottomTile.HeightValue && BottomTile.X != skipble.X && BottomTile.Y != skipble.Y)
                temptile = BottomTile;
            return temptile;
        }


}