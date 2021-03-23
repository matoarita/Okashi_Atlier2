using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothToggle : MonoBehaviour {

    private GameObject canvas;
    private GameObject status_panel;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //ステータスパネルの取得
        status_panel = canvas.transform.Find("StatusPanel").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCostumeToggle()
    {
        status_panel.GetComponent<StatusPanel>().OnCostumeChange();
    }
}
