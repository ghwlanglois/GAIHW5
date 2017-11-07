using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    char[][] world;

    const char WALKABLE = '.';
    const char OUT_OF_BOUNDS = '@';
    const char TREE = 'T';

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    int distBetweenPoints(Point a, Point b) {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }

    Point getNextPoint(Dictionary<Point,int> fScore) {
        int minScore = int.MaxValue;
        Point closest = null;
        foreach (Point p in fScore.Keys) {
            if (fScore[p] < minScore) {
                closest = p;
                minScore = fScore[p];
            }
        }
        return closest;
    }

    List<Point> aStar(Point start, Point goal) {
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

        while (openSet.Count > 0) {
            //the node in openSet having the lowest fScore[] value
            Point current = getNextPoint(fScore);
            if (current == goal) {
                return reconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            for (int i = 0; i < 4; i++) {
                //TODO: actually get neighbors
                Point neighbor = null;
                if (closedSet.Contains(neighbor)){
                    continue;       // Ignore the neighbor which is already evaluated.
                }

                if (!openSet.Contains(neighbor) && neighbor.Type == WALKABLE) {	// Discover a new node
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
        }
        //There is no path
        return null;
    }

    List<Point> reconstructPath(Dictionary<Point,Point> cameFrom, Point current) {
        List<Point> path = new List<Point>();
        path.Add(current);
        while (cameFrom.ContainsKey(current)) {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}
