using Microsoft.AspNetCore.Mvc;

namespace Tests;

public class MapBuilderTests
{
    [Fact]
    public void MapNotNull()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {false, false, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);

        // Act
        var map = mapBuilder.BuildMap(settings[0], settings[1], settings[2]);

        // Assert
        Assert.NotNull(map);
    }

    [Fact]
    public void TilesNotEmpty()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {false, false, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);

        // Act
        var map = mapBuilder.BuildMap(settings[0], settings[1], settings[2]);

        // Assert
        Assert.NotEmpty(map.Tiles);
    }

    [Fact]
    public void TilesHaveRivers()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {true, false, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);
        var result = false;

        // Act
        var map = mapBuilder.BuildMap(settings[0], settings[1], settings[2]);
        for (var x = 0; x < map.Width; ++x)
        {
            for (var y = 0; y < map.Height; ++y)
            {
                if (!map.Tiles[x, y].HasRiver) continue;
                result = map.Tiles[x, y].HasRiver;
                break;
            }
        }

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TilesHaveStructures()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {false, true, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);
        var result = false;

        // Act
        var map = mapBuilder.BuildMap(settings[0], settings[1], settings[2]);
        for (var x = 0; x < map.Width; ++x)
        {
            for (var y = 0; y < map.Height; ++y)
            {
                if (!map.Tiles[x, y].Structure) continue;
                result = map.Tiles[x, y].Structure;
                break;
            }
        }

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TilesPerlinNoiseRange()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {false, false, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);
        var result = true;

        // Act
        var map = mapBuilder.BuildMap(settings[0], settings[1], settings[2]);
        for (var x = 0; x < map.Width; ++x)
        {
            for (var y = 0; y < map.Height; ++y)
            {
                var isInRange = map.Tiles[x, y].BiomeInfo.NoiseNumber is > 0 and < 1;
                if (!isInRange) continue;
                result = false;
                break;
            }
        }

        // Assert
        Assert.True(result);
    }
    
}