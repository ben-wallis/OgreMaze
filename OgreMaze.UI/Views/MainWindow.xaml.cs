using System.Windows;
using Microsoft.Win32;
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

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }
    }
}
