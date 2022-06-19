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
    public void IndexViewResultNull()
    {
        // Arrange
        var mapController = new MapController();
        const int sizeId = 0;
        const int  lenOfPix = 1;
        var settings = new[]{false,false,false };
        
        // Act
        var result = mapController.Index(sizeId, lenOfPix, settings[0], settings[0], settings[0]);
        
        // Assert
        Assert.Null(result);

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
    public void IndexViewNameEqualHeattMap()
    {
        // Arrange
        var mapController = new MapController();
        const int sizeId = 0;
        const int lenOfPix = 1;
        var settings = new[]{false,false,false };
        
        // Act
        var result = mapController.Index(sizeId, lenOfPix, settings[0], settings[0], settings[0]) as ViewResult;
        
        // Assert
        Assert.Equal("DrawHeatMap", result?.ViewName);

    }
    [Fact]
    public void MapNotNull()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[]{false,false,false };
        var mapBuilder = new MapBuilder(side,side,lenOfPix);
        
        // Act
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        
        // Assert
        Assert.NotNull(map);

    }
    [Fact]
    public void MapNull()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[]{false,false,false };
        var mapBuilder = new MapBuilder(side,side,lenOfPix);
        
        // Act
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        
        // Assert
        Assert.Null(map);

    }
    [Fact]
    public void TilesNotEmpty()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[]{false,false,false };
        var mapBuilder = new MapBuilder(side,side,lenOfPix);
        
        // Act
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        
        // Assert
        Assert.NotEmpty(map.Tiles);

    }
    [Fact]
    public void TilesEmpty()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[]{false,false,false };
        var mapBuilder = new MapBuilder(side,side,lenOfPix);
        
        // Act
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
        
        // Assert
        Assert.Empty(map.Tiles);

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
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
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
    public void TilesHaveNoRivers()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {false, false, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);
        var result = false;
        
        // Act
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
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
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
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
    public void TilesHaveNoStructures()
    {
        // Arrange
        const int side = 100;
        const int lenOfPix = 1;
        var settings = new[] {false, false, false};
        var mapBuilder = new MapBuilder(side, side, lenOfPix);
        var result = false;
        
        // Act
        var map = mapBuilder.BuildMap(settings[0],settings[1],settings[2]);
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
    
   
}