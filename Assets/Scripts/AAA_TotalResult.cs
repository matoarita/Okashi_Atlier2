using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using DG.Tweening;

public class AAA_TotalResult : MonoBehaviour {

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private PlayerItemList pitemlist;
    private SoundController sc;
    private BGM sceneBGM;

    private GameObject canvas;

    private GameObject PanelMaster;
    private GameObject TotalResult_panel1;
    private GameObject TotalResult_panel2;
    private GameObject TotalResult_panel3;
    private GameObject Contest_scorepanel;
    private GameObject playRank;
    private GameObject playRankAnim;
    private GameObject hukidashi;

    private GameObject button_panel1;
    private GameObject button_panel2;
    private GameObject button_panel3;
    private GameObject button_panel4;
    private GameObject button_panel5;

    private GameObject TitleButton;

    private GameObject Effect_1;
    private GameObject Effect_2;

    private GameObject BG_panel;
    private GameObject BG_1;
    private GameObject BG_2;

    private PlayableDirector playableDirector;

    private Animator chara_animator;
    private Sprite charaIcon_sprite_1;
    private Sprite charaIcon_sprite_2;
    private Sprite charaIcon_sprite_3;
    private Sprite charaIcon_sprite_4;
    private Sprite charaIcon_sprite_5;
    private Sprite charaIcon_sprite_6;
    private GameObject chara_Icon;

    //Live2Dモデルの取得    
    private GameObject _model_root_obj;
    private GameObject _model_obj;
    private GameObject _model_move;
    private CubismRenderController cubism_rendercontroller;
    private Animator live2d_animator;

    private Text girllv_param_text;
    private Text girl_exp_param_text;
    private Text total_recipi_count_text;
    private Text total_collection_count_text;
    private Text total_costume_count_text;
    private Text total_okashiHighScore_text;
    private float total_costume_per;
    private string _rank;
    private Text player_rank_text;
    private Text player_rankanim_text;
    private Text player_shogo_text;
    private string player_shogo;
    private bool Live2Dexp_flag;

    private Sprite texture2d;
    private GameObject ClearImg_obj;
    private Image ClearImg;

    private GameObject ClearItemTextPanel;
    private Text ClearItemName;

    private Girl1_status girl1_status;

    private List<GameObject> ed_view_list = new List<GameObject>();
    private Dictionary<int, int> EDList;

    private int total_score;

    private Text contest_score_text;

    private int _collection_count;
    private int i;
    private int page_cullent;
    private string _hukidashi_content;

    private Tween coinTween;
    private Sequence sequence;
    private Sequence sequence2;
    private int currentDispCoin;
    private int preDispCoin;
    private float countTime;

    private bool panel3_anim_start;

    private bool Live2D_USE = true; //Live2Dモデルを使うかどうか。画像自体は、ヒエラルキー上でON/OFFする。

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //BGMの取得
        sceneBGM = GameObject.FindWithTag("BGM").gameObject.GetComponent<BGM>();

        BG_panel = GameObject.FindWithTag("BG").gameObject;
        BG_1 = BG_panel.transform.Find("Title_BG").gameObject;
        BG_2 = BG_panel.transform.Find("Title_BG_Black").gameObject;
        BG_2.SetActive(false);

