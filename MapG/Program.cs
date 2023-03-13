using Kse.Algorithms.Samples;

bool IsEquel(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}

bool IsWall(string c)
{
    return c == "█";
}

void PrintMapWithPath(string[,] map, List<Point> path)
{
    Point start = path[0];
    Point end = path[^1];

    foreach (Point p in path)
    {
        if (IsEquel(p, start))
        {
            map[p.Column, p.Row] = "A";
        } else if (IsEquel(p, end))
        {
            map[p.Column, p.Row] = "B";
        }
        else
        {
            map[p.Column, p.Row] = ".";
        }
    }
    new MapPrinter().Print(map);
}

List<Point> GetNeighbours(string[,] map, Point p)
{
    List<Point> result = new List<Point>();
    int px = p.Column;
    int py = p.Row;
    
    if (py + 1 < map.GetLength(1) && py + 1 >= 0 && px < map.GetLength(0) && px >= 0 && !IsWall(map[px, py + 1]))
    {
        result.Add(new Point(px , py+1));
    }
    if (py - 1 < map.GetLength(1) && py - 1 >= 0 && px < map.GetLength(0) && px >= 0 && !IsWall(map[px, py - 1]))
    {
        result.Add(new Point(px , py-1));
    }
    if (py < map.GetLength(1) && py >= 0 && px + 1 < map.GetLength(0) && px+1 >= 0 && !IsWall(map[px + 1, py]))
    {
        result.Add(new Point(px + 1 , py));
    }
    if (py < map.GetLength(1) && py >= 0 && px - 1 < map.GetLength(0) && px - 1 >= 0 && !IsWall(map[px - 1, py]))
    {
        result.Add(new Point(px - 1 , py));
    }
    return result;
}

List<Point> SearchBFS(string[,] map, Point start, Point end)
{
    Queue<Point> frontier = new Queue<Point>();
    Dictionary<Point, Point?> cameFrom = new Dictionary<Point, Point?>();
    cameFrom.Add(start,null);
    frontier.Enqueue(start);
    while (frontier.Count>0)
    {
        Point cur = frontier.Dequeue();
        if (IsEquel(cur,end))
        {
            break;
        }

        foreach (Point neighbour in GetNeighbours(map,cur))
        {
            if (!cameFrom.TryGetValue(neighbour, out _))
            {
              cameFrom.Add(neighbour,cur);  
              frontier.Enqueue(neighbour);
            }
        }
    }

    List<Point> path = new List<Point>();
    Point? current = end;
    while (!IsEquel(current.Value, start))
    {
        path.Add(current.Value);
        cameFrom.TryGetValue(current.Value, out current);
    }
    path.Add(start);
    path.Reverse();
    return path;
}
var generator = new MapGenerator(new MapGeneratorOptions()
{
    Height = 35,
    Width = 90,
    Seed = 12312,
    
});

string[,] map = generator.Generate();

/*List<Point> path = new List<Point>(new Point[]
    {
        new Point(0,0),
        new Point(1,0),
        new Point(2,0),
        new Point(3,0),
    }
);
Point p = new Point(8, 2);
map[8, 2] = "X";*
/*foreach (Point n in  GetNeighbours(map,p))
{
    map[n.Column, n.Row] = "M";
}*/
Point start = new Point(0, 0);
Point end = new Point(0, 2);
List<Point> path = SearchBFS(map, start, end);

PrintMapWithPath(map,path);
//PrintMapWithPath(map,path);
//new MapPrinter().Print(map);