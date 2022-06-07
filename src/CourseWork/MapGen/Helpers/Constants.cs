namespace CourseWork.MapGen.Helpers;

public static class Constants
{ 
    private static readonly RgbColor DeepWater = new (0, 27, 115);
    private static readonly RgbColor Ocean = new (0, 60, 255);
    private static readonly RgbColor ShallowWater = new (0, 118, 169);
    public static readonly RgbColor Coast = new (53, 195, 255);
    public static readonly RgbColor Sand = new (249, 221, 84);
    private static readonly RgbColor Grass = new (57, 205, 72);
    private static readonly RgbColor Forest = new (2, 137, 15);
    private static readonly RgbColor DeepForest = new (17, 65, 22);
    private static readonly RgbColor Rock = new (191, 185, 183);
    private static readonly RgbColor HardRock = new (154, 148, 186);
    public static readonly RgbColor Snow = new (236, 256, 238);
    public static readonly RgbColor Swamp = new (101, 85, 63);

    
    public static readonly RgbColor Coldest = new(0, 255, 255);
    public static readonly RgbColor Colder = new (175, 255, 255);
    public static readonly RgbColor Cold = new (0, 255, 119);
    private static readonly RgbColor Warm = new (255, 255, 54);
    private static readonly RgbColor Warmer = new (255, 168, 54);
    public static readonly RgbColor Warmest = new (252, 92, 60);
    
    public static readonly RgbColor Wettest = new (0, 0, 134);
    public static readonly RgbColor Wetter = new (0, 137, 255);
    public static readonly RgbColor Wet = new (0, 222, 255);
    private static readonly RgbColor Dry = new (137, 255, 0);
    private static readonly RgbColor Dryer = new (255, 239, 0);
    public static readonly RgbColor Driest = new (255, 119, 0);

    private static readonly RgbColor Strawberry = new (255, 0, 0);
    private static readonly RgbColor Blueberry = new (0, 43, 255);
    private static readonly RgbColor Iron = new (255, 236, 156);
    private static readonly RgbColor Coal = new (0, 0, 0);
    private static readonly RgbColor PinkCoral = new (201, 71, 255);
    private static readonly RgbColor VioletCoral = new (83, 0, 155);
    public static readonly RgbColor SwampTree = new (60, 102, 62);
    public static readonly RgbColor Cactus = new (25, 189, 31);

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
        Swamp
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
    public const double HeightValCoast = 0.53;
    private const double HeightValSand = 0.54;
    private const double HeightValGrass = 0.59;
    private const double HeightValForest = 0.64;
    public const double HeightValDeepForest = 0.68;
    private const double HeightValRock = 0.69;
    private const double HeightValHardRock = 0.7;

    private const double HeatValColdest = 0.44;
    private const double HeatValColder = 0.48;
    private const double HeatValCold = 0.57;
    private const double HeatValWarm = 0.66;
    private const double HeatValWarmer = 0.69;
    
    private const double MoistureValWettest = 0.44;
    private const double MoistureValWetter = 0.48;
    private const double MoistureValWet = 0.57;
    private const double MoistureValDry = 0.66;
    private const double MoistureValDrier = 0.69;
    
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
        {Coal, Biomes.Rock},
        {PinkCoral,Biomes.Coast},
        {VioletCoral,Biomes.Coast}
    };
    
    public static readonly Dictionary<double, TilesMoisture> MoistureValues = new()
    {
        {MoistureValWettest, new TilesMoisture(MoistureType.Wettest, Wettest)},
        {MoistureValWetter, new TilesMoisture(MoistureType.Wetter, Wetter)},
        {MoistureValWet, new TilesMoisture(MoistureType.Wet, Wet)},
        {MoistureValDry, new TilesMoisture(MoistureType.Dry, Dry)},
        {MoistureValDrier, new TilesMoisture(MoistureType.Dryer, Dryer)}
    };
    
    public static readonly Dictionary<double, TilesHeat> HeatValues = new()
    {
        {HeatValColdest, new TilesHeat(HeatType.Coldest, Coldest)},
        {HeatValColder, new TilesHeat(HeatType.Colder, Colder)},
        {HeatValCold, new TilesHeat(HeatType.Cold, Cold)},
        {HeatValWarm, new TilesHeat(HeatType.Warm, Warm)},
        {HeatValWarmer, new TilesHeat(HeatType.Warmer, Warmer)}
    };

}