using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bouton : MonoBehaviour
{
    public Button btn;
    public LevelLoader LL;
    public string Type;

    void Start()
    {
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (Type == "TvW")
        {
            if (LL.GUI_Type == "Waypoint")
            {
                LL.GUI_Type = "Tile";
            }
            else
            {
                LL.GUI_Type = "Waypoint";
            }
            btn.GetComponentInChildren<Text>().text = LL.GUI_Type;
        }
        else if (Type == "H")
        {
            if (LL.H_Type == "Default Heuristic")
            {
                LL.H_Type = "Extra Heuristic";
            }
            else
            {
                LL.H_Type = "Default Heuristic";
            }
            btn.GetComponentInChildren<Text>().text = LL.H_Type;
        }
    }
}