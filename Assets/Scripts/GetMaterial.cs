using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GetMaterial : MonoBehaviour
{

    private GameObject canvas;

    private GameObject text_area;
    private Text _text;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private SoundController sc;
    private TimeController time_controller;

    private GameObject tansaku_panel;

    private GameObject getmatplace_panel_obj;
    private GetMatPlace_Panel getmatplace_panel;

    private PlayerItemList pitemlist;

    private Buf_Power_Keisan bufpower_keisan;

    private ItemDataBase database;

    private ItemMatPlaceDataBase matplace_database;

    private GameObject TansakuLoding_Panel;

    // アイテムのデータを保持する辞書
    Dictionary<int, string> itemInfo;
    Dictionary<int, string> itemrareInfo;

    // 材料をドロップするアイテムの辞書
    Dictionary<int, float> itemDropDict;
    Dictionary<int, float> itemrareDropDict;

    // ドロップする個数の辞書
    Dictionary<int, float> itemDropKosuDict;
    Dictionary<int, float> itemrareDropKosuDict;

    //イベント発生か、アイテム取得の確率パネル
    Dictionary<int, float> eventDict;

    //宝箱のデータを保持する辞書
    Dictionary<int, string> treasureInfo;
    Dictionary<int, float> treasureDropDict;

    private float randomPoint;
    private float rare_event_kakuritsu;

    private int itemId, itemKosu;
    private string itemName;

    private int event_num;

    private int player_girl_findpower_final;
    private int _buf_findpower;

    private int random;
    private int i, count, empty;
    private int index;
    private int _itemid;
    private int _getMoney;

    private int cullent_total_mat;

    private string[] _a = new string[3];
    private string[] _a_final = new string[3];
    private string _a_zairyomax;

    private string[] _b = new string[3];
    private string[] _b_final = new string[3];

    private int[] kettei_item = new int[3];
    private int[] kettei_kosu = new int[3];

    private int mat_cost;
    private string mat_place;
    private float total;

    private bool mat_anim_on;
    private bool mat_anim_end;
    private int mat_anim_status;
    private float timeOut;

    private GameObject NextButton_obj;
    private GameObject OpenTreasureButton_obj;
    private Text treasure_text;
    private GameObject TreasureImage_obj;
    private GameObject CharacterSDImage;

    public int Treasure_Status; //宝箱なのか、怪しい場所を散策なのかを判別する
    private Image _TreasureImg;
    private Sprite treasure1;
    private Sprite treasure1Open;

    private GameObject TreasureGetitem_obj;
    private Image TreasureGetitem_img;

    private GameObject HeroineLifePanel;
    private Text HeroineLifeText;

    // Use this for initialization
    void Start()
    {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        //時間管理オブジェクトの取得
        time_controller = canvas.transform.Find("MainUIPanel/TimePanel").GetComponent<TimeController>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = canvas.transform.Find("MainUIPanel/MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        //材料採取地パネルの取得
        getmatplace_panel_obj = canvas.transform.Find("GetMatPlace_Panel").gameObject;
        getmatplace_panel = getmatplace_panel_obj.GetComponent<GetMatPlace_Panel>();

        //ヒロインライフパネル
        HeroineLifePanel = getmatplace_panel_obj.transform.Find("Comp/HeroineLife").gameObject;
        HeroineLifeText = HeroineLifePanel.transform.Find("HPguage/HPparam").GetComponent<Text>();

        //材料採取のための、消費コスト
        mat_cost = 0;

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //宝箱画像       
        treasure1 = Resources.Load<Sprite>("Sprites/Items/" + "treasureBox");
        treasure1Open = Resources.Load<Sprite>("Sprites/Items/" + "treasureBoxOpen");        

        mat_anim_status = 0;
        mat_anim_on = false;
        mat_anim_end = false;

        cullent_total_mat = 0;

        TansakuLoding_Panel = canvas.transform.Find("GetMatPlace_Panel/Comp/Slot_View/TansakuLodingPanel").gameObject;

        tansaku_panel = canvas.transform.Find("GetMatPlace_Panel/Comp/Slot_View/Tansaku_panel").gameObject;

        NextButton_obj = tansaku_panel.transform.Find("TansakuActionList/Viewport/Content/Next_tansaku").gameObject;
        OpenTreasureButton_obj = tansaku_panel.transform.Find("TansakuActionList/Viewport/Content/Open_treasure").gameObject;
        treasure_text = OpenTreasureButton_obj.transform.Find("Text").GetComponent<Text>();

        TreasureGetitem_obj = getmatplace_panel_obj.transform.Find("Comp/Slot_View/Image/TreasureGetImage").gameObject;
        TreasureGetitem_img = TreasureGetitem_obj.transform.Find("ItemImage").GetComponent<Image>(); //アイテムの画像データ

        TreasureImage_obj = getmatplace_panel_obj.transform.Find("Comp/Slot_View/Image/TreasureImage").gameObject;
        CharacterSDImage = getmatplace_panel_obj.transform.Find("Comp/Slot_View/Image/CharacterSD").gameObject;

        _TreasureImg = TreasureImage_obj.GetComponent<Image>(); //アイテムの画像データ
        Treasure_Status = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (mat_anim_on == true)
        {
            switch (mat_anim_status)
            {
                case 0: //初期化 状態１

                    //音を鳴らす
                    sc.PlaySe(24);

                    NextButton_obj.SetActive(false);
                    OpenTreasureButton_obj.SetActive(false);
                    TreasureGetitem_obj.SetActive(false);

                    timeOut = 1.0f;
                    mat_anim_status = 1;

                    _text.text = "探索中 .";
                    break;

                case 1: // 状態2

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.0f;
                        mat_anim_status = 2;

                        _text.text = "探索中 . .";
                    }
                    break;

                case 2:

                    if (timeOut <= 0.0)
                    {
                        timeOut = 2.0f;
                        mat_anim_status = 3;

                    }
                    break;

                case 3: //アニメ終了。判定する

                    //黒で画面消えてる最中に表示を切り替え
                    TreasureImage_obj.SetActive(false);
                    CharacterSDImage.SetActive(true);

                    mat_anim_on = false;
                    mat_anim_end = true;
                    mat_anim_status = 0;
                    
                    break;

                default:
                    break;
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }

    }



    public void GetRandomMaterials(int _index) //材料を３つランダムでゲットする処理
    {
        
        index = _index; //採取地IDの決定

        // 入手できるアイテムのデータベース
        InitializeDicts();

        mat_cost = matplace_database.matplace_lists[index].placeCost;
        mat_place = matplace_database.matplace_lists[index].placeName;

        //妹の体力がないと、先へ進めない。井戸や近くの森は、ハートがなくても採れる。
        if (PlayerStatus.girl1_Love_exp <= 0 && matplace_database.matplace_lists[index].placeType != 0)
        {
            _text.text = "にいちゃん。足が痛くてもう動けないよ～・・。" + "\n" + "（ハートが０になったので、動けないようだ。）";
        }
        else
        {
            //お金のチェック       
            if (PlayerStatus.player_money < mat_cost)
            {
                _text.text = "にいちゃん。お金が足りないよ～・・。";
            }
            else
            {
                //カゴの大きさのチェック。取った数の総量がMAXを超えると、これ以上取れない。
                if (PlayerStatus.player_zairyobox >= cullent_total_mat)
                {

                    //お金の消費
                    moneyStatus_Controller.UseMoney(mat_cost);

                    //日数の経過
                    PlayerStatus.player_time += 3; //場所に関係なく、一回とるごとに30分
                    time_controller.TimeKoushin();
                   
                    //妹の体力消費 一回の行動で1減る。0で倒れる。井戸などでは、減らない。
                    if (matplace_database.matplace_lists[index].placeType != 0)
                    {
                        PlayerStatus.girl1_Love_exp -= 1;
                        HeroineLifeText.text = PlayerStatus.girl1_Love_exp.ToString();
                    }

                    //プレイヤーのアイテム発見力をバフつきで計算
                    _buf_findpower = bufpower_keisan.Buf_findpower_Keisan(); //プレイヤー装備品計算
                    player_girl_findpower_final = PlayerStatus.player_girl_findpower + _buf_findpower;

                    //レアイベントの発生確率。アイテム発見力が上がることで、上昇する。
                    rare_event_kakuritsu = (player_girl_findpower_final - PlayerStatus.player_girl_findpower_def) * 0.3f;

                    //ウェイトアニメ
                    mat_anim_on = true;
                    mat_anim_end = false;

                    //画面を黒くする。
                    TansakuLoding_Panel.GetComponent<CanvasGroup>().DOFade(1, 0.2f); //背景黒フェード

                    tansaku_panel.SetActive(false);
                    StartCoroutine("Mat_Judge_anim_co");

                }
                else
                {
                    _text.text = "もうカゴがいっぱいだよ～。";
                }

            }
        }
    }

    IEnumerator Mat_Judge_anim_co()
    {
        while (mat_anim_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        tansaku_panel.SetActive(true);
        TansakuLoding_Panel.GetComponent<CanvasGroup>().DOFade(0, 0.2f); //背景黒フェード


        //イベント発生orアイテム取得　の抽選
        InitializeEventDicts();
        event_num = ChooseEvent(); //eventDictから算出

        switch (event_num)
        {
            case 0: //アイテム取得

                //アイテムの取得
                mat_result();
                break;

            case 1: //イベント発生

                switch (mat_place)
                {
                    case "Forest":

                        //イベント１
                        event_Forest();
                        break;

                    case "BirdSanctuali":

                        event_BirdSanctuali();
                        break;

                    case "BerryFarm":

                        event_BerryFarm();
                        break;

                    case "Lavender_field":

                        event_LavenderField();
                        break;

                    case "HimawariHill":

                        event_HimawariHill();
                        break;

                    default:

                        //イベント１
                        event_Forest();
                        break;
                }

                break;

            case 2: //発見力があがると、見つけやすくなるレアイベント

                switch (mat_place)
                {
                    case "Forest":

                        //レアイベント
                        rare_event_Forest();
                        break;


                    default:

                        //アイテムの取得
                        mat_result();
                        break;
                }

                break;

            case 3: //お宝を発見

                switch (mat_place)
                {
                    case "Forest":

                        //イベント１
                        treasure_Check();
                        break;

                    case "BirdSanctuali":

                        treasure_Check2();
                        break;

                    case "HimawariHill":

                        treasure_no();
                        break;

                    default:

                        //イベント１
                        treasure_no();
                        break;
                }
                
                break;

            default:

                //アイテムの取得
                mat_result();
                break;
        }

    }

    void mat_result()
    {
        for (count = 0; count < 3; count++) //3回繰り返す
        {
            // ドロップアイテムの抽選
            itemId = Choose();
            itemName = itemInfo[itemId];

            //  個数の抽選
            itemKosu = ChooseKosu();
            kettei_kosu[count] = itemKosu;


            if (itemName == "Non" || itemName == "なし") //Nonかなし、の場合は何も手に入らない。Nonの確率は0%
            {
                _a[count] = "";
                kettei_kosu[count] = 0;
            }
            else
            {

                //itemNameをもとに、アイテムデータベースのアイテムIDを取得
                i = 0;

                while (i < database.items.Count)
                {
                    if (database.items[i].itemName == itemName)
                    {
                        kettei_item[count] = i; //一致したときのiが、DBのitemIDのこと
                        break;
                    }
                    ++i;
                }

                cullent_total_mat += kettei_kosu[count]; //現在拾った材料の数

                _a[count] = database.items[kettei_item[count]].itemNameHyouji + " を" + kettei_kosu[count] + "個　手に入れた！";

                //アイテムの取得処理
                pitemlist.addPlayerItem(kettei_item[count], kettei_kosu[count]);

                //取得したアイテムをリストに入れ、あとでリザルト画面で表示
                getmatplace_panel.result_items[kettei_item[count]] += kettei_kosu[count];
            }
        }

        //通常アイテムとは別に、レアアイテムのドロップも抽選する。
        for (count = 0; count < 1; count++) //1回繰り返す
        {
            // レアドロップアイテムの抽選
            itemId = rareChoose();
            itemName = itemrareInfo[itemId];

            //  個数の抽選
            itemKosu = ChooserareKosu();
            kettei_kosu[count] = itemKosu;

            //Debug.Log("レアアイテムの抽選 ダイスの目: " + randomPoint + " 結果 itemID:" + itemId + " itemName: " + itemName);

            if (itemName == "Non" || itemName == "なし") //Nonかなし、の場合は何も手に入らない。Nonの確率は0%
            {
                _b[count] = "";
                kettei_kosu[count] = 0;
            }
            else
            {

                //itemNameをもとに、アイテムデータベースのアイテムIDを取得
                i = 0;

                while (i < database.items.Count)
                {
                    if (database.items[i].itemName == itemName)
                    {
                        kettei_item[count] = i; //一致したときのiが、DBのitemIDのこと
                        break;
                    }
                    ++i;
                }

                cullent_total_mat += kettei_kosu[count]; //現在拾った材料の数

                _b[count] = "\n" + "<color=#E37BB5>" + database.items[kettei_item[count]].itemNameHyouji + "</color>" + " を" + kettei_kosu[count] + "個　手に入れた！";

                //アイテムの取得処理
                pitemlist.addPlayerItem(kettei_item[count], kettei_kosu[count]);

                //取得したアイテムをリストに入れ、あとでリザルト画面で表示
                getmatplace_panel.result_items[kettei_item[count]] += kettei_kosu[count];
            }
        }




        //テキストに結果反映

        //まず初期化
        count = 0;
        empty = 0;

        for (i = 0; i < _a_final.Length; i++)
        {
            _a_final[i] = "";
            _b_final[i] = "";
        }

        //空白は無視するように調整
        for (i = 0; i < _a.Length; i++)
        {
            if (_a[i] == "") //空白は無視
            {
                empty++;
            }
            else
            {
                if (count == 0)
                {
                    _a_final[count] = _a[i];
                }
                else
                {
                    _a_final[count] = "\n" + _a[i];
                }
                count++;
            }
        }

        if (_a_final.Length == empty && _b[0] == "") //何もなかったとき
        {
            _text.text = "特に何も見つからなかった。";

            //音を鳴らす
            sc.PlaySe(6);
        }
        else //何か一つでもアイテムを見つけた
        {

            if (PlayerStatus.player_zairyobox >= cullent_total_mat)
            {
                _a_zairyomax = "";
            }
            else
            {
                _a_zairyomax = "\n" + "もうカゴがいっぱい。";
                getmatplace_panel.SisterOn1();
            }

            if (_a_final.Length == empty)
            {
                _text.text = _b[0] + _a_zairyomax;
            }
            else
            {
                _text.text = _a_final[0] + _a_final[1] + _a_final[2] + _b[0] + _a_zairyomax;
            }


            //音を鳴らす
            sc.PlaySe(9);
        }

    }


    void InitializeDicts()
    {
        //通常アイテム
        itemInfo = new Dictionary<int, string>();

        itemInfo.Add(0, matplace_database.matplace_lists[index].dropItem1); //アイテムデータベースに登録されているアイテム名と同じにする
        itemInfo.Add(1, matplace_database.matplace_lists[index].dropItem2);
        itemInfo.Add(2, matplace_database.matplace_lists[index].dropItem3);
        itemInfo.Add(3, matplace_database.matplace_lists[index].dropItem4);
        itemInfo.Add(4, matplace_database.matplace_lists[index].dropItem5);
        itemInfo.Add(5, matplace_database.matplace_lists[index].dropItem6);
        itemInfo.Add(6, matplace_database.matplace_lists[index].dropItem7);
        itemInfo.Add(7, matplace_database.matplace_lists[index].dropItem8);
        itemInfo.Add(8, matplace_database.matplace_lists[index].dropItem9);
        itemInfo.Add(9, matplace_database.matplace_lists[index].dropItem10);

        itemDropDict = new Dictionary<int, float>();
        itemDropDict.Add(0, matplace_database.matplace_lists[index].dropProb1); //こっちは確率テーブル
        itemDropDict.Add(1, matplace_database.matplace_lists[index].dropProb2);
        itemDropDict.Add(2, matplace_database.matplace_lists[index].dropProb3);
        itemDropDict.Add(3, matplace_database.matplace_lists[index].dropProb4);
        itemDropDict.Add(4, matplace_database.matplace_lists[index].dropProb5);
        itemDropDict.Add(5, matplace_database.matplace_lists[index].dropProb6);
        itemDropDict.Add(6, matplace_database.matplace_lists[index].dropProb7);
        itemDropDict.Add(7, matplace_database.matplace_lists[index].dropProb8);
        itemDropDict.Add(8, matplace_database.matplace_lists[index].dropProb9);
        itemDropDict.Add(9, matplace_database.matplace_lists[index].dropProb10);

        itemDropKosuDict = new Dictionary<int, float>();
        itemDropKosuDict.Add(1, 75.0f); //1個　75%
        itemDropKosuDict.Add(2, 25.0f); //2個　25%
        itemDropKosuDict.Add(3, 0.0f); //3個　15%


        //レア関係
        itemrareInfo = new Dictionary<int, string>();
        itemrareInfo.Add(0, matplace_database.matplace_lists[index].dropRare1);
        itemrareInfo.Add(1, matplace_database.matplace_lists[index].dropRare2);
        itemrareInfo.Add(2, matplace_database.matplace_lists[index].dropRare3);

        itemrareDropDict = new Dictionary<int, float>();
        itemrareDropDict.Add(0, matplace_database.matplace_lists[index].dropRareProb1);
        itemrareDropDict.Add(1, matplace_database.matplace_lists[index].dropRareProb2);
        itemrareDropDict.Add(2, matplace_database.matplace_lists[index].dropRareProb3);

        itemrareDropKosuDict = new Dictionary<int, float>();
        itemrareDropKosuDict.Add(1, 95.0f); //1個
        itemrareDropKosuDict.Add(2, 5.0f); //2個
        itemrareDropKosuDict.Add(3, 0.0f); //3個


    }



    //
    //★★★マップイベントの設定まとめ★★★
    //

    //近くの森
    void event_Forest()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                _text.text = "にいちゃん。だんごむし、みつけた～！";
                break;

            case 1:

                _text.text = "わ～い。ちょうちょ～～。（妹はサボっている。）";
                break;

            case 2:

                _text.text = "にいちゃん。腹へった～。" + "\n" + "妹は帰りたそうにしている。";
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                _text.text = "にいちゃん！！　とんぼが飛んでる～！！";
                break;

            default:

                _text.text = "ギャーー！ムカデ！！にいちゃん！！";

                //音を鳴らす
                sc.PlaySe(6);


                break;
        }
    }

    void rare_event_Forest()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                event_itemGet02();
                break;

            case 3:

                event_itemGet01();
                break;

            default:

                if (player_girl_findpower_final >= 120 && !GameMgr.MapEvent_06[0])                 
                {
                    //バードサンクチュアリを発見
                    _text.text = "にいちゃん！！ なんか抜け道があるよ？";
                    getmatplace_panel.next_flag = 100;
                    NextButton_obj.SetActive(true);
                }
                else
                {
                    event_itemGet01();                   
                }

                break;
        }
    }

    //バードサンクチュアリ
    void event_BirdSanctuali()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                _text.text = "にいちゃん！　とりさん、ふわふわ～！";
                break;

            case 1:

                _text.text = "どんぐり.. ないかな。（妹はサボっている。）";
                break;

            case 2:

                _text.text = "にいちゃん。お花畑きもちいい・・。" + "\n" + "妹は寝ている。";
                break;

            case 3:

                event_itemGet01();
                break;

            default:

                if (player_girl_findpower_final >= 150) //player_girl_findpowerは、girl_status内でパラメータ処理
                {
                    _text.text = "にいちゃん！！　ここに石像があるよ？";
                }
                else
                {
                    _text.text = "はっぱがキラキラしてる！！にいちゃん！！";

                    //音を鳴らす
                    //sc.PlaySe(6);
                }

                break;
        }
    }

    //ベリーファーム
    void event_BerryFarm()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                _text.text = "にいちゃん。この実、食べられるのかなぁ～？";
                break;

            case 1:

                _text.text = "えへへ..。赤いどんぐりないかな。";
                break;

            case 2:

                _text.text = "あ！てんとうむしだ！　にいちゃん！" + "\n" + "妹は、はしゃいでいる。";
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                _text.text = "赤い実と黒い実。黒いやつはすっぱいんだよね..。" + "\n" + "妹は、熱中している。";
                break;

            default:

                _text.text = "ギャッ！　草のとげがささった！！　いだいよ～・・。";

                //音を鳴らす
                sc.PlaySe(6);


                break;
        }
    }

    //ラベンダー畑
    void event_LavenderField()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                _text.text = "にいちゃん。水がいっぱいで、すっごくひろ～いね！";
                break;

            case 1:

                _text.text = "さやさや..。ぽかぽか。" + "\n" + "風がきもちいいねぇ。にいちゃん。";
                break;

            case 2:

                _text.text = "この紫のお花、ラベンダーっていうの？　いい香り～。";
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                _text.text = "お花がつぶれちゃうから、この草に寝ようね！にいちゃん。" + "\n" + "妹は、ごろごろしている。";
                break;

            default:

                _text.text = "ギャーー！どろんこの地面に足がはいっちゃった..！　どろどろ～・・。";

                //音を鳴らす
                sc.PlaySe(6);


                break;
        }
    }

    //ひまわりの丘
    void event_HimawariHill()
    {
        if (!GameMgr.MapEvent_04[1])
        {
            GameMgr.MapEvent_04[1] = true;

            _text.text = "兄ちゃん！！ なんかここに、建物があるよ？" + "\n" + GameMgr.ColorYellow + "絞り器" + "</color>" + "をみつけた！";

            //アイテムの取得処理
            pitemlist.addPlayerItemString("oil_extracter", 1);

            //取得したアイテムをリストに入れ、あとでリザルト画面で表示
            _itemid = pitemlist.SearchItemString("oil_extracter");
            getmatplace_panel.result_items[_itemid] += 1;

            //音を鳴らす
            sc.PlaySe(1);
        }
        else
        {
            _text.text = "廃屋がある。";
        }
    }


    void event_itemGet01()
    {
        _text.text = "にいちゃん。みてみて！　キラキラな石！" +
                        "\n" + GameMgr.ColorYellow + database.items[database.SearchItemIDString("kirakira_stone1")].itemNameHyouji + "</color>" + "をみつけた！";

        //アイテムの取得処理
        pitemlist.addPlayerItemString("kirakira_stone1", 1);

        //取得したアイテムをリストに入れ、あとでリザルト画面で表示
        _itemid = pitemlist.SearchItemString("kirakira_stone1");
        getmatplace_panel.result_items[_itemid] += 1;

        //音を鳴らす
        sc.PlaySe(1);
    }

    void event_itemGet02()
    {
        _getMoney = Random.Range(0, 100);

        _text.text = "にいちゃん。お金ひろった！へへ。" +
                        "\n" + _getMoney + "</color>" + "ルピアをみつけた！";

        //所持金をプラス
        moneyStatus_Controller.GetMoney(_getMoney); //アニメつき  

        //音を鳴らす
        sc.PlaySe(1);
    }




    //イベントの発生確率をセット
    void InitializeEventDicts()
    {       

        switch (mat_place)
        {
            case "Forest":

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 70.0f); //採集
                eventDict.Add(1, 25.0f); //20%でイベント発生
                eventDict.Add(2, 0.0f + rare_event_kakuritsu); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + rare_event_kakuritsu); //お宝発見
                break;

            case "BirdSanctuali":

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 70.0f); //採集
                eventDict.Add(1, 25.0f); //20%でイベント発生
                eventDict.Add(2, 0.0f + rare_event_kakuritsu); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + rare_event_kakuritsu); //お宝発見
                break;

            case "HimawariHill":

                if (!GameMgr.MapEvent_04[1])
                {
                    eventDict = new Dictionary<int, float>();
                    eventDict.Add(0, 70.0f); //採集
                    eventDict.Add(1, 30.0f); //30%でイベント 廃屋をみつけるまでは、発生しやすくなる。
                    eventDict.Add(2, 0.0f + rare_event_kakuritsu); //発見力があがることで発生しやすくなるレアイベント
                }
                else
                {
                    eventDict = new Dictionary<int, float>();
                    eventDict.Add(0, 80.0f); //採集
                    eventDict.Add(1, 10.0f); //10%でイベント
                    eventDict.Add(2, 0.0f + rare_event_kakuritsu); //発見力があがることで発生しやすくなるレアイベント
                    eventDict.Add(3, 10.0f + rare_event_kakuritsu); //お宝発見
                }
                break;

            default:

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 70.0f); //採集
                eventDict.Add(1, 25.0f); //10%でイベント
                eventDict.Add(2, 0.0f + rare_event_kakuritsu); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + rare_event_kakuritsu); //お宝発見
                break;
        }
    }







    //
    //お宝関係
    //
    void treasure_Check()
    {
        //おたからを発見
        //sc.PlaySe(84);
        _text.text = "にいちゃん！！ なんかあやしい草むらがあるよ..？　しらべる？" + "\n" + "（ハートを" + GameMgr.ColorPink + "３つ" + "</color>" + "消費するよ。）";

        //_TreasureImg.sprite = treasure1;
        OpenTreasureButton_obj.SetActive(true);
        treasure_text.text = "調べる";
        TreasureImage_obj.SetActive(false);
        CharacterSDImage.SetActive(false);

        //Treasure_Status = 0; //0=宝箱
        Treasure_Status = 1;
    }

    void treasure_Check2()
    {
        //怪しげな場所
        //sc.PlaySe(84);
        _text.text = "にいちゃん！！ きれいなお花畑！　探検してみる？" + "\n" + "（ハートを" + GameMgr.ColorPink + "３つ" + "</color>" + "消費するよ。）";

        //_TreasureImg.sprite = treasure1;
        OpenTreasureButton_obj.SetActive(true);
        treasure_text.text = "探検";
        TreasureImage_obj.SetActive(false);
        CharacterSDImage.SetActive(false);

        Treasure_Status = 1;
    }

    void treasure_no()
    {
        _text.text = "とくに何もみつからなかった。";
    }

    //GetMatPlace_Panelから呼び出し
    public void GetTreasureBox(string _place)
    {
        _TreasureImg.sprite = treasure1Open;
        //TreasureImage_obj.SetActive(false);

        switch (_place)
        {
            case "Forest":

                Treasure_Forest();
                break;

            case "Lavender_field":

                Treasure_Forest();
                break;

            case "StrawberryGarden":

                Treasure_Forest();
                break;

            case "HimawariHill":

                Treasure_Forest();
                break;

            case "BirdSanctuali":

                Treasure_Forest();
                break;

            default:
                Treasure_Forest();
                break;
        }
    }

    void Treasure_Forest()
    {
        InitializeTreasureDicts(0); //中の番号で、どの宝箱かを指定する

        //何が当たるかな？
        // 宝箱アイテムの抽選。
        itemId = TreasureChoose();
        itemName = treasureInfo[itemId];

        if (itemName == "Non") //はずれ
        {
            _text.text = "にいちゃん..。なにもなかった～..。";
        }
        else
        {
            Treasure_GetItem();
        }
    }

    void Treasure_GetItem()
    {
        _text.text = "にいちゃん！　やったぁ！！" + "\n" +
            GameMgr.ColorYellow + database.items[database.SearchItemIDString(itemName)].itemNameHyouji + "</color>" + "をみつけた！";

        TreasureGetitem_obj.SetActive(true);
        TreasureGetitem_img.sprite = database.items[database.SearchItemIDString(itemName)].itemIcon_sprite;

        //アイテムの取得処理
        pitemlist.addPlayerItemString(itemName, 1);

        //取得したアイテムをリストに入れ、あとでリザルト画面で表示
        _itemid = pitemlist.SearchItemString(itemName);
        getmatplace_panel.result_items[_itemid] += 1;

        //音を鳴らす
        switch (Treasure_Status)
        {
            case 0: //宝箱

                sc.PlaySe(87); //あけた音
                sc.PlaySe(86); //獲得音

                break;

            case 1: //あやしい場所を探索

                sc.PlaySe(86); //獲得音

                break;


            default:

                break;
        }
        
    }

    //宝箱データのセッティング。せっかくなので、骨董品とか収集品とかレシピとか、コレクションアイテム中心にする。
    void InitializeTreasureDicts(int _treasure_num)
    {

        //まずは初期化
        treasureInfo = new Dictionary<int, string>();
        treasureDropDict = new Dictionary<int, float>();

        switch (_treasure_num)
        {
            case 0: //お宝セットテーブル１

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名　ItemDatabaseのitemNameと同じ名前にする。
                treasureInfo.Add(1, "doro_dango");
                treasureInfo.Add(2, "kirakira_stone1");
                treasureInfo.Add(3, "compass");
                treasureInfo.Add(4, "copper_coin");
                treasureInfo.Add(5, "star_bottle");
                treasureInfo.Add(6, "diamond_1");
               
                treasureDropDict.Add(0, 20.0f); //こっちは確率テーブル　はずれの場合はなにもなし。
                treasureDropDict.Add(1, 30.0f);
                treasureDropDict.Add(2, 20.0f);
                treasureDropDict.Add(3, 8.0f + rare_event_kakuritsu);
                treasureDropDict.Add(4, 8.0f + rare_event_kakuritsu);
                treasureDropDict.Add(5, 8.0f + rare_event_kakuritsu);
                treasureDropDict.Add(6, 6.0f + rare_event_kakuritsu);
                break;

            default:

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名
                treasureInfo.Add(1, "kirakira_stone1");
                treasureInfo.Add(2, "kirakira_stone1");
                treasureInfo.Add(3, "kirakira_stone1");
                treasureInfo.Add(4, "kirakira_stone1");

                treasureDropDict.Add(0, 50.0f); //こっちは確率テーブル
                treasureDropDict.Add(1, 20.0f);
                treasureDropDict.Add(2, 10.0f);
                treasureDropDict.Add(3, 10.0f);
                treasureDropDict.Add(4, 10.0f);
                break;
        }
        
    }

    int TreasureChoose()
    {
        // 確率の合計値を格納
        total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in treasureDropDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in treasureDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }

    int Choose()
    {
        // 確率の合計値を格納
        total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }

    int ChooseKosu()
    {
        // 確率の合計値を格納
        total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemDropKosuDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemDropKosuDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }

    int rareChoose()
    {
        // 確率の合計値を格納
        total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemrareDropDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemrareDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }

    int ChooserareKosu()
    {
        // 確率の合計値を格納
        total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemrareDropKosuDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemrareDropKosuDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }

    int ChooseEvent()
    {
        // 確率の合計値を格納
        total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in eventDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in eventDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }

    public void SetInit()
    {
        cullent_total_mat = 0;
        _a_zairyomax = "";
    }
}
