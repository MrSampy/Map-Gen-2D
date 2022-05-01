using CourseWork.Map.Helpers;

namespace CourseWork.Map;

public class MapBuilder
{   
    
    private static Random Rnd = Random.Shared;
    private int Seed { get; }
    public int Width { get; }
    public int Height { get; }
    public int MaxRiverCount { get; }
    public Tile[,] Tiles;
    public List<River> Rivers = new List<River>();
    
    public MapBuilder(int width, int height)
    {   
        
        Width = width; 
        Height = height;
        MaxRiverCount = Rnd.Next(0,3);
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
                    (perlinNoise.Noise(2 * x * widthDivisor, 2 * y * heightDivisor, -0.5) + 1) / 2 * 0.8 +
                    (perlinNoise.Noise(4 * x * widthDivisor, 4 * y * heightDivisor, 0) + 1) / 2 * 0.2 +
                    (perlinNoise.Noise(8 * x * widthDivisor, 8 * y * heightDivisor, 0.5) + 1) / 2 * 0.1;
                
                tempheight = Math.Min(1, Math.Max(0, tempheight));
                tempheight = Math.Round(tempheight,3);
                Tiles[x, y] = new Tile(x, y, tempheight);
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

    private void FindPath(Tile starttyle, Tile lasttyle, ref River river)
    {
        
        
    }

    private void GenerateRivers()
    {
         List<Tile> r = new List<Tile>();
         
        int riverCount = MaxRiverCount;
        while (riverCount != 0)
        {
            int x = Rnd.Next(0, Width - 1), y = Rnd.Next(0, Height - 1);
            if(!Tiles[x,y].IsLand || Tiles[x,y].HeightValue<Constants.MinRiverHeight)
                continue;
            River river= new River();
            FindPath(Tiles[x,y], null, ref river);
            


        }

        


    }
}