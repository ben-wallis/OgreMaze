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
        bool ShowPath { get; }
        ICommand GenerateMapCommand { get; }
        ICommand LoadMapFromFileCommand { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}