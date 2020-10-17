using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainListController2 : MonoBehaviour
{
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

        if (GameMgr.hiroba_event_end[0])
        {
            MenuWindowExpand();
        }

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

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

        //最初はoff
        npc4_toggle_obj.SetActive(false);
        npc5_toggle_obj.SetActive(false);
        npc6_toggle_obj.SetActive(false);
        npc7_toggle_obj.SetActive(false);
        npc8_toggle_obj.SetActive(false);
        shopstreet_toggle_obj.SetActive(false);

        //フラグをチェックし、必要ならONにする。
        ToggleFlagCheck();
    }

    public void MenuWindowExpand()
    {
        MovedPos = new Vector3(190, defaultPos.y, defaultPos.z);
        gridlayout.constraintCount = 2;
        list_BG.GetComponent<RectTransform>().sizeDelta = new Vector2(410, list_BG.GetComponent<RectTransform>().sizeDelta.y);
        this.transform.localPosition = MovedPos;
    }

    public void ToggleFlagCheck()
    {
        //新しく4人のNPCのとこへいける
        if(GameMgr.hiroba_event_end[0])
        {
            npc5_toggle_obj.SetActive(true);
            npc6_toggle_obj.SetActive(true);
            npc7_toggle_obj.SetActive(true);
            //npc8_toggle_obj.SetActive(true);
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ベンチ　いちご少女
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

                default:

                    GameMgr.hiroba_event_ID = 0; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }
            
            GameMgr.hiroba_event_flag = true;
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
                        GameMgr.hiroba_event_ID = 1040; //そのときに呼び出すイベント番号 placeNumとセットで使う。

                        MenuWindowExpand();                       
                    }
                    else
                    {
                        GameMgr.hiroba_event_ID = 1041; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
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

    //村長の家
    public void OnNPC3_toggle()
    {
        if (npc3_toggle.isOn == true)
        {
            npc3_toggle.isOn = false;

            //村長の家押した　宴の処理へ
            GameMgr.hiroba_event_placeNum = 2; //

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[0] || !GameMgr.hiroba_event_end[3] || !GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 2040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else //一度アマクサとあう＋花屋へいく＋図書館でドーナツの話をきくと、イベントが進む。
                    {
                        if (!GameMgr.hiroba_event_end[1])
                        {
                            
                            GameMgr.hiroba_event_ID = 2045; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 2046; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        
                    }
                    break;

                default:

                    GameMgr.hiroba_event_ID = 2000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }

            GameMgr.hiroba_event_flag = true;
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
                            if (pitemlist.ReturnItemKosu("himawari_Oil") != 9999)
                            {
                                pitemlist.SearchDeleteItem("himawari_Oil");
                                pitemlist.addPlayerItemString("flyer", 1);

                                sceneBGM.FadeOutBGM();
                                Hiroba_main2.bgm_change_flag = true;
                                GameMgr.hiroba_event_ID = 3042; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                            }
                            else
                            {
                                GameMgr.hiroba_event_ID = 3041; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                            }
                        }
                    }
                    else //ドーナツレシピを教わった。
                    {
                        GameMgr.hiroba_event_ID = 3043; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        
                    }
                    break;

                default:

                    GameMgr.hiroba_event_ID = 3000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }

            GameMgr.hiroba_event_flag = true;
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

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[6])
                    {
                        if (!GameMgr.hiroba_event_end[3])
                        {
                            GameMgr.hiroba_event_ID = 4040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 4041;
                        }
                    }
                    else //油の話をききにくる。
                    {
                        if(!GameMgr.hiroba_event_end[7])
                        {
                            GameMgr.hiroba_event_ID = 4042; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                        else
                        {
                            GameMgr.hiroba_event_ID = 4043; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                        }
                    }
                    break;

                default:

                    GameMgr.hiroba_event_ID = 4000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }

            GameMgr.hiroba_event_flag = true;
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

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    if (!GameMgr.hiroba_event_end[4] && !GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 5040; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    }
                    else if (GameMgr.hiroba_event_end[4] && !GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 5041;
                    }
                    else if(GameMgr.hiroba_event_end[5])
                    {
                        GameMgr.hiroba_event_ID = 5042;
                    }
                    break;

                default:

                    GameMgr.hiroba_event_ID = 5000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }

            GameMgr.hiroba_event_flag = true;
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

            //イベント発生フラグをチェック
            switch (GameMgr.GirlLoveEvent_num) //現在発生中のスペシャルイベント番号にそって、イベントを発生させる。
            {
                case 40: //ドーナツイベント時

                    //ひそひそ　ランダムでひとつ、ヒントかメッセージをだす。ベニエのこともあるし、お菓子のレシピや場所のヒント、だったりもする。
                    rndnum = Random.Range(0, 5);
                    GameMgr.hiroba_event_ID = 6040 + rndnum; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;

                default:

                    GameMgr.hiroba_event_ID = 6000; //そのときに呼び出すイベント番号 placeNumとセットで使う。
                    break;
            }

            GameMgr.hiroba_event_flag = true;
            Hiroba_main2.EventReadingStart();

            CanvasOff();
        }
    }

    //不思議なお店
    public void OnNPC8_toggle()
    {
        if (npc8_toggle.isOn == true)
        {
            npc8_toggle.isOn = false;

            //エメラルどんぐりショップへ
            FadeManager.Instance.LoadScene("Emerald_Shop", 0.3f);
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
