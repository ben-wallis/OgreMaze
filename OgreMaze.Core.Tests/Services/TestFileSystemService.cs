using System.Linq;
using NUnit.Framework;
using OgreMaze.Core.Services;

namespace OgreMaze.Core.Tests.Services
{
    [TestFixture]
    public class TestFileSystemService
    {
        // C:\TestFile.txt contents:
        //@@........
        //@@O.......
        //.....O.O..
        //..........
        //..O.O.....
        //..O....O.O
        //.O........
        //..........
        //.....OO...
        //.........$
        [Test]
        public void ReadFileAsIEnumerable_ReturnsIEnumerableWithCorrectCount()
        {
            // Arrange
            const int ExpectedCount = 10;
            var fileSystemService = new FileSystemService();

            // Act
            var result = fileSystemService.ReadFileAsIEnumerable(@"C:\TestFile.txt");

            // Assert
            Assert.AreEqual(ExpectedCount, result.Count());
        }

        [Test]
        public void ReadFileAsIEnumerable_ReturnsIEnumerableWithCorrectContent()
        {
            // Arrange
            var fileSystemService = new FileSystemService();

            // Act
            var result = fileSystemService.ReadFileAsIEnumerable(@"C:\TestFile.txt");

            // Assert
            var enumerable = result as string[] ?? result.ToArray();
            Assert.AreEqual("@@........", enumerable.ElementAt(0));
            Assert.AreEqual("@@O.......", enumerable.ElementAt(1));
            Assert.AreEqual(".....O.O..", enumerable.ElementAt(2));
            Assert.AreEqual("..........", enumerable.ElementAt(3));
            Assert.AreEqual("..O.O.....", enumerable.ElementAt(4));
            Assert.AreEqual("..O....O.O", enumerable.ElementAt(5));
            Assert.AreEqual(".O........", enumerable.ElementAt(6));
            Assert.AreEqual("..........", enumerable.ElementAt(7));
            Assert.AreEqual(".....OO...", enumerable.ElementAt(8));
            Assert.AreEqual(".........$", enumerable.ElementAt(9));
        }
    }
}
