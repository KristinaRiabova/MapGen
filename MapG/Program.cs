using Kse.Algorithms.Samples;

int height = 10;
int width = 30;

bool IsEquel(Point a, Point b)
{
    return a.Column == b.Column && a.Row == b.Row;
}


bool IsWall(string c)
{
    return c == "█";
}
int [] counter = new int[9];
void PrintMapWithPath(string[,] map, List<Point> path)
{
    Point start = path[0];
    Point end = path[^1];

    foreach (Point p in path)
    {
        if (IsEquel(p, start))
        {
            map[p.Column, p.Row] = "A";
        } 
        else if (IsEquel(p, end))
        {
            map[p.Column, p.Row] = "B";
        }
        else
        {
            switch (map[p.Column, p.Row])
            {
                case "1":
                    counter[0]++;
                    break;
                case "2":
                    counter[1]++;
                    break;
                case "3":
                    counter[2]++;
                    break;
                case "4":
                    counter[3]++;
                    break;
                case "5":
                    counter[4]++;
                    break;
                case "6":
                    counter[5]++;
                    break;
                case "7":
                    counter[6]++;
                    break;
                case "8":
                    counter[7]++;
                    break;
                case "9":
                    counter[8]++;
                    break;
            }
            map[p.Column, p.Row] = ".";
        }
    }

    for (int i = 0; i < counter.Length; i++)
    {
        Console.WriteLine("Symbol " + (i+1) + " in maze : " + counter[i] + " times");
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
        result.Add(new Point(px, py + 1));
    }
    if (py - 1 < map.GetLength(1) && py - 1 >= 0 && px < map.GetLength(0) && px >= 0 && !IsWall(map[px, py - 1]))
    {
        result.Add(new Point(px, py - 1));
    }
    if (py < map.GetLength(1) && py >= 0 && px + 1 < map.GetLength(0) && px + 1 >= 0 && !IsWall(map[px + 1, py]))
    {
        result.Add(new Point(px + 1, py));
    }
    if (py < map.GetLength(1) && py >= 0 && px - 1 < map.GetLength(0) && px - 1 >= 0 && !IsWall(map[px - 1, py]))
    {
        result.Add(new Point(px - 1, py));
    }

    return result;
}

List<Point> SearchDijkstra(string[,] map, Point start, Point end)
{
    
    List<Point> unvisited = new List<Point>();
    Dictionary<Point, float> distance = new Dictionary<Point, float>();
    
    Dictionary<Point, Point?> previous = new Dictionary<Point, Point?>();

    
    foreach (int i in Enumerable.Range(0, map.GetLength(0)))
    {
        foreach (int j in Enumerable.Range(0, map.GetLength(1)))
        {
            Point p = new Point(i, j);
            unvisited.Add(p);
            distance.Add(p, float.MaxValue);
            previous.Add(p, null);
        }
    }
    
    
    distance[start] = 0;

    
    while (unvisited.Count > 0)
    {
        
        Point currentPoint = unvisited.OrderBy(p => distance[p]).First();

        
        if (IsEquel(currentPoint, end))
        {
            break;
        }
    
        
        unvisited.Remove(currentPoint);
        
        
        foreach (Point neighbour in GetNeighbours(map, currentPoint))
        {
            
            float alt = distance[currentPoint] + 1; 
            
            
            if (alt < distance[neighbour])
            {
                
                distance[neighbour] = alt;
                previous[neighbour] = currentPoint;
            }
        }

    }
    
   
    List<Point> path = new List<Point>();
    Point? current = end;
    
    
    if (previous[current.Value] == null)
    {
        return path;
    }

    while (current != null)
    {
        
        path.Add(current.Value);
       
        current = previous[current.Value];
    }
    
    
    path.Reverse();
    Console.WriteLine(distance[end]-1);
    return path;
}
/*List<Point> SearchBFS(string[,] map, Point start, Point end)
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
);*/

    var generator = new MapGenerator(new MapGeneratorOptions()
    {
        Height = height,
        Width = width,
        Seed = 3234,
        Noise = 0.1f,
        AddTraffic = true,
        TrafficSeed = 34322264
    });

    string[,] map = generator.Generate();



double time = 0;

int[] neededKms = new int[9];

for (int i = 0; i < neededKms.Length; i++)
{
    neededKms[i] = 60 - ((i + 1) - 1) * 6;
}



    
    Point start = new Point(0, 0);
    Point end = new Point(29, 7);
    List<Point> path = SearchDijkstra(map, start, end);
    PrintMapWithPath(map, path);
    for (int i = 0; i < counter.Length; i++)
    {
        if (counter[i] == 0) continue;
        else
        {
            time += 60 * counter[i] / neededKms[i];
        }
    }
Console.WriteLine(time + " mins");
