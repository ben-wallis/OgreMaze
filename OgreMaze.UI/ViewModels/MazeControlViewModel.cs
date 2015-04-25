using System.ComponentModel;
using System.Runtime.CompilerServices;
using OgreMaze.Core;
using OgreMaze.UI.Annotations;

namespace OgreMaze.UI.ViewModels
{
    public class MazeControlViewModel : INotifyPropertyChanged, IMazeControlViewModel
    {
        private SwampTile[,] _swampTiles;
        private SwampTile[,] _swampTilesWithPath;
        private bool _showPath;
        private int _mazeWidth;
        private int _mazeHeight;

        public MazeControlViewModel()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SwampTile[,] SwampTiles
        {
            get { return _swampTiles; }
            set
            {
                _swampTiles = value;
                OnPropertyChanged();
            }
        }

        public SwampTile[,] SwampTilesWithPath
        {
            get { return _swampTilesWithPath; }
            set
            {
                _swampTilesWithPath = value;
                OnPropertyChanged();
            }
        }

        public int MazeWidth
        {
            get { return _mazeWidth; }
            set
            {
                _mazeWidth = value;
                OnPropertyChanged();
            }
        }

        public int MazeHeight
        {
            get { return _mazeHeight; }
            set
            {
                _mazeHeight = value;
                OnPropertyChanged();
            }
        }

        public bool ShowPath
        {
            get { return _showPath; }
            set
            {
                _showPath = value;
                OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
