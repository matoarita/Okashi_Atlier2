using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TasteHintPanel : MonoBehaviour {

    //private Compound_Main compound_Main;
    private Girl1_status girl1_status;
    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private Text Okashi_hint_title;
    private Text Okashi_lasthint_text;
    private Text Okashi_lastname_text;
    private Text Okashi_lastscore_text;
    private Text Okashi_lastshokukan_param_text;
    private Text Okashi_lastshokukan_mes_text;
    private Text Okashi_lastsweat_param_text;
    private Text Okashi_lastsour_param_text;
    private Text Okashi_lastbitter_param_text;
    private Sprite Okashi_Img;
    private Image Okashi_Icon;
    private GameObject HikariIcon_Normal;
    private GameObject HikariIcon_Angry;
    private Text OneComment_text;
    private List<string> _one_comment_lib = new List<string>();
    private string _one_comment;
    private Text NowEat_text;
    private TMP_InputField hakushi_inputField;

    private GameObject charaIcon_obj;
    private GameObject hinttext_obj;
    private GameObject hinttextcontest_obj;
    private Text Okashi_contesthint_text;

    private GameObject hintpanel_obj1;
    private GameObject hintpanel_obj2;

    private GameObject category_view;
    private List<GameObject> view_toggle = new List<GameObject>();

    private int i, random;
    private bool ev_yusen;
    private int whitememo_kosu;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetInit();
    }

    private void SetInit()
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        hintpanel_obj1 = this.transform.Find("HintPanel/Panel_1").gameObject;
        hintpanel_obj1.SetActive(true);
        hintpanel_obj2 = this.transform.Find("HintPanel/Panel_2").gameObject;
        hintpanel_obj2.SetActive(false);

        charaIcon_obj = hintpanel_obj1.transform.Find("CharaIcon").gameObject;
        hinttext_obj = hintpanel_obj1.transform.Find("HintText").gameObject;
        hinttextcontest_obj = hintpanel_obj1.transform.Find("HintTextContest").gameObject;
        hinttextcontest_obj.SetActive(false);

        Okashi_hint_title = hintpanel_obj1.transform.Find("HintTitle").GetComponent<Text>();
        Okashi_lasthint_text = hintpanel_obj1.transform.Find("HintText").GetComponent<Text>();
        Okashi_contesthint_text = hintpanel_obj1.transform.Find("HintTextContest").GetComponent<Text>();

        Okashi_lastname_text = hintpanel_obj1.transform.Find("OkashiName").GetComponent<Text>();
        Okashi_lastscore_text = hintpanel_obj1.transform.Find("OkashiScore").GetComponent<Text>();

        Okashi_lastshokukan_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelA/PanelA_Param/Text").GetComponent<Text>();
        Okashi_lastshokukan_mes_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelA/PanelA_Title/Text").GetComponent<Text>();

        Okashi_lastsweat_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelB/PanelB_Param/Text").GetComponent<Text>();
        Okashi_lastsour_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelC/PanelC_Param/Text").GetComponent<Text>();
        Okashi_lastbitter_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelD/PanelD_Param/Text").GetComponent<Text>();

        Okashi_Img = database.items[GameMgr.Okashi_lastID].itemIcon_sprite;
        Okashi_Icon = hintpanel_obj1.transform.Find("OkashiImage").GetComponent<Image>(); //画像アイコン
        Okashi_Icon.sprite = Okashi_Img;

        OneComment_text = hintpanel_obj1.transform.Find("OneCommentText").GetComponent<Text>();
        NowEat_text = hintpanel_obj1.transform.Find("NowEatText").GetComponent<Text>();

        //顔アイコン
        HikariIcon_Normal = hintpanel_obj1.transform.Find("CharaIcon/HikariIcon1").gameObject;
        HikariIcon_Angry = hintpanel_obj1.transform.Find("CharaIcon/HikariIcon2").gameObject;

        Hikari_TasteHintDraw();

        //白紙メモ関係
        view_toggle.Clear();
        category_view = this.transform.Find("HintPanel/CategoryView/Viewport/Content").gameObject;
        foreach(Transform obj in category_view.transform)
        {
            view_toggle.Add(obj.gameObject);
            obj.gameObject.SetActive(false);
            if(obj.name == "WhiteMemoToggle_Taste") { obj.gameObject.SetActive(true); }
            if (obj.name == "WhiteMemoToggle_TasteContest") { obj.gameObject.SetActive(true); }
        }
        hakushi_inputField = hintpanel_obj2.transform.Find("Scroll View/Viewport/Content/InputField_TMP").GetComponent<TMP_InputField>();
        GameMgr.System_WhiteMemo_Num = 0;

        //持ってるメモの枚数でトグルをON
        whitememo_kosu = pitemlist.ReturnEventItemKosu("MemoWhite");
        switch(whitememo_kosu)
        {
            case 1:

                for (i = 0; i < view_toggle.Count; i++)
                {
                    WhiteMemo_ON(i, "WhiteMemoToggle_1");                    
                }
                break;

            case 2:

                for (i = 0; i < view_toggle.Count; i++)
                {
                    WhiteMemo_ON(i, "WhiteMemoToggle_1");
                    WhiteMemo_ON(i, "WhiteMemoToggle_2");
                }
                break;

            case 3:

                for (i = 0; i < view_toggle.Count; i++)
                {
                    WhiteMemo_ON(i, "WhiteMemoToggle_1");
                    WhiteMemo_ON(i, "WhiteMemoToggle_2");
                    WhiteMemo_ON(i, "WhiteMemoToggle_3");
                }
                break;

            default:

                break;
        }
    }

    void WhiteMemo_ON(int _list, string _name)
    {
        if (view_toggle[_list].name == _name)
        {
            view_toggle[_list].SetActive(true);
        }
    }

    public void BackOption()
    {

        GameMgr.compound_status = 0;
        GameMgr.Scene_Status = 0;
        this.gameObject.SetActive(false);

    }

    void Hikari_TasteHintDraw()
    {
        charaIcon_obj.SetActive(true);
        hinttext_obj.SetActive(true);
        hinttextcontest_obj.SetActive(false);

        Okashi_hint_title.text = "◆ さっき食べたおかしメモ ◆";

        Okashi_lasthint_text.text = GameMgr.Okashi_lasthint;

        Okashi_lastname_text.text = GameMgr.ColorGold + GameMgr.Okashi_lastslot + "</color>" + GameMgr.Okashi_lastname;
        Okashi_lastscore_text.text = GameMgr.Okashi_last_totalscore.ToString();

        Okashi_lastshokukan_param_text.text = GameMgr.Okashi_lastshokukan_param.ToString();
        Okashi_lastshokukan_mes_text.text = GameMgr.Okashi_lastshokukan_mes;

        Okashi_lastsweat_param_text.text = GameMgr.Okashi_lastsweat_param.ToString();
        Okashi_lastsour_param_text.text = GameMgr.Okashi_lastsour_param.ToString();
        Okashi_lastbitter_param_text.text = GameMgr.Okashi_lastbitter_param.ToString();

        Okashi_Img = database.items[GameMgr.Okashi_lastID].itemIcon_sprite;
        Okashi_Icon.sprite = Okashi_Img;

        OneComment_text.text = "";
        RandomOneComment();

        NowEat_text.text = GameMgr.NowEatOkashiName;

        //顔アイコン
        if (GameMgr.Okashi_totalscore <= 30)
        {
            HikariIcon_Normal.SetActive(false);
            HikariIcon_Angry.SetActive(true);
        }
        else
        {
            HikariIcon_Normal.SetActive(true);
            HikariIcon_Angry.SetActive(false);
        }
    }

    void Contest_TasteHintDraw()
    {
        charaIcon_obj.SetActive(false);
        hinttext_obj.SetActive(false);
        hinttextcontest_obj.SetActive(true);

        Okashi_hint_title.text = "◆ 前回コンテストのおかし ◆";

        Okashi_contesthint_text.text = GameMgr.contest_lasthint_text;

        Okashi_lastname_text.text = GameMgr.ColorGold + GameMgr.contest_okashiSlotName + "</color>" + GameMgr.contest_okashiNameHyouji;
        Okashi_lastscore_text.text = GameMgr.contest_TotalScore.ToString();

        Okashi_lastshokukan_param_text.text = GameMgr.contest_shokukan_param.ToString();
        Okashi_lastshokukan_mes_text.text = GameMgr.contest_shokukan_mes;

        Okashi_lastsweat_param_text.text = GameMgr.contest_sweat_param.ToString();
        Okashi_lastsour_param_text.text = GameMgr.contest_sour_param.ToString();
        Okashi_lastbitter_param_text.text = GameMgr.contest_bitter_param.ToString();

        Okashi_Img = database.items[database.SearchItemID(GameMgr.contest_okashiID)].itemIcon_sprite;
        Okashi_Icon.sprite = Okashi_Img;

        //顔アイコン じいさんとかになる
        /*if (GameMgr.Okashi_totalscore <= 30)
        {
            HikariIcon_Normal.SetActive(false);
            HikariIcon_Angry.SetActive(true);
        }
        else
        {
            HikariIcon_Normal.SetActive(true);
            HikariIcon_Angry.SetActive(false);
        }*/
    }

    void RandomOneComment()
    {
        _one_comment_lib.Clear();
        ev_yusen = false;

        switch (GameMgr.GirlLoveEvent_num)
        {
            case 0: //オリジナルクッキーを食べたい

                CheckFirstZairyoNo();
                if (!ev_yusen)
                {
                    _one_comment_lib.Add("にいちゃん。" + GameMgr.ColorPink + "さくさく感" + "</color>" + "の出し方は、ショップのおねえちゃんが知ってたかも？");
                }
                break;

            case 1: //ぶどうクッキー

                CheckFirstZairyoNo();
                if (!ev_yusen)
                {
                    _one_comment_lib.Add("にいちゃん。森で果物が取れたかも。" + GameMgr.ColorPink + "「外へ出る」" + "</color>" + "からお外へ出れるよ～！");
                    _one_comment_lib.Add("にいちゃん。　ヒカリのお口にタッチしたら、ヒントを教えてあげる！");
                }
                break;

            case 2: //かわいいクッキー

                CheckFirstZairyoNo();

                _one_comment_lib.Add("にいちゃん。きらきらの材料は、ショップのおねえちゃんが売ってたかも？");

                break;

            case 10: //ラスク食べたい


                break;

            case 11: //すっぱいラスク食べたい

                _one_comment_lib.Add("すっぱいくだもの..。近くの森か、ベリーファームで、見かけたかも。");
                _one_comment_lib.Add("ベリーのくだものは、すっぱいのが多いんだよ～。にいちゃん！");
                _one_comment_lib.Add("普通のラスクに「バター」を入れても、さくさくになっておいしいかも？");
                break;

            case 20: //クレープ食べたい

                if (!GameMgr.ShopEvent_stage[2])
                {
                    _one_comment_lib.Add("にいちゃん。クレープのことは、ショップのおねえちゃんが知ってたかも？");
                }
                else
                {
                    if (!GameMgr.FarmEvent_stage[0]) //はじめて牧場をおとずれる。プリンさんからたまごの話をきいてから、フラグがたつ。
                    {
                        _one_comment_lib.Add("にいちゃん！　たまごを取りに、牧場へいこう！");
                    }
                    else
                    {

                    }
                }
                break;

            case 21: //豪華なクレープ食べたい

                /*if(GameMgr.Okashi_totalscore <= GameMgr.low_score)
                {
                    _one_comment_lib.Add("にいちゃん！　豪華さのひけつは、ショップにヒントがあったかも？");
                }*/
                break;

            case 22: //アイス食べたい

                if (!GameMgr.ShopEvent_stage[7])
                {
                    _one_comment_lib.Add("にいちゃん！　アイスクリームについて、ショップのおねえちゃんに聞こう！");
                }
                break;

            case 30: //シュークリーム食べたい


                break;

            case 40: //ドーナツ食べたい


                break;

            case 50: //ステージ１ラスト　コンテスト開始


                break;

            default:
                break;
        }

        if(_one_comment_lib.Count <= 0)
        {
            _one_comment = "";
        } else
        {
            random = Random.Range(0, _one_comment_lib.Count);
            _one_comment = _one_comment_lib[random];
        }
        

        OneComment_text.text = _one_comment;
    }

    void CheckFirstZairyoNo()
    {
        //はじめて材料がなくなった時は、材料を買いにいけることを教えてくれる。
        if (pitemlist.KosuCount("komugiko") <= 1 || pitemlist.KosuCount("butter") <= 1 || pitemlist.KosuCount("suger") <= 1)
        {
            _one_comment_lib.Add("にいちゃん、材料が足りなくなってきたから、ショップへ、材料買いにいこ～よ～。");
            ev_yusen = true;
        }
    }

    public void WhiteMemoSave()
    {
        Debug.Log("白紙メモ　セーブ");

        //開かれている白紙めもの番号に応じてセーブするstringを変える

        switch (GameMgr.System_WhiteMemo_Num)
        {
            case 0:

                GameMgr.System_WhiteMemo_text[GameMgr.System_WhiteMemo_Num] = hakushi_inputField.text;
                break;

            case 1:

                GameMgr.System_WhiteMemo_text[GameMgr.System_WhiteMemo_Num] = hakushi_inputField.text;
                break;

            case 2:

                GameMgr.System_WhiteMemo_text[GameMgr.System_WhiteMemo_Num] = hakushi_inputField.text;
                break;
        }
    }

    //味のメモ　デフォルト
    public void OnMemoToggle_1()
    {
        hintpanel_obj1.SetActive(true);
        hintpanel_obj2.SetActive(false);

        Hikari_TasteHintDraw();
    }

    //味のメモ　コンテスト
    public void OnMemoToggle_2()
    {
        hintpanel_obj1.SetActive(true);
        hintpanel_obj2.SetActive(false);

        Contest_TasteHintDraw();
    }

    //白紙のメモ１
    public void OnMemoToggle_white1()
    {
        GameMgr.System_WhiteMemo_Num = 0;

        WhiteMemo_Draw();
    }

    //白紙のメモ２
    public void OnMemoToggle_white2()
    {
        GameMgr.System_WhiteMemo_Num = 1;
        
        WhiteMemo_Draw();
    }

    //白紙のメモ３
    public void OnMemoToggle_white3()
    {
        GameMgr.System_WhiteMemo_Num = 2;

        WhiteMemo_Draw();
    }

    //白紙のメモ４
    public void OnMemoToggle_white4()
    {
        GameMgr.System_WhiteMemo_Num = 3;

        WhiteMemo_Draw();
    }

    //白紙のメモ５
    public void OnMemoToggle_white5()
    {
        GameMgr.System_WhiteMemo_Num = 4;

        WhiteMemo_Draw();
    }

    void WhiteMemo_Draw()
    {
        hintpanel_obj1.SetActive(false);
        hintpanel_obj2.SetActive(true);

        hakushi_inputField.text = GameMgr.System_WhiteMemo_text[GameMgr.System_WhiteMemo_Num];
    }
}
