namespace OgreMaze.Core
{
    internal interface ISwampNavigator
    {
        void GenerateMapAndNavigate(int width, int height);
        void NavigateMap(string mapFile);
    }
}