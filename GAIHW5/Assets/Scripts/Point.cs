using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point : MonoBehaviour
{
    public int x;
    public int y;
    public char type;

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

    Point (int x, int y) {
        this.x = x;
        this.y = y;
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

    // override object.GetHashCode
    public override int GetHashCode() {
        int tmp = (Y + ((X + 1) / 2));
        return X + (tmp * tmp);
    }
}
