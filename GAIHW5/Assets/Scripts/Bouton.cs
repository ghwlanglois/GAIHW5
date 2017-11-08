using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bouton : MonoBehaviour
{
    public Button btn;
    public LevelLoader LL;

    void Start()
    {
        btn.onClick.AddListener(TaskOnClick);
        btn.GetComponent<Text>().text = "Waypoint";
    }

    void TaskOnClick()
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
}