using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class LevelLoader : MonoBehaviour {

    public GameObject Block;
    public GameObject Pellet;
    public string fileName;
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
            Debug.Log("Could not read input file for level loader.");
        }

        int x = 0, y = 0, j = 0, u = 0;

        for (j = 0; j < i; ++j) 
        {
            for (u = 0; u < grid[j].Length-1; ++u)
            {
                if (grid[j][u] == "T")
                {
                    Instantiate(Block, new Vector3(x, y, 0), Quaternion.identity);
                }
                else if (grid[j][u] == "." && grid[j][u+1] == ".")
                {
                    Instantiate(Pellet, new Vector3(x, y, 0), Quaternion.identity);
                    ++u;
                }
                ++x;
            }
            ++y;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
