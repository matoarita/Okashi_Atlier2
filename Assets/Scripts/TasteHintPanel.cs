using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TasteHintPanel : MonoBehaviour {

    private Compound_Main compound_Main;
    private Girl1_status girl1_status;
    private PlayerItemList pitemlist;

    private ItemDataBase database;

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
    private InputField hakushi_inputField;

    private GameObject hintpanel_obj1;
    private GameObject hintpanel_obj2;

    private int random;
    private bool ev_yusen;

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

        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        hintpanel_obj1 = this.transform.Find("HintPanel/Panel_1").gameObject;
        hintpanel_obj1.SetActive(true);
        hintpanel_obj2 = this.transform.Find("HintPanel/Panel_2").gameObject;
        hintpanel_obj2.SetActive(false);

        Okashi_lasthint_text = hintpanel_obj1.transform.Find("HintText").GetComponent<Text>();
        Okashi_lasthint_text.text = GameMgr.Okashi_lasthint;

        Okashi_lastname_text = hintpanel_obj1.transform.Find("OkashiName").GetComponent<Text>();
        Okashi_lastname_text.text = GameMgr.ColorGold + GameMgr.Okashi_lastslot + "</color>" + GameMgr.Okashi_lastname;

        Okashi_lastscore_text = hintpanel_obj1.transform.Find("OkashiScore").GetComponent<Text>();
        Okashi_lastscore_text.text = GameMgr.Okashi_last_totalscore.ToString();

        Okashi_lastshokukan_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelA/PanelA_Param/Text").GetComponent<Text>();
        Okashi_lastshokukan_param_text.text = GameMgr.Okashi_lastshokukan_param.ToString();

        Okashi_lastshokukan_mes_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelA/PanelA_Title/Text").GetComponent<Text>();
        Okashi_lastshokukan_mes_text.text = GameMgr.Okashi_lastshokukan_mes;

        Okashi_lastsweat_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelB/PanelB_Param/Text").GetComponent<Text>();
        Okashi_lastsweat_param_text.text = GameMgr.Okashi_lastsweat_param.ToString();

        Okashi_lastsour_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelC/PanelC_Param/Text").GetComponent<Text>();
        Okashi_lastsour_param_text.text = GameMgr.Okashi_lastsour_param.ToString();

        Okashi_lastbitter_param_text = hintpanel_obj1.transform.Find("TasteParamScrollView/Viewport/Content/PanelD/PanelD_Param/Text").GetComponent<Text>();
        Okashi_lastbitter_param_text.text = GameMgr.Okashi_lastbitter_param.ToString();

        Okashi_Img = database.items[GameMgr.Okashi_lastID].itemIcon_sprite;
        Okashi_Icon = hintpanel_obj1.transform.Find("OkashiImage").GetComponent<Image>(); //画像アイコン
        Okashi_Icon.sprite = Okashi_Img;

        OneComment_text = hintpanel_obj1.transform.Find("OneCommentText").GetComponent<Text>();
        OneComment_text.text = "";
        RandomOneComment();

        NowEat_text = hintpanel_obj1.transform.Find("NowEatText").GetComponent<Text>();
        NowEat_text.text = GameMgr.NowEatOkashiName;

        //顔アイコン
        HikariIcon_Normal = hintpanel_obj1.transform.Find("CharaIcon/HikariIcon1").gameObject;
        HikariIcon_Angry = hintpanel_obj1.transform.Find("CharaIcon/HikariIcon2").gameObject;
        if(GameMgr.Okashi_totalscore <= 30)
        {
            HikariIcon_Normal.SetActive(false);
            HikariIcon_Angry.SetActive(true);
        }
        else
        {
            HikariIcon_Normal.SetActive(true);
            HikariIcon_Angry.SetActive(false);
        }

        //白紙メモ関係
        hakushi_inputField = hintpanel_obj2.transform.Find("Scroll View/Viewport/Content/InputField(Legacy)").GetComponent<InputField>();
        GameMgr.System_WhiteMemo_Num = 0;
    }

    public void BackOption()
    {

        GameMgr.compound_status = 0;
        this.gameObject.SetActive(false);

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
        //開かれている白紙めもの番号に応じてセーブするstringを変える

        switch (GameMgr.System_WhiteMemo_Num)
        {
            case 0:

                GameMgr.System_WhiteMemo_text[GameMgr.System_WhiteMemo_Num] = hakushi_inputField.text;
                break;
        }
    }

    //味のメモ　デフォルト
    public void OnMemoToggle_1()
    {
        hintpanel_obj1.SetActive(true);
        hintpanel_obj2.SetActive(false);
    }

    //白紙のメモ１
    public void OnMemoToggle_white1()
    {
        hintpanel_obj1.SetActive(false);
        hintpanel_obj2.SetActive(true);

        GameMgr.System_WhiteMemo_Num = 0;

        hakushi_inputField.text = GameMgr.System_WhiteMemo_text[GameMgr.System_WhiteMemo_Num];
    }
}