        foreach (Transform child in canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/EDLastScoreView/Viewport/Content/EDlastscoreList4/ED_View/Viewport/Content").transform) //
        {
            ed_view_list.Add(child.gameObject);
        }

        //パネルのメイン
        PanelMaster = canvas.transform.Find("ResultGroup").gameObject;
        TotalResult_panel1 = canvas.transform.Find("ResultGroup/ResultPanel_1").gameObject;
        TotalResult_panel2 = canvas.transform.Find("ResultGroup/ResultPanel_2").gameObject;
        TotalResult_panel3 = canvas.transform.Find("ResultGroup/ResultPanel_3").gameObject;
        //TotalResult_panel3.SetActive(false);

        //パネル１
        contest_score_text = canvas.transform.Find("ResultGroup/ResultPanel_1/ContestScorePanel/ContestScore").GetComponent<Text>();
        ClearImg_obj = canvas.transform.Find("ResultGroup/ResultPanel_1/ContestOkashiPanel/ClearItemIcon/ClearItemIconImg").gameObject;
        ClearImg = canvas.transform.Find("ResultGroup/ResultPanel_1/ContestOkashiPanel/ClearItemIcon/ClearItemIconImg").GetComponent<Image>(); //アイテムの画像データ
        ClearItemTextPanel = canvas.transform.Find("ResultGroup/ResultPanel_1/ClearItemTextPanel").gameObject;
        ClearItemName = canvas.transform.Find("ResultGroup/ResultPanel_1/ClearItemTextPanel/Panel/ClearItemText").GetComponent<Text>();
        Contest_scorepanel = canvas.transform.Find("ResultGroup/ResultPanel_1/ContestScorePanel").gameObject;
        Effect_1 = canvas.transform.Find("ResultGroup/ResultPanel_1/ParticleQClearEffect").gameObject;
        Effect_1.SetActive(false);
        Effect_2 = canvas.transform.Find("ResultGroup/ResultPanel_1/Particle_KiraExplode").gameObject;
        Effect_2.SetActive(false);
        button_panel1 = canvas.transform.Find("ResultGroup/ResultPanel_1/ButtonPanel_1").gameObject;

        //パネル２
        girllv_param_text = canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/Hlv_Param").GetComponent<Text>();
        girl_exp_param_text = canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/TotalHeart_Param").GetComponent<Text>();

        total_recipi_count_text = canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/EDLastScoreView/Viewport/Content/EDlastscoreList/TotalRecipiCount").GetComponent<Text>();
        //total_collection_count_text = canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/TotalCollection").GetComponent<Text>();
        total_costume_count_text = canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/EDLastScoreView/Viewport/Content/EDlastscoreList2/TotalCostume").GetComponent<Text>();
        total_okashiHighScore_text = canvas.transform.Find("ResultGroup/ResultPanel_2/ImageBG/EDLastScoreView/Viewport/Content/EDlastscoreList3/GameTotalHighScore").GetComponent<Text>();
        button_panel2 = canvas.transform.Find("ResultGroup/ResultPanel_2/ButtonPanel_2").gameObject;
        button_panel3 = canvas.transform.Find("ResultGroup/ResultPanel_2/ButtonPanel_3").gameObject;

        //パネル３
        player_rank_text = canvas.transform.Find("ResultGroup/ResultPanel_3/ImageBG/PlayerRankAnim/PlayerRank").GetComponent<Text>();
        player_rankanim_text = canvas.transform.Find("ResultGroup/ResultPanel_3/ImageBG/PlayerRankAnim/StageClear_Button/TextPlate/PlayerRankAnim").GetComponent<Text>();
        player_shogo_text = canvas.transform.Find("ResultGroup/ResultPanel_3/ImageBG/ShogoPanel/PlayerShogo").GetComponent<Text>();
        button_panel4 = canvas.transform.Find("ResultGroup/ResultPanel_3/ButtonPanel_4").gameObject;
        button_panel5 = canvas.transform.Find("ResultGroup/ResultPanel_3/ButtonPanel_5").gameObject;
        TitleButton = canvas.transform.Find("ResultGroup/ResultPanel_3/TitleButtonPanel").gameObject;
        playRankAnim = canvas.transform.Find("ResultGroup/ResultPanel_3/ImageBG/PlayerRankAnim").gameObject;
        playRank = canvas.transform.Find("ResultGroup/ResultPanel_3/ImageBG/PlayerRankAnim/PlayerRank").gameObject;
        hukidashi = canvas.transform.Find("ResultGroup/ResultPanel_3/hukidashiPanel").gameObject;
        hukidashi.SetActive(false);

        playableDirector = playRankAnim.GetComponent<PlayableDirector>();
        playableDirector.enabled = false;

        //キャラ系
        charaIcon_sprite_1 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_01");
        charaIcon_sprite_2 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_02");
        charaIcon_sprite_3 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_03");
        charaIcon_sprite_4 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_04");
        charaIcon_sprite_5 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_05");
        charaIcon_sprite_6 = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/hikari_saiten_face_06");        
        chara_Icon = canvas.transform.Find("ResultGroup/ResultPanel_3/CharaImgAnim/chara_Img").gameObject;
        chara_Icon.GetComponent<Image>().sprite = charaIcon_sprite_1;
        chara_Icon.SetActive(false);

        //Live2Dモデルの取得
        _model_root_obj = GameObject.FindWithTag("CharacterRoot").gameObject;
        _model_move = _model_root_obj.transform.Find("CharacterMove").gameObject;
        _model_obj = _model_root_obj.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;
        live2d_animator = _model_obj.GetComponent<Animator>();
        cubism_rendercontroller = _model_obj.GetComponent<CubismRenderController>();

        _model_obj.SetActive(true);                       
        _model_move.SetActive(false);              
        
        chara_animator = _model_root_obj.GetComponent<Animator>();
        chara_animator.SetInteger("trans_anim", 0);        


        //総合得点
        total_score = 0;

        //現在閲覧中のページ
        page_cullent = 1;

        panel3_anim_start = false;
        Live2Dexp_flag = false;

        //** デバッグ用 **/
        //DebugParam();
        // *** //

        //★エンディング　各スコアの計算　重要
        KeisanParam();

        //パネル１からのアニメーション自動スタート　デバッグでなければ、これをオンにする。
        Panel1_Action();

        
        if (GameMgr.ending_number == 1) //Bad EDのときはいなくなる。
        {
            _model_obj.SetActive(false);
            chara_Icon.SetActive(false);
        }
    }

