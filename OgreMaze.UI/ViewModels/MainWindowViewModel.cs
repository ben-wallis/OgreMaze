using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using OgreMaze.Core;
using OgreMaze.Core.Services;
using OgreMaze.UI.Annotations;

namespace OgreMaze.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
    {
        private readonly ISwampNavigator _swampNavigator;
        private readonly IMapService _mapService;

        private int _generateMapHeight;
        private int _generateMapWidth;
        private int _generateMapPercentSinkholes;
        private bool _pathFound;
        private bool _showPath;

        public MainWindowViewModel(ISwampNavigator swampNavigator, IMazeControlViewModel mazeControlViewModel, IMapService mapService)
        {
            _swampNavigator = swampNavigator;
            _mapService = mapService;
            MazeControlViewModel = mazeControlViewModel;
            GenerateMapCommand = new RelayCommand(OnGenerateMap);
            LoadMapFromFileCommand = new RelayCommand(OnLoadMapFromFile);
            GenerateMapWidth = 20;
            GenerateMapHeight = 20;
            GenerateMapPercentSinkholes = 15;
        }

        public IMazeControlViewModel MazeControlViewModel { get; private set; }

        public ICommand GenerateMapCommand { get; private set; }
        public ICommand ShowPathCommand { get; private set; }
        public ICommand LoadMapFromFileCommand { get; private set; }

        public bool PathFound
        {
            get { return _pathFound; }
            private set
            {
                _pathFound = value;
                OnPropertyChanged();
            }
        }

        public bool ShowPath
        {
            get { return _showPath; }
            set
            {
                _showPath = value;
                if (value && MazeControlViewModel.SwampTilesWithPath == null)
                {
                    _swampNavigator.DrawPath();
                    MazeControlViewModel.SwampTilesWithPath = (SwampTile[,])_mapService.Map.Clone();
                }

                MazeControlViewModel.ShowPath = value;
                OnPropertyChanged();
            }
        }

        public int GenerateMapHeight
        {
            get { return _generateMapHeight; }
            set
            {
                _generateMapHeight = value;
                OnPropertyChanged();
            }
        }

        public int GenerateMapWidth
        {
            get { return _generateMapWidth; }
            set
            {
                _generateMapWidth = value;
                OnPropertyChanged();
            }
        }

        public int GenerateMapPercentSinkholes
        {
            get { return _generateMapPercentSinkholes; }
            set
            {
                _generateMapPercentSinkholes = value;
                OnPropertyChanged();
            }
        }

        public string MapFilePath { get; set; }


        private void OnGenerateMap()
        {
            PathFound = _swampNavigator.GenerateMapAndNavigate(GenerateMapWidth, GenerateMapHeight, GenerateMapPercentSinkholes);
            UpdateMazeControlViewModel();
        }

        private void OnLoadMapFromFile()
        {
            PathFound = _swampNavigator.NavigateMap(MapFilePath);
            UpdateMazeControlViewModel();
        }

        private void UpdateMazeControlViewModel()
        {
            ShowPath = false;
            MazeControlViewModel.SwampTiles = (SwampTile[,])_mapService.Map.Clone();
            MazeControlViewModel.SwampTilesWithPath = null;
            MazeControlViewModel.ShowPath = false;
            MazeControlViewModel.MazeWidth = _mapService.Width + 1;
            MazeControlViewModel.MazeHeight = _mapService.Height + 1;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
