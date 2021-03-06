using CourseWork.MapGen.Helpers;

namespace CourseWork.MapGen;

public sealed class MapBuilder
{
    private static readonly Random Random = Random.Shared;
    private readonly Map _map;
    private readonly RiversInfo _rivers;
    private readonly CastlesInfo _castles;
    private readonly List<Castle> _cast;
    private readonly List<Tile> _structures;
    private readonly int _numOfRanges;

    private struct Castle
    {
        public readonly List<Tile> CWalls;
        public Castle(List<Tile> wall) => CWalls = wall;
    }

    public MapBuilder(int mapWidth, int mapHeight, int lenOfPix)
    {
        const int mapDivisor = 50;
        const int minSeedValue = 1;
        const int maxSeedValue = 1000;

        _map = new Map(mapWidth, mapHeight, lenOfPix);
        _rivers = new RiversInfo(_map.Width, _map.Height);
        _castles = new CastlesInfo(_map.Width, _map.Height);
        _cast = new List<Castle>();
        _structures = new List<Tile>();
        _numOfRanges = _map.Height / mapDivisor;

        var heightSeed = Random.Next(minSeedValue, maxSeedValue);
        var heatSeed = Random.Next(minSeedValue, maxSeedValue);
        var moistureSeed = Random.Next(minSeedValue, maxSeedValue);
        var heightPerlinNoise = new PerlinNoise(heightSeed, _map.Width, _map.Height);
        var heatPerlinNoise = new PerlinNoise(heatSeed, _map.Width, _map.Height);
        var moisturePerlinNoise = new PerlinNoise(moistureSeed, _map.Width, _map.Height);

        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                var height = heightPerlinNoise.MakeNumber(x, y);
                var heat = heatPerlinNoise.MakeNumber(x, y);
                var moisture = moisturePerlinNoise.MakeNumber(x, y);
                _map.Tiles[x, y] = new Tile(x, y, new[] {height, heat, moisture});
            }
        }

        UpdateHeatMap();
        UpdateMoistureMap();
        UpdateBiomeMap();
        FindNeighbours();
    }

    public Map BuildMap(bool hasRiver, bool hasCastles, bool hasParticles)
    {
        if (hasParticles)
        {
            foreach (var elem in Constants.SmallObj)
                AddParticles(elem.Key, elem.Value);
        }

        if (hasRiver)
            AddRivers();
        UpdateBitmasks();
        if (hasCastles)
            AddCastles();
        return _map;
    }

    private void UpdateBiomeMap() =>
        Parallel.For(0, _map.Width, x => { Parallel.For(0, _map.Height, y => _map.Tiles[x, y].UpdateBiome()); });

    private void UpdateHeatMap()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                double newHeight = 0;

                foreach (var biome in Constants.UpdateHeat.Where(biome
                             => _map.Tiles[x, y].HeightInfo!.Height == biome.Key))
                    newHeight = _map.Tiles[x, y].HeatInfo!.NoiseNumber * biome.Value;

                if (Convert.ToBoolean(newHeight))
                    _map.Tiles[x, y].UpdateHeat(newHeight);
            }
        }
    }

    private void FindNeighbours()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                var leftTile = y - 1 < 0 ? null : _map.Tiles[x, y - 1];
                var rightTile = y + 1 >= _map.Height ? null : _map.Tiles[x, y + 1];
                var topTile = x - 1 < 0 ? null : _map.Tiles[x - 1, y];
                var bottomTile = x + 1 >= _map.Width ? null : _map.Tiles[x + 1, y];
                _map.Tiles[x, y].Neighbours = new[] {leftTile, rightTile, topTile, bottomTile};
            }
        }
    }

    private void UpdateBitmasks() =>
        Parallel.For(0, _map.Width, x => { Parallel.For(0, _map.Height, y => _map.Tiles[x, y].UpdateBitmask()); });


    private void UpdateRivers()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                if (!_map.Tiles[x, y].HasRiver) continue;
                _map.Tiles[x, y].HeightInfo = new TilesHeight(Constants.Biomes.Coast, Constants.Coast);
                var moistureCof = _map.Tiles[x, y].MoistureInfo!.NoiseNumber * Constants.MoistureUpCoastR;
                _map.Tiles[x, y].UpdateMoisture(moistureCof);
                _map.Tiles[x, y].BiomeInfo.Color = Constants.RiversUpdate[_map.Tiles[x, y].BiomeInfo.Biome];
            }
        }
    }

    private void FindPath(Tile startTile, ref List<Tile> river)
    {
        ref var tempTile = ref _map.Tiles[startTile.X, startTile.Y];
        if (river.Contains(tempTile))
        {
            river.Clear();
            return;
        }

        if (!tempTile.IsLand || tempTile.HasRiver) return;
        const int turnNum = 10;
        const int range = 2;
        const int maxNeighbor = 3;

        river.Add(tempTile);

        var isClear = Convert.ToBoolean(river.Count);
        var nextTile = tempTile.GetNextPixRiver(isClear ? river.Last() : tempTile);

        if ((isClear || Random.Next(turnNum) == 0) && river.Count >= range)
        {
            var newtTile = tempTile.Neighbours[Random.Next(maxNeighbor)];
            var isVal = newtTile != nextTile && newtTile != river[^range];
            nextTile = (newtTile != null && isVal) ? newtTile : nextTile;
        }

        FindPath(nextTile, ref river);
    }

    private void AddParticles(RgbColor color, Constants.Biomes biome)
    {
        var attempt = 50;
        var counter = _numOfRanges;
        const int bordersRange = 2;
        const int particleNum = 20;

        while (attempt != 0)
        {
            if (counter == 0)
                break;

            var x = Random.Next(1, _map.Width - Constants.RangeOfParticles - bordersRange);
            var y = Random.Next(1, _map.Height - Constants.RangeOfParticles - bordersRange);

            if (_map.Tiles[x, y].HeightInfo!.Height != biome)
            {
                --attempt;
            }
            else
            {
                for (var i = x; i < Constants.RangeOfParticles + x; ++i)
                for (var j = y; j < Constants.RangeOfParticles + y; ++j)
                    if (_map.Tiles[i, j].HeightInfo!.Height == biome && Random.Next(particleNum) == 0)
                        _map.Tiles[i, j].HeightInfo!.Color = color;

                --counter;
            }
        }
    }

    private Dictionary<(int x, int y), bool> GetCondition(Tile tile, bool isXChange, int changeNumber)
    {
        var conditionsX = new Dictionary<(int x, int y), bool>
        {
            {(x: tile.X + changeNumber, y: tile.Y), tile.X + changeNumber < _map.Width},
            {(x: tile.X - changeNumber, y: tile.Y), tile.X - changeNumber >= 0}
        };

        var conditionsY = new Dictionary<(int x, int y), bool>
        {
            {(x: tile.X, y: tile.Y + changeNumber), tile.Y + changeNumber < _map.Height},
            {(x: tile.X, y: tile.Y - changeNumber), tile.Y - changeNumber >= 0}
        };

        return isXChange ? conditionsX : conditionsY;
    }

    private void ExtendRiver(IReadOnlyList<Tile> river)
    {
        _rivers.Extend(river.Count, out var extension);
        const int range = 2;
        var isXChange = true;
        var extendCounter = extension.Count - range;
        var extendCoord = 0;

        for (var i = 0; i < river.Count; ++i)
        {
            if (i == extension[^1][extendCoord])
            {
                ++extendCoord;
                --extendCounter;
            }

            isXChange = (river[i] != river[^1]) ? river[i + 1].Y != river[i].Y : isXChange;
            for (var j = 1; j <= extension[extendCounter][0]; j++)
            {
                var conditions = GetCondition(river[i], isXChange, j);

                foreach (var condition in conditions.Where(condition => condition.Value))
                    _map.Tiles[condition.Key.x, condition.Key.y].HasRiver = true;
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
            var isMinRiverGen = _map.Tiles[x, y].HeightInfo!.NoiseNumber < Constants.MinRiverGeneration;
            var isCorrectLand = _map.Tiles[x, y].HasRiver || isMinRiverGen;

            if (!_map.Tiles[x, y].IsLand || isCorrectLand)
                continue;

            var river = new List<Tile>();
            FindPath(_map.Tiles[x, y], ref river);
            var isSand = river.Count == 0 || river.Last().HeightInfo!.Height != Constants.Biomes.Sand;

            if (isSand || river.Count <= Constants.MinRiverLength)
                continue;

            foreach (var riv in river)
                _map.Tiles[riv.X, riv.Y].HasRiver = true;

            ExtendRiver(river);
            --riverCount;
        }

        UpdateRivers();
    }

    private void MakeStructure(Tile tile)
    {
        if (!tile.IsLand || _structures.Contains(tile)) return;
        var color = !tile.HasRiver ? Constants.Road : Constants.Bridge;
        _map.Tiles[tile.X, tile.Y].HeightInfo!.Color = color;
    }

    private void FillFullPathCastle(IReadOnlyList<Tile> path)
    {
        var isXChange = true;
        for (var i = 0; i < path.Count; ++i)
        {
            isXChange = (path[i] != path.Last()) ? path[i + 1].Y != path[i].Y : isXChange;
            for (var j = 1; j <= _castles.RoadWidth; j++)
            {
                var conditions = GetCondition(path[i], isXChange, j);

                foreach (var tempTile in conditions.Select(condition => condition.Value
                             ? _map.Tiles[condition.Key.x, condition.Key.y]
                             : _structures.Last()))
                    MakeStructure(tempTile);
            }
        }
    }

    private void FillMainPath(IReadOnlyCollection<Tile> path)
    {
        foreach (var tile in path.Where(tile => !_structures.Contains(_map.Tiles[tile.X, tile.Y])))
            _map.Tiles[tile.X, tile.Y].HeightInfo!.Color = !tile.HasRiver ? Constants.Road : Constants.Bridge;
        _structures.AddRange(path);
    }

    private void AddCastles()
    {
        const int cofDivision = 100;
        var stop = _map.Height / cofDivision;
        var allRoads = new List<Tile>();
        var counter = 0;
        while (counter != stop)
        {
            var roads = new List<Tile>();
            while (true)
            {
                var x = Random.Next(_castles.WallLength, _map.Width - _castles.WallLength - 1);
                var y = Random.Next(_castles.WallLength, _map.Height - _castles.WallLength - 1);
                if (!CreateCastle(x, y, true))
                    continue;
                break;
            }

            while (true)
            {
                var road = new List<Tile>();
                var index = Random.Next(0, _cast[counter].CWalls.Count - 1);
                road.Add(_cast[counter].CWalls[index]);
                if (!FindRoad(ref road))
                    continue;
                FillMainPath(road);
                roads.AddRange(road);
                break;
            }

            const int minNumCastles = 2;
            const int maxNumCastles = 4;
            var castleCount = Random.Next(minNumCastles, maxNumCastles);
            var attempts = 100;
            while (castleCount != 0)
            {
                var road = new List<Tile>();
                if (attempts == 0)
                    break;
                var index = Random.Next(minNumCastles, roads.Count - minNumCastles);
                if (roads[index].HeightInfo!.Color != Constants.Road)
                    continue;
                road.Add(roads[index]);
                if (!FindRoad(ref road))
                {
                    --attempts;
                    continue;
                }

                FillMainPath(road);
                roads.AddRange(road);
                --castleCount;
            }

            ++counter;
            allRoads.AddRange(roads);
        }

        FillFullPathCastle(allRoads);
    }

    private void CheckTerritory(int x, int y, out bool isSuitable)
    {
        isSuitable = true;
        var xOutRange = x + _castles.WallLength >= _map.Width;
        var yOutRange = y + _castles.WallLength >= _map.Height;
        if (xOutRange || yOutRange)
            return;
        for (var i = x; i < _castles.WallLength + x; i++)
        {
            for (var j = y; j < _castles.WallLength + y; j++)
            {
                var range = _map.Tiles[i, j].HeightInfo!.NoiseNumber
                    is > Constants.MaxStructureVal or < Constants.MinStructureVal;
                var isFree = _map.Tiles[i, j].HasRiver || _map.Tiles[i, j].Structure;
                if (!isFree && !range) continue;
                isSuitable = false;
                break;
            }
        }
    }

    private bool CreateCastle(int x, int y, bool isMainCast)
    {
        CheckTerritory(x, y, out var isSuitable);
        var walls = new List<Tile>();
        if (!isSuitable)
            return isSuitable;

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
                _map.Tiles[i, j].HeightInfo!.Color = (isx || isy) ? Constants.Wall : Constants.Floor;
                if (isMainCast && (isBotBorder || isTopBorder))
                    walls.Add(_map.Tiles[i, j]);
                _map.Tiles[i, j].Structure = true;
                _structures.Add(_map.Tiles[i, j]);
            }
        }

        if (isMainCast)
            _cast.Add(new Castle(walls));
        return isSuitable;
    }

    private bool FindRoad(ref List<Tile> road)
    {
        var numb = 0;
        var isStop = false;
        var counter = 10;
        const int roadNumber = 5;
        const int maxRoad = 3;
        while (true)
        {
            if (Random.Next(roadNumber) == 0)
                numb = Random.Next(maxRoad);
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
            if (!isStop || Random.Next(roadNumber) != 0) continue;
            if (CreateCastle(tempTile.X, tempTile.Y, false))
                break;
            --counter;
            if (counter == 0)
                return false;
        }

        return true;
    }

    private void UpdateMoistureMap()
    {
        for (var x = 0; x < _map.Width; x++)
        {
            for (var y = 0; y < _map.Height; y++)
            {
                double newHeight = 0;

                foreach (var biome in Constants.MoistureUpdate.Where(biome
                             => _map.Tiles[x, y].HeightInfo!.Height == biome.Key))
                    newHeight = _map.Tiles[x, y].MoistureInfo!.NoiseNumber * biome.Value;

                if (Convert.ToBoolean(newHeight))
                    _map.Tiles[x, y].UpdateMoisture(newHeight);
            }
        }
    }
}