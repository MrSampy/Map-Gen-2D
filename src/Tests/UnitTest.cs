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
}