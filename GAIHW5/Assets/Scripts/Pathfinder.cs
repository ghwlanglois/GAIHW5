using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pathfinder : MonoBehaviour {

    char[][] world;

    const char WALKABLE = '.';
    const char OUT_OF_BOUNDS = '@';
    const char TREE = 'T';

    public float pathDelay = .5f;

    public Color inQueueColor = Color.yellow;
    public Color exploredColor = Color.blue;
    public Color pathColor = Color.green;

    public Slider slider;
    float Weight = .5f;

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

    public void SetWeight () {
        this.Weight = slider.value;
    }

    float distBetweenPoints(Point a, Point b) {
        if (GameManager.INSTANCE.distHeuristic) {
            return Mathf.Sqrt(Mathf.Pow(a.X - b.X,2) + Mathf.Pow(a.Y - b.Y,2));
        }
        return Mathf.Abs(b.X - a.X) + Mathf.Abs(b.Y - a.Y);
    }

    Point getNextPoint(HashSet<Point> openSet, Dictionary<Point,float> fScore) {
        float minScore = float.PositiveInfinity;
        Point closest = null;
        foreach (Point p in openSet) {
            if (fScore[p] < minScore) {
                closest = p;
                minScore = fScore[p];
            }
        }
        return closest;
    }

    public IEnumerator aStar(Point start, Point goal, List<Point> path) {
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
        Dictionary<Point, float> gScore = new Dictionary<Point, float>();

        // The cost of going from start to start is zero.
        gScore[start] = 0;

        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        Dictionary<Point, float> fScore = new Dictionary<Point, float>();
        Point last = null;
        // For the first node, that value is completely heuristic.
        fScore[start] = Weight * distBetweenPoints(start, goal);
        int breakout = 0;
        while (openSet.Count > 0 && breakout < 100000) {
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
            if (current.isWaypoint && last != null) {
                Debug.DrawLine(last.transform.position, current.transform.position, exploredColor, 10f);
            }
            foreach (Point neighbor in current.Neighbors) {
                Debug.Log("Neighbor"); Debug.Log(neighbor);
                if (closedSet.Contains(neighbor)){
                    continue;       // Ignore the neighbor which is already evaluated.
                }

                if (!openSet.Contains(neighbor) && neighbor.Type == WALKABLE) { // Discover a new node
                    neighbor.SR.color = inQueueColor;
                    openSet.Add(neighbor);
                    if (current.isWaypoint)
                        Debug.DrawLine(current.transform.position, neighbor.transform.position, inQueueColor, 10f);
                }


                // The distance from start to a neighbor
                float tentative_gScore = gScore[current] + distBetweenPoints(current, neighbor);
                if (gScore.ContainsKey(neighbor) && tentative_gScore >= gScore[neighbor]) {
                    continue; // This is not a better path.
                }

                // This path is the best until now. Record it!
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentative_gScore;
                fScore[neighbor] = (1-Weight) * gScore[neighbor] + Weight * distBetweenPoints(neighbor, goal);
            }
            if (breakout%10 == 0 || (current.isWaypoint && breakout %5==0)) {
                yield return null;
            }
            last = current;
        }
    }

    IEnumerator reconstructPath(Dictionary<Point,Point> cameFrom, Point current, List<Point> path) {
        path.Add(current);
        while (cameFrom.ContainsKey(current)) {
            
            current = cameFrom[current];
            path.Insert(0, current);
        }
        Point last = null;
        foreach(Point p in path) {
            p.SR.color = pathColor;
            if (p.isWaypoint&&last!=null) {
                Debug.DrawLine(last.transform.position, p.transform.position, pathColor, 10f);
            }
            last = p;
        }
        yield return null;
    }
}
