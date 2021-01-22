using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIPanel : MonoBehaviour {

    private GameObject canvas;
    private GameObject UIOpenButton_obj;
    private GameObject TimePanel_obj;

    private SoundController sc;

    private Compound_Main compound_Main;

    private int total_obj_count;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        UIOpenButton_obj = canvas.transform.Find("MainUIOpenButton").gameObject;
        TimePanel_obj = this.transform.Find("TimePanel").gameObject;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        total_obj_count = 0;
        foreach (Transform child in this.transform)
        {
            total_obj_count++;
        }
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

        if(GameMgr.TimeUSE_FLAG == false)
        {
            TimePanel_obj.SetActive(false);
        }

        compound_Main.CheckButtonFlag();
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
