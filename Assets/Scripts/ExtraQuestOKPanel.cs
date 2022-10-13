using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtraQuestOKPanel : MonoBehaviour {

    private Button button;

    private ItemDataBase database;
    private Girl1_status girl1_status;

    private Text stagenum_text;
    private Image okashiImage;
    private Text ExtraQuestText;

    private Text ExtraQuest_Score;

    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;

    private GameObject Panel1;
    private GameObject Panel2;

    private GameObject quest_panel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //スペシャルお菓子クエストの取得
        special_quest = Special_Quest.Instance.GetComponent<Special_Quest>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        Panel1 = this.transform.Find("Panel1").gameObject;
        Panel1.SetActive(true);
        Panel2 = this.transform.Find("Panel2").gameObject;
        Panel2.SetActive(false);

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
        girlEat_judge.PanelResultOFF();
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
