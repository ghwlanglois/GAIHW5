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
    public string fileName;
    public GameObject[][] TileGrid;
    public int[][] TileStates; //0 = unexplored, 1 = explored, 2 = used

    string[][] grid;

    // Use this for initialization
    void Start ()
    {
        int i = 0;
        try
        {
            string line;
            StreamReader reader = new StreamReader(fileName, Encoding.Default);
            using (reader)
            {
                do
                {
                    line = reader.ReadLine();

                    if (line != null)
                    {
                        string[] entries = line.Split();
                        if (System.Array.IndexOf(entries, "T") != -1 ||
                            System.Array.IndexOf(entries, "@") != -1 ||
                            System.Array.IndexOf(entries, ".") != -1)
                        {
                            grid[i] = entries;
                            ++i;
                        }
                    }
                }
                while (line != null);

                reader.Close();
            }
        }
        catch
        {
            Debug.Log("Could not read input file " + fileName + " for level loader.");
        }

        int j = 0, u = 0, uT = 0;
        float x = 0, y = 0;

        for (; j < i; ++j) 
        {
            for (; u < grid[j].Length-1; ++u)
            {
                if (grid[j][u] == "T")
                {
                    TileGrid[j][uT] = Instantiate(Tree, new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (grid[j][u] == "@")
                {
                    TileGrid[j][uT] = Instantiate(Block, new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (grid[j][u] == "." && grid[j][u + 1] == ".")
                {
                    TileGrid[j][uT] = Instantiate(Tile, new Vector3(x, y, 0), Quaternion.identity);
                    ++u;
                }
                else if (grid[j][u] == "." && grid[j][u + 1] != ".")
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
