using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContestKakuninHyoujiPanel : MonoBehaviour {

    private PlayerItemList pitemlist;
    private ContestStartListDataBase conteststartList_database;
    private ItemMatPlaceDataBase matplace_database;

    private TimeController time_controller;

    private SoundController sc;

    private GameObject ContestOn_obj;
    private GameObject NoContestText_obj;

    private Sprite texture2d;
    private Image _Img;

    private Text contestname;
    private Text contestday;
    private Text contestmoney;
    private Text contest_desc;

    private GameObject quest_text1;
    private GameObject quest_text2;
    private GameObject quest_dayout;
    private GameObject quest_day_today;
    private GameObject quest_clientpanel;
    private GameObject contest_commentPanel;

    private GameObject contest_placelist;
    private GameObject placeicon_obj;

    private int i, _kosu;
    private int _money;
    private int _list;
    private int gotonum;
    private string _area;

    private bool ContestAccepted_ON;

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

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //時間管理オブジェクトの取得
        time_controller = TimeController.Instance.GetComponent<TimeController>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        contestname = this.transform.Find("PanelB/OnPanel/Contest_Title").GetComponent<Text>();
        contestday = this.transform.Find("PanelB/OnPanel/Contest_Day").GetComponent<Text>();
        contestmoney = this.transform.Find("PanelB/OnPanel/Contest_Money").GetComponent<Text>();
        contest_desc = this.transform.Find("PanelB/OnPanel/CommentPanel/Contest_Comment").GetComponent<Text>();
        contest_commentPanel = this.transform.Find("PanelB/OnPanel/CommentPanel").gameObject;
        contest_placelist = this.transform.Find("PanelB/OffPanel/ScrollView/Viewport/Content").gameObject;

        _Img = this.transform.Find("PanelB/OnPanel/ImageIcon").GetComponent<Image>(); //アイテムの画像データ

        ContestOn_obj = this.transform.Find("PanelB/OnPanel").gameObject; //
        NoContestText_obj = this.transform.Find("PanelB/OffPanel").gameObject; //


        //受注コンテストがあるかをチェック　そのリスト番号もいれとく
        ContestAccepted_ON = false;
        i = 0;
        while (i < conteststartList_database.conteststart_lists.Count)
        {
            if (conteststartList_database.conteststart_lists[i].Contest_Accepted == 1)
            {
                _list = i;
                ContestAccepted_ON = true;
                break;
            }
            i++;
        }

        for(i =0; i< matplace_database.matplace_lists.Count; i++)
        {
            switch(matplace_database.matplace_lists[i].placeName)
            {
                case "Or_Contest_A1":

                    if(matplace_database.matplace_lists[i].placeFlag == 1)
                    {
                        placeicon_obj = contest_placelist.transform.Find("ContestMoveButtonA_Panel").gameObject;
                        placeicon_obj.SetActive(true);
                        placeicon_obj.transform.Find("ContestMoveButtonA/Icon").GetComponent<Image>().sprite = matplace_database.matplace_lists[i].mapIcon_sprite;
                        placeicon_obj.transform.Find("Text").GetComponent<Text>().text = "春会場";
                    }
                    else
                    {
                        contest_placelist.transform.Find("ContestMoveButtonA_Panel").gameObject.SetActive(false);
                    }
                    break;

                case "Or_Contest_B1":

                    if (matplace_database.matplace_lists[i].placeFlag == 1)
                    {
                        placeicon_obj = contest_placelist.transform.Find("ContestMoveButtonB_Panel").gameObject;
                        placeicon_obj.SetActive(true);
                        placeicon_obj.transform.Find("ContestMoveButtonB/Icon").GetComponent<Image>().sprite = matplace_database.matplace_lists[i].mapIcon_sprite;
                        placeicon_obj.transform.Find("Text").GetComponent<Text>().text = "夏会場";
                    }
                    else
                    {
                        contest_placelist.transform.Find("ContestMoveButtonB_Panel").gameObject.SetActive(false);
                    }
                    break;

                case "Or_Contest_C1":

                    if (matplace_database.matplace_lists[i].placeFlag == 1)
                    {
                        placeicon_obj = contest_placelist.transform.Find("ContestMoveButtonC_Panel").gameObject;
                        placeicon_obj.SetActive(true);
                        placeicon_obj.transform.Find("ContestMoveButtonC/Icon").GetComponent<Image>().sprite = matplace_database.matplace_lists[i].mapIcon_sprite;
                        placeicon_obj.transform.Find("Text").GetComponent<Text>().text = "秋会場";
                    }
                    else
                    {
                        contest_placelist.transform.Find("ContestMoveButtonC_Panel").gameObject.SetActive(false);
                    }
                    break;

                case "Or_Contest_D1":

                    if (matplace_database.matplace_lists[i].placeFlag == 1)
                    {
                        placeicon_obj = contest_placelist.transform.Find("ContestMoveButtonD_Panel").gameObject;
                        placeicon_obj.SetActive(true);
                        placeicon_obj.transform.Find("ContestMoveButtonD/Icon").GetComponent<Image>().sprite = matplace_database.matplace_lists[i].mapIcon_sprite;
                        placeicon_obj.transform.Find("Text").GetComponent<Text>().text = "冬会場";
                    }
                    else
                    {
                        contest_placelist.transform.Find("ContestMoveButtonD_Panel").gameObject.SetActive(false);
                    }
                    break;
            }
            contest_placelist.SetActive(false);
            contest_placelist.SetActive(true); //scrollの整列しなおし
        }
    }

    private void OnEnable()
    {
        InitSetup();

        if (ContestAccepted_ON)
        {
            NoContestText_obj.SetActive(false);
            ContestOn_obj.SetActive(true);
            UpdateContestDetailedPanel();
        } else
        {
            NoContestText_obj.SetActive(true);
            ContestOn_obj.SetActive(false);
        }
    }

    //PanelBを描画する 受注リストのリスト配列番号を受け取って、中身を更新
    void UpdateContestDetailedPanel()
    {        
        _money = conteststartList_database.conteststart_lists[_list].Contest_Cost;
        if (_money == 0)
        {
            contestmoney.text = "無料";
        }
        else
        {
            contestmoney.text = _money.ToString();
        }

        contestname.text = conteststartList_database.conteststart_lists[_list].ContestNameHyouji;     
        contest_desc.text = conteststartList_database.conteststart_lists[_list].Contest_themeComment;
        contestday.text = GameMgr.contest_accepted_list[0].Month.ToString() + "/" + GameMgr.contest_accepted_list[0].Day.ToString();


        //texture2d = questset_database.questTakeset[_list].questIcon;
        //_Img.sprite = texture2d;
    }

    public void OnContestGoCheck()
    {
        //入店の音
        sc.PlaySe(150);
        GameMgr.ShopEnter_ButtonON = true;

        //エリア判定
        if (conteststartList_database.conteststart_lists[_list].ContestID >= 3000)
        {
            gotonum = 30;
        }
        else if (conteststartList_database.conteststart_lists[_list].ContestID >= 2000)
        {
            gotonum = 20;
        }
        else if (conteststartList_database.conteststart_lists[_list].ContestID >= 1000)
        {
            gotonum = 10;
        }
        else
        {
            gotonum = 0;
        }
        GameMgr.SceneSelectNum = gotonum;
        FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
    }

    public void BackOption()
    {

        GameMgr.compound_status = 110;
        this.gameObject.SetActive(false);
    }

    public void OnMoveContestA()
    {
        //入店の音
        sc.PlaySe(150);
        GameMgr.ShopEnter_ButtonON = true;

        GameMgr.SceneSelectNum = 0;
        FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
    }

    public void OnMoveContestB()
    {
        //入店の音
        sc.PlaySe(150);
        GameMgr.ShopEnter_ButtonON = true;

        GameMgr.SceneSelectNum = 10;
        FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
    }

    public void OnMoveContestC()
    {
        //入店の音
        sc.PlaySe(150);
        GameMgr.ShopEnter_ButtonON = true;

        GameMgr.SceneSelectNum = 20;
        FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
    }

    public void OnMoveContestD()
    {
        //入店の音
        sc.PlaySe(150);
        GameMgr.ShopEnter_ButtonON = true;

        GameMgr.SceneSelectNum = 30;
        FadeManager.Instance.LoadScene("Or_Contest_Reception", GameMgr.SceneFadeTime);
    }
}
