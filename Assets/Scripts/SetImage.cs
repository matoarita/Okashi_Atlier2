using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

// アイテム画像を表示するスクリプト


public class SetImage : MonoBehaviour
{
    private GameObject canvas;

    public Image item_screen;
    //public GameObject gameobject; //publicで宣言すると、unityヒエラルキー上で見えるようになる。そこに、他のゲームオブジェクトを紐づけすることができる。

    //private Sprite sprite;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject Card_param_obj;
    private GameObject Card_param_obj2;
    private GameObject TasteSubWindow;
    private GameObject Slot_SubWindow;

    private GameObject NewRecipi_Prefab1;
    private GameObject NewRecipi;
    private int newrecipi_id;
    private Text newrecipi_text;
    private string newrecipi_name;
    private Texture2D newrecipi_Img;
    private Image newrecipi_Img_hyouji;

    private GameObject BlackImage;

    private PlayerItemList pitemlist;
    private ExpTable exp_table;
    private Exp_Controller exp_Controller;

    private Compound_Keisan compound_keisan;

    private SlotNameDataBase slotnamedatabase;
    private SlotChangeName slotchangename;

    private Texture2D texture2d;
    private Texture2D card_template_1;
    private Texture2D card_template_2;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject extremePanel_obj;
    private ExtremePanel extremePanel;

    public int itemID; //CardViewなどから呼び出すこともあり。
    private Image item_Icon;
    private Text item_Name;
    private string _name;
    private string item_SlotName;

    private Text item_Rank;
    private Text item_RankDesc;
    private Text item_Shokukan_Type;
    private string rank;

    public string item_type;
    public string item_type_sub;

    private string _quality;
    private string _quality_bar;

    private string[] _slot = new string[10];
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。スロット覧用
    private string[] _slotHyouji2 = new string[10]; //日本語に変換後の表記を格納する。フルネーム用  

    private Text item_Category;
    private string category;
    private string subcategory;
   
    private Text item_Name_Full;
    private Text item_Quality;
    private Text item_Quality_Bar;
    private Text item_Quality_Score;

    private Text item_Rich;
    private Text item_Sweat;
    private Text item_Bitter;
    private Text item_Sour;

    private Text item_Shokukan;
    private Text item_Crispy;
    private Text item_Fluffy;
    private Text item_Smooth;
    private Text item_Hardness;

    private Text item_Powdery;
    private Text item_Oily;
    private Text item_Watery;

    private Text item_lastRich;
    private Text item_lastSweat;
    private Text item_lastBitter;
    private Text item_lastSour;

    private Text item_lastShokukan_Type;
    private Text item_lastShokukan;

    private Text item_lastPowdery;
    private Text item_lastOily;
    private Text item_lastWatery;

    private Text item_LastTotalScore;
    private Text item_Hint;

    private Text[] item_Slot = new Text[10];

    private GameObject item_HighScoreFlag;
    private GameObject item_HighScoreFlag_2;

    private GameObject kosu_panel;
    private Text kosu_text;

    private int i, count;

    private int _quality_score;
    private int _rich_score;
    private int _sweat_score;
    private int _sour_score;
    private int _bitter_score;

    private int _shokukan_score;
    private int _crispy_score;
    private int _fluffy_score;
    private int _smooth_score;
    private int _hardness_score;
    private int _jiggly_score;
    private int _chewy_score;

    private int _juice_score;

    private int _powdery_score;
    private int _oily_score;
    private int _watery_score;

    private int _lastquality_score;
    private int _lastrich_score;
    private int _lastsweat_score;
    private int _lastsour_score;
    private int _lastbitter_score;

    private int _lastshokukan_score;
    private int _lastcrispy_score;
    private int _lastfluffy_score;
    private int _lastsmooth_score;
    private int _lasthardness_score;
    private int _lastjiggly_score;
    private int _lastchewy_score;
    private int _lastjuice_score;

    private int _eat_kaisu;
    private int _highscore_flag;
    private int _lasttotal_score;
    private string _lasthint_text;

    private Slider _Shokukan_slider;
    private Slider _Sweat_slider;
    private Slider _Bitter_slider;
    private Slider _Sour_slider;

    private Slider _Crispy_slider;
    private Slider _Fluffy_slider;
    private Slider _Smooth_slider;
    private Slider _Hardness_slider;

    private Slider _Shokukan_lastslider;
    private Slider _Sweat_lastslider;
    private Slider _Bitter_lastslider;
    private Slider _Sour_lastslider;

    public int check_counter; //cardViewから直接指定
    public int Pitem_or_Origin; //プレイヤーアイテムか、オリジナルアイテムかの判定

    public Vector3 def_scale; //cardview.csからも指定できる

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    private SoundController sc;

    public int anim_status = 0; //リザルトのときのみ、アニメーションをちょっと変えるためのフラグ。 CardViewから読んでいる。


