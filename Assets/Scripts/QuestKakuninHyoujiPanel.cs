using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestKakuninHyoujiPanel : MonoBehaviour {

    private QuestSetDataBase questset_database;
    private PlayerItemList pitemlist;
    private ItemMatPlaceDataBase matplace_database;

    private SoundController sc;
    private TimeController time_controller;

    private GameObject NoQuestText_obj;

    private Sprite texture2d;
    private Image _Img;

    private Text questname;
    private Text questday;
    private Text questmoney;
    private Text quest_clientname;
    private Text quest_area;
    private Text quest_desc;
    private Text quest_player_kosu;    
    private Text item_kosu;

    private GameObject quest_etc_text1;
    private GameObject quest_text1;
    private GameObject quest_text2;
    private GameObject quest_dayout;
    private GameObject quest_day_today;
    private GameObject quest_clientpanel;
    private GameObject quest_commentPanel;

    private GameObject bar_placelist;
    private GameObject placeicon_obj;

    private int _kosu;
    private int _money;
    private string _area;

    private int _Limit_day;
    private int _Nokori_day;

    private int i;

    // Use this for initialization
    void Start () {

        

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitSetup()
    {
        //クエスト受注データベース取得
        questset_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        questname = this.transform.Find("PanelB/Quest_name").GetComponent<Text>();
        item_kosu = this.transform.Find("PanelB/Quest_Kosu").GetComponent<Text>();
        questday = this.transform.Find("PanelB/Quest_Day").GetComponent<Text>();
        quest_text1 = this.transform.Find("PanelB/Quest_Day_text1").gameObject;
        quest_text2 = this.transform.Find("PanelB/Quest_Day_text2").gameObject;
        quest_etc_text1 = this.transform.Find("PanelB/Text").gameObject;
        quest_dayout = this.transform.Find("PanelB/Quest_Day_Out").gameObject;
        quest_day_today = this.transform.Find("PanelB/Quest_Day_Today").gameObject;
        questmoney = this.transform.Find("PanelB/Quest_Money").GetComponent<Text>();
        quest_clientname = this.transform.Find("PanelB/ClientPanel/Quest_ClientName").GetComponent<Text>();
        quest_area = this.transform.Find("PanelB/ClientPanel/Quest_Place").GetComponent<Text>();
        quest_desc = this.transform.Find("PanelB/CommentPanel/Quest_Comment").GetComponent<Text>();
        quest_player_kosu = this.transform.Find("PanelB/Quest_PlayerItemKosu").GetComponent<Text>();
        quest_clientpanel = this.transform.Find("PanelB/ClientPanel").gameObject;
        quest_commentPanel = this.transform.Find("PanelB/CommentPanel").gameObject;
        bar_placelist = this.transform.Find("PanelA/QuestCheckList_ScrollView/ScrollView/Viewport/Content").gameObject;

        _Img = this.transform.Find("PanelB/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        NoQuestText_obj = this.transform.Find("PanelA/QuestCheckList_ScrollView/NoQuestText").gameObject; //

        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            switch (matplace_database.matplace_lists[i].placeName)
            {
                case "Or_Bar_A1":

                    if (matplace_database.matplace_lists[i].placeFlag == 1)
                    {
                        placeicon_obj = bar_placelist.transform.Find("BarMoveButtonA_Panel").gameObject;
                        placeicon_obj.SetActive(true);
                        placeicon_obj.transform.Find("BarMoveButtonA/Icon").GetComponent<Image>().sprite = matplace_database.matplace_lists[i].mapIcon_sprite;
                        placeicon_obj.transform.Find("BarMoveButtonA/Text").GetComponent<Text>().text = matplace_database.matplace_lists[i].placeNameHyouji;
                    }
                    else
                    {
                        bar_placelist.transform.Find("BarMoveButtonA_Panel").gameObject.SetActive(false);
                    }
                    break;

                case "Or_Bar_C1":

                    if (matplace_database.matplace_lists[i].placeFlag == 1)
                    {
                        placeicon_obj = bar_placelist.transform.Find("BarMoveButtonB_Panel").gameObject;
                        placeicon_obj.SetActive(true);
                        placeicon_obj.transform.Find("BarMoveButtonB/Icon").GetComponent<Image>().sprite = matplace_database.matplace_lists[i].mapIcon_sprite;
                        placeicon_obj.transform.Find("BarMoveButtonB/Text").GetComponent<Text>().text = matplace_database.matplace_lists[i].placeNameHyouji;
                    }
                    else
                    {
                        bar_placelist.transform.Find("BarMoveButtonB_Panel").gameObject.SetActive(false);
                    }
                    break;
                
            }
            bar_placelist.SetActive(false);
            bar_placelist.SetActive(true); //scrollの整列しなおし
        }
    }

    private void OnEnable()
    {
        InitSetup();

        if (questset_database.questTakeset.Count > 0)
        {
            NoQuestText_obj.SetActive(false);
        } else
        {
            NoQuestText_obj.SetActive(true);
        }
    }

    //PanelBを描画する 受注リストのリスト配列番号を受け取って、中身を更新
    public void UpdateQuestDetailedPanel(int _list)
    {

        _money = questset_database.questTakeset[_list].Quest_buy_price;
        _kosu = questset_database.questTakeset[_list].Quest_kosu_default;

        questname.text = questset_database.questTakeset[_list].Quest_Title;
        item_kosu.text = _kosu.ToString();
        questmoney.text = (_money*_kosu).ToString();
        quest_clientname.text = questset_database.questTakeset[_list].Quest_ClientName;
        quest_desc.text = questset_database.questTakeset[_list].Quest_desc;

        if (questset_database.questTakeset[_list].Quest_itemName == "Non")
        {
            quest_player_kosu.text = pitemlist.KosuCountSubType(questset_database.questTakeset[_list].Quest_itemSubtype).ToString();
        }
        else {
            quest_player_kosu.text = pitemlist.KosuCount(questset_database.questTakeset[_list].Quest_itemName).ToString();
        }

        if (!GameMgr.System_BarQuest_LimitDayON)
        {
            questday.text = "";
            quest_text1.SetActive(false);
            quest_text2.SetActive(false);
            quest_etc_text1.SetActive(false);

            quest_clientpanel.transform.localPosition = new Vector3(0, 30, 0);
            quest_commentPanel.transform.localPosition = new Vector3(0, 20, 0);
        }
        else
        {
            quest_etc_text1.SetActive(true);
            quest_clientpanel.transform.localPosition = new Vector3(0, 0, 0); //デフォポジション
            quest_commentPanel.transform.localPosition = new Vector3(0, 0, 0);

            //あと何日
            _Limit_day = time_controller.CullenderKeisanInverse(questset_database.questTakeset[_list].Quest_LimitMonth, questset_database.questTakeset[_list].Quest_LimitDay);
            _Nokori_day = _Limit_day - PlayerStatus.player_day;

            if (_Nokori_day < 0)
            {
                questday.text = "";
                quest_text1.SetActive(false);
                quest_text2.SetActive(false);
                quest_dayout.SetActive(true);
                quest_day_today.SetActive(false);
            }
            else if (_Nokori_day == 0)
            {
                questday.text = "";
                quest_text1.SetActive(false);
                quest_text2.SetActive(false);
                quest_dayout.SetActive(false);
                quest_day_today.SetActive(true);
            }
            else
            {
                questday.text = _Nokori_day.ToString();
                quest_text1.SetActive(true);
                quest_text2.SetActive(true);
                quest_dayout.SetActive(false);
                quest_day_today.SetActive(false);
            }
        }
        

        switch (questset_database.questTakeset[_list].Quest_AreaType)
        {
            case 10:

                _area = "春酒場よいどれ亭";
                break;

            case 20:

                _area = "夏酒場";
                break;

            case 30:

                _area = "秋酒場";
                break;

            case 40:

                _area = "冬酒場";
                break;

            default:

                _area = "ガレット酒場";
                break;
        }
        quest_area.text = _area;

        texture2d = questset_database.questTakeset[_list].questIcon;
        _Img.sprite = texture2d;
    }

    public void BackOption()
    {

        GameMgr.compound_status = 110;
        this.gameObject.SetActive(false);
    }

    public void OnMoveBarA()
    {
        //入店の音
        sc.PlaySe(38);
        sc.PlaySe(51);

        GameMgr.ShopEnter_ButtonON = true;

        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
    }

    public void OnMoveBarB()
    {
        //入店の音
        sc.PlaySe(38);
        sc.PlaySe(51);
        GameMgr.ShopEnter_ButtonON = true;

        GameMgr.SceneSelectNum = 20;
        FadeManager.Instance.LoadScene("Or_Bar", GameMgr.SceneFadeTime);
    }
}
