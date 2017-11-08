using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class LevelLoader : MonoBehaviour {
    public LevelLoader LL;
    public GameObject Tile;
    public GameObject Waypoint;
    public TextAsset Map;
    public Pathfinder PF;
    public GameObject[][] TileGrid;
    public HashSet<GameObject> WaypointGrid;
    public string GUI_Type;

    public int ax;
    public int ay;
    public int bx;
    public int by;

    char[][] grid;
    int height;
    int width;
    public string H_Type;

    // Use this for initialization
    void Start ()
    {
        GUI_Type = "Tile";
        List<string> mapLines = new List<string>(Map.text.Split('\n'));

        string tmp;

        //TODO: save type
        mapLines.RemoveAt(0);

        tmp = mapLines[0].Substring(7);
        height = int.Parse(tmp);

        mapLines.RemoveAt(0);

        tmp = mapLines[0].Substring(6);
        width = int.Parse(tmp);

        mapLines.RemoveAt(0);
        mapLines.RemoveAt(0);

        grid = new char[height][];
        TileGrid = new GameObject[height][];
        for (int row =0; row<height; row++) {
            grid[row] = new char[width];
            TileGrid[row] = new GameObject[width];
        }

        int i = 0;
        foreach (string line in mapLines){
            if (line != null)
            {
                if (line.Length>0)
                {
                    char[] entries = line.ToCharArray();
                    grid[i] = entries;
                    ++i;
                } 
            }
        }

        float y = 0;
        for (int j = 0; j < height; ++j) {
            float x = 0;
            for (int u = 0; u < width; ++u) {
                char t = grid[j][u];
                GameObject go = CreatePoint(j, u, x, y, t);
                Point p = go.GetComponent<Point>();
                p.X = j; p.Y = u; p.Type = t;
                TileGrid[j][u] = (go);
                x += 1.025f;
            }
            y -= 1.025f;
        }

        GenerateWaypoints();
        SetColors(false);
    }

    GameObject CreatePoint(int j, int uT, float x, float y, char t)
    {
        GameObject point = Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
        TileInteract TI = point.GetComponent<TileInteract>();
        //TI.LL = this;
        //TI.PF = PF;
        return point;
    }

    GameObject CreateWaypoint(int x, int y) {
        float nx = -0.5125f + x * -1.025f;
        float ny = 0.5125f + y * 1.025f;
        GameObject point = Instantiate(Waypoint, new Vector3(ny, nx, 0), Quaternion.identity);
        Point p = point.GetComponent<Point>();
        p.X = x;
        p.Y = y;
        p.isWaypoint = true;
        return point;
    }

    void GenerateWaypoints() {
        WaypointGrid = new HashSet<GameObject>();
        for (int x = 0; x < height - 1; x++) {
            for (int y = 0; y < width - 1; y++) {
                int c = 0;
                if (grid[x][y] == '.') {
                    c++;
                }
                if (grid[x + 1][y] == '.') {
                    c++;
                }
                if (grid[x][y + 1] == '.') {
                    c++;
                }
                if (grid[x + 1][y + 1] == '.') {
                    c++;
                }
                if (c == 3) {
                    WaypointGrid.Add(CreateWaypoint(x, y));
                }
            }
        }
        GenerateEdges();
    }

    void GenerateEdges() {
        HashSet<GameObject> used = new HashSet<GameObject>();
        foreach (GameObject w in WaypointGrid) {
            used.Add(w);
            foreach(GameObject other in WaypointGrid) {
                if (used.Contains(other)) {
                    continue;
                }
                Point way = w.GetComponent<Point>();
                Point o = other.GetComponent<Point>();
                if (LineOfSight(way, o)) {
                    way.AddNeighbor(o);
                    o.AddNeighbor(way);
                    //Debug.DrawLine(way.transform.position, o.transform.position, Color.blue, Mathf.Infinity);
                }
            }
        }
    }

    int GCD(int a, int b) {
        while (b > 0) {
            int rem = a % b;
            a = b;
            b = rem;
        }
        return a;
    }

    bool LineOfSight(Point a, Point b) {
        int xDiff = b.X - a.X;
        int yDiff = b.Y - a.Y;
        int gcd = Mathf.Max(GCD(xDiff, yDiff),1);
        xDiff /= gcd;
        yDiff /= gcd;
        int startX = a.X + (xDiff > 0 ? 1 : 0);
        int startY = a.Y + (yDiff > 0 ? 1 : 0);
        int loops = 0;
        int curX = startX;
        int curY = startY;
        Point p;

        if (GameManager.INSTANCE.levelLoader.TileGrid[startX][startY].GetComponent<Point>().Type != '.') {
            return false;
        } if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff)) {
            while (loops < 10000) {
                loops++;
                for (int y = 0; y < Mathf.Abs(yDiff); y++) {
                    curY += yDiff > 0 ? 1 : -1;
                    p = GameManager.INSTANCE.levelLoader.TileGrid[curX][curY].GetComponent<Point>();
                    if (p.Type != '.') {
                        loops = 10000;
                        break;
                    }
                    if (isAdjacent(curX, curY, b)) {
                        return true;
                    }
                }
                if (loops == 10000) {
                    break;
                }
                for (int x = 0; x < Mathf.Abs(xDiff); x++) {
                    curX += xDiff > 0 ? 1 : -1;
                    p = GameManager.INSTANCE.levelLoader.TileGrid[curX][curY].GetComponent<Point>();
                    if (p.Type != '.') {
                        loops = 10000;
                        break;
                    }
                    if (isAdjacent(curX, curY, b)) {
                        return true;
                    }
                }
            }
        }
        else {
            while (loops < 10000) {
                loops++;
                for (int x = 0; x < Mathf.Abs(xDiff); x++) {
                    curX += xDiff > 0 ? 1 : -1;
                    p = GameManager.INSTANCE.levelLoader.TileGrid[curX][curY].GetComponent<Point>();
                    if (p.Type != '.') {
                        loops = 10000;
                        break;
                    }
                    if (isAdjacent(curX, curY, b)) {
                        Debug.Log(curX); Debug.Log(curY); Debug.Log(p);
                        return true;
                    }
                }
                if (loops == 10000) {
                    break;
                }
                for (int y = 0; y < Mathf.Abs(yDiff); y++) {
                    curY += yDiff > 0 ? 1 : -1;
                    p = GameManager.INSTANCE.levelLoader.TileGrid[curX][curY].GetComponent<Point>();
                    if (p.Type != '.') {
                        loops = 10000;
                        break;
                    }
                    if (isAdjacent(curX, curY, b)) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool isAdjacent(int x, int y, Point p) {
        return (p.X == x || p.X == x + 1) && (p.Y == y || p.Y == y + 1);
    }

    public void SetColors(bool waypoints) {
        Color wColor = waypoints ? Color.grey : Color.red;
        Color tColor = Color.black;
        foreach (GameObject[] array in TileGrid) {
            foreach(GameObject tile in array) {
                if (tile != null)
                {
                    Point p = tile.GetComponent<Point>();
                    switch (tile.GetComponent<Point>().Type)
                    {
                        case '@':
                            p.SR.enabled = false;
                            break;
                        case 'T':
                            p.SR.color = tColor;
                            break;
                        case '.':
                            p.SR.color = wColor;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        if (waypoints) {
            HashSet<Point> used = new HashSet<Point>();
            foreach (GameObject g in WaypointGrid) {
                if (g != null)
                {
                    Point p = g.GetComponent<Point>();
                    used.Add(p);
                    p.SR.color = Color.red;
                    foreach (Point o in p.Neighbors)
                    {
                        if (used.Contains(o))
                        {
                            continue;
                        }
                        Debug.DrawLine(p.transform.position, o.transform.position, Color.red, 10f);
                    }
                }
            }
        }
       
    }

    private void Update() {
        if (GUI_Type == "Waypoint") {
            SetColors(true);
        } else {
            SetColors(false);
        }
    }
}
