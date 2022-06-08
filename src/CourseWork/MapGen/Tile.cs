using CourseWork.MapGen.Helpers;

namespace CourseWork.MapGen;

public sealed class Tile
{
    public int X { get; }
    public int Y { get; }
    public double HeightValue { get; set; }
    public TilesBiome? Biome;
    public TilesHeat? Heat;
    public TilesMoisture? Moisture;
    public readonly bool IsLand;
    public Tile? LeftTile;
    public Tile? RightTile;
    public Tile? TopTile;
    public Tile? BottomTile;
    public bool HasRiver;
    public Tile?[] Neighbours;
    public bool Structure;
    public Tile(int x, int y, IReadOnlyList<double> heightValues)
    {
        X = x;
        Y = y;
        HasRiver = false;
        Structure = false;
        Neighbours = new Tile[4];
        UpdateHeight(heightValues[0]);
        UpdateHeat(heightValues[1]);
        UpdateMoisture(heightValues[2]);
        IsLand = HeightValue>=Constants.HeightValCoast;
    }

    public void UpdateMoisture(double newMoisture)
    {
        Moisture = new TilesMoisture(Constants.MoistureType.Driest, Constants.Driest);
        foreach (var elem in Constants.MoistureValues.Where(elem => newMoisture < elem.Key))
        {
            Moisture = new TilesMoisture(elem.Value.TMoisture, elem.Value.Color);
            break;
        }
        Moisture.MoistureValue = newMoisture;

    }

    public void UpdateHeat(double newHeat)
    {
        Heat = new TilesHeat(Constants.HeatType.Warmest, Constants.Warmest);
        foreach (var elem in Constants.HeatValues.Where(elem => newHeat< elem.Key))
        {
            Heat = new TilesHeat(elem.Value.THeat, elem.Value.Color);
            break;
        }
        Heat.HeatValue = newHeat; 


    }
    public void UpdateHeight(double newBiome)
    {
        Biome = new TilesBiome(Constants.Biomes.Snow, Constants.Snow);
        foreach (var elem in Constants.HeightValues.Where(elem => newBiome < elem.Key))
        {
            Biome = new TilesBiome(elem.Value.TBiome, elem.Value.Color);
            break;
        }

        HeightValue = newBiome;
    }
    public void UpdateBiome()
    {
        var isMountain = HeightValue>=Constants.HeightValDeepForest;
        var isValid = isMountain || !IsLand;
        if(isValid) return;
        var isColdest = Heat!.THeat is Constants.HeatType.Coldest or Constants.HeatType.Colder;
        var isWettest = Moisture!.TMoisture is Constants.MoistureType.Wettest or Constants.MoistureType.Wetter;
        var isDriest = Moisture.TMoisture is Constants.MoistureType.Driest or Constants.MoistureType.Dryer;
        var isHot = Heat.THeat is Constants.HeatType.Warmest or Constants.HeatType.Warmer;
        var rnd = new Random();
        if (isColdest && isWettest)
        {
            var color = rnd.Next(10) == 0 ? Constants.SwampTree : Constants.Swamp;
            Biome = new TilesBiome(Constants.Biomes.Swamp, color);
        }
        else if (isDriest && isHot)
        {
            var color = rnd.Next(10) == 0 ? Constants.Cactus : Constants.Sand;
            Biome = new TilesBiome(Constants.Biomes.Sand, color);
        }

    }

    private bool IsEqualBiome(Tile? tile) => (tile != null && tile.Biome!.TBiome == Biome!.TBiome);
    private bool IsEqualHeat(Tile? tile) => (tile != null && tile.Heat!.THeat == Heat!.THeat);
    private bool IsEqualMoisture(Tile? tile) => (tile != null && tile.Moisture!.TMoisture == Moisture!.TMoisture);


    public void UpdateBitmask()
    {
       var biomeIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualBiome(neighbour));
       var heatIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualHeat(neighbour));
       var moistureIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualMoisture(neighbour));
       if(heatIsBorder)
          Heat!.Darkish(0);
       if(moistureIsBorder)
           Moisture!.Darkish(0);
       if (!biomeIsBorder) return;
       const double shadFactor = 0.8;
       Biome!.Darkish(shadFactor);
    }

    public Tile GetNextPixRiver(Tile skipped)
    {
        bool IsNextTile(Tile? tile1, Tile? tile2) =>
            (tile1 != null && skipped != tile1 && tile1.HeightValue < tile2!.HeightValue);

        Tile tempTile = new Tile(-1, -1,new double[]{10,10,10});
        tempTile = Neighbours.Aggregate(tempTile, (acc, neighbour) => (IsNextTile(neighbour, acc) ? neighbour : acc)!);
        return tempTile;
    }
}