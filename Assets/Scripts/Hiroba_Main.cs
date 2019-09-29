using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hiroba_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    // Use this for initialization
    void Start () {

        Debug.Log("main scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //所持アイテム画面を開く。初期設定で最初はOFF。
        playeritemlist_onoff = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        text_scenario();

        //text_area.SetActive(false);
        playeritemlist_onoff.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void text_scenario()
    {
        switch (GameMgr.scenario_flag)
        {
            case 120:
                _text.text = "材料か..。どうするかな？";
                break;

            default:
                _text.text = "どうするかな？";
                break;
        }
    }
}
