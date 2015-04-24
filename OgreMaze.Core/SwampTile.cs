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

        public TileType SwampTileType { get; private set; }
        public int Xpos { get; private set; }
        public int Ypos { get; private set; }

        public int MovementCostFromStartingPoint { get; set; } // G
        public int EstimatedMovementCostToDestination { get; set; } // H
        public SwampTile ParentSwampTile { get; set; }

        public int Cost
        {
            get
            {
                return MovementCostFromStartingPoint
                       + EstimatedMovementCostToDestination;
            }
        }
    }
}
