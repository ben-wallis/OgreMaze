namespace OgreMaze.Core.Services
{
    internal interface IMapGenerationService
    {
        SwampTile[,] GenerateMap(int width, int height);
    }
}