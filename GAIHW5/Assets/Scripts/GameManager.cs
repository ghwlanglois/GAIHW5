using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager INSTANCE;

    Agent[] agents;

    public LevelLoader levelLoader;
    public Pathfinder PF;
    int select;

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

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            int j = (int)(Input.mousePosition.x / 1.025f),
                u = (int)(Input.mousePosition.y / 1.025f);
            Debug.Log(j.ToString() + ", " + u.ToString());
            if (levelLoader.TileGrid[j][u] != null)
            {
                if (select == 0)
                {
                    PF.A = levelLoader.TileGrid[j][u].GetComponent<Point>();
                    select = 1;
                }
                else
                {
                    PF.B = levelLoader.TileGrid[j][u].GetComponent<Point>();
                    select = 0;
                }
            }
            levelLoader.UpdateColors();
        }
    }
}
