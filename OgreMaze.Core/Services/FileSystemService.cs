using System.Collections.Generic;
using System.IO;

namespace OgreMaze.Core.Services
{
    internal class FileSystemService : IFileSystemService
    {
        public IEnumerable<string> ReadFileAsIEnumerable(string filePath)
        {
            return File.ReadLines(filePath);
        }
    }
}
