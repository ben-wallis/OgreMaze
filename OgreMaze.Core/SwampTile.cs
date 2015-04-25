using System;
using OgreMaze.Core.Enums;

namespace OgreMaze.Core
{
    public class SwampTile
    {
        private TileType _swampTileType;

        public SwampTile(TileType tileType, int xPos, int yPos)
        {
            SwampTileType = tileType;
            X = xPos;
            Y = yPos;
        }

        public TileType SwampTileType
        {
            get { return _swampTileType; }
            set { _swampTileType = value; }
        }

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