    void DebugParam()
    {
        //Panel3_Action();     
        GameMgr.ending_number = 4;
        GameMgr.contest_TotalScore = 130;
        PlayerStatus.girl1_Love_exp = 1300;
        GameMgr.contest_okashiNameHyouji = "ストロベリークッキー";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if (Live2Dexp_flag)
        {
            Live2Dexp_flag = false;

            Live2DExpression();
        }
    }

    //
    //** パネル１アクション **//
    //

    void Panel1_Action()
    {
        //演出メモ
        //①アイテムフレームとアイテム画像が真ん中からぽわん！　その後、作ったお菓子はふわふわ浮いている。周りには★たち。       
        //②その後、アイテム名が、上からフェードイン。①→②はほぼウェイトなし。
        //③点数がピロロロロロロ　ジャン　
        //④半ウェイトおいて、タリラリー♪と、キラエフェクト（点数に応じて、音が変わる。）
        //⑤少しおいて、右にネクストボタンがフェードイン


        //動くものは最初、表示をオフ。
        sequence = DOTween.Sequence();

        contest_score_text.GetComponent<CanvasGroup>().alpha = 0;
        Contest_scorepanel.GetComponent<CanvasGroup>().alpha = 0;
        button_panel1.GetComponent<CanvasGroup>().alpha = 0;
        button_panel1.SetActive(false);
        ClearImg_obj.GetComponent<CanvasGroup>().alpha = 0;
        ClearItemTextPanel.GetComponent<CanvasGroup>().alpha = 0;
        contest_score_text.text = "0";

        //カウントアップのための秒数を割り出す。
        countTime = GameMgr.contest_TotalScore * 0.03f; //1ごとに0.03fで表示する

        //①②
        StartCoroutine("panel1_anim1");
    }

