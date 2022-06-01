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
    private GameObject text_kaigyo_button;
    private GameObject text_kaigyo_buttonPanel;

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
    private float rare_event_kakuritsu_hosei;

    //private int rare_eventitem_max;

    private int itemId, itemKosu;
    private string itemName;

    private int event_num;

    private int player_girl_findpower_final;
    private int _buf_findpower;
    private int _findpower_girl_getmat, _findpower_girl_getmat_final;

    private int random, random_param;
    private int i, count, empty;
    private int index;
    private int _itemid;
    private int _getMoney;
    private int _prob;

    private int cullent_total_mat;

    private string[] _a = new string[3];
    private string[] _a_final = new string[3];
    private string _a_zairyomax;

    private string[] _b = new string[3];
    private string[] _b_final = new string[3];

    private int tansaku_count = 3;
    private int tansaku_gyou = 3; //テキストエリアに表示する行数
    private List<string> _tansaku_result_temp = new List<string>();
    private int[] kettei_item;
    private int[] kettei_kosu;

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

    public int Treasure_Status; //宝箱か、怪しい場所を散策か、といったタイプを判別する
    private Image _TreasureImg;
    private Sprite treasure1;
    private Sprite treasure1Open;

    private GameObject TreasureGetitem_obj;
    private Image TreasureGetitem_img;

    private GameObject HeroineLifePanel;
    private Text HeroineLifeText;

    private int page_count, lastpage_count;
    private int hyouji_max;

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
        text_kaigyo_button = canvas.transform.Find("MessageWindow/KaigyoButton").gameObject;
        text_kaigyo_buttonPanel = canvas.transform.Find("MessageWindow/KaigyoButtonPanel").gameObject;

        //時間管理オブジェクトの取得
        time_controller = canvas.transform.Find("MainUIPanel/Comp/TimePanel").GetComponent<TimeController>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = canvas.transform.Find("MainUIPanel/Comp/MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        //材料採取地パネルの取得
        getmatplace_panel_obj = canvas.transform.Find("GetMatPlace_Panel").gameObject;
        getmatplace_panel = getmatplace_panel_obj.GetComponent<GetMatPlace_Panel>();

        //ヒロインライフパネル
        HeroineLifePanel = canvas.transform.Find("MainUIPanel/Comp/GetMatStatusPanel/HeroineLife").gameObject;
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

        kettei_item = new int[99];
        kettei_kosu = new int[99];

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
                    text_kaigyo_button.SetActive(false);
                    text_kaigyo_buttonPanel.SetActive(false);

                    timeOut = 1.0f;
                    mat_anim_status = 1;

                    switch(mat_place)
                    {
                        case "Ido":
                            _text.text = "うんしょ .";
                            break;

                        default:
                            _text.text = "探索中 .";
                            break;
                    }
                    break;

                case 1: // 状態2

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.0f;
                        mat_anim_status = 2;

                        switch (mat_place)
                        {
                            case "Ido":
                                _text.text = "うんしょ . うんしょ . . ";
                                break;

                            default:
                                _text.text = "探索中 . .";
                                break;
                        }
                        
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
                    //CharacterSDImage.SetActive(true);

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
        ResetItemDicts();
        InitializeDicts(_index);

        mat_cost = matplace_database.matplace_lists[index].placeCost;
        mat_place = matplace_database.matplace_lists[index].placeName;

        //妹のハートがある程度ないと、先へ進めない。
        if (PlayerStatus.player_girl_lifepoint < matplace_database.matplace_lists[index].placeHP && matplace_database.matplace_lists[index].placeType != 0)
        {
            _text.text = "にいちゃん。足が痛くてもう動けないよ～・・。" + "\n" + "（これ以上は、動けないようだ。）";
        }
        else
        {
            //お金のチェック       
            /*if (PlayerStatus.player_money < mat_cost)
            {
                _text.text = "にいちゃん。お金が足りないよ～・・。";
            }
            else
            {*/
            //カゴの大きさのチェック。取った数の総量がMAXを超えると、これ以上取れない。
            if (PlayerStatus.player_zairyobox >= cullent_total_mat)
            {

                //お金の消費
                //moneyStatus_Controller.UseMoney(mat_cost);

                //日数の経過
                //PlayerStatus.player_time += 6; //場所に関係なく、一回とるごとに30分
                time_controller.SetMinuteToHour(6);
                time_controller.TimeKoushin();

                //妹の体力消費 一回の行動でマップに応じた量減る。
                if (matplace_database.matplace_lists[index].placeType != 0)
                {
                    GirlLifeDegKeisan(matplace_database.matplace_lists[index].placeHP);
                }

                //腹も減る
                if (GameMgr.Story_Mode != 0)
                {
                    if (GameMgr.outgirl_Nowprogress) { }
                    else
                    {
                        PlayerStatus.player_girl_manpuku -= 5;
                    }
                }

                //プレイヤーのアイテム発見力をバフつきで計算
                _buf_findpower = bufpower_keisan.Buf_findpower_Keisan(); //プレイヤー装備品計算
                player_girl_findpower_final = PlayerStatus.player_girl_findpower + _buf_findpower;

                //レアイベントの発生確率。アイテム発見力が上がることで、上昇する。
                rare_event_kakuritsu = (player_girl_findpower_final - PlayerStatus.player_girl_findpower_def) * 0.1f;
                if(rare_event_kakuritsu >= 50.0f)
                {
                    rare_event_kakuritsu = 50.0f;
                }

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
                if (GameMgr.outgirl_Nowprogress)
                {
                    _text.text = "もうカゴがいっぱいだ。";
                }
                else
                {
                    _text.text = "もうカゴがいっぱいだよ～。";
                }
            }

            //}
        }
    }

    //入れた数値分、体力を減らす処理
    void GirlLifeDegKeisan(int _deghp)
    {
        PlayerStatus.player_girl_lifepoint -= _deghp;

        if (PlayerStatus.player_girl_lifepoint <= 0) //体力の下限0
        {
            PlayerStatus.player_girl_lifepoint = 0;
        }
        HeroineLifeText.text = PlayerStatus.player_girl_lifepoint.ToString();

        //PlayerStatus.girl1_Love_exp -= matplace_database.matplace_lists[index].placeHP;
        //HeroineLifeText.text = PlayerStatus.girl1_Love_exp.ToString();
    }

    //入れた数値分、体力を上げる処理
    void GirlLifeUpKeisan(int _uphp)
    {
        PlayerStatus.player_girl_lifepoint += _uphp;

        if (PlayerStatus.player_girl_lifepoint >= 99) //体力の上限99
        {
            PlayerStatus.player_girl_lifepoint = 99;
        }
        HeroineLifeText.text = PlayerStatus.player_girl_lifepoint.ToString();

        //PlayerStatus.girl1_Love_exp -= matplace_database.matplace_lists[index].placeHP;
        //HeroineLifeText.text = PlayerStatus.girl1_Love_exp.ToString();
    }

    IEnumerator Mat_Judge_anim_co()
    {
        while (mat_anim_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }
       
        TansakuLoding_Panel.GetComponent<CanvasGroup>().DOFade(0, 0.2f); //背景黒フェード


        //イベント発生orアイテム取得　の抽選
        InitializeEventDicts(); //採集、イベント、レアイベント、宝箱の4種類から一つ
        event_num = ChooseEvent(); //eventDictから算出

        if(GameMgr.outgirl_Nowprogress) //妹が外出してていない場合。　イベントなしで採取のみ。
        {
            event_num = 0;
        }

        switch (event_num)
        {
            case 0: //アイテム取得

                //アイテムの取得
                mat_result();
                break;

            case 1: //イベント発生

                tansaku_panel.SetActive(true);
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

                    case "CatGrave":

                        event_CatGrave();
                        break;

                    default:

                        //イベント１
                        event_Forest();
                        break;
                }

                break;

            case 2: //レアイベント　発見力があがると、見つけやすくなる

                tansaku_panel.SetActive(true);
                switch (mat_place)
                {
                    case "Forest":

                        //レアイベント
                        rare_event_Forest(); //バードサンクチュアリ発見など
                        break;

                    case "BerryFarm":

                        //アイテムの取得
                        mat_result();
                        break;

                    case "Lavender_field":

                        rare_event_LavenderField(); //アイスの実の森発見など。現在は未実装。
                        break;

                    case "HimawariHill":

                        rare_event_HimawariHill(); //ただレアアイテムがたまにでる。
                        break;

                    default:

                        //アイテムの取得
                        mat_result();
                        break;
                }

                break;

            case 3: //お宝を発見

                tansaku_panel.SetActive(true);
                switch (mat_place)
                {
                    case "Forest":

                        //イベント１
                        treasure_Check();
                        break;

                    case "BirdSanctuali":

                        treasure_Check2();
                        break;

                    case "BerryFarm":

                        treasure_Check3();
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

                tansaku_panel.SetActive(true);
                //アイテムの取得
                mat_result();
                break;
        }

    }

    void mat_result()
    {
        _tansaku_result_temp.Clear();

        switch (mat_place)
        {
            case "Ido":

                //井戸は一回のみ
                ItemGetMethod(0);
                break;

            default:

                for (count = 0; count < tansaku_count; count++) //3回繰り返す
                {
                    ItemGetMethod(count);

                }
                break;
        }
        

        //通常アイテムとは別に、レアアイテムのドロップも抽選する。
        for (count = 0; count < 1; count++) //1回繰り返す
        {
            RareItemGetMethod(count);            
        }




        //テキストに結果反映

        //まず初期化
        count = 0;
        empty = 0;

        _text.text = "";
        page_count = 0;
        if (_tansaku_result_temp.Count > tansaku_gyou)
        {
            text_kaigyo_button.SetActive(true);
            text_kaigyo_buttonPanel.SetActive(true);
        }
        else
        {
            text_kaigyo_button.SetActive(false);
            text_kaigyo_buttonPanel.SetActive(false);
            tansaku_panel.SetActive(true);
        }

        //ラストページにきたら、探索パネルを表示する。を検出するメソッド
        lastpage_count = 0;
        while (i < 99)
        {

            if (_tansaku_result_temp.Count - (i * tansaku_gyou) < tansaku_gyou)
            {
                break;
            }
            lastpage_count++;
            i++;
        }
        //Debug.Log("最後のページ: " + lastpage_count);
        //Debug.Log("現在のページ: " + page_count);

        if (_tansaku_result_temp.Count == 0)
        {
            _text.text = "特に何も見つからなかった。";
            //音を鳴らす
            sc.PlaySe(6);
        }
        else //何か一つでもアイテムを見つけた
        {
            if (PlayerStatus.player_zairyobox >= cullent_total_mat)
            {
            }
            else
            {
                if (GameMgr.outgirl_Nowprogress)
                {
                    _tansaku_result_temp.Add("もうカゴがいっぱいだ。");
                }
                else
                {
                    _tansaku_result_temp.Add("もうカゴがいっぱい。");
                    getmatplace_panel.SisterOn1();
                }
            }

            textArea_Koushin();           

            //音を鳴らす
            sc.PlaySe(9);
        }

    }

    void ItemGetMethod(int _count)
    {
        // ドロップアイテムの抽選
        itemId = Choose();
        itemName = itemInfo[itemId];

        //  個数の抽選
        itemKosu = ChooseKosu();
        kettei_kosu[_count] = itemKosu;


        if (itemName == "Non" || itemName == "なし") //Nonかなし、の場合は何も手に入らない。Nonの確率は0%
        {
            kettei_kosu[_count] = 0;
        }
        else
        {

            //itemNameをもとに、アイテムデータベースのアイテムIDを取得
            i = 0;

            while (i < database.items.Count)
            {
                if (database.items[i].itemName == itemName)
                {
                    kettei_item[_count] = i; //一致したときのiが、DBのitemIDのこと
                    break;
                }
                ++i;
            }

            cullent_total_mat += kettei_kosu[_count]; //現在拾った材料の数

            _tansaku_result_temp.Add(GameMgr.ColorYellow + database.items[kettei_item[_count]].itemNameHyouji + "</color>" + " を" + kettei_kosu[_count] + "個　手に入れた！");

            //アイテムの取得処理
            pitemlist.addPlayerItem(database.items[kettei_item[_count]].itemName, kettei_kosu[_count]);

            //取得したアイテムをリストに入れ、あとでリザルト画面で表示
            getmatplace_panel.result_items[database.items[kettei_item[_count]].itemName] += kettei_kosu[_count];
        }
    }

    void RareItemGetMethod(int _count)
    {
        // レアドロップアイテムの抽選
        itemId = rareChoose();
        itemName = itemrareInfo[itemId];

        //  個数の抽選
        itemKosu = ChooserareKosu();
        kettei_kosu[_count] = itemKosu;

        //Debug.Log("レアアイテムの抽選 ダイスの目: " + randomPoint + " 結果 itemID:" + itemId + " itemName: " + itemName);

        if (itemName == "Non" || itemName == "なし") //Nonかなし、の場合は何も手に入らない。Nonの確率は0%
        {
            kettei_kosu[_count] = 0;
        }
        else
        {

            //itemNameをもとに、アイテムデータベースのアイテムIDを取得
            i = 0;

            while (i < database.items.Count)
            {
                if (database.items[i].itemName == itemName)
                {
                    kettei_item[_count] = i; //一致したときのiが、DBのitemIDのこと
                    break;
                }
                ++i;
            }

            cullent_total_mat += kettei_kosu[_count]; //現在拾った材料の数

            _tansaku_result_temp.Add("<color=#E37BB5>" + database.items[kettei_item[_count]].itemNameHyouji + "</color>" + " を" + kettei_kosu[_count] + "個　手に入れた！");

            //アイテムの取得処理
            pitemlist.addPlayerItem(database.items[kettei_item[_count]].itemName, kettei_kosu[_count]);

            //取得したアイテムをリストに入れ、あとでリザルト画面で表示
            getmatplace_panel.result_items[database.items[kettei_item[_count]].itemName] += kettei_kosu[_count];
        }
    }

    //採集アイテムが4個以上のとき、3行ずつ表示する。ボタンを押すと、次のページへ送り出す。
    public void KaigyoButton()
    {
        sc.PlaySe(30);

        page_count++;
        if (_tansaku_result_temp.Count - (page_count * tansaku_gyou) < 0)
        {
            page_count = 0;
        }
        textArea_Koushin();

        //現在のページが、ラストページならば、探索パネル表示
        if (page_count >= lastpage_count)
        {
            tansaku_panel.SetActive(true);
            //text_kaigyo_buttonPanel.SetActive(false);
        }
    }

    void textArea_Koushin()
    {
        _text.text = "";

        hyouji_max = tansaku_gyou;

        if(_tansaku_result_temp.Count - (page_count* tansaku_gyou) < tansaku_gyou)
        {
            hyouji_max = _tansaku_result_temp.Count - (page_count * tansaku_gyou);
        }

        for (i = 0; i < hyouji_max; i++)
        {
            if (i == 0)
            {
                _text.text += _tansaku_result_temp[i+ (page_count * tansaku_gyou)];
            }
            else
            {
                _text.text += "\n" + _tansaku_result_temp[i+ (page_count * tansaku_gyou)];
            }
        }
    }

    void ResetItemDicts()
    {
        itemInfo = new Dictionary<int, string>();
        itemDropDict = new Dictionary<int, float>();
        itemDropKosuDict = new Dictionary<int, float>();
        itemrareInfo = new Dictionary<int, string>();
        itemrareDropDict = new Dictionary<int, float>();
        itemrareDropKosuDict = new Dictionary<int, float>();
    }

    void InitializeDicts(int _index)
    {
        //通常アイテム       
        itemInfo.Add(0, matplace_database.matplace_lists[_index].dropItem1); //アイテムデータベースに登録されているアイテム名と同じにする
        itemInfo.Add(1, matplace_database.matplace_lists[_index].dropItem2);
        itemInfo.Add(2, matplace_database.matplace_lists[_index].dropItem3);
        itemInfo.Add(3, matplace_database.matplace_lists[_index].dropItem4);
        itemInfo.Add(4, matplace_database.matplace_lists[_index].dropItem5);
        itemInfo.Add(5, matplace_database.matplace_lists[_index].dropItem6);
        itemInfo.Add(6, matplace_database.matplace_lists[_index].dropItem7);
        itemInfo.Add(7, matplace_database.matplace_lists[_index].dropItem8);
        itemInfo.Add(8, matplace_database.matplace_lists[_index].dropItem9);
        itemInfo.Add(9, matplace_database.matplace_lists[_index].dropItem10);

        //こっちは入手確率テーブル
        itemDropDict.Add(0, matplace_database.matplace_lists[_index].dropProb1); 
        itemDropDict.Add(1, matplace_database.matplace_lists[_index].dropProb2);
        itemDropDict.Add(2, matplace_database.matplace_lists[_index].dropProb3);
        itemDropDict.Add(3, matplace_database.matplace_lists[_index].dropProb4);
        itemDropDict.Add(4, matplace_database.matplace_lists[_index].dropProb5);
        itemDropDict.Add(5, matplace_database.matplace_lists[_index].dropProb6);
        itemDropDict.Add(6, matplace_database.matplace_lists[_index].dropProb7);
        itemDropDict.Add(7, matplace_database.matplace_lists[_index].dropProb8);
        itemDropDict.Add(8, matplace_database.matplace_lists[_index].dropProb9);
        itemDropDict.Add(9, matplace_database.matplace_lists[_index].dropProb10);

        //個数
        itemDropKosuDict.Add(1, 75.0f); //1個　75%
        itemDropKosuDict.Add(2, 25.0f); //2個　25%
        itemDropKosuDict.Add(3, 0.0f); //3個　15%


        //レア関係
        
        itemrareInfo.Add(0, matplace_database.matplace_lists[_index].dropRare1);
        itemrareInfo.Add(1, matplace_database.matplace_lists[_index].dropRare2);
        itemrareInfo.Add(2, matplace_database.matplace_lists[_index].dropRare3);
        
        itemrareDropDict.Add(0, matplace_database.matplace_lists[_index].dropRareProb1);
        itemrareDropDict.Add(1, matplace_database.matplace_lists[_index].dropRareProb2);
        itemrareDropDict.Add(2, matplace_database.matplace_lists[_index].dropRareProb3);
       
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

    //バードサンクチュアリ
    void event_BirdSanctuali()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                random_param = Random.Range(5, 15);
                _text.text = "にいちゃん！　とりさん、ふわふわ～！" + "\n" + "ハートが " + GameMgr.ColorPink + random_param + " </color> " + "上がった！";
                PlayerStatus.girl1_Love_exp += random_param;
                sc.PlaySe(17);
                break;

            case 1:

                _text.text = "どんぐり.. ないかな。（妹はサボっている。）";
                break;

            case 2:

                _text.text = "にいちゃん。お花畑きもちいいね。すやぁ～。" + "\n" + "妹は寝ている。" + "体力を５回復。";
                GirlLifeUpKeisan(5);
                sc.PlaySe(17);
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                _text.text = "お花たちが、踊るように風で揺れている。";
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

                _text.text = "えへへ..。赤いどんぐりないかな。" + "\n" + "妹は、どんぐり探しに夢中のようだ。";
                break;

            case 2:

                random_param = Random.Range(1, 10);
                _text.text = "あ！てんとうむしだ！　にいちゃん！" + "\n" + "妹は、はしゃいでいる。ハートが " + GameMgr.ColorPink + random_param + "</color>" + "上がった！";
                PlayerStatus.girl1_Love_exp += random_param;
                sc.PlaySe(17);
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                _text.text = "赤と黒。くろいやつはすっぱいんだよね..。" + "\n" + "妹は、熱中している。";
                break;

            case 5:

                event_itemGet02(2);
                break;

            default:

                _text.text = "ギャッ！　草のとげがささった！！　いだいよ～・・。";

                //音を鳴らす
                sc.PlaySe(6);


                break;
        }
    }

    //アメジストの庭
    void event_LavenderField()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                random_param = Random.Range(3, 10);
                _text.text = "にいちゃん。水がいっぱい！　すっごくひろ～い。" + "\n" + "妹は感動しているようだ。ハートが " + GameMgr.ColorPink + random_param + "</color>" + "上がった！";
                PlayerStatus.girl1_Love_exp += random_param;
                sc.PlaySe(17);
                break;

            case 1:

                _text.text = "風がさやさや。" + "\n" + "きもちいいねぇ～・・。にいちゃん。";
                break;

            case 2:

                random_param = Random.Range(2, 6);
                _text.text = "この紫のお花、ラベンダーっていうの？　いい香り～。" + "\n" + "ハートが " + GameMgr.ColorPink + random_param + " </color> " + "上がった！";
                PlayerStatus.girl1_Love_exp += random_param;
                sc.PlaySe(17);
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                _text.text = "お花がつぶれちゃうから、草の上に寝ようね。　にいちゃん！" + "\n" + "妹は、気持ちよさそうにしている。";
                break;

            case 5:

                _text.text = "にいちゃん。ここ、色んなお花がいっぱ～い！";
                break;

            case 6:

                event_itemGet02(3);
                break;

            default:

                _text.text = "ギャーー！どろんこにはまっちゃった..！　どろどろ～。" + "\n" + "体力が１下がった。";
                GirlLifeDegKeisan(1);

                //音を鳴らす
                sc.PlaySe(6);


                break;
        }
    }
   

    //ひまわりの丘
    void event_HimawariHill()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                _text.text = "にいちゃん。ひまわり、すっごくきれい～！";
                break;

            case 1:

                event_itemGet01();
                break;

            case 2:

                _text.text = "なつかしい匂いがする。" + "ゆっくりと畑を歩いた。" + "体力を５回復。";
                GirlLifeUpKeisan(5);
                sc.PlaySe(17);
                break;

            default:

                _text.text = "にいちゃん。昔ままと、ここにきたことあるのかなぁ？";
                break;
        }

        /*
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
        }*/
    }

    //ねこのお墓
    void event_CatGrave()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                random_param = Random.Range(3, 10);
                _text.text = "木漏れ日があったかいね～。にいちゃん！" + "\n" + "妹は、ひなたぼっこをしている。ハートが " + GameMgr.ColorPink + random_param + "</color>" + "上がった！";
                PlayerStatus.girl1_Love_exp += random_param;
                sc.PlaySe(17);
                break;

            case 1:

                _text.text = "あ、にいちゃん！　あそこにくろねこ、いるよ～！" + "\n" + "妹は、目をキラキラさせながらじっと見ている。";
                break;

            case 2:

                random_param = Random.Range(2, 6);
                _text.text = "にいちゃん～。ねこさんのおはか、お参りしていこ～！" + "\n" + "ハートが " + GameMgr.ColorPink + random_param + " </color> " + "上がった！";
                PlayerStatus.girl1_Love_exp += random_param;
                sc.PlaySe(17);
                break;

            case 3:

                event_itemGet03();
                break;

            case 4:

                _text.text = "ねこ～♪　ねこ～♪" + "\n" + "妹は、ねこたちを追っかけまわしている。";
                break;

            case 5:

                random_param = Random.Range(2, 6);
                _text.text = "くん..。くん..。にいちゃん！　このりんご.. おいしいよ♪　シャリシャリ。" + "\n" + "最大体力が " + GameMgr.ColorPink + random_param + " </color> " + "上がった！";
                PlayerStatus.player_girl_maxlifepoint += random_param;
                break;

            case 6:

                event_itemGet04();
                break;

            default:

                _text.text = "うわ..！にいちゃん。ねこちゃんのフン、ふんじゃった..。" + "\n" + "体力が２下がった。";
                GirlLifeDegKeisan(2);

                //音を鳴らす
                sc.PlaySe(6);


                break;
        }
    }



    //レアイベント関係
    void rare_event_Forest()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                event_itemGet02(1);
                break;

            case 1:

                event_itemGet02(1);
                break;

            case 3:

                event_itemGet01();
                break;

            default:

                if (player_girl_findpower_final >= 120 && !GameMgr.MapEvent_06[0]) //バードサンクチュアリ見つけたらもう出ない。      
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

    void rare_event_LavenderField()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                event_itemGet02(3);
                break;

            case 1:

                event_itemGet02(3);
                break;

            case 3:

                event_itemGet03();
                break;

            default:

                event_itemGet01();

                /*if (player_girl_findpower_final >= 150 && !GameMgr.MapEvent_06[0]) 
                {
                    //アイスの実の森を発見
                    _text.text = "にいちゃん！！ なんか抜け道があるよ？";
                    getmatplace_panel.next_flag = 100;
                    NextButton_obj.SetActive(true);
                }
                else
                {
                    event_itemGet01();
                }*/

                break;
        }
    }
    void rare_event_HimawariHill()
    {
        random = Random.Range(0, 10);

        switch (random)
        {
            case 0:

                event_itemGet02(4);
                break;

            case 1:

                event_itemGet02(4);
                break;

            case 3:

                event_itemGet01();
                break;

            case 4:

                event_itemGet03();
                break;

            default:

                event_itemGet04();

                /*if (player_girl_findpower_final >= 150 && !GameMgr.MapEvent_06[0]) 
                {
                    //アイスの実の森を発見
                    _text.text = "にいちゃん！！ なんか抜け道があるよ？";
                    getmatplace_panel.next_flag = 100;
                    NextButton_obj.SetActive(true);
                }
                else
                {
                    event_itemGet01();
                }*/

                break;
        }
    }

    //
    //イベントリスト
    //
    void event_itemGet01()
    {
        _text.text = "にいちゃん。みてみて！　キラキラな石！" +
                        "\n" + GameMgr.ColorYellow + database.items[database.SearchItemIDString("kirakira_stone1")].itemNameHyouji + "</color>" + "をみつけた！";

        //アイテムの取得処理
        pitemlist.addPlayerItemString("kirakira_stone1", 1);

        //取得したアイテムをリストに入れ、あとでリザルト画面で表示
        //_itemid = pitemlist.SearchItemString("kirakira_stone1");
        getmatplace_panel.result_items["kirakira_stone1"] += 1;

        //音を鳴らす
        sc.PlaySe(1);
    }

    void event_itemGet02(int _place)
    {
        //少額か、大金塊か
        _prob = Random.Range(0, 100);

        if (_prob <= 5) //大金塊 5%
        {
            switch(_place)
            {
                case 1:
                    _getMoney = 500;
                    break;
                case 2:
                    _getMoney = 1000;
                    break;
                case 3:
                    _getMoney = 2000;
                    break;
                case 4:
                    _getMoney = 3000;
                    break;
            }
            //_getMoney = Random.Range(1000, 2000);

            _text.text = "にいちゃん。すごい！　金塊を見つけたよ！！" +
                            "\n" + GameMgr.ColorYellow + _getMoney + "</color>" + "ルピアをみつけた！";

            //所持金をプラス
            moneyStatus_Controller.GetMoney(_getMoney); //アニメつき  

            //音を鳴らす
            sc.PlaySe(1);
        }
        else
        {
            switch (_place)
            {
                case 1:
                    _getMoney = 10;
                    break;
                case 2:
                    _getMoney = 50;
                    break;
                case 3:
                    _getMoney = 100;
                    break;
                case 4:
                    _getMoney = 500;
                    break;
            }
            //_getMoney = Random.Range(0, 200);

            _text.text = "にいちゃん。お金ひろった！へへ。" +
                            "\n" + GameMgr.ColorYellow + _getMoney + "</color>" + "ルピアをみつけた！";

            //所持金をプラス
            moneyStatus_Controller.GetMoney(_getMoney); //アニメつき  

            //音を鳴らす
            sc.PlaySe(1);
        }
    }

    void event_itemGet03()
    {
        _text.text = "にいちゃん。みてみて！　テカテカしてる石！" +
                        "\n" + GameMgr.ColorYellow + database.items[database.SearchItemIDString("kirakira_stone2")].itemNameHyouji + "</color>" + "をみつけた！";

        //アイテムの取得処理
        pitemlist.addPlayerItemString("kirakira_stone2", 1);

        //取得したアイテムをリストに入れ、あとでリザルト画面で表示
        //_itemid = pitemlist.SearchItemString("kirakira_stone2");
        getmatplace_panel.result_items["kirakira_stone2"] += 1;

        //音を鳴らす
        sc.PlaySe(1);
    }

    void event_itemGet04()
    {
        _text.text = "にいちゃん。みてみて！　くるくる渦巻きの石！" +
                        "\n" + GameMgr.ColorYellow + database.items[database.SearchItemIDString("kirakira_stone3")].itemNameHyouji + "</color>" + "をみつけた！";

        //アイテムの取得処理
        pitemlist.addPlayerItemString("kirakira_stone3", 1);

        //取得したアイテムをリストに入れ、あとでリザルト画面で表示
        //_itemid = pitemlist.SearchItemString("kirakira_stone3");
        getmatplace_panel.result_items["kirakira_stone3"] += 1;

        //音を鳴らす
        sc.PlaySe(1);
    }


    //イベントの発生確率をセット
    void InitializeEventDicts()
    {
        if(rare_event_kakuritsu >= 15) //レアイベント発生確率などは、最大１５％ぐらいが上限。でないと採取が出にくくなるため。
        {
            rare_event_kakuritsu_hosei = 15;
        }
        else
        {
            rare_event_kakuritsu_hosei = rare_event_kakuritsu;
        }
        
        switch (mat_place)
        {
            case "Forest":

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 80.0f); //採集
                eventDict.Add(1, 15.0f); //20%でイベント発生
                eventDict.Add(2, 0.0f + rare_event_kakuritsu_hosei); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + rare_event_kakuritsu_hosei); //お宝発見
                break;

            case "BirdSanctuali":

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 80.0f); //採集
                eventDict.Add(1, 15.0f); //20%でイベント発生
                eventDict.Add(2, 0.0f + rare_event_kakuritsu_hosei); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + rare_event_kakuritsu_hosei); //お宝発見
                break;

            case "BerryFarm":

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 80.0f); //採集
                eventDict.Add(1, 15.0f); //20%でイベント発生
                eventDict.Add(2, 0.0f); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + (rare_event_kakuritsu_hosei * 1.5f)); //お宝発見
                break;

            case "HimawariHill":

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 80.0f); //採集
                eventDict.Add(1, 10.0f); //10%でイベント
                eventDict.Add(2, 10.0f + rare_event_kakuritsu_hosei); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 0.0f); //お宝発見

                /*
                if (!GameMgr.MapEvent_04[1])
                {
                    eventDict = new Dictionary<int, float>();
                    eventDict.Add(0, 70.0f); //採集
                    eventDict.Add(1, 30.0f); //30%でイベント 廃屋をみつけるまでは、発生しやすくなる。
                    eventDict.Add(2, 0.0f + rare_event_kakuritsu_hosei); //発見力があがることで発生しやすくなるレアイベント
                }
                else
                {
                    eventDict = new Dictionary<int, float>();
                    eventDict.Add(0, 80.0f); //採集
                    eventDict.Add(1, 10.0f); //10%でイベント
                    eventDict.Add(2, 0.0f + rare_event_kakuritsu_hosei); //発見力があがることで発生しやすくなるレアイベント
                    eventDict.Add(3, 10.0f + rare_event_kakuritsu_hosei); //お宝発見
                }*/
                break;

            default:

                eventDict = new Dictionary<int, float>();
                eventDict.Add(0, 80.0f); //採集
                eventDict.Add(1, 15.0f); //10%でイベント
                eventDict.Add(2, 0.0f + rare_event_kakuritsu_hosei); //発見力があがることで発生しやすくなるレアイベント
                eventDict.Add(3, 5.0f + rare_event_kakuritsu_hosei); //お宝発見
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
        _text.text = "にいちゃん！！ なんかあやしい草むらがあるよ..？　しらべる？" + "\n" + "（体力を" + GameMgr.ColorPink + "３つ" + "</color>" + "消費するよ。）";

        //_TreasureImg.sprite = treasure1;
        OpenTreasureButton_obj.SetActive(true);
        treasure_text.text = "調べる";
        TreasureImage_obj.SetActive(false);
        //CharacterSDImage.SetActive(false);

        //Treasure_Status = 0; //0=宝箱 音と画像が変わるだけ。
        Treasure_Status = 1;
    }

    void treasure_Check2()
    {
        //怪しげな場所
        //sc.PlaySe(84);
        _text.text = "にいちゃん！！ きれいなお花畑！　探検してみる？" + "\n" + "（体力を" + GameMgr.ColorPink + "３つ" + "</color>" + "消費するよ。）";

        //_TreasureImg.sprite = treasure1;
        OpenTreasureButton_obj.SetActive(true);
        treasure_text.text = "探検";
        TreasureImage_obj.SetActive(false);
        //CharacterSDImage.SetActive(false);

        Treasure_Status = 1;
    }

    void treasure_Check3()
    {
        //怪しげな場所
        //sc.PlaySe(84);
        _text.text = "にいちゃん！！ あやしげな木があるよ！　みてみる？" + "\n" + "（体力を" + GameMgr.ColorPink + "３つ" + "</color>" + "消費するよ。）";

        //_TreasureImg.sprite = treasure1;
        OpenTreasureButton_obj.SetActive(true);
        treasure_text.text = "調べる";
        TreasureImage_obj.SetActive(false);
        //CharacterSDImage.SetActive(false);

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

            case "BerryFarm":

                Treasure_BerryFarm();
                break;

            case "Lavender_field":

                Treasure_Forest();
                break;

            case "StrawberryGarden":

                Treasure_Forest();
                break;

            case "HimawariHill":

                Treasure_Himawari();
                break;

            case "BirdSanctuali":

                Treasure_BirdSanctuali();
                break;

            default:
                Treasure_Forest();
                break;
        }
    }

    void Treasure_Forest()
    {
        InitializeTreasureDicts(0); //中の番号で、どの宝箱かを指定する

        TreasureGetAction();
    }

    void Treasure_BirdSanctuali()
    {
        InitializeTreasureDicts(2); //中の番号で、どの宝箱かを指定する

        TreasureGetAction();
    }

    void Treasure_BerryFarm()
    {
        InitializeTreasureDicts(3); //中の番号で、どの宝箱かを指定する

        TreasureGetAction();
    }

    void Treasure_Himawari()
    {
        InitializeTreasureDicts(1); //中の番号で、どの宝箱かを指定する

        TreasureGetAction();
        
    }

    void TreasureGetAction()
    {
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

        //ゲットアニメーション
        //まず、初期値。
        Sequence sequence = DOTween.Sequence();
        TreasureGetitem_obj.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(TreasureGetitem_obj.transform.DOScale(new Vector3(0.65f, 0.65f, 1.0f), 0.0f)
            ); //

        //移動のアニメ
        sequence.Append(TreasureGetitem_obj.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f)
            .SetEase(Ease.OutElastic)); //はねる動き
                                        //.SetEase(Ease.OutExpo)); //スケール小からフェードイン
        sequence.Join(TreasureGetitem_obj.GetComponent<CanvasGroup>().DOFade(1, 0.2f));

        //アイテムの取得処理
        pitemlist.addPlayerItemString(itemName, 1);

        //取得したアイテムをリストに入れ、あとでリザルト画面で表示
        //_itemid = pitemlist.SearchItemString(itemName);
        getmatplace_panel.result_items[itemName] += 1;

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

        //通常アイテムか、レシピなどのイベントアイテムかでるかを決める。これもくじ引き。
        //random = Random.Range(0, 100);

        switch (_treasure_num)
        {
            case 0: //お宝セットテーブル　森

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名　ItemDatabaseのitemNameと同じ名前にする。
                treasureInfo.Add(1, "doro_dango");
                treasureInfo.Add(2, "kirakira_stone1");
                treasureInfo.Add(3, "emerald_suger");
                treasureInfo.Add(4, "earlgrey_leaf");
                treasureInfo.Add(5, "copper_coin");
                treasureInfo.Add(6, "grape");

                treasureDropDict.Add(0, 10.0f); //こっちは確率テーブル　はずれの場合はなにもなし。
                treasureDropDict.Add(1, 10.0f);
                treasureDropDict.Add(2, 20.0f + rare_event_kakuritsu);
                treasureDropDict.Add(3, 20.0f + rare_event_kakuritsu);
                treasureDropDict.Add(4, 10.0f + rare_event_kakuritsu);
                treasureDropDict.Add(5, 10.0f + rare_event_kakuritsu);
                treasureDropDict.Add(6, 20.0f);
                break;

            case 1: //お宝セットテーブル　ひまわりの丘

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名　ItemDatabaseのitemNameと同じ名前にする。
                treasureInfo.Add(1, "doro_dango");
                treasureInfo.Add(2, "kirakira_stone1");
                treasureInfo.Add(3, "rich_komugiko");

                treasureDropDict.Add(0, 10.0f); //こっちは確率テーブル　はずれの場合はなにもなし。
                treasureDropDict.Add(1, 0.0f);
                treasureDropDict.Add(2, 20.0f);
                treasureDropDict.Add(3, 70.0f + rare_event_kakuritsu);
                break;

            case 2: //お宝セットテーブル　バードサンクチュアリ

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名　ItemDatabaseのitemNameと同じ名前にする。
                treasureInfo.Add(1, "doro_dango");
                treasureInfo.Add(2, "kirakira_stone1");
                treasureInfo.Add(3, "egg_premiaum");
                treasureInfo.Add(4, "diamond_1");
                treasureInfo.Add(5, "kirakira_stone2");

                treasureDropDict.Add(0, 10.0f); //こっちは確率テーブル　はずれの場合はなにもなし。
                treasureDropDict.Add(1, 0.0f);
                treasureDropDict.Add(2, 20.0f);
                treasureDropDict.Add(3, 50.0f + rare_event_kakuritsu);
                treasureDropDict.Add(4, 10.0f + rare_event_kakuritsu);
                treasureDropDict.Add(5, 10.0f + rare_event_kakuritsu);
                break;

            case 3: //お宝セットテーブル　ベリーファーム

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名　ItemDatabaseのitemNameと同じ名前にする。
                treasureInfo.Add(1, "strawberry");
                treasureInfo.Add(2, "cherry");
                treasureInfo.Add(3, "blackberry");

                treasureDropDict.Add(0, 20.0f); //こっちは確率テーブル　はずれの場合はなにもなし。
                treasureDropDict.Add(1, 40.0f);
                treasureDropDict.Add(2, 39.0f);
                treasureDropDict.Add(3, 1.0f + (rare_event_kakuritsu*0.5f)); //rare_event_kakuritsu = アイテム発見力によるバフは最大で50%
                break;

            default:

                treasureInfo.Add(0, "Non"); //宝箱データ　こっちはアイテム名
                treasureInfo.Add(1, "kirakira_stone1");
                treasureInfo.Add(2, "kirakira_stone1");
                treasureInfo.Add(3, "kirakira_stone1");
                treasureInfo.Add(4, "kirakira_stone1");

                treasureDropDict.Add(0, 10.0f); //こっちは確率テーブル
                treasureDropDict.Add(1, 30.0f);
                treasureDropDict.Add(2, 20.0f);
                treasureDropDict.Add(3, 20.0f);
                treasureDropDict.Add(4, 20.0f);
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

    //GetMatPlace_Panel.csからよみだし
    public void SetInit()
    {
        cullent_total_mat = 0;
        _a_zairyomax = "";
    }

    //妹が外出から帰ってきて材料をゲットする処理 EventDataBaseから読み出し
    public void OutGirlGetRandomMaterials(int _index) 
    {

        index = _index; //採取地IDの決定
        _findpower_girl_getmat_final = 0;

        // 入手できるアイテムのデータベース
        ResetItemDicts();
        InitializeDicts(_index);

        //プレイヤーのアイテム発見力をバフつきで計算
        _buf_findpower = bufpower_keisan.Buf_findpower_Keisan(); //プレイヤー装備品計算
        player_girl_findpower_final = PlayerStatus.player_girl_findpower + _buf_findpower;
       
        _findpower_girl_getmat = player_girl_findpower_final - PlayerStatus.player_girl_findpower_def;

        _findpower_girl_getmat_final = 0;
        while(_findpower_girl_getmat >= 30)
        {
            _findpower_girl_getmat -= 30;
            _findpower_girl_getmat_final++;
        }
        if(_findpower_girl_getmat_final < 1) { _findpower_girl_getmat_final = 0; }
        //Debug.Log("_findpower_girl_getmat_final: " + _findpower_girl_getmat_final);

        //アイテムの入手
        for (count = 0; count < 5 + _findpower_girl_getmat_final; count++) //〇回繰り返す
        {
            ItemGetMethod(count);
        }

        //レアアイテムの入手
        for (count = 0; count < 3 + _findpower_girl_getmat_final; count++) //〇回繰り返す
        {

            RareItemGetMethod(count);
        }
    }
}
