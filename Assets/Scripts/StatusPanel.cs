using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatusPanel : MonoBehaviour {

    private GameObject canvas;

    private PlayerItemList pitemlist;

    private SoundController sc;

    private Compound_Main compound_Main;
    private Girl1_status girl1_status;

    private GameObject statusList;
    private GameObject paramview1;
    private GameObject paramview2;
    private GameObject paramview3;

    private List<GameObject> costume_list = new List<GameObject>();

    private Text girlLV_param;
    private Text girlHeart_param;
    private Text girlFind_power_param;

    private Text zairyobox_lv_param;

    private GameObject _model_obj;

    private int[] acce_num_before = new int[GameMgr.Accesory_Num.Length];

    private Sprite cosIcon_sprite;
    private int change_acce_id;
    private int i, count;

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

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //Live2Dモデルの取得
        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;

        //調合メイン取得
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        //各ステータスパネルの値を取得。
        statusList = this.transform.Find("StatusList").gameObject;
        paramview1 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView1/Viewport/Content").gameObject;
        paramview2 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView2/Scroll View/Viewport/Content").gameObject;
        paramview3 = this.transform.Find("StatusList/Viewport/Content/Panel_B/ParamView3/Scroll View/Viewport/Content").gameObject;

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

        costume_list[0].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true; //デフォルト服は常時ON
        cosIcon_sprite = Resources.Load<Sprite>("Sprites/" + pitemlist.emeralditemlist[0].event_fileName);
        costume_list[0].transform.Find("ClothToggle/Background/Image").GetComponent<Image>().sprite = cosIcon_sprite;
        costume_list[0].transform.Find("ClothToggle/Background/Image").GetComponent<Image>().color = Color.white;

        for ( i=0; i < pitemlist.emeralditemlist.Count; i++ )
        {
            if(pitemlist.emeralditemlist[i].event_itemName == "Meid_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //メイド服
            {
                costume_list[1].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Sukumizu_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //スク水
            {
                costume_list[2].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "PinkGoth_Costume" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //ピンクゴスロリ
            {
                costume_list[3].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Glass_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //メガネ
            {
                costume_list[6].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "Bafomet_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //バフォメットの角
            {
                costume_list[7].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
            }

            if (pitemlist.emeralditemlist[i].event_itemName == "AngelWing_Acce" && pitemlist.emeralditemlist[i].ev_itemKosu >= 1) //天使のはね
            {
                costume_list[8].transform.Find("ClothToggle").GetComponent<Toggle>().interactable = true;
            }
        }


        //値を更新
        girlLV_param = paramview1.transform.Find("ParamA_param/Text").GetComponent<Text>();
        girlHeart_param = paramview1.transform.Find("ParamB_param/Text").GetComponent<Text>();
        girlFind_power_param = paramview1.transform.Find("ParamC_param/Text").GetComponent<Text>();
        zairyobox_lv_param = paramview2.transform.Find("Panel_1/Param").GetComponent<Text>();
      
        girlLV_param.text = girl1_status.girl1_Love_lv.ToString();
        girlHeart_param.text = girl1_status.girl1_Love_exp.ToString();
        girlFind_power_param.text = PlayerStatus.player_girl_findpower.ToString();

        zairyobox_lv_param.text = PlayerStatus.player_zairyobox_lv.ToString();
    }

    public void OnCostumeChange() //0~5までのトグルを押すと、衣装チェンジ
    {
        for( i = 0; i < 6; i++)
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
            if (costume_list[i+6].transform.Find("ClothToggle").GetComponent<Toggle>().isOn == true)
            {
                Debug.Log("アクセチェンジ: " + i);

                GameMgr.Accesory_Num[i] = 1;               
            }
            else
            {
                GameMgr.Accesory_Num[i] = 0;
            }
        }

        _model_obj.GetComponent<Live2DCostumeTrigger>().ChangeAcce();
    }
}
