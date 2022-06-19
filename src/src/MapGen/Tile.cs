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
    private readonly bool _isMountain;
    public Tile(int x, int y, IReadOnlyList<double> heightNumbers)
    {
        X = x;
        Y = y;
        HasRiver = false;
        Structure = false;
        Neighbours = new Tile[4];
        UpdateHeight(heightNumbers[0]);
        UpdateHeat(heightNumbers[1]);
        UpdateMoisture(heightNumbers[2]);
        IsLand = HeightInfo!.NoiseNumber>=Constants.HeightValCoast;
        _isMountain = HeightInfo.NoiseNumber > Constants.HeightValDeepForest;
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
    public void UpdateBiome()
    {
        Constants.BiomeType biome;
        var isCoast = !IsLand || HeightInfo!.Height == Constants.Biomes.Sand;
        if (!isCoast && !_isMountain )
        {
            var x1 = (int) MoistureInfo!.Moisture;
            var y1 = (int) HeatInfo!.Heat; 
            biome = Constants.BiomeTable[x1, y1];
        }
        else
        {
            var numOfBiome = (int) HeightInfo!.Height;
            biome = (Constants.BiomeType) numOfBiome;
        }
        BiomeInfo = new TilesBiome(biome, Constants.BiomesUpdate[biome]);
    }
    private bool IsEqualTile(Tile? tile, Func<Tile, bool> func) => tile != null && func(tile);

    public void UpdateBitmask()
    {
        var functions = new Func<Tile, bool>[]
        {
            tile => tile.HeightInfo!.Height == HeightInfo!.Height,
            tile => tile.HeatInfo!.Heat == HeatInfo!.Heat,
            tile => tile.MoistureInfo!.Moisture == MoistureInfo!.Moisture,
            tile => tile.BiomeInfo.Biome == BiomeInfo.Biome
        };
        
        var settings = functions.Select(t => 
            !Neighbours.Aggregate(true, (acc, neighbour) => acc && IsEqualTile(neighbour, t))).ToList();

        const double shadFactor = 0.8;
        if (settings[0])
            HeightInfo!.Darkish(shadFactor);
        if(settings[1])
            HeatInfo!.Darkish(0);
        if(settings[2])
            MoistureInfo!.Darkish(0);
        if(settings[3])
            BiomeInfo.Darkish(shadFactor);
    }

    public Tile GetNextPixRiver(Tile skipped)
    {
        bool IsNextTile(Tile? tile1, Tile? tile2) =>
            tile1 != null && skipped != tile1 && tile1.HeightInfo!.NoiseNumber < tile2!.HeightInfo!.NoiseNumber;

        var tempTile = new Tile(-1, -1,new double[]{10,10,10});
        tempTile = Neighbours.Aggregate(tempTile, (acc, neighbour) => (IsNextTile(neighbour, acc) ? neighbour : acc)!);
        return tempTile;
    }
}