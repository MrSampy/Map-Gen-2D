using CourseWork.Map.Helpers;

namespace CourseWork.Map;

public class MapBuilder
{
    private static readonly Random Random = Random.Shared;
    private int Seed { get; }
    public int Width { get; }
    public int Height { get; }
    public int LenOfPixel { get; }
    private readonly int _numOfRanges;
    public readonly Tile[,] Tiles;
    private readonly RiversInfo _rivers;
    private readonly CastlesInfo _castles;
    private readonly List<Castle> _cast;
    private readonly List<Tile> _structures;

    private struct Castle
    {
        public List<Tile> CWalls;
        public Castle(List<Tile> w) => CWalls = w;
    }

    public MapBuilder(int width, int height, int lenOfPix)
    {
        Width = width;
        Height = height;
        LenOfPixel = lenOfPix;
        _rivers = new RiversInfo(width, height);
        _castles = new CastlesInfo(width, height);
        Tiles = new Tile[Width, Height];
        Seed = Random.Next(1, 1000);
        _cast = new List<Castle>();
        _structures = new List<Tile>();
        _numOfRanges = Height / 50;
        PerlinNoise perlinNoise = new PerlinNoise(Seed, Width, Height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile(x, y, perlinNoise.MakeNumber(x, y));
            }
        }

        FindNeighbours();
        foreach (var elem in Constants.SmallObj)
            CreateSmallObjects(elem.Key, elem.Value, 50);
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
                Tiles[x, y].Neighbours = new[]
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
                    Tiles[x, y].Moisture = new TilesMoisture(Constants.MoistureType.Wet, Constants.Wet);
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
            if ((isClear || Random.Next(10) == 0) && river.Count >= 2)
            {
                Tile? newtTile = tempTile.Neighbours[Random.Next(3)];
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
        const double k1 = 1.9;
        const double k2 = 2.5;
        border.Add(new[] {_rivers.MaxRiverWidth, (int) Math.Ceiling(_rivers.MaxRiverWidth / k2)});
        for (int i = 1; i < 5; ++i)
        {
            int elem = (int) Math.Ceiling(border[i - 1][0] / k1);
            int elemMin = (int) Math.Ceiling(elem / k2);
            border.Add(new[] {elem, elemMin});
        }

        List<int> arrBorders = new List<int>();
        int counter = 0;
        arrBorders.Add(Random.Next(1, riverLength / 5));
        while (counter != 3)
        {
            int point = Random.Next(1, riverLength - 2);
            if (arrBorders.Contains(point))
                continue;
            arrBorders.Add(point);
            ++counter;
        }

        arrBorders.Sort();
        arrBorders.Add(0);
        for (int i = 0; i < 6; ++i)
            extend.Add(i != 5 ? new[] {Random.Next(border[i][1], border[i][0])} : arrBorders.ToArray());
    }

    private void CreateSmallObjects(RgbColor color, Constants.Biomes biome, int attemp)
    {
        int counter = _numOfRanges;

        while (attemp != 0)
        {
            if (counter == 0)
                break;

            int x = Random.Next(1, Width - Constants.RangeOfObj - 2);
            int y = Random.Next(1, Height - Constants.RangeOfObj - 2);
            if (Tiles[x, y].Biome.TBiome != biome)
                --attemp;
            else
            {
                for (int i = x; i < Constants.RangeOfObj + x; ++i)
                for (int j = y; j < Constants.RangeOfObj + y; ++j)
                    if (Tiles[i, j].Biome.TBiome == biome && Random.Next(20) == 0)
                        Tiles[i, j].Biome.Color = color;
                --counter;
            }
        }
    }

