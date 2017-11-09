The two heuristics implemented were a simple total distance on the grid, (Xdist + Ydist), and a diagonal distance formula heuristic.
These can be switched between by clicking the button, whichever the button says is the heuristic currently being used

The weight slider corresponds to how much weight the heuristic is given.

For all searches, the untouched nodes are red, the nodes waiting to be explored are yellow, the explored nodes are blue and the path is green. Trees are black, the Out of bounds nodes are not displayed

To select which nodes to path between, click two points. the two most recent points will be the start and end points.

Notes: For waypoints, the edges are what is shown using gizmos. due to the way gizmos works, we could not display the edges permanently and thusly they dissapear after 10 seconds.
Our frame rate is trash, we update the colors as fast as possible but due to the large nature of the maps it is slower than desiredl