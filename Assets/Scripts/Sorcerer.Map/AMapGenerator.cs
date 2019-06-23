namespace Sorcerer.Map
{
    public abstract class AMapGenerator<T> where T : IMapGenerationOptions
    {
        public Map map { get; private set; }
        public T options { get; private set; }

        public Cell[,] cells { get { return map.cells; } }

        public AMapGenerator(Map map, T options)
        {
            this.map = map;
            this.options = options;
        }

        public abstract void Populate();
    }
}