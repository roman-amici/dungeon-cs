using System.Drawing;

namespace Map;

public static class MapGenerator
{
    private static void Fill(DungeonMap<MapTile> map, MapTile tile, MapRect rect)
    {
        for (var x = rect.X; x < rect.X + rect.Width; x++)
        {
            for (var y = rect.Y; y < rect.Y + rect.Height; y++)
            {
                map.Map[x,y] = tile;
            }
        }
    }

    private static void FillX(DungeonMap<MapTile> map, MapTile tile, MapCoord start, MapCoord end)
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

    private static void FillY(DungeonMap<MapTile> map, MapTile tile, MapCoord start, MapCoord end)
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


    public const uint NumRooms = 10;
    public const uint MaxRoomSize = 25;
    public const uint MinRoomSize = 5;
    public static DungeonMap<MapTile> GenerateRoomMap(uint width, uint height, Random random)
    {
        var map = new DungeonMap<MapTile>(width, height);

        var rooms = new List<MapRect>();
        while (rooms.Count < NumRooms)
        {
            var room = new MapRect(
                random.Next(0, (int)width),
                random.Next(0, (int)width),
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
            Fill(map, MapTile.Floor, room);
        }

        for (var i = 0; i < rooms.Count; i++)
        {
            var room1 = rooms[i];
            for (var j = i; j < rooms.Count; j++)
            {
                var room2 = rooms[j];
                if (random.Next(0,2) == 1)
                {
                    var elbow = new MapCoord(room2.Center.X, room1.Center.Y);
                    FillX(map, MapTile.Floor, room1.Center, elbow);
                    FillY(map, MapTile.Floor, elbow, room2.Center);
                }
                else
                {
                    var elbow = new MapCoord(room1.Center.X, room2.Center.Y);
                    FillY(map, MapTile.Floor, room1.Center, elbow);
                    FillX(map, MapTile.Floor, elbow, room2.Center);
                }
            }
        }

        foreach(var room in rooms.Skip(1))
        {
            map.EnemySpawns.Add(room.Center);
        }

        map.PlayerStart = rooms[0].Center;

        return map;
    }

    public static int CountNeighbors(DungeonMap<MapTile> map, MapTile tile, MapCoord coord)
    {
        var count = 0;
        for (var dx = -1; dx < 2; dx++)
        {
            for (var dy = -1; dy < 2; dy++)
            {
                if (dx == 0 && dy == 0)
                {
                    continue;
                }

                if (map.Map[coord.X+dx,coord.Y + dy] == tile)
                {
                    count++;
                }
            }
        }

        return count;
    }

    public static void CellularMapIterate(DungeonMap<MapTile> map)
    {
        var mapCopy = new DungeonMap<MapTile>((MapTile[,])map.Map.Clone());
        for(var x = 1; x < mapCopy.Map.GetLength(0) - 1; x++)
        {
            for (var y = 1; y < mapCopy.Map.GetLength(1) - 1; y++)
            {
                var neighbors = CountNeighbors(mapCopy, MapTile.Wall, new(x,y));
                if (neighbors > 4 | neighbors == 0)
                {
                    map.Map[x,y] = MapTile.Wall;
                }
                else
                {
                    map.Map[x,y] = MapTile.Floor;
                }
            }
        }
    }

    public static void PlacePlayerNearCenter(DungeonMap<MapTile> map)
    {
        for(var x = map.Map.GetLength(0) / 2; x < map.Map.GetLength(0) ; x++)
        {
            for (var y = map.Map.GetLength(1) / 2; y < map.Map.GetLength(1); y++)
            {
                if (map.Map[x,y] == MapTile.Floor)
                {
                    map.PlayerStart = new(x,y);
                    return;
                }
            }
        }
    }

    private static void PlaceEnemiesAwayFromPlayer(DungeonMap<MapTile> map, Random random)
    {
        var playerPoint = map.PlayerStart.ToPoint();
        while (map.EnemySpawns.Count < NumRooms)
        {
            var x = random.Next(1, map.Map.GetLength(0));
            var y = random.Next(1, map.Map.GetLength(1));

            if (map.Map[x,y] == MapTile.Floor && new Point2D(x,y).DistanceCartesian(playerPoint) > 10.0)
            {
                map.EnemySpawns.Add(new(x,y));
            }
        }
    }

    public static DungeonMap<MapTile> GenerateCellularAutomataMap(uint width, uint height, Random random)
    {
        var map = new DungeonMap<MapTile>(width, height);

        for(var x = 1; x < width - 1; x++)
        {
            for (var y = 1; y < height - 1; y++)
            {
                if (random.Next(0,100) >= 55)
                {
                    map.Map[x,y] = MapTile.Floor;
                }
            }
        }

        for (var i = 0; i < 10; i++)
        {
            CellularMapIterate(map);
        }

        PlacePlayerNearCenter(map);
        PlaceEnemiesAwayFromPlayer(map, random);

        return map;
    }
}