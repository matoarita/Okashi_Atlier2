using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hiroba_Main2 : MonoBehaviour
{

    private GameObject text_area;
    private Text _text;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject canvas;


    // Use this for initialization
    void Start()
    {

        //Debug.Log("main scene loaded");

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //所持アイテム画面を開く。初期設定で最初はOFF。
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        text_scenario();

        //text_area.SetActive(false);
        playeritemlist_onoff.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void text_scenario()
    {
        switch (GameMgr.scenario_flag)
        {
            case 120:
                _text.text = "材料か..。どうするかな？";
                break;

            default:
                _text.text = "ここは、村のメイン広場のようだ。人々が佇んでいる。";
                break;
        }
    }
}
