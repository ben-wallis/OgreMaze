using System;
using System.Linq;

using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core
{
    internal class SwampMap
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly ITileService _tileService;

        public SwampMap(IFileSystemService fileSystemService, ITileService tileService)
        {
            _fileSystemService = fileSystemService;
            _tileService = tileService;
        }

        public TileType[,] Map { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public void LoadMap(string mapFilePath)
        {
            var mapContents = _fileSystemService.ReadFileAsIEnumerable(mapFilePath);

            var currentLine = 0;
            var lastLineLength = 0;
            foreach (var line in mapContents)
            {
                if (Map == null)
                {
                    Map = new TileType[line.Length, mapContents.Count()];
                }

                if (currentLine > 0 && line.Length != lastLineLength)
                {
                    throw new Exception("Unable to use jagged maps");
                }

                for (var stringPos = 0; stringPos < line.Length; stringPos++)
                {
                    Map[stringPos, currentLine] = _tileService.GetTileTypeFromChar(line[stringPos]);
                }
                
                lastLineLength = line.Length;
                currentLine++;
            }
        }
    }
}
