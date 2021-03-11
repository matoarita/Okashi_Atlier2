﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TasteHintPanel : MonoBehaviour {

    private Compound_Main compound_Main;

    private Text Okashi_lasthint_text;
    private Text Okashi_lastname_text;
    private Text Okashi_lastscore_text;
    private Text Okashi_lastshokukan_param_text;
    private Text Okashi_lastshokukan_mes_text;
    private Text Okashi_lastsweat_param_text;
    private Text Okashi_lastsour_param_text;
    private Text Okashi_lastbitter_param_text;
    private Text Okashi_onepoint_text;

    private string _onepoint;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetInit();
    }

    private void SetInit()
    {
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        Okashi_lasthint_text = this.transform.Find("HintPanel/HintText").GetComponent<Text>();
        Okashi_lasthint_text.text = GameMgr.Okashi_lasthint;

        Okashi_lastname_text = this.transform.Find("HintPanel/OkashiName").GetComponent<Text>();
        Okashi_lastname_text.text = GameMgr.Okashi_lastname;

        Okashi_lastscore_text = this.transform.Find("HintPanel/OkashiScore").GetComponent<Text>();
        Okashi_lastscore_text.text = GameMgr.Okashi_totalscore.ToString();

        Okashi_lastshokukan_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelA/PanelA_Param/Text").GetComponent<Text>();
        Okashi_lastshokukan_param_text.text = GameMgr.Okashi_lastshokukan_param.ToString();

        Okashi_lastshokukan_mes_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelA/PanelA_Title/Text").GetComponent<Text>();
        Okashi_lastshokukan_mes_text.text = GameMgr.Okashi_lastshokukan_mes;

        Okashi_lastsweat_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelB/PanelB_Param/Text").GetComponent<Text>();
        Okashi_lastsweat_param_text.text = GameMgr.Okashi_lastsweat_param.ToString();

        Okashi_lastsour_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelC/PanelC_Param/Text").GetComponent<Text>();
        Okashi_lastsour_param_text.text = GameMgr.Okashi_lastsour_param.ToString();

        Okashi_lastbitter_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelD/PanelD_Param/Text").GetComponent<Text>();
        Okashi_lastbitter_param_text.text = GameMgr.Okashi_lastbitter_param.ToString();

        Okashi_onepoint_text = this.transform.Find("HintPanel/OnepointText").GetComponent<Text>();
        SetOnepointHint();
        Okashi_onepoint_text.text = _onepoint;
    }

    public void BackOption()
    {

        compound_Main.compound_status = 0;
        this.gameObject.SetActive(false);

    }

    void SetOnepointHint()
    {
        switch(GameMgr.Okashi_OnepointHint_num)
        {
            case 0:

                _onepoint = "ショップへ行くと、おねえちゃんが「さくさく感」について教えてくれそう。";
                break;

            default: //9999とか。空にする。

                _onepoint = "";
                break;
        }
    }
}
