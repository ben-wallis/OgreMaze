using System;
using System.Linq;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    internal class MapService : IMapService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly ITileService _tileService;

        public MapService(IFileSystemService fileSystemService, ITileService tileService)
        {
            _fileSystemService = fileSystemService;
            _tileService = tileService;
        }

        public SwampTile[,] Map { get; private set; }

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
                    Map = new SwampTile[line.Length, mapContents.Count()];
                }

                if (currentLine > 0 && line.Length != lastLineLength)
                {
                    throw new Exception("Unable to use jagged maps");
                }

                for (var stringPos = 0; stringPos < line.Length; stringPos++)
                {
                    Map[stringPos, currentLine] = new SwampTile(_tileService.GetTileTypeFromChar(line[stringPos]),
                        stringPos, currentLine);
                }
                
                lastLineLength = line.Length;
                currentLine++;
            }
        }

        public SwampTile FindFirstTileContaining(TileType tileType)
        {
            for (var y = 0; y < Map.GetLength(1); y++)
            {
                for (var x = 0; x < Map.GetLength(0); x++)
                {
                    if (Map[x, y].SwampTileType == tileType)
                    {
                        return Map[x, y];
                    }
                }
            }

            throw new Exception("No occurances of the specified in map!");
        }
    }
}