    IEnumerator panel1_anim1()
    {
        //①②のアニメが終了待ち
        while (!TotalResult_panel1.GetComponent<TotalResultPanel_1>().end_resultanim_1)
        {
            yield return null;
        }

        TotalResult_panel1.GetComponent<TotalResultPanel_1>().end_resultanim_1 = false;

        yield return new WaitForSeconds(1.0f);

        //③点数アニメ開始
        sequence.Append(Contest_scorepanel.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));

        sequence.Append(Contest_scorepanel.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f)
            .SetEase(Ease.OutExpo));
        sequence.Join(Contest_scorepanel.GetComponent<CanvasGroup>().DOFade(1, 0.2f));

        contest_score_text.GetComponent<CanvasGroup>().alpha = 1;
        UpdateCoin(GameMgr.contest_TotalScore);
    }

    //③数字演出
    void UpdateCoin(int num)
    {
        DOTween.Kill(coinTween);
        coinTween = DOTween.To(() => currentDispCoin, (val) =>
        {
            //Debug.Log("bang");
            currentDispCoin = val;

            if (currentDispCoin < GameMgr.low_score) //文字色をかえる。
            {
                contest_score_text.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
                //白(255f / 255f, 255f / 255f, 255f / 255f)　(129f / 255f, 87f / 255f, 60f / 255f) 青文字(105f / 255f, 168f / 255f, 255f / 255f)      
            }
            else if (currentDispCoin >= GameMgr.low_score && currentDispCoin < GameMgr.high_score)
            {
                contest_score_text.color = new Color(255f / 255f, 252f / 255f, 158f / 255f); //うす黄色

            }
            else
            {
                contest_score_text.color = new Color(255f / 255f, 252f / 255f, 158f / 255f); //うす黄色　ピンク(255f / 255f, 105f / 255f, 139f / 255f)
            }

            contest_score_text.text = string.Format("{0:#,0}", val);

            if (currentDispCoin != preDispCoin)
            {
                sc.PlaySe(37); //トゥルルルルという文字送り音
            }
            preDispCoin = currentDispCoin; //前回の値も保存
        }, num, countTime).SetEase(Ease.OutQuart)
        .OnComplete(EndCountUpAnim); //エンドアニメ　再生終了時;
    }

    //④数字がすべて表示された後のアニメ
    void EndCountUpAnim()
    {
        //音を鳴らす
        //sc.PlaySe(4);
        sc.PlaySe(27);
        sc.PlaySe(78);
        sc.PlaySe(17);
        sc.PlaySe(19);

        Effect_1.SetActive(true); //エフェクト発生
        Effect_2.SetActive(true);

        //お菓子のアニメーション
        OkashiTojoAnim();       

        StartCoroutine("panel1_anim2");
    }

    //⑤ネクストボタン登場
    IEnumerator panel1_anim2()
    {
        yield return new WaitForSeconds(1.0f);

        button_panel1.SetActive(true);
        sequence.Append(button_panel1.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    void OkashiTojoAnim()
    {
        //まず、初期値。
        Sequence sequence = DOTween.Sequence();
        ClearImg_obj.GetComponent<CanvasGroup>().alpha = 0;
        ClearItemTextPanel.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(ClearImg_obj.transform.DOScale(new Vector3(0.5f, 0.5f, 1.0f), 0.0f)
            );
        sequence.Join(ClearItemTextPanel.transform.DOScale(new Vector3(0.5f, 0.5f, 1.0f), 0.0f)
            );//

        //移動のアニメ
        sequence.Append(ClearImg_obj.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.5f)
            .SetEase(Ease.OutElastic)); //はねる動き
                                        //.SetEase(Ease.OutExpo)); //スケール小からフェードイン
        sequence.Join(ClearImg_obj.transform.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        sequence.Join(ClearItemTextPanel.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 1.5f)
            .SetEase(Ease.OutElastic)); //はねる動き
        sequence.Join(ClearItemTextPanel.transform.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }



    //
    //** パネル２アクション **//
    //


    void Panel2_Action()
    {
        //①まず、左、パラメータテキストは見えてない状態からスタート。
        //②ハートレベルから順番にピロロロ　ジャン　→　少しウェイト　→　ピロロロ　ジャン.. 繰り返し
        //なくても良い。
    }




    //
    //** パネル３アクション **//
    //

    void Panel3_Action()
    {
        //①少しウェイトおいて、キラキラキララ～♪　エフェクト波紋が広がって、ランク登場。パティシエランク「なんとか！」
        //②ウェイト　タラララーーーン！！　称号が登場　シメ。
        //③ほぼ同時に、右のヒカリちゃんが、登場。採点と同じ登場でＯＫ。　ランクによって、笑顔や表情が変わる。
        //④吹き出しがピコンとでて、中に、「ありがとう～」のメッセージ。

        panel3_anim_start = true;

        button_panel4.GetComponent<CanvasGroup>().alpha = 0;
        button_panel5.GetComponent<CanvasGroup>().alpha = 0;
        //TitleButton.GetComponent<CanvasGroup>().alpha = 0;
        TotalResult_panel3.transform.Find("ImageBG").GetComponent<CanvasGroup>().alpha = 0;

        button_panel4.SetActive(false);
        button_panel5.SetActive(false);
        //TitleButton.SetActive(false);

        //①②キラキラ～　ランク登場
        StartCoroutine("panel3_anim1_Start");

    }

    IEnumerator panel3_anim1_Start()
    {
        yield return new WaitForSeconds(0.5f);

        panel3_anim1();
    }

    void panel3_anim1()
    {
        sc.PlaySe(30);

        //まず、パティシエのウィンドウがでる。
        sequence2 = DOTween.Sequence();

        sequence2.Append(TotalResult_panel3.transform.Find("ImageBG").DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //

        sequence2.Append(TotalResult_panel3.transform.Find("ImageBG").DOLocalMove(new Vector3(50f, 0f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)
            .OnComplete(() => End_P3ImageBGmove()));
        sequence2.Join(TotalResult_panel3.transform.Find("ImageBG").GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    
    void End_P3ImageBGmove()
    {
        //ランクが登場する演出
        StartCoroutine("effectAnim");
    }

    IEnumerator effectAnim()
    {
        yield return new WaitForSeconds(1.0f);

        playableDirector.enabled = true;
        playableDirector.Play();
        sc.PlaySe(72); //ための音　登場時の音はTimeline上で設定している。3　72

        yield return new WaitForSeconds(6.0f);

        playableDirector.enabled = false;
        playableDirector.time = 0;
        //sc.StopSe();

        //④吹き出しとキャラクタの表情アニメ登場

        sc.PlaySe(17);
        sc.PlaySe(43);
        hukidashi.SetActive(true);

        EDHukidashiText(); //テキスト内容決定       
        hukidashi.GetComponent<TextController>().SetText(_hukidashi_content);

        //キャラアニメ
        _model_move.SetActive(true);       
        CharaAnim();

        yield return new WaitForSeconds(0.5f);

        //⑤遅れて、バックボタンとタイトルボタンも登場

        button_panel4.SetActive(true);
        sequence.Append(button_panel4.GetComponent<CanvasGroup>().DOFade(1, 0.3f));
        button_panel5.SetActive(true);
        sequence.Append(button_panel5.GetComponent<CanvasGroup>().DOFade(1, 0.3f));

        yield return new WaitForSeconds(0.5f);

        //TitleButton.SetActive(true);
        //sequence.Append(TitleButton.GetComponent<CanvasGroup>().DOFade(1, 0.3f));
    }





    void KeisanParam()
    {
        //コンテストクリア時のアイテムと名前　コンテストスコア
        ClearItemName.text = GameMgr.contest_okashiSlotName + GameMgr.contest_okashiNameHyouji;
        ClearImg.sprite = database.items[GameMgr.contest_okashiID].itemIcon_sprite;
        //contest_score_text.text = GameMgr.contest_TotalScore.ToString();

        //ハート総数とレベル
        girllv_param_text.text = PlayerStatus.girl1_Love_lv.ToString();
        girl_exp_param_text.text = PlayerStatus.girl1_Love_exp.ToString();

        //レシピパーセント表示
        databaseCompo.RecipiCount_database();
        total_recipi_count_text.text = GameMgr.game_Recipi_archivement_rate.ToString("f2") + "%";
        //GameMgr.game_Cullent_recipi_count + " / " + GameMgr.game_All_recipi_count

        //コレクションアイテムの総数を計算
        /*_collection_count = 0;
        for (i = 0; i < GameMgr.CollectionItems.Count; i++)
        {
            if (GameMgr.CollectionItems[i])
            {
                _collection_count++;
            }
        }*/
        //total_collection_count_text.text = _collection_count.ToString() + " / " + GameMgr.CollectionItems.Count.ToString();

        //衣装総数を計算
        total_costume_count_text.text = pitemlist.emeralditemlist_CostumeCount().ToString() + " / " + pitemlist.emeralditemlist_CostumeAllCount().ToString();
        total_costume_per = pitemlist.emeralditemlist_CostumeCount() / pitemlist.emeralditemlist_CostumeAllCount();

        //ゲーム中最高スコアを表示
        total_okashiHighScore_text.text = GameMgr.Okashi_toplast_score.ToString();

        //EDタイプの計算
        ChangeEDNumArray();
        if (GameMgr.ending_number < 5)
        {
            ed_view_list[EDList[GameMgr.ending_number - 1]].transform.Find("Text1_on").gameObject.SetActive(true);
        }
        else
        {
            //特殊エンドの場合。ないかも。
        }

        //上記のパラメータをもとに、ゲームトータルスコアを計算
        //ハート総数+コンテストスコア＋コスチュームの数*100
        //total_score = GameMgr.contest_TotalScore + PlayerStatus.girl1_Love_exp + (pitemlist.emeralditemlist_CostumeCount() * 100);
        total_score = PlayerStatus.girl1_Love_exp;

        //パティシエランク計算　トータルスコアをもとに、SS S A B C D 6段階
        player_rank_text.text = "";
        player_shogo = "-";

        //特殊な称号を取得してた場合、そっちが優先される。
        if (GameMgr.special_shogo_flag)
        {
            _rank = "★";
            switch (GameMgr.special_shogo_num)
            {
                case 0: //スカーレット

                    player_shogo = GameMgr.SearchTitleCollectionNameString("title100");
                    GameMgr.SetTitleCollectionFlag("title100", true);
                    break;

                case 1: //ホワイトプリム

                    player_shogo = GameMgr.SearchTitleCollectionNameString("title101");
                    GameMgr.SetTitleCollectionFlag("title101", true);
                    break;

                case 2: //ハイルング

                    player_shogo = GameMgr.SearchTitleCollectionNameString("title103");
                    GameMgr.SetTitleCollectionFlag("title103", true);
                    break;

                case 3: //ブルーヴェール

                    player_shogo = GameMgr.SearchTitleCollectionNameString("title102");
                    GameMgr.SetTitleCollectionFlag("title102", true);
                    break;

                case 4: //ししゃもマニア

                    player_shogo = GameMgr.SearchTitleCollectionNameString("title104");
                    GameMgr.SetTitleCollectionFlag("title104", true);
                    break;

                case 5: //ゴールドマスター

                    player_shogo = GameMgr.SearchTitleCollectionNameString("title105");
                    GameMgr.SetTitleCollectionFlag("title105", true);
                    break;
            }
        }
        else
        {
            //ハートが7777以上　かつ　レシピ75%以上達成　かつ　コンテストで優勝
            if (total_score >= 7777 && GameMgr.game_Recipi_archivement_rate >= 75.0f && GameMgr.Contest_yusho_flag)
            {
                _rank = "SS";
                player_shogo = GameMgr.SearchTitleCollectionNameString("title7");
                GameMgr.SetTitleCollectionFlag("title7", true);
            }
            else
            {
                if (total_score < 400)
                {
                    _rank = "D";
                    player_shogo = GameMgr.SearchTitleCollectionNameString("title1");
                    GameMgr.SetTitleCollectionFlag("title1", true);
                }
                else if (total_score >= 400 && total_score < 800)
                {
                    _rank = "C";
                    player_shogo = GameMgr.SearchTitleCollectionNameString("title3");
                    GameMgr.SetTitleCollectionFlag("title3", true);
                }
                else if (total_score >= 800 && total_score < 1500)
                {
                    _rank = "B";
                    player_shogo = GameMgr.SearchTitleCollectionNameString("title4");
                    GameMgr.SetTitleCollectionFlag("title4", true);
                }
                else if (total_score >= 1500 && total_score < 2500)
                {
                    _rank = "A";
                    player_shogo = GameMgr.SearchTitleCollectionNameString("title5");
                    GameMgr.SetTitleCollectionFlag("title5", true);
                }
                else if (total_score >= 2500)
                {
                    _rank = "S";
                    player_shogo = GameMgr.SearchTitleCollectionNameString("title6");
                    GameMgr.SetTitleCollectionFlag("title6", true);
                }
            }
            
        }
        

        player_rank_text.text = _rank;
        player_rankanim_text.text = _rank;

        //称号計算　通常は、パティシエランクに合わせて決める。特別な条件をクリアすると、特殊な称号がもらえるようにする。
        player_shogo_text.text = "";
        player_shogo_text.text = player_shogo;
    }

    public void OnEndSceneButton()
    {
        sceneBGM.FadeOutBGM();
        FadeManager.Instance.LoadScene("120_AutoSave", 2.0f);
    }

    public void OnNextButton()
    {
        switch(page_cullent)
        {
            case 1:

                page_cullent = 2;
                break;

            case 2:

                page_cullent = 3;                               
                break;

            case 3:

                page_cullent = 4;
                break;
        }

        PageStatus();

        PanelMove();
    }

    public void OnBackButton()
    {
        switch (page_cullent)
        {
            case 2:

                page_cullent = 1;
                
                break;

            case 3:

                page_cullent = 2;
                
                break;

            case 4:

                page_cullent = 3;

                break;
        }

        PageStatus();

        PanelMove();
    }
    
    void PageStatus()
    {
        switch (page_cullent)
        {
            case 1:

                Effect_1.SetActive(true);

                break;

            case 2:

                Effect_1.SetActive(false);

                break;

            case 3:

                Effect_1.SetActive(false);
                //BG_1.SetActive(true);
                //BG_2.SetActive(false);

                TotalResult_panel3.SetActive(true);

                if (!panel3_anim_start)//アニメスタート
                {
                    Panel3_Action();
                }
                else
                {
                    _model_move.SetActive(true);
                    Live2Dexp_flag = true; //Live2D表情変更。LateUpdateで行う。
                }
                break;

            case 4:

                _model_move.SetActive(false);
                //BG_1.SetActive(false);
                //BG_2.SetActive(true);
                break;
        }
    }

    void PanelMove()
    {
        sequence = DOTween.Sequence();

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(PanelMaster.transform.DOLocalMove(new Vector3(800f - (800f * (page_cullent - 1)), 0f, 0), 0.0f)
            //.SetRelative()
            .SetEase(Ease.OutExpo) //元の位置に戻る。
            .OnComplete(() => Endpanelmove())); //②数字演出開始　再生終了時 また移動ボタンが押せるように。

    }

    void Endpanelmove()
    {

    }

    void CharaAnim()
    {       
        //キャラアニメ
        chara_animator.SetInteger("trans_anim", 10);

        Live2Dexp_flag = true; //Live2D表情変更。LateUpdateで行う。
           
    }

    void Live2DExpression()
    {
        live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。

        //表情
        switch (GameMgr.ending_number)
        {
            case 1: //Bad ED


                break;

            case 2:

                live2d_animator.SetInteger("trans_expression", 2);
                break;

            case 3:

                live2d_animator.SetInteger("trans_expression", 9);
                break;

            case 4:

                live2d_animator.SetInteger("trans_expression", 50);
                break;
        }
    }

    void ChangeEDNumArray() //EDnumが3~0の順なので、順番を逆に入れ替える
    {
        EDList = new Dictionary<int, int>();
        EDList.Add(3, 0);
        EDList.Add(2, 1);
        EDList.Add(1, 2);
        EDList.Add(0, 3);
    }

    void EDHukidashiText()
    {
        if (GameMgr.special_shogo_flag)
        {
            switch (GameMgr.special_shogo_num)
            {
                case 0: //スカーレット

                    _hukidashi_content = ".." + "いちごのお菓子で優勝しちゃったね・・!" + "\n" + "いちご好きのおにいちゃんにピッタリの、赤い称号をもらたよ～☆";
                    break;

                case 1: //ホワイトプリム

                    _hukidashi_content = ".." + "おにいちゃん！" + "\n" + "天使のようなふわふわのクレープ！舌が舞いをまうよ～！" + "\n" + "はいコレ！天使のような称号、あげる～♪";
                    break;

                case 2: //ハイルング

                    _hukidashi_content = ".." + "にいちゃん、夢のプリンセストータで優勝しちゃった・・！" + "\n" + "じいちゃんから特別な称号もらったよ！！　おめでと☆";
                    break;

                case 3: //ブルーヴェール

                    _hukidashi_content = ".." + "おにいちゃん！" + "\n" + "ブルーヴェール.. かがやくような青色のお菓子で、みんな目が輝いてたよ！！" + "\n" + "特別な称号あげるね！ はい！☆";
                    break;

                case 4: //ししゃもマニア

                    _hukidashi_content = ".." + "おにいちゃん！" + "\n" + "..やっぱりにいちゃん、ししゃも大好きなんだね！" + "\n" + "ししゃもマニアな兄に、ししゃもバッチあげよう♪";
                    break;

                case 5: //ゴールドマスター

                    _hukidashi_content = ".." + "おにいちゃん！" + "\n" + "お金がとうとう" + GameMgr.GoldMasterMoneyLine.ToString() + GameMgr.MoneyCurrency + "こえちゃった・・！" + "\n" + "ここまでがんばってくれて、ありがと♪" + "\n" + "これは、スペシャルな称号だよ！！";
                    break;
            }
        }
        else
        {
            switch (GameMgr.ending_number)
            {
                case 1: //Bad ED

                    _hukidashi_content = ".." + GameMgr.mainGirl_Name + "はいなくなってしまった..。" + "\n" + "ハートをもっと上げて、再度挑戦してね！";
                    break;

                case 2:

                    _hukidashi_content = "おにいちゃん！" + "\n" + "お菓子、とってもうんめぇ～～！！" + "\n" + "まだまだ遊び足りないよ～！！";
                    break;

                case 3:

                    _hukidashi_content = "おにいちゃん！" + "\n" + "ありがと～！　また会おうね。" + "\n" + "..でもまだ、真の結末があるみたいだよ。";
                    break;

                case 4:

                    _hukidashi_content = "にいちゃん" + "\n" + GameMgr.mainGirl_Name + "のハートいっぱいにしてくれて、ありがとう！" + "\n" + "にいちゃん.. だ～いすき♪　また会いたいな～♪";
                    break;
            }
        }
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
    
}
