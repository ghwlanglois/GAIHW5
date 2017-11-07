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

    // Use this for initialization
    void Start ()
    {
        int i = 0;
        foreach (string line in Map.text.Split('\n')){
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
                ++x;
                ++uT;
            }
            ++y;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //TODO Update GUI on GameObject TileGrid elements in conjunction with TileStates
	}
}
