using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecipiButton : MonoBehaviour {

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject extremePanel_obj;
    private ExtremePanel extremePanel;

    // Use this for initialization
    void Start () {

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //エクストリームパネルオブジェクトの取得
        extremePanel_obj = GameObject.FindWithTag("ExtremePanel");
        extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        
        compound_Main.compound_status = 0;
        extremePanel.LifeAnimeOnTrue();

        card_view.DeleteCard_DrawView();
        Destroy(transform.parent.parent.gameObject);
    }
}
