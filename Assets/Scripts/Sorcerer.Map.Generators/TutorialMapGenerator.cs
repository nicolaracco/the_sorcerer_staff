using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sorcerer.Map.Generators
{
    public struct TutorialMapGenerationOptions : IMapGenerationOptions
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public int maxRooms { get; private set; }
        public int roomMinSize { get; private set; }
        public int roomMaxSize { get; private set; }
        public int? seed { get; private set; }

        public TutorialMapGenerationOptions(int width, int height, int maxRooms, int roomMinSize, int roomMaxSize, int? seed = null)
        {
            this.width = width;
            this.height = height;
            this.maxRooms = maxRooms;
            this.roomMinSize = roomMinSize;
            this.roomMaxSize = roomMaxSize;
            this.seed = seed;
        }
    }

    public class TutorialMapGenerator : AMapGenerator<TutorialMapGenerationOptions>
    {
        public TutorialMapGenerator(Map map, TutorialMapGenerationOptions options)
            : base(map, options)
        {}

        public override void Populate()
        {
            System.Random rnd = options.seed.HasValue 
                ? new System.Random(options.seed.Value) 
                : new System.Random();
            List<RectInt> rooms = new List<RectInt>();
            for (int i = 0; i < options.maxRooms; i++)
            {
                int w = rnd.Next(options.roomMinSize, options.roomMaxSize);
                int h = rnd.Next(options.roomMinSize, options.roomMaxSize);
                int x = rnd.Next(0, options.width - w - 1);
                int y = rnd.Next(0, options.height - h - 1);
                RectInt newRoom = new RectInt(x, y, w, h);
                if (!newRoom.IntersectAny(rooms))
                {
                    createRoom(newRoom);
                    Vector2Int newRoomCenter = newRoom.Center();
                    if (rooms.Count == 0)
                        map.AddEntity(new PlayerEntity(map, newRoomCenter));
                    else
                    {
                        Vector2Int prevRoomCenter = rooms[rooms.Count - 1].Center();
                        if (rnd.Next(1) == 1)
                        {
                            createHorizontalTunnel(prevRoomCenter.x, newRoomCenter.x, prevRoomCenter.y);
                            createVerticalTunnel(prevRoomCenter.y, newRoomCenter.y, newRoomCenter.x);
                        } else
                        {
                            createVerticalTunnel(prevRoomCenter.y, newRoomCenter.y, prevRoomCenter.x);
                            createHorizontalTunnel(prevRoomCenter.x, newRoomCenter.x, newRoomCenter.y);
                        }
                    }
                    rooms.Add(newRoom);
                }
            }
        }

        private void createRoom(RectInt area)
        {
            for (int x = area.xMin + 1; x < area.xMax; x++)
                for (int y = area.yMin + 1; y < area.yMax; y++)
                {
                    Cell cell = map.CellAt(x, y);
                    cell.isMovementBlocked = cell.isSightBlocked = false;
                }
        }

        private void createHorizontalTunnel(int x1, int x2, int y)
        {
            for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            {
                Cell cell = map.CellAt(x, y);
                cell.isMovementBlocked = cell.isSightBlocked = false;
            }
        }

        private void createVerticalTunnel(int y1, int y2, int x)
        {
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                Cell cell = map.CellAt(x, y);
                cell.isMovementBlocked = cell.isSightBlocked = false;
            }
        }
    }
}