using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainListController2 : MonoBehaviour
{
    private GameObject canvas;

    private Hiroba_Main2 Hiroba_main2;

    private GameObject npc1_toggle_obj;
    private GameObject npc2_toggle_obj;
    private GameObject npc3_toggle_obj;
    private GameObject hiroba1_toggle_obj;

    private Toggle npc1_toggle;
    private Toggle npc2_toggle;
    private Toggle npc3_toggle;
    private Toggle hiroba1_toggle;

    private GameObject text_area;
    private Text _text;

    private GameObject timepanel;

    // Use this for initialization
    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //広場スクリプト取得
        Hiroba_main2 = GameObject.FindWithTag("Hiroba_Main2").GetComponent<Hiroba_Main2>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //時間オブジェクトの取得
        timepanel = canvas.transform.Find("TimePanel").gameObject;

        npc1_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC1_SelectToggle").gameObject;
        npc2_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject;
        npc3_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC3_SelectToggle").gameObject;
        hiroba1_toggle_obj = this.transform.Find("Viewport/Content_Main/Hiroba1_SelectToggle").gameObject;


        npc1_toggle = npc1_toggle_obj.GetComponent<Toggle>();
        npc2_toggle = npc2_toggle_obj.GetComponent<Toggle>();
        npc3_toggle = npc3_toggle_obj.GetComponent<Toggle>();
        hiroba1_toggle = hiroba1_toggle_obj.GetComponent<Toggle>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnNPC1_toggle()
    {
        if (npc1_toggle.isOn == true)
        {
            npc1_toggle.isOn = false;
           
            //いちご少女押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 0; //いちご少女を押した　という指定番号

                                               //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 4: //ドーナツイベント時

                    GameMgr.hiroba_event_ID = 40; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;

                default:

                    GameMgr.hiroba_event_ID = 0; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }
            
            GameMgr.hiroba_event_flag = true;
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    public void OnNPC2_toggle()
    {
        if (npc2_toggle.isOn == true)
        {
            npc2_toggle.isOn = false;

            //噴水押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 1; //いちご少女を押した　という指定番号

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 4: //ドーナツイベント時

                    GameMgr.hiroba_event_ID = 1040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;

                default:

                    GameMgr.hiroba_event_ID = 1000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }
           
            GameMgr.hiroba_event_flag = true;
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;

            //村長の家押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 2; //いちご少女を押した　という指定番号
            GameMgr.hiroba_event_ID = 2000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
            GameMgr.hiroba_event_flag = true;
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    public void OnHiroba1_Button()
    {
        if (hiroba1_toggle.isOn == true)
        {
            hiroba1_toggle.isOn = false;
            FadeManager.Instance.LoadScene("Compound", 0.3f);
        }
    }

    void CanvasOff()
    {
        text_area.SetActive(false);
        timepanel.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
