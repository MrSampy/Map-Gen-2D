using Course_work.Map.Helpers;
namespace Course_work.Map;

public class MapGenerator
{
    private Random Rnd = new Random();
    private int Seed { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Tile[,] Tiles;
    public int NumOfGeoZones = 11;
    public int LengthofGeoZone { get; set; }
    public MapGenerator(int width, int height)
    {   
        Width = width; 
        Height = height;
        LengthofGeoZone = (int)(Width/NumOfGeoZones);
        Tiles = new Tile[Width,Height];
        Seed = Rnd.Next(1,1000);
        PerlinNoise perlinNoise = new PerlinNoise(Seed);
        double widthDivisor = 1 / (double)Width;
        double heightDivisor = 1 / (double)Height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                double tempheight =
                    (perlinNoise.Noise(2 * x* widthDivisor, 2 * y * heightDivisor, -0.5) + 1) / 2 * 0.8 +
                    (perlinNoise.Noise(4 * x * widthDivisor, 4 * y * heightDivisor, 0) + 1) / 2 * 0.2 +
                    (perlinNoise.Noise(8 * x * widthDivisor, 8 * y * heightDivisor, +0.5) + 1) / 2 * 0.1;
                tempheight = Math.Min(1, Math.Max(0, tempheight));
                tempheight = Math.Round(tempheight,3);
                Tiles[x, y] = new Tile(x,y,tempheight);
            }
        }
        FindNeighbours();
        UpdateBitmasks();
    }

    private void FindNeighbours()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {   
                Tiles[x, y].LeftTile = (y-1<0) ? null : Tiles[x, y-1];
                Tiles[x, y].RightTile = (y + 1 >= Height) ? null : Tiles[x, y+1];
                Tiles[x, y].TopTile = (x-1<0) ? null : Tiles[x-1, y];
                Tiles[x, y].BottomTile = (x + 1 >= Width) ? null : Tiles[x+1,y];
            }
        }
    }
    
    private void UpdateBitmasks()
    {
        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
                Tiles[x,y].UpdateBitmask();
    }
}