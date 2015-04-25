using System.Collections.Generic;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    public interface IMapService
    {
        SwampTile[,] Map { get; }
        int Width { get; }
        int Height { get; }

        void GenerateAndLoadMap(int width, int height, int percentSinkholes);
        void LoadMapFromFile(string mapFilePath);
        void DrawMap();
        SwampTile FindFirstTileContaining(TileType tileType);
        List<SwampTile> GetDropZoneTiles(SwampTile destinationTile);
        bool OgreCanFitInTile(SwampTile tile);
        void RecordOgreFootPrints(SwampTile tile);
    }
}