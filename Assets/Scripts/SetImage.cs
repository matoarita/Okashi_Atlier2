﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// アイテム画像を表示するスクリプト


public class SetImage : MonoBehaviour
{

    public Image item_screen;
    //public GameObject gameobject; //publicで宣言すると、unityヒエラルキー上で見えるようになる。そこに、他のゲームオブジェクトを紐づけすることができる。

    //private Sprite sprite;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject Card_param_obj;

    private PlayerItemList pitemlist;
    private ExpTable exp_table;

    private SlotNameDataBase slotnamedatabase;

    private Texture2D texture2d;
    private Texture2D card_template_1;
    private Texture2D card_template_2;
    private ItemDataBase database;

    private Image item_Icon;
    private Text item_Name;

    private Text item_Rank;
    private Text item_RankDesc;
    private string rank;

    private string item_type;
    private string item_type_sub;

    private string _quality;
    private string _quality_bar;

    private string _rich;
    private string _sweat;
    private string _sour;
    private string _bitter;

    private string _crispy;
    private string _fluffy;
    private string _smooth;
    private string _hardness;
    private string _jiggly;
    private string _chewy;

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

    public int check_counter;
    public int Pitem_or_Origin; //プレイヤーアイテムか、オリジナルアイテムかの判定


    // Use this for initialization
    void Start()
    {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

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
        item_Crispy = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemCrispyScore").gameObject.GetComponent<Text>(); //さくさくの値
        item_Fluffy = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemFluffyScore").gameObject.GetComponent<Text>(); //ふわふわの値
        item_Smooth = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemSmoothScore").gameObject.GetComponent<Text>(); //口溶けの値
        item_Hardness = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemHardnessScore").gameObject.GetComponent<Text>(); //歯ごたえの値
        item_Jiggly = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemJigglyScore").gameObject.GetComponent<Text>(); //ぷるぷるの値
        item_Chewy = this.transform.Find("Card_Param_window/Card_Parameter/Card_Param_Window/ItemChewyScore").gameObject.GetComponent<Text>(); //ぐみぐみの値

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
        

        Card_param_obj.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Card_draw(); 
    }

