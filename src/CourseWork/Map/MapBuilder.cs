using CourseWork.Map.Helpers;

namespace CourseWork.Map;

public class MapBuilder
{
    private static Random Rnd = Random.Shared;
    private int Seed { get; }
    public int Width { get; }
    public int Height { get; }
    public Tile[,] Tiles;
    private RiversInfo Rivers;
    public int LenofPixel { get; }

    public MapBuilder(int width, int height, int lenpfpix)
    {
        Width = width;
        Height = height;
        LenofPixel = lenpfpix;
        Rivers = new RiversInfo(width, height);
        Tiles = new Tile[Width, Height];
        Seed = Rnd.Next(1, 1000);

        PerlinNoise perlinNoise = new PerlinNoise(Seed, Width, Height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile(x, y, perlinNoise.MakeNumber(x, y));
            }
        }

        FindNeighbours();
        GenerateRivers();
        UpdateBitmasks();
    }

    private void FindNeighbours()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Tiles[x, y].LeftTile = (y - 1 < 0) ? null : Tiles[x, y - 1];
                Tiles[x, y].RightTile = (y + 1 >= Height) ? null : Tiles[x, y + 1];
                Tiles[x, y].TopTile = (x - 1 < 0) ? null : Tiles[x - 1, y];
                Tiles[x, y].BottomTile = (x + 1 >= Width) ? null : Tiles[x + 1, y];
                Tiles[x, y].Neighbours = new Tile?[4]
                    {Tiles[x, y].LeftTile, Tiles[x, y].RightTile, Tiles[x, y].TopTile, Tiles[x, y].BottomTile};
            }
        }
    }

    private void UpdateBitmasks()
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            Tiles[x, y].UpdateBitmask();
    }

    private void FindPath(Tile starttile, ref List<Tile> river)
    {
        ref Tile temptile = ref Tiles[starttile.X, starttile.Y];
        if (river.Contains(temptile))
        {
            return;
        }
        if (temptile.IsLand && !temptile.HasRiver)
        {
            river.Add(temptile);
            Tile nexttile = temptile.GetNextPixRiver(Convert.ToBoolean(river.Count) ? river.Last() : temptile);
            FindPath(nexttile, ref river);
        }
        
    }

    private void GenerateRivers()
    {
        Console.WriteLine($"{Rivers.MaxRiverWidth}");
        int riverCount = Rivers.MaxRiverCount;
        while (riverCount != 0)
        {
            int x = Rnd.Next(0, Width - 1);
            int y = Rnd.Next(0, Height - 1);
            if (Tiles[x, y].HeightValue < Constants.MinRiverHeight)
                continue;
            List<Tile> river = new List<Tile>();
            FindPath(Tiles[x, y], ref river);
            bool isSand = river.Last().Biome.TBiome != Constants.Biomes.Sand;
            if (isSand)
                continue;
            
            foreach (var riv in river)
                Tiles[riv.X, riv.Y].HasRiver = true;
            
            bool isXChange = true;
            for (int i = 0; i < river.Count; ++i)
            {
                isXChange = (river[i]!=river.Last())?river[i + 1].Y != river[i].Y:isXChange;
                for (int j = 1; j <= Rivers.MaxRiverWidth; j++)
                {
                    if (isXChange)
                    {
                        if (river[i].X + j < Width)
                            Tiles[river[i].X + j, river[i].Y].HasRiver = true;
                        if (river[i].X - j >= 0)
                            Tiles[river[i].X - j, river[i].Y].HasRiver = true;
                    }
                    else
                    {
                        if (river[i].Y + j < Height)
                            Tiles[river[i].X, river[i].Y + j].HasRiver = true;
                        if (river[i].Y - j >= 0)
                            Tiles[river[i].X, river[i].Y - j].HasRiver = true;
                    }
                }
                
            }
            
            --riverCount;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (Tiles[x, y].HasRiver)
                {
                    Tiles[x, y].Biome = new TilesBiome(Constants.Biomes.ShallowWater, Constants.ShallowWater);
                    int newMois = (int)Tiles[x, y].Moisture.TMoisture - 1; 
                    foreach (var elem in Constants.MoistureVals)
                    {
                        if ((int)elem.Value.TMoisture == newMois)
                            Tiles[x, y].Moisture = new TilesMoisture(elem.Value.TMoisture,elem.Value._Color);
                    }
                }
            }
        }

    }
}