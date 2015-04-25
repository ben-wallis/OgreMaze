namespace OgreMaze.Core
{
    public interface ISwampNavigator
    {
        bool GenerateMapAndNavigate(int width, int height, int sinkholesPerHundred);
        bool NavigateMap(string mapFile);
        void DrawPath();
    }
}