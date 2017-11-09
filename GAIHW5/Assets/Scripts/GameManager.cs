using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager INSTANCE;

    Agent[] agents;

    public LevelLoader levelLoader;
    public Pathfinder PF;
    public Text heuristicButton;
    int select;
    public Point[] points = { null, null };
    int pIndex = 0;
    public bool distHeuristic = false;
    public bool waypoints = false;

    public Agent[] Agents {
        get;
        set;
    }

	// Use this for initialization
	void Start () {
		if (INSTANCE != null) {
            this.enabled = false;
            return;
        }
        select = 0;
        INSTANCE = this;
        Agents = FindObjectsOfType<Agent>();
        //Debug.Log(Agents.Length);
    }

    public void ClearPoints() {
        points [0] = points [1] = null;
        pIndex = 0;
        PF.StopAllCoroutines();
        StopAllCoroutines();
    }

    public void FindPath() {
        PF.StopAllCoroutines();
        StopAllCoroutines();
        if (points[0] == null || points[1] == null) {
            Debug.LogError("Two points have nto been specified");
            return;
        }
        List<Point> path = new List<Point>();
        StartCoroutine(PF.aStar(points[0], points[1], path));
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            int u = (int)(p.x / 1.025f),
                j = (int)Mathf.Abs(p.y / 1.025f);
            Debug.Log(j.ToString() + ", " + u.ToString());
            p.z = 0;
            if (!waypoints && levelLoader.isValidCoord(j, u)) {
                Point po = levelLoader.TileGrid[j][u].GetComponent<Point>();
                if (po.Type != '.') {
                    return;
                }
                po.SR.color = Color.white;
                points[pIndex++ % 2] = po;
            } else if (waypoints) {
                Point po = null;
                float minD = float.PositiveInfinity;
                foreach (GameObject go in levelLoader.WaypointGrid) {
                    Point point = go.GetComponent<Point>();
                    float d = Mathf.Sqrt(Mathf.Pow(point.X - j, 2) + Mathf.Pow(point.Y - u, 2));
                    if (d < minD) {
                        minD = d;
                        po = point;
                    }
                }
                if (minD < 10f) {
                    points[pIndex++ % 2] = po;
                    po.SR.color = Color.white;
                }
            }
            //if (levelLoader.TileGrid[j][u] != null) {
            //    if (select == 0) {
            //        PF.A = levelLoader.TileGrid[j][u].GetComponent<Point>();
            //        select = 1;
            //    }
            //    else {
            //        PF.B = levelLoader.TileGrid[j][u].GetComponent<Point>();
            //        select = 0;
            //    }
            //}
        }
    }

    public void switchHeuristic() {
        distHeuristic = !distHeuristic;
        heuristicButton.text = distHeuristic ? "Distance Formula" : "Xdist + Ydist";
    }
}
