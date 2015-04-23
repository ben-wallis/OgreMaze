using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    internal interface ITileService
    {
        TileType GetTileTypeFromChar(char inputChar);
    }
}