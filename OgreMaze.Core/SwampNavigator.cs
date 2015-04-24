using System;
using System.Collections.Generic;
using System.Linq;

using OgreMaze.Core.Enums;
using OgreMaze.Core.Services;

namespace OgreMaze.Core
{
    internal class SwampNavigator
    {
        private readonly IMapService _mapService;
        private readonly ITileService _tileService;
        private SwampTile _startingTile;
        private SwampTile _destinationTile;
        private SwampTile _currentTile;

        internal List<SwampTile> _openTiles = new List<SwampTile>();
        internal List<SwampTile> _closedTiles = new List<SwampTile>();
 
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
            _currentTile = _startingTile;
            _openTiles.Add(_startingTile);
            
            while (_currentTile != _destinationTile)
            {
                ProcessCurrentTile();
                UpdateOpenTileCosts();
                _currentTile = GetLowestCostTileFromOpenTiles();
            }
        }

        private void ProcessCurrentTile()
        {
            _openTiles.Remove(_currentTile);
            _closedTiles.Add(_currentTile);

            ConsiderNeighbouringTiles(_currentTile);
        }

        private void UpdateOpenTileCosts()
        {
            foreach (var tile in _openTiles)
            {
                tile.MovementCostFromStartingPoint = tile.ParentSwampTile.MovementCostFromStartingPoint + 10;

                var estimatedHorizontalMovementToDestination = Math.Abs(_destinationTile.Xpos - tile.Xpos);
                var estimatedVerticalMovementToDestination = Math.Abs(_destinationTile.Ypos - tile.Ypos);
                tile.EstimatedMovementCostToDestination = (estimatedVerticalMovementToDestination
                                                          + estimatedHorizontalMovementToDestination) * 10;
            }
        }

        private SwampTile GetLowestCostTileFromOpenTiles()
        {
            return _openTiles.OrderByDescending(p => p.Cost).First();
        }

        internal void ConsiderNeighbouringTiles(SwampTile tile)
        {
            var x = tile.Xpos;
            var y = tile.Ypos;

            var northTile = y - 1 >= 0 && _tileService.TilePassable(_mapService.Map[x, y - 1])
                                ? _mapService.Map[x, y - 1]
                                : null;
            var southTile = y + 1 <= _mapService.Height && _tileService.TilePassable(_mapService.Map[x, y + 1])
                                ? _mapService.Map[x, y + 1]
                                : null;
            var westTile = x - 1 >= 0 && _tileService.TilePassable(_mapService.Map[x - 1, y])
                               ? _mapService.Map[x - 1, y]
                               : null;

            var eastTile = x + 1 >= _mapService.Width && _tileService.TilePassable(_mapService.Map[x + 1, y])
                               ? _mapService.Map[x + 1, y]
                               : null;

            if (northTile != null)
            {
                ConsiderTile(northTile);
            }
            if (southTile != null)
            {
                ConsiderTile(southTile);
            }
            if (westTile != null)
            {
                ConsiderTile(westTile);
            }
            if (eastTile != null)
            {
                ConsiderTile(eastTile);
            }
        }

        internal void ConsiderTile(SwampTile tile)
        {
            if (_currentTile == _startingTile)
            {
                tile.ParentSwampTile = _currentTile;
                _openTiles.Add(tile);
            }
            else
            {
                if (_openTiles.Contains(tile))
                {
                    if (tile.MovementCostFromStartingPoint > _currentTile.MovementCostFromStartingPoint + 10)
                    {
                        tile.ParentSwampTile = _currentTile;
                    }
                }
                _openTiles.Add(tile);
            }
        }
    }
}
