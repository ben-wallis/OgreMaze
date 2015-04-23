using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void TestGetTileTypeFromString_Ogre_ReturnsCorrectTileType(string inputString, string tileType)
        {
            // Arrange
            var expectedTileType = TileServiceTestUtility.TileTypeTextToEnum(tileType);

            var tileService = new TileService();

            // Act
            var result = tileService.GetTileTypeFromChar(inputString[0]);

            // Assert
            Assert.AreEqual(expectedTileType, result);
        }



        [TestCase("Ogre", true)]
        [TestCase("SinkHole", false)]
        [TestCase("Empty", true)]
        [TestCase("Gold", true)]
        public void TestTilePassable_ReturnsCorrectValues(string tileType, bool expectedResult)
        {
            // Arrange
            var testTileType = TileServiceTestUtility.TileTypeTextToEnum(tileType);

            var tileService = new TileService();

            // Act
            var result = tileService.TilePassable(testTileType);

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
                    default:
                        throw new Exception("Invalid Tile Type");
                }
            }
        }
    }
}
