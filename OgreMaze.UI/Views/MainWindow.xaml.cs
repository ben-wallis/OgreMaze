using OgreMaze.UI.ViewModels;

namespace OgreMaze.UI.Views
{
    public partial class MainWindow : IMainWindow
    {
        public MainWindow(IMainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
