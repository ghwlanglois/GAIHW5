using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeInput : MonoBehaviour {

    public InputField IF;
    public Pathfinder PF;

    // Use this for initialization
    void Start () {
        IF.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
	
	// Update is called once per frame
	void ValueChangeCheck() {
        PF.Weight = int.Parse(IF.text);
	}
}
