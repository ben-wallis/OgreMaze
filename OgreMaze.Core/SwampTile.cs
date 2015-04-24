using System;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core
{
    public class SwampTile
    {
        public SwampTile(TileType tileType, int xPos, int yPos)
        {
            SwampTileType = tileType;
            Xpos = xPos;
            Ypos = yPos;
        }

        public TileType SwampTileType { get; set; }
        public int Xpos { get; private set; }
        public int Ypos { get; private set; }

        public int MovementCostFromStartingPoint { get; set; } // G
        public SwampTile ParentSwampTile { get; set; }

        public int Cost { get; set; }

        public int EstimatedCostToTile(SwampTile tile)
        {
            var estimatedHorizontalMovementToDestination = Math.Abs(tile.Xpos - Xpos);
            var estimatedVerticalMovementToDestination = Math.Abs(tile.Ypos - Ypos);
            return (estimatedVerticalMovementToDestination
                    + estimatedHorizontalMovementToDestination)*10;
        }
    }
}
