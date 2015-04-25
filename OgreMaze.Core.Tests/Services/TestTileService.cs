using System;

using NUnit.Framework;

using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core.Tests.Services
{
    [TestFixture]
    public class TestTileService
    {
        private TileServiceTestUtility _testUtility;

        [SetUp]
        public void TileServiceTestUtilitySetup()
        {
            _testUtility = new TileServiceTestUtility();
        }

        [TestCase("@", "Ogre")]
        [TestCase("O", "SinkHole")]
        [TestCase(".", "Empty")]
        [TestCase("$", "Gold")]
        public void TestGetTileTypeFromString_ReturnsCorrectTileType(string inputString, string tileType)
        {
            // Arrange
            var expectedTileType = TileServiceTestUtility.TileTypeTextToEnum(tileType);

            var tileService = new TileService();

            // Act
            var result = tileService.GetTileTypeFromChar(inputString[0]);

            // Assert
            Assert.AreEqual(expectedTileType, result);
        }

        [TestCase("Ogre", '@')]
        [TestCase("SinkHole", 'O')]
        [TestCase("Empty", '.')]
        [TestCase("Gold", '$')]
        [TestCase("OgreFootprints", '&')]
        public void TestGetCharFromTileType_ReturnsCorrectChar(string tileType, char expectedResult)
        {
            var inputTileType = TileServiceTestUtility.TileTypeTextToEnum(tileType);

            var tileService = new TileService();

            // Act
            var result = tileService.GetCharFromTileType(inputTileType);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
        
        [TestCase("Ogre", true)]
        [TestCase("SinkHole", false)]
        [TestCase("Empty", true)]
        [TestCase("Gold", true)]
        public void TestTilePassable_ReturnsCorrectValues(string tileType, bool expectedResult)
        {
            // Arrange
            var testTileType = TileServiceTestUtility.TileTypeTextToEnum(tileType);
            var testTile = new SwampTile(testTileType, 0, 0);

            var tileService = new TileService();

            // Act
            var result = tileService.TilePassable(testTile);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        private class TileServiceTestUtility
        {
            public static TileType TileTypeTextToEnum(string tileTypeText)
            {
                switch (tileTypeText)
                {
                    case "Ogre":
                        return TileType.Ogre;
                    case "SinkHole":
                        return TileType.SinkHole;
                    case "Empty":
                        return TileType.Empty;
                    case "Gold":
                        return TileType.Gold;
                    case "OgreFootprints":
                        return TileType.OgreFootprints;
                    default:
                        throw new Exception("Invalid Tile Type");
                }
            }
        }
    }
}
