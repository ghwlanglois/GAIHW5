using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class LevelLoader : MonoBehaviour {

    public GameObject Block;
    public GameObject Tree;
    public GameObject Tile;
    public GameObject PartialTile;
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
                char[] entries = line.ToCharArray();
                if (System.Array.IndexOf(entries, 'T') != -1 ||
                    System.Array.IndexOf(entries, '@') != -1 ||
                    System.Array.IndexOf(entries, '.') != -1)
                {
                    grid[i] = entries;
                    Debug.Log("Hello");
                    ++i;
                } 
            }
        }
        int j = 0, u = 0, uT = 0;
        float x = 0, y = 0;

        for (; j < i; ++j) 
        {
            for (; u < grid[j].Length-1; ++u)
            {
                if (grid[j][u] == 'T') {
                    TileGrid[j][uT] = Instantiate(Tree, new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (grid[j][u] == '@') {
                    TileGrid[j][uT] = Instantiate(Block, new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (grid[j][u] == '.' && grid[j][u + 1] == '.') {
                    TileGrid[j][uT] = Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
                    ++u;
                }
                else if (grid[j][u] == '.' && grid[j][u + 1] != '.')
                {
                    TileGrid[j][uT] = Instantiate(PartialTile, new Vector3(x, y, 0), Quaternion.identity);
                    x -= 0.5f;
                }
                TileStates[j][uT] = 0;
                ++x;
                ++uT;
            }
            ++y;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Change the tile colors
        for (int i = 0; i < TileStates.Length; ++i)
        {
            for (int j = 0; j < TileStates[i].Length; ++j)
            {
                SpriteRenderer sr = TileGrid[i][j].GetComponent<SpriteRenderer>();
                if (TileStates[i][j] == 0)
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
