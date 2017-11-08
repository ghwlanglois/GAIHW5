using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class LevelLoader : MonoBehaviour {
    
    public GameObject Tile;
    public TextAsset Map;
    public GameObject[][] TileGrid;
    public HashSet<GameObject> WaypointGrid;

    char[][] grid;
    int height;
    int width;

    // Use this for initialization
    void Start ()
    {
        List<string> mapLines = new List<string>(Map.text.Split('\n'));

        string tmp;

        //TODO: save type
        mapLines.RemoveAt(0);

        tmp = mapLines[0].Substring(7);
        height = int.Parse(tmp);

        mapLines.RemoveAt(0);

        tmp = mapLines[0].Substring(6);
        width = int.Parse(tmp);

        mapLines.RemoveAt(0);
        mapLines.RemoveAt(0);

        grid = new char[height][];
        TileGrid = new GameObject[height][];
        for (int row =0; row<height; row++) {
            grid[row] = new char[width];
            TileGrid[row] = new GameObject[width];
        }

        int i = 0;
        foreach (string line in mapLines){
            if (line != null)
            {
                if (line.Length>0)
                {
                    char[] entries = line.ToCharArray();
                    grid[i] = entries;
                    ++i;
                } 
            }
        }

        float y = 0;
        for (int j = 0; j < height; ++j) {
            float x = 0;
            for (int u = 0; u < width; ++u) {
                char t = grid[j][u];
                GameObject go = Puntos(j, u, x, y, t);
                Point p = go.GetComponent<Point>();
                p.X = j; p.Y = u; p.Type = t;
                TileGrid[j][u] = (go);
                x += 1.025f; 
            }
            y -= 1.025f;
        }

        GenerateWaypoints();
        ResetColors();

        //float y = 0;
        //for (int j = 0; j < height; ++j)
        //{
        //    float x = 0;
        //    int uT = 0;
        //    for (int u = 0; u < width-1; ++u)
        //    {
        //        char t = grid[j][u];
        //        char next_t = grid[j][u+1];
        //        if (t == 'T')
        //        {
        //            Puntos(j, uT, x, y, t);
        //        }
        //        else if (t == '@')
        //        {
        //            Puntos(j, uT, x, y, t);
        //        }
        //        else if (t == '.' && next_t == '.')
        //        {
        //            Puntos(j, uT, x, y, t);
        //            ++u;
        //        }
        //        else if (t == '.' && next_t != '.')
        //        {
        //            x -= 0.257f;
        //            Puntos(j, uT, x, y, t);
        //            x -= 0.257f;
        //        }
        //        TileStates[j][uT] = 0;
        //        x += 1.025f;
        //        ++uT;
        //    }
        //    y -= 1.025f;
        //}
    }

    GameObject Puntos(int j, int uT, float x, float y, char t)
    {
        GameObject point = Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
        SpriteRenderer sr = point.GetComponent<SpriteRenderer>();
        return point;
    }

    void GenerateWaypoints() {
        WaypointGrid = new HashSet<GameObject>();
    }

    public void ResetColors() {
        foreach (GameObject[] array in TileGrid) {
            foreach(GameObject tile in array) {
                Point p = tile.GetComponent<Point>();
                switch (tile.GetComponent<Point>().Type) {
                    case '@':
                        p.SR.enabled = false;
                        break;
                    case 'T':
                        p.SR.color = Color.grey;
                        break;
                    case '.':
                        p.SR.color = Color.red;
                        break;
                    default:
                        break;
                }
            }
        }
       
    }
}