    private void GenerateRivers()
    {
        int riverCount = _rivers.MaxRiverCount;
        while (riverCount != 0)
        {
            int x = Random.Next(_rivers.MaxRiverWidth, Width - _rivers.MaxRiverWidth - 1);
            int y = Random.Next(_rivers.MaxRiverWidth, Height - _rivers.MaxRiverWidth - 1);
            if (Tiles[x, y].HeightValue < Constants.MinRiverHeight)
                continue;
            List<Tile> river = new List<Tile>();
            FindPath(Tiles[x, y], ref river);
            bool isSand = (river.Count == 0) || river.Last().Biome.TBiome != Constants.Biomes.Sand;
            if (isSand)
                continue;
            foreach (var riv in river)
                Tiles[riv.X, riv.Y].HasRiver = true;
            Extend(river.Count, out List<int[]> extension);
            bool isXChange = true;
            int extendCounter = extension.Count - 2;
            int extendCoord = 0;

            for (int i = 0; i < river.Count; ++i)
            {
                if (i == extension[^1][extendCoord])
                {
                    ++extendCoord;
                    --extendCounter;
                }

                isXChange = (river[i] != river.Last()) ? river[i + 1].Y != river[i].Y : isXChange;
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
        List<Tile> roads = new List<Tile>();

        void FillMainPath(ref List<Tile> path)
        {
            for (int i = 0; i < path.Count; i++)
                if (!_structures.Contains(Tiles[path[i].X, path[i].Y]))
                    Tiles[path[i].X, path[i].Y].Biome.Color = (!path[i].HasRiver) ? Constants.Road : Constants.Bridge;

            _structures.AddRange(path);
            roads.AddRange(path);
            path.Clear();
        }

        void FillFullPAth(List<Tile> path)
        {
            Tile tempTile;
            bool isXChange = true;
            for (int i = 0; i < path.Count; ++i)
            {
                isXChange = (path[i] != path.Last()) ? path[i + 1].Y != path[i].Y : isXChange;
                for (int j = 1; j <= _castles.RoadWidth; j++)
                {
                    if (isXChange)
                    {
                        tempTile = (path[i].X + j < Width) ? (Tiles[path[i].X + j, path[i].Y]) : _structures.Last();
                        if (tempTile.IsLand && !_structures.Contains(tempTile))
                            Tiles[tempTile.X, tempTile.Y].Biome.Color =
                                (!tempTile.HasRiver) ? Constants.Road : Constants.Bridge;
                        tempTile = (path[i].X - j >= 0) ? (Tiles[path[i].X - j, path[i].Y]) : _structures.Last();
                        if (tempTile.IsLand && !_structures.Contains(tempTile))
                            Tiles[tempTile.X, tempTile.Y].Biome.Color =
                                (!tempTile.HasRiver) ? Constants.Road : Constants.Bridge;
                    }
                    else
                    {
                        tempTile = (path[i].Y + j < Height) ? (Tiles[path[i].X, path[i].Y + j]) : _structures.Last();
                        if (tempTile.IsLand && !_structures.Contains(tempTile))
                            Tiles[tempTile.X, tempTile.Y].Biome.Color =
                                (!tempTile.HasRiver) ? Constants.Road : Constants.Bridge;
                        tempTile = (path[i].Y - j >= 0) ? (Tiles[path[i].X, path[i].Y - j]) : _structures.Last();
                        if (tempTile.IsLand && !_structures.Contains(tempTile))
                            Tiles[tempTile.X, tempTile.Y].Biome.Color =
                                (!tempTile.HasRiver) ? Constants.Road : Constants.Bridge;
                    }
                }
            }
        }

        while (true)
        {
            int x = Random.Next(_castles.WallLength, Width - _castles.WallLength - 1);
            int y = Random.Next(_castles.WallLength, Height - _castles.WallLength - 1);
            if (!CreateCastle(x, y))
                continue;
            break;
        }

        List<Tile> road = new List<Tile>();
        //int attempts = 20;
        while (true)
        {
            int index = Random.Next(0, _cast[0].CWalls.Count - 1);
            road.Add(_cast[0].CWalls[index]);
            if (!FindRoad(ref road))
                continue;


            FillMainPath(ref road);
            _structures.AddRange(road);
            roads.AddRange(road);
            road.Clear();
            break;
        }


        int castleCount = _castles.MaxCastleNumber;
        while (castleCount != 0)
        {
            /*  if (attempts == 0)
                  break;*/
            int index = Random.Next(2, roads.Count - 2);
            if (roads[index].Biome.Color != Constants.Road)
                continue;
            road.Add(roads[index]);
            if (!FindRoad(ref road))
            {
                // --attempts;
                continue;
            }

            FillMainPath(ref road);
            --castleCount;
        }

        FillFullPAth(roads);
    }

    private bool CreateCastle(int x, int y)
    {
        bool isSuitable = false;
        List<Tile> walls = new List<Tile>();
        if (x + _castles.WallLength >= Width || y + _castles.WallLength >= Height)
            return false;
        for (int i = x; i < _castles.WallLength + x; i++)
        {
            for (int j = y; j < _castles.WallLength + y; j++)
            {
                bool range = Tiles[i, j].HeightValue > Constants.MaxStructureVal ||
                             Tiles[i, j].HeightValue < Constants.MinStructureVal;
                bool isFree = Tiles[i, j].HasRiver || Tiles[i, j].Structure;
                if (isFree || range)
                {
                    isSuitable = true;
                    break;
                }
            }
        }

        if (isSuitable)
            return false;
        int xUp = x + _castles.WallWidth;
        int xDown = x + _castles.WallLength - _castles.WallWidth - 1;
        int yUp = y + _castles.WallWidth;
        int yDown = y + _castles.WallLength - _castles.WallWidth - 1;
        int xTop = xUp - _castles.WallWidth;
        int xBot = xDown + _castles.WallWidth;
        int yTop = yUp - _castles.WallWidth;
        int yBot = yDown + _castles.WallWidth;


        for (int i = x; i < _castles.WallLength + x; i++)
        {
            for (int j = y; j < _castles.WallLength + y; j++)
            {
                bool isx = i < xUp || i > xDown;
                bool isy = j < yUp || j > yDown;
                bool isTopBorder = i == xTop || i == xBot;
                bool isBotBorder = j == yTop || j == yBot;
                Tiles[i, j].Biome.Color = (isx || isy) ? Constants.Wall : Constants.Floor;
                if (isBotBorder || isTopBorder)
                    walls.Add(Tiles[i, j]);
                Tiles[i, j].Structure = true;
                _structures.Add(Tiles[i, j]);
            }
        }

        _cast.Add(new Castle(walls));
        return true;
    }

    private bool FindRoad(ref List<Tile> road)
    {
        int numb = 0;
        bool isStop = false;
        int counter = 10;
        while (true)
        {
            if (Random.Next(5) == 0)
                numb = Random.Next(3);
            Tile? tempTile = road.Last().Neighbours[numb];
            tempTile = (tempTile == null) ? road.Last() : tempTile;
            bool isStructure = _structures.Contains(tempTile) || road.Contains(tempTile);
            if (!tempTile.IsLand || isStructure)
            {
                road.Clear();
                return false;
            }

            road.Add(tempTile);
            if (road.Count == _castles.MinPathLength)
                isStop = true;
            if (isStop && Random.Next(5) == 0)
            {
                if (CreateCastle(tempTile.X, tempTile.Y))
                    break;
                --counter;
                if (counter == 0)
                    return false;
            }
        }

        return true;
    }
}