using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear_Button : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //ステージクリアボタンの音量
        this.GetComponent<AudioSource>().volume = 1.0f * GameMgr.MasterVolumeParam * GameMgr.SeVolumeParam;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //ステージクリアボタンの音量
        this.GetComponent<AudioSource>().volume = 1.0f * GameMgr.MasterVolumeParam * GameMgr.SeVolumeParam;
    }
}
