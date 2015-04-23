using System.Collections.Generic;

namespace OgreMaze.Core.Services
{
    internal interface IFileSystemService
    {
        IEnumerable<string> ReadFileAsIEnumerable(string filePath);
    }
}