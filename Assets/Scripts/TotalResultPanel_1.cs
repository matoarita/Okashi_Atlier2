using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalResultPanel_1 : MonoBehaviour {

    public bool end_resultanim_1;

	// Use this for initialization
	void Start () {

        end_resultanim_1 = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EndAnim1()
    {
        end_resultanim_1 = true;
    }
}
