using System;
using Sorcerer.Map;
using UnityEngine;

namespace Sorcerer.Map
{
    /// <summary>
    /// Field of View layer. Greatly inspired from https://github.com/FaronBracy/RogueSharp/blob/master/RogueSharp/FieldOfView.cs
    /// </summary>
    public static class FieldOfView
    {
        private enum Quadrant { SE, NE, SW, NW }

        /// <summary>
        /// Compute field of view in the given map
        /// </summary>
        /// <param name="map">map that will receive the fov settings</param>
        /// <param name="origin">fov origin</param>
        /// <param name="radius">fov radius</param>
        public static void Compute(IMap map, Vector2Int origin, int radius)
        {
            ClearFovInCells(map);
            foreach (ICell borderCell in map.BorderCellsInSquare(origin, radius))
                foreach (ICell cell in map.CellsAlongLine(origin, borderCell.position))
                {
                    if (Math.Abs(Vector2Int.Distance(cell.position, origin)) > radius)
                        break;
                    if (!cell.isSightBlocked)
                        map.CellAt(cell.position).isInFov = true;
                    else
                    {
                        // light walls
                        map.CellAt(cell.position).isInFov = true;
                        break;
                    }
                }
            PostProcessFov(map, origin, radius);
        }

        /// <summary>
        /// Post processing step created based on the algorithm at this website:
        /// https://sites.google.com/site/jicenospam/visibilitydetermination
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        private static void PostProcessFov(IMap map, Vector2Int origin, int radius)
        {
            foreach (ICell cell in map.CellsInSquare(origin, radius))
            {
                if (cell.position.x > origin.x)
                {
                    if (cell.position.y > origin.y)
                        PostProcessFovQuadrant(map, cell, Quadrant.SE);
                    else if (cell.position.y < origin.y)
                        PostProcessFovQuadrant(map, cell, Quadrant.NE);
                }
                else if (cell.position.x < origin.x)
                {
                    if (cell.position.y > origin.y)
                        PostProcessFovQuadrant(map, cell, Quadrant.SW);
                    else if (cell.position.y < origin.y)
                        PostProcessFovQuadrant(map, cell, Quadrant.NW);
                }
            }
        }

        private static void PostProcessFovQuadrant(IMap map, ICell cell, Quadrant quadrant)
        {
            ICell c1, c2;
            switch (quadrant)
            {
                case Quadrant.NE:
                {
                    c1 = map.CellAt(cell.position + Vector2Int.up);
                    c2 = map.CellAt(cell.position + Vector2Int.left);
                    break;
                }
                case Quadrant.SE:
                {
                    c1 = map.CellAt(cell.position + Vector2Int.down);
                    c2 = map.CellAt(cell.position + Vector2Int.left);
                    break;
                }
                case Quadrant.SW:
                {
                    c1 = map.CellAt(cell.position + Vector2Int.down);
                    c2 = map.CellAt(cell.position + Vector2Int.right);
                    break;
                }
                case Quadrant.NW:
                {
                    c1 = map.CellAt(cell.position + Vector2Int.up);
                    c2 = map.CellAt(cell.position + Vector2Int.right);
                    break;
                }
                default:
                    throw new Exception("Unexpected quadrant");
            }
            ICell cx = map.CellAt(c2.position.x, c1.position.y);
            if (!cell.isInFov && cell.isSightBlocked)
                if ((!c1.isSightBlocked && c1.isInFov) || (!c2.isSightBlocked && c2.isInFov)
                    || (!cx.isSightBlocked && cx.isInFov))
                    cell.isInFov = true;
        }

        private static void ClearFovInCells(IMap map)
        {
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Height; y++)
                    map.CellAt(x, y).isInFov = false;
        }
    }
}