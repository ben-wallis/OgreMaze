using System;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core
{
    public class SwampTile
    {
        public SwampTile(TileType tileType, int xPos, int yPos)
        {
            SwampTileType = tileType;
            X = xPos;
            Y = yPos;
        }

        public TileType SwampTileType { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public int MovementCostFromStartingPoint { get; set; } // G
        public SwampTile ParentSwampTile { get; set; }

        public int Cost { get; set; }

        public int EstimatedCostToTile(SwampTile tile)
        {
            var estimatedHorizontalMovementToDestination = Math.Abs(tile.X - X);
            var estimatedVerticalMovementToDestination = Math.Abs(tile.Y - Y);
            return (estimatedVerticalMovementToDestination
                    + estimatedHorizontalMovementToDestination)*10;
        }
    }
}
