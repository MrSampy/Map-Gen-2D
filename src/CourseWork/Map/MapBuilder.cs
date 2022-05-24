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
    private CastlesInfo Castles;
    public int LenofPixel { get; }

    public MapBuilder(int width, int height, int lenpfpix)
    {
        Width = width;
        Height = height;
        LenofPixel = lenpfpix;
        Rivers = new RiversInfo(width, height);
        Castles = new CastlesInfo(width,height);
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
        UpdateCastles();
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

    private void UpdateRivers()
    {
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
    private void FindPath(Tile startTile, ref List<Tile> river)
    {
        ref Tile tempTile = ref Tiles[startTile.X, startTile.Y];
        if (river.Contains(tempTile))
        {
            river.Clear();
            return;
        }

        if (tempTile.IsLand && !tempTile.HasRiver)
        {
            river.Add(tempTile);
            bool isClear = Convert.ToBoolean(river.Count);
            Tile nextTile = tempTile.GetNextPixRiver(isClear ? river.Last() : tempTile);
            if ((isClear || Rnd.Next(10) == 0) && river.Count>=2)
            {
                Tile newtTile = tempTile.Neighbours[Rnd.Next(3)];
                    bool isVal = newtTile != nextTile && newtTile != river[river.Count - 2];
                    if (newtTile != null && isVal)
                    {
                        nextTile = newtTile;
                    }

            }

            FindPath(nextTile, ref river);
        }
        
    }

    private void Extend(int riverLength, out List<int[]> extend)
    {   
        extend = new List<int[]>();
        List<int[]> border = new List<int[]>();
        double k1 = 1.9;
        double k2 = 2.5;
        border.Add(new []{Rivers.MaxRiverWidth,(int)Math.Ceiling(Rivers.MaxRiverWidth/k2)});
        for (int i = 1; i < 5; ++i)
        {
            int elem = (int)Math.Ceiling(border[i - 1][0]/k1);
            int elemMin = (int)Math.Ceiling(elem/k2);
            border.Add(new []{elem,elemMin});
        }

        List<int> arrBorders = new List<int>();
        int counter = 0;
        arrBorders.Add(Rnd.Next(1,riverLength/5));
        while (counter!=3)
        {
            int point = Rnd.Next(1,riverLength-2);
            if(arrBorders.Contains(point))
                continue; 
            arrBorders.Add(point);
            ++counter;
        }
        arrBorders.Sort();
        arrBorders.Add(0);
        for (int i = 0; i < 6; ++i)
        {
            if (i != 5)
                extend.Add(new[] {Rnd.Next(border[i][1], border[i][0])});
            else
                extend.Add(arrBorders.ToArray());
        }
    }

    private void GenerateRivers()
    {
        int riverCount = Rivers.MaxRiverCount;
        while (riverCount != 0)
        {
            int x = Rnd.Next(Rivers.MaxRiverWidth, Width - Rivers.MaxRiverWidth - 1);
            int y = Rnd.Next(Rivers.MaxRiverWidth, Height - Rivers.MaxRiverWidth - 1);
            if (Tiles[x, y].HeightValue < Constants.MinRiverHeight)
                continue;
            List<Tile> river = new List<Tile>();
            FindPath(Tiles[x, y], ref river);
            bool isSand = (river.Count == 0) || river.Last().Biome.TBiome != Constants.Biomes.Sand;
            if (isSand)
                continue;
            
            foreach (var riv in river)
                Tiles[riv.X, riv.Y].HasRiver = true;
            List<int[]> extension;
            Extend(river.Count,out extension);
            bool isXChange = true;
            int extendCounter = extension.Count - 2;
            int extendCoord = 0;
            
            for (int i = 0; i < river.Count; ++i)
            {
               if (i == extension[extension.Count-1][extendCoord])
                {   
                    ++extendCoord;
                    --extendCounter;
                }

                isXChange = (river[i]!=river.Last())?river[i + 1].Y != river[i].Y:isXChange;
                for (int j = 1; j <= extension[extendCounter][0]; j++)
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

        UpdateRivers();
    }

    private void UpdateCastles()
    {
        int castleCount = Castles.MaxCastleNumber;
        while (castleCount != 0)
        {
            int x = Rnd.Next(Castles.WallLength, Width - Castles.WallLength - 1);
            int y = Rnd.Next(Castles.WallLength, Height - Castles.WallLength - 1);
            bool isSuitable = false;
            List<Tile> Walls;
            List<Tile> Floors;
            for (int i = x; i < Castles.WallLength+x; i++)
            {
               for (int j = y; j < Castles.WallLength+y; j++)
                {
                    bool range = Tiles[i, j].HeightValue > Constants.MaxStructureVal || Tiles[i,j].HeightValue<Constants.MinStructureVal;
                    bool isFree = Tiles[i, j].HasRiver || Tiles[i, j].Structure;
                    if (isFree || range)
                    {
                        isSuitable = true;
                        break;
                    }
                    

                }   
            }
            if(isSuitable)
                continue;
            int xUp=x+Castles.WallWidth;
            int xDown=x+Castles.WallLength - Castles.WallWidth - 1;
            int yUp=y+Castles.WallWidth;
            int yDown=y+Castles.WallLength - Castles.WallWidth - 1;
            
            
            for (int i = x; i < Castles.WallLength+x; i++)
            {
                for (int j = y; j < Castles.WallLength+y; j++)
                {
                    bool isx = i < xUp || i > xDown;
                    bool isy = j < yUp || j > yDown;
                    Tiles[i, j].Biome._Color = (isx || isy) ? Constants.Wall : Constants.Floor;
                    Tiles[i, j].Structure = true;
                }
            }
            --castleCount;
        }


    }





}