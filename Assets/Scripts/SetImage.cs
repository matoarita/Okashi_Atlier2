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

    private GameObject Card_TemplateMain_obj;
    private GameObject Card_param_obj;
    private GameObject Card_param_obj2;
    public bool CardTasteView_flag1; //CardViewから読み出し　味のビューがON/OFF ONなら、ショップ画面などでは左よりに位置し、右にカウンタを表示
    private GameObject TasteSubWindow;
    private GameObject Slot_SubWindow;
    public bool taste_slot_flag;
    private GameObject SlotChangeButton;

    private GameObject NewRecipi_Prefab1;
    private GameObject NewRecipi;
    private int newrecipi_id;
    private Text newrecipi_text;
    private string newrecipi_name;
    private Sprite newrecipi_Img;
    private Image newrecipi_Img_hyouji;

    private GameObject BlackImage;
    private GameObject debugTaste_ScorePanel;

    private PlayerItemList pitemlist;
    private ExpTable exp_table;
    private Exp_Controller exp_Controller;

    private GameObject card_view_obj;
    private CardView card_view;

    private Compound_Keisan compound_keisan;

    private SlotNameDataBase slotnamedatabase;
    private SlotChangeName slotchangename;
    private ItemSubTypeSetDatabase itemsubtypeset_database;
    private ItemCardEffectDataBase itemCardEffect_database;
    private MagicSkillListDataBase magicskill_database;

    private Sprite texture2d;
    private Texture2D card_template_1;
    private Texture2D card_template_2;
    private Texture2D card_template_3;
    private Texture2D card_template_4;
    private Texture2D card_template_10;
    private Texture2D card_template_100;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private GameObject CompleteImage;

    private HikariMakeStartPanel hikarimake_startpanel;

    private SlotHyoujiButton slot_hyouji_button;

    public int itemID; //CardViewなどから呼び出すこともあり。
    private Image item_Icon;
    private Text item_Name;
    private string _name;
    private string item_SlotName;

    private GameObject effectPrefab;
    private GameObject effectPrefab_Init;
    private GameObject itemEffectPanel;

    private Text item_Rank;
    private Text item_RankDesc;
    private Text item_Shokukan_Type;
    private string rank;

    public string item_type;
    public string item_type_sub;
    public string item_type_subB;
    public string item_type_sub_category;

    private string _quality;
    private string _quality_bar;

    private int _rare;
    private int _secretFlag;

    private string[] _slot;
    private string[] _koyuslot;
    private string[] _slotHyouji1 = new string[10]; //日本語に変換後の表記を格納する。スロット覧用
    private string[] _slotHyouji2 = new string[10]; //日本語に変換後の表記を格納する。フルネーム用 
    private string[] _magicslot;
    private int[] _msvalue;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの所持数
    List<int> itemslotScore = new List<int>();

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
    private Text item_Beauty;

    private Text item_Powdery;
    private Text item_Oily;
    private Text item_Watery;

    private Text item_Beauty_debug;
    private Text item_Spwind;
    private Text item_Sp_score2;
    private Text item_Sp_score3;
    private Text item_Sp_score4;
    private Text item_Sp_score5;
    private Text item_Sp_score6;
    private Text item_Sp_score7;
    private Text item_Sp_score8;
    private Text item_Sp_score9;
    private Text item_Sp_score10;

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

    private Text item_MS_aisho;

    private Text[] item_Slot = new Text[10];

    private GameObject item_HighScoreFlag;
    private GameObject item_HighScoreFlag_2;

    private GameObject kosu_panel;
    private Text kosu_text;

    private GameObject secret_panel;
    private GameObject hlvbonus_panel;

    private GameObject magicview_content;
    private GameObject magicPrefab;
    private GameObject magicPrefab2;
    private GameObject magicPrefab3;
    private GameObject magicPrefab4;
    private GameObject magicPrefab5;
    private List<GameObject> _magicicon_listitem = new List<GameObject>();

    private int i, j, count;
    private int _mid;
    private int _itemid;

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

    private int _beauty_score;
    private int _tea_flavor_score;
    private int _spwind_score;
    private int _sp_score2;
    private int _sp_score3;
    private int _sp_score4;
    private int _sp_score5;
    private int _sp_score6;
    private int _sp_score7;
    private int _sp_score8;
    private int _sp_score9;
    private int _sp_score10;

    private int _magic_addbeauty;
    private int _magic_addspscore1;
    private int _magic_addspscore2;
    private int _magic_addspscore3;
    private int _magic_addspscore4;
    private int _magic_addspscore5;
    private int _magic_addspscore6;
    private int _magic_addspscore7;
    private int _magic_addspscore8;
    private int _magic_addspscore9;
    private int _magic_addspscore10;

    private int _attri1;
    private int _attri2;
    private int _attri3;
    private int _attri4;
    private int _attri5;
    private int _attri6;
    private int _attri7;
    private int _attri8;
    private int _attri9;
    private int _attri10;

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
    private Slider _Beauty_slider;

    private Slider _Shokukan_lastslider;
    private Slider _Sweat_lastslider;
    private Slider _Bitter_lastslider;
    private Slider _Sour_lastslider;

    public int check_counter; //cardViewから直接指定
    public int Pitem_or_Origin; //プレイヤーアイテムか、オリジナルアイテムかの判定

    public Vector3 def_scale; //cardview.csからも指定できる

    private int _slot_beauty, total_beauty;
    private bool SpScoreHyouji_OFF;

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
        if (SpScoreHyouji_OFF) //シーンによっては、強制的に表示させたくない場合があるのでここでSPスコアオフ
        {
            debugTaste_ScorePanel.SetActive(false);
        }
        else
        {
            if (GameMgr.DEBUG_MODE)
            {
                debugTaste_ScorePanel.SetActive(true);
            }
            else
            {
                if (GameMgr.DEBUG_TasteSPScore_ON)
                {
                    debugTaste_ScorePanel.SetActive(true);
                }
                else
                {
                    if (pitemlist.KosuCount("monocle") >= 1) //モノクルをもっていればSPスコアみれる
                    {
                        debugTaste_ScorePanel.SetActive(true);
                    }
                    else
                    {
                        debugTaste_ScorePanel.SetActive(false);
                    }
                }
            }
        }
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

                //フェードインでスっと表示されるアニメ。
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

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //アイテムサブタイプの表記を分けるデータベース
        itemsubtypeset_database = ItemSubTypeSetDatabase.Instance.GetComponent<ItemSubTypeSetDatabase>();

        //魔法エフェクトの計算データベース
        itemCardEffect_database = ItemCardEffectDataBase.Instance.GetComponent<ItemCardEffectDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        database = ItemDataBase.Instance.GetComponent<ItemDataBase>(); // ややこしいけど、ItemDataBaseという自作スクリプトをどこかに作ると、それ自体を型として扱える？っぽい。
                                                                       //　宣言の時に、ItemDataBase型で変数を宣言し、Start()内で、初期化する。ヒエラルキー内のGameobject（IteamDataBaseという名前）にC#スクリプト「ItemDataBase」は紐づけていて、
                                                                       // そのC#スクリプト（コンポーネント）を取得している一文。あらかじめ、publicでGame Objectを宣言し、先にヒエラルキー上で「ItemDataBaseオブジェクト」を紐づけする必要がある。

        item_screen = this.transform.Find("Item_card_template").GetComponent<Image>(); //カードテンプレートのデータ
        card_template_1 = Resources.Load<Texture2D>("Sprites/Icon/card_template_1"); //コモン
        card_template_2 = Resources.Load<Texture2D>("Sprites/Icon/card_template_2"); //アンコモン
        card_template_3 = Resources.Load<Texture2D>("Sprites/Icon/card_template_3"); //レア
        card_template_4 = Resources.Load<Texture2D>("Sprites/Icon/card_template_4"); //スーパーレア
        card_template_10 = Resources.Load<Texture2D>("Sprites/Icon/card_template_10"); //器具のカードテンプレ画像
        card_template_100 = Resources.Load<Texture2D>("Sprites/Icon/card_template_100"); //シークレットカード

        Card_TemplateMain_obj = this.transform.Find("Item_card_template").gameObject;
        //Card_TemplateMain_obj.SetActive(true);
        Card_param_obj = this.transform.Find("Card_Param_window").gameObject;
        Card_param_obj2 = this.transform.Find("Card_Param_window2").gameObject;
        Slot_SubWindow = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot").gameObject;
        TasteSubWindow = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow").gameObject;
        SlotChangeButton = this.transform.Find("Card_Param_window/Card_Parameter/SlotHyoujiButton").gameObject;
        slot_hyouji_button = SlotChangeButton.GetComponent<SlotHyoujiButton>();
        //TasteSubWindow.SetActive(false);




        _slot = new string[database.items[0].toppingtype.Length];
        _koyuslot = new string[database.items[0].koyu_toppingtype.Length];
        _magicslot = new string[database.items[0].item_MagicSlot.Length];
        _msvalue = new int[database.items[0].item_MagicSlotValue.Length];

        for (i = 0; i < _slotHyouji1.Length; i++)
        {
            _slotHyouji1[i] = "";
            _slotHyouji2[i] = "";
        }

        magicPrefab = (GameObject)Resources.Load("Prefabs/card_magiciconObj");
        magicPrefab2 = (GameObject)Resources.Load("Prefabs/card_magiciconObj2");
        magicPrefab3 = (GameObject)Resources.Load("Prefabs/card_magiciconObj3");
        magicPrefab4 = (GameObject)Resources.Load("Prefabs/card_magiciconObj4");
        magicPrefab5 = (GameObject)Resources.Load("Prefabs/card_magiciconObj5");
        magicview_content = this.transform.Find("Item_card_template/MagicIconView/Viewport/Content").gameObject;

        _magic_addbeauty = 0;
        _magic_addspscore1 = 0;
        _magic_addspscore2 = 0;
        _magic_addspscore3 = 0;
        _magic_addspscore4 = 0;
        _magic_addspscore5 = 0;
        _magic_addspscore6 = 0;
        _magic_addspscore7 = 0;
        _magic_addspscore8 = 0;
        _magic_addspscore9 = 0;
        _magic_addspscore10 = 0;



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
        item_Beauty = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemBeautyScore").gameObject.GetComponent<Text>();

        //item_Rich = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemRichScore").gameObject.GetComponent<Text>(); //味のコクの値
        item_Shokukan = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemShokukanScore").gameObject.GetComponent<Text>(); //食感の値
        item_Crispy = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/CrispyScore").gameObject.GetComponent<Text>(); //さくさくの値
        item_Fluffy = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/FluffyScore").gameObject.GetComponent<Text>(); //ふわふわの値
        item_Smooth = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/SmoothScore").gameObject.GetComponent<Text>(); //なめらかの値
        item_Hardness = this.transform.Find("Card_Param_window/Card_Parameter/TasteSubWindow/HardnessScore").gameObject.GetComponent<Text>(); //かたさの値        

        item_Powdery = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemPowdery").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_Oily = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemOily").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_Watery = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemWatery").gameObject.GetComponent<Text>(); //粉っぽいの値

        //デバッグ用　見た目などのパラメータ
        debugTaste_ScorePanel = this.transform.Find("Card_Param_window/Card_Parameter/DebugTasteScorePanel").gameObject;
        item_Beauty_debug = debugTaste_ScorePanel.transform.Find("ItemBeautyScore").gameObject.GetComponent<Text>(); //見た目の値デバッグ用
        item_Spwind = debugTaste_ScorePanel.transform.Find("ItemSP_windScore").gameObject.GetComponent<Text>(); //風
        item_Sp_score2 = debugTaste_ScorePanel.transform.Find("ItemSP_Score2").gameObject.GetComponent<Text>(); //海
        item_Sp_score3 = debugTaste_ScorePanel.transform.Find("ItemSP_Score3").gameObject.GetComponent<Text>(); //愛
        item_Sp_score4 = debugTaste_ScorePanel.transform.Find("ItemSP_Score4").gameObject.GetComponent<Text>(); //宇宙
        item_Sp_score5 = debugTaste_ScorePanel.transform.Find("ItemSP_Score5").gameObject.GetComponent<Text>(); //大人
        item_Sp_score6 = debugTaste_ScorePanel.transform.Find("ItemSP_Score6").gameObject.GetComponent<Text>(); //子供
        item_Sp_score7 = debugTaste_ScorePanel.transform.Find("ItemSP_Score7").gameObject.GetComponent<Text>(); //メルヘン
        item_Sp_score8 = debugTaste_ScorePanel.transform.Find("ItemSP_Score8").gameObject.GetComponent<Text>(); //芸術
        item_Sp_score9 = debugTaste_ScorePanel.transform.Find("ItemSP_Score9").gameObject.GetComponent<Text>(); //光らしさキラキラ感
        item_Sp_score10 = debugTaste_ScorePanel.transform.Find("ItemSP_Score10").gameObject.GetComponent<Text>(); //和風感        


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

        item_MS_aisho = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/MS_aisho_text").gameObject.GetComponent<Text>(); //魔法スロットの相性表示
        //item_MS_aisho.text = "";

        kosu_panel = this.transform.Find("Item_card_template/ItemKosu_Panel").gameObject;
        kosu_panel.SetActive(false);
        kosu_text = this.transform.Find("Item_card_template/ItemKosu_Panel/ItemKosu").gameObject.GetComponent<Text>();

        //シークレット表示
        secret_panel = this.transform.Find("Item_card_template/SecretPanel").gameObject;
        //secret_panel.SetActive(false);

        //HLV99ボーナス
        hlvbonus_panel = this.transform.Find("Item_card_template/HlvBonusPanel").gameObject;

        //魔法のエフェクトパネル
        if (effectPrefab_Init == null)
        {
            effectPrefab = (GameObject)Resources.Load("Prefabs/ItemCardEffectPanel");
            effectPrefab_Init = Instantiate(effectPrefab, this.transform);
            effectPrefab_Init.name = "ItemCardEffectPanel";
            effectPrefab_Init.transform.localPosition = new Vector3(0, 40, 0);
        }
        itemEffectPanel = this.transform.Find("ItemCardEffectPanel").gameObject; //エフェクトパネル

        //各パラメータバーの取得
        _Shokukan_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemShokukanBar").gameObject.GetComponent<Slider>();
        _Sweat_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSweatBar").gameObject.GetComponent<Slider>();
        _Bitter_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemBitterBar").gameObject.GetComponent<Slider>();
        _Sour_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSourBar").gameObject.GetComponent<Slider>();
        _Beauty_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemBeautyBar").gameObject.GetComponent<Slider>();

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

        //スロットのスコア計算用
        InitializeItemSlotDicts();

        //新レシピプレファブの取得
        NewRecipi_Prefab1 = (GameObject)Resources.Load("Prefabs/NewRecipiMessage");

        audioSource = GetComponent<AudioSource>();

    }

    public void SetInit()
    {
        Card_draw();
    }

    public void SetInitYosoku()
    {
        Card_drawYosoku();
    }

    //コンテストロフィーの表示の場合
    public void SetInitContestClear()
    {
        Card_drawContestClear();
        CardParamOFF_2();
    }

    //２で優勝したおかしを表示の場合など　一時アイテムリストにカードデータを入れて汎用的に表示
    public void SetInitCommonItemData()
    {
        Card_drawCommonHyouji();
        CardParamOFF_2();
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
        
        if (pitemlist.eventitemlist[check_counter].ev_magicbook == 0)
        {
            item_Category.text = "レシピ";
            if (pitemlist.eventitemlist[check_counter].event_itemDesc == "non")
            {
                item_RankDesc.text = pitemlist.eventitemlist[check_counter].event_itemNameHyouji;
            }
            else
            {
                item_RankDesc.text = pitemlist.eventitemlist[check_counter].event_itemDesc; //何か入ってるときは説明を表示
            }
            
            
        }
        else
        {
            item_Category.text = "魔法の本";
            item_RankDesc.text = pitemlist.eventitemlist[check_counter].event_itemDesc; //魔法の本は、魔法の説明文を表示
        }

        texture2d = Resources.Load<Sprite>("Sprites/" + pitemlist.eventitemlist[check_counter].event_fileName);

        // texture2dを使い、Spriteを作って、反映させる
        item_Icon.sprite = texture2d;

        //デフォルトカードテンプレとりあえず入力し初期化。　レシピなど。
        item_screen.sprite = Sprite.Create(card_template_1,
                                   new Rect(0, 0, card_template_1.width, card_template_1.height),
                                   Vector2.zero);

        foreach (Transform child in magicview_content.transform)
        {
            Destroy(child.gameObject);
        }
        _magicicon_listitem.Clear();

        SpScoreHyouji_OFF = false; //trueだと強制的にSPスコア表示はオフになる。デバッグモードでも表示されない。一部の画面でのみ。
        //Debug.Log("処理順か？");

        
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
                item_type_subB = database.items[check_counter].itemType_subB.ToString();
                item_type_sub_category = database.items[check_counter].itemType_sub_category.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = database.items[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = database.items[check_counter].itemIcon_sprite;

                //カードのスロット部分の名
                item_SlotName = database.items[check_counter].item_SlotName;

                //カードのアイテム名フル
                _name = database.items[check_counter].itemNameHyouji;                
                
                //アイテムの品質値
                _quality = database.items[check_counter].Quality.ToString();

                //レアリティ
                _rare = database.items[check_counter].Rare;

                //シークレット表示フラグ
                _secretFlag = database.items[check_counter].SecretFlag;

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

                _beauty_score = database.items[check_counter].Beauty;
                _tea_flavor_score = database.items[check_counter].Tea_Flavor;
                _spwind_score = database.items[check_counter].SP_wind;
                _sp_score2 = database.items[check_counter].SP_Score2;
                _sp_score3 = database.items[check_counter].SP_Score3;
                _sp_score4 = database.items[check_counter].SP_Score4;
                _sp_score5 = database.items[check_counter].SP_Score5;
                _sp_score6 = database.items[check_counter].SP_Score6;
                _sp_score7 = database.items[check_counter].SP_Score7;
                _sp_score8 = database.items[check_counter].SP_Score8;
                _sp_score9 = database.items[check_counter].SP_Score9;
                _sp_score10 = database.items[check_counter].SP_Score10;

                _attri1 = database.items[check_counter].Attribute1;
                _attri2 = database.items[check_counter].Attribute2;
                _attri3 = database.items[check_counter].Attribute3;
                _attri4 = database.items[check_counter].Attribute4;
                _attri5 = database.items[check_counter].Attribute5;
                _attri6 = database.items[check_counter].Attribute6;
                _attri7 = database.items[check_counter].Attribute7;
                _attri8 = database.items[check_counter].Attribute8;
                _attri9 = database.items[check_counter].Attribute9;
                _attri10 = database.items[check_counter].Attribute10;

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
                _lastjuice_score = database.items[check_counter].last_juice_score;

                _eat_kaisu = database.items[check_counter].Eat_kaisu;
                _highscore_flag = database.items[check_counter].HighScore_flag;
                _lasttotal_score = database.items[check_counter].last_total_score;
                _lasthint_text = database.items[check_counter].last_hinttext;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = database.items[check_counter].toppingtype[i].ToString();
                }

                for (i = 0; i < _koyuslot.Length; i++)
                {
                    _koyuslot[i] = database.items[check_counter].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < _magicslot.Length; i++)
                {
                    _magicslot[i] = database.items[check_counter].item_MagicSlot[i].ToString();
                    _msvalue[i] = database.items[check_counter].item_MagicSlotValue[i];
                }

                break;

            case 1: //オリジナルプレイヤーアイテムリストを選択した場合

                //アイテムID
                itemID = pitemlist.player_originalitemlist[check_counter].itemID;

                //アイテムタイプを代入//
                item_type = pitemlist.player_originalitemlist[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = pitemlist.player_originalitemlist[check_counter].itemType_sub.ToString();
                item_type_subB = pitemlist.player_originalitemlist[check_counter].itemType_subB.ToString();
                item_type_sub_category = pitemlist.player_originalitemlist[check_counter].itemType_sub_category.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = pitemlist.player_originalitemlist[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = pitemlist.player_originalitemlist[check_counter].itemIcon_sprite;

                //カードのスロット部分の名
                item_SlotName = pitemlist.player_originalitemlist[check_counter].item_SlotName;

                //カードのアイテム名
                //item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + pitemlist.player_originalitemlist[check_counter].itemNameHyouji;
                _name = pitemlist.player_originalitemlist[check_counter].itemNameHyouji;

                //アイテムの品質値
                _quality = pitemlist.player_originalitemlist[check_counter].Quality.ToString();

                //レアリティ
                _rare = pitemlist.player_originalitemlist[check_counter].Rare;

                //シークレット表示フラグ
                _secretFlag = pitemlist.player_originalitemlist[check_counter].SecretFlag;

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

                _beauty_score = pitemlist.player_originalitemlist[check_counter].Beauty;
                _tea_flavor_score = pitemlist.player_originalitemlist[check_counter].Tea_Flavor;
                _spwind_score = pitemlist.player_originalitemlist[check_counter].SP_wind;
                _sp_score2 = pitemlist.player_originalitemlist[check_counter].SP_Score2;
                _sp_score3 = pitemlist.player_originalitemlist[check_counter].SP_Score3;
                _sp_score4 = pitemlist.player_originalitemlist[check_counter].SP_Score4;
                _sp_score5 = pitemlist.player_originalitemlist[check_counter].SP_Score5;
                _sp_score6 = pitemlist.player_originalitemlist[check_counter].SP_Score6;
                _sp_score7 = pitemlist.player_originalitemlist[check_counter].SP_Score7;
                _sp_score8 = pitemlist.player_originalitemlist[check_counter].SP_Score8;
                _sp_score9 = pitemlist.player_originalitemlist[check_counter].SP_Score9;
                _sp_score10 = pitemlist.player_originalitemlist[check_counter].SP_Score10;

                _attri1 = pitemlist.player_originalitemlist[check_counter].Attribute1;
                _attri2 = pitemlist.player_originalitemlist[check_counter].Attribute2;
                _attri3 = pitemlist.player_originalitemlist[check_counter].Attribute3;
                _attri4 = pitemlist.player_originalitemlist[check_counter].Attribute4;
                _attri5 = pitemlist.player_originalitemlist[check_counter].Attribute5;
                _attri6 = pitemlist.player_originalitemlist[check_counter].Attribute6;
                _attri7 = pitemlist.player_originalitemlist[check_counter].Attribute7;
                _attri8 = pitemlist.player_originalitemlist[check_counter].Attribute8;
                _attri9 = pitemlist.player_originalitemlist[check_counter].Attribute9;
                _attri10 = pitemlist.player_originalitemlist[check_counter].Attribute10;

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
                _lastjuice_score = pitemlist.player_originalitemlist[check_counter].last_juice_score;

                _eat_kaisu = pitemlist.player_originalitemlist[check_counter].Eat_kaisu;
                _highscore_flag = pitemlist.player_originalitemlist[check_counter].HighScore_flag;
                _lasttotal_score = pitemlist.player_originalitemlist[check_counter].last_total_score;
                _lasthint_text = pitemlist.player_originalitemlist[check_counter].last_hinttext;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = pitemlist.player_originalitemlist[check_counter].toppingtype[i].ToString();
                }

                for (i = 0; i < _koyuslot.Length; i++)
                {
                    _koyuslot[i] = pitemlist.player_originalitemlist[check_counter].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < _magicslot.Length; i++)
                {
                    _magicslot[i] = pitemlist.player_originalitemlist[check_counter].item_MagicSlot[i].ToString();
                    _msvalue[i] = pitemlist.player_originalitemlist[check_counter].item_MagicSlotValue[i];
                }

                break;

            case 2: //エクストリームパネルに設定したアイテムリストを選択した場合

                //アイテムID
                itemID = pitemlist.player_extremepanel_itemlist[check_counter].itemID;

                //アイテムタイプを代入//
                item_type = pitemlist.player_extremepanel_itemlist[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = pitemlist.player_extremepanel_itemlist[check_counter].itemType_sub.ToString();
                item_type_subB = pitemlist.player_extremepanel_itemlist[check_counter].itemType_subB.ToString();
                item_type_sub_category = pitemlist.player_extremepanel_itemlist[check_counter].itemType_sub_category.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = pitemlist.player_extremepanel_itemlist[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = pitemlist.player_extremepanel_itemlist[check_counter].itemIcon_sprite;

                //カードのスロット部分の名
                item_SlotName = pitemlist.player_extremepanel_itemlist[check_counter].item_SlotName;

                //カードのアイテム名
                _name = pitemlist.player_extremepanel_itemlist[check_counter].itemNameHyouji;

                //アイテムの品質値
                _quality = pitemlist.player_extremepanel_itemlist[check_counter].Quality.ToString();

                //レアリティ
                _rare = pitemlist.player_extremepanel_itemlist[check_counter].Rare;

                //シークレット表示フラグ
                _secretFlag = pitemlist.player_extremepanel_itemlist[check_counter].SecretFlag;

                //甘さなどのパラメータを代入
                _quality_score = pitemlist.player_extremepanel_itemlist[check_counter].Quality;
                _rich_score = pitemlist.player_extremepanel_itemlist[check_counter].Rich;
                _sweat_score = pitemlist.player_extremepanel_itemlist[check_counter].Sweat;
                _bitter_score = pitemlist.player_extremepanel_itemlist[check_counter].Bitter;
                _sour_score = pitemlist.player_extremepanel_itemlist[check_counter].Sour;

                _crispy_score = pitemlist.player_extremepanel_itemlist[check_counter].Crispy;
                _fluffy_score = pitemlist.player_extremepanel_itemlist[check_counter].Fluffy;
                _smooth_score = pitemlist.player_extremepanel_itemlist[check_counter].Smooth;
                _hardness_score = pitemlist.player_extremepanel_itemlist[check_counter].Hardness;

                _juice_score = pitemlist.player_extremepanel_itemlist[check_counter].Juice;

                _beauty_score = pitemlist.player_extremepanel_itemlist[check_counter].Beauty;
                _tea_flavor_score = pitemlist.player_extremepanel_itemlist[check_counter].Tea_Flavor;
                _spwind_score = pitemlist.player_extremepanel_itemlist[check_counter].SP_wind;
                _sp_score2 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score2;
                _sp_score3 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score3;
                _sp_score4 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score4;
                _sp_score5 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score5;
                _sp_score6 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score6;
                _sp_score7 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score7;
                _sp_score8 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score8;
                _sp_score9 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score9;
                _sp_score10 = pitemlist.player_extremepanel_itemlist[check_counter].SP_Score10;

                _attri1 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute1;
                _attri2 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute2;
                _attri3 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute3;
                _attri4 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute4;
                _attri5 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute5;
                _attri6 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute6;
                _attri7 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute7;
                _attri8 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute8;
                _attri9 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute9;
                _attri10 = pitemlist.player_extremepanel_itemlist[check_counter].Attribute10;

                _powdery_score = pitemlist.player_extremepanel_itemlist[check_counter].Powdery;
                _oily_score = pitemlist.player_extremepanel_itemlist[check_counter].Oily;
                _watery_score = pitemlist.player_extremepanel_itemlist[check_counter].Watery;

                //前回の味読み込み
                _lastrich_score = pitemlist.player_extremepanel_itemlist[check_counter].last_rich_score;
                _lastsweat_score = pitemlist.player_extremepanel_itemlist[check_counter].last_sweat_score;
                _lastbitter_score = pitemlist.player_extremepanel_itemlist[check_counter].last_bitter_score;
                _lastsour_score = pitemlist.player_extremepanel_itemlist[check_counter].last_sour_score;

                _lastcrispy_score = pitemlist.player_extremepanel_itemlist[check_counter].last_crispy_score;
                _lastfluffy_score = pitemlist.player_extremepanel_itemlist[check_counter].last_fluffy_score;
                _lastsmooth_score = pitemlist.player_extremepanel_itemlist[check_counter].last_smooth_score;
                _lasthardness_score = pitemlist.player_extremepanel_itemlist[check_counter].last_hardness_score;
                _lastjuice_score = pitemlist.player_extremepanel_itemlist[check_counter].last_juice_score;

                _eat_kaisu = pitemlist.player_extremepanel_itemlist[check_counter].Eat_kaisu;
                _highscore_flag = pitemlist.player_extremepanel_itemlist[check_counter].HighScore_flag;
                _lasttotal_score = pitemlist.player_extremepanel_itemlist[check_counter].last_total_score;
                _lasthint_text = pitemlist.player_extremepanel_itemlist[check_counter].last_hinttext;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = pitemlist.player_extremepanel_itemlist[check_counter].toppingtype[i].ToString();
                }

                for (i = 0; i < _koyuslot.Length; i++)
                {
                    _koyuslot[i] = pitemlist.player_extremepanel_itemlist[check_counter].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < _magicslot.Length; i++)
                {
                    _magicslot[i] = pitemlist.player_extremepanel_itemlist[check_counter].item_MagicSlot[i].ToString();
                    _msvalue[i] = pitemlist.player_extremepanel_itemlist[check_counter].item_MagicSlotValue[i];
                }

                break;

            case 3: //表示などの確認用のチェック用アイテムリストを選択した場合。これはプレーヤは触れず、内部処理用のもの。セーブもされないTempデータ。

                //アイテムID
                itemID = pitemlist.player_check_itemlist[check_counter].itemID;

                //アイテムタイプを代入//
                item_type = pitemlist.player_check_itemlist[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = pitemlist.player_check_itemlist[check_counter].itemType_sub.ToString();
                item_type_subB = pitemlist.player_check_itemlist[check_counter].itemType_subB.ToString();
                item_type_sub_category = pitemlist.player_check_itemlist[check_counter].itemType_sub_category.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = pitemlist.player_check_itemlist[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = pitemlist.player_check_itemlist[check_counter].itemIcon_sprite;

                //カードのスロット部分の名
                item_SlotName = pitemlist.player_check_itemlist[check_counter].item_SlotName;

                //カードのアイテム名
                _name = pitemlist.player_check_itemlist[check_counter].itemNameHyouji;

                //アイテムの品質値
                _quality = pitemlist.player_check_itemlist[check_counter].Quality.ToString();

                //レアリティ
                _rare = pitemlist.player_check_itemlist[check_counter].Rare;

                //シークレット表示フラグ
                _secretFlag = pitemlist.player_check_itemlist[check_counter].SecretFlag;

                //甘さなどのパラメータを代入
                _quality_score = pitemlist.player_check_itemlist[check_counter].Quality;
                _rich_score = pitemlist.player_check_itemlist[check_counter].Rich;
                _sweat_score = pitemlist.player_check_itemlist[check_counter].Sweat;
                _bitter_score = pitemlist.player_check_itemlist[check_counter].Bitter;
                _sour_score = pitemlist.player_check_itemlist[check_counter].Sour;

                _crispy_score = pitemlist.player_check_itemlist[check_counter].Crispy;
                _fluffy_score = pitemlist.player_check_itemlist[check_counter].Fluffy;
                _smooth_score = pitemlist.player_check_itemlist[check_counter].Smooth;
                _hardness_score = pitemlist.player_check_itemlist[check_counter].Hardness;

                _juice_score = pitemlist.player_check_itemlist[check_counter].Juice;

                _beauty_score = pitemlist.player_check_itemlist[check_counter].Beauty;
                _tea_flavor_score = pitemlist.player_check_itemlist[check_counter].Tea_Flavor;
                _spwind_score = pitemlist.player_check_itemlist[check_counter].SP_wind;
                _sp_score2 = pitemlist.player_check_itemlist[check_counter].SP_Score2;
                _sp_score3 = pitemlist.player_check_itemlist[check_counter].SP_Score3;
                _sp_score4 = pitemlist.player_check_itemlist[check_counter].SP_Score4;
                _sp_score5 = pitemlist.player_check_itemlist[check_counter].SP_Score5;
                _sp_score6 = pitemlist.player_check_itemlist[check_counter].SP_Score6;
                _sp_score7 = pitemlist.player_check_itemlist[check_counter].SP_Score7;
                _sp_score8 = pitemlist.player_check_itemlist[check_counter].SP_Score8;
                _sp_score9 = pitemlist.player_check_itemlist[check_counter].SP_Score9;
                _sp_score10 = pitemlist.player_check_itemlist[check_counter].SP_Score10;

                _attri1 = pitemlist.player_check_itemlist[check_counter].Attribute1;
                _attri2 = pitemlist.player_check_itemlist[check_counter].Attribute2;
                _attri3 = pitemlist.player_check_itemlist[check_counter].Attribute3;
                _attri4 = pitemlist.player_check_itemlist[check_counter].Attribute4;
                _attri5 = pitemlist.player_check_itemlist[check_counter].Attribute5;
                _attri6 = pitemlist.player_check_itemlist[check_counter].Attribute6;
                _attri7 = pitemlist.player_check_itemlist[check_counter].Attribute7;
                _attri8 = pitemlist.player_check_itemlist[check_counter].Attribute8;
                _attri9 = pitemlist.player_check_itemlist[check_counter].Attribute9;
                _attri10 = pitemlist.player_check_itemlist[check_counter].Attribute10;

                _powdery_score = pitemlist.player_check_itemlist[check_counter].Powdery;
                _oily_score = pitemlist.player_check_itemlist[check_counter].Oily;
                _watery_score = pitemlist.player_check_itemlist[check_counter].Watery;

                //前回の味読み込み
                _lastrich_score = pitemlist.player_check_itemlist[check_counter].last_rich_score;
                _lastsweat_score = pitemlist.player_check_itemlist[check_counter].last_sweat_score;
                _lastbitter_score = pitemlist.player_check_itemlist[check_counter].last_bitter_score;
                _lastsour_score = pitemlist.player_check_itemlist[check_counter].last_sour_score;

                _lastcrispy_score = pitemlist.player_check_itemlist[check_counter].last_crispy_score;
                _lastfluffy_score = pitemlist.player_check_itemlist[check_counter].last_fluffy_score;
                _lastsmooth_score = pitemlist.player_check_itemlist[check_counter].last_smooth_score;
                _lasthardness_score = pitemlist.player_check_itemlist[check_counter].last_hardness_score;
                _lastjuice_score = pitemlist.player_check_itemlist[check_counter].last_juice_score;

                _eat_kaisu = pitemlist.player_check_itemlist[check_counter].Eat_kaisu;
                _highscore_flag = pitemlist.player_check_itemlist[check_counter].HighScore_flag;
                _lasttotal_score = pitemlist.player_check_itemlist[check_counter].last_total_score;
                _lasthint_text = pitemlist.player_check_itemlist[check_counter].last_hinttext;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = pitemlist.player_check_itemlist[check_counter].toppingtype[i].ToString();
                }

                for (i = 0; i < _koyuslot.Length; i++)
                {
                    _koyuslot[i] = pitemlist.player_check_itemlist[check_counter].koyu_toppingtype[i].ToString();
                }

                for (i = 0; i < _magicslot.Length; i++)
                {
                    _magicslot[i] = pitemlist.player_check_itemlist[check_counter].item_MagicSlot[i].ToString();
                    _msvalue[i] = pitemlist.player_check_itemlist[check_counter].item_MagicSlotValue[i];
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

    //カード描画用のパラメータ　予測表示用
    void Card_drawYosoku()
    {
        //アイテムID
        itemID = pitemlist.player_yosokuitemlist[check_counter].itemID;

        //アイテムタイプを代入//
        item_type = pitemlist.player_yosokuitemlist[check_counter].itemType.ToString();

        //サブカテゴリーの代入
        item_type_sub = pitemlist.player_yosokuitemlist[check_counter].itemType_sub.ToString();
        item_type_subB = pitemlist.player_yosokuitemlist[check_counter].itemType_subB.ToString();
        item_type_sub_category = pitemlist.player_yosokuitemlist[check_counter].itemType_sub_category.ToString();

        /* アイテム解説の表示 */
        item_RankDesc.text = pitemlist.player_yosokuitemlist[check_counter].itemDesc;

        // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
        texture2d = pitemlist.player_yosokuitemlist[check_counter].itemIcon_sprite;

        //カードのスロット部分の名
        item_SlotName = pitemlist.player_yosokuitemlist[check_counter].item_SlotName;

        //カードのアイテム名
        _name = pitemlist.player_yosokuitemlist[check_counter].itemNameHyouji;

        //アイテムの品質値
        _quality = pitemlist.player_yosokuitemlist[check_counter].Quality.ToString();

        //レアリティ
        _rare = pitemlist.player_yosokuitemlist[check_counter].Rare;

        //シークレット表示フラグ
        _secretFlag = pitemlist.player_yosokuitemlist[check_counter].SecretFlag;

        //甘さなどのパラメータを代入
        _quality_score = pitemlist.player_yosokuitemlist[check_counter].Quality;
        _rich_score = pitemlist.player_yosokuitemlist[check_counter].Rich;
        _sweat_score = pitemlist.player_yosokuitemlist[check_counter].Sweat;
        _bitter_score = pitemlist.player_yosokuitemlist[check_counter].Bitter;
        _sour_score = pitemlist.player_yosokuitemlist[check_counter].Sour;

        _crispy_score = pitemlist.player_yosokuitemlist[check_counter].Crispy;
        _fluffy_score = pitemlist.player_yosokuitemlist[check_counter].Fluffy;
        _smooth_score = pitemlist.player_yosokuitemlist[check_counter].Smooth;
        _hardness_score = pitemlist.player_yosokuitemlist[check_counter].Hardness;

        _juice_score = pitemlist.player_yosokuitemlist[check_counter].Juice;

        _beauty_score = pitemlist.player_yosokuitemlist[check_counter].Beauty;
        _tea_flavor_score = pitemlist.player_yosokuitemlist[check_counter].Tea_Flavor;
        _spwind_score = pitemlist.player_yosokuitemlist[check_counter].SP_wind;
        _sp_score2 = pitemlist.player_yosokuitemlist[check_counter].SP_Score2;
        _sp_score3 = pitemlist.player_yosokuitemlist[check_counter].SP_Score3;
        _sp_score4 = pitemlist.player_yosokuitemlist[check_counter].SP_Score4;
        _sp_score5 = pitemlist.player_yosokuitemlist[check_counter].SP_Score5;
        _sp_score6 = pitemlist.player_yosokuitemlist[check_counter].SP_Score6;
        _sp_score7 = pitemlist.player_yosokuitemlist[check_counter].SP_Score7;
        _sp_score8 = pitemlist.player_yosokuitemlist[check_counter].SP_Score8;
        _sp_score9 = pitemlist.player_yosokuitemlist[check_counter].SP_Score9;
        _sp_score10 = pitemlist.player_yosokuitemlist[check_counter].SP_Score10;

        _attri1 = pitemlist.player_yosokuitemlist[check_counter].Attribute1;
        _attri2 = pitemlist.player_yosokuitemlist[check_counter].Attribute2;
        _attri3 = pitemlist.player_yosokuitemlist[check_counter].Attribute3;
        _attri4 = pitemlist.player_yosokuitemlist[check_counter].Attribute4;
        _attri5 = pitemlist.player_yosokuitemlist[check_counter].Attribute5;
        _attri6 = pitemlist.player_yosokuitemlist[check_counter].Attribute6;
        _attri7 = pitemlist.player_yosokuitemlist[check_counter].Attribute7;
        _attri8 = pitemlist.player_yosokuitemlist[check_counter].Attribute8;
        _attri9 = pitemlist.player_yosokuitemlist[check_counter].Attribute9;
        _attri10 = pitemlist.player_yosokuitemlist[check_counter].Attribute10;

        _powdery_score = pitemlist.player_yosokuitemlist[check_counter].Powdery;
        _oily_score = pitemlist.player_yosokuitemlist[check_counter].Oily;
        _watery_score = pitemlist.player_yosokuitemlist[check_counter].Watery;

        //前回の味読み込み
        //_lastquality_score = pitemlist.player_yosokuitemlist[check_counter].Quality;
        _lastrich_score = pitemlist.player_yosokuitemlist[check_counter].last_rich_score;
        _lastsweat_score = pitemlist.player_yosokuitemlist[check_counter].last_sweat_score;
        _lastbitter_score = pitemlist.player_yosokuitemlist[check_counter].last_bitter_score;
        _lastsour_score = pitemlist.player_yosokuitemlist[check_counter].last_sour_score;

        _lastcrispy_score = pitemlist.player_yosokuitemlist[check_counter].last_crispy_score;
        _lastfluffy_score = pitemlist.player_yosokuitemlist[check_counter].last_fluffy_score;
        _lastsmooth_score = pitemlist.player_yosokuitemlist[check_counter].last_smooth_score;
        _lasthardness_score = pitemlist.player_yosokuitemlist[check_counter].last_hardness_score;
        _lastjuice_score = pitemlist.player_yosokuitemlist[check_counter].last_juice_score;

        _eat_kaisu = pitemlist.player_yosokuitemlist[check_counter].Eat_kaisu;
        _highscore_flag = pitemlist.player_yosokuitemlist[check_counter].HighScore_flag;
        _lasttotal_score = pitemlist.player_yosokuitemlist[check_counter].last_total_score;
        _lasthint_text = pitemlist.player_yosokuitemlist[check_counter].last_hinttext;

        for (i = 0; i < _slot.Length; i++)
        {
            _slot[i] = pitemlist.player_yosokuitemlist[check_counter].toppingtype[i].ToString();
        }

        for (i = 0; i < _koyuslot.Length; i++)
        {
            _koyuslot[i] = pitemlist.player_yosokuitemlist[check_counter].koyu_toppingtype[i].ToString();
        }

        for (i = 0; i < _magicslot.Length; i++)
        {
            _magicslot[i] = pitemlist.player_yosokuitemlist[check_counter].item_MagicSlot[i].ToString();
            _msvalue[i] = pitemlist.player_yosokuitemlist[check_counter].item_MagicSlotValue[i];
        }

        //カード　スロット名 現在は、特に表示はしていない
        Slotname_Hyouji();

        //実際にカードの表示を更新する部分
        DrawCardParam();
    }

    //カード描画用のパラメータ　コンテストクリア時のお菓子パラメータ用
    void Card_drawContestClear()
    {
        //アイテムID
        itemID = GameMgr.contestclear_collection_list[check_counter].ItemData.itemID;

        //アイテムタイプを代入//
        item_type = GameMgr.contestclear_collection_list[check_counter].ItemData.itemType.ToString();

        //サブカテゴリーの代入
        item_type_sub = GameMgr.contestclear_collection_list[check_counter].ItemData.itemType_sub.ToString();
        item_type_subB = GameMgr.contestclear_collection_list[check_counter].ItemData.itemType_subB.ToString();
        item_type_sub_category = GameMgr.contestclear_collection_list[check_counter].ItemData.itemType_sub_category.ToString();

        /* アイテム解説の表示 */
        item_RankDesc.text = GameMgr.contestclear_collection_list[check_counter].ItemData.itemDesc;

        // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
        texture2d = GameMgr.contestclear_collection_list[check_counter].ItemData.itemIcon_sprite;

        //カードのスロット部分の名
        item_SlotName = GameMgr.contestclear_collection_list[check_counter].ItemData.item_SlotName;

        //カードのアイテム名
        _name = GameMgr.contestclear_collection_list[check_counter].ItemData.itemNameHyouji;

        //アイテムの品質値
        _quality = GameMgr.contestclear_collection_list[check_counter].ItemData.Quality.ToString();

        //レアリティ
        _rare = GameMgr.contestclear_collection_list[check_counter].ItemData.Rare;

        //シークレット表示フラグ
        _secretFlag = GameMgr.contestclear_collection_list[check_counter].ItemData.SecretFlag;

        //甘さなどのパラメータを代入
        _quality_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Quality;
        _rich_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Rich;
        _sweat_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Sweat;
        _bitter_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Bitter;
        _sour_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Sour;

        _crispy_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Crispy;
        _fluffy_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Fluffy;
        _smooth_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Smooth;
        _hardness_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Hardness;

        _juice_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Juice;

        _beauty_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Beauty;
        _tea_flavor_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Tea_Flavor;
        _spwind_score = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_wind;
        _sp_score2 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score2;
        _sp_score3 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score3;
        _sp_score4 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score4;
        _sp_score5 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score5;
        _sp_score6 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score6;
        _sp_score7 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score7;
        _sp_score8 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score8;
        _sp_score9 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score9;
        _sp_score10 = GameMgr.contestclear_collection_list[check_counter].ItemData.SP_Score10;

        _attri1 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute1;
        _attri2 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute2;
        _attri3 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute3;
        _attri4 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute4;
        _attri5 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute5;
        _attri6 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute6;
        _attri7 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute7;
        _attri8 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute8;
        _attri9 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute9;
        _attri10 = GameMgr.contestclear_collection_list[check_counter].ItemData.Attribute10;

        _powdery_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Powdery;
        _oily_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Oily;
        _watery_score = GameMgr.contestclear_collection_list[check_counter].ItemData.Watery;

        //前回の味読み込み
        _lastrich_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_rich_score;
        _lastsweat_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_sweat_score;
        _lastbitter_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_bitter_score;
        _lastsour_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_sour_score;

        _lastcrispy_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_crispy_score;
        _lastfluffy_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_fluffy_score;
        _lastsmooth_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_smooth_score;
        _lasthardness_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_hardness_score;
        _lastjuice_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_juice_score;

        _eat_kaisu = GameMgr.contestclear_collection_list[check_counter].ItemData.Eat_kaisu;
        _highscore_flag = GameMgr.contestclear_collection_list[check_counter].ItemData.HighScore_flag;
        _lasttotal_score = GameMgr.contestclear_collection_list[check_counter].ItemData.last_total_score;
        _lasthint_text = GameMgr.contestclear_collection_list[check_counter].ItemData.last_hinttext;

        for (i = 0; i < _slot.Length; i++)
        {
            _slot[i] = GameMgr.contestclear_collection_list[check_counter].ItemData.toppingtype[i].ToString();
        }

        for (i = 0; i < _koyuslot.Length; i++)
        {
            _koyuslot[i] = GameMgr.contestclear_collection_list[check_counter].ItemData.koyu_toppingtype[i].ToString();
        }

        for (i = 0; i < _magicslot.Length; i++)
        {
            _magicslot[i] = GameMgr.contestclear_collection_list[check_counter].ItemData.item_MagicSlot[i].ToString();
            _msvalue[i] = GameMgr.contestclear_collection_list[check_counter].ItemData.item_MagicSlotValue[i];
        }

        //カード　スロット名 現在は、特に表示はしていない
        Slotname_Hyouji();

        //実際にカードの表示を更新する部分
        DrawCardParam();
    }

    //カード描画用のパラメータ　汎用　ここに入れたリストのアイテムデータをカード表示する
    void Card_drawCommonHyouji()
    {
        //アイテムID
        itemID = GameMgr.common_itemdatahyouji_list[check_counter].itemID;

        //アイテムタイプを代入//
        item_type = GameMgr.common_itemdatahyouji_list[check_counter].itemType.ToString();

        //サブカテゴリーの代入
        item_type_sub = GameMgr.common_itemdatahyouji_list[check_counter].itemType_sub.ToString();
        item_type_subB = GameMgr.common_itemdatahyouji_list[check_counter].itemType_subB.ToString();
        item_type_sub_category = GameMgr.common_itemdatahyouji_list[check_counter].itemType_sub_category.ToString();

        /* アイテム解説の表示 */
        item_RankDesc.text = GameMgr.common_itemdatahyouji_list[check_counter].itemDesc;

        // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
        _itemid = database.SearchItemID(itemID);
        texture2d = database.items[_itemid].itemIcon_sprite; //ロードすると画像データは読み込みされないので、直接itemsの画像をいれる

        //カードのスロット部分の名
        item_SlotName = GameMgr.common_itemdatahyouji_list[check_counter].item_SlotName;

        //カードのアイテム名
        _name = GameMgr.common_itemdatahyouji_list[check_counter].itemNameHyouji;

        //アイテムの品質値
        _quality = GameMgr.common_itemdatahyouji_list[check_counter].Quality.ToString();

        //レアリティ
        _rare = GameMgr.common_itemdatahyouji_list[check_counter].Rare;

        //シークレット表示フラグ
        _secretFlag = GameMgr.common_itemdatahyouji_list[check_counter].SecretFlag;

        //甘さなどのパラメータを代入
        _quality_score = GameMgr.common_itemdatahyouji_list[check_counter].Quality;
        _rich_score = GameMgr.common_itemdatahyouji_list[check_counter].Rich;
        _sweat_score = GameMgr.common_itemdatahyouji_list[check_counter].Sweat;
        _bitter_score = GameMgr.common_itemdatahyouji_list[check_counter].Bitter;
        _sour_score = GameMgr.common_itemdatahyouji_list[check_counter].Sour;

        _crispy_score = GameMgr.common_itemdatahyouji_list[check_counter].Crispy;
        _fluffy_score = GameMgr.common_itemdatahyouji_list[check_counter].Fluffy;
        _smooth_score = GameMgr.common_itemdatahyouji_list[check_counter].Smooth;
        _hardness_score = GameMgr.common_itemdatahyouji_list[check_counter].Hardness;

        _juice_score = GameMgr.common_itemdatahyouji_list[check_counter].Juice;

        _beauty_score = GameMgr.common_itemdatahyouji_list[check_counter].Beauty;
        _tea_flavor_score = GameMgr.common_itemdatahyouji_list[check_counter].Tea_Flavor;
        _spwind_score = GameMgr.common_itemdatahyouji_list[check_counter].SP_wind;
        _sp_score2 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score2;
        _sp_score3 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score3;
        _sp_score4 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score4;
        _sp_score5 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score5;
        _sp_score6 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score6;
        _sp_score7 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score7;
        _sp_score8 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score8;
        _sp_score9 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score9;
        _sp_score10 = GameMgr.common_itemdatahyouji_list[check_counter].SP_Score10;

        _attri1 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute1;
        _attri2 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute2;
        _attri3 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute3;
        _attri4 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute4;
        _attri5 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute5;
        _attri6 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute6;
        _attri7 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute7;
        _attri8 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute8;
        _attri9 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute9;
        _attri10 = GameMgr.common_itemdatahyouji_list[check_counter].Attribute10;

        _powdery_score = GameMgr.common_itemdatahyouji_list[check_counter].Powdery;
        _oily_score = GameMgr.common_itemdatahyouji_list[check_counter].Oily;
        _watery_score = GameMgr.common_itemdatahyouji_list[check_counter].Watery;

        //前回の味読み込み
        _lastrich_score = GameMgr.common_itemdatahyouji_list[check_counter].last_rich_score;
        _lastsweat_score = GameMgr.common_itemdatahyouji_list[check_counter].last_sweat_score;
        _lastbitter_score = GameMgr.common_itemdatahyouji_list[check_counter].last_bitter_score;
        _lastsour_score = GameMgr.common_itemdatahyouji_list[check_counter].last_sour_score;

        _lastcrispy_score = GameMgr.common_itemdatahyouji_list[check_counter].last_crispy_score;
        _lastfluffy_score = GameMgr.common_itemdatahyouji_list[check_counter].last_fluffy_score;
        _lastsmooth_score = GameMgr.common_itemdatahyouji_list[check_counter].last_smooth_score;
        _lasthardness_score = GameMgr.common_itemdatahyouji_list[check_counter].last_hardness_score;
        _lastjuice_score = GameMgr.common_itemdatahyouji_list[check_counter].last_juice_score;

        _eat_kaisu = GameMgr.common_itemdatahyouji_list[check_counter].Eat_kaisu;
        _highscore_flag = GameMgr.common_itemdatahyouji_list[check_counter].HighScore_flag;
        _lasttotal_score = GameMgr.common_itemdatahyouji_list[check_counter].last_total_score;
        _lasthint_text = GameMgr.common_itemdatahyouji_list[check_counter].last_hinttext;

        for (i = 0; i < _slot.Length; i++)
        {
            _slot[i] = GameMgr.common_itemdatahyouji_list[check_counter].toppingtype[i].ToString();
        }

        for (i = 0; i < _koyuslot.Length; i++)
        {
            _koyuslot[i] = GameMgr.common_itemdatahyouji_list[check_counter].koyu_toppingtype[i].ToString();
        }

        for (i = 0; i < _magicslot.Length; i++)
        {
            _magicslot[i] = GameMgr.common_itemdatahyouji_list[check_counter].item_MagicSlot[i].ToString();
            _msvalue[i] = GameMgr.common_itemdatahyouji_list[check_counter].item_MagicSlotValue[i];
        }

        //カード　スロット名 現在は、特に表示はしていない
        Slotname_Hyouji();

        //実際にカードの表示を更新する部分
        DrawCardParam();
    }

    void DrawCardParam()
    {
        CardTasteView_flag1 = false;
        SpScoreHyouji_OFF = false; //trueだと強制的にSPスコア表示はオフになる。デバッグモードでも表示されない。一部の画面でのみ。
        //Debug.Log("処理順か？");

        // texture2dを使い、Spriteを作って、反映させる
        item_Icon.sprite = texture2d;

        //魔法がかかってるおかしは、エフェクトパネルもON/OFF
        DrawMagicEffect();

        //ウィンドアークなどかけてたら、魔法アイコンを表示
        DrawMagicIconView();        

        //サブカテゴリーを検出し、subCategoryの内容に、日本語名で入力
        if (_secretFlag == 1) //シークレットは少しカードの柄が変わる。
        {
            item_screen.sprite = Sprite.Create(card_template_100,
                                       new Rect(0, 0, card_template_100.width, card_template_100.height),
                                       Vector2.zero);
        }
        else
        {
            switch (item_type_sub)
            {
                //器具系は、枠の色が違う。
                case "Machine":
                    item_screen.sprite = Sprite.Create(card_template_10,
                                       new Rect(0, 0, card_template_10.width, card_template_10.height),
                                       Vector2.zero);
                    break;

                default:

                    //レア度に応じて、カードの枠が変わる。
                    switch (_rare)
                    {
                        case 1:

                            item_screen.sprite = Sprite.Create(card_template_1,
                                       new Rect(0, 0, card_template_1.width, card_template_1.height),
                                       Vector2.zero);
                            break;

                        case 2:

                            item_screen.sprite = Sprite.Create(card_template_2,
                                       new Rect(0, 0, card_template_2.width, card_template_2.height),
                                       Vector2.zero);
                            break;

                        case 3:

                            item_screen.sprite = Sprite.Create(card_template_3,
                                       new Rect(0, 0, card_template_3.width, card_template_3.height),
                                       Vector2.zero);
                            break;

                        case 4:

                            item_screen.sprite = Sprite.Create(card_template_4,
                                       new Rect(0, 0, card_template_4.width, card_template_4.height),
                                       Vector2.zero);
                            break;

                        default: //レア度がないやつ。

                            break;
                    }

                    break;
            }
        }


        /* カテゴリーの表示 */

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

        item_Shokukan.text = "-";
        item_Shokukan_Type.text = "";

        //サブカテゴリーを検出し、subCategoryの内容に、日本語名で入力
        itemsubtypeset_database.SetImageSub(item_type_sub);

        //GameMgr.Item_ShokukanTypeNumは、上のitemsubtypeset_databaseで設定している
        switch (GameMgr.Item_ShokukanTypeNum)
        {
            case 0: //さくさく

                Crispy_Text();
                break;

            case 1: //ふわふわ

                Fluffy_Text();
                break;

            case 2: //なめらか

                Smooth_Text();
                break;

            case 3: //歯ごたえ

                Hardness_Text();
                break;

            case 4: //のどごし

                Juice_Text();
                break;

            case 5: //香り

                Tea_Text();
                break;

            case 90: //食感

                break;

            default:

                break;
        }

        //食感表示
        ShokukanText();

        if (_secretFlag == 1)
        {
            //最終的なテキストを表示 "\n"で改行 隠しレシピは後ろに★がつく
            item_Category.text = category + " - " + GameMgr.Item_subcategoryText + "★";
        }
        else
        {
            //最終的なテキストを表示 "\n"で改行
            item_Category.text = category + " - " + GameMgr.Item_subcategoryText;
        }


        /* カテゴリーここまで */

        //甘さ・苦さ・酸味の表示
        SlotScoreKeisan(); //スロットの見た目の値も表示用に計算
        total_beauty = _beauty_score + _slot_beauty;

        //item_Rich.text = _rich_score.ToString();
        item_Sweat.text = _sweat_score.ToString();
        item_Bitter.text = _bitter_score.ToString();
        item_Sour.text = _sour_score.ToString();
        item_Crispy.text = _crispy_score.ToString();
        item_Fluffy.text = _fluffy_score.ToString();
        item_Smooth.text = _smooth_score.ToString();
        item_Hardness.text = _hardness_score.ToString();
        item_Beauty.text = total_beauty.ToString();

        //SPスコア表示　見た目・風らしさ      
        item_Beauty_debug.text = _beauty_score.ToString() + " + " + (_slot_beauty + _magic_addbeauty).ToString();
        item_Spwind.text = (_spwind_score + _magic_addspscore1).ToString();
        item_Sp_score2.text = (_sp_score2 + _magic_addspscore2).ToString();
        item_Sp_score3.text = (_sp_score3 + _magic_addspscore3).ToString();
        item_Sp_score4.text = (_sp_score4 + _magic_addspscore4).ToString();
        item_Sp_score5.text = (_sp_score5 + _magic_addspscore5).ToString();
        item_Sp_score6.text = (_sp_score6 + _magic_addspscore6).ToString();
        item_Sp_score7.text = (_sp_score7 + _magic_addspscore7).ToString();
        item_Sp_score8.text = (_sp_score8 + _magic_addspscore8).ToString();
        item_Sp_score9.text = (_sp_score9 + _magic_addspscore9).ToString();
        item_Sp_score10.text = (_sp_score10 + _magic_addspscore10).ToString();
        //

        //ゲージの更新
        _Shokukan_slider.value = _shokukan_score;
        _Sweat_slider.value = _sweat_score;
        _Bitter_slider.value = _bitter_score;
        _Sour_slider.value = _sour_score;
        _Crispy_slider.value = _crispy_score;
        _Fluffy_slider.value = _fluffy_score;
        _Smooth_slider.value = _smooth_score;
        _Hardness_slider.value = _hardness_score;
        _Beauty_slider.value = total_beauty;


        //粉っぽさなどの、マイナス要素の表示
        if ( _powdery_score > GameMgr.Watery_Line)
        {
            item_Powdery.text = "粉っぽい";
        } else
        {
            item_Powdery.text = "";
        }
        if (_oily_score > GameMgr.Watery_Line)
        {
            item_Oily.text = "油っぽい";
        }
        else
        {
            item_Oily.text = "";
        }
        //水系アイテムには水っぽいはつかない
        if (item_type_sub == "Juice" || item_type_sub == "Soda" || item_type_sub == "Tea" || item_type_sub == "Tea_Mat" || item_type_sub == "Tea_Potion" || 
            item_type_sub == "Coffee_Mat" || item_type_sub == "Coffee" || item_type_sub == "AromaPotion" ||
            item_type_subB == "a_WaterSoda" || item_type_subB == "a_SugerWater" || item_type_subB == "a_AppaleilJelly")
        {
            item_Watery.text = "";
        }
        else
        {
            if (_watery_score > GameMgr.Watery_Line)
            {
                item_Watery.text = "水っぽい";
            }
            else
            {
                item_Watery.text = "";
            }
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

        

        //名称デフォルト
        item_Name.text = _name;

        //
        //味ウィンドウの表示のON/OFFを分類
        //
        Card_param_obj.SetActive(false);
        Card_param_obj2.SetActive(false);
        TasteSubWindow.SetActive(false);
        Slot_SubWindow.SetActive(true);
        taste_slot_flag = false; //デフォルトでは、スロットを表示

        if (item_type == "Mat" || item_type == "Etc")
        {
            switch (item_type_sub)
            {
                case "Appaleil":

                    DrawTasteWindow1();                   
                    break;

                case "Appaleil_Icecream":

                    DrawTasteWindow1();
                    break;

                case "Source":

                    DrawTasteWindow1();
                    break;

                /*case "Material": //Sourceと似てるが、こっちは表示をしない

                    DrawTasteWindow1();
                    break;*/

                case "Cream":

                    DrawTasteWindow1();
                    break;

                case "Water":

                    DrawTasteWindow1();
                    break;

                case "Fruits":
                    DrawTasteWindow2();
                    break;

                case "FrozenFruits":
                    DrawTasteWindow2();
                    break;

                case "Berry":
                    DrawTasteWindow2();
                    break;

                case "Nuts":
                    DrawTasteWindow2();
                    break;

                case "Harb":
                    DrawTasteWindow2();
                    break;

                case "Flower":
                    DrawTasteWindow2();
                    break;

                case "Vegetable":
                    DrawTasteWindow2();
                    break;

                case "Potion":
                    DrawTasteWindow2();
                    break;

                case "AromaPotion":
                    DrawTasteWindow2();
                    break;

                default:
                    
                    break;
            }

            switch (item_type_subB)
            {
                case "a_LumiSuger":

                    DrawTasteWindow1();
                    break;

                case "a_SugerFlower":

                    DrawTasteWindow1();
                    break;

                case "a_LumiSugerFlower":

                    DrawTasteWindow1();
                    break;                

                case "a_CacaoMass":

                    DrawTasteWindow1();
                    break;
            }

            if(item_type_sub_category == "Original")
            {
                DrawTasteWindow2();
            }

            //回数表示がついてたら、パラメータを表示する。
            if(_attri6 > 0)
            {
                DrawTasteWindow2();
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
            DrawTasteWindow2();

            item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + _name;
        }

        if (_secretFlag == 1)
        {
            Debug.Log("_secretFlag表示ON");
            secret_panel.SetActive(true);
        }
        else
        {
            secret_panel.SetActive(false);
        }

        //味ビュー１が表示されてるかどうかをフラグにも入れる
        CheckTasteViewStatus();

        //スロット切り替えボタンの描画順を更新
        SlotButtonOrderKoushin();
    }

    void DrawTasteWindow1()
    {
        Card_param_obj.SetActive(true); //
        Card_param_obj2.SetActive(false);
        TasteSubWindow.SetActive(true); //生地系のときに、下にさくさく感とかを表示
        Slot_SubWindow.SetActive(false);
        SlotChangeButtonON();
        taste_slot_flag = true; //現在テイストサブウィンドウを表示
        item_Shokukan.text = "-";

        item_Name.text = GameMgr.ColorGold + item_SlotName + "</color>" + _name;
    }

    void DrawTasteWindow2()
    {
        Card_param_obj.SetActive(true);
        Card_param_obj2.SetActive(false);

    }


    //「さくさく感」「歯ごたえ」などの表記 GameMgr.Item_ShokukanTypeTextはItemSubTypeSetDatabaseで設定
    void ShokukanText()
    {
        item_Shokukan_Type.text = "-"; //初期値
        item_lastShokukan_Type.text = "-";
        item_Shokukan_Type.text = GameMgr.Item_ShokukanTypeText;
        item_lastShokukan_Type.text = GameMgr.Item_ShokukanTypeText;
    }

    void Crispy_Text()
    {
        item_Shokukan.text = _crispy_score.ToString();
        _shokukan_score = _crispy_score;
        _lastshokukan_score = _lastcrispy_score;
    }

    void Fluffy_Text()
    {
        item_Shokukan.text = _fluffy_score.ToString();
        _shokukan_score = _fluffy_score;
        _lastshokukan_score = _lastfluffy_score;
    }

    void Smooth_Text()
    {
        item_Shokukan.text = _smooth_score.ToString();
        _shokukan_score = _smooth_score;
        _lastshokukan_score = _lastsmooth_score;
    }

    void Hardness_Text()
    {
        item_Shokukan.text = _hardness_score.ToString();
        _shokukan_score = _hardness_score;
        _lastshokukan_score = _lasthardness_score;
    }

    void Juice_Text()
    {
        item_Shokukan.text = _juice_score.ToString();
        _shokukan_score = _juice_score;
        _lastshokukan_score = _lastjuice_score;
    }

    void Tea_Text()
    {
        item_Shokukan.text = _tea_flavor_score.ToString();
        _shokukan_score = _tea_flavor_score;
        _lastshokukan_score = _lastcrispy_score;
    }



    void DrawMagicEffect()
    {
        //Debug.Log("魔法エフェクト　カード表示check");

        item_MS_aisho.text = "";

        //魔法の食感計算の表示部分
        itemCardEffect_database.MagicEffect_SlotKeisan(_magicslot, _msvalue, itemID, 0);
        item_MS_aisho.text = itemCardEffect_database.item_MS_aisho;

        _magic_addbeauty = itemCardEffect_database._add_magicbeauty;
        _magic_addspscore1 = itemCardEffect_database._ms_sp_score1;
        _magic_addspscore2 = itemCardEffect_database._ms_sp_score2;
        _magic_addspscore3 = itemCardEffect_database._ms_sp_score3;
        _magic_addspscore4 = itemCardEffect_database._ms_sp_score4;
        _magic_addspscore5 = itemCardEffect_database._ms_sp_score5;
        _magic_addspscore6 = itemCardEffect_database._ms_sp_score6;
        _magic_addspscore7 = itemCardEffect_database._ms_sp_score7;
        _magic_addspscore8 = itemCardEffect_database._ms_sp_score8;
        _magic_addspscore9 = itemCardEffect_database._ms_sp_score9;
        _magic_addspscore10 = itemCardEffect_database._ms_sp_score10;

        //魔法のエフェクト表示部分
        itemEffectPanel.GetComponent<ItemCardEffectPanel>().MagicEffect_Hyouji(_magicslot, 0); //2番目の数字は、アクセスする場所を指定　0=カードから
    }

    void DrawMagicIconView()
    {
        //Debug.Log("魔法アイコン　カード表示check");
        Debug.Log("_attri2*(WindArk回数) " + _attri2);
        Debug.Log("_attri4*(生地混ぜ回数) " + _attri4);
        Debug.Log("_attri5*(FireArk回数) " + _attri5);
        Debug.Log("_attri6*(素材の変質回数) " + _attri6);

        foreach (Transform child in magicview_content.transform)
        {
            Destroy(child.gameObject);
        }
        _magicicon_listitem.Clear();

        if (_attri2 > 0) //WindArk回数
        {
            for (i = 0; i < _attri2; i++)
            {
                _magicicon_listitem.Add(Instantiate(magicPrefab, magicview_content.transform));
            }
        }
        if (_attri4 > 0) //生地まぜ回数
        {
            for (i = 0; i < _attri4; i++)
            {
                _magicicon_listitem.Add(Instantiate(magicPrefab2, magicview_content.transform));
            }
        }
        if (_attri5 > 0) //FireArk回数
        {
            for (i = 0; i < _attri5; i++)
            {
                _magicicon_listitem.Add(Instantiate(magicPrefab3, magicview_content.transform));
            }
        }
        if (_attri6 > 0) //ライトニンググレープなどの変質回数
        {
            for (i = 0; i < _attri6; i++)
            {
                _magicicon_listitem.Add(Instantiate(magicPrefab4, magicview_content.transform));
            }
        }
        if (_attri7 > 0) //手作りのぬくもり回数
        {
            for (i = 0; i < _attri7; i++)
            {
                _magicicon_listitem.Add(Instantiate(magicPrefab5, magicview_content.transform));
            }
        }
    }

    //調合完了後、カードのボタンを押すと呼び出される。
    public void CompoundResult_Button()
    {
        if (GameMgr.compound_select != 8)
        {
            //レベルアップチェック用オブジェクトの取得
            exp_table = GameObject.FindWithTag("ExpTable").GetComponent<ExpTable>();

            //Expコントローラーの取得
            exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

            //ブラックエフェクトを取得
            BlackImage = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/BlackImage").gameObject; //魔法エフェクト用の半透明で幕

            //カード表示用オブジェクトの取得
            card_view_obj = GameObject.FindWithTag("CardView");
            card_view = card_view_obj.GetComponent<CardView>();


            //半透明黒パネルはoff
            BlackImage.GetComponent<CanvasGroup>().alpha = 0;

            //チュートリアル最初のクッキーのときだけは、強制的に新しいお菓子閃いたことにする
            if (GameMgr.tutorial_ON == true)
            {
                if (GameMgr.tutorial_Num == 75 || GameMgr.tutorial_Num == 265)
                {
                    exp_Controller.NewRecipiFlag = true;
                    exp_Controller.NewRecipi_compoID = exp_Controller.result_ID; //コンポ調合データベースのIDを代入
                }
            }

            //新しいレシピをひらめいたかどうかチェック
            if (exp_Controller.NewRecipiFlag == true)
            {
                newrecipi_id = exp_Controller.NewRecipi_compoID;

                //調合DBの名前と一致するものを、アイテムDBから検索。表示名前と画像を取得
                /*i = 0;
                while (i < database.items.Count)
                {
                    if (database.items[i].itemName == databaseCompo.compoitems[newrecipi_id].cmpitemID_result)
                    {
                        newrecipi_name = database.items[i].itemNameHyouji;
                        newrecipi_Img = database.items[i].itemIcon_sprite;
                        break;
                    }
                    i++;
                }*/

                newrecipi_name = GameMgr.System_newrecipi_name;
                newrecipi_Img = GameMgr.System_newrecipi_sprite;

                //取得
                NewRecipi = Instantiate(NewRecipi_Prefab1, canvas.transform);
                newrecipi_text = NewRecipi.transform.Find("Panel/RecipiTextPanel/Text").gameObject.GetComponent<Text>();
                newrecipi_Img_hyouji = NewRecipi.transform.Find("Panel/ItemPanel/ItemImage").gameObject.GetComponent<Image>();

                //表示
                newrecipi_text.text = GameMgr.ColorLemon + newrecipi_name + "</color>" + "\n" + "を覚えた！";

                // texture2dを使い、Spriteを作って、反映させる
                newrecipi_Img_hyouji.sprite = newrecipi_Img;

                //アニメーション
                //まず、初期値。
                Sequence sequence = DOTween.Sequence();
                NewRecipi.transform.Find("Panel/ItemPanel").GetComponent<CanvasGroup>().alpha = 0;
                sequence.Append(NewRecipi.transform.Find("Panel/ItemPanel").DOScale(new Vector3(0.65f, 0.65f, 1.0f), 0.0f)
                    ); //

                //移動のアニメ
                sequence.Append(NewRecipi.transform.Find("Panel/ItemPanel").DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f)
                    .SetEase(Ease.OutElastic)); //はねる動き
                                                //.SetEase(Ease.OutExpo)); //スケール小からフェードイン
                sequence.Join(NewRecipi.transform.Find("Panel/ItemPanel").GetComponent<CanvasGroup>().DOFade(1, 0.2f));


                //音を鳴らす 新しいレシピ閃いたときの音 scのほうに音を送ると、途中で音が途切れない。
                sc.PlaySe(25);

                exp_Controller.NewRecipiFlag = false; //オフにしておく。

                card_view.DeleteCard_DrawView();
            }
            else
            {

                //完成時パネルの取得
                CompleteImage = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/CompletePanel").gameObject; //調合成功時のイメージパネル
                CompleteImage.SetActive(false);

                //別画面で（たとえばピクニック中など）戻るときは、status=0にならず、また調合画面に戻る。
                if (GameMgr.picnic_event_reading_now)
                {
                    GameMgr.compound_status = 6; // 調合の画面に戻る。
                }
                else
                {
                    //調合完了後、また調合画面に戻るか、メイン画面に戻るか
                    switch (GameMgr.compound_select)
                    {
                        case 1: //レシピ調合

                            if (GameMgr.OkashiMake_PanelSetType != 0) //新しいお菓子がセットされているので、一度オフ
                            {
                                GameMgr.compound_status = 0;
                                GameMgr.CompoundSceneStartON = false; //調合シーン終了

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
                                GameMgr.compound_status = 1; // もう一回、レシピ調合の画面に戻る。
                            }
                            break;

                        case 3: //オリジナル調合

                            if (GameMgr.OkashiMake_PanelSetType != 0) //新しいお菓子がセットされているので、一度オフ
                            {
                                GameMgr.compound_status = 0;
                                GameMgr.CompoundSceneStartON = false; //調合シーン終了

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
                                GameMgr.compound_status = 3; // もう一回、オリジナル調合の画面に戻る。
                            }
                            break;

                        default:

                            GameMgr.CompoundSceneStartON = false; //調合シーン終了
                            GameMgr.compound_status = 0;
                            break;

                    }
                }

                exp_Controller.EffectListClear();
                card_view.DeleteCard_DrawView();

            }

        }
        else
        {
            hikarimake_startpanel = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/HikariMakeStartPanel").GetComponent<HikariMakeStartPanel>();
            hikarimake_startpanel.ResultHikariMakeCardView_andOFF();
        }
    }
   

    //調合アニメ時、パラメータ部分はオフにする。
    public void CardParamON()
    {
        Card_param_obj.SetActive(true);
        Card_param_obj2.SetActive(true);
        CheckTasteViewStatus();
    }

    public void CardParamOFF()
    {
        Card_param_obj.SetActive(false);
        Card_param_obj2.SetActive(false);
        CheckTasteViewStatus();
    }

    public void CardParamOFF_2()
    {
        Card_param_obj2.SetActive(false);
        CheckTasteViewStatus();
    }

    public void CardParamSpScoreOFF() //外部からSpスコア表示はオフにする。強制力が高い。
    {
        SpScoreHyouji_OFF = true;
        debugTaste_ScorePanel.SetActive(false);
        Debug.Log("カードSPスコア表示オフ");
    }

    public void CardOFF_ItemOnlyHyouji() //カード類の表示をオフにし、アイテムアイコンのみ表示
    {
        Card_TemplateMain_obj.GetComponent<Image>().enabled = false;
        Card_TemplateMain_obj.transform.Find("ItemName").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("ItemCategory").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("ItemRankDesc").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("ItemRankDesc").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("ItemKosu_Panel").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("SecretPanel").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("HlvBonusPanel").gameObject.SetActive(false);
        Card_TemplateMain_obj.transform.Find("MagicIconView").gameObject.SetActive(false);

        CardParamOFF();
    }

    public void CardOFF_ItemAnimON() //さらにアイテムをふわふわとアニメする
    {
        Card_TemplateMain_obj.GetComponent<Animator>().enabled = true;
    }

    public void CardOFF_PlateHyouji() //さらにお皿画像を表示　仕上げのときに使う
    {
        Card_TemplateMain_obj.transform.Find("PlateImage").gameObject.SetActive(true);
    }

    public void CardOFF_PlateHyoujiOFF() //お皿画像表示をオフ
    {
        Card_TemplateMain_obj.transform.Find("PlateImage").gameObject.SetActive(false);
    }

    public void CardALLParamOFF() //カードの表示そのものもオフにする。ただし、調合リザルトボタンはONのまま
    {
        Card_TemplateMain_obj.SetActive(false);
        Card_param_obj.SetActive(false);
        Card_param_obj2.SetActive(false);
        CheckTasteViewStatus();
    }

    public void CardMagicEffect_OFF() //演出魔法エフェクトの表示をオフ
    {
        itemEffectPanel.SetActive(false);
    }

    public void CardParamMagic_Hyouji(string _magicname)
    {
        _mid = magicskill_database.SearchSkillString(_magicname);

        _name = magicskill_database.magicskill_lists[_mid].skillNameHyouji;

        //item_RankDesc.text = "";
        item_Name.text = _name;
    }

    public void SecretFlag_Hyouji()
    {
        Debug.Log("_secretFlag: " + _secretFlag);
        //シークレットアイテムの場合、シークレット表示
        if ( _secretFlag == 1)
        {
            Debug.Log("_secretFlag表示ON");            
            secret_panel.SetActive(true);
        }
    }

    void CheckTasteViewStatus()
    {
        //味ビュー１が表示されてるかどうかをフラグにも入れる CardViewなどから読みだして、使う
        if (Card_param_obj.activeInHierarchy)
        {
            CardTasteView_flag1 = true;
        }
        else
        {
            CardTasteView_flag1 = false;
        }
    }

    public void HLVBonus_Hyouji()
    {
        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        if (exp_Controller.Comp_method_bunki == 0 || exp_Controller.Comp_method_bunki == 2) //オリジナル調合かレシピ調合の場合のみ。
        {
            if (databaseCompo.compoitems[exp_Controller.result_ID].buf_kouka_on != 0) //バフ計算するものだけ、バフ計算。例えばクッキー×ぶどう＝ぶどうクッキーのときは、バフ計算しない
            {
                hlvbonus_panel.SetActive(true);
            }
        }
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

    public void OnSlotHyoujiChangeButton()
    {
        if(taste_slot_flag) //テイストサブウィンドウがオンの場合は、スロット表示に切り替え
        {
            taste_slot_flag = false;
            TasteSubWindow.SetActive(false);
            Slot_SubWindow.SetActive(true);
        }
        else
        {
            taste_slot_flag = true;
            TasteSubWindow.SetActive(true);
            Slot_SubWindow.SetActive(false);
        }
    }

    public void SlotButtonOrderKoushin() //スロット切り替えボタンの描画順を更新
    {
        slot_hyouji_button.Koushin();
    }

    public void SlotChangeButtonON()
    {
        SlotChangeButton.SetActive(true);
    }

    public void SlotChangeButtonOFF()
    {
        SlotChangeButton.SetActive(false);
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

    void InitializeItemSlotDicts()
    {
        itemslotInfo.Clear();
        itemslotScore.Clear();

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslotScore.Add(0);
        }

    }

    public void UseToggleSetInit(int _toggletype, int _kettei_item1)
    {
        this.transform.Find("CardUseSelect_ScrollView").gameObject.SetActive(true);
        foreach (Transform child in this.transform.Find("CardUseSelect_ScrollView/Viewport/Content/").transform) //
        {
            child.gameObject.SetActive(false);
        }
        this.transform.Find("CardUseSelect_ScrollView/Viewport/Content/CardCancel_Toggle").gameObject.SetActive(true);

        //飾るアイテム表示
        foreach (string key in GameMgr.BGAcceItemsName.Keys)
        {
            if (key == database.items[_kettei_item1].itemName)
            {
                //this.transform.Find("CardUseSelect_ScrollView").gameObject.SetActive(true);
                this.transform.Find("CardUseSelect_ScrollView/Viewport/Content/CardDeco_Toggle").gameObject.SetActive(true);
            }
        }

        //食器アイテム表示
        foreach (string key2 in GameMgr.PlateSetItemsName.Keys)
        {
            if (key2 == database.items[_kettei_item1].itemName)
            {
                //this.transform.Find("CardUseSelect_ScrollView").gameObject.SetActive(true);
                this.transform.Find("CardUseSelect_ScrollView/Viewport/Content/CardPlate_Toggle").gameObject.SetActive(true);
            }
        }

        //コレクションアイテムは、「コレクション」ボタンを表示する。
        /*
        for( i=0; i< GameMgr.CollectionItemsName.Count; i++)
        {
            if(database.items[_kettei_item1].itemName == GameMgr.CollectionItemsName[i])
            {
                _cardImage_obj[0].transform.Find("CardUseSelect_ScrollView").gameObject.SetActive(true);
                _cardImage_obj[0].transform.Find("CardUseSelect_ScrollView/Viewport/Content/CardCollect_Toggle").gameObject.SetActive(true);
            }
        }
        */
    }

    //主に、デバッグの見た目のパラメータ計算用　実際の_basebeautyの値は、スロットの値は含まないが、結局GirlEatJudgeでスロットの見た目の値も加算して計算するので、
    //ここで表示をして分かりやすく確認
    void SlotScoreKeisan()
    {
        //一回まず各スコアを初期化。
        for (i = 0; i < itemslotScore.Count; i++)
        {
            itemslotScore[i] = 0;
        }

        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _slot.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                if (_slot[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    itemslotScore[count]++;
                }
                count++;
            }
        }

        //お菓子の固有トッピングスロットも見る。一致する効果があれば、所持数+1。
        for (i = 0; i < _koyuslot.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                if (_koyuslot[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log("_koyutp: " + _koyuslot[i]);
                    itemslotScore[count]++;
                }
                count++;
            }
        }

        _slot_beauty = 0;
        //①  スロットがついていれば、絶対に加算される。スロットネームDBのtotal_scoreを加算する。
        for (i = 0; i < itemslotScore.Count; i++)
        {

            //0はNonなので、無視
            if (i != 0)
            {
                //トッピングされているものに応じて、得点+見た目点数。２つは、意味は違うが、実質の計算上一緒なので、topping_scoreは基本0でもいいかも。
                if (itemslotScore[i] > 0)
                {
                    _slot_beauty += slotnamedatabase.slotname_lists[i].slot_Beauty * itemslotScore[i]; //見た目に対するボーナス得点
                }

            }
        }
    }
}