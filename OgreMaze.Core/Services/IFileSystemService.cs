using System.Collections.Generic;

namespace OgreMaze.Core.Services
{
    public interface IFileSystemService
    {
        IEnumerable<string> ReadFileAsIEnumerable(string filePath);
    }
}