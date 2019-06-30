using UnityEngine;

namespace Sorcerer.Map.Generators
{
    public class BaseMapGenerator : AMapGenerator<MapGenerationOptions>
    {
        public BaseMapGenerator(Map map, MapGenerationOptions options) : base(map, options) {}

        public override void Populate()
        {
            for (int x = 0; x < options.width; x++)
                for (int y = 0; y < options.height; y++)
                {
                    Cell cell = map.CellAt(x, y);
                    cell.isMovementBlocked = cell.isSightBlocked = false;
                }
            map.AddEntity(new PlayerEntity(
                map, new Vector2Int(options.width / 2, options.height / 2)
            ));
            map.AddEntity(new Entity(
                map, "@", new Vector2Int(map.Width / 2 - 5, map.Height / 2)
            ));
        }
    }
}