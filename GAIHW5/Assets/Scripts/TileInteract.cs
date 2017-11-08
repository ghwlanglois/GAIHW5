using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInteract : MonoBehaviour {

    public Button btn;
    public LevelLoader LL;
    public Pathfinder PF;
    int select;

    // Use this for initialization
    void Start ()
    {
        select = 0;
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void TaskOnClick() {
        for (int j = 0; j < LL.TileGrid.Length; ++j)
        {
            for (int u = 0; u < LL.TileGrid[j].Length; ++u)
            {
                if (LL.TileGrid[j][u] == this)
                {
                    if (select == 0)
                    {
                        PF.A = LL.TileGrid[j][u].GetComponent<Point>();
                        select = 1;
                    }
                    else
                    {
                        PF.B = LL.TileGrid[j][u].GetComponent<Point>();
                        select = 0;
                    }
                    break;
                }
            }
        }
    }
}
