using System;
using System.Collections.Generic;
using System.Linq;
using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core
{
    public class SwampNavigator : ISwampNavigator
    {
        private readonly IMapService _mapService;
        private readonly ITileService _tileService;
        private SwampTile _startingTile;
        private SwampTile _destinationTile;
        private List<SwampTile> _dropZoneTiles = new List<SwampTile>();
        private SwampTile _currentTile;

        private readonly List<SwampTile> _openTiles = new List<SwampTile>();
        private readonly List<SwampTile> _closedTiles = new List<SwampTile>();
 
        public SwampNavigator(IMapService mapService, ITileService tileService)
        {
            _mapService = mapService;
            _tileService = tileService;
        }

        public bool GenerateMapAndNavigate(int width, int height, int sinkholesPerHundred)
        {
            _mapService.GenerateAndLoadMap(width, height, sinkholesPerHundred);
            return NavigateMap();
        }

        public bool NavigateMap(string mapFile)
        {
            _mapService.LoadMapFromFile(mapFile);
            return NavigateMap();
        }

        public bool NavigateMap()
        {
            _startingTile = _mapService.FindFirstTileContaining(TileType.Ogre);
            _destinationTile = _mapService.FindFirstTileContaining(TileType.Gold);
            _dropZoneTiles = _mapService.GetDropZoneTiles(_destinationTile);
            
            _currentTile = _startingTile;
            _openTiles.Clear();
            _closedTiles.Clear();

            _openTiles.Add(_startingTile);
            
            // Main A* algorithm loop, runs until destination reached (any of the drop zone tiles are added to the closed list)
            // or there are no more options to consider
            try {
                while (!_closedTiles.Any(c => _dropZoneTiles.Any(d => d == c)))
                {
                    _currentTile = GetLowestCostTileFromOpenTiles();
                    _openTiles.Remove(_currentTile);
                    _closedTiles.Add(_currentTile);
                    ConsiderNeighbouringTiles(_currentTile);
                    UpdateOpenTileCosts();
                }
                // Destination reached, set the current tile to the tile in the drop zone that's reached
                // the closed list so we can walk back from it to draw the ogre's path.
                _currentTile = _closedTiles.First(c => _dropZoneTiles.Any(d => d == c));

                Console.WriteLine(string.Empty);
                Console.WriteLine("The ogre has found his gold, yay!");
                Console.WriteLine(string.Empty);
                return true;
            }
            catch (InvalidOperationException e)
            {
                if (e.Message != "Sequence contains no elements") throw;

                // If there are no more open tiles to consider, there is no possible path to the gold. Sad Ogre :(
                Console.WriteLine(string.Empty);
                Console.WriteLine("No more open tiles, ogre has ran out of options! Gold is unreachable :(");
                Console.WriteLine(string.Empty);
                return false;
            }
        }

        internal void ConsiderNeighbouringTiles(SwampTile tile)
        {
            if (TileValidForConsideration(tile.X, tile.Y - 1))
            {
                ConsiderTile(_mapService.Map[tile.X, tile.Y - 1]);
            }

            if (TileValidForConsideration(tile.X, tile.Y + 1))
            {
                ConsiderTile(_mapService.Map[tile.X, tile.Y + 1]);
            }

            if (TileValidForConsideration(tile.X - 1, tile.Y))
            {
                ConsiderTile(_mapService.Map[tile.X - 1, tile.Y]);
            }

            if (TileValidForConsideration(tile.X + 1, tile.Y))
            {
                ConsiderTile(_mapService.Map[tile.X + 1, tile.Y]);
            }
        }

        private bool TileValidForConsideration(int x, int y)
        {
            // If the tile is:
            // a) Within the bounds of the map
            // b) A passable tile (not a sinkhole)
            // c) Not in the closed list
            // d) Big enough to fit an Ogre (the East, SouthEast and South tiles are also passable)
            // Then it is valid for consideration.
            return x <= _mapService.Width && y <= _mapService.Height && x >= 0 && y >= 0 &&
                   _tileService.TilePassable(_mapService.Map[x, y]) && !_closedTiles.Contains(_mapService.Map[x, y]) &&
                   _mapService.OgreCanFitInTile(_mapService.Map[x, y]);
        }
        
        internal void ConsiderTile(SwampTile tile)
        {
            if (!_openTiles.Contains(tile))
            {
                _openTiles.Add(tile);
                tile.ParentSwampTile = _currentTile;
            }
            else
            {
                if (tile.MovementCostFromStartingPoint > (_currentTile.MovementCostFromStartingPoint + 10))
                {
                    tile.ParentSwampTile = _currentTile;
                }
            }
        }

        private void UpdateOpenTileCosts()
        {
            foreach (var tile in _openTiles)
            {
                tile.MovementCostFromStartingPoint = tile.ParentSwampTile.MovementCostFromStartingPoint + 10;
                tile.Cost = tile.MovementCostFromStartingPoint + tile.EstimatedCostToTile(_destinationTile);
            }
        }

        private SwampTile GetLowestCostTileFromOpenTiles()
        {
            return _openTiles.OrderBy(p => p.Cost).First();
        }
        
        public void DrawPath()
        {
            _mapService.RecordOgreFootPrints(_currentTile);
            while (_currentTile.ParentSwampTile != null)
            {
                _currentTile = _currentTile.ParentSwampTile;
                _mapService.RecordOgreFootPrints(_currentTile);
            }

            _mapService.DrawMap();
            Console.ReadLine();
        }
    }
}
