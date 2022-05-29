using CourseWork.Map.Helpers;

namespace CourseWork.Map;

public class Tile
{
    public int X { get; }
    public int Y { get; }
    public double HeightValue { get; }
    public TilesBiome Biome;
    public readonly TilesHeat Heat;
    public TilesMoisture Moisture;
    public readonly bool IsLand;
    public Tile? LeftTile;
    public Tile? RightTile;
    public Tile? TopTile;
    public Tile? BottomTile;
    private bool _isBorder;
    public bool HasRiver;
    public Tile?[] Neighbours;
    public bool Structure;

    public Tile(int x, int y, double heightValue)
    {
        X = x;
        Y = y;
        HasRiver = false;
        HeightValue = heightValue;
        Structure = false;
        Neighbours = new Tile[4];
        Biome = new TilesBiome(Constants.Biomes.Snow, Constants.Snow);
        foreach (var elem in Constants.HeightVals)
        {
            if (heightValue < elem.Key)
            {
                Biome = new TilesBiome(elem.Value.TBiome, elem.Value.Color);
                break;
            }
        }

        Heat = new TilesHeat(Constants.HeatType.Coldest, Constants.Coldest);
        foreach (var elem in Constants.HeatVals)
        {
            if (heightValue < elem.Key)
            {
                Heat = new TilesHeat(elem.Value.THeat, elem.Value.Color);
                break;
            }
        }

        Moisture = new TilesMoisture(Constants.MoistureType.Wetter, Constants.Wetter);
        foreach (var elem in Constants.MoistureVals)
        {
            if (heightValue < elem.Key)
            {
                Moisture = new TilesMoisture(elem.Value.TMoisture, elem.Value.Color);
                break;
            }
        }

        bool isDeepWater = Biome.TBiome != Constants.Biomes.DeepWater;
        bool isShallWater = Biome.TBiome != Constants.Biomes.ShallowWater;
        IsLand = (isDeepWater && isShallWater);
    }

    private bool IsEqualBiome(Tile? tile) => (tile != null && tile.Biome.TBiome == Biome.TBiome);

    public void UpdateBitmask()
    {
        _isBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualBiome(neighbour));
        if (_isBorder)
        {
            const double shadFactor = 0.8;
            Biome.Darkify(shadFactor);
            if (Biome.TBiome != Constants.Biomes.River)
            {
                Moisture.Darkify(0);
                Heat.Darkify(0);
            }
        }
    }

    public Tile GetNextPixRiver(Tile skippble)
    {
        bool IsNextTile(Tile? tile1, Tile? tile2) =>
            (tile1 != null && skippble != tile1 && tile1.HeightValue < tile2.HeightValue);

        Tile tempTile = new Tile(-1, -1, 10);
        tempTile = Neighbours.Aggregate(tempTile, (acc, neighbour) => (IsNextTile(neighbour, acc) ? neighbour : acc));
        return tempTile;
    }
}