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

    private SoundController sc;

    private ExpTable exp_table;

    private Compound_Main compound_Main;
    private Girl1_status girl1_status;

    private GameObject statusList;   
    private GameObject paramview1;
    private GameObject paramview2;
    private GameObject paramview3;

    private GameObject StatusList_obj;
    private GameObject Costume_Panel_obj;
    private GameObject Collection_Panel_obj;

    private List<GameObject> costume_list = new List<GameObject>();

    private Text girlLV_param;
    private Text renkinnextLV_param;
    private Text girlHeart_param;
    private Text girlFind_power_param;
    private Text girlLifepoint_param;
    private Text playerLV_param;
    private Text girlExtremeKaisu_param;

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

        //経験値テーブルを取得
        exp_table = GameObject.FindWithTag("ExpTable").GetComponent<ExpTable>();

        //アクセサリーのスタート　配列番号
        Acce_Startnum = 6;

        //各ステータスパネルの値を取得。
        statusList = this.transform.Find("StatusList").gameObject;
        paramview1 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView1/Viewport/Content").gameObject;
        paramview2 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView2/Scroll View/Viewport/Content").gameObject;
        paramview3 = this.transform.Find("CostumePanel/ParamView3/Scroll View/Viewport/Content").gameObject;

        StatusList_obj = this.transform.Find("StatusList").gameObject;
        Costume_Panel_obj = this.transform.Find("CostumePanel").gameObject;
        Collection_Panel_obj = this.transform.Find("CollectionPanel").gameObject;
        collectionitem_toggle_obj = (GameObject)Resources.Load("Prefabs/CollectionIcon");

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
        girlLifepoint_param = paramview1.transform.Find("ParamE_param/Text").GetComponent<Text>();
        girlExtremeKaisu_param = paramview1.transform.Find("ParamH_param/Text").GetComponent<Text>();
        renkinnextLV_param = paramview1.transform.Find("ParamG_param/Text").GetComponent<Text>();
        zairyobox_lv_param = paramview2.transform.Find("Panel_1/Param").GetComponent<Text>();

        /* メインステータス画面更新 */
        OnStatusMainPanel();
        this.transform.Find("StatusPanelSelect_ScrollView/Viewport/Content/StatusMain_Toggle").GetComponent<Toggle>().isOn = true;

    }

    public void OnStatusMainPanel()
    {
        WindowAllOFF();
        StatusList_obj.SetActive(true);

        //パラメータ更新
        girlLV_param.text = PlayerStatus.girl1_Love_lv.ToString();
        girlHeart_param.text = PlayerStatus.girl1_Love_exp.ToString();
        girlFind_power_param.text = PlayerStatus.player_girl_findpower.ToString();
        girlExtremeKaisu_param.text = PlayerStatus.player_extreme_kaisu_Max.ToString();
        renkinnextLV_param.text = (exp_table.exp_table[PlayerStatus.player_renkin_lv + 1] - PlayerStatus.player_renkin_exp).ToString(); //次レベルに必要な経験値がでる。

        if (PlayerStatus.player_girl_lifepoint <= 3)
        {
            girlLifepoint_param.text = GameMgr.ColorRed + PlayerStatus.player_girl_lifepoint.ToString() + "</color>" + " / " + PlayerStatus.player_girl_maxlifepoint.ToString();
        }
        else
        {
            girlLifepoint_param.text = PlayerStatus.player_girl_lifepoint.ToString() + " / " + PlayerStatus.player_girl_maxlifepoint.ToString();
        }
        playerLV_param.text = PlayerStatus.player_renkin_lv.ToString();
        zairyobox_lv_param.text = PlayerStatus.player_zairyobox_lv.ToString();

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

            if (pitemlist.emeralditemlist[i].event_itemName == "Sukumizu_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //スク水
            {
                costume_list[1].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, 1);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "PinkGoth_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //ピンクゴスロリ
            {
                costume_list[2].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, 2);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Glass_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //メガネ
            {
                costume_list[Acce_Startnum].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Bafomet_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //バフォメットの角
            {
                costume_list[Acce_Startnum+1].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 1);
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "AngelWing_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //天使のはね
            {
                costume_list[Acce_Startnum + 2].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
                CostumeIconDraw(i, Acce_Startnum + 2);
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

    void WindowAllOFF()
    {
        StatusList_obj.SetActive(false);
        Costume_Panel_obj.SetActive(false);
        Collection_Panel_obj.SetActive(false);
    }
}
