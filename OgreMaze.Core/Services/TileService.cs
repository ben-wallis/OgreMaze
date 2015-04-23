using System;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core.Services
{
    internal class TileService : ITileService
    {
        public TileType GetTileTypeFromChar(char inputChar)
        {
            switch (inputChar)
            {
                case '@':
                    return TileType.Ogre;
                case 'O':
                    return TileType.SinkHole;
                case '.':
                    return TileType.Empty;
                case '$':
                    return TileType.Gold;
                default:
                    throw new Exception("Invalid Tile Type");
            }
        }

        public bool TilePassable(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Empty:
                    return true;
                case TileType.Gold:
                    return true;
                case TileType.Ogre:
                    return true;
                case TileType.SinkHole:
                    return false;
                default:
                    throw new Exception("Invalid Tile Type");
            }
        }
    }
}
