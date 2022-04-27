namespace Course_work.Map.Helpers;

public static class Constants
{
    public static RgbColor DeepWater = new RgbColor(0,60,255);
    public static RgbColor ShallowWater = new RgbColor(53,195,255);
    public static RgbColor Sand = new RgbColor(249,221,84);
    public static RgbColor Grass = new RgbColor(57,205,72);
    public static RgbColor Forest = new RgbColor(2,137,15);
    public static RgbColor Rock = new RgbColor(191,185,183);
    public static RgbColor Snow = new RgbColor(236,256,238);
    public static RgbColor Border = new RgbColor(0,0,0);
    public static RgbColor Coldest = new RgbColor(175,255,255);
    public static RgbColor Colder = new RgbColor(0,255,255);
    public static RgbColor Cold = new RgbColor(0,255,179);
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
    
    public enum TileGroupType
    {
        Water, 
        Land
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
    public const double HeightValShalWat = 0.54;
    public const double HeightValSand = 0.55;
    public const double HeightValGrass = 0.58;
    public const double HeightValForest = 0.67;
    public const double HeightValRock = 0.7;

}