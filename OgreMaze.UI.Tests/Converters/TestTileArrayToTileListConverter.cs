using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OgreMaze.Core;
using OgreMaze.UI.Converters;

namespace OgreMaze.UI.Tests.Converters
{
    [TestFixture]
    public class TestTileArrayToTileListConverter
    {
        [Test]
        public void Convert_ReturnsCorrectlySizedListOfLists()
        {
            // Arrange
            const int TestArrayWidth = 5;
            const int TestArrayHeight = 10;
            var testArray = new SwampTile[TestArrayWidth, TestArrayHeight];

            var converter = new TileArrayToTileListConverter();

            // Act
            var result = (List<List<SwampTile>>)converter.Convert(testArray, null, null, null);

            // Assert
            Assert.AreEqual(TestArrayHeight, result.Count);
            var firstListItem = result.First();
            Assert.AreEqual(TestArrayWidth, firstListItem.Count);
        }
    }
}
