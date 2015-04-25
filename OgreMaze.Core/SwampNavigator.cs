using System;
using System.Collections.Generic;
using System.Linq;
using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core
{
    internal class SwampNavigator : ISwampNavigator
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

        public void Navigate(string mapFile)
        {
            _mapService.LoadMap(mapFile);
            _startingTile = _mapService.FindFirstTileContaining(TileType.Ogre);
            _destinationTile = _mapService.FindFirstTileContaining(TileType.Gold);
            _dropZoneTiles = _mapService.GetDropZoneTiles(_destinationTile);
            
            _currentTile = _startingTile;
            _openTiles.Add(_startingTile);
            
            // Main A* algorithm loop, runs until destination reached (any of the drop zone tiles are added to the closed list)
            // or there are no more options to consider
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

            DrawPath();
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
            try
            {
                return _openTiles.OrderBy(p => p.Cost).First();
            }
            catch (Exception)
            {
                // If there are no more open tiles to consider, there is no possible path to the gold. Sad Ogre :(
                Console.WriteLine("Ran out of open tiles, ogre has ran out of options!");
                DrawPath();
                return _currentTile;
            }
        }
        
        private void DrawPath()
        {
            _mapService.RecordOgreFootPrints(_currentTile);
            while (_currentTile.ParentSwampTile != null)
            {
                _currentTile = _currentTile.ParentSwampTile;
                _mapService.RecordOgreFootPrints(_currentTile);
            }

            for (var y = 0; y <= _mapService.Height; y++)
            {
                var line = String.Empty;

                for (var x = 0; x <= _mapService.Width; x++)
                {
                    line += _tileService.GetCharFromTileType(_mapService.Map[x, y].SwampTileType);
                }
                Console.WriteLine(line);
            }
            Console.ReadLine();
        }
    }
}
