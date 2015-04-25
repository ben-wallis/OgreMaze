using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    public interface ITileService
    {
        TileType GetTileTypeFromChar(char inputChar);
        char GetCharFromTileType(TileType tileType);
        bool TilePassable(SwampTile tile);
    }
}