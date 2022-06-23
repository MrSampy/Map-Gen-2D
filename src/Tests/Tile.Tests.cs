using Microsoft.AspNetCore.Mvc;

namespace Tests;

public class TileTests
{
    [Fact]
    public void HeightMatch()
    {
        // Arrange
        const int x = 0;
        const int y = 0;
        var heightNumbers = new [] {0.651, 0.622, 0.586};
    
        // Act
        var tile = new Tile(x, y, heightNumbers);
        
        // Assert
        Assert.Equal(Constants.Biomes.DeepForest, tile.HeightInfo!.Height);
    }

    [Fact]
    public void HeatMatch()
    {
        // Arrange
        const int x = 0;
        const int y = 0;
        var heightNumbers = new [] {0.651, 0.578, 0.343};

        // Act
        var tile = new Tile(x, y, heightNumbers);
        
        // Assert
        Assert.Equal(Constants.HeatType.Warm, tile.HeatInfo!.Heat);
    }

    [Fact]
    public void MoistureMatch()
    {
        // Arrange
        const int x = 0;
        const int y = 0;
        var heightNumbers = new [] {0.456, 0.649, 0.453};

        // Act
        var tile = new Tile(x, y, heightNumbers);
        
        // Assert
        Assert.Equal(Constants.MoistureType.Wetter, tile.MoistureInfo!.Moisture);
    }

    [Fact]
    public void IsLowestTile()
    {
        // Arrange
        const int x = 3;
        const int y = 3;
        const double height = 0.456;
        const double heat = 0.649;
        const double moisture = 0.453;
        var tile1 = new Tile(x, y, new[] {height, heat, moisture});
        var tile2 = new Tile(x - 1, y, new[] {height + 0.01, heat, moisture});
        var tile3 = new Tile(x, y - 1, new[] {height + 0.02, heat, moisture});
        var tile4 = new Tile(x + 1, y, new[] {height + 0.03, heat, moisture});
        var tile5 = new Tile(x, y + 1, new[] {height + 0.04, heat, moisture});
        tile1.Neighbours = new[] {tile2, tile3, tile4, tile5};

        // Act
        var result = tile1.GetNextPixRiver(tile5);

        // Assert
        Assert.Equal(tile2, result);
    }

    [Fact]
    public void IsBorderMatch()
    {
        // Arrange
        const int x = 0;
        const int y = 0;
        const double shadFactor = 0.8;
        var heightNumbers = new double[] {0.651, 0.622, 0.586};
        var tile = new Tile(x, y, heightNumbers)
        {
            Neighbours = new[] {(Tile?) null, (Tile?) null, (Tile?) null, (Tile?) null}
        };
        var newRed = (int) (tile.HeightInfo!.Color.Red * shadFactor);
        var newGreen = (int) (tile.HeightInfo!.Color.Green * shadFactor);
        var newBlue = (int) (tile.HeightInfo!.Color.Blue * shadFactor);
        var expected = new []{newRed,newGreen,newBlue};

         // Act
        tile.UpdateBiome();
        tile.UpdateBitmask();
        var result = new []{tile.HeightInfo!.Color.Red,tile.HeightInfo!.Color.Green,tile.HeightInfo!.Color.Blue};
        
        // Assert
        Assert.Equal(expected,result);

    }
}