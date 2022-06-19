using Microsoft.AspNetCore.Mvc;

namespace Tests;

public class UnitTest
{
    [Fact]
    public void IndexViewResultNotNull()
    {
        // Arrange
        var mapController = new MapController();
        const int sizeId = 0;
        const int  lenOfPix = 1;
        var settings = new[]{false,false,false };
        
        // Act
        var result = mapController.Index(sizeId, lenOfPix, settings[0], settings[0], settings[0]);
        
        // Assert
        Assert.NotNull(result);

    }
    [Fact]
    public void IndexViewNameEqualHeightMap()
    {
        // Arrange
        var mapController = new MapController();
        const int sizeId = 0;
        const int lenOfPix = 1;
        var settings = new[]{false,false,false };
        
        // Act
        var result = mapController.Index(sizeId, lenOfPix, settings[0], settings[0], settings[0]) as ViewResult;
        
        // Assert
        Assert.Equal("DrawHeightMap", result?.ViewName);

    }
    [Fact]
    public void MapNotNull()
    {
        // Arrange
        const int side = 100;
        const int leOfPix = 1;
        var settings = new[]{false,false,false };
        var mapBuilder = new MapBuilder(side,side,leOfPix);
        Map map;
        // Act
        map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        // Assert
        Assert.NotNull(map);

    }
    [Fact]
    public void TilesNotEmpty()
    {
        // Arrange
        const int side = 100;
        const int leOfPix = 1;
        var settings = new[]{false,false,false };
        var mapBuilder = new MapBuilder(side,side,leOfPix);
        Map map;
        // Act
        map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        // Assert
        Assert.NotEmpty(map.Tiles);

    }
    [Fact]
    public void TilesHaveRivers()
    {
        // Arrange
        const int side = 100;
        const int leOfPix = 1;
        var settings = new[]{true,false,false };
        var mapBuilder = new MapBuilder(side,side,leOfPix);
        var result = false;
        Map map;
        // Act
        map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        for (var x = 0; x < map.Width; ++x)
        {
            for (var y = 0; y < map.Height; y++)
            {
                if (map.Tiles[x, y].HasRiver)
                {
                    result = map.Tiles[x, y].HasRiver;
                    break;
                }
            }
        }
        // Assert
        Assert.True(result);

    }
}