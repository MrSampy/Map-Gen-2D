namespace CourseWork.MapGen;

public sealed class Map
{
    public int Width { get; }
    public int Height { get; }
    public int LenOfPixel { get; }
    public readonly Tile[,] Tiles;

    public Map(int width, int height, int lenOfPix)
    {
        Width = width;
        Height = height;
        LenOfPixel = lenOfPix;
        Tiles = new Tile[Width, Height];
    }



}