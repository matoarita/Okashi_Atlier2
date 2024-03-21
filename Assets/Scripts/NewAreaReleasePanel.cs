using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAreaReleasePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCloseWindow()
    {
        GameMgr.scenario_ON = false;
        this.gameObject.SetActive(false);
    }
}