    void Card_draw()
    {
        
        switch (Pitem_or_Origin)
        {
            case 0: //プレイヤーアイテムリストを選択した場合
                //Debug.Log("プレイヤーアイテムリスト　check_counter:" + check_counter);

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


    //カード描画部分
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
                _rich = database.items[check_counter].Rich.ToString();
                _sweat = database.items[check_counter].Sweat.ToString();
                _bitter = database.items[check_counter].Bitter.ToString();
                _sour = database.items[check_counter].Sour.ToString();              

                _crispy = database.items[check_counter].Crispy.ToString();
                _fluffy = database.items[check_counter].Fluffy.ToString();
                _smooth = database.items[check_counter].Smooth.ToString();
                _hardness = database.items[check_counter].Hardness.ToString();
                _jiggly = database.items[check_counter].Jiggly.ToString();
                _chewy = database.items[check_counter].Chewy.ToString();

                //数値として代入
                _quality_score = database.items[check_counter].Quality;
                _rich_score = database.items[check_counter].Rich;
                _sweat_score = database.items[check_counter].Sweat;
                _bitter_score = database.items[check_counter].Bitter;
                _sour_score = database.items[check_counter].Sour;

                _crispy_score = database.items[check_counter].Crispy;
                _fluffy_score = database.items[check_counter].Fluffy;
                _smooth_score = database.items[check_counter].Smooth;
                _hardness_score = database.items[check_counter].Hardness;


                for (i=0; i<_slot.Length; i++)
                {
                    _slot[i] = database.items[check_counter].toppingtype[i].ToString();
                }

                
                //カード正式名称（ついてるスロット名も含めた名前）

                for (i = 0; i < _slot.Length; i++)
                {
                    count = 0;

                    //スロット名を日本語に変換。DBから変換。Nonは、空白になる。
                    while ( count < slotnamedatabase.slotname_lists.Count)
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
                _rich = pitemlist.player_originalitemlist[check_counter].Rich.ToString();
                _sweat = pitemlist.player_originalitemlist[check_counter].Sweat.ToString();
                _bitter = pitemlist.player_originalitemlist[check_counter].Bitter.ToString();
                _sour = pitemlist.player_originalitemlist[check_counter].Sour.ToString();               

                _crispy = pitemlist.player_originalitemlist[check_counter].Crispy.ToString();
                _fluffy = pitemlist.player_originalitemlist[check_counter].Fluffy.ToString();
                _smooth = pitemlist.player_originalitemlist[check_counter].Smooth.ToString();
                _hardness = pitemlist.player_originalitemlist[check_counter].Hardness.ToString();
                _jiggly = pitemlist.player_originalitemlist[check_counter].Jiggly.ToString();
                _chewy = pitemlist.player_originalitemlist[check_counter].Chewy.ToString();


                //数値として代入
                _quality_score = pitemlist.player_originalitemlist[check_counter].Quality;
                _rich_score = pitemlist.player_originalitemlist[check_counter].Rich;
                _sweat_score = pitemlist.player_originalitemlist[check_counter].Sweat;
                _bitter_score = pitemlist.player_originalitemlist[check_counter].Bitter;
                _sour_score = pitemlist.player_originalitemlist[check_counter].Sour;

                _crispy_score = pitemlist.player_originalitemlist[check_counter].Crispy;
                _fluffy_score = pitemlist.player_originalitemlist[check_counter].Fluffy;
                _smooth_score = pitemlist.player_originalitemlist[check_counter].Smooth;
                _hardness_score = pitemlist.player_originalitemlist[check_counter].Hardness;



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

        // texture2dを使い、Spriteを作って、反映させる
        item_Icon.sprite = Sprite.Create(texture2d,
                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                   Vector2.zero);

        item_screen.sprite = Sprite.Create(card_template_1,
                                   new Rect(0, 0, card_template_1.width, card_template_1.height),
                                   Vector2.zero);


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
                category = "くすり";
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
            default:
                // 処理３　指定がなかった場合
                subcategory = "";
                break;
        }

        //最終的なテキストを表示 "\n"で改行
        item_Category.text = category + " - " + subcategory;

        /* カテゴリーここまで */

        //甘さ・苦さ・酸味の表示
        item_Rich.text = _rich;
        item_Sweat.text = _sweat;
        item_Bitter.text = _bitter;
        item_Sour.text = _sour;

        item_Crispy.text = _crispy;
        item_Fluffy.text = _fluffy;
        item_Smooth.text = _smooth;
        item_Hardness.text = _hardness;
        item_Jiggly.text = _jiggly;
        item_Chewy.text = _chewy;


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



        //スロット名の表示

        for (i = 0; i < _slot.Length; i++)
        {
            if ( _slot[i] == "Non" ) //Nonは空白表示。
            {
                _slot[i] = "";
            }
       
            item_Slot[i].text = _slotHyouji1[i]; //スロット表示１のほうが、スロットに表示する用のテキスト。スロット表示２は、アイテムのフルネームのほう。
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
            //スロット名+アイテム名の表示
            item_Name_Full.text = _slotHyouji2[0] + _slotHyouji2[1] + _slotHyouji2[2] + _slotHyouji2[3] + _slotHyouji2[4] + _slotHyouji2[5] + _slotHyouji2[6] + _slotHyouji2[7] + _slotHyouji2[8] + _slotHyouji2[9] + item_Name.text;
            item_Name.text = item_Name_Full.text; //お菓子
            //Card_param_obj.SetActive(true);
        }
        else
        {
            //Card_param_obj.SetActive(true);
        }


    }

    public void CompoundResult_DestroySelf()
    {
        //レベルアップチェック用オブジェクトの取得
        exp_table = GameObject.FindWithTag("ExpTable").GetComponent<ExpTable>();

        if (exp_table.check_on == true)
        {
            //レベルチェック中は、カードを消せないようにする。
        }
        else
        {
            compound_Main_obj = GameObject.FindWithTag("Compound_Main");
            compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

            compound_Main.compound_status = 0;
            Destroy(this.gameObject);
        }
    }
}