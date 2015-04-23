using System;
using System.Collections.Generic;

using Moq;

using NUnit.Framework;

using OgreMaze.Core.Services;

namespace OgreMaze.Core.Tests
{
    [TestFixture]
    public class TestSwampMap
    {
        [Test]
        public void LoadMap_MapHasCorrectDimensions()
        {
            // Arrange
            const string TestMapFilePath = "C:\\Test.txt";

            var mockFileSystemService = new Mock<IFileSystemService>();
            var testMapList = new List<string> { "--------", "--------", "--------" };
            mockFileSystemService.Setup(f => f.ReadFileAsIEnumerable(TestMapFilePath)).Returns(testMapList);
            var mockTileService = new Mock<ITileService>();

            var swampMap = new SwampMap(mockFileSystemService.Object, mockTileService.Object);

            // Act
            swampMap.LoadMap(TestMapFilePath);

            // Assert
            Assert.IsNotNull(swampMap.Map);
            Assert.AreEqual(8,swampMap.Map.GetLength(0));
            Assert.AreEqual(3, swampMap.Map.GetLength(1));
        }

        [Test]
        public void LoadMap_JaggedMapThrowsException()
        {
            // Arrange
            const string TestMapFilePath = "C:\\Test.txt";

            var mockFileSystemService = new Mock<IFileSystemService>();
            var testMapList = new List<string> { "--------", "----", "-------" };
            mockFileSystemService.Setup(f => f.ReadFileAsIEnumerable(TestMapFilePath)).Returns(testMapList);
            var mockTileService = new Mock<ITileService>();

            var swampMap = new SwampMap(mockFileSystemService.Object, mockTileService.Object);

            // Act
            
            // Assert
            Assert.Throws<Exception>(() => swampMap.LoadMap(TestMapFilePath));
        }
    }
}
