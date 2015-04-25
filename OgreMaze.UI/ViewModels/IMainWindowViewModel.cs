using System.ComponentModel;
using System.Windows.Input;

namespace OgreMaze.UI.ViewModels
{
    public interface IMainWindowViewModel
    {
        IMazeControlViewModel MazeControlViewModel { get; }
        int GenerateMapHeight { get; set; }
        int GenerateMapWidth { get; set; }
        bool PathFound { get; }
        ICommand GenerateMapCommand { get; }
        ICommand ShowPathCommand { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}