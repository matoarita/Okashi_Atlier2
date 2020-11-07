using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIPanel : MonoBehaviour {

    private GameObject canvas;
    private GameObject UIOpenButton_obj;

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        UIOpenButton_obj = canvas.transform.Find("MainUIOpenButton").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOpenButton()
    {
        foreach (Transform child in this.transform) //content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            child.gameObject.SetActive(true);
            //child.GetComponent<CanvasGroup>().alpha = 1;
        }
        UIOpenButton_obj.SetActive(false);
        
    }

    public void OnCloseButton()
    {
        foreach (Transform child in this.transform) //content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            child.gameObject.SetActive(false);
            //child.GetComponent<CanvasGroup>().alpha = 0;
        }
        UIOpenButton_obj.SetActive(true);
    }
}
