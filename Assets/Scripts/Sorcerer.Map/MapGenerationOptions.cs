namespace Sorcerer.Map
{
    public struct MapGenerationOptions : IMapGenerationOptions
    {
        public int width { get; private set; }
        public int height { get; private set; }

        public MapGenerationOptions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}