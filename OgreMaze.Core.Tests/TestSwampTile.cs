using NUnit.Framework;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Tests
{
    [TestFixture]
    public class TestSwampTile
    {
        [TestCase(0,0,9,9,180)]
        [TestCase(9,9,0,0,180)]
        [TestCase(2,3,7,8,100)]
        [TestCase(7,8,2,3,100)]
        [TestCase(6,4,1,2,70)]
        [TestCase(1,2,6,4,70)]
        public void EstimatedCostToTile_ReturnsCorrectValue(int startTileX, int startTileY, int destTileX, int destTileY, int expectedResult)
        {
            // Arrange
            var tile = new SwampTile(TileType.Empty, startTileX, startTileY);
            var destinationTile = new SwampTile(TileType.Empty, destTileX, destTileY);

            // Act
            var result = tile.EstimatedCostToTile(destinationTile);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
