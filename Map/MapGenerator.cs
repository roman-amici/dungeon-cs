using System.Drawing;

namespace Map;



public class MapGenerator(uint width, uint height)
{
    public const uint NumRooms = 10;

    public const uint MaxRoomSize = 50;
    public const uint MinRoomSize = 5;

    private void Fill(DungeonMap<Tile> map, Tile tile, MapRect rect)
    {
        for (var x = rect.X; x < rect.X + rect.Width; x++)
        {
            for (var y = rect.Y; y < rect.Y + rect.Height; y++)
            {
                map.Map[x,y] = tile;
            }
        }
    }

    private void FillX(DungeonMap<Tile> map, Tile tile, MapCoord start, MapCoord end)
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

    private void FillY(DungeonMap<Tile> map, Tile tile, MapCoord start, MapCoord end)
    {
        var x = start.X;

        var startY = start.Y;
        var endY = end.Y;
        if (end.Y < start.Y)
        {
            startY = end.Y;
            endY = start.Y;
        }

        for (var y = start.Y; y <= end.Y; y++)
        {
            map.Map[x, y] = tile;
        }
    }

    public DungeonMap<Tile> Generate(Random random)
    {
        var map = new DungeonMap<Tile>(width, height);

        var rooms = new List<MapRect>();

        while (rooms.Count < NumRooms)
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

            if (rooms.Any(x => x.Intersects(room)))
            {
                continue;
            }

            rooms.Add(room);
        }

        foreach (var room in rooms)
        {
            Fill(map, Tile.Floor, room);
        }

        for (var i = 0; i < rooms.Count; i++)
        {
            var room1 = rooms[i];
            for (var j = i; j < rooms.Count; j++)
            {
                var room2 = rooms[j];
                var elbow = new MapCoord(room2.Center.X, room1.Y);
                FillX(map, Tile.Floor, room1.Center, elbow);
                FillY(map, Tile.Floor, elbow, room2.Center);
            }
        }

        return map;
    }
}