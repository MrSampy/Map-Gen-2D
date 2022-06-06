using CourseWork.MapGen.Helpers;

namespace CourseWork.MapGen;

public sealed class Tile
{
    public int X { get; }
    public int Y { get; }
    public double HeightValue { get; }
    public double HeatValue { get; }
    public double MoistureValue { get; }
    public TilesBiome Biome;
    public readonly TilesHeat Heat;
    public TilesMoisture Moisture;
    public readonly bool IsLand;
    public Tile? LeftTile;
    public Tile? RightTile;
    public Tile? TopTile;
    public Tile? BottomTile;
    public bool HasRiver;
    public Tile?[] Neighbours;
    public bool Structure;

    public Tile(int x, int y, double[] heightValues)
    {
        X = x;
        Y = y;
        HasRiver = false;
        HeightValue = heightValues[0];
        HeatValue = heightValues[1];
        MoistureValue = heightValues[2];
        Structure = false;
        Neighbours = new Tile[4];
        Biome = new TilesBiome(Constants.Biomes.Snow, Constants.Snow);
        foreach (var elem in Constants.HeightValues.Where(elem => HeightValue < elem.Key))
        {
            Biome = new TilesBiome(elem.Value.TBiome, elem.Value.Color);
            break;
        }

        Heat = new TilesHeat(Constants.HeatType.Coldest, Constants.Coldest);
        foreach (var elem in Constants.HeatValues.Where(elem => HeatValue < elem.Key))
        {
            Heat = new TilesHeat(elem.Value.THeat, elem.Value.Color);
            break;
        }

        Moisture = new TilesMoisture(Constants.MoistureType.Wetter, Constants.Wetter);
        foreach (var elem in Constants.MoistureValues.Where(elem => MoistureValue < elem.Key))
        {
            Moisture = new TilesMoisture(elem.Value.TMoisture, elem.Value.Color);
            break;
        }

        var isDeepWater = Biome.TBiome != Constants.Biomes.DeepWater;
        var isShallWater = Biome.TBiome != Constants.Biomes.ShallowWater;
        IsLand = (isDeepWater && isShallWater);
    }

    private bool IsEqualBiome(Tile? tile) => (tile != null && tile.Biome.TBiome == Biome.TBiome);
    private bool IsEqualHeat(Tile? tile) => (tile != null && tile.Heat.THeat == Heat.THeat);
    private bool IsEqualMoisture(Tile? tile) => (tile != null && tile.Moisture.TMoisture == Moisture.TMoisture);


    public void UpdateBitmask()
    {
       bool isHeightBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualBiome(neighbour));
       bool isHeatBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualHeat(neighbour));
       bool isMoistureBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualMoisture(neighbour));
       if(isHeatBorder)
           Heat.Darkish(0);
       if(isMoistureBorder)
           Moisture.Darkish(0);
       if (isHeightBorder)
       {
           const double shadFactor = 0.8;
           Biome.Darkish(shadFactor);
       }

    }

    public Tile GetNextPixRiver(Tile skipped)
    {
        bool IsNextTile(Tile? tile1, Tile? tile2) =>
            (tile1 != null && skipped != tile1 && tile1.HeightValue < tile2.HeightValue);

        Tile tempTile = new Tile(-1, -1,new double[]{10,10,10});
        tempTile = Neighbours.Aggregate(tempTile, (acc, neighbour) => (IsNextTile(neighbour, acc) ? neighbour : acc));
        return tempTile;
    }
}