using System;
using Moq;
using NUnit.Framework;
using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core.Tests
{
    [TestFixture]
    public class TestSwampNavigator
    {
        private SwampNavigatorTestUtility _testUtility;

        [SetUp]
        public void SwampNavigatorTestSetup()
        {
            _testUtility = new SwampNavigatorTestUtility();
        }

        [Test]
        public void Navigate_LoadsMapFromPath()
        {
            // Arrange
            _testUtility.MockMapService.Setup(m => m.LoadMap(_testUtility.TestPath)).Verifiable();

            // Act
            _testUtility.TestSwampNavigator.Navigate(_testUtility.TestPath);

            // Assert
            _testUtility.MockMapService.Verify(m => m.LoadMap(_testUtility.TestPath));
        }

        [Test]
        public void Navigate_CallsFindStartingPoint()
        {
            // Arrange
            _testUtility.MockMapService.Setup(m => m.FindStartingTile()).Verifiable();

            // Act
            _testUtility.TestSwampNavigator.Navigate(_testUtility.TestPath);

            // Assert
            _testUtility.MockMapService.Verify(m => m.FindStartingTile());
        }

        [Test]
        public void FindOpenTiles_WorksCorrectly_TopLeft()
        {
            // Arrange

            // Act
            _testUtility.TestSwampNavigator.FindNeighbouringOpenTiles(_testUtility.TestMap1[0,0]);

            // Assert
            Assert.That(_testUtility.TestSwampNavigator._openTiles, Contains.Item(_testUtility.TestMap1[1, 0]));
            Assert.That(_testUtility.TestSwampNavigator._openTiles, Contains.Item(_testUtility.TestMap1[0, 1]));
            Assert.AreEqual(2, _testUtility.TestSwampNavigator._openTiles.Count);
        }

        [Test]
        public void FindOpenTiles_WorksCorrectly_Middle()
        {
            // Arrange

            // Act
            _testUtility.TestSwampNavigator.FindNeighbouringOpenTiles(_testUtility.TestMap1[1,1]);

            // Assert
            Assert.That(_testUtility.TestSwampNavigator._openTiles, Contains.Item(_testUtility.TestMap1[1, 0]));
            Assert.That(_testUtility.TestSwampNavigator._openTiles, Contains.Item(_testUtility.TestMap1[0, 1]));
            Assert.That(_testUtility.TestSwampNavigator._openTiles, Contains.Item(_testUtility.TestMap1[1, 2]));
            Assert.AreEqual(3, _testUtility.TestSwampNavigator._openTiles.Count);
        }

        private class SwampNavigatorTestUtility
        {
            public SwampNavigatorTestUtility()
            {
                // Mock setups
                MockMapService = new Mock<IMapService>();
                MockMapService.SetupGet(m => m.Height).Returns(3);
                MockMapService.SetupGet(m => m.Width).Returns(4);

                TestTileService = new TileService();

                TestPath = "C:\\TestFile.txt";

                // Test Map 1:
                // @@..
                // @@O.
                // ....
                TestMap1 = new SwampTile[4,3];
                TestMap1[0, 0] = new SwampTile(TileType.Ogre, 0, 0);
                TestMap1[1, 0] = new SwampTile(TileType.Ogre, 1, 0);
                TestMap1[2, 0] = new SwampTile(TileType.Empty, 2, 0);
                TestMap1[3, 0] = new SwampTile(TileType.Empty, 3, 0);
                TestMap1[0, 1] = new SwampTile(TileType.Ogre, 0, 1);
                TestMap1[1, 1] = new SwampTile(TileType.Ogre, 1, 1);
                TestMap1[2, 1] = new SwampTile(TileType.SinkHole, 2, 1);
                TestMap1[3, 1] = new SwampTile(TileType.Empty, 3, 1);
                TestMap1[0, 2] = new SwampTile(TileType.Empty, 0, 2);
                TestMap1[1, 2] = new SwampTile(TileType.Empty, 1, 2);
                TestMap1[2, 2] = new SwampTile(TileType.Empty, 2, 2);
                TestMap1[3, 2] = new SwampTile(TileType.Empty, 3, 2);

                MockMapService.SetupGet(m => m.Map).Returns(TestMap1);

                // Class under test instantiation
                TestSwampNavigator = new SwampNavigator(MockMapService.Object, TestTileService);
            }

            public Mock<IMapService> MockMapService { get; private set; }
            public ITileService TestTileService { get; private set; }

            public SwampNavigator TestSwampNavigator { get; private set; }

            public string TestPath { get; private set; }

            public SwampTile[,] TestMap1 { get; private set; }
        }
    }

    
}
