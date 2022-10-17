using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtraQuestOKPanel : MonoBehaviour {

    private Button button;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private Girl1_status girl1_status;
    private SoundController sc;

    private Text stagenum_text;
    private Image okashiImage;
    private Text ExtraQuestText;
    private Text ExtraQuest_Score;
    private Text ExtraQuest_GetItemText;
    private Image ExtraQuest_GetItemImage;

    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;

    private GameObject Panel1;
    private GameObject Panel2;
    private GameObject Panel3;

    private GameObject quest_panel;

    private int ItemRank;

    private Dictionary<int, string> ItemGetDict;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        Panel1 = this.transform.Find("Panel1").gameObject;
        Panel1.SetActive(true);
        Panel2 = this.transform.Find("Panel2").gameObject;
        Panel2.SetActive(false);
        Panel3 = this.transform.Find("Panel3").gameObject;
        Panel3.SetActive(false);

        //パネル1
        button = this.transform.Find("Panel1/Button").GetComponent<Button>();
        button.interactable = false;

        stagenum_text = this.transform.Find("Panel1/QuestClear/stageNumberText").GetComponent<Text>();
        stagenum_text.text = GameMgr.stage_quest_num.ToString();

        okashiImage = this.transform.Find("Panel1/ItemImgPanel/ItemImg").GetComponent<Image>();

        quest_panel = this.transform.Find("Panel1/QuestPanel").gameObject;
        ExtraQuestText = quest_panel.transform.Find("Image/QuestClearText").GetComponent<Text>();
        ExtraQuestText.text = GameMgr.ExtraClear_QuestName;

        //パネル2
        ExtraQuest_Score = this.transform.Find("Panel2/QuestClearScore/ScoreText").GetComponent<Text>();
        ExtraQuest_Score.text = GameMgr.Okashi_spquest_MaxScore.ToString();

        //パネル3
        ExtraQuest_GetItemText = this.transform.Find("Panel3/ItemGetPanel/ItemText").GetComponent<Text>();
        ExtraQuest_GetItemImage = this.transform.Find("Panel3/ItemGetPanel/ItemImagePanel/ItemImage").GetComponent<Image>();

        if (GameMgr.Story_Mode == 0)
        {
            okashiImage.sprite = special_quest.OkashiQuest_sprite;
        }
        else
        {
            okashiImage.sprite = database.items[database.SearchItemID(GameMgr.SpecialQuestClear_okashiItemID)].itemIcon_sprite;
        }

        StartAnim(); //開いた最初のアニメ
        StartCoroutine("WaitButton");
    }

    IEnumerator WaitButton()
    {

        yield return new WaitForSeconds(1.0f); //1~2秒まったら、ボタンがおせるようになる。連打防止。
        button.interactable = true;
    }

    public void MainQuestOKButtonON()
    {
        Panel2.SetActive(true);
        Panel1.SetActive(false);
        //girlEat_judge.PanelResultOFF();
    }

    public void MainQuestOKButtonON2()
    {
        if (GameMgr.Okashi_spquest_MaxScore <= 100) //100点以下だと、アイテムもらえない
        {
            girlEat_judge.PanelResultOFF();
        }
        else
        {
            Panel3.SetActive(true);
            GetItemMethod();
        }
        
    }

    public void MainQuestOKButtonON3()
    {
        girlEat_judge.PanelResultOFF();
    }

    void GetItemMethod()
    {
        sc.PlaySe(86); //76

        if(GameMgr.Okashi_spquest_MaxScore >= 60 && GameMgr.Okashi_spquest_MaxScore < 100) //ランク１は使わない
        {
            ItemRank = 1;
        }
        else if (GameMgr.Okashi_spquest_MaxScore >= 100 && GameMgr.Okashi_spquest_MaxScore < 150)
        {
            ItemRank = 2;
        }
        else if (GameMgr.Okashi_spquest_MaxScore >= 150 && GameMgr.Okashi_spquest_MaxScore < 200)
        {
            ItemRank = 3;
        }
        else if (GameMgr.Okashi_spquest_MaxScore >= 200)
        {
            ItemRank = 4;
        }
        /*else if (GameMgr.Okashi_spquest_MaxScore >= 300)
        {
            ItemRank = 5;
        }*/

        ItemGetDict = new Dictionary<int, string>();
        switch (GameMgr.ExtraClear_QuestNum)
        {
            case 0: //ハートを100
               
                break;

            case 1: //300

                break;

            case 2: //650

                break;

            case 3: //1000

                ItemGetDict.Add(1, "shokukan_powerup1");
                ItemGetDict.Add(2, "shokukan_powerup1");
                ItemGetDict.Add(3, "shokukan_powerup2");
                ItemGetDict.Add(4, "shokukan_powerup3");
                ItemGetDict.Add(5, "shokukan_powerup3");
                break;

            case 4: //230点以上のスーパークレープ　条件分岐

                break;

            case 10: //茶色いクッキー

                ItemGetDict.Add(1, "cookie_powerup1");
                ItemGetDict.Add(2, "cookie_powerup2");
                ItemGetDict.Add(3, "cookie_powerup3");
                ItemGetDict.Add(4, "cookie_powerup4");
                ItemGetDict.Add(5, "cookie_powerup5");
                break;

            case 11: //お茶会用のお茶

                ItemGetDict.Add(1, "tea_powerup1");
                ItemGetDict.Add(2, "tea_powerup2");
                ItemGetDict.Add(3, "tea_powerup3");
                ItemGetDict.Add(4, "tea_powerup4");
                ItemGetDict.Add(5, "tea_powerup5");
                break;

            case 12: //ヒカリが3種類のお菓子を作れるようにする。

                ItemGetDict.Add(1, "hikari_powerup1");
                ItemGetDict.Add(2, "hikari_powerup1");
                ItemGetDict.Add(3, "hikari_powerup2");
                ItemGetDict.Add(4, "hikari_powerup3");
                ItemGetDict.Add(5, "hikari_powerup3");
                break;

            case 13: //カミナリのようにすっぱいクレープ 酸味が100以上か、絶妙にすっぱいときのクレープ　すっぱすぎてもクリアできる

                ItemGetDict.Add(1, "crepe_powerup1");
                ItemGetDict.Add(2, "crepe_powerup2");
                ItemGetDict.Add(3, "crepe_powerup3");
                ItemGetDict.Add(4, "crepe_powerup4");
                ItemGetDict.Add(5, "crepe_powerup5");
                break;

            case 14: //300点以上のいちごのクレープ

                break;

            case 20: //ハート3000以上

                break;

            case 21: //ムーディーな大人のおかし　カンノーリ　ティラミス　コーヒー　カフェオレシュー　ココアクッキー　ビスコッティ

                ItemGetDict.Add(1, "crepe_powerup1");
                ItemGetDict.Add(2, "otona_powerup1");
                ItemGetDict.Add(3, "candy_powerup1");
                ItemGetDict.Add(4, "candy_powerup1");
                ItemGetDict.Add(5, "crepe_powerup5");

                if(ItemRank == 4) //レシピの獲得
                {

                }
                break;

            case 22: //ヒカリが10種類のお菓子を作れる

                break;

            case 23: //200をこえる夢のようなパンケーキ

                break;

            case 24: //300点超えのプリンセストータ

                break;

            default: //

                break;
        }
        if(ItemGetDict.Count == 0)
        {
            ItemGetDict.Add(1, "neko_badge1");
            ItemGetDict.Add(2, "neko_badge2");
            ItemGetDict.Add(3, "neko_badge3");
            ItemGetDict.Add(4, "neko_badge4");
            ItemGetDict.Add(5, "neko_badge5");
        }


            //描画更新とアイテム取得処理
            ExtraQuest_GetItemText.text = database.items[database.SearchItemIDString(ItemGetDict[ItemRank])].itemNameHyouji;
            ExtraQuest_GetItemImage.sprite = database.items[database.SearchItemIDString(ItemGetDict[ItemRank])].itemIcon_sprite;
            pitemlist.addPlayerItem(ItemGetDict[ItemRank], 1);
        
    }


    void StartAnim()
    {
        //アニメーション
        //まず、初期値。
        Sequence sequence = DOTween.Sequence();
        quest_panel.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(quest_panel.transform.DOScale(new Vector3(0.65f, 0.65f, 1.0f), 0.0f)
            ); //

        //移動のアニメ
        sequence.Append(quest_panel.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f)
            .SetEase(Ease.OutElastic)); //はねる動き
                                        //.SetEase(Ease.OutExpo)); //スケール小からフェードイン
        sequence.Join(quest_panel.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    
}
