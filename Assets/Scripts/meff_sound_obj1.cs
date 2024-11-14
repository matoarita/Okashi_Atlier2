using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meff_sound_obj1 : MonoBehaviour {

    private SoundController sc;

    // Use this for initialization
    void Start () {

        Init_Setting();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {        
        Init_Setting();
    }

    void Init_Setting()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
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
