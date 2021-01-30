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
    }

    public void BackOption()
    {

        compound_Main.compound_status = 0;
        this.gameObject.SetActive(false);

    }
}