    // Use this for initialization
    void Start()
    {
        SetData();
    }   

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        SetData();        
    }

    //カードを表示用のアニメ
    public void CardHyoujiAnim()
    {
        //
        //Dotweenでアニメーションの設定
        //
        switch (anim_status)
        {
            case 0:

                Sequence sequence = DOTween.Sequence();

                //まず、初期値。
                this.GetComponent<CanvasGroup>().alpha = 0;
                sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
                /*sequence.Append(transform.DOLocalMove(new Vector3(30f, 0, 0), 0.0f)
                    .SetRelative());*/ //元の位置から30px右に置いておく。
                                       //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

                //移動のアニメ
                sequence.Append(transform.DOScale(def_scale, 0.2f)
                    .SetEase(Ease.OutExpo));
                /*sequence.Append(transform.DOLocalMove(new Vector3(-30f, 0, 0), 0.3f)
                    .SetRelative()
                    .SetEase(Ease.OutExpo)); *///30px右から、元の位置に戻る。
                sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));

                break;

            case 99: //アニメなし

                break;

            default:

                break;
        }
    }

    void SetData()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        database = ItemDataBase.Instance.GetComponent<ItemDataBase>(); // ややこしいけど、ItemDataBaseという自作スクリプトをどこかに作ると、それ自体を型として扱える？っぽい。
                                                                       //　宣言の時に、ItemDataBase型で変数を宣言し、Start()内で、初期化する。ヒエラルキー内のGameobject（IteamDataBaseという名前）にC#スクリプト「ItemDataBase」は紐づけていて、
                                                                       // そのC#スクリプト（コンポーネント）を取得している一文。あらかじめ、publicでGame Objectを宣言し、先にヒエラルキー上で「ItemDataBaseオブジェクト」を紐づけする必要がある。

        item_screen = this.transform.Find("Item_card_template").GetComponent<Image>(); //カードテンプレートのデータ
        card_template_1 = Resources.Load<Texture2D>("Sprites/Items/card_template_1");
        card_template_2 = Resources.Load<Texture2D>("Sprites/Items/card_template_2");

        Card_param_obj = this.transform.Find("Card_Param_window").gameObject;
        Card_param_obj2 = this.transform.Find("Card_Param_window2").gameObject;
        Slot_SubWindow = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot").gameObject;
        TasteSubWindow = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow").gameObject;
        //TasteSubWindow.SetActive(false);

        for (i = 0; i < _slotHyouji1.Length; i++)
        {
            _slotHyouji1[i] = "";
            _slotHyouji2[i] = "";
        }

        //各要素の取得
        item_Icon = this.transform.Find("Item_card_template/ItemIcon").gameObject.GetComponent<Image>(); //画像アイコン
        item_Name = this.transform.Find("Item_card_template/ItemName").gameObject.GetComponent<Text>(); //名前
        item_Rank = this.transform.Find("Item_card_template/ItemRank").gameObject.GetComponent<Text>(); //ランク表示
        item_Category = this.transform.Find("Item_card_template/ItemCategory").gameObject.GetComponent<Text>(); //カテゴリー
        item_RankDesc = this.transform.Find("Item_card_template/ItemRankDesc").gameObject.GetComponent<Text>(); //ランクに合わせて、おいしさや食感を表示するテキスト
        item_Shokukan_Type = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/TxCrispy").gameObject.GetComponent<Text>(); //お菓子のタイプによって、食感表示を変える

        item_Name_Full = this.transform.Find("Card_Param_window/Card_Name/Tx_Name").gameObject.GetComponent<Text>(); //名前（スロット名も含む正式名称）の値

        item_Quality = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/Quality_Rank").gameObject.GetComponent<Text>(); //品質のランク
        item_Quality_Bar = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/Quality_Bar").gameObject.GetComponent<Text>(); //品質の★の数
        item_Quality_Score = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/Quality_Score").gameObject.GetComponent<Text>(); //品質の★の数

        item_Sweat = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSweatScore").gameObject.GetComponent<Text>(); //甘さの値
        item_Bitter = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemBitterScore").gameObject.GetComponent<Text>(); //苦さの値
        item_Sour = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSourScore").gameObject.GetComponent<Text>(); //すっぱさの値

        //item_Rich = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemRichScore").gameObject.GetComponent<Text>(); //味のコクの値
        item_Shokukan = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemShokukanScore").gameObject.GetComponent<Text>(); //食感の値
        item_Crispy = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/CrispyScore").gameObject.GetComponent<Text>(); //さくさくの値
        item_Fluffy = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/FluffyScore").gameObject.GetComponent<Text>(); //ふわふわの値
        item_Smooth = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/SmoothScore").gameObject.GetComponent<Text>(); //なめらかの値
        item_Hardness = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/HardnessScore").gameObject.GetComponent<Text>(); //かたさの値

        item_Powdery = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemPowdery").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_Oily = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemOily").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_Watery = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemWatery").gameObject.GetComponent<Text>(); //粉っぽいの値

        //スロット表示
        item_Slot[0] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_01").gameObject.GetComponent<Text>(); //Slot01の値
        item_Slot[1] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_02").gameObject.GetComponent<Text>(); //Slot02の値
        item_Slot[2] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_03").gameObject.GetComponent<Text>(); //Slot03の値
        item_Slot[3] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_04").gameObject.GetComponent<Text>(); //Slot04の値
        item_Slot[4] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_05").gameObject.GetComponent<Text>(); //Slot05の値
        item_Slot[5] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_06").gameObject.GetComponent<Text>(); //Slot06の値
        item_Slot[6] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_07").gameObject.GetComponent<Text>(); //Slot07の値
        item_Slot[7] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_08").gameObject.GetComponent<Text>(); //Slot08の値
        item_Slot[8] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_09").gameObject.GetComponent<Text>(); //Slot09の値
        item_Slot[9] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/Panel/ScrollView/Viewport/Content/ItemSlot_10").gameObject.GetComponent<Text>(); //Slot10の値

        kosu_panel = this.transform.Find("Item_card_template/ItemKosu_Panel").gameObject;
        kosu_panel.SetActive(false);
        kosu_text = this.transform.Find("Item_card_template/ItemKosu_Panel/ItemKosu").gameObject.GetComponent<Text>();

        //各パラメータバーの取得
        _Shokukan_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemShokukanBar").gameObject.GetComponent<Slider>();
        _Sweat_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSweatBar").gameObject.GetComponent<Slider>();
        _Bitter_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemBitterBar").gameObject.GetComponent<Slider>();
        _Sour_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSourBar").gameObject.GetComponent<Slider>();

        _Crispy_slider = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/CrispyBar").gameObject.GetComponent<Slider>();
        _Fluffy_slider = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/FluffyBar").gameObject.GetComponent<Slider>();
        _Smooth_slider = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/SmoothBar").gameObject.GetComponent<Slider>();
        _Hardness_slider = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/HardnessBar").gameObject.GetComponent<Slider>();

        //前回のスコア関係
        item_lastShokukan_Type = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/TxCrispy").gameObject.GetComponent<Text>();
        item_lastSweat = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemSweatScore").gameObject.GetComponent<Text>(); //甘さの値
        item_lastBitter = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemBitterScore").gameObject.GetComponent<Text>(); //苦さの値
        item_lastSour = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemSourScore").gameObject.GetComponent<Text>(); //すっぱさの値

        //item_lastRich = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window/ItemRichScore").gameObject.GetComponent<Text>(); //味のコクの値
        item_lastShokukan = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemShokukanScore").gameObject.GetComponent<Text>(); //さくさくの値

        item_lastPowdery = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemPowdery").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_lastOily = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemOily").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_lastWatery = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemWatery").gameObject.GetComponent<Text>(); //粉っぽいの値

        item_LastTotalScore = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemLastTotalScore").gameObject.GetComponent<Text>(); //最高得点
        item_Hint = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemHint").gameObject.GetComponent<Text>(); //前回の妹からのヒント
        item_HighScoreFlag = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/HighScoreFlag").gameObject; //ハイスコア時、星のエンブレムがでる。
        item_HighScoreFlag_2 = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/HighScoreFlag_2").gameObject; //ハイスコア時、星のエンブレムがでる。

        _Shokukan_lastslider = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemShokukanBar").gameObject.GetComponent<Slider>();
        _Sweat_lastslider = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemSweatBar").gameObject.GetComponent<Slider>();
        _Bitter_lastslider = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemBitterBar").gameObject.GetComponent<Slider>();
        _Sour_lastslider = this.transform.Find("Card_Param_window2/Card_Parameter/Card_Param_Window_Taste/ItemSourBar").gameObject.GetComponent<Slider>();

        //スロットをもとに、正式名称を計算するメソッド
        slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();

        //新レシピプレファブの取得
        NewRecipi_Prefab1 = (GameObject)Resources.Load("Prefabs/NewRecipiMessage");

        audioSource = GetComponent<AudioSource>();

    }

    public void SetInit()
    {
        Card_draw();
    }

    //アイテムリストから開いた場合
    public void SetInitPitemList()
    {
        Card_draw();
        CardParamOFF_2();
    }

    //お店でイベントアイテムを選択した場合
    public void SetInitEventItem()
    {
        Card_draw2();
        CardParamOFF();
    }

    //カード描画用のパラメータ読み込み
    void Card_draw2()
    {
        //アイテムタイプを代入//
        item_Name.text = pitemlist.eventitemlist[check_counter].event_itemNameHyouji;
        item_Category.text = "レシピ";
        item_RankDesc.text = pitemlist.eventitemlist[check_counter].event_itemNameHyouji;

        texture2d = Resources.Load<Texture2D>("Sprites/" + pitemlist.eventitemlist[check_counter].event_fileName);

        // texture2dを使い、Spriteを作って、反映させる
        item_Icon.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);
    }

    //カード描画用のパラメータ読み込み
    void Card_draw()
    {

        switch (Pitem_or_Origin)
        {
            case 0: //店売りアイテムリストを選択した場合

                //アイテムID
                itemID = database.items[check_counter].itemID;

                //アイテムタイプを代入//
                item_type = database.items[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = database.items[check_counter].itemType_sub.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = database.items[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = database.items[check_counter].itemIcon;

                //カードのスロット部分の名
                item_SlotName = database.items[check_counter].item_SlotName;

                //カードのアイテム名フル
                _name = database.items[check_counter].itemNameHyouji;                
                
                //アイテムの品質値
                _quality = database.items[check_counter].Quality.ToString();

                //甘さなどのパラメータを代入
                _quality_score = database.items[check_counter].Quality;
                _rich_score = database.items[check_counter].Rich;
                _sweat_score = database.items[check_counter].Sweat;
                _bitter_score = database.items[check_counter].Bitter;
                _sour_score = database.items[check_counter].Sour;

                _crispy_score = database.items[check_counter].Crispy;
                _fluffy_score = database.items[check_counter].Fluffy;
                _smooth_score = database.items[check_counter].Smooth;
                _hardness_score = database.items[check_counter].Hardness;

                _juice_score = database.items[check_counter].Juice;

                _powdery_score = database.items[check_counter].Powdery;
                _oily_score = database.items[check_counter].Oily;
                _watery_score = database.items[check_counter].Watery;

                //前回の味読み込み
                //_lastquality_score = database.items[check_counter].Quality;
                _lastrich_score = database.items[check_counter].last_rich_score;
                _lastsweat_score = database.items[check_counter].last_sweat_score;
                _lastbitter_score = database.items[check_counter].last_bitter_score;
                _lastsour_score = database.items[check_counter].last_sour_score;

                _lastcrispy_score = database.items[check_counter].last_crispy_score;
                _lastfluffy_score = database.items[check_counter].last_fluffy_score;
                _lastsmooth_score = database.items[check_counter].last_smooth_score;
                _lasthardness_score = database.items[check_counter].last_hardness_score;
                _lastjuice_score = _lastsweat_score + _lastbitter_score + _lastsour_score;

                _eat_kaisu = database.items[check_counter].Eat_kaisu;
                _highscore_flag = database.items[check_counter].HighScore_flag;
                _lasttotal_score = database.items[check_counter].last_total_score;
                _lasthint_text = database.items[check_counter].last_hinttext;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = database.items[check_counter].toppingtype[i].ToString();
                }


                break;

            case 1: //オリジナルプレイヤーアイテムリストを選択した場合

                //アイテムID
                itemID = pitemlist.player_originalitemlist[check_counter].itemID;

                //アイテムタイプを代入//
                item_type = pitemlist.player_originalitemlist[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = pitemlist.player_originalitemlist[check_counter].itemType_sub.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = pitemlist.player_originalitemlist[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = pitemlist.player_originalitemlist[check_counter].itemIcon;

                //カードのスロット部分の名
                item_SlotName = pitemlist.player_originalitemlist[check_counter].item_SlotName;

                //カードのアイテム名
                //item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + pitemlist.player_originalitemlist[check_counter].itemNameHyouji;
                _name = pitemlist.player_originalitemlist[check_counter].itemNameHyouji;

                //アイテムの品質値
                _quality = pitemlist.player_originalitemlist[check_counter].Quality.ToString();

                //甘さなどのパラメータを代入
                _quality_score = pitemlist.player_originalitemlist[check_counter].Quality;
                _rich_score = pitemlist.player_originalitemlist[check_counter].Rich;
                _sweat_score = pitemlist.player_originalitemlist[check_counter].Sweat;
                _bitter_score = pitemlist.player_originalitemlist[check_counter].Bitter;
                _sour_score = pitemlist.player_originalitemlist[check_counter].Sour;

                _crispy_score = pitemlist.player_originalitemlist[check_counter].Crispy;
                _fluffy_score = pitemlist.player_originalitemlist[check_counter].Fluffy;
                _smooth_score = pitemlist.player_originalitemlist[check_counter].Smooth;
                _hardness_score = pitemlist.player_originalitemlist[check_counter].Hardness;

                _juice_score = pitemlist.player_originalitemlist[check_counter].Juice;

                _powdery_score = pitemlist.player_originalitemlist[check_counter].Powdery;
                _oily_score = pitemlist.player_originalitemlist[check_counter].Oily;
                _watery_score = pitemlist.player_originalitemlist[check_counter].Watery;

                //前回の味読み込み
                //_lastquality_score = pitemlist.player_originalitemlist[check_counter].Quality;
                _lastrich_score = pitemlist.player_originalitemlist[check_counter].last_rich_score;
                _lastsweat_score = pitemlist.player_originalitemlist[check_counter].last_sweat_score;
                _lastbitter_score = pitemlist.player_originalitemlist[check_counter].last_bitter_score;
                _lastsour_score = pitemlist.player_originalitemlist[check_counter].last_sour_score;

                _lastcrispy_score = pitemlist.player_originalitemlist[check_counter].last_crispy_score;
                _lastfluffy_score = pitemlist.player_originalitemlist[check_counter].last_fluffy_score;
                _lastsmooth_score = pitemlist.player_originalitemlist[check_counter].last_smooth_score;
                _lasthardness_score = pitemlist.player_originalitemlist[check_counter].last_hardness_score;
                _lastjuice_score = _lastsweat_score + _lastbitter_score + _lastsour_score;

                _eat_kaisu = pitemlist.player_originalitemlist[check_counter].Eat_kaisu;
                _highscore_flag = pitemlist.player_originalitemlist[check_counter].HighScore_flag;
                _lasttotal_score = pitemlist.player_originalitemlist[check_counter].last_total_score;
                _lasthint_text = pitemlist.player_originalitemlist[check_counter].last_hinttext;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = pitemlist.player_originalitemlist[check_counter].toppingtype[i].ToString();
                }                
                

                break;

            default:
                break;
        }

        //カード　スロット名 現在は、特に表示はしていない
        Slotname_Hyouji();

        //実際にカードの表示を更新する部分
        DrawCardParam();
    }    

    void DrawCardParam()
    {
    
        // texture2dを使い、Spriteを作って、反映させる
        item_Icon.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);

        //サブカテゴリーを検出し、subCategoryの内容に、日本語名で入力
        switch (item_type_sub)
        {
            //器具系は、枠の色が違う。
            case "Machine":
                item_screen.sprite = Sprite.Create(card_template_2,
                                   new Rect(0, 0, card_template_2.width, card_template_2.height),
                                   Vector2.zero);
                break;

            default:

                item_screen.sprite = Sprite.Create(card_template_1,
                                   new Rect(0, 0, card_template_1.width, card_template_1.height),
                                   Vector2.zero);
                break;
        }


        /* カテゴリーの表示 ついでに、ランクによって、「ふわふわ感」などの表示も行う。*/


        //itemTypeを検出し、Categoryの内容に、日本語名で入力
        switch (item_type)
        {
            case "Okashi":
                category = "お菓子";
                break;
            case "Acce":
                category = "アクセ";
                break;
            case "Potion":
                category = "トッピング";
                break;
            case "Mat":
                category = "材料";
                break;
            case "Etc":
                category = "その他";
                break;
            default:
                category = "";
                break;
        }


        item_Shokukan_Type.text = "";

        //サブカテゴリーを検出し、subCategoryの内容に、日本語名で入力
        switch (item_type_sub)
        {
            case "Non":
                subcategory = "";
                break;
            case "Cookie":
                subcategory = "クッキー";
                Crispy_Text();
                break;
            case "Bread":
                subcategory = "パン";
                Crispy_Text();
                break;
            case "Rusk":
                subcategory = "ラスク";
                Crispy_Text();
                break;
            case "Pie":
                subcategory = "パイ";
                Crispy_Text();
                break;
            case "Chocolate":
                subcategory = "チョコレート";
                Smooth_Text();
                break;
            case "Chocolate_Mat":
                subcategory = "チョコレート";
                Smooth_Text();
                break;           
            case "Cake":
                subcategory = "ケーキ";
                Fluffy_Text();               
                break;
            case "PanCake":
                subcategory = "パンケーキ";
                Fluffy_Text();
                break;
            case "Financier":
                subcategory = "フィナンシェ";
                Fluffy_Text();
                break;
            case "Maffin":
                subcategory = "マフィン";
                Fluffy_Text();
                break;
            case "Cannoli":
                subcategory = "カンノーリ";
                Crispy_Text();
                break;
            case "Crepe":
                subcategory = "クレープ";
                Fluffy_Text();
                break;
            case "Crepe_Mat":
                subcategory = "クレープ";
                Fluffy_Text();
                break;
            case "Creampuff":
                subcategory = "シュークリーム";
                Fluffy_Text();
                break;
            case "Biscotti":
                subcategory = "ビスコッティ";
                Hardness_Text();                
                break;
            case "Donuts":
                subcategory = "ドーナツ";
                Fluffy_Text();
                break;
            case "IceCream":
                subcategory = "アイスクリーム";
                Smooth_Text();
                break;
            case "Parfe":
                subcategory = "パフェ";
                Smooth_Text();
                break;
            case "Juice":
                subcategory = "ジュース";
                Juice_Text();
                break;
            case "Tea":
                subcategory = "お茶";
                Crispy_Text();
                item_Shokukan_Type.text = "香り";
                item_lastShokukan_Type.text = "香り";
                break;
            case "Tea_Mat":
                subcategory = "お茶";
                Crispy_Text();
                item_Shokukan_Type.text = "香り";
                item_lastShokukan_Type.text = "香り";
                break;
            case "Fruits":
                subcategory = "フルーツ";
                Etc_Text_Non();
                break;
            case "Nuts":
                subcategory = "ナッツ";
                Etc_Text_Non();
                break;
            case "Source":
                subcategory = "お菓子材料";
                Etc_Text();                
                break;
            case "Potion":
                subcategory = "お菓子材料";
                Etc_Text();
                break;
            case "Appaleil":
                subcategory = "生地";
                Etc_Text();
                break;
            case "Pate":
                subcategory = "生地";
                Etc_Text();
                break;
            case "Cream":               
                subcategory = "クリーム";
                Etc_Text();
                break;
            case "Cookie_base":
                subcategory = "生地";
                break;
            case "Pie_base":
                subcategory = "生地";
                break;
            case "Chocolate_base":
                subcategory = "生地";
                break;
            case "Cake_base":
                subcategory = "生地";
                break;
            case "Komugiko":
                subcategory = "小麦粉";
                break;
            case "Suger":
                subcategory = "砂糖";
                break;
            case "Butter":
                subcategory = "バター";
                break;
            case "Egg":
                subcategory = "たまご";
                break;            
            case "Machine":
                subcategory = "器具";
                break;
            
            default:
                // 処理３　指定がなかった場合
                subcategory = "";
                break;
        }

        //最終的なテキストを表示 "\n"で改行
        item_Category.text = category + " - " + subcategory;

        /* カテゴリーここまで */

        //甘さ・苦さ・酸味の表示
        //item_Rich.text = _rich_score.ToString();
        item_Sweat.text = _sweat_score.ToString();
        item_Bitter.text = _bitter_score.ToString();
        item_Sour.text = _sour_score.ToString();
        item_Crispy.text = _crispy_score.ToString();
        item_Fluffy.text = _fluffy_score.ToString();
        item_Smooth.text = _smooth_score.ToString();
        item_Hardness.text = _hardness_score.ToString();


        //ゲージの更新
        _Shokukan_slider.value = _shokukan_score;
        _Sweat_slider.value = _sweat_score;
        _Bitter_slider.value = _bitter_score;
        _Sour_slider.value = _sour_score;
        _Crispy_slider.value = _crispy_score;
        _Fluffy_slider.value = _fluffy_score;
        _Smooth_slider.value = _smooth_score;
        _Hardness_slider.value = _hardness_score;


        //粉っぽさなどの、マイナス要素の表示
        if ( _powdery_score > 50 )
        {
            item_Powdery.text = "粉っぽい";
        } else
        {
            item_Powdery.text = "";
        }
        if (_oily_score > 50)
        {
            item_Oily.text = "油っぽい";
        }
        else
        {
            item_Oily.text = "";
        }
        if (_watery_score > 50)
        {
            item_Watery.text = "水っぽい";
        }
        else
        {
            item_Watery.text = "";
        }

        if (_eat_kaisu > 0)
        {
            //前回の最高味を表示

            //甘さ・苦さ・酸味の表示
            //item_Rich.text = _lastrich_score.ToString();
            item_lastShokukan.text = _lastshokukan_score.ToString();
            item_lastSweat.text = _lastsweat_score.ToString();
            item_lastBitter.text = _lastbitter_score.ToString();
            item_lastSour.text = _lastsour_score.ToString();

            //ゲージの更新
            _Shokukan_lastslider.value = _lastshokukan_score;
            _Sweat_lastslider.value = _lastsweat_score;
            _Bitter_lastslider.value = _lastbitter_score;
            _Sour_lastslider.value = _lastsour_score;

            //最高得点の表示
            item_LastTotalScore.text = _lasttotal_score.ToString();
            item_Hint.text = _lasthint_text;
        }
        else
        {
            //甘さ・苦さ・酸味の表示
            //item_Rich.text = "-";
            item_lastShokukan.text = "-";
            item_lastSweat.text = "-";
            item_lastBitter.text = "-";
            item_lastSour.text = "-";

            //ゲージの更新
            _Shokukan_lastslider.value = 0;
            _Sweat_lastslider.value = 0;
            _Bitter_lastslider.value = 0;
            _Sour_lastslider.value = 0;

            //最高得点の表示
            item_LastTotalScore.text = "";
            item_Hint.text = "";
        }

        //ハイスコアゲットできたら、星マーク
        if (_highscore_flag == 1)
        {
            item_HighScoreFlag.SetActive(true);
            item_HighScoreFlag_2.SetActive(false);
        }
        else if (_highscore_flag == 2)
        {
            item_HighScoreFlag.SetActive(true);
            item_HighScoreFlag_2.SetActive(true);
        }
        else
        {
            item_HighScoreFlag.SetActive(false);
            item_HighScoreFlag_2.SetActive(false);
        }


        //品質の表示
        /*
        if (_quality_score <= 0)
        {
            _quality = "";
        }
        else if (_quality_score > 0 && _quality_score <= 20)
        {
            _quality = "F";
            _quality_bar = "★";
        }
        else if (_quality_score > 20 && _quality_score <= 30)
        {
            _quality = "D";
            _quality_bar = "★★";
        }
        else if (_quality_score > 30 && _quality_score <= 40)
        {
            _quality = "D+";
            _quality_bar = "★★+";
        }
        else if (_quality_score > 40 && _quality_score <= 60) //50が平均値
        {
            _quality = "C";
            _quality_bar = "★★★";
        }
        else if (_quality_score > 60 && _quality_score <= 70)
        {
            _quality = "B";
            _quality_bar = "★★★★";
        }
        else if (_quality_score > 70 && _quality_score <= 80)
        {
            _quality = "B+";
            _quality_bar = "★★★★+";
        }
        else if (_quality_score > 80 && _quality_score <= 88)
        {
            _quality = "A";
            _quality_bar = "★★★★★";
        }
        else if (_quality_score > 88 && _quality_score <= 93)
        {
            _quality = "S";
            _quality_bar = "★★★★★★";
        }
        else if (_quality_score > 95 && _quality_score <= 99)
        {
            _quality = "SS";
            _quality_bar = "★★★★★★";
        }
        else if (_quality_score > 100)
        {
            _quality = "SSS+";
            _quality_bar = "★★★★★★★";
        }

        item_Quality.text = _quality;
        item_Quality_Bar.text = _quality_bar;
        item_Quality_Score.text = _quality_score.ToString();
        */

        //名称デフォルト
        item_Name.text = _name;

        //
        //味ウィンドウの表示のON/OFFを分類
        //
        Card_param_obj.SetActive(false);
        Card_param_obj2.SetActive(false);
        TasteSubWindow.SetActive(false);
        Slot_SubWindow.SetActive(false);

        if (item_type == "Mat" || item_type == "Etc")
        {
            switch (item_type_sub)
            {
                case "Appaleil":
                    Card_param_obj.SetActive(true);
                    Card_param_obj2.SetActive(false);
                    TasteSubWindow.SetActive(true);
                    item_Shokukan.text = "-";

                    item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + _name;
                    break;
                case "Cream":
                    Card_param_obj.SetActive(true);
                    Card_param_obj2.SetActive(false);
                    TasteSubWindow.SetActive(true);
                    item_Shokukan.text = "-";

                    item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + _name;
                    break;
                case "Fruits":
                    Card_param_obj.SetActive(true);
                    Card_param_obj2.SetActive(false);
                    break;

                case "Berry":
                    Card_param_obj.SetActive(true);
                    Card_param_obj2.SetActive(false);
                    break;

                case "Pate":
                    //Card_param_obj.SetActive(true);
                    break;

                case "Cookie_base":
                    //Card_param_obj.SetActive(true);
                    break;

                case "Pie_base":
                    //Card_param_obj.SetActive(true);
                    break;

                case "Chocolate_base":
                    //Card_param_obj.SetActive(true);
                    break;

                case "Cake_base":
                    //Card_param_obj.SetActive(true);
                    break;

                default:
                    
                    break;
            }
        }
        else if (item_type == "Okashi")
        {
            Card_param_obj.SetActive(true);
            Card_param_obj2.SetActive(true);
            //Slot_SubWindow.SetActive(true);

            item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + _name;
        }
        else if (item_type == "Potion")
        {
            Card_param_obj.SetActive(true);
            Card_param_obj2.SetActive(false);

            item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + _name;
        }
    }

    void Crispy_Text()
    {
        item_Shokukan_Type.text = "さくさく感";
        item_lastShokukan_Type.text = "さくさく感";
        item_Shokukan.text = _crispy_score.ToString();
        _shokukan_score = _crispy_score;
        _lastshokukan_score = _lastcrispy_score;
    }

    void Fluffy_Text()
    {
        item_Shokukan_Type.text = "ふわふわ感";
        item_lastShokukan_Type.text = "ふわふわ感";
        item_Shokukan.text = _fluffy_score.ToString();
        _shokukan_score = _fluffy_score;
        _lastshokukan_score = _lastfluffy_score;
    }

    void Smooth_Text()
    {
        item_Shokukan_Type.text = "くちどけ感";
        item_lastShokukan_Type.text = "くちどけ感";
        item_Shokukan.text = _smooth_score.ToString();
        _shokukan_score = _smooth_score;
        _lastshokukan_score = _lastsmooth_score;
    }

    void Hardness_Text()
    {
        item_Shokukan_Type.text = "歯ごたえ";
        item_lastShokukan_Type.text = "歯ごたえ";
        item_Shokukan.text = _hardness_score.ToString();
        _shokukan_score = _hardness_score;
        _lastshokukan_score = _lasthardness_score;
    }

    void Juice_Text()
    {
        item_Shokukan_Type.text = "のどごし";
        item_lastShokukan_Type.text = "のどごし";
        item_Shokukan.text = _juice_score.ToString();
        _shokukan_score = _juice_score;
        _lastshokukan_score = _lastsweat_score + _lastbitter_score + _lastsour_score;
    }

    void Etc_Text()
    {
        item_Shokukan_Type.text = "食感";
        item_lastShokukan_Type.text = "食感";
    }

    void Etc_Text_Non()
    {
        item_Shokukan_Type.text = "-";
        item_lastShokukan_Type.text = "-";
    }


    //調合完了後、カードのボタンを押すと呼び出される。
    public void CompoundResult_Button()
    {
        //レベルアップチェック用オブジェクトの取得
        exp_table = GameObject.FindWithTag("ExpTable").GetComponent<ExpTable>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //エクストリームパネルオブジェクトの取得
        extremePanel_obj = canvas.transform.Find("MainUIPanel/ExtremePanel").gameObject;
        extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

        //ブラックエフェクトを取得
        BlackImage = canvas.transform.Find("Compound_BGPanel_A/BlackImage").gameObject; //魔法エフェクト用の半透明で幕

        if (exp_table.check_on == true)
        {
            //レベルチェック中は、カードを消せないようにする。
        }
        else
        {
            //半透明黒パネルはoff
            BlackImage.GetComponent<CanvasGroup>().alpha = 0;

            //新しいレシピをひらめいたかどうかチェック
            if ( exp_Controller.NewRecipiFlag == true)
            {
                newrecipi_id = exp_Controller.NewRecipi_compoID;

                //調合DBの名前と一致するものを、アイテムDBから検索。表示名前と画像を取得
                i = 0;
                while ( i < database.items.Count )
                {
                    if( database.items[i].itemName == databaseCompo.compoitems[newrecipi_id].cmpitemID_result)
                    {
                        newrecipi_name = database.items[i].itemNameHyouji;
                        newrecipi_Img = database.items[i].itemIcon;
                        break;
                    }
                    i++;
                }

                //取得
                NewRecipi = Instantiate(NewRecipi_Prefab1, canvas.transform);
                newrecipi_text = NewRecipi.transform.Find("Image/Text").gameObject.GetComponent<Text>();
                newrecipi_Img_hyouji = NewRecipi.transform.Find("Image/ItemImage").gameObject.GetComponent<Image>();

                //表示
                newrecipi_text.text = GameMgr.ColorLemon + newrecipi_name + "</color>" + "\n" + "を閃いた！";

                // texture2dを使い、Spriteを作って、反映させる
                newrecipi_Img_hyouji.sprite = Sprite.Create(newrecipi_Img,
                                           new Rect(0, 0, newrecipi_Img.width, newrecipi_Img.height),
                                           Vector2.zero);

                //音を鳴らす 新しいレシピ閃いたときの音 scのほうに音を送ると、途中で音が途切れない。
                //audioSource.PlayOneShot(sound1);
                sc.PlaySe(25);

                exp_Controller.NewRecipiFlag = false; //オフにしておく。

                Destroy(this.gameObject);
            }
            else
            {
                compound_Main_obj = GameObject.FindWithTag("Compound_Main");
                compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

                //調合完了後、また調合画面に戻るか、メイン画面に戻るか
                switch(compound_Main.compound_select)
                {
                    case 1: //レシピ調合

                        if (extremePanel.extreme_itemID != 9999) //新しいお菓子がセットされているので、一度オフ
                        {
                            compound_Main.compound_status = 0;

                            if (GameMgr.tutorial_ON == true)
                            {
                                if (GameMgr.tutorial_Num == 180)
                                {
                                    GameMgr.tutorial_Progress = true;
                                    GameMgr.tutorial_Num = 190;
                                }
                            }
                        }
                        else
                        {
                            compound_Main.compound_status = 1; // もう一回、レシピ調合の画面に戻る。
                        }
                        break;

                    case 3: //オリジナル調合

                        if (extremePanel.extreme_itemID != 9999) //新しいお菓子がセットされているので、一度オフ
                        {
                            compound_Main.compound_status = 0;


                            if (GameMgr.tutorial_ON == true)
                            {
                                if (GameMgr.tutorial_Num == 180)
                                {
                                    GameMgr.tutorial_Progress = true;
                                    GameMgr.tutorial_Num = 190;
                                }
                            }
                        }
                        else
                        {
                            compound_Main.compound_status = 3; // もう一回、オリジナル調合の画面に戻る。
                        }
                        break;

                    default:

                        compound_Main.compound_status = 0;
                        break;

                }
                

                if (exp_Controller.compound_success == true)
                {
                    extremePanel.LifeAnimeOnTrue();
                }
                else
                {

                }

                exp_Controller.EffectListClear();
                Destroy(this.gameObject);

            }
        }
    }

    //調合アニメ時、パラメータ部分はオフにする。
    public void CardParamON()
    {
        Card_param_obj.SetActive(true);
        Card_param_obj2.SetActive(true);
    }

    public void CardParamOFF()
    {
        Card_param_obj.SetActive(false);
        Card_param_obj2.SetActive(false);
    }

    public void CardParamOFF_2()
    {
        Card_param_obj2.SetActive(false);
    }

    public void Kosu_ON(int _kosu)
    {
        kosu_panel.SetActive(true);
        kosu_text.text = _kosu.ToString();
    }

    public void Kosu_OFF()
    {
        kosu_panel.SetActive(false);
    }

    void Slotname_Hyouji()
    {
        for (i = 0; i < _slot.Length; i++)
        {
            count = 0;

            //スロット名を日本語に変換。DBから変換。Nonは、空白になる。
            while (count < slotnamedatabase.slotname_lists.Count)
            {
                if (slotnamedatabase.slotname_lists[count].slotName == _slot[i])
                {
                    _slotHyouji1[i] = slotnamedatabase.slotname_lists[count].slot_Hyouki_1;
                    _slotHyouji2[i] = "<color=#0000FF>" + slotnamedatabase.slotname_lists[count].slot_Hyouki_2 + "</color>";
                    break;
                }
                count++;
            }
        }

        //スロット名の表示+初期化
        for (i = 0; i < _slot.Length; i++)
        {
            if (_slot[i] == "Non") //Nonは空白表示。
            {
                _slot[i] = "";
            }

            item_Slot[i].text = _slotHyouji1[i]; //スロット表示１のほうが、スロットに表示する用のテキスト。スロット表示２は、アイテムのフルネームのほう。
        }

        for (i = 0; i < _slotHyouji2.Length; i++)
        {
            _slotHyouji2[i] = "";
        }
    }


    /*public void SetYosokuInit() //生成するカードのパラメータを、あらかじめ予測して表示する
    {
        //合成計算オブジェクトの取得
        compound_keisan = GameObject.FindWithTag("Compound_Keisan").GetComponent<Compound_Keisan>();

        Card_YosokuDraw();
    }

    void Card_YosokuDraw()
    {
        check_counter = compound_keisan._baseID;

        //アイテムタイプを代入//
        item_type = compound_keisan._base_itemType;

        //サブカテゴリーの代入
        item_type_sub = compound_keisan._base_itemType_sub;

        // アイテム解説の表示
        item_RankDesc.text = compound_keisan._base_itemdesc;

        // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
        texture2d = database.items[check_counter].itemIcon;

        //カードのアイテム名
        item_Name.text = database.items[check_counter].itemNameHyouji;

        //アイテムの品質値
        _quality = compound_keisan._basequality.ToString();

        //甘さなどのパラメータを代入
        _quality_score = compound_keisan._basequality;
        _rich_score = compound_keisan._baserich;
        _sweat_score = compound_keisan._basesweat;
        _bitter_score = compound_keisan._basebitter;
        _sour_score = compound_keisan._basesour;

        _crispy_score = compound_keisan._basecrispy;
        _fluffy_score = compound_keisan._basefluffy;
        _smooth_score = compound_keisan._basesmooth;
        _hardness_score = compound_keisan._basehardness;

        _powdery_score = compound_keisan._basepowdery;
        _oily_score = compound_keisan._baseoily;
        _watery_score = compound_keisan._basewatery;

        _eat_kaisu = database.items[check_counter].Eat_kaisu;
        _highscore_flag = database.items[check_counter].HighScore_flag;
        _lasttotal_score = database.items[check_counter].last_total_score;
        _lasthint_text = database.items[check_counter].last_hinttext;

        DrawCard();
    }*/
}