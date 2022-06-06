namespace CourseWork.MapGen.Helpers;

public static class Constants
{ 
    private static readonly RgbColor DeepWater = new (0, 27, 115);
    private static readonly RgbColor Ocean = new (0, 60, 255);
    public static readonly RgbColor ShallowWater = new (0, 118, 169);
    private static readonly RgbColor Coast = new (53, 195, 255);
    private static readonly RgbColor Sand = new (249, 221, 84);
    private static readonly RgbColor Grass = new (57, 205, 72);
    private static readonly RgbColor Forest = new (2, 137, 15);
    private static readonly RgbColor DeepForest = new (17, 65, 22);
    private static readonly RgbColor Rock = new (191, 185, 183);
    private static readonly RgbColor HardRock = new (154, 148, 186);
    public static readonly RgbColor Snow = new (236, 256, 238);
    
    public static readonly RgbColor Coldest = new(0, 255, 255);
    private static readonly RgbColor Colder = new (175, 255, 255);
    private static readonly RgbColor Cold = new (0, 255, 119);
    private static readonly RgbColor Warm = new (255, 255, 54);
    private static readonly RgbColor Warmer = new (255, 168, 54);
    private static readonly RgbColor Warmest = new (252, 92, 60);
    
    private static readonly RgbColor Wettest = new (0, 0, 134);
    public static readonly RgbColor Wetter = new (0, 137, 255);
    public static readonly RgbColor Wet = new (0, 222, 255);
    private static readonly RgbColor Dry = new (137, 255, 0);
    private static readonly RgbColor Dryer = new (255, 239, 0);
    private static readonly RgbColor Driest = new (255, 119, 0);

    private static readonly RgbColor Strawberry = new (255, 0, 0);
    private static readonly RgbColor Blueberry = new (0, 43, 255);
    private static readonly RgbColor Iron = new (255, 236, 156);
    private static readonly RgbColor Coal = new (0, 0, 0);

    public static readonly RgbColor Wall = new (58, 53, 53);
    public static readonly RgbColor Floor = new (90, 53, 42);
    public static readonly RgbColor Road = new (201, 180, 48);
    public static readonly RgbColor Bridge = new (97, 79, 0);

    public enum Biomes
    {
        DeepWater,
        Ocean,
        ShallowWater,
        Coast,
        Sand,
        Grass,
        Forest,
        DeepForest,
        Rock,
        HardRock,
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

    private const double HeightValDeepWat = 0.42;
    private const double HeightValOcean = 0.46;
    private const double HeightValShallWat = 0.49;
    private const double HeightValCoast = 0.53;
    private const double HeightValSand = 0.54;
    private const double HeightValGrass = 0.59;
    private const double HeightValForest = 0.64;
    private const double HeightValDeepForest = 0.68;
    private const double HeightValRock = 0.69;
    private const double HeightValHardRock = 0.7;

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
    
    public static readonly Dictionary<double, TilesBiome> HeightValues = new()
    {
        {HeightValDeepWat, new TilesBiome(Biomes.DeepWater, DeepWater)},
        {HeightValOcean, new TilesBiome(Biomes.Ocean, Ocean)},
        {HeightValShallWat, new TilesBiome(Biomes.ShallowWater, ShallowWater)},
        {HeightValCoast, new TilesBiome(Biomes.Coast, Coast)},
        {HeightValSand, new TilesBiome(Biomes.Sand, Sand)},
        {HeightValGrass, new TilesBiome(Biomes.Grass, Grass)},
        {HeightValForest, new TilesBiome(Biomes.Forest, Forest)},
        {HeightValDeepForest, new TilesBiome(Biomes.DeepForest, DeepForest)},
        {HeightValRock, new TilesBiome(Biomes.Rock, Rock)},
        {HeightValHardRock, new TilesBiome(Biomes.HardRock, HardRock)}

    };

    public static readonly Dictionary<RgbColor, Biomes> SmallObj = new()
    {
        {Strawberry, Biomes.Grass},
        {Blueberry, Biomes.Grass},
        {Iron, Biomes.Rock},
        {Coal, Biomes.Rock}
    };

    public static readonly Dictionary<double, TilesMoisture> MoistureValues = new()
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

    public static readonly Dictionary<double, TilesHeat> HeatValues = new()
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