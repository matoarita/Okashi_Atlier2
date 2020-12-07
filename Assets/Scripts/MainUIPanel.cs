using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIPanel : MonoBehaviour {

    private GameObject canvas;
    private GameObject UIOpenButton_obj;

    private SoundController sc;

    private Compound_Main compound_Main;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        UIOpenButton_obj = canvas.transform.Find("MainUIOpenButton").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOpenButton()
    {
        foreach (Transform child in this.transform) 
        {
            child.gameObject.SetActive(true);
            //child.GetComponent<CanvasGroup>().alpha = 1;
        }
        UIOpenButton_obj.SetActive(false);
        compound_Main.QuestClearCheck();
    }

    public void OnCloseButton()
    {
        foreach (Transform child in this.transform) 
        {
            child.gameObject.SetActive(false);
            //child.GetComponent<CanvasGroup>().alpha = 0;
        }
        UIOpenButton_obj.SetActive(true);
    }
}
