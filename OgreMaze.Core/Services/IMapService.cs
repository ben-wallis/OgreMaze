using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    internal interface IMapService
    {
        SwampTile[,] Map { get; }
        int Width { get; }
        int Height { get; }
        void LoadMap(string mapFilePath);

        SwampTile FindFirstTileContaining(TileType tileType);
    }
}