using CourseWork.MapGen.Helpers;

namespace CourseWork.MapGen;

public sealed class Tile
{
    public int X { get; }
    public int Y { get; }
    public TilesHeight? HeightInfo;
    public TilesHeat? HeatInfo;
    public TilesMoisture? MoistureInfo;
    public TilesBiome BiomeInfo;
    public readonly bool IsLand;
    public bool HasRiver;
    public Tile?[] Neighbours;
    public bool Structure;
    public readonly bool IsMountain;
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
        IsLand = HeightInfo!.NoiseNumber>=Constants.HeightValCoast;
        IsMountain = HeightInfo.NoiseNumber > Constants.HeightValDeepForest;
    }

    public void UpdateMoisture(double newMoisture)
    {
        MoistureInfo = new TilesMoisture(Constants.MoistureType.Driest, Constants.Driest);
        foreach (var elem in Constants.MoistureValues.Where(elem => newMoisture < elem.Key))
        {
            MoistureInfo = new TilesMoisture(elem.Value.Moisture, elem.Value.Color);
            break;
        }
        MoistureInfo.NoiseNumber = newMoisture;

    }

    public void UpdateHeat(double newHeat)
    {
        HeatInfo = new TilesHeat(Constants.HeatType.Warmest, Constants.Warmest);
        foreach (var elem in Constants.ValuesHeat.Where(elem => newHeat< elem.Key))
        {
            HeatInfo = new TilesHeat(elem.Value.Heat, elem.Value.Color);
            break;
        }
        HeatInfo.NoiseNumber = newHeat; 


    }
    private void UpdateHeight(double newBiome)
    {
        HeightInfo = new TilesHeight(Constants.Biomes.Snow, Constants.Snow);
        foreach (var elem in Constants.HeightValues.Where(elem => newBiome < elem.Key))
        {
            HeightInfo = new TilesHeight(elem.Value.Height, elem.Value.Color);
            break;
        }

        HeightInfo.NoiseNumber = newBiome;
    }
    
    private delegate bool IsEqual(Tile? tile);
    private bool IsEqualTile(Tile? tile, IsEqual func) => tile != null && func(tile);

    public void UpdateBitmask()
    {
       var heightIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => 
           acc && IsEqualTile(neighbour,tile => tile!.HeightInfo!.Height == HeightInfo!.Height));
       
       var heatIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => 
           acc && IsEqualTile(neighbour,tile => tile!.HeatInfo!.Heat == HeatInfo!.Heat));
       
       var moistureIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => 
           acc && IsEqualTile(neighbour,tile => tile!.MoistureInfo!.Moisture == MoistureInfo!.Moisture));
      
       var biomeIsBorder = !Neighbours.Aggregate(true, (acc, neighbour) => 
           acc && IsEqualTile(neighbour,tile => tile!.BiomeInfo.Biome == BiomeInfo.Biome));
      
       const double shadFactor = 0.8;
       if(heatIsBorder)
          HeatInfo!.Darkish(0);
       if(moistureIsBorder)
           MoistureInfo!.Darkish(0);
       if(biomeIsBorder)
           BiomeInfo.Darkish(shadFactor);
       if (heightIsBorder)
           HeightInfo!.Darkish(shadFactor);
    }

    public Tile GetNextPixRiver(Tile skipped)
    {
        bool IsNextTile(Tile? tile1, Tile? tile2) =>
            tile1 != null && skipped != tile1 && tile1.HeightInfo!.NoiseNumber < tile2!.HeightInfo!.NoiseNumber;

        Tile tempTile = new Tile(-1, -1,new double[]{10,10,10});
        tempTile = Neighbours.Aggregate(tempTile, (acc, neighbour) => (IsNextTile(neighbour, acc) ? neighbour : acc)!);
        return tempTile;
    }
}