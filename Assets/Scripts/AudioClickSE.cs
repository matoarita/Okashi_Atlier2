using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClickSE : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        VolumeSetting();
    }

    public void VolumeSetting()
    {
        this.GetComponent<AudioSource>().volume = 1.0f * GameMgr.MasterVolumeParam; //
    }
}
