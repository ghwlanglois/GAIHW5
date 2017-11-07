using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class LevelLoader : MonoBehaviour {
    
    public GameObject Tile;
    public TextAsset Map;
    public GameObject[][] TileGrid;
    public int[][] TileStates; //0 = unexplored, 1 = explored, 2 = used

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
        Debug.Log(tmp);
        height = int.Parse(tmp);

        mapLines.RemoveAt(0);

        tmp = mapLines[0].Substring(6);
        Debug.Log(tmp);
        width = int.Parse(tmp);

        mapLines.RemoveAt(0);
        mapLines.RemoveAt(0);

        grid = new char[height][];
        TileGrid = new GameObject[height][];
        TileStates = new int[height][];
        for (int row =0; row<height; row++) {
            grid[row] = new char[width];
            TileGrid[row] = new GameObject[width];
            TileStates[row] = new int[width];
        }

        int i = 0;
        foreach (string line in mapLines){
            if (line != null)
            {
                if (line.Contains("@"))
                {
                    char[] entries = line.ToCharArray();
                    Debug.Log(entries);
                    grid[i] = entries;
                    ++i;
                } 
            }
        }

        float y = 0;
        for (int j = 0; j < height; ++j)
        {
            float x = 0;
            int uT = 0;
            for (int u = 0; u < width-1; ++u)
            {
                char t = grid[j][u];
                char next_t = grid[j][u+1];
                if (t == 'T')
                {
                    Puntos(j, uT, x, y, t);
                }
                else if (t == '@')
                {
                    Puntos(j, uT, x, y, t);
                }
                else if (t == '.' && next_t == '.')
                {
                    Puntos(j, uT, x, y, t);
                    ++u;
                }
                else if (t == '.' && next_t != '.')
                {
                    x -= 0.257f;
                    Puntos(j, uT, x, y, t);
                    x -= 0.257f;
                }
                TileStates[j][uT] = 0;
                x += 1.025f;
                ++uT;
            }
            y -= 1.025f;
        }
    }

    void Puntos(int j, int uT, float x, float y, char t)
    {
        TileGrid[j][uT] = Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
        TileGrid[j][uT].GetComponent<Point>().x = (int)(x);
        TileGrid[j][uT].GetComponent<Point>().y = (int)(y);
        TileGrid[j][uT].GetComponent<Point>().Type = t;
    }

    // Update is called once per frame
    void Update()
    {
        //Change the tile colors
        for (int i = 0; i < TileStates.Length; ++i)
        {
            for (int j = 0; j < TileStates[i].Length - 1; ++j)
            {
                Debug.Log("Checking TileGrid[" + i.ToString() + "][" + j.ToString() + "] = " + TileGrid[i][j].name);
                if (TileGrid[i][j].name.Contains("Point"))
                {
                    SpriteRenderer sr = TileGrid[i][j].GetComponent<SpriteRenderer>();
                    char t = TileGrid[i][j].GetComponent<Point>().Type;
                    if (t == '@')
                    {
                        sr.color = Color.black;
                    }
                    else if (t == 'T')
                    {
                        sr.color = Color.gray;
                    }
                    else if (TileStates[i][j] == 0)
                    {
                        sr.color = Color.red;
                    }
                    else if (TileStates[i][j] == 1)
                    {
                        sr.color = Color.white;
                    }
                    else if (TileStates[i][j] == 2)
                    {
                        sr.color = Color.green;
                    }
                }
            }
        }
    }
}
