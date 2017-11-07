using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

    //char[][] world;

    //const char WALKABLE = '.';
    //const char OUT_OF_BOUNDS = '@';
    //const char TREE = 'T';

    //// Use this for initialization
    //void Start() {

    //}

    //// Update is called once per frame
    //void Update() {

    //}


    //void aStar(Point start, Point end) {
    //    // The set of nodes already evaluated
    //    HashSet<Point> closedSet = new HashSet<Point>();

    //    // The set of currently discovered nodes that are not evaluated yet.
    //    // Initially, only the start node is known.
    //    HashSet<Point> openSet = new HashSet<Point>();
    //    openSet.Add(start);

    //    // For each node, which node it can most efficiently be reached from.
    //    // If a node can be reached from many nodes, cameFrom will eventually contain the
    //    // most efficient previous step.
    //    Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

    //    // For each node, the cost of getting from the start node to that node.
    //    Dictionary<Point, int> gScore = new Dictionary<Point, int>();

    //    // The cost of going from start to start is zero.
    //    gScore[start] = 0;

    //    // For each node, the total cost of getting from the start node to the goal
    //    // by passing by that node. That value is partly known, partly heuristic.
    //    Dictionary<Point, int> fScore = map with default value of Infinity

    //    // For the first node, that value is completely heuristic.
    //    fScore[start] := heuristic_cost_estimate(start, goal)

    //while openSet is not empty
    //    current := the node in openSet having the lowest fScore[] value
    //    if current = goal
    //        return reconstruct_path(cameFrom, current)

    //    openSet.Remove(current)
    //    closedSet.Add(current)

    //    for each neighbor of current
    //        if neighbor in closedSet
    //            continue		// Ignore the neighbor which is already evaluated.

    //        if neighbor not in openSet	// Discover a new node
    //            openSet.Add(neighbor)

    //        // The distance from start to a neighbor
    //    tentative_gScore:= gScore[current] + dist_between(current, neighbor)
    //        if tentative_gScore >= gScore[neighbor]
    //            continue		// This is not a better path.

    //        // This path is the best until now. Record it!
    //        cameFrom[neighbor] := current
    //        gScore[neighbor] := tentative_gScore
    //        fScore[neighbor] := gScore[neighbor] + heuristic_cost_estimate(neighbor, goal)

    //return failure
    //}

    //function reconstruct_path(cameFrom, current) {
    //    total_path := [current]
    //    while current in cameFrom.Keys:
    //        current:= cameFrom[current]
    //        total_path.append(current)
    //    return total_path
    //}
}
