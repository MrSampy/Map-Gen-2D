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
        public readonly List<Tile>  CWalls;
        public Castle(List<Tile> w) => CWalls = w;
    }

    public MapBuilder(int width, int height, int lenOfPix,bool?[] accuracy)
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
        var perlinNoise = new PerlinNoise(Seed, Width, Height);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                Tiles[x, y] = new Tile(x, y, perlinNoise.MakeNumber(x, y));
            }
        }

        FindNeighbours();
        if(accuracy[2].GetValueOrDefault(false))
            foreach (var elem in Constants.SmallObj)
                CreateSmallObjects(elem.Key, elem.Value, 50);
        if(accuracy[0].GetValueOrDefault(false))
          GenerateRivers();
        UpdateBitmasks();
        if(accuracy[1].GetValueOrDefault(false))
            UpdateCastles();
    }

    private void FindNeighbours()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
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
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            Tiles[x, y].UpdateBitmask();
    }

    private void UpdateRivers()
    {
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (!Tiles[x, y].HasRiver) continue;
                Tiles[x, y].Biome = new TilesBiome(Constants.Biomes.ShallowWater, Constants.ShallowWater);
                Tiles[x, y].Moisture = new TilesMoisture(Constants.MoistureType.Wet, Constants.Wet);
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
            var isClear = Convert.ToBoolean(river.Count);
            var nextTile = tempTile.GetNextPixRiver(isClear ? river.Last() : tempTile);
            if ((isClear || Random.Next(10) == 0) && river.Count >= 2)
            {
                var newtTile = tempTile.Neighbours[Random.Next(3)];
                var isVal = newtTile != nextTile && newtTile != river[^2];
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
        var border = new List<int[]>();
        const double k1 = 1.9;
        const double k2 = 2.5;
        border.Add(new[] {_rivers.MaxRiverWidth, (int) Math.Ceiling(_rivers.MaxRiverWidth / k2)});
        for (int i = 1; i < 5; ++i)
        {
            var elem = (int) Math.Ceiling(border[i - 1][0] / k1);
            var elemMin = (int) Math.Ceiling(elem / k2);
            border.Add(new[] {elem, elemMin});
        }

        var arrBorders = new List<int>();
        var counter = 0;
        arrBorders.Add(Random.Next(1, riverLength / 5));
        while (counter != 3)
        {
            var point = Random.Next(1, riverLength - 2);
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

    private void CreateSmallObjects(RgbColor color, Constants.Biomes biome, int attempt)
    {
        var counter = _numOfRanges;

        while (attempt != 0)
        {
            if (counter == 0)
                break;

            var x = Random.Next(1, Width - Constants.RangeOfObj - 2);
            var y = Random.Next(1, Height - Constants.RangeOfObj - 2);
            if (Tiles[x, y].Biome.TBiome != biome)
                --attempt;
            else
            {
                for (var i = x; i < Constants.RangeOfObj + x; ++i)
                for (var j = y; j < Constants.RangeOfObj + y; ++j)
                    if (Tiles[i, j].Biome.TBiome == biome && Random.Next(20) == 0)
                        Tiles[i, j].Biome.Color = color;
                --counter;
            }
        }
    }

    private void GenerateRivers()
    {
        var riverCount = _rivers.MaxRiverCount;
        while (riverCount != 0)
        {
            var x = Random.Next(_rivers.MaxRiverWidth, Width - _rivers.MaxRiverWidth - 1);
            var y = Random.Next(_rivers.MaxRiverWidth, Height - _rivers.MaxRiverWidth - 1);
            if (Tiles[x, y].HeightValue < Constants.MinRiverHeight)
                continue;
            var river = new List<Tile>();
            FindPath(Tiles[x, y], ref river);
            var isSand = (river.Count == 0) || river.Last().Biome.TBiome != Constants.Biomes.Sand;
            if (isSand)
                continue;
            foreach (var riv in river)
                Tiles[riv.X, riv.Y].HasRiver = true;
            Extend(river.Count, out var extension);
            var isXChange = true;
            var extendCounter = extension.Count - 2;
            var extendCoord = 0;

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
        var roads = new List<Tile>();

        void FillMainPath(ref List<Tile> path)
        {
            foreach (var tile in path.Where(tile => !_structures.Contains(Tiles[tile.X, tile.Y])))
                Tiles[tile.X, tile.Y].Biome.Color = (!tile.HasRiver) ? Constants.Road : Constants.Bridge;
            _structures.AddRange(path);
            roads.AddRange(path);
            path.Clear();
        }

        void FillFullPAth(List<Tile> path)
        {
            void MakeStructure(Tile tile)
            {
                if (tile.IsLand && !_structures.Contains(tile))
                    Tiles[tile.X, tile.Y].Biome.Color =
                        (!tile.HasRiver) ? Constants.Road : Constants.Bridge;
                
            }

            var isXChange = true;
            for (var i = 0; i < path.Count; ++i)
            {
                isXChange = (path[i] != path.Last()) ? path[i + 1].Y != path[i].Y : isXChange;
                for (var j = 1; j <= _castles.RoadWidth; j++)
                {
                    Tile tempTile;
                    if (isXChange)
                    {
                        tempTile = (path[i].X + j < Width) ? (Tiles[path[i].X + j, path[i].Y]) : _structures.Last();
                        MakeStructure(tempTile);
                        tempTile = (path[i].X - j >= 0) ? (Tiles[path[i].X - j, path[i].Y]) : _structures.Last();
                        MakeStructure(tempTile);
                    }
                    else
                    {
                        tempTile = (path[i].Y + j < Height) ? (Tiles[path[i].X, path[i].Y + j]) : _structures.Last();
                        MakeStructure(tempTile);
                        tempTile = (path[i].Y - j >= 0) ? (Tiles[path[i].X, path[i].Y - j]) : _structures.Last();
                        MakeStructure(tempTile);
                    }
                }
            }
        }

        while (true)
        {
            var x = Random.Next(_castles.WallLength, Width - _castles.WallLength - 1);
            var y = Random.Next(_castles.WallLength, Height - _castles.WallLength - 1);
            if (!CreateCastle(x, y))
                continue;
            break;
        }

        List<Tile> road = new List<Tile>();
        //int attempts = 20;
        while (true)
        {
            var index = Random.Next(0, _cast[0].CWalls.Count - 1);
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
        var isSuitable = false;
        var walls = new List<Tile>();
        if (x + _castles.WallLength >= Width || y + _castles.WallLength >= Height)
            return false;
        for (var i = x; i < _castles.WallLength + x; i++)
        {
            for (var j = y; j < _castles.WallLength + y; j++)
            {
                var range = Tiles[i, j].HeightValue > Constants.MaxStructureVal ||
                            Tiles[i, j].HeightValue < Constants.MinStructureVal;
                var isFree = Tiles[i, j].HasRiver || Tiles[i, j].Structure;
                if (!isFree && !range) continue;
                isSuitable = true;
                break;
            }
        }

        if (isSuitable)
            return false;
        var xUp = x + _castles.WallWidth;
        var xDown = x + _castles.WallLength - _castles.WallWidth - 1;
        var yUp = y + _castles.WallWidth;
        var yDown = y + _castles.WallLength - _castles.WallWidth - 1;
        var xTop = xUp - _castles.WallWidth;
        var xBot = xDown + _castles.WallWidth;
        var yTop = yUp - _castles.WallWidth;
        var yBot = yDown + _castles.WallWidth;


        for (var i = x; i < _castles.WallLength + x; i++)
        {
            for (var j = y; j < _castles.WallLength + y; j++)
            {
                var isx = i < xUp || i > xDown;
                var isy = j < yUp || j > yDown;
                var isTopBorder = i == xTop || i == xBot;
                var isBotBorder = j == yTop || j == yBot;
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
        var numb = 0;
        var isStop = false;
        var counter = 10;
        while (true)
        {
            if (Random.Next(5) == 0)
                numb = Random.Next(3);
            var tempTile = road.Last().Neighbours[numb];
            tempTile ??= road.Last();
            var isStructure = _structures.Contains(tempTile) || road.Contains(tempTile);
            if (!tempTile.IsLand || isStructure)
            {
                road.Clear();
                return false;
            }

            road.Add(tempTile);
            if (road.Count == _castles.MinPathLength)
                isStop = true;
            if (!isStop || Random.Next(5) != 0) continue;
            if (CreateCastle(tempTile.X, tempTile.Y))
                break;
            --counter;
            if (counter == 0)
                return false;
        }

        return true;
    }
}