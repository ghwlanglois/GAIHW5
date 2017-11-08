using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point : MonoBehaviour
{
    public bool isWaypoint = false;
    [SerializeField]
    int x;
    [SerializeField]
    int y;
    [SerializeField]
    char type;
    SpriteRenderer sr;
    HashSet<Point> neighbors;

    public int X {
        get {
            return x;
        }
        set {
            this.x = value;
        }
    }

    public int Y {
        get {
            return y;
        }
        set {
            this.y = value;
        }
    }

    public char Type {
        get {
            return type;
        }
        set {
            type = value;
        }
    }

    public SpriteRenderer SR {
        get {
            return sr;
        }
    }

    public HashSet<Point> Neighbors {
        get {
            if (neighbors == null) {
                neighbors = new HashSet<Point>();
            } if (!isWaypoint) {
                if (x > 0) {
                    neighbors.Add(GameManager.INSTANCE.levelLoader.TileGrid[X - 1][Y].GetComponent<Point>());
                }
                if (x < GameManager.INSTANCE.levelLoader.TileGrid.Length - 1) {
                    neighbors.Add(GameManager.INSTANCE.levelLoader.TileGrid[X + 1][Y ].GetComponent<Point>());
                }
                if (y > 0) {
                    neighbors.Add(GameManager.INSTANCE.levelLoader.TileGrid[X][Y - 1].GetComponent<Point>());
                }
                if (y < GameManager.INSTANCE.levelLoader.TileGrid[0].Length - 1) {
                    neighbors.Add(GameManager.INSTANCE.levelLoader.TileGrid[X][Y + 1].GetComponent<Point>());
                }
            }
            return neighbors;
        }
    }

    public void AddNeighbor(Point p) {
        if (neighbors == null) {
            neighbors = new HashSet<Point>();
        }
        neighbors.Add(p);
    }

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    // override object.Equals
    public override bool Equals(object obj) {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType()) {
            return false;
        }

        Point o = (Point)obj;
        return this.X == o.X && this.Y == o.Y;
    }

    // override object.Equals
    static public bool operator== (Point a, Point b) {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
       
        return a.X == b.X && a.Y == b.Y;
    }

    // override object.Equals
    static public bool operator !=(Point a, Point b) {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        return a.X != b.X || a.Y != b.Y;
    }


    // override object.GetHashCode
    public override int GetHashCode() {
        int tmp = (Y + ((X + 1) / 2));
        return X + (tmp * tmp);
    }

    public override string ToString() {
        return "("+X.ToString()+","+Y.ToString()+") "+Type;
    }
}
