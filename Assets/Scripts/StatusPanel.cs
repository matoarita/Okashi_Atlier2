using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatusPanel : MonoBehaviour {

    private GameObject canvas;

    private PlayerItemList pitemlist;

    private Buf_Power_Keisan bufpower_keisan;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private SoundController sc;

    private ExpTable exp_table;
    private HikariOkashiExpTable hikariokashi_exp_table;

    private Compound_Main compound_Main;
    private Girl1_status girl1_status;

    private GameObject statusList;   
    private GameObject paramview1;
    private GameObject paramview2;
    private GameObject paramview3;
    private GameObject HikariOkashiParamView;
    private GameObject HikariOkashiParamView2;

    private GameObject StatusList_obj;
    private GameObject StatusList_SelectView_obj;
    private GameObject Costume_Panel_obj;
    private GameObject Collection_Panel_obj;
    private GameObject HikariStatusList_obj;

    private GameObject HikariParam_Toggle_obj;

    private GameObject hikariokashiparam_Prefab;
    private List<GameObject> hikariokashiparam_list = new List<GameObject>();

    private List<GameObject> costume_list = new List<GameObject>();

    private Text girlLV_param;
    private Text girlHeart_param;
    private Text girlFind_power_param;
    private Text girlLifepoint_param;
    private Text playerLV_param;
    private Text playerMP_param;
    private Text playerNinki_param;
    private Text playerPRank_param;
    private Text girlExtremeKaisu_param;
    private Text BoxLv_param;
    private Text Okashi_SPquest_eatkaisu_param;
    private Text Okashi_SPquest_MaxScore_param;
    private Text Okashi_Game_MaxScore_param;

    private Text girlFind_power_param_buf;

    private Text zairyobox_lv_param;

    private GameObject _model_obj;
    private GameObject collectionitem_toggle_obj;
    public List<GameObject> collectionitem_toggle = new List<GameObject>();

    private int[] acce_num_before = new int[GameMgr.Accesory_Num.Length];

    private Sprite cosIcon_sprite;
    private Sprite hatena_sprite;
    private int change_acce_id;
    private int i, count;
    private int _itemID;
    private int nowlv;

    private int _buf_findpower;
    private int player_girl_findpower_final;

    private int Acce_Startnum;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        StatusPanelInit();
    }

    void StatusPanelInit()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //Live2Dモデルの取得
        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;

        //調合メイン取得
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //経験値テーブルを取得
        exp_table = ExpTable.Instance.GetComponent<ExpTable>();

        //ヒカリ経験値テーブルを取得
        hikariokashi_exp_table = HikariOkashiExpTable.Instance.GetComponent<HikariOkashiExpTable>();

        //アクセサリーのスタート　配列番号
        Acce_Startnum = 6;     

        //各ステータスパネルの値を取得。
        statusList = this.transform.Find("StatusList").gameObject;
        paramview1 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView1/Viewport/Content").gameObject;
        paramview2 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView2/Scroll View/Viewport/Content").gameObject;
        paramview3 = this.transform.Find("CostumePanel/ParamView3/Scroll View/Viewport/Content").gameObject;        

        StatusList_obj = this.transform.Find("StatusList").gameObject;
        StatusList_SelectView_obj = this.transform.Find("StatusPanelSelect_ScrollView").gameObject;
        Costume_Panel_obj = this.transform.Find("CostumePanel").gameObject;
        Collection_Panel_obj = this.transform.Find("CollectionPanel").gameObject;
        collectionitem_toggle_obj = (GameObject)Resources.Load("Prefabs/CollectionIcon");
        HikariStatusList_obj = this.transform.Find("HikariStatusList").gameObject;

        hatena_sprite = Resources.Load<Sprite>("Sprites/Icon/question");

        foreach (Transform child in Collection_Panel_obj.transform.Find("ParamView/Scroll View/Viewport/Content").transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }       

        collectionitem_toggle.Clear();
        for (i = 0; i < GameMgr.CollectionItems.Count; i++)
        {
            collectionitem_toggle.Add(Instantiate(collectionitem_toggle_obj, Collection_Panel_obj.transform.Find("ParamView/Scroll View/Viewport/Content").transform));
        }       

        girlLV_param = paramview1.transform.Find("ParamA_param/Text").GetComponent<Text>();
        girlHeart_param = paramview1.transform.Find("ParamB_param/Text").GetComponent<Text>();
        girlFind_power_param = paramview1.transform.Find("ParamC_param/Text").GetComponent<Text>();
        playerLV_param = paramview1.transform.Find("ParamD_param/Text").GetComponent<Text>();
        playerMP_param = paramview1.transform.Find("ParamF_param/Text").GetComponent<Text>();
        playerNinki_param = paramview1.transform.Find("ParamM_param/Text").GetComponent<Text>();
        playerPRank_param = paramview1.transform.Find("ParamO_param/Text").GetComponent<Text>();
        girlLifepoint_param = paramview1.transform.Find("ParamE_param/Text").GetComponent<Text>();
        girlExtremeKaisu_param = paramview1.transform.Find("ParamH_param/Text").GetComponent<Text>();
        BoxLv_param = paramview1.transform.Find("ParamI_param/Text").GetComponent<Text>();
        zairyobox_lv_param = paramview2.transform.Find("Panel_1/Param").GetComponent<Text>();
        Okashi_SPquest_eatkaisu_param = paramview1.transform.Find("ParamJ_param/TextKosu").GetComponent<Text>();
        Okashi_SPquest_MaxScore_param = paramview1.transform.Find("ParamK_param/TextKosu").GetComponent<Text>();
        Okashi_Game_MaxScore_param = paramview1.transform.Find("ParamL_param/TextKosu").GetComponent<Text>();

        //ヒカリお菓子ステータス関係
        InitHikariOkashiParam_View();

        HikariParam_Toggle_obj = this.transform.Find("StatusPanelSelect_ScrollView/Viewport/Content/HikariParam_Toggle").gameObject;
        HikariParam_Toggle_obj.SetActive(true);

        /*if(GameMgr.Story_Mode == 1)
        {
            if(GameMgr.GirlLoveEvent_num >= 0) //エクストラ最初から表示
            {
                HikariParam_Toggle_obj.SetActive(true);
            }
        }*/

        /* メインステータス画面更新 */
        this.transform.Find("StatusPanelSelect_ScrollView/Viewport/Content/StatusMain_Toggle").GetComponent<Toggle>().isOn = true;
        this.transform.Find("StatusPanelSelect_ScrollView/Viewport/Content/Costume_Toggle").GetComponent<Toggle>().isOn = false;
        this.transform.Find("StatusPanelSelect_ScrollView/Viewport/Content/Collection_Toggle").GetComponent<Toggle>().isOn = false;
        this.transform.Find("StatusPanelSelect_ScrollView/Viewport/Content/HikariParam_Toggle").GetComponent<Toggle>().isOn = false;
        OnStatusMainPanel();

        //画面のアニメ
        OpenAnim();
    }

    void OpenAnim()
    {
        //まず、初期値。
        StatusList_obj.GetComponent<CanvasGroup>().alpha = 0;
        StatusList_SelectView_obj.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(StatusList_obj.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
        sequence.Join(StatusList_SelectView_obj.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative());


        sequence.Append(StatusList_obj.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(StatusList_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        sequence.Join(StatusList_SelectView_obj.transform.DOLocalMove(new Vector3(50f, 0f, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(StatusList_SelectView_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    public void OnStatusMainPanel()
    {
        WindowAllOFF();
        StatusList_obj.SetActive(true);



        //
        //パラメータ更新
        //
        girlLV_param.text = PlayerStatus.girl1_Love_lv.ToString();
        girlHeart_param.text = PlayerStatus.girl1_Love_exp.ToString();
        girlFind_power_param.text = PlayerStatus.player_girl_findpower.ToString();
        girlExtremeKaisu_param.text = PlayerStatus.player_extreme_kaisu_Max.ToString();
        BoxLv_param.text = PlayerStatus.player_zairyobox_lv.ToString();


        if (PlayerStatus.player_girl_lifepoint <= 3)
        {
            girlLifepoint_param.text = GameMgr.ColorRed + PlayerStatus.player_girl_lifepoint.ToString() + "</color>" + " / " + PlayerStatus.player_girl_maxlifepoint.ToString();
        }
        else
        {
            girlLifepoint_param.text = PlayerStatus.player_girl_lifepoint.ToString() + " / " + PlayerStatus.player_girl_maxlifepoint.ToString();
        }
        playerLV_param.text = PlayerStatus.player_renkin_lv.ToString();
        //zairyobox_lv_param.text = PlayerStatus.player_zairyobox_lv.ToString();

        playerMP_param.text = PlayerStatus.player_mp.ToString() + " / " + PlayerStatus.player_maxmp.ToString();
        playerNinki_param.text = PlayerStatus.player_ninki_param.ToString();
        PlayerStatus.SetPatissierRank(PlayerStatus.player_ninki_param); //パティシエランクのチェック
        playerPRank_param.text = PlayerStatus.player_patissier_Rank_hyouki;


        //装備品があった場合、バフ効果も表示        
        girlFind_power_param_buf = paramview1.transform.Find("ParamC_param/Buf_Text").GetComponent<Text>();

        _buf_findpower = bufpower_keisan.Buf_findpower_Keisan(); //プレイヤー装備品計算
        girlFind_power_param_buf.text = _buf_findpower.ToString();

        if (_buf_findpower > 0)
        {
            paramview1.transform.Find("ParamC_param/Text_plus").gameObject.SetActive(true);
            paramview1.transform.Find("ParamC_param/Buf_Text").gameObject.SetActive(true);
        }
        else
        {
            paramview1.transform.Find("ParamC_param/Text_plus").gameObject.SetActive(false);
            paramview1.transform.Find("ParamC_param/Buf_Text").gameObject.SetActive(false);
        }

        
        /*if (GameMgr.Story_Mode == 1)
        {
            paramview1.transform.Find("ParamJ").gameObject.SetActive(true);
            paramview1.transform.Find("ParamJ_param").gameObject.SetActive(true);
            paramview1.transform.Find("ParamK").gameObject.SetActive(true);
            paramview1.transform.Find("ParamK_param").gameObject.SetActive(true);
            Okashi_SPquest_eatkaisu_param.text = GameMgr.Okashi_spquest_eatkaisu.ToString();
            Okashi_SPquest_MaxScore_param.text = GameMgr.Okashi_spquest_MaxScore.ToString();
            
        }
        else
        {
            paramview1.transform.Find("ParamJ").gameObject.SetActive(false);
            paramview1.transform.Find("ParamJ_param").gameObject.SetActive(false);
            paramview1.transform.Find("ParamK").gameObject.SetActive(false);
            paramview1.transform.Find("ParamK_param").gameObject.SetActive(false);
        }*/
        Okashi_Game_MaxScore_param.text = GameMgr.Okashi_toplast_score.ToString();

    }

    public void OnCostumePanel()
    {
        WindowAllOFF();
        Costume_Panel_obj.SetActive(true);

        //コスチューム関係のフラグ
        costume_list.Clear();
        count = 0;
        foreach (Transform child in paramview3.transform) //
        {
            costume_list.Add(child.gameObject);
            costume_list[count].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = false;
            count++;
        }

        for (i = 0; i < GameMgr.Accesory_Num.Length; i++)
        {
            acce_num_before[i] = GameMgr.Accesory_Num[i]; //アクセの場合、トグルチェンジ前の状態を保存。（Live2Dは1Frame内で同時にアニメーション切り替えができないため）
        }

        costume_list[0].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true; //デフォルト服は常時インタラクトON
        CostumeIconDraw(0, 0);

        for (i = 0; i < pitemlist.emeralditemlist.Count; i++)
        {
            /* コスチューム */
            if (pitemlist.emeralditemlist[i].event_itemName == "Meid_Black_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //黒エプロン
            {
                costume_list[1].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, 1);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Sukumizu_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //スク水
            {
                costume_list[2].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, 2);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "PinkGoth_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //ピンクの白い服
            {
                costume_list[3].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, 3);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "RedDress_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //赤い服
            {
                costume_list[4].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, 4);
            }

            /* アクセサリー */
            if (pitemlist.emeralditemlist[i].event_itemName == "Glass_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //メガネ
            {
                costume_list[Acce_Startnum].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "BalloonHat_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //バルーンハット
            {
                costume_list[Acce_Startnum + 1].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 1);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "AngelWing_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //天使のはね
            {
                costume_list[Acce_Startnum + 2].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 2);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Nekomimi_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //ねこみみ
            {
                costume_list[Acce_Startnum + 3].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 3);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "FlowerHairpin_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //お花のヘアピン
            {
                costume_list[Acce_Startnum + 4].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 4);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "TwincleStarDust_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //ティンクルスターダスト
            {
                costume_list[Acce_Startnum + 5].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 5);
            }
        }

        //現在装備しているアクセや服に応じて、トグルをONにする。
        costume_list[GameMgr.Costume_Num].transform.Find("ClothToggle").GetComponent<Toggle>().SetIsOnWithoutCallback(true);
        for (i = 0; i < GameMgr.Accesory_Num.Length; i++)
        {
            if (GameMgr.Accesory_Num[i] == 1)
            {
                //トグルをonにするときに、コールバックを呼ばずにONにできる書き方。ToggleExt.csを追加している。
                costume_list[i + Acce_Startnum].transform.Find("ClothToggle").GetComponent<Toggle>().SetIsOnWithoutCallback(true);
            }
            else
            {
                costume_list[i + Acce_Startnum].transform.Find("ClothToggle").GetComponent<Toggle>().isOn = false;
            }
        }
    }

    void CostumeIconDraw(int _list, int _num)
    {
        cosIcon_sprite = Resources.Load<Sprite>("Sprites/" + pitemlist.emeralditemlist[_list].event_fileName);
        costume_list[_num].transform.Find("ClothToggle/Background/Image").GetComponent<Image>().sprite = cosIcon_sprite;
        costume_list[_num].transform.Find("ClothToggle/Background/Image").GetComponent<Image>().color = Color.white;
    }

    public void OnCollectionPanel()
    {
        WindowAllOFF();
        Collection_Panel_obj.SetActive(true);

        for (i = 0; i < GameMgr.CollectionItems.Count; i++)
        {
            if(GameMgr.CollectionItems[i]) //登録済みの場合、コレクションとして表示される。
            {
                _itemID = database.SearchItemIDString(GameMgr.CollectionItemsName[i]);
                collectionitem_toggle[i].transform.Find("CollectionToggle/Background/Image").GetComponent<Image>().sprite = database.items[_itemID].itemIcon_sprite;
                collectionitem_toggle[i].transform.Find("CollectionToggle").GetComponent<Toggle>().interactable = true;
            }
            else //それ以外は？で表示
            {
                collectionitem_toggle[i].transform.Find("CollectionToggle/Background/Image").GetComponent<Image>().sprite = hatena_sprite;
                collectionitem_toggle[i].transform.Find("CollectionToggle").GetComponent<Toggle>().interactable = false;
            }
        }
    }


    public void OnCostumeChange() //0~5までのトグルを押すと、衣装チェンジ
    {
        for( i = 0; i < Acce_Startnum; i++)
        {
            if(costume_list[i].transform.Find("ClothToggle").GetComponent<Toggle>().isOn == true)
            {
                Debug.Log("衣装チェンジ: " + i);
                GameMgr.Costume_Num = i;
                             
                _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeCostume();
            }
        }
    }

    public void OnAccesoryChange() //6~12までのトグルを押すと、アクセサリーチェンジ
    {
        for (i = 0; i < GameMgr.Accesory_Num.Length; i++)
        {
            if (costume_list[i+ Acce_Startnum].transform.Find("ClothToggle").GetComponent<Toggle>().isOn == true)
            {
                Debug.Log("アクセチェンジON: " + i);

                GameMgr.Accesory_Num[i] = 1;               
            }
            else
            {
                Debug.Log("アクセチェンジOFF: " + i);
                GameMgr.Accesory_Num[i] = 0;
            }
        }

        _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeAcce();
    }

    public void OnHikariOkashiPanel()
    {
        WindowAllOFF();
        HikariStatusList_obj.SetActive(true);

        //パラメータ更新

    }

    void WindowAllOFF()
    {
        StatusList_obj.SetActive(false);
        Costume_Panel_obj.SetActive(false);
        Collection_Panel_obj.SetActive(false);
        HikariStatusList_obj.SetActive(false);
    }

    void InitHikariOkashiParam_View()
    {
        HikariOkashiParamView = this.transform.Find("HikariStatusList/Viewport/Content/Panel/HikariOkashiParamView/Viewport/Content").gameObject;
        HikariOkashiParamView2 = this.transform.Find("HikariStatusList/Viewport/Content/Panel/HikariOkashiParamView2/ScrollView/Viewport/Content").gameObject;
        hikariokashiparam_Prefab = (GameObject)Resources.Load("Prefabs/HikariOkashiParam");

        foreach (Transform child in HikariOkashiParamView.transform) //
        {
            Destroy(child.gameObject);
        }

        hikariokashiparam_list.Clear();
        for (i = 0; i < PlayerStatus.player_girl_okashiparam_Count; i++)
        {
            hikariokashiparam_list.Add(Instantiate(hikariokashiparam_Prefab, HikariOkashiParamView.transform));
        }

        i = 0;
        foreach(string key in PlayerStatus.player_girl_okashiparam_NameList.Values)
        {
            hikariokashiparam_list[i].transform.Find("nameText").GetComponent<Text>().text = key;
            i++;
        }

        nowlv = 1;
        for (i = 0; i < PlayerStatus.player_girl_okashiparam_Count; i++)
        {
            switch(i)
            {
                case 0: //アパレイユ

                    SetOkashiLV(i, PlayerStatus.player_girl_appaleil_lv, PlayerStatus.player_girl_appaleil_exp, 1);
                    break;
                case 1: //クリーム

                    SetOkashiLV(i, PlayerStatus.player_girl_cream_lv, PlayerStatus.player_girl_cream_exp, 0);
                    break;
                case 2: //クッキー

                    SetOkashiLV(i, PlayerStatus.player_girl_cookie_lv, PlayerStatus.player_girl_cookie_exp, 0);
                    break;
                case 3: //チョコレート

                    SetOkashiLV(i, PlayerStatus.player_girl_chocolate_lv, PlayerStatus.player_girl_chocolate_exp, 0);
                    //hikariokashiparam_list[i].SetActive(false); //チョコレートは現在使わないのでOFF
                    break;
                case 4: //クレープ

                    SetOkashiLV(i, PlayerStatus.player_girl_crepe_lv, PlayerStatus.player_girl_crepe_exp, 0);
                    break;
                case 5: //シュークリーム

                    SetOkashiLV(i, PlayerStatus.player_girl_creampuff_lv, PlayerStatus.player_girl_creampuff_exp, 0);
                    break;
                case 6: //ドーナツ

                    SetOkashiLV(i, PlayerStatus.player_girl_donuts_lv, PlayerStatus.player_girl_donuts_exp, 0);
                    break;
                case 7: //ケーキ

                    SetOkashiLV(i, PlayerStatus.player_girl_cake_lv, PlayerStatus.player_girl_cake_exp, 0);
                    break;
                case 8: //ラスク

                    SetOkashiLV(i, PlayerStatus.player_girl_rusk_lv, PlayerStatus.player_girl_rusk_exp, 0);
                    break;
                case 9: //キャンディ

                    SetOkashiLV(i, PlayerStatus.player_girl_candy_lv, PlayerStatus.player_girl_candy_exp, 0);
                    break;
                case 10: //ゼリー

                    SetOkashiLV(i, PlayerStatus.player_girl_jelly_lv, PlayerStatus.player_girl_jelly_exp, 0);
                    break;
                case 11: //ジュース

                    SetOkashiLV(i, PlayerStatus.player_girl_juice_lv, PlayerStatus.player_girl_juice_exp, 0);
                    break;
                case 12: //ティー

                    SetOkashiLV(i, PlayerStatus.player_girl_tea_lv, PlayerStatus.player_girl_tea_exp, 0);
                    break;
                case 13: //アイスクリーム

                    SetOkashiLV(i, PlayerStatus.player_girl_icecream_lv, PlayerStatus.player_girl_icecream_exp, 0);
                    break;
                case 14: //レアお菓子

                    SetOkashiLV(i, PlayerStatus.player_girl_rareokashi_lv, PlayerStatus.player_girl_rareokashi_exp, 0);
                    break;
            }   
            
            if( nowlv >= 9) //LVカンストのとき　マスターを表示
            {
                hikariokashiparam_list[i].transform.Find("MaxLvPanel").gameObject.SetActive(true);
            }
        }

        HikariOkashiParamView2.transform.Find("HikariOkashiParam2_A/ParamText").GetComponent<Text>().text = PlayerStatus.player_girl_eatCount_tabetai.ToString();
        HikariOkashiParamView2.transform.Find("HikariOkashiParam2_B/ParamText").GetComponent<Text>().text = PlayerStatus.player_girl_eatCount.ToString();
        HikariOkashiParamView2.transform.Find("HikariOkashiParam2_C/ParamText").GetComponent<Text>().text = databaseCompo.Hikarimake_Totalcount().ToString();
    }

    void SetOkashiLV(int _id, int _lv, int _exp, int _tableType)
    {
        nowlv = _lv;
        hikariokashiparam_list[_id].transform.Find("LvText").GetComponent<Text>().text = nowlv.ToString();
        if(_tableType == 0)
        {
            hikariokashiparam_list[_id].transform.Find("param_guage").GetComponent<Slider>().maxValue = GameMgr.Hikariokashi_Exptable[nowlv];
        }
        else if (_tableType == 1)
        {
            hikariokashiparam_list[_id].transform.Find("param_guage").GetComponent<Slider>().maxValue = GameMgr.Hikariokashi_Exptable2[nowlv];
        }
        
        hikariokashiparam_list[_id].transform.Find("param_guage").GetComponent<Slider>().value = _exp;
    }
}
