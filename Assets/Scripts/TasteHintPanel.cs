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
    private GameObject HikariIcon_Angry;
    private Text OneComment_text;
    private List<string> _one_comment_lib = new List<string>();
    private string _one_comment;

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

        Okashi_lasthint_text = this.transform.Find("HintPanel/HintText").GetComponent<Text>();
        Okashi_lasthint_text.text = GameMgr.Okashi_lasthint;

        Okashi_lastname_text = this.transform.Find("HintPanel/OkashiName").GetComponent<Text>();
        Okashi_lastname_text.text = GameMgr.Okashi_lastname;

        Okashi_lastscore_text = this.transform.Find("HintPanel/OkashiScore").GetComponent<Text>();
        Okashi_lastscore_text.text = GameMgr.Okashi_totalscore.ToString();

        Okashi_lastshokukan_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelA/PanelA_Param/Text").GetComponent<Text>();
        Okashi_lastshokukan_param_text.text = GameMgr.Okashi_lastshokukan_param.ToString();

        Okashi_lastshokukan_mes_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelA/PanelA_Title/Text").GetComponent<Text>();
        Okashi_lastshokukan_mes_text.text = GameMgr.Okashi_lastshokukan_mes;

        Okashi_lastsweat_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelB/PanelB_Param/Text").GetComponent<Text>();
        Okashi_lastsweat_param_text.text = GameMgr.Okashi_lastsweat_param.ToString();

        Okashi_lastsour_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelC/PanelC_Param/Text").GetComponent<Text>();
        Okashi_lastsour_param_text.text = GameMgr.Okashi_lastsour_param.ToString();

        Okashi_lastbitter_param_text = this.transform.Find("HintPanel/TasteParamScrollView/Viewport/Content/PanelD/PanelD_Param/Text").GetComponent<Text>();
        Okashi_lastbitter_param_text.text = GameMgr.Okashi_lastbitter_param.ToString();

        Okashi_Img = database.items[GameMgr.Okashi_lastID].itemIcon_sprite;
        Okashi_Icon = this.transform.Find("HintPanel/OkashiImage").GetComponent<Image>(); //画像アイコン
        Okashi_Icon.sprite = Okashi_Img;

        OneComment_text = this.transform.Find("HintPanel/OneCommentText").GetComponent<Text>();
        OneComment_text.text = "";
        RandomOneComment();

        HikariIcon_Angry = this.transform.Find("HintPanel/CharaIcon/HikariIcon2").gameObject;
        if(GameMgr.Okashi_totalscore <= 30)
        {
            HikariIcon_Angry.SetActive(true);
        }
        else
        {
            HikariIcon_Angry.SetActive(false);
        }
    }

    public void BackOption()
    {

        compound_Main.compound_status = 0;
        this.gameObject.SetActive(false);

    }

    void RandomOneComment()
    {
        _one_comment_lib.Clear();
        ev_yusen = false;

        switch (GameMgr.OkashiQuest_Num)
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
                }
                break;

            case 2: //かわいいクッキー

                CheckFirstZairyoNo();

                _one_comment_lib.Add("にいちゃん。きらきらの材料は、ショップのおねえちゃんが売ってたかも？");

                break;

            case 10: //ラスク食べたい


                break;

            case 11: //すっぱいラスク食べたい


                break;

            case 20: //クレープ食べたい


                break;

            case 21: //豪華なクレープ食べたい


                break;

            case 22: //アイスクリームを食べたい


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

        random = Random.Range(0, _one_comment_lib.Count);
        _one_comment = _one_comment_lib[random];

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
}
