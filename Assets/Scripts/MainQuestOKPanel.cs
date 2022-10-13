using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainQuestOKPanel : MonoBehaviour {

    private Button button;

    private ItemDataBase database;

    private Text stagenum_text;
    private Image okashiImage;

    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;

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

        girlEat_judge = GameObject.FindWithTag("GirlEat_Judge").GetComponent<GirlEat_Judge>();

        button = this.transform.Find("Panel1/Button").GetComponent<Button>();
        button.interactable = false;

        stagenum_text = this.transform.Find("Panel1/QuestClear/stageNumberText").GetComponent<Text>();
        stagenum_text.text = GameMgr.stage_quest_num.ToString();

        okashiImage = this.transform.Find("Panel1/ItemImgPanel/ItemImg").GetComponent<Image>();

        quest_panel = this.transform.Find("Panel1/QuestPanel").gameObject;
        
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
