using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject shopitemlist_onoff;

    private GameObject backbutton_obj;

    public int shop_status;

    // Use this for initialization
    void Start () {

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;
        backbutton_obj.SetActive(false);

        //ショップリスト画面を開く。初期設定で最初はOFF。
        shopitemlist_onoff = GameObject.FindWithTag("ShopitemList_ScrollView");
        shopitemlist_onoff.SetActive(false);

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //初期メッセージ
        _text.text = "いらっしゃい～。好きなものを選んでね。";
        text_area.SetActive(false);

        shop_status = 0;
    }
	
	// Update is called once per frame
	void Update () {

        //宴のシナリオ表示（イベント進行中かどうか）を優先するかどうかをまず判定する。
        if (GameMgr.scenario_ON == true)
        {
            shopitemlist_onoff.SetActive(false);
            backbutton_obj.SetActive(false);
            text_area.SetActive(false);

            shop_status = 0;
        }
        else
        {
            //Debug.Log("shop_status" + shop_status);
            switch (shop_status)
            {
                case 0:

                    shopitemlist_onoff.SetActive(true); //ショップリスト画面を表示。
                    backbutton_obj.SetActive(true);
                    text_area.SetActive(true);

                    shop_status = 1; //ショップのシーンに入っている、というフラグ         

                    break;

                case 1:
                    break;
                
            }
        }

	}

}
