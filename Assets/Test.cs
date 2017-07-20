using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {

    Dictionary<string, int> dic;
	// Use this for initialization
	void Start () {
        dic = new Dictionary<string, int>();

        dic.Add("1", 1);
        dic.Add("2", 2);
        dic.Add("3", 3);
        dic.Add("4", 4);

        Debug.Log(dic.Count);
        dic.Remove("1");
        foreach (KeyValuePair<string, int> kv in dic)
        {
            Debug.Log(kv.Value);
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
