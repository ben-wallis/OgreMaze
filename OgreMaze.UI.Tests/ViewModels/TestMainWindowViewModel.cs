using Moq;
using NUnit.Framework;
using OgreMaze.Core;
using OgreMaze.Core.Services;
using OgreMaze.UI.ViewModels;

namespace OgreMaze.UI.Tests.ViewModels
{
    [TestFixture]
    public class TestMainWindowViewModel
    {
        private MainWindowViewModelTestUtility _testUtility;

        [SetUp]
        public void MainWindowViewModelTestSetup()
        {
            _testUtility = new MainWindowViewModelTestUtility();
        }

        [Test]
        public void GenerateMapCommand_CallsSwampNavigatorGenerateMapAndNavigate()
        {
            // Arrange
            
            // Act
            _testUtility.TestViewModel.GenerateMapCommand.Execute(null);

            // Assert
            _testUtility.MockSwampNavigator.Verify(s => s.GenerateMapAndNavigate(_testUtility.TestMapWidth, _testUtility.TestMapHeight));
            _testUtility.MockMazeControlViewModel.Verify();
        }

        [Test]
        public void ShowPathCommand_CallsSwampNavigatorDrawPath()
        {
            // Arrange
            _testUtility.MockSwampNavigator.Setup(s => s.DrawPath()).Verifiable();

            // Act
            _testUtility.TestViewModel.ShowPathCommand.Execute(null);

            // Assert
            _testUtility.MockSwampNavigator.Verify(s => s.DrawPath());
        }

        private class MainWindowViewModelTestUtility
        {
            public MainWindowViewModelTestUtility()
            {
                // Test data setup
                TestMapWidth = 5;
                TestMapHeight = 10;
                TestMap = new SwampTile[TestMapWidth, TestMapHeight];

                // Mock setups
                MockMapService = new Mock<IMapService>();
                MockSwampNavigator = new Mock<ISwampNavigator>();
                MockMazeControlViewModel = new Mock<IMazeControlViewModel>();

                MockSwampNavigator.Setup(s => s.GenerateMapAndNavigate(TestMapWidth, TestMapHeight)).Verifiable();
                MockMapService.SetupGet(m => m.Map).Returns(TestMap).Verifiable();
                MockMazeControlViewModel.SetupSet(v => v.SwampTiles = TestMap).Verifiable();

                // Class under test instantiation
                TestViewModel = new MainWindowViewModel(MockSwampNavigator.Object, MockMazeControlViewModel.Object,
                    MockMapService.Object) {GenerateMapWidth = TestMapWidth, GenerateMapHeight = TestMapHeight};
            }

            public Mock<IMapService> MockMapService { get; private set; }
            public Mock<ISwampNavigator> MockSwampNavigator { get; private set; }
            public Mock<IMazeControlViewModel> MockMazeControlViewModel { get; private set; }

            public MainWindowViewModel TestViewModel { get; private set; }
            public int TestMapHeight { get; set; }
            public int TestMapWidth { get; set; }

            public SwampTile[,] TestMap;
        }
    }
}
