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

    public Point A;
    public Point B;

    public float Weight;

    // Update is called once per frame
    void Update() {
        //if (Input.GetKeyDown(KeyCode.T)) {
        //    Point Ap = GameManager.INSTANCE.levelLoader.TileGrid[ax][ay].GetComponent<Point>();
        //    Point Bp = GameManager.INSTANCE.levelLoader.TileGrid[bx][by].GetComponent<Point>();
        //    Debug.Log(Ap); Debug.Log(Bp);
        //    List<Point> path = new List<Point>();
        //    StartCoroutine(aStar(Ap,Bp,path));
        //}
        //if (Input.GetKeyDown(KeyCode.W)) {
        //    Debug.Log(A); Debug.Log(B);
        //    List<Point> path = new List<Point>();
        //    StartCoroutine(aStar(A, B, path));
        //}
    }

    int distBetweenPoints(Point a, Point b) {
        return (int)Mathf.Sqrt(Mathf.Pow(a.X - b.X,2) + Mathf.Pow(a.Y - b.Y,2));
    }

    Point getNextPoint(HashSet<Point> openSet, Dictionary<Point,int> fScore) {
        int minScore = int.MaxValue;
        Point closest = null;
        foreach (Point p in openSet) {
            if (fScore[p] < minScore) {
                closest = p;
                minScore = fScore[p];
            }
        }
        return closest;
    }

    IEnumerator aStar(Point start, Point goal, List<Point> path) {
        GameManager.INSTANCE.levelLoader.SetColors(start.isWaypoint);
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
            if (current == goal) {
                StartCoroutine(reconstructPath(cameFrom, current, path));
                yield break;
            }

            openSet.Remove(current);
            closedSet.Add(current);
            current.SR.color = exploredColor;
            Debug.Log(current);
            Debug.Log(current.Neighbors.Count);
            foreach (Point neighbor in current.Neighbors) {
                Debug.Log("Neighbor"); Debug.Log(neighbor);
                if (closedSet.Contains(neighbor)){
                    continue;       // Ignore the neighbor which is already evaluated.
                }

                if (!openSet.Contains(neighbor) && neighbor.Type == WALKABLE) { // Discover a new node
                    neighbor.SR.color = inQueueColor;
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
            if (breakout%10 == 0) {
                yield return null;
            }
        }
    }

    IEnumerator reconstructPath(Dictionary<Point,Point> cameFrom, Point current, List<Point> path) {
        path.Add(current);
        while (cameFrom.ContainsKey(current)) {
            
            current = cameFrom[current];
            path.Insert(0, current);
        }
        foreach(Point p in path) {
            p.SR.color = pathColor;
            yield return new WaitForSeconds(pathDelay / 4);
        }
        
    }
}
