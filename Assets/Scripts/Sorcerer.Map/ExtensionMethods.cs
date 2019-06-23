using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sorcerer.Map
{
    public static class ExtensionMethods
    {
        public static bool Intersect(this RectInt rect, RectInt other)
        {
            return rect.xMin <= other.xMax && rect.xMax >= other.xMin &&
                   rect.yMin <= other.yMax && rect.yMax >= other.yMin;
        }

        public static bool IntersectAny(this RectInt rect, IEnumerable<RectInt> others)
        {
            foreach (RectInt other in others)
                if (rect.Intersect(other))
                    return true;
            return false;
        }

        public static Vector2Int Center(this RectInt rect)
        {
            return new Vector2Int((int)Mathf.Floor(rect.center.x), (int)Mathf.Floor(rect.center.y));
        }
    }
}