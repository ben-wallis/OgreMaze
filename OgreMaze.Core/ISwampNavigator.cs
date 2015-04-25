namespace OgreMaze.Core
{
    public interface ISwampNavigator
    {
        bool GenerateMapAndNavigate(int width, int height);
        void NavigateMap(string mapFile);
        void DrawPath();
    }
}