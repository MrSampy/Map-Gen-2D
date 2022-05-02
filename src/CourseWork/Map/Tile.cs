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

    private bool IsEqualBiome(Tile tile) => (tile != null && tile._Biome.TBiome == _Biome.TBiome);
    public void UpdateBitmask()
        {   
            int counter = 0;
            if (IsEqualBiome(TopTile))
                counter += 1;
            if (IsEqualBiome(RightTile))
                counter += 2;
            if (IsEqualBiome(BottomTile))
                counter += 4;
            if (IsEqualBiome(LeftTile))
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
            bool IsNextTile(Tile tile) => (tile != null && tile.HeightValue > tile.HeightValue && tile.X != skipble.X && tile.Y != skipble.Y );
            Tile temptile = new Tile(-1,-1,10);
            if (IsNextTile(LeftTile))
                temptile = LeftTile;    
            if (IsNextTile(RightTile))
                temptile = RightTile;
            if (IsNextTile(TopTile))
                temptile = TopTile;
            if (IsNextTile(BottomTile))
                temptile = BottomTile;
            return temptile;
        }


}