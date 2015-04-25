using System.ComponentModel;
using OgreMaze.Core;

namespace OgreMaze.UI.ViewModels
{
    public interface IMazeControlViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;
        SwampTile[,] SwampTiles { get; set; }
        SwampTile[,] SwampTilesWithPath { get; set; }
        bool ShowPath { get; set; }
        int MazeWidth { get; set; }
        int MazeHeight { get; set; }
    }
}