using System;
using System.Collections.Generic;
using System.Linq;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    internal class MapService : IMapService
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly ITileService _tileService;
        private readonly IMapGenerationService _mapGenerationService;

        public MapService(IFileSystemService fileSystemService, ITileService tileService, IMapGenerationService mapGenerationService)
        {
            _fileSystemService = fileSystemService;
            _tileService = tileService;
            _mapGenerationService = mapGenerationService;
        }

        public SwampTile[,] Map { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public void GenerateAndLoadMap(int width, int height)
        {
            Map = _mapGenerationService.GenerateMap(width, height);
            Width = width - 1;
            Height = height - 1;
            Console.WriteLine("Generated new map:");
            Console.WriteLine(string.Empty);
            DrawMap();
            Console.WriteLine(string.Empty);
        }

        public void LoadMapFromFile(string mapFilePath)
        {
            var mapContents = _fileSystemService.ReadFileAsIEnumerable(mapFilePath).ToList();

            var currentLine = 0;
            foreach (var line in mapContents)
            {
                if (Map == null)
                {
                    Map = new SwampTile[line.Length, mapContents.Count()];
                    Width = line.Length - 1;
                    Height = mapContents.Count - 1;
                }

                if (line.Length != Width + 1)
                {
                    throw new Exception("Unable to use jagged maps");
                }

                for (var stringPos = 0; stringPos < line.Length; stringPos++)
                {
                    Map[stringPos, currentLine] = new SwampTile(_tileService.GetTileTypeFromChar(line[stringPos]),
                        stringPos, currentLine);
                }

                currentLine++;
            }
        }

        public void DrawMap()
        {
            for (var y = 0; y <= Height; y++)
            {
                var line = String.Empty;

                for (var x = 0; x <= Width; x++)
                {
                    line += _tileService.GetCharFromTileType(Map[x, y].SwampTileType);
                }
                Console.WriteLine(line);
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

            throw new Exception("No occurrences of the specified in map!");
        }

        public bool OgreCanFitInTile(SwampTile tile)
        {
            if (tile.X + 1 > Width || tile.Y + 1 > Height)
            {
                return false;
            }

            var eastTilePassable = _tileService.TilePassable(Map[tile.X + 1, tile.Y]);
            var southTilePassable = _tileService.TilePassable(Map[tile.X, tile.Y + 1]);
            var southEastTilePassable = _tileService.TilePassable(Map[tile.X + 1, tile.Y + 1]);
            return (eastTilePassable && southTilePassable && southEastTilePassable);
        }

        public List<SwampTile> GetDropZoneTiles(SwampTile destinationTile)
        {
            var dropZoneTiles = new List<SwampTile> {destinationTile};

            // Ogres are fat, add up to 3 other possible tiles that will make him touch the gold
            var northOfDest = destinationTile.Y - 1 >= 0 ? Map[destinationTile.X, destinationTile.Y - 1] : null;
            var westOfDest = destinationTile.X - 1 >= 0 ? Map[destinationTile.X - 1, destinationTile.Y] : null;
            var northWestOfDest = destinationTile.Y - 1 >= 0 && destinationTile.X - 1 >= 0
                ? Map[destinationTile.X - 1, destinationTile.Y - 1]
                : null;

            dropZoneTiles.Add(northOfDest);
            dropZoneTiles.Add(westOfDest);
            dropZoneTiles.Add(northWestOfDest);

            return dropZoneTiles;
        }

        public void RecordOgreFootPrints(SwampTile tile)
        {
            Map[tile.X, tile.Y].SwampTileType = TileType.OgreFootprints;
            Map[tile.X, tile.Y + 1].SwampTileType = TileType.OgreFootprints;
            Map[tile.X + 1, tile.Y + 1].SwampTileType = TileType.OgreFootprints;
            Map[tile.X + 1, tile.Y].SwampTileType = TileType.OgreFootprints;
        }
    }
}
