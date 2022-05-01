using System.Drawing;

namespace CourseWork.Map.Helpers;

public static class Constants
{
    public static RgbColor DeepWater = new RgbColor(0,60,255);
    public static RgbColor ShallowWater = new RgbColor(53,195,255);
    public static RgbColor Sand = new RgbColor(249,221,84);
    public static RgbColor Grass = new RgbColor(57,205,72);
    public static RgbColor Forest = new RgbColor(2,137,15);
    public static RgbColor Rock = new RgbColor(191,185,183);
    public static RgbColor Snow = new RgbColor(236,256,238);
    public static RgbColor Coldest = new RgbColor(0,255,255);
    public static RgbColor Colder = new RgbColor(175,255,255);
    public static RgbColor Cold = new RgbColor(0,255,119);
    public static RgbColor Warm = new RgbColor(255,255,54);
    public static RgbColor Warmer = new RgbColor(255,168,54);
    public static RgbColor Warmest = new RgbColor(252,92,60);
    public enum Biome
    {
        DeepWater,
        ShallowWater,
        Sand,
        Grass,
        Forest,
        Rock,
        Snow
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
    public const double HeightValDeepWat = 0.48;
    public const double HeightValShalWat = 0.53;
    public const double HeightValSand = 0.54;
    public const double HeightValGrass = 0.6;
    public const double HeightValForest = 0.66;
    public const double HeightValRock = 0.69;
    
    public const double HeatValColder1 = 0.45;
    public const double HeatValCold1 = 0.52;
    public const double HeatValWarm1 = 0.55;
    public const double HeatValWarmer1 = 0.57;
    public const double HeatValWarmest = 0.58;
    public const double HeatValWarmer2 = 0.61;
    public const double HeatValWarm2 = 0.63;
    public const double HeatValCold2 = 0.65;
    public const double HeatValColder2 = 0.68;

    public static readonly Dictionary<double, TilesBiome> HeightVals = new()
    {
        {HeightValDeepWat, new TilesBiome(Biome.DeepWater,DeepWater)},
        {HeightValShalWat, new TilesBiome(Biome.ShallowWater,ShallowWater)},
        {HeightValSand, new TilesBiome(Biome.Sand,Sand)},
        {HeightValGrass, new TilesBiome(Biome.Grass,Grass)},
        {HeightValForest, new TilesBiome(Biome.Forest,Forest)},
        {HeightValRock, new TilesBiome(Biome.Rock,Rock)}
        
    };
    
    public static readonly Dictionary<double, TilesHeat> HeatVals = new()
    {
        {HeatValColder1, new TilesHeat(HeatType.Colder,Colder)},
        {HeatValCold1, new TilesHeat(HeatType.Cold,Cold)},
        {HeatValWarm1, new TilesHeat(HeatType.Warm,Warm)},
        {HeatValWarmer1, new TilesHeat(HeatType.Warmer,Warmer)},
        {HeatValWarmest, new TilesHeat(HeatType.Warmest,Warmest)},
        {HeatValWarmer2, new TilesHeat(HeatType.Warmer,Warmer)},
        {HeatValWarm2, new TilesHeat(HeatType.Warm,Warm)},
        {HeatValCold2, new TilesHeat(HeatType.Cold,Cold)},
        {HeatValColder2, new TilesHeat(HeatType.Colder,Colder)},
    };
    public const double MinRiverHeight = HeightValSand;


}