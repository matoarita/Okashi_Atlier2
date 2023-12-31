﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExtraQuestOKPanel : MonoBehaviour {

    private GameObject canvas;

    private Button button1;
    private Button button2;

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private Girl1_status girl1_status;
    private SoundController sc;

    private Text stagenum_text;
    private Image okashiImage;
    private Text ExtraQuestText;

    private GirlEat_Judge girlEat_judge;
    private Special_Quest special_quest;

    private GameObject Panel1;

    private GameObject extra_quest_treasurepanel;
    private GameObject quest_panel;

    private int ItemRank;

    private Dictionary<int, string> ItemGetDict;

    private Tween coinTween;
    private int currentDispCoin;
    private int preDispCoin;

    private float countTime;
    

    private Transform resulttransform;

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

        canvas = GameObject.FindWithTag("Canvas");

        Panel1 = this.transform.Find("Panel1").gameObject;
        Panel1.SetActive(true);

        extra_quest_treasurepanel = canvas.transform.Find("ScoreHyoujiPanel/ExtraQuestTreasurePanel").gameObject;
        extra_quest_treasurepanel.SetActive(false);

        //パネル1
        button1 = this.transform.Find("Panel1/Button").GetComponent<Button>();
        button1.interactable = false;

        stagenum_text = this.transform.Find("Panel1/QuestClear/stageNumberText").GetComponent<Text>();
        stagenum_text.text = GameMgr.stage_quest_num.ToString();

        okashiImage = this.transform.Find("Panel1/ItemImgPanel/ItemImg").GetComponent<Image>();

        quest_panel = this.transform.Find("Panel1/QuestPanel").gameObject;
        ExtraQuestText = quest_panel.transform.Find("Image/QuestClearText").GetComponent<Text>();
        ExtraQuestText.text = GameMgr.ExtraClear_QuestName;

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
        button1.interactable = true;
    }

    public void MainQuestOKButtonON()
    {
        extra_quest_treasurepanel.SetActive(true);
        Panel1.SetActive(false);
        //girlEat_judge.PanelResultOFF();

        //宝箱演出パネルへ処理うつる。数字演出処理へ。
        extra_quest_treasurepanel.GetComponent<ExtraQuestTreasurePanel>().TreasurePanel_Start();
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
        sequence.Append(quest_panel.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.5f)
            .SetEase(Ease.OutElastic)); //はねる動き
                                        //.SetEase(Ease.OutExpo)); //スケール小からフェードイン
        sequence.Join(quest_panel.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    
}
