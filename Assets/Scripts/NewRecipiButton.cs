using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecipiButton : MonoBehaviour {

    private GameObject canvas;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject extremePanel_obj;
    private ExtremePanel extremePanel;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //エクストリームパネルオブジェクトの取得
        extremePanel_obj = canvas.transform.Find("ExtremePanel").gameObject;
        extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {

        if (GameMgr.tutorial_ON == true)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 80;
        }

        compound_Main.compound_status = 0;
        extremePanel.LifeAnimeOnTrue();

        card_view.DeleteCard_DrawView();
        Destroy(transform.parent.parent.gameObject);
        
    }
}
