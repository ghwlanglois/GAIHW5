using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    char[][] world;

    const char WALKABLE = '.';
    const char OUT_OF_BOUNDS = '@';
    const char TREE = 'T';

    public float pathDelay = .5f;

    public Color inQueueColor = Color.yellow;
    public Color exploredColor = Color.blue;
    public Color pathColor = Color.green;

    public int ax;
    public int ay;
    public int bx;
    public int by;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            Point A = GameManager.INSTANCE.levelLoader.TileGrid[ax][ay].GetComponent<Point>();
            Point B = GameManager.INSTANCE.levelLoader.TileGrid[bx][by].GetComponent<Point>();
            Debug.Log(A); Debug.Log(B);
            List<Point> path = new List<Point>();
            StartCoroutine(aStar(A,B,path));
        }
    }

    int distBetweenPoints(Point a, Point b) {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }

    Point getNextPoint(HashSet<Point> openSet, Dictionary<Point,int> fScore) {
        int minScore = int.MaxValue;
        Point closest = null;
        foreach (Point p in fScore.Keys) {
            if (fScore[p] < minScore && openSet.Contains(p)) {
                closest = p;
                minScore = fScore[p];
            }
        }
        return closest;
    }

    IEnumerator aStar(Point start, Point goal, List<Point> path) {
        GameManager.INSTANCE.levelLoader.ResetColors();
        // The set of nodes already evaluated
        HashSet<Point> closedSet = new HashSet<Point>();

        // The set of currently discovered nodes that are not evaluated yet.
        // Initially, only the start node is known.
        HashSet<Point> openSet = new HashSet<Point>();
        openSet.Add(start);

        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

        // For each node, the cost of getting from the start node to that node.
        Dictionary<Point, int> gScore = new Dictionary<Point, int>();

        // The cost of going from start to start is zero.
        gScore[start] = 0;

        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        Dictionary<Point, int> fScore = new Dictionary<Point, int>();

        // For the first node, that value is completely heuristic.
        fScore[start] = distBetweenPoints(start, goal);
        int breakout = 0;
        while (openSet.Count > 0 && breakout < 10000) {
            breakout++;
            //the node in openSet having the lowest fScore[] value
            Point current = getNextPoint(openSet,fScore);
            Debug.Log("current");
            Debug.Log(current);
            if (current == goal) {
                StartCoroutine(reconstructPath(cameFrom, current, path));
                yield break;
            }

            openSet.Remove(current);
            closedSet.Add(current);
            GameManager.INSTANCE.levelLoader.TileGrid[current.X][current.Y].GetComponent<SpriteRenderer>().color = exploredColor;
            Debug.Log("Neighbors");
            for (int i = 0; i < 4; i++) {
                //TODO: actually get neighbors
                Point neighbor;
                try {
                    neighbor = GameManager.INSTANCE.levelLoader.TileGrid[current.X + (i % 2) * (i >= 2 ? -1 : 1)][current.Y + ((i + 1) % 2) * (i >= 2 ? -1 : 1)].GetComponent<Point>();
                } catch (System.Exception e) {
                    continue;
                }
                Debug.Log(neighbor);
                if (closedSet.Contains(neighbor)){
                    continue;       // Ignore the neighbor which is already evaluated.
                }

                if (!openSet.Contains(neighbor) && neighbor.Type == WALKABLE) { // Discover a new node
                    GameManager.INSTANCE.levelLoader.TileGrid[neighbor.X][neighbor.Y].GetComponent<SpriteRenderer>().color = inQueueColor;
                    openSet.Add(neighbor);
                }


                // The distance from start to a neighbor
                int tentative_gScore = gScore[current] + distBetweenPoints(current, neighbor);
                if (gScore.ContainsKey(neighbor) && tentative_gScore >= gScore[neighbor]) {
                    continue; // This is not a better path.
                }

                // This path is the best until now. Record it!
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentative_gScore;
                fScore[neighbor] = gScore[neighbor] + distBetweenPoints(neighbor, goal);
            }
            yield return new WaitForSeconds(pathDelay);
        }
    }

    IEnumerator reconstructPath(Dictionary<Point,Point> cameFrom, Point current, List<Point> path) {
        path.Add(current);
        while (cameFrom.ContainsKey(current)) {
            
            current = cameFrom[current];
            path.Insert(0, current);
        }
        foreach(Point p in path) {
            GameManager.INSTANCE.levelLoader.TileGrid[p.X][p.Y].GetComponent<SpriteRenderer>().color = pathColor;
            yield return new WaitForSeconds(pathDelay / 4);
        }
        
    }
}
