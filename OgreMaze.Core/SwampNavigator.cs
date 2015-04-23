using System.Collections.Generic;
using OgreMaze.Core.Services;

namespace OgreMaze.Core
{
    internal class SwampNavigator
    {
        private readonly IMapService _mapService;
        private readonly ITileService _tileService;
        private SwampTile _startingTile;
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
            _startingTile = _mapService.FindStartingTile();
            _currentTile = _startingTile;

            ProcessCurrentTile();
            
        }

        private void ProcessCurrentTile()
        {
            FindNeighbouringOpenTiles(_currentTile);
            if (_openTiles.Contains(_currentTile))
            {
                _openTiles.Remove(_currentTile);
            }
            _closedTiles.Add(_currentTile);
        }

        private void UpdateOpenTileCosts()
        {
            foreach (var tile in _openTiles)
            {
                // TODO: update tile costs
            }
        }

        internal void FindNeighbouringOpenTiles(SwampTile tile)
        {
            var x = tile.Xpos;
            var y = tile.Ypos;

            if (y - 1 >= 0 && _tileService.TilePassable(_mapService.Map[x, y - 1].SwampTileType))
            {
                AddToOpenTiles(x, y - 1);
                SetParentTileToCurrentTile(x, y - 1);
            }
            if (y + 1 <= _mapService.Height && _tileService.TilePassable(_mapService.Map[x, y + 1].SwampTileType))
            {
                AddToOpenTiles(x, y + 1);
                SetParentTileToCurrentTile(x, y + 1);
            }
            if (x - 1 >= 0 && _tileService.TilePassable(_mapService.Map[x - 1, y].SwampTileType))
            {
                AddToOpenTiles(x - 1, y);
                SetParentTileToCurrentTile(x - 1, y);
            }
            if (x + 1 <= _mapService.Width && _tileService.TilePassable(_mapService.Map[x + 1, y].SwampTileType))
            {
                AddToOpenTiles(x + 1, y);
                SetParentTileToCurrentTile(x + 1, y);
            }
        }

        private void AddToOpenTiles(int x, int y)
        {
            _openTiles.Add(_mapService.Map[x, y]);
        }

        private void SetParentTileToCurrentTile(int x, int y)
        {
            _mapService.Map[x, y].ParentSwampTile = _currentTile;
        }

    }
}
