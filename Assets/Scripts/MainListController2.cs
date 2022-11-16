using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainListController2 : MonoBehaviour
{
    //
    //** 広場全部で共通スクリプト **
    //


    private GameObject canvas;

    private ItemMatPlaceDataBase matplace_database;
    private PlayerItemList pitemlist;

    private Hiroba_Main2 Hiroba_main2;

    private GameObject npc1_toggle_obj;
    private GameObject npc2_toggle_obj;
    private GameObject npc3_toggle_obj;
    private GameObject npc4_toggle_obj;
    private GameObject npc5_toggle_obj;
    private GameObject npc6_toggle_obj;
    private GameObject npc7_toggle_obj;
    private GameObject npc8_toggle_obj;
    private GameObject shopstreet_toggle_obj;
    private GameObject hiroba1_toggle_obj;

    private Toggle npc1_toggle;
    private Toggle npc2_toggle;
    private Toggle npc3_toggle;
    private Toggle npc4_toggle;
    private Toggle npc5_toggle;
    private Toggle npc6_toggle;
    private Toggle npc7_toggle;
    private Toggle npc8_toggle;
    private Toggle shopstreet_toggle;
    private Toggle hiroba1_toggle;

    //3番外関連のトグル
    private GameObject hiroba3_npc1_toggle_obj;
    private GameObject hiroba3_back_toggle_obj;

    private Toggle hiroba3_npc1_toggle;
    private Toggle hiroba3_back_toggle;

    private GameObject text_area;
    private Text _text;

    private GridLayoutGroup gridlayout;
    private GameObject list_BG;

    private GameObject timepanel;

    private BGM sceneBGM;

    private Vector3 defaultPos;
    private Vector3 MovedPos;

    private int rndnum;

    // Use this for initialization
    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //広場スクリプト取得
        Hiroba_main2 = GameObject.FindWithTag("Hiroba_Main2").GetComponent<Hiroba_Main2>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //時間オブジェクトの取得
        timepanel = canvas.transform.Find("TimePanel").gameObject;

        //自身のレイアウトグループ情報の取得
        gridlayout = this.transform.Find("Viewport/Content_Main").GetComponent<GridLayoutGroup>();
        list_BG = this.transform.Find("ListBGimage").gameObject;
        defaultPos = this.transform.localPosition;

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Hiroba2":
                npc1_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC1_SelectToggle").gameObject;
                npc2_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC2_SelectToggle").gameObject;
                npc3_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC3_SelectToggle").gameObject;
                npc4_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC4_SelectToggle").gameObject;
                npc5_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC5_SelectToggle").gameObject;
                npc6_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC6_SelectToggle").gameObject;
                npc7_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC7_SelectToggle").gameObject;
                npc8_toggle_obj = this.transform.Find("Viewport/Content_Main/NPC8_SelectToggle").gameObject;
                shopstreet_toggle_obj = this.transform.Find("Viewport/Content_Main/ShopStreet_SelectToggle").gameObject;
                hiroba1_toggle_obj = this.transform.Find("Viewport/Content_Main/Hiroba1_SelectToggle").gameObject;


                npc1_toggle = npc1_toggle_obj.GetComponent<Toggle>();
                npc2_toggle = npc2_toggle_obj.GetComponent<Toggle>();
                npc3_toggle = npc3_toggle_obj.GetComponent<Toggle>();
                npc4_toggle = npc4_toggle_obj.GetComponent<Toggle>();
                npc5_toggle = npc5_toggle_obj.GetComponent<Toggle>();
                npc6_toggle = npc6_toggle_obj.GetComponent<Toggle>();
                npc7_toggle = npc7_toggle_obj.GetComponent<Toggle>();
                npc8_toggle = npc8_toggle_obj.GetComponent<Toggle>();
                shopstreet_toggle = shopstreet_toggle_obj.GetComponent<Toggle>();
                hiroba1_toggle = hiroba1_toggle_obj.GetComponent<Toggle>();

                npc1_toggle.interactable = true;
                npc2_toggle.interactable = true;
                npc3_toggle.interactable = true;
                npc4_toggle.interactable = true;
                npc5_toggle.interactable = true;
                npc6_toggle.interactable = true;
                npc7_toggle.interactable = true;
                npc8_toggle.interactable = true;

                //最初はoff
                npc4_toggle_obj.SetActive(false);
                npc5_toggle_obj.SetActive(false);
                npc6_toggle_obj.SetActive(false);
                npc7_toggle_obj.SetActive(false);
                npc8_toggle_obj.SetActive(false);
                shopstreet_toggle_obj.SetActive(false);
                break;

            case "Hiroba3":

                hiroba3_npc1_toggle_obj = this.transform.Find("Viewport/Content_Main/hiroba3_NPC1_SelectToggle").gameObject;
                hiroba3_back_toggle_obj = this.transform.Find("Viewport/Content_Main/hiroba3_Back_SelectToggle").gameObject;
                hiroba1_toggle_obj = this.transform.Find("Viewport/Content_Main/Hiroba1_SelectToggle").gameObject;

                hiroba3_npc1_toggle = hiroba3_npc1_toggle_obj.GetComponent<Toggle>();
                hiroba3_back_toggle = hiroba3_back_toggle_obj.GetComponent<Toggle>();
                hiroba1_toggle = hiroba1_toggle_obj.GetComponent<Toggle>();

                //最初はoff
                hiroba3_npc1_toggle_obj.SetActive(false);
                break;
        }

        //フラグをチェックし、必要ならONにする。
        ToggleFlagCheck();

        //時間が遅いと、お店などは閉まって入れなくなる。
        if (GameMgr.Story_Mode != 0)
        {

            switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
            {
                case 1:

                    break;

                case 2: //深夜→朝

                    break;

                case 3: //朝

                    break;

                case 4: //昼

                    break;

                case 5: //夕方

                    break;

                case 6: //夜

                    switch (SceneManager.GetActiveScene().name)
                    {
                        case "Hiroba2":

                            npc1_toggle.interactable = false;
                            npc2_toggle.interactable = false;
                            npc3_toggle.interactable = false;
                            npc4_toggle.interactable = false;
                            npc5_toggle.interactable = false;
                            npc6_toggle.interactable = false;
                            npc7_toggle.interactable = false;

                            //npc8_toggle.interactable = false; //3番街
                            break;
                    }
                    break;
            }
        }
    }

    public void MenuWindowExpand()
    {
        MovedPos = new Vector3(190, defaultPos.y, defaultPos.z);
        gridlayout.constraintCount = 2;
        list_BG.GetComponent<RectTransform>().sizeDelta = new Vector2(480, list_BG.GetComponent<RectTransform>().sizeDelta.y);
        this.transform.localPosition = MovedPos;
    }

    public void ToggleFlagCheck()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Hiroba2":

                if (GameMgr.Story_Mode == 0)
                {
                    //新しく3人のNPCのとこへいける
                    if (GameMgr.hiroba_event_end[0])
                    {
                        npc5_toggle_obj.SetActive(true);
                        npc6_toggle_obj.SetActive(true);
                        npc7_toggle_obj.SetActive(true);
                    }

                    //パン工房へいける
                    if (GameMgr.hiroba_event_end[0] && GameMgr.hiroba_event_end[1])
                    {
                        npc4_toggle_obj.SetActive(true);
                    }

                    //ストロベリーガーデンへいける
                    if (GameMgr.hiroba_event_end[2])
                    {
                        matplace_database.matPlaceKaikin("StrawberryGarden"); //ストロベリーガーデン解禁　いちごがとれるようになる。
                    }

                    //ひまわり畑へいける
                    if (GameMgr.hiroba_event_end[7])
                    {
                        matplace_database.matPlaceKaikin("HimawariHill"); //ひまわり畑解禁　ひまわりの種がとれるようになる。
                    }

                    if (GameMgr.hiroba_event_end[0])
                    {
                        MenuWindowExpand();
                    }
                }
                else
                {
                    MenuWindowExpand();
                    npc4_toggle_obj.SetActive(true);
                    npc5_toggle_obj.SetActive(true);
                    npc6_toggle_obj.SetActive(true);
                    npc7_toggle_obj.SetActive(true);

                    //3番街へいける
                    if (GameMgr.GirlLoveSubEvent_stage1[160])
                    {
                        npc8_toggle_obj.SetActive(true);
                    }
                }
                break;

            case "Hiroba3":

                MenuWindowExpand();

                if(GameMgr.GirlLoveSubEvent_stage1[160])
                {
                    hiroba3_npc1_toggle_obj.SetActive(true);
                }
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //
    //広場関連
    //

    //ベンチ　いちご少女
    public void OnNPC1_toggle()
    {
        if (npc1_toggle.isOn == true)
        {
            npc1_toggle.isOn = false;
           
            //いちご少女押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 0; //いちご少女を押した　という指定番号

            if (GameMgr.Story_Mode == 0)
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {
                    case 40: //ドーナツイベント時

                        if (!GameMgr.hiroba_event_end[2])
                        {
                            GameMgr.hiroba_event_ID = 40; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 41; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }

                        break;

                    case 50: //コンテストイベント時

                        if (!GameMgr.hiroba_event_end[10])
                        {
                            GameMgr.hiroba_event_ID = 50; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            if (!GameMgr.hiroba_ichigo_first)
                            {
                                GameMgr.hiroba_event_ID = 51; //いちごお菓子もってきた。初回
                            }
                            else
                            {
                                GameMgr.hiroba_event_ID = 52; //いちごお菓子もってきた。二回目以降
                            }

                            GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                            GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
                        }

                        break;

                    default:

                        GameMgr.hiroba_event_ID = 0;
                        break;
                }
            }
            else
            {
                if (!GameMgr.hiroba_ichigo_first)
                {
                    GameMgr.hiroba_event_ID = 10050; //いちごお菓子もってきた。初回
                }
                else
                {
                    GameMgr.hiroba_event_ID = 52; //いちごお菓子もってきた。二回目以降
                }

                GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
            }
            
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //噴水
    public void OnNPC2_toggle()
    {
        if (npc2_toggle.isOn == true)
        {
            npc2_toggle.isOn = false;

            //噴水押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 1; //

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[0])
                    {
                        sceneBGM.FadeOutBGM();
                        Hiroba_main2.bgm_change_flag = true;
                        GameMgr.hiroba_event_ID = 1040;

                        MenuWindowExpand();                       
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 1041; 
                    }
                    break;

                case 50:

                    GameMgr.hiroba_event_ID = 1050;
                    break;

                default:

                    GameMgr.hiroba_event_ID = 1000;
                    break;
            }
           
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //村長の家
    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;

            //村長の家押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 2; //

            if (GameMgr.Story_Mode == 0)
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {
                    case 40: //ドーナツイベント時

                        /*if (!GameMgr.hiroba_event_end[0] || !GameMgr.hiroba_event_end[3] || !GameMgr.hiroba_event_end[5])
                        {
                            GameMgr.hiroba_event_ID = 2040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else //最初アマクサにあったら、すぐイベントが進む。
                        {*/
                        if (!GameMgr.hiroba_event_end[1])
                        {
                            sceneBGM.FadeOutBGM();
                            Hiroba_main2.bgm_change_flag = true;
                            GameMgr.hiroba_event_ID = 2045;
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 2046;
                        }

                        //}
                        break;

                    case 50:

                        GameMgr.hiroba_event_ID = 2050;
                        break;

                    default:

                        GameMgr.hiroba_event_ID = 2000;
                        break;
                }
            }
            else
            {
                sceneBGM.FadeOutBGM();
                Hiroba_main2.bgm_change_flag = true;
                GameMgr.hiroba_event_ID = 12000;
            }

            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //パン工房
    public void OnNPC4_toggle()
    {
        if (npc4_toggle.isOn == true)
        {
            npc4_toggle.isOn = false;

            //パン工房押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 3; //

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[8])
                    {
                        if (!GameMgr.hiroba_event_end[6])
                        {
                            sceneBGM.FadeOutBGM();
                            Hiroba_main2.bgm_change_flag = true;
                            GameMgr.hiroba_event_ID = 3040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            //ひまわり油をもっていたら、イベントが進む。ひまわり油は削除する。
                            if (pitemlist.ReturnItemKosu("himawari_Oil") >= 1)
                            {
                                pitemlist.SearchDeleteItem("himawari_Oil");
                                pitemlist.addPlayerItemString("flyer", 1);

                                sceneBGM.FadeOutBGM();
                                Hiroba_main2.bgm_change_flag = true;
                                GameMgr.hiroba_event_ID = 3042;
                            }
                            else
                            {
                                GameMgr.hiroba_event_ID = 3041;
                            }
                        }
                    }
                    else //ドーナツレシピを教わった。
                    {
                        GameMgr.hiroba_event_ID = 3043;
                        
                    }
                    break;

                case 50:

                    if (!GameMgr.hiroba_event_end[11])
                    {
                        GameMgr.hiroba_event_ID = 3050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 3051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    
                    break;

                default:

                    GameMgr.hiroba_event_ID = 3000;
                    break;
            }

            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //お花屋さん
    public void OnNPC5_toggle()
    {
        if (npc5_toggle.isOn == true)
        {
            npc5_toggle.isOn = false;

            //お花屋さん押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 4; //

            if (GameMgr.Story_Mode == 0)
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {
                    case 40: //ドーナツイベント時

                        if (!GameMgr.hiroba_event_end[6])
                        {
                            if (!GameMgr.hiroba_event_end[3])
                            {
                                GameMgr.hiroba_event_ID = 4040;
                            }
                            else
                            {
                                GameMgr.hiroba_event_ID = 4041;
                            }
                        }
                        else //油の話をききにくる。
                        {
                            if (!GameMgr.hiroba_event_end[7])
                            {
                                GameMgr.hiroba_event_ID = 4042;
                            }
                            else
                            {
                                GameMgr.hiroba_event_ID = 4043;
                            }
                        }
                        break;

                    case 50:

                        if (!GameMgr.hiroba_event_end[12])
                        {
                            GameMgr.hiroba_event_ID = 4050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 4051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }

                        break;

                    default:

                        GameMgr.hiroba_event_ID = 4000;
                        break;
                }
            }
            else
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {
                    case 50:

                        GameMgr.hiroba_event_ID = 14050; //そのときに呼び出すイベント番号 placeNumとセットで使う。

                        break;

                    default:

                        GameMgr.hiroba_event_ID = 14000;
                        break;
                }
            }

            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //図書館
    public void OnNPC6_toggle()
    {
        if (npc6_toggle.isOn == true)
        {
            npc6_toggle.isOn = false;

            //図書館押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 5; //

            //図書室はBGMかえる
            sceneBGM.FadeOutBGM();
            Hiroba_main2.bgm_change_flag = true;

            if (GameMgr.Story_Mode == 0)
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {
                    case 40: //ドーナツイベント時

                        if (!GameMgr.hiroba_event_end[4] && !GameMgr.hiroba_event_end[5])
                        {
                            GameMgr.hiroba_event_ID = 5040;
                        }
                        else if (GameMgr.hiroba_event_end[4] && !GameMgr.hiroba_event_end[5])
                        {
                            GameMgr.hiroba_event_ID = 5041;
                        }
                        else if (GameMgr.hiroba_event_end[5])
                        {
                            GameMgr.hiroba_event_ID = 5042;
                        }
                        break;

                    case 50:

                        if (!GameMgr.hiroba_event_end[13])
                        {
                            GameMgr.hiroba_event_ID = 5050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 5051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }

                        break;

                    default:

                        GameMgr.hiroba_event_ID = 5000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        break;
                }
            }
            else
            {
                if(GameMgr.GirlLoveEvent_num == 50) //コンテスト時
                {
                    if (!GameMgr.hiroba_event_end[13])
                    {
                        GameMgr.hiroba_event_ID = 5050; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 5051; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                }
                else
                {
                    GameMgr.hiroba_event_ID = 15000;
                }
            }

            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //井戸端の奥さん
    public void OnNPC7_toggle()
    {
        if (npc7_toggle.isOn == true)
        {
            npc7_toggle.isOn = false;

            //井戸端の奥さん押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 6; //

            if (GameMgr.Story_Mode == 0)
            {
                //イベント発生フラグをチェック
                switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
                {
                    case 40: //ドーナツイベント時

                        //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。ベニエのこともあるし、お菓子のレシピや場所のヒント、だったりもする。
                        rndnum = Random.Range(0, 5);
                        GameMgr.hiroba_event_ID = 6040 + rndnum;
                        break;

                    case 50: //

                        //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。
                        rndnum = Random.Range(0, 5);
                        GameMgr.hiroba_event_ID = 6050;
                        break;

                    default:

                        GameMgr.hiroba_event_ID = 6000;
                        break;
                }
            }
            else
            {
                //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。
                rndnum = Random.Range(0, 7);
                GameMgr.hiroba_event_ID = 16000 + rndnum;
            }
           
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //3番街へ
    public void OnNPC8_toggle()
    {
        if (npc8_toggle.isOn == true)
        {
            npc8_toggle.isOn = false;

            //3番街へ（主にNPCイベント関係）
            FadeManager.Instance.LoadScene("Hiroba3", 0.3f);
        }
    }

    //
    //3番街関連
    //

    //モーセの礼拝堂
    public void OnHiroba3_NPC1_toggle()
    {
        if (hiroba3_npc1_toggle.isOn == true)
        {
            hiroba3_npc1_toggle.isOn = false;

            //モーセの礼拝堂　宴の処理へ
            GameMgr.hiroba_event_placeNum = 100; //

            //イベント発生フラグをチェック
            if (GameMgr.GirlLoveSubEvent_stage1[161]) //モーセクリア済み
            {
                GameMgr.hiroba_event_ID = 10002;

                GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ
            }
            else
            {
                if (GameMgr.GirlLoveSubEvent_stage1[160])
                {
                    GameMgr.hiroba_event_ID = 10001;

                    GameMgr.event_pitem_use_select = true; //イベント途中で、アイテム選択画面がでる時は、これをtrueに。
                    GameMgr.hiroba_event_ON = true; //アイテムを使うときに、広場イベントかどうかフラグ

                    //下は、使うときだけtrueにすればOK
                    GameMgr.KoyuJudge_ON = true;//固有のセット判定を使う場合は、使うを宣言するフラグと、そのときのGirlLikeSetの番号も入れる。
                    GameMgr.KoyuJudge_num = GameMgr.Mose_Okashi_num01;//GirlLikeSetの番号を直接指定
                    GameMgr.NPC_Dislike_UseON = true; //判定時、そのお菓子の種類が合ってるかどうかのチェックもする
                }
                else
                {
                    GameMgr.hiroba_event_ID = 10000; //デフォルト
                }
            }

            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //3番街から広場へ戻る
    public void OnHiroba3_Backtoggle()
    {
        if (hiroba3_back_toggle.isOn == true)
        {
            hiroba3_back_toggle.isOn = false;

            //3番街へ（主にNPCイベント関係）
            FadeManager.Instance.LoadScene("Hiroba2", 0.3f);
        }
    }

    //アトリエ戻る
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
