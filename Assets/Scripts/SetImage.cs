using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private GameObject NewRecipi_Prefab1;
    private GameObject NewRecipi;
    private int newrecipi_id;
    private Text newrecipi_text;
    private string newrecipi_name;
    private Texture2D newrecipi_Img;
    private Image newrecipi_Img_hyouji;

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

    private Image item_Icon;
    private Text item_Name;

    private Text item_Rank;
    private Text item_RankDesc;
    private string rank;

    private string item_type;
    private string item_type_sub;

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

    private Text item_Crispy;
    private Text item_Fluffy;
    private Text item_Smooth;
    private Text item_Hardness;
    private Text item_Jiggly;
    private Text item_Chewy;

    private Text item_Powdery;
    private Text item_Oily;
    private Text item_Watery;

    private Text[] item_Slot = new Text[10];

    private int i, count;

    private int _quality_score;
    private int _rich_score;
    private int _sweat_score;
    private int _sour_score;
    private int _bitter_score;

    private int _crispy_score;
    private int _fluffy_score;
    private int _smooth_score;
    private int _hardness_score;
    private int _jiggly_score;
    private int _chewy_score;

    private int _powdery_score;
    private int _oily_score;
    private int _watery_score;

    private Slider _Crispy_slider;

    public int check_counter;
    public int Pitem_or_Origin; //プレイヤーアイテムか、オリジナルアイテムかの判定

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    private SoundController sc;


    // Use this for initialization
    void Start()
    {

        SetData();

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

        item_Name_Full = this.transform.Find("Card_Param_window/Card_Name/Tx_Name").gameObject.GetComponent<Text>(); //名前（スロット名も含む正式名称）の値

        item_Quality = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Quality/Quality_Rank").gameObject.GetComponent<Text>(); //品質のランク
        item_Quality_Bar = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Quality/Quality_Bar").gameObject.GetComponent<Text>(); //品質の★の数
        item_Quality_Score = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Quality/Quality_Score").gameObject.GetComponent<Text>(); //品質の★の数

        item_Sweat = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSweatScore").gameObject.GetComponent<Text>(); //甘さの値
        item_Bitter = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemBitterScore").gameObject.GetComponent<Text>(); //苦さの値
        item_Sour = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemSourScore").gameObject.GetComponent<Text>(); //すっぱさの値

        item_Rich = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemRichScore").gameObject.GetComponent<Text>(); //味のコクの値
        item_Crispy = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemCrispyScore").gameObject.GetComponent<Text>(); //さくさくの値
        item_Fluffy = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemFluffyScore").gameObject.GetComponent<Text>(); //ふわふわの値
        item_Smooth = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemSmoothScore").gameObject.GetComponent<Text>(); //しっとりの値
        item_Hardness = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemHardnessScore").gameObject.GetComponent<Text>(); //ほろほろの値
        item_Jiggly = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemJigglyScore").gameObject.GetComponent<Text>(); //ぷるぷるの値
        item_Chewy = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemChewyScore").gameObject.GetComponent<Text>(); //ぐみぐみの値

        item_Powdery = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemPowdery").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_Oily = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemOily").gameObject.GetComponent<Text>(); //粉っぽいの値
        item_Watery = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemWatery").gameObject.GetComponent<Text>(); //粉っぽいの値

        item_Slot[0] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_01").gameObject.GetComponent<Text>(); //Slot01の値
        item_Slot[1] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_02").gameObject.GetComponent<Text>(); //Slot02の値
        item_Slot[2] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_03").gameObject.GetComponent<Text>(); //Slot03の値
        item_Slot[3] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_04").gameObject.GetComponent<Text>(); //Slot04の値
        item_Slot[4] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_05").gameObject.GetComponent<Text>(); //Slot05の値
        item_Slot[5] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_06").gameObject.GetComponent<Text>(); //Slot06の値
        item_Slot[6] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_07").gameObject.GetComponent<Text>(); //Slot07の値
        item_Slot[7] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_08").gameObject.GetComponent<Text>(); //Slot08の値
        item_Slot[8] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_09").gameObject.GetComponent<Text>(); //Slot09の値
        item_Slot[9] = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Slot/ItemSlot_10").gameObject.GetComponent<Text>(); //Slot10の値

        //各パラメータバーの取得
        _Crispy_slider = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window_Taste/ItemCrispyScore/Slider").gameObject.GetComponent<Slider>();

        //スロットをもとに、正式名称を計算するメソッド
        slotchangename = GameObject.FindWithTag("SlotChangeName").gameObject.GetComponent<SlotChangeName>();

        //新レシピプレファブの取得
        NewRecipi_Prefab1 = (GameObject)Resources.Load("Prefabs/NewRecipiMessage");

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        SetData();
    }

    public void SetInit()
    {
        Card_draw();
    }

    public void SetYosokuInit() //生成するカードのパラメータを、あらかじめ予測して表示する
    {
        //合成計算オブジェクトの取得
        compound_keisan = GameObject.FindWithTag("Compound_Keisan").GetComponent<Compound_Keisan>();

        Card_YosokuDraw();
    }

    void Card_draw()
    {
        
        switch (Pitem_or_Origin)
        {
            case 0: //プレイヤーアイテムリストを選択した場合
                //Debug.Log("プレイヤーアイテムリスト　check_counter:" + check_counter);

                Card_param_obj.SetActive(false);
                Pitemlist_CardDraw();
                break;

            case 1: //オリジナルアイテムリストを選択した場合
                //Debug.Log("オリジンアイテムリスト　check_counter:" + check_counter);

                //プレイヤー所持アイテムリストの取得
                pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

                //オリジナルアイテムのときだけ、効果覧を表示
                Card_param_obj.SetActive(true);

                Pitemlist_CardDraw();
                break;

            default:
                break;
        }

    }


    //カード描画用のパラメータ読み込み
    void Pitemlist_CardDraw() {

        switch (Pitem_or_Origin)
        {
            case 0: //店売りアイテムリストを選択した場合

                //アイテムタイプを代入//
                item_type = database.items[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = database.items[check_counter].itemType_sub.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = database.items[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = database.items[check_counter].itemIcon;

                //カードのアイテム名
                item_Name.text = database.items[check_counter].itemNameHyouji;

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

                _powdery_score = database.items[check_counter].Powdery;
                _oily_score = database.items[check_counter].Oily;
                _watery_score = database.items[check_counter].Watery;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = database.items[check_counter].toppingtype[i].ToString();
                }


                //カード正式名称（ついてるスロット名も含めた名前）

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


                break;

            case 1: //オリジナルプレイヤーアイテムリストを選択した場合

                //アイテムタイプを代入//
                item_type = pitemlist.player_originalitemlist[check_counter].itemType.ToString();

                //サブカテゴリーの代入
                item_type_sub = pitemlist.player_originalitemlist[check_counter].itemType_sub.ToString();

                /* アイテム解説の表示 */
                item_RankDesc.text = pitemlist.player_originalitemlist[check_counter].itemDesc;

                // アイテムデータベース(ItemDataBaseスクリプト・オブジェクト）に登録された「0」番のアイテムアイコンを、texture2d型の変数へ取得。「itemIcon」画像はTexture2D型で読み込んでる。
                texture2d = pitemlist.player_originalitemlist[check_counter].itemIcon;

                //カードのアイテム名
                item_Name.text = pitemlist.player_originalitemlist[check_counter].itemNameHyouji;

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

                _powdery_score = pitemlist.player_originalitemlist[check_counter].Powdery;
                _oily_score = pitemlist.player_originalitemlist[check_counter].Oily;
                _watery_score = pitemlist.player_originalitemlist[check_counter].Watery;

                for (i = 0; i < _slot.Length; i++)
                {
                    _slot[i] = pitemlist.player_originalitemlist[check_counter].toppingtype[i].ToString();
                }

                //カード正式名称（ついてるスロット名も含めた名前）

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


                break;

            default:
                break;
        }

        DrawCard();
    }

    void Card_YosokuDraw()
    {
        check_counter = compound_keisan._baseID;

        //アイテムタイプを代入//
        item_type = compound_keisan._base_itemType;

        //サブカテゴリーの代入
        item_type_sub = compound_keisan._base_itemType_sub;

        /* アイテム解説の表示 */
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


        for (i = 0; i < _slot.Length; i++)
        {
            _slot[i] = compound_keisan._basetp[i].ToString();
        }


        //カード正式名称（ついてるスロット名も含めた名前）

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

        DrawCard();
    }

    void DrawCard()
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
            default:
                category = "";
                break;
        }

        //サブカテゴリーを検出し、subCategoryの内容に、日本語名で入力
        switch (item_type_sub)
        {
            case "Non":
                subcategory = "";
                //RankDesc_Hyouji();
                break;
            case "Cookie":
                subcategory = "クッキー系";
                //RankDesc_Hyouji();
                break;
            case "Pie":
                subcategory = "パイ系";
                //RankDesc_Hyouji();
                break;
            case "Chocolate":
                subcategory = "チョコレート系";
                //RankDesc_Hyouji();
                break;
            case "Cake":
                subcategory = "ケーキ系";
                //RankDesc_Hyouji();
                break;
            case "Fruits":
                subcategory = "フルーツ";
                break;
            case "Nuts":
                subcategory = "ナッツ";
                break;
            case "Source":
                subcategory = "お菓子材料";
                break;
            case "Pate":
                subcategory = "生地";
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
            case "IceCream":
                subcategory = "アイスクリーム";
                break;
            case "Parfe":
                subcategory = "パフェ";
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
        item_Rich.text = _rich_score.ToString();
        item_Sweat.text = _sweat_score.ToString();
        item_Bitter.text = _bitter_score.ToString();
        item_Sour.text = _sour_score.ToString();

        item_Crispy.text = _crispy_score.ToString();
        item_Fluffy.text = _fluffy_score.ToString();
        item_Smooth.text = _smooth_score.ToString();
        item_Hardness.text = _hardness_score.ToString();
        item_Jiggly.text = _jiggly_score.ToString();
        item_Chewy.text = _chewy_score.ToString();

        //ゲージの更新
        _Crispy_slider.value = _crispy_score;


        //粉っぽさなどの、マイナス要素の表示
        if( _powdery_score > 50 )
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


        //品質の表示
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



        //スロット名の表示+初期化

        for (i = 0; i < _slot.Length; i++)
        {
            if ( _slot[i] == "Non" ) //Nonは空白表示。
            {
                _slot[i] = "";
            }
       
            item_Slot[i].text = _slotHyouji1[i]; //スロット表示１のほうが、スロットに表示する用のテキスト。スロット表示２は、アイテムのフルネームのほう。
        }

        for ( i = 0; i < _slotHyouji2.Length; i++ )
        {
            _slotHyouji2[i] = "";
        }
        


        if (item_type == "Mat")
        {
            switch (item_type_sub)
            {
                /*case "Appaleil":
                    Card_param_obj.SetActive(true);
                    break;*/

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
                    //Card_param_obj.SetActive(true);
                    break;
            }
        }
        else if (item_type == "Okashi")
        {
            //スロットの正式名称計算
            slotchangename.slotChangeName(Pitem_or_Origin, check_counter, "blue");

            _slotHyouji2[0] = slotchangename._slotHyouji[0];
            _slotHyouji2[1] = slotchangename._slotHyouji[1];
            _slotHyouji2[2] = slotchangename._slotHyouji[2];
            _slotHyouji2[3] = slotchangename._slotHyouji[3];
            _slotHyouji2[4] = slotchangename._slotHyouji[4];
            _slotHyouji2[5] = slotchangename._slotHyouji[5];
            _slotHyouji2[6] = slotchangename._slotHyouji[6];
            _slotHyouji2[7] = slotchangename._slotHyouji[7];
            _slotHyouji2[8] = slotchangename._slotHyouji[8];
            _slotHyouji2[9] = slotchangename._slotHyouji[9];

            //スロット名+アイテム名の表示
            item_Name_Full.text = _slotHyouji2[0] + _slotHyouji2[1] + _slotHyouji2[2] + _slotHyouji2[3] + _slotHyouji2[4] + _slotHyouji2[5] + _slotHyouji2[6] + _slotHyouji2[7] + _slotHyouji2[8] + _slotHyouji2[9] + item_Name.text;
            //item_Name_Full.text = "<color=#0000FF>" + slot_Hyouji + "</color>" + item_Name.text;
            item_Name.text = item_Name_Full.text; //お菓子
            Card_param_obj.SetActive(true);
        }
        else
        {
            //Card_param_obj.SetActive(true);
        }


    }

    public void CompoundResult_Button()
    {
        //レベルアップチェック用オブジェクトの取得
        exp_table = GameObject.FindWithTag("ExpTable").GetComponent<ExpTable>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //エクストリームパネルオブジェクトの取得
        extremePanel_obj = canvas.transform.Find("ExtremePanel").gameObject;
        extremePanel = extremePanel_obj.GetComponent<ExtremePanel>();

        if (exp_table.check_on == true)
        {
            //レベルチェック中は、カードを消せないようにする。
        }
        else
        {
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

                Destroy(this.gameObject);

            }
        }
    }

    //調合アニメ時、パラメータ部分はオフにする。
    public void CardParamOFF()
    {
        Card_param_obj.SetActive(false);
    }
}