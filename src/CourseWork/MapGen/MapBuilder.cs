using CourseWork.MapGen.Helpers;
namespace CourseWork.MapGen;

public sealed class MapBuilder
{
    private static Random Random = Random.Shared;
    private readonly Map _map;
    private readonly RiversInfo _rivers;
    private readonly CastlesInfo _castles;
    private readonly List<Castle> _cast;
    private readonly List<Tile> _structures;
    private readonly int _numOfRanges;
    private struct Castle
    {
        public readonly List<Tile>  CWalls;
        public Castle(List<Tile> w) => CWalls = w;
    }

    public MapBuilder(int width, int height, int lenOfPix)
    {
        _map = new Map(width,height,lenOfPix);
        _rivers = new RiversInfo(_map.Width, _map.Height);
        _castles = new CastlesInfo(_map.Width, _map.Height);
        var heightSeed = Random.Next(1, 1000);
        var heatSeed = Random.Next(1, 1000);
        var moistureSeed = Random.Next(1, 1000);
        _cast = new List<Castle>();
        _structures = new List<Tile>();
        _numOfRanges = _map.Height / 50;
        var perlinNoise1 = new PerlinNoise(heightSeed, _map.Width, _map.Height);
        var perlinNoise2 = new PerlinNoise(heatSeed, _map.Width, _map.Height);
        var perlinNoise3 = new PerlinNoise(moistureSeed, _map.Width, _map.Height);

        for (var x = 0; x <  _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                _map.Tiles[x, y] = new Tile(x, y, new []{perlinNoise1.MakeNumber(x, y),
                    perlinNoise2.MakeNumber(x, y),
                    perlinNoise3.MakeNumber(x, y)});
            }
        }
        UpdateMap();
        UpdateHeatMap();
        FindNeighbours();

    }

    public Map BuildMap(bool hasRiver, bool hasCastles, bool hasParticles)
    {
        if(hasParticles)
            foreach (var elem in Constants.SmallObj)
                AddSmallObjects(elem.Key, elem.Value);
        if(hasRiver)
            AddRivers();
        UpdateBitmasks();
        if(hasCastles)
            AddCastles();
        return _map;
    }

    private void UpdateMap()
    {
        for (var x = 0; x < _map.Width; x++)
        for (var y = 0; y < _map.Height; y++)
                _map.Tiles[x,y].UpdateBiome();
    }
    private bool IsCold (Constants.HeatType heat) => heat is Constants.HeatType.Cold 
    or Constants.HeatType.Colder or Constants.HeatType.Coldest;
    private void UpdateHeatMap()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                _map.Tiles[x, y].Heat = _map.Tiles[x, y].Biome.TBiome switch
                {
                    Constants.Biomes.Snow => new TilesHeat(Constants.HeatType.Coldest, Constants.Coldest),
                    Constants.Biomes.HardRock => new TilesHeat(Constants.HeatType.Colder, Constants.Colder),
                    Constants.Biomes.Rock => new TilesHeat(Constants.HeatType.Cold, Constants.Cold),
                    Constants.Biomes.DeepWater => new TilesHeat(Constants.HeatType.Colder, Constants.Colder),
                    Constants.Biomes.Ocean => new TilesHeat(Constants.HeatType.Cold, Constants.Cold),
                    _ => _map.Tiles[x, y].Heat
                };
            }
        }
    }

    private void FindNeighbours()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                _map.Tiles[x, y].LeftTile = (y - 1 < 0) ? null : _map.Tiles[x, y - 1];
                _map.Tiles[x, y].RightTile = (y + 1 >= _map.Height) ? null : _map.Tiles[x, y + 1];
                _map.Tiles[x, y].TopTile = (x - 1 < 0) ? null : _map.Tiles[x - 1, y];
                _map.Tiles[x, y].BottomTile = (x + 1 >= _map.Width) ? null : _map.Tiles[x + 1, y];
                _map.Tiles[x, y].Neighbours = new[]
                    {_map.Tiles[x, y].LeftTile, _map.Tiles[x, y].RightTile, 
                     _map.Tiles[x, y].TopTile, _map.Tiles[x, y].BottomTile};
            }
        }
    }
    
    private void UpdateBitmasks()
    {
        for (var x = 0; x < _map.Width; x++)
        for (var y = 0; y < _map.Height; y++)
            _map.Tiles[x, y].UpdateBitmask();
    }

    private void UpdateRivers()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                if (!_map.Tiles[x, y].HasRiver) continue;
                _map.Tiles[x, y].Biome = new TilesBiome(Constants.Biomes.Coast, Constants.Coast);
            }
        }
    }

    private void FindPath(Tile startTile, ref List<Tile> river)
    {
        ref Tile tempTile = ref _map.Tiles[startTile.X, startTile.Y];
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
    
    private void AddSmallObjects(RgbColor color, Constants.Biomes biome)
    {
        int attempt = 50;
        var counter = _numOfRanges;
        while (attempt != 0)
        {
            if (counter == 0)
                break;

            var x = Random.Next(1, _map.Width - Constants.RangeOfObj - 2);
            var y = Random.Next(1, _map.Height - Constants.RangeOfObj - 2);
            if (_map.Tiles[x, y].Biome.TBiome != biome)
                --attempt;
            else
            {
                for (var i = x; i < Constants.RangeOfObj + x; ++i)
                for (var j = y; j < Constants.RangeOfObj + y; ++j)
                    if (_map.Tiles[i, j].Biome.TBiome == biome && Random.Next(20) == 0)
                        _map.Tiles[i, j].Biome.Color = color;
                --counter;
            }
        }
    }

    private void AddRivers()
    {
        var riverCount = _rivers.MaxRiverCount;
        while (riverCount != 0)
        {
            var x = Random.Next(_rivers.MaxRiverWidth, _map.Width - _rivers.MaxRiverWidth - 1);
            var y = Random.Next(_rivers.MaxRiverWidth, _map.Height - _rivers.MaxRiverWidth - 1);
            if (_map.Tiles[x, y].HeightValue < Constants.MinRiverHeight)
                continue;
            var river = new List<Tile>();
            FindPath(_map.Tiles[x, y], ref river);
            var isSand = (river.Count == 0) || river.Last().Biome.TBiome != Constants.Biomes.Sand;
            if (isSand)
                continue;
            foreach (var riv in river)
                _map.Tiles[riv.X, riv.Y].HasRiver = true;
            _rivers.Extend(river.Count, out var extension);
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
                        if (river[i].X + j < _map.Width)
                            _map.Tiles[river[i].X + j, river[i].Y].HasRiver = true;
                        if (river[i].X - j >= 0)
                            _map.Tiles[river[i].X - j, river[i].Y].HasRiver = true;
                    }
                    else
                    {
                        if (river[i].Y + j < _map.Height)
                            _map.Tiles[river[i].X, river[i].Y + j].HasRiver = true;
                        if (river[i].Y - j >= 0)
                            _map.Tiles[river[i].X, river[i].Y - j].HasRiver = true;
                    }
                }
            }

            --riverCount;
        }

        UpdateRivers();
    }

    private void AddCastles()
    {
        int stop = _map.Height / 100;
        var roads = new List<Tile>();
        void FillMainPath(ref List<Tile> path)
        {
            foreach (var tile in path.Where(tile => !_structures.Contains(_map.Tiles[tile.X, tile.Y])))
                _map.Tiles[tile.X, tile.Y].Biome.Color = (!tile.HasRiver) ? Constants.Road : Constants.Bridge;
            _structures.AddRange(path);
            roads.AddRange(path);
            path.Clear();
        }
        void FillFullPAth(List<Tile> path)
        {
            void MakeStructure(Tile tile)
            {
                if (tile.IsLand && !_structures.Contains(tile))
                    _map.Tiles[tile.X, tile.Y].Biome.Color =
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
                        tempTile = (path[i].X + j < _map.Width)
                            ? (_map.Tiles[path[i].X + j, path[i].Y])
                            : _structures.Last();
                        MakeStructure(tempTile);
                        tempTile = (path[i].X - j >= 0)
                            ? (_map.Tiles[path[i].X - j, path[i].Y])
                            : _structures.Last();
                        MakeStructure(tempTile);
                    }
                    else
                    {
                        tempTile = (path[i].Y + j < _map.Height)
                            ? (_map.Tiles[path[i].X, path[i].Y + j])
                            : _structures.Last();
                        MakeStructure(tempTile);
                        tempTile = (path[i].Y - j >= 0)
                            ? (_map.Tiles[path[i].X, path[i].Y - j])
                            : _structures.Last();
                        MakeStructure(tempTile);
                    }
                }
            }
        }
        int counter = 0;
        while (counter!=stop)
        {
            while (true)
            {
                var x = Random.Next(_castles.WallLength, _map.Width - _castles.WallLength - 1);
                var y = Random.Next(_castles.WallLength, _map.Height - _castles.WallLength - 1);
                if (!CreateCastle(x, y, true))
                    continue;
                break;
            }
            List<Tile> road = new List<Tile>();
            while (true)
            {
                var index = Random.Next(0, _cast[counter].CWalls.Count - 1);
                road.Add(_cast[counter].CWalls[index]);
                if (!FindRoad(ref road))
                    continue;


                FillMainPath(ref road);
                _structures.AddRange(road);
                roads.AddRange(road);
                break;
            }
            int castleCount = Random.Next(2,4);
            int attempts = 100;
            while (castleCount != 0)
            {
                if (attempts == 0)
                    break;
                int index = Random.Next(2, roads.Count - 2);
                if (roads[index].Biome.Color != Constants.Road)
                    continue;
                road.Add(roads[index]);
                if (!FindRoad(ref road))
                {
                    --attempts;
                    continue;
                }

                FillMainPath(ref road);
                --castleCount;
            }

            FillFullPAth(roads);
            ++counter;
        }
        
        
    }

    private bool CreateCastle(int x, int y, bool isMainCast)
    {
        var isSuitable = false;
        var walls = new List<Tile>();
        if (x + _castles.WallLength >= _map.Width || y + _castles.WallLength >= _map.Height)
            return false;
        for (var i = x; i < _castles.WallLength + x; i++)
        {
            for (var j = y; j < _castles.WallLength + y; j++)
            {
                var range = _map.Tiles[i, j].HeightValue > Constants.MaxStructureVal ||
                            _map.Tiles[i, j].HeightValue < Constants.MinStructureVal;
                var isFree = _map.Tiles[i, j].HasRiver || _map.Tiles[i, j].Structure;
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
                _map.Tiles[i, j].Biome.Color = (isx || isy) ? Constants.Wall : Constants.Floor;
                if (isMainCast && (isBotBorder || isTopBorder))
                    walls.Add(_map.Tiles[i, j]);
                _map.Tiles[i, j].Structure = true;
                _structures.Add(_map.Tiles[i, j]);
            }
        }
        if(isMainCast)
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
            if (CreateCastle(tempTile.X, tempTile.Y,false))
                break;
            --counter;
            if (counter == 0)
                return false;
        }

        return true;
    }

}