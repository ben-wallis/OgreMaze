using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core.Tests.Services
{
    [TestFixture]
    public class TestMapService
    {
        private SwampMapTestUtility _testUtility;

        [SetUp]
        public void SwampMapTestSetup()
        {
            _testUtility = new SwampMapTestUtility();
        }

        [Test]
        public void LoadMap_MapHasCorrectDimensions()
        {
            // Arrange
            var testMapList = new List<string> { "--------", "--------", "--------" };
            _testUtility.MockFileSystemService.Setup(f => f.ReadFileAsIEnumerable(_testUtility.TestMapFilePath)).Returns(testMapList);
            
            // Act
            _testUtility.TestMapService.LoadMap(_testUtility.TestMapFilePath);

            // Assert
            Assert.IsNotNull(_testUtility.TestMapService.Map);
            Assert.AreEqual(8, _testUtility.TestMapService.Map.GetLength(0));
            Assert.AreEqual(3, _testUtility.TestMapService.Map.GetLength(1));
        }

        [Test]
        public void LoadMap_JaggedMapThrowsException()
        {
            // Arrange
            var testMapList = new List<string> { "--------", "----", "-------" };
            _testUtility.MockFileSystemService.Setup(f => f.ReadFileAsIEnumerable(_testUtility.TestMapFilePath)).Returns(testMapList);
            
            // Act
            
            // Assert
            Assert.Throws<Exception>(() => _testUtility.TestMapService.LoadMap(_testUtility.TestMapFilePath));
        }

        [Test]
        public void FindStartingPoint_TestMap1_ReturnsCorrectCoordinates()
        {
            // Arrange
            const int ExpectedXCoordinate = 0;
            const int ExpectedYCoordinate = 0;

            _testUtility.MockFileSystemService.Setup(f => f.ReadFileAsIEnumerable(_testUtility.TestMapFilePath))
                .Returns(_testUtility.TestMap1);
            _testUtility.MockTileService.Setup(t => t.GetTileTypeFromChar('@')).Returns(TileType.Ogre);
            _testUtility.TestMapService.LoadMap(_testUtility.TestMapFilePath);

            // Act
            var result = _testUtility.TestMapService.FindStartingTile();

            // Assert
            Assert.AreEqual(ExpectedXCoordinate, result.Xpos);
            Assert.AreEqual(ExpectedYCoordinate, result.Ypos);
        }

        [Test]
        public void FindStartingPoint_TestMap2_ReturnsCorrectCoordinates()
        {
            // Arrange
            const int ExpectedXCoordinate = 7;
            const int ExpectedYCoordinate = 8;

            _testUtility.MockFileSystemService.Setup(f => f.ReadFileAsIEnumerable(_testUtility.TestMapFilePath))
                .Returns(_testUtility.TestMap2);
            _testUtility.MockTileService.Setup(t => t.GetTileTypeFromChar('@')).Returns(TileType.Ogre);
            _testUtility.TestMapService.LoadMap(_testUtility.TestMapFilePath);

            // Act
            var result = _testUtility.TestMapService.FindStartingTile();

            // Assert
            Assert.AreEqual(ExpectedXCoordinate, result.Xpos);
            Assert.AreEqual(ExpectedYCoordinate, result.Ypos);
        }

        private class SwampMapTestUtility
        {
            public SwampMapTestUtility()
            {
                // Mock Setups
                MockFileSystemService = new Mock<IFileSystemService>();
                MockTileService = new Mock<ITileService>();

                // Test Maps Setup
                TestMap1 = new List<string>
                {
                    "@@........",
                    "@@O.......",
                    ".....O.O..",
                    "..........",
                    "..O.O.....",
                    "..O....O.O",
                    ".O........",
                    "..........",
                    ".....OO...",
                    ".........$"
                };

                TestMap2 = new List<string>
                {
                    "$.O...O...",
                    "...O......",
                    "..........",
                    "O..O..O...",
                    "..........",
                    "O..O..O...",
                    "..........",
                    "......OO..",
                    "O..O...@@.",
                    ".......@@."
                };

                TestMapFilePath = "C:\\Test.txt";

                // Class under test instantiation
                TestMapService = new MapService(MockFileSystemService.Object, MockTileService.Object);
            }

            public Mock<IFileSystemService> MockFileSystemService { get; private set; }
            public Mock<ITileService> MockTileService { get; private set; }

            public MapService TestMapService { get; private set; }

            public List<string> TestMap1 { get; private set; }
            public List<string> TestMap2 { get; private set; }

            public string TestMapFilePath { get; private set; }
            
        }
    }
}
