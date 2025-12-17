using System.Drawing;

namespace Map;



public class MapGenerator(uint width, uint height)
{
    public const uint NumRooms = 10;

    public const uint MaxRoomSize = 50;
    public const uint MinRoomSize = 5;

    private void Fill(DungeonMap<MapTile> map, MapTile tile, MapRect rect)
    {
        for (var x = rect.X; x < rect.X + rect.Width; x++)
        {
            for (var y = rect.Y; y < rect.Y + rect.Height; y++)
            {
                map.Map[x,y] = tile;
            }
        }
    }

    private void FillX(DungeonMap<MapTile> map, MapTile tile, MapCoord start, MapCoord end)
    {
        var y = start.Y;

        var startX = start.X;
        var endX = end.X;
        if (end.X < start.X)
        {
            startX = end.X;
            endX = start.X;
        }

        for (var x = startX; x <= endX; x++)
            {
                map.Map[x, y] = tile;
            }
    }

    private void FillY(DungeonMap<MapTile> map, MapTile tile, MapCoord start, MapCoord end)
    {
        var x = start.X;

        var startY = start.Y;
        var endY = end.Y;
        if (end.Y < start.Y)
        {
            startY = end.Y;
            endY = start.Y;
        }

        for (var y = startY; y <= endY; y++)
        {
            map.Map[x, y] = tile;
        }
    }

    public DungeonMap<MapTile> Generate(Random random)
    {
        var map = new DungeonMap<MapTile>(width, height);

        while (map.Rooms.Count < NumRooms)
        {
            var room = new MapRect(
                (uint)random.Next(0, (int)width),
                (uint)random.Next(0, (int)width),
                (uint)random.Next((int)MinRoomSize, (int)MaxRoomSize),
                (uint)random.Next((int)MinRoomSize, (int)MaxRoomSize));

            if (room.IntervalX.End >= width || room.IntervalY.End >= height)
            {
                continue;
            }

            if (map.Rooms.Any(x => x.Intersects(room)))
            {
                continue;
            }

            map.Rooms.Add(room);
        }

        foreach (var room in map.Rooms)
        {
            Fill(map, MapTile.Floor, room);
        }

        for (var i = 0; i < map.Rooms.Count; i++)
        {
            var room1 = map.Rooms[i];
            for (var j = i; j < map.Rooms.Count; j++)
            {
                var room2 = map.Rooms[j];
                var elbow = new MapCoord(room2.Center.X, room1.Y);
                FillX(map, MapTile.Floor, room1.Center, elbow);
                FillY(map, MapTile.Floor, elbow, room2.Center);
            }
        }

        return map;
    }
}