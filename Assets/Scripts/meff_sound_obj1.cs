using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meff_sound_obj1 : MonoBehaviour {

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSound01()
    {
        //Debug.Log("OnSound01()");
        sc.PlaySe(172);
    }

    public void OnSound02()
    {
        //Debug.Log("OnSound02()");
        sc.PlaySe(173);
        sc.PlaySe(174);
    }
}
