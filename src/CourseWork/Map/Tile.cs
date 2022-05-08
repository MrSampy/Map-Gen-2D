using CourseWork.Map.Helpers;

namespace CourseWork.Map;

public class Tile
{
    public int X { get; }
    public int Y { get; }
    public double HeightValue { get; }
    public TilesBiome Biome;
    public readonly TilesHeat Heat;
    public readonly bool IsLand;
    public Tile? LeftTile, RightTile, TopTile, BottomTile;
    public bool IsBorder, HasRiver;
    public Tile?[] Neighbours;

    public Tile(int x, int y, double heightvalue)
    {
        X = x;
        Y = y;
        HasRiver = false;
        HeightValue = heightvalue;
        Biome = new TilesBiome(Constants.Biomes.Snow, Constants.Snow);
        foreach (var elem in Constants.HeightVals)
        {
            if (heightvalue < elem.Key)
            {
                Biome = new TilesBiome(elem.Value.TBiome, elem.Value._Color);
                break;
            }
        }

        Heat = new TilesHeat(Constants.HeatType.Coldest, Constants.Coldest);
        foreach (var elem in Constants.HeatVals)
        {
            if (heightvalue < elem.Key)
            {
                Heat = new TilesHeat(elem.Value.THeat, elem.Value._Color);
                break;
            }
        }

        IsLand = (Biome.TBiome != Constants.Biomes.DeepWater && Biome.TBiome != Constants.Biomes.ShallowWater);
    }

    private bool IsEqualBiome(Tile tile) => (tile != null && tile.Biome.TBiome == Biome.TBiome);

    public void UpdateBitmask()
    {
        IsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualBiome(neighbour));
        if (IsBorder)
        {
            double shadFactor = 0.8;
            Biome.Darkify(shadFactor);
            if (Biome.TBiome != Constants.Biomes.River)
                Heat.Darkify(0);
        }
    }

    public Tile GetNextPixRiver(Tile skippble)
    {
        bool IsNextTile(Tile tile1, Tile tile2) =>
            (tile1 != null && skippble != tile1 && tile1.HeightValue < tile2.HeightValue);

        Tile temptile = new Tile(-1, -1, 10);
        temptile = Neighbours.Aggregate(temptile, (acc, neighbour) => (IsNextTile(neighbour, acc) ? neighbour : acc));
        return temptile;
    }
}