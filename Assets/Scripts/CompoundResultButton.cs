using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundResultButton : MonoBehaviour {

    private SetImage setimage;
    private SoundController sc;
    private keyManager keymanager;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //キー入力受付コントローラーの取得
        keymanager = keyManager.Instance.GetComponent<keyManager>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {

            sc.PlaySe(0);
            setimage = this.transform.parent.GetComponent<SetImage>();
            setimage.CompoundResult_Button();
        }
    }


}
