using UnityEngine;

namespace Sorcerer.Map
{
    public class BaseMapGenerator : AMapGenerator<MapGenerationOptions>
    {
        public BaseMapGenerator(Map map, MapGenerationOptions options) : base(map, options) {}

        public override void Populate()
        {
            for (int x = 0; x < options.width; x++)
                for (int y = 0; y < options.height; y++)
                {
                    Cell cell = map.cells[x, y];
                    cell.isMovementBlocked = cell.isSightBlocked = false;
                }
            map.playerStartPosition = new Vector2Int(options.width / 2, options.height / 2);
        }
    }
}