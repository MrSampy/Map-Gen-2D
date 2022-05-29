namespace CourseWork.Map.Helpers;

public static class Constants
{
    private static readonly RgbColor DeepWater = new RgbColor(0, 60, 255);
    public static readonly RgbColor ShallowWater = new RgbColor(53, 195, 255);
    private static readonly RgbColor Sand = new RgbColor(249, 221, 84);
    private static readonly RgbColor Grass = new RgbColor(57, 205, 72);
    private static readonly RgbColor Forest = new RgbColor(2, 137, 15);
    private static readonly RgbColor Rock = new RgbColor(191, 185, 183);
    public static readonly RgbColor Snow = new RgbColor(236, 256, 238);
    public static readonly RgbColor Coldest = new RgbColor(0, 255, 255);
    private static readonly RgbColor Colder = new RgbColor(175, 255, 255);
    private static readonly RgbColor Cold = new RgbColor(0, 255, 119);
    private static readonly RgbColor Warm = new RgbColor(255, 255, 54);
    private static readonly RgbColor Warmer = new RgbColor(255, 168, 54);
    private static readonly RgbColor Warmest = new RgbColor(252, 92, 60);
    private static readonly RgbColor Wettest = new RgbColor(0, 0, 134);
    public static readonly RgbColor Wetter = new RgbColor(0, 137, 255);
    public static readonly RgbColor Wet = new RgbColor(0, 222, 255);
    private static readonly RgbColor Dry = new RgbColor(137, 255, 0);
    private static readonly RgbColor Dryer = new RgbColor(255, 239, 0);
    private static readonly RgbColor Driest = new RgbColor(255, 119, 0);

    private static readonly RgbColor Strawberry = new RgbColor(255, 0, 0);
    private static readonly RgbColor Blueberry = new RgbColor(0, 43, 255);
    private static readonly RgbColor Iron = new RgbColor(255, 236, 156);
    private static readonly RgbColor Coal = new RgbColor(0, 0, 0);

    public static readonly RgbColor Wall = new RgbColor(58, 53, 53);
    public static readonly RgbColor Floor = new RgbColor(90, 53, 42);
    public static readonly RgbColor Road = new RgbColor(201, 180, 48);
    public static readonly RgbColor Bridge = new RgbColor(97, 79, 0);

    public enum Biomes
    {
        DeepWater,
        ShallowWater,
        Sand,
        Grass,
        Forest,
        Rock,
        Snow,
        River
    }

    public enum HeatType
    {
        Coldest,
        Colder,
        Cold,
        Warm,
        Warmer,
        Warmest
    }


    public enum MoistureType
    {
        Wettest,
        Wetter,
        Wet,
        Dry,
        Dryer,
        Driest
    }

    private const double HeightValDeepWat = 0.48;
    private const double HeightValShalWat = 0.53;
    private const double HeightValSand = 0.54;
    private const double HeightValGrass = 0.6;
    private const double HeightValForest = 0.66;
    private const double HeightValRock = 0.69;

    private const double HeatValColder1 = 0.45;
    private const double HeatValCold1 = 0.52;
    private const double HeatValWarm1 = 0.55;
    private const double HeatValWarmer1 = 0.57;
    private const double HeatValWarmest = 0.58;
    private const double HeatValWarmer2 = 0.61;
    private const double HeatValWarm2 = 0.63;
    private const double HeatValCold2 = 0.65;
    private const double HeatValColder2 = 0.68;

    private const double MoistureValWettest = 0.49;
    private const double MoistureValWetter = 0.51;
    private const double MoistureValWet1 = 0.55;
    private const double MoistureValDry1 = 0.56;
    private const double MoistureValDrier1 = 0.58;
    private const double MoistureValDriest = 0.59;
    private const double MoistureValDrier2 = 0.62;
    private const double MoistureValDry2 = 0.67;
    private const double MoistureValWet2 = 0.7;

    public const double MinRiverHeight = 0.62;
    public const double MaxStructureVal = 0.65;
    public const double MinStructureVal = 0.56;
    public const int RangeOfObj = 20;
    
    public static readonly Dictionary<double, TilesBiome> HeightVals = new()
    {
        {HeightValDeepWat, new TilesBiome(Biomes.DeepWater, DeepWater)},
        {HeightValShalWat, new TilesBiome(Biomes.ShallowWater, ShallowWater)},
        {HeightValSand, new TilesBiome(Biomes.Sand, Sand)},
        {HeightValGrass, new TilesBiome(Biomes.Grass, Grass)},
        {HeightValForest, new TilesBiome(Biomes.Forest, Forest)},
        {HeightValRock, new TilesBiome(Biomes.Rock, Rock)}
    };

    public static readonly Dictionary<RgbColor, Biomes> SmallObj = new()
    {
        {Strawberry, Biomes.Grass},
        {Blueberry, Biomes.Grass},
        {Iron, Biomes.Rock},
        {Coal, Biomes.Rock}
    };

    public static readonly Dictionary<double, TilesMoisture> MoistureVals = new()
    {
        {MoistureValWettest, new TilesMoisture(MoistureType.Wettest, Wettest)},
        {MoistureValWetter, new TilesMoisture(MoistureType.Wetter, Wetter)},
        {MoistureValWet1, new TilesMoisture(MoistureType.Wet, Wet)},
        {MoistureValDry1, new TilesMoisture(MoistureType.Dry, Dry)},
        {MoistureValDrier1, new TilesMoisture(MoistureType.Dryer, Dryer)},
        {MoistureValDriest, new TilesMoisture(MoistureType.Driest, Driest)},
        {MoistureValDrier2, new TilesMoisture(MoistureType.Dryer, Dryer)},
        {MoistureValDry2, new TilesMoisture(MoistureType.Dry, Dry)},
        {MoistureValWet2, new TilesMoisture(MoistureType.Wet, Wet)}
    };

    public static readonly Dictionary<double, TilesHeat> HeatVals = new()
    {
        {HeatValColder1, new TilesHeat(HeatType.Colder, Colder)},
        {HeatValCold1, new TilesHeat(HeatType.Cold, Cold)},
        {HeatValWarm1, new TilesHeat(HeatType.Warm, Warm)},
        {HeatValWarmer1, new TilesHeat(HeatType.Warmer, Warmer)},
        {HeatValWarmest, new TilesHeat(HeatType.Warmest, Warmest)},
        {HeatValWarmer2, new TilesHeat(HeatType.Warmer, Warmer)},
        {HeatValWarm2, new TilesHeat(HeatType.Warm, Warm)},
        {HeatValCold2, new TilesHeat(HeatType.Cold, Cold)},
        {HeatValColder2, new TilesHeat(HeatType.Colder, Colder)},
    };
}