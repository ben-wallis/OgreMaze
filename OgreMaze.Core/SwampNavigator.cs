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
        private List<SwampTile> _destinationTiles = new List<SwampTile>();
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
            _destinationTiles.Add(_destinationTile);

            // Ogres are fat, add up to 3 possible tiles that will make him touch the gold
            if (_destinationTile.Xpos - 1 >= 0)
            {
                _destinationTiles.Add(_mapService.Map[_destinationTile.Xpos - 1, _destinationTile.Ypos]);
                if (_destinationTile.Ypos - 1 >= 0)
                {
                    _destinationTiles.Add(_mapService.Map[_destinationTile.Xpos - 1, _destinationTile.Ypos - 1]);
                }
            }
            if (_destinationTile.Ypos - 1 >= 0)
            {
                _destinationTiles.Add(_mapService.Map[_destinationTile.Xpos, _destinationTile.Ypos - 1]);
            }
            
            _currentTile = _startingTile;
            _openTiles.Add(_startingTile);
            
            while (!_closedTiles.Any(c => _destinationTiles.Any(d => d == c)))
            {
                _currentTile = GetLowestCostTileFromOpenTiles();
                _openTiles.Remove(_currentTile);
                _closedTiles.Add(_currentTile);

                ConsiderNeighbouringTiles(_currentTile);

                UpdateOpenTileCosts();
            }

            _currentTile = _closedTiles.First(c => _destinationTiles.Any(d => d == c));


            DrawPath();
        }

        private void DrawPath()
        {
            while (_currentTile != _startingTile)
            {
                _mapService.Map[_currentTile.Xpos, _currentTile.Ypos].SwampTileType = TileType.Ogre;
                _currentTile = _currentTile.ParentSwampTile;
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
            catch (Exception e)
            {
                Console.WriteLine("Ran out of open tiles");
                DrawPath();
                return _currentTile;
            }
        }

        private bool OgreCanFitInTile(SwampTile tile)
        {
            var x = tile.Xpos;
            var y = tile.Ypos;

            if (tile == _destinationTile)
            {
                return true;
            }

            if (x + 1 > _mapService.Width || y + 1 > _mapService.Height)
            {
                return false;
            }

            var eastTilePassable = _tileService.TilePassable(_mapService.Map[x + 1, y]);
            var southTilePassable = _tileService.TilePassable(_mapService.Map[x, y + 1]);
            var southEastTilePassable = _tileService.TilePassable(_mapService.Map[x + 1, y + 1]);
            return (eastTilePassable && southTilePassable && southEastTilePassable);
        }

        internal void ConsiderNeighbouringTiles(SwampTile tile)
        {
            var x = tile.Xpos;
            var y = tile.Ypos;

            var northTile = y - 1 >= 0 && _tileService.TilePassable(_mapService.Map[x, y - 1]) &&
                            !_closedTiles.Contains(_mapService.Map[x, y - 1]) &&
                            OgreCanFitInTile(_mapService.Map[x, y - 1])
                ? _mapService.Map[x, y - 1]
                : null;
            var southTile = y + 1 <= _mapService.Height && _tileService.TilePassable(_mapService.Map[x, y + 1]) &&
                            !_closedTiles.Contains(_mapService.Map[x, y + 1]) &&
                            OgreCanFitInTile(_mapService.Map[x, y + 1])
                ? _mapService.Map[x, y + 1]
                : null;
            var westTile = x - 1 >= 0 && _tileService.TilePassable(_mapService.Map[x - 1, y]) &&
                           !_closedTiles.Contains(_mapService.Map[x - 1, y]) &&
                           OgreCanFitInTile(_mapService.Map[x - 1, y])
                ? _mapService.Map[x - 1, y]
                : null;

            var eastTile = x + 1 <= _mapService.Width && _tileService.TilePassable(_mapService.Map[x + 1, y]) &&
                           !_closedTiles.Contains(_mapService.Map[x + 1, y]) &&
                           OgreCanFitInTile(_mapService.Map[x + 1, y])
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
    }
}
