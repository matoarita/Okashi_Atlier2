using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRankTojoSE : MonoBehaviour {

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        sc.PlaySe(27);
        sc.PlaySe(4);
    }
}
