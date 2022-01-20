using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear_Button : MonoBehaviour {

    private AudioSource[] audiosource;

    private int i;

	// Use this for initialization
	void Start ()
    {
        SetInit();
    }
	
    void SetInit()
    {
        audiosource = this.GetComponents<AudioSource>();

        //ステージクリアボタンの音量
        for (i = 0; i < audiosource.Length; i++)
        {
            audiosource[i].volume = 1.0f * GameMgr.MasterVolumeParam * GameMgr.SeVolumeParam;
        }
        //this.GetComponent<AudioSource>().volume = 1.0f * GameMgr.MasterVolumeParam * GameMgr.SeVolumeParam;
    }
    // Update is called once per frame
    void Update () {
		
	}

    private void OnEnable()
    {
        SetInit();
    }
}
