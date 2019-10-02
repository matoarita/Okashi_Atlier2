using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GirlEat_Main : MonoBehaviour
{
    private GameObject utageScenario_obj;
    private Utage_scenario utageScenario;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;
    private GameObject girleat_select;
    private GameObject girleat_toggle_present;
    private GameObject girleat_toggle_watch;

    private GameObject playeritemlist_onoff;

    private GameObject backbutton_obj;

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private Girl1_status girl1_status; //女の子１のステータスを取得。

    private GameObject window_param_result_obj;
    private Text window_result_text;

    private Slider _slider; //好感度バーを取得
    private int _exp;

    private int sw;

    public int girleat_status;

    private int final_kettei_item1; //女の子にあげるアイテムの、アイテムID。

    // Use this for initialization
    void Start()
    {
        Debug.Log("Girl scene loaded");

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;

        //プレイヤー所持アイテムリストパネルの取得
        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        canvas = GameObject.FindWithTag("Canvas");
        girleat_select = canvas.transform.Find("GirlEat_Select").gameObject;
        girleat_toggle_present = girleat_select.transform.Find("Viewport/Content/GirlEat_Toggle_Present").gameObject;
        girleat_toggle_watch = girleat_select.transform.Find("Viewport/Content/GirlEat_Toggle_Watch").gameObject;


        //所持アイテム画面を開く。初期設定で最初はOFF。
        playeritemlist_onoff = GameObject.FindWithTag("PlayeritemList_ScrollView");

        //比較値計算結果用のパネル　デバッグ用
        window_param_result_obj = GameObject.FindWithTag("Canvas").transform.Find("Window_Param_Result").gameObject;
        window_result_text = window_param_result_obj.transform.Find("Viewport/Content/Text").gameObject.GetComponent<Text>();

        //no.SetActive(true);

        playeritemlist_onoff.SetActive(false);

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子


        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        sw = 0;
        girleat_status = 0;
        girl1_status.girl_comment_flag = false;

        _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();
        _slider.value = girl1_status.girl1_Love_exp;

        final_kettei_item1 = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if (girleat_status == 0)
        {
            playeritemlist_onoff.SetActive(false);
            backbutton_obj.SetActive(true);
            girleat_select.SetActive(true);

            //初期メッセージ
            _text.text = "こんにちわ～。今日はおヒマですか？";

        }

        if (girleat_status == 2) //女の子にアイテムをあげたあとの処理
        {
            //backbutton_obj.SetActive(false);

            //感想を表示「Utage_scenario.cs」で処理する。
            girleat_status = 3; //感想を表示中。
            girl1_status.girl_comment_flag = true; //→宴表示をON


        }

        if (girleat_status == 3) //女の子感想を言い中の処理。
        {
            //女の子が感想を言い終えたフラグを検出
            if (girl1_status.girl_comment_endflag == true)
            {
                girl1_status.girl_comment_endflag = false;
                _exp = 0;

                girleat_status = 10;
                sw = 0;

                text_area.SetActive(true);
                _text.text = "好感度: " + girl1_status.girl1_Getlove_exp + " アップ！！";

                StartCoroutine(Okashi_after());
            }
        }

        if (girleat_status == 5) //女の子を眺める
        {

        }

        if (girleat_status == 10) //好感度バーのアニメーションの処理
        {
            StartCoroutine("Okashi_after");

            if (_exp <= girl1_status.girl1_Getlove_exp)
            {
                ++girl1_status.girl1_Love_exp;
                ++_exp;
                _slider.value = girl1_status.girl1_Love_exp;

            }
            else if (_exp > girl1_status.girl1_Getlove_exp)
            {
                
            }

            
        }
    }


    IEnumerator Okashi_after() //お菓子の感想をいったあとに、記憶について、もしくはストーリーが展開
    {
        //while (!Input.GetMouseButtonDown(0)) yield return null;
        while (!text_area.GetComponent<MessageWindow>().window_clickon) yield return null;

        window_param_result_obj.SetActive(false);
        backbutton_obj.SetActive(true);
        girleat_status = 0;
        
    }

    public void OnCheck_1() //女の子にお菓子をあげる
    { 
        if (girleat_toggle_present.GetComponent<Toggle>().isOn == true)
        {
            girleat_toggle_present.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            backbutton_obj.SetActive(false);
            playeritemlist_onoff.SetActive(true); //アイテム画面を表示。
                                                  //no.SetActive(true);
            girleat_select.SetActive(false);

            girleat_status = 1; //女の子にアイテムをあげるシーンに入っています、というフラグ。分岐があるわけではないが、0の繰り返しを避ける意味がある。

            _text.text = "あげたいアイテムを選択してください。";
            
        }
    }

    public void OnCheck_2() //眺める（話かけて、噂を聞いたりする。）
    {
        if (girleat_toggle_watch.GetComponent<Toggle>().isOn == true)
        {
            girleat_toggle_watch.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            //shop_select.SetActive(false);

            girleat_status = 5; //眺めるを押したときのフラグ

            _text.text = "ん？どうしました？";

        }
    }

}