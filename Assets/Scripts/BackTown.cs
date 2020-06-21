using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackTown : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        text_area = canvas.transform.Find("MessageWindow").gameObject; //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickToTown()
    {
        _text.text = "また来てね～";

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Compound", 0.3f);
        GameMgr.Scene_back_home = true;
    }

    public void OnClickToTown_notext()
    {

        //メインシーン読み込み
        FadeManager.Instance.LoadScene("Compound", 0.3f);
        GameMgr.Scene_back_home = true;
    }
}
