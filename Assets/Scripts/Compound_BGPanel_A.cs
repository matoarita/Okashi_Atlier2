using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Compound_BGPanel_A : MonoBehaviour {

    private GameObject canvas;

    private SoundController sc;
    
    private GameObject text_area;
    private Text _text;

    private GameObject MagicButton;
    private GameObject LearningButton;
    private GameObject HikariMakeButton;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject SelectCompo_panel_1;
    private GameObject BlackImage;

    private string originai_text;
    private string extreme_text;
    private string recipi_text;
    private string magic_text;
    private string hikarimake_text;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        //調合選択画面の取得
        SelectCompo_panel_1 = this.transform.Find("SelectPanel_1").gameObject;
        SelectCompo_panel_1.SetActive(false);

        //windowテキストエリアの取得
        text_area = this.transform.Find("MessageWindowComp").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //デフォルトではOFF
        HikariMakeButton = this.transform.Find("SelectPanel_1/Scroll View/Viewport/Content/HikariMakeButton").gameObject;
        HikariMakeButton.SetActive(false);
        MagicButton = this.transform.Find("SelectPanel_1/Scroll View/Viewport/Content/MagicButton").gameObject;
        MagicButton.SetActive(false);
        LearningButton = this.transform.Find("SelectPanel_1/Scroll View/Viewport/Content/LearningButton").gameObject;
        LearningButton.SetActive(false);

        BlackImage = this.transform.Find("BlackImage").gameObject;

        //各調合時のシステムメッセージ集
        originai_text = "新しくお菓子を作ろう！" + "\n" + "好きな材料を" + GameMgr.ColorYellow +
            "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";
        extreme_text = "仕上げをしよう！にいちゃん！ 一個目の材料を選んでね。";
        recipi_text = "ヒカリのお菓子手帳だよ！" + "\n" + "にいちゃんのレシピ　が増えたら、ここに書いてくね！";
        magic_text = "にいちゃん！　ふしぎな魔法をヒカリがかけてあげる！" + "\n" + "使いたい魔法を選んでね！";
        hikarimake_text = "にいちゃん！　ヒカリお菓子作りの手伝いしたいな！" + "\n" +
            "好きな材料を" + GameMgr.ColorYellow +
            "２つ" + "</color>" + "か" + GameMgr.ColorYellow + "３つ" + "</color>" + "選んでね。";

        //音ならす
        //sc.PlaySe(25); //25 鐘の音:50 キラリン:17

        if (GameMgr.picnic_event_reading_now)
        {
            this.transform.Find("SelectPanel_1/Picnic_yesno").gameObject.SetActive(true);
            this.transform.Find("SelectPanel_1/No").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("SelectPanel_1/Picnic_yesno").gameObject.SetActive(false);
            this.transform.Find("SelectPanel_1/No").gameObject.SetActive(true);
        }

        if (GameMgr.compound_select == 6)
        {
            //アニメーションスタート
            OnStartAnim();
        }
        else
        {
            this.transform.Find("SelectPanel_1").gameObject.SetActive(false);
        }

        //各コマンドのON/OFF
        CheckCommandFlag();
    }

    void CheckCommandFlag()
    {
        //ヒカリが外にいってるときは、選択できない

        //各コマンドのON/OFF
        if (GameMgr.System_HikariMakeUse_Flag) //ヒカリお菓子作り解禁
        {
            HikariMakeButton.SetActive(true);
        }

        if (GameMgr.System_MagicUse_Flag)
        {
            MagicButton.SetActive(true);
            LearningButton.SetActive(true);
        }

        if (GameMgr.outgirl_Nowprogress) //外出中
        {
            HikariMakeButton.GetComponent<Button>().interactable = false;
            MagicButton.GetComponent<Button>().interactable = false;
            LearningButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            HikariMakeButton.GetComponent<Button>().interactable = true;
            MagicButton.GetComponent<Button>().interactable = true;
            LearningButton.GetComponent<Button>().interactable = true;
        }
    }

    void OnStartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        SelectCompo_panel_1.SetActive(true);
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.transform.Find("SelectPanel_1").GetComponent<CanvasGroup>().alpha = 0;
        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(this.transform.Find("SelectPanel_1").transform.DOLocalMove(new Vector3(0f, 20f, 0), 0.0f)
            .SetRelative()); //元の位置から30px右に置いておく。

        //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/

        //まず画面をフェードイン
        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(1, 0.0f)
            .SetEase(Ease.OutQuart));

        //次にクエストタイトルを登場させる
        sequence.Join(this.transform.Find("SelectPanel_1").transform.DOLocalMove(new Vector3(0.0f, 0.0f, 0.0f), 0.7f)
            .SetEase(Ease.OutQuart)); //30px右から、元の位置に戻る。
        sequence.Join(this.transform.Find("SelectPanel_1").GetComponent<CanvasGroup>().DOFade(1, 0.3f)
             .SetEase(Ease.OutQuart));
    }

    //レシピ調合をON
    public void OnCheck_1_button()
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = recipi_text;
        GameMgr.compound_status = 1;
    }

    //トッピング調合をON
    public void OnCheck_2_button()
    {
        if (GameMgr.tutorial_ON == true)
        {
            if (GameMgr.tutorial_Num == 210) //エクストリーム説明中は、押しても反応なし
            {

            }
            else
            {
                card_view.DeleteCard_DrawView();
                SelectCompo_panel_1.SetActive(false);

                _text.text = extreme_text;
                GameMgr.compound_status = 2;

                if (GameMgr.tutorial_ON == true)
                {
                    if (GameMgr.tutorial_Num == 220)
                    {
                        GameMgr.tutorial_Progress = true;
                        GameMgr.tutorial_Num = 230;
                    }
                }
            }
        }
        else
        {
            card_view.DeleteCard_DrawView();
            SelectCompo_panel_1.SetActive(false);

            _text.text = extreme_text;
            GameMgr.compound_status = 2;

        }

    }

    //オリジナル調合をON
    public void OnCheck_3_button() //調合選択画面からボタンを選択して、オリジナル調合をON
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = originai_text;
        GameMgr.compound_status = 3;
    }

    //ヒカリに作らせる
    public void OnCheck_4_button() //調合選択画面からボタンを選択して、ヒカリにつくらせるをON
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        //_text.text = hikarimake_text;
        GameMgr.compound_status = 8;
    }

    //魔法を使う
    public void OnCheck_5_button() //魔法
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = magic_text;
        GameMgr.compound_status = 20;

        //sc.PlaySe(148); //魔法選択音
    }

    //魔法・スキルを覚える
    public void OnCheck_6_button()
    {
        card_view.DeleteCard_DrawView();
        SelectCompo_panel_1.SetActive(false);

        _text.text = magic_text;
        GameMgr.compound_status = 30;
    }

    public void BlackImageON()
    {
        BlackImage.GetComponent<CanvasGroup>().alpha = 1;
        BlackImage.GetComponent<Image>().raycastTarget = true;
    }

    public void BlackImageOFF()
    {
        BlackImage.GetComponent<CanvasGroup>().alpha = 0;
        BlackImage.GetComponent<Image>().raycastTarget = false;
    }

    /*public void OnCheck_4() //ブレンド調合をON
    {
        if (blend_toggle.GetComponent<Toggle>().isOn == true)
        {

            //Debug.Log("check4");
            _text.text = "ブレンド調合をします。まずはレシピを選ぶ。";
            compound_status = 5;
        }
    }*/

    /*public void OnCheck_5() //"焼き"をON
    {
        if (roast_toggle.GetComponent<Toggle>().isOn == true)
        {
            roast_toggle.GetComponent<Toggle>().isOn = false;

            card_view.DeleteCard_DrawView();

            _text.text = "作った生地を焼きます。焼きたい生地を選んでください。";
            GameMgr.compound_status = 5;
        }
    }*/

    //デバッグ用　コマンドを解禁する
    public void DebugCommandKaikinON()
    {
        GameMgr.System_HikariMakeUse_Flag = true;
        GameMgr.System_MagicUse_Flag = true;

        CheckCommandFlag();
    }
}
