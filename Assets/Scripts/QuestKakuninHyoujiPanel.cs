using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestKakuninHyoujiPanel : MonoBehaviour {

    private QuestSetDataBase questset_database;

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
    private Text item_kosu;

    private int _kosu;
    private int _money;
    private string _area;

    private int _Limit_day;
    private int _Nokori_day;

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

        questname = this.transform.Find("PanelB/Quest_name").GetComponent<Text>();
        item_kosu = this.transform.Find("PanelB/Quest_Kosu").GetComponent<Text>();
        questday = this.transform.Find("PanelB/Quest_Day").GetComponent<Text>();
        questmoney = this.transform.Find("PanelB/Quest_Money").GetComponent<Text>();
        quest_clientname = this.transform.Find("PanelB/Quest_ClientName").GetComponent<Text>();
        quest_area = this.transform.Find("PanelB/Quest_Place").GetComponent<Text>();
        quest_desc = this.transform.Find("PanelB/Quest_Comment").GetComponent<Text>();

        _Img = this.transform.Find("PanelB/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        NoQuestText_obj = this.transform.Find("PanelA/QuestCheckList_ScrollView/NoQuestText").gameObject; //
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

        //あと何日
        _Limit_day = time_controller.CullenderKeisanInverse(questset_database.questTakeset[_list].Quest_LimitMonth, questset_database.questTakeset[_list].Quest_LimitDay);
        _Nokori_day = _Limit_day - PlayerStatus.player_day;
        questday.text = _Nokori_day.ToString();

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
}
