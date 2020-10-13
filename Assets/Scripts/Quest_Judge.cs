using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class Quest_Judge : MonoBehaviour {

    private GameObject canvas;

    private GameObject text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _text; //同じく、Scene「Compund」用。

    private GameObject shopMain_obj;
    private Shop_Main shopMain;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject shopquestlistController_obj;
    private ShopQuestListController shopquestlistController;
    private GameObject back_ShopFirst_obj;
    private Button back_ShopFirst_btn;

    private Toggle questListToggle;
    private Toggle nouhinToggle;

    private Exp_Controller exp_Controller;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private GameObject updown_counter_obj;
    private Updown_counter updown_counter;
    private Button[] updown_button = new Button[2];

    private PlayerItemList pitemlist;
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private QuestSetDataBase quest_database;

    private GameObject black_effect;

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの点数
    List<int> itemslot_NouhinScore = new List<int>();
    List<int> itemslot_PitemScore = new List<int>();
    private int check_slot_nouhinscore;

    //お菓子の点数
    List<int> result_OkashiScore = new List<int>();

    private GameObject yes; //PlayeritemList_ScrollViewの子オブジェクト「yes」ボタン
    private Text yes_text;
    private GameObject no; //PlayeritemList_ScrollViewの子オブジェクト「no」ボタン
    private Text no_text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private SoundController sc;

    private Dictionary<int, int> deleteOriginalList = new Dictionary<int, int>(); //オリジナルアイテムリストの削除用のリスト。ID, 個数のセット

    private int i, count, list_count;
    private bool nouhinOK_flag;
    private int nouhinOK_status;

    private int _getMoney;
    private string _kanso;

    private int _id;
    private int _Qid;
    private int _questID;
    private int _qitemID;

    private int del_itemid;
    private int del_itemkosu;

    private int set_kaisu;
    private int okashi_totalscore;
    private int okashi_totalkosu;
    private int okashi_score;

    private string _filename;
    private string _itemname;
    private string _itemsubtype;

    private int _kosu_default;
    private int _kosu_total;
    private int _kosu_min;
    private int _kosu_max;
    private int _buy_price;

    private int _rich;
    private int _sweat;
    private int _bitter;
    private int _sour;

    private int _crispy;
    private int _fluffy;
    private int _smooth;
    private int _hardness;
    private int _jiggly;
    private int _chewy;

    private string[] _tp;

    private string _a;
    private int _temp_shokukan;

    private int itemType;
    private string _basename;
    private int _basehp;
    private int _baseday;
    private int _basequality;
    private int _baseexp;
    private float _baseprobability;
    private int _baserich;
    private int _basesweat;
    private int _basebitter;
    private int _basesour;
    private int _basecrispy;
    private int _basefluffy;
    private int _basesmooth;
    private int _basehardness;
    private int _basejiggly;
    private int _basechewy;
    private int _basepowdery;
    private int _baseoily;
    private int _basewatery;
    private float _basegirl1_like;
    private int _basecost;
    private int _basesell;
    private string[] _basetp;
    private string[] _koyutp;
    private string _base_itemType;
    private string _base_itemType_sub;
    private int _base_extreme_kaisu;
    private int _base_item_hyouji;

    private bool judge_anim_on;
    private int judge_anim_status;
    private bool judge_end;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    //時間
    private float timeOut;

    private GameObject eat_hukidashiPrefab;
    private GameObject eat_hukidashiitem;
    private Text eat_hukidashitext;

    private GameObject character;

    private GameObject questResultPanel;
    private bool endresultbutton;
    private Transform questResultPanel_tsukatext_pos;
    private Vector3 questResultPanel_tsukatext_defpos;

    private int keta;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        shopMain_obj = GameObject.FindWithTag("Shop_Main");
        shopMain = shopMain_obj.GetComponent<Shop_Main>();

        shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();
        back_ShopFirst_obj = shopquestlistController_obj.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        questListToggle = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_QuestList").GetComponent<Toggle>();
        nouhinToggle = shopquestlistController_obj.transform.Find("CategoryView/Viewport/Content/Cate_Nouhin").GetComponent<Toggle>();

        yes = shopquestlistController_obj.transform.Find("Yes").gameObject;
        yes_text = yes.GetComponentInChildren<Text>();
        no = shopquestlistController_obj.transform.Find("No").gameObject;
        no_text = no.GetComponentInChildren<Text>();

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();       

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //クエストデータベースの取得
        quest_database = QuestSetDataBase.Instance.GetComponent<QuestSetDataBase>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();

        //黒半透明パネルの取得
        black_effect = canvas.transform.Find("Black_Panel_A").gameObject;

        //キャラクタ取得
        character = GameObject.FindWithTag("Character");

        //Prefab内の、コンテンツ要素を取得
        eat_hukidashiPrefab = (GameObject)Resources.Load("Prefabs/QuestJudge_hukidashi");

        //初期化
        _basetp = new string[database.items[0].toppingtype.Length];
        _koyutp = new string[database.items[0].koyu_toppingtype.Length];
        _tp = new string[quest_database.questset[0].Quest_topping.Length];

        InitializeItemSlotDicts();

        judge_anim_on = false;
        judge_anim_status = 0;
        judge_end = false;

        //クエストリザルトパネル
        questResultPanel = canvas.transform.Find("QuestResultPanel").gameObject;
        questResultPanel.SetActive(false);
        endresultbutton = false;
        questResultPanel_tsukatext_pos = questResultPanel.transform.Find("QuestResultImage/MoneyTsukaText").transform;
        questResultPanel_tsukatext_defpos = questResultPanel_tsukatext_pos.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (judge_anim_on == true)
        {
            switch (judge_anim_status)
            {
                case 0: //初期化 状態１

                    MoneyStatus_Panel_obj.SetActive(false);
                    //text_area.SetActive(false);
                    shopquestlistController_obj.SetActive(false);
                    black_effect.SetActive(false);
                    back_ShopFirst_obj.SetActive(false);

                    timeOut = 1.5f;
                    judge_anim_status = 1;


                    //カメラ寄る。
                    trans = 2; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    //吹き出しの作成
                    eat_hukidashiitem = Instantiate(eat_hukidashiPrefab, character.transform);
                    eat_hukidashitext = eat_hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                    eat_hukidashitext.text = ".";
                    sc.PlaySe(7);

                    _text.text = "鑑定中.";

                    break;

                case 1: // 状態2

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.5f;
                        judge_anim_status = 2;

                        eat_hukidashitext.text = ". .";

                        _text.text = "鑑定中. .";

                    }
                    break;

                case 2:

                    if (timeOut <= 0.0)
                    {
                        timeOut = 1.0f;
                        judge_anim_status = 3;

                        //eat_hukidashitext.text = ". .";

                    }
                    break;

                case 3: //アニメ終了。判定する

                    MoneyStatus_Panel_obj.SetActive(true);
                    //text_area.SetActive(true);


                    //食べ中吹き出しの削除
                    if (eat_hukidashiitem != null)
                    {
                        Destroy(eat_hukidashiitem);
                    }

                    judge_anim_on = false;
                    judge_end = true;
                    judge_anim_status = 0;

                    //カメラ寄る。
                    trans = 0; //transが0以下のときに、ズームアウトするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    break;

                default:
                    break;
            }

            //時間減少
            timeOut -= Time.deltaTime;
        }
    }




    //
    //指定のアイテムを、必要個数だけ納品する場合の処理。味の判定などはしない。
    //
    public void Quest_result(int _ID)
    {
        _qitemID = _ID;

        SetInitQItem(_qitemID);

        nouhinOK_flag = false;
        deleteOriginalList.Clear();

        _kosu_total = _kosu_default; //トータルで〇個いる。デフォルトアイテムから１個、プレイヤーアイテムリストから、１個＋１個のような感じで、減っていく。

        //プレイヤーのアイテムリストを検索
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            if (pitemlist.playeritemlist[i] > 0) //持っている個数が1以上のアイテムのみ、探索。
            {                

                //まず該当アイテムがあるかどうか調べる。
                if( _itemname == database.items[i].itemName)
                {

                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.playeritemlist[i] >= _kosu_total)
                    {
                        nouhinOK_flag = true;

                        //所持アイテムを削除
                        pitemlist.deletePlayerItem(i, _kosu_total);
                    }
                    else
                    {
                        _kosu_total -= pitemlist.playeritemlist[i];
                        //さらにデリートリストに追加しておく。
                        del_itemid = i;
                        del_itemkosu = pitemlist.playeritemlist[i];

                        nouhinOK_flag = false;
                    }
                }
            }
        }
        
        if (!nouhinOK_flag) //上の探索で納品OKがtrueなら、オリジナルアイテムリストは検索しない
        {
            //次にプレイヤーのオリジナルアイテムリストを検索。player_originalitemlistは個数が1以上のものしかセットされていない。
            i = 0;
            while (i < pitemlist.player_originalitemlist.Count)
            {

                //まず該当アイテムがあるかどうか調べる。
                if (_itemname == pitemlist.player_originalitemlist[i].itemName)
                {
                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.player_originalitemlist[i].ItemKosu >= _kosu_total)
                    {
                        nouhinOK_flag = true;

                        //デリートリストに追加しておく。
                        //さらにデリートリストに追加しておく。あとで降順に削除
                        deleteOriginalList.Add(i, _kosu_total);

                        break;
                    }
                    else
                    {
                        _kosu_total -= pitemlist.player_originalitemlist[i].ItemKosu;
                        //さらにデリートリストに追加しておく。あとで降順に削除
                        deleteOriginalList.Add(i, pitemlist.player_originalitemlist[i].ItemKosu);

                        nouhinOK_flag = false;
                    }
                }
                i++;
            }
        }

        StartCoroutine("Okashi_Judge_Anim1");
         
    }

    IEnumerator Okashi_Judge_Anim1()
    {
        judge_anim_on = true;

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        judge_end = false;

        if (nouhinOK_flag)
        {
            //アイテム削除
            if (deleteOriginalList.Count > 0)
            {
                pitemlist.deletePlayerItem(del_itemid, del_itemkosu); //デフォルトアイテムから先に削除
                DeleteOriginalItem(); //オリジナルからも削除
            }

            _getMoney = _buy_price * _kosu_default;

            //足りてるので、納品完了の処理
            _text.text = "報酬 " + GameMgr.ColorYellow + _getMoney + "</color>" + "G を受け取った！" + "\n" + "ありがとう！お客さんもとても喜んでいるわ！";

            //該当のクエストを削除
            quest_database.questTakeset.RemoveAt(_qitemID);

            Debug.Log("納品完了！");

            //ジャキーンみたいな音を鳴らす。
            //sc.PlaySe(4);
            sc.PlaySe(76);
            sc.PlaySe(31);

            //クエストリザルト画面をだす。
            questResultPanel.SetActive(true);
            questResultPanel.transform.Find("QuestResultImage/GetMoneyParam").GetComponent<Text>().text = _getMoney.ToString();
            keta = Digit(_getMoney);
            questResultPanel_tsukatext_pos.DOLocalMove(new Vector3(20f * (keta - 1), 0f, 0), 0.0f).SetRelative();

            StartCoroutine("EndQuestResultButton");

        }
        else
        {
            sc.PlaySe(6);
            _text.text = "まだ数が足りてないようね..。";

            //リスト更新
            shopquestlistController.NouhinList_DrawView();

            back_ShopFirst_obj.SetActive(true);
            ResetQuestStatus();
        }
    }

        void DeleteOriginalItem()
    {

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteOriginalList.Count > 0)
        {
            //Debug.Log("オリジナルアイテムを納品");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteOriginalList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                if (deletePair.Key == exp_Controller._temp_extreme_id && exp_Controller._temp_extremeSetting == true)
                {
                    exp_Controller._temp_extreme_id = 9999;
                    exp_Controller._temp_extremeSetting = false;
                }
                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);

                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }
    }





    //
    //クッキーなどの判定するお菓子を納品した場合の処理
    //
    public void Okashi_Judge(int _ID)
    {
        _qitemID = _ID;

        Okashi_Judge_Method();
    }
   

    void Okashi_Judge_Method()
    {
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        SetInitQItem(_qitemID); //依頼アイテムのパラメータを代入

        deleteOriginalList.Clear();
        result_OkashiScore.Clear();
        okashi_totalscore = 0;
        okashi_totalkosu = 0;

        set_kaisu = pitemlistController._listcount.Count;

        //listcount分 提出するアイテムのパラメータを判定する。
        for (list_count = 0; list_count < set_kaisu; list_count++)
        {
            //選択したアイテムのデータをセット
            SetInitNouhinItem(pitemlistController._listcount[list_count]);

            //
            //お菓子の正解判定。①タイプ　②味　③スロットを見る。
            //A. 一つでも違うのが入っていると、失格
            //B. タイプはOKで、味が足りない場合は、やはり失格 nouhinOK_status = 2;
            //C. それをこえたら、各アイテムごとの平均値*納品個数をみて、最終的なスコアをだす。
            //

            okashi_score = 0;            
            check_slot_nouhinscore = 0;

            //①指定のトッピングがあるかをチェック。一つでも指定のものがあれば、OK

            nouhinOK_status = 2; //先にNGはたてておく。

            //納品用スコアがすべて０の場合、トッピングを計算しないので、無視する。
            for(i=0; i < itemslot_NouhinScore.Count; i++)
            {
                //0はNonなので、無視
                if (i != 0)
                {
                    check_slot_nouhinscore += itemslot_NouhinScore[i];
                }
            }

            if (check_slot_nouhinscore == 0)
            {
                nouhinOK_status = 0;
            }
            else
            {
                i = 0;
                while (i < itemslot_NouhinScore.Count)
                {
                    //0はNonなので、無視
                    if (i != 0)
                    {
                        //納品スコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                        if (itemslot_PitemScore[i] >= itemslot_NouhinScore[i])
                        {
                            if (itemslot_NouhinScore[i] != 0)
                            {
                                nouhinOK_status = 0;
                                break;
                            }
                        }
                        //一つでも満たしてないものがある場合は、NGフラグがたつ。いちごくっきーがほしいのに、いちごがのってなければ、ダメ、という理屈。
                        else
                        {

                        }
                    }
                    i++;
                }
            }

            //②味パラメータの計算
            if (_baserich >= _rich)
            {
                if (_rich != 0)
                {
                    okashi_score += _baserich;
                }
            }
            else
            {
                okashi_score += 0;
                //nouhinOK_status = 2;
                _a = "コクがちょっと足りないみたい。";
            }

            if (_sweat > 0)
            {
                if (_basesweat >= _sweat)
                {

                    okashi_score += _basesweat;

                }
                else
                {
                    okashi_score += 0;
                    //nouhinOK_status = 2;
                    _a = "甘さがちょっと足りないみたい。";
                }
            }

            if (_sour > 0)
            {
                if (_basebitter >= _bitter)
                {

                    okashi_score += _basebitter;

                }
                else
                {
                    okashi_score += 0;
                    //nouhinOK_status = 2;
                    _a = "苦味がちょっと足りないみたい。";
                }
            }

            if (_sour > 0)
            {
                if (_basesour >= _sour)
                {

                    okashi_score += _basesour;

                }
                else
                {
                    okashi_score += 0;
                    //nouhinOK_status = 2;
                    _a = "酸味がちょっと足りないみたい。";
                }
            }

            if (_crispy > 0)
            {
                if (_basecrispy >= _crispy)
                {
                    _temp_shokukan = _basecrispy - _crispy;
                    okashi_score += _temp_shokukan;
                }
                else
                {
                    okashi_score += 0;
                    //nouhinOK_status = 2;                
                    _a = "さくさくした感じがちょっと足りないみたい。";
                }
            }

            if (_fluffy > 0)
            {
                if (_basefluffy >= _fluffy)
                {
                    _temp_shokukan = _basefluffy - _fluffy;
                    okashi_score += _temp_shokukan;

                }
                else
                {
                    okashi_score += 0;
                    //nouhinOK_status = 2;
                    _a = "ふんわり感がちょっと足りないみたい。";
                }
            }

            if (_smooth > 0)
            {
                if (_basesmooth >= _smooth)
                {
                    _temp_shokukan = _basesmooth - _smooth;
                    okashi_score += _temp_shokukan;

                }
                else
                {
                    okashi_score += (int)(_basesmooth * 0.5f);
                    //nouhinOK_status = 2;
                    _a = "なめらかな感じがちょっと足りないみたい。";
                }
            }

            if (_hardness > 0)
            {
                if (_basehardness >= _hardness)
                {
                    _temp_shokukan = _basehardness - _hardness;
                    okashi_score += _temp_shokukan;

                }
                else
                {
                    okashi_score += (int)(_basehardness * 0.5f);
                    //nouhinOK_status = 2;
                    _a = "歯ごたえがちょっと足りないみたい。";
                }
            }

            if (_jiggly > 0)
            {
                if (_basejiggly >= _jiggly)
                {
                    _temp_shokukan = _basejiggly - _jiggly;
                    okashi_score += _temp_shokukan;

                }
                else
                {
                    okashi_score += (int)(_basejiggly * 0.5f);
                    //nouhinOK_status = 2;
                    _a = "ぷにぷに感がちょっと足りないみたい。";
                }
            }

            if (_chewy > 0)
            {
                if (_basechewy >= _chewy)
                {
                    _temp_shokukan = _basechewy - _chewy;
                    okashi_score += _temp_shokukan;
                }
                else
                {
                    okashi_score += (int)(_basechewy * 0.5f);
                    //nouhinOK_status = 2;
                    _a = "噛みごたえがちょっと足りないみたい。";
                }
            }

            //特定のお菓子の判定。一致していない場合は、③は計算するまでもなく不正解となる。
            if (_itemname == "Non") //特に指定なし
            {
                //③お菓子の種別の計算
                if (_itemsubtype == "Non") //特に指定なし
                {

                }
                else if (_itemsubtype == _base_itemType_sub) //お菓子の種別が一致している。
                {

                }
                else
                {
                    //不正解。そもそも違うお菓子を納品している。
                    nouhinOK_status = 1;
                }
            }
            else if (_itemname == _basename) //お菓子の名前が一致している。
            {
                //サブは計算せず、特定のお菓子自体が正解なら、正解

            }
            else
            {
                //不正解。そもそも違うお菓子を納品している。
                nouhinOK_status = 1;
            }

            //④トッピングスロットをみて、スコアを加算する。
            for (i = 0; i < itemslot_PitemScore.Count; i++)
            {
                //0はNonなので、無視
                if (i != 0)
                {
                    //トッピングごとに、得点を加算する。妹の採点のtotal_scoreの加算値と共有。
                    if (itemslot_PitemScore[i] > 0)
                    {
                        okashi_score += slotnamedatabase.slotname_lists[i].slot_totalScore;                       
                    }
                }
            }

            //⑤油っこいなどのマイナスの値がついてた場合、マイナス補正
            if (_basepowdery > 50)
            {
                okashi_score -= 30;
            }
            if (_baseoily > 50)
            {
                okashi_score -= 30;
            }
            if (_basewatery > 50)
            {
                okashi_score -= 30;
            }

            //採点はここまで


            //スコアを保持
            result_OkashiScore.Add(okashi_score * pitemlistController._listkosu[list_count]);
            okashi_totalkosu += pitemlistController._listkosu[list_count];

            //アイテムを削除
            switch (itemType)
            {
                case 0:

                    //もし、エクストリームパネルにセットされているお菓子を納品し、個数が０になった場合。処理が必要。
                    if (exp_Controller._temp_extreme_id == _id)
                    {
                        if (pitemlist.playeritemlist[_id] <= 0)
                        {
                            exp_Controller._temp_extreme_id = 9999;
                        }
                    }
                    //所持アイテムを削除
                    pitemlist.deletePlayerItem(_id, _kosu_default);
                    break;

                case 1:

                    //もし、エクストリームパネルにセットされているお菓子を納品し、個数が０になった場合。処理が必要。
                    if (exp_Controller._temp_extreme_id == _id)
                    {
                        if (pitemlist.player_originalitemlist[_id].ItemKosu <= 0)
                        {
                            exp_Controller._temp_extreme_id = 9999;
                        }
                    }

                    //所持アイテムをリストに追加し、あとで降順に削除
                    deleteOriginalList.Add(_id, _kosu_default);
                    break;

            }
        }

        //各スコアを加算し、平均をとり、最終スコアを算出
        for (i = 0; i < result_OkashiScore.Count; i++)
        {
            okashi_totalscore += result_OkashiScore[i];
        }
        if (okashi_totalkosu == 0) { okashi_totalkosu = 1; }

        //最終スコア
        okashi_totalscore /= okashi_totalkosu;

        if(okashi_totalscore <= 0) //0点以下でも、無条件でダメ
        {
            nouhinOK_status = 2;
        }

        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        DeleteOriginalItem();
        

        StartCoroutine("Okashi_Judge_Anim2");
        
    }

    IEnumerator Okashi_Judge_Anim2()
    {
        judge_anim_on = true;

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        judge_end = false;

        switch (nouhinOK_status)
        {
            case 0: //正解の場合

                //味によって、取得のお金が増減する。おいしいと、お金もちょっとプラス。

                if (okashi_totalscore < 30) //粗悪なお菓子だと、マイナス評価
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 0.5f);
                    _kanso = "う～ん..。お客さん不満だったみたい。次からは気をつけてね。" + "\n" + "報酬額を少し減らされてしまった！";
                    
                }
                else if (okashi_totalscore >= 30 && okashi_totalscore < 45) //30~45
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 0.75f);
                    _kanso = "少し不満が残るわね..。";
                }
                else if (okashi_totalscore >= 45 && okashi_totalscore < GameMgr.low_score) //45~60
                {
                    _getMoney = _buy_price * _kosu_default;
                    _kanso = "まずまずの出来ね。";
                }
                else if (okashi_totalscore >= GameMgr.low_score && okashi_totalscore < 75) //60~75
                {
                    _getMoney = _buy_price * _kosu_default;
                    _kanso = "ありがとう！　おいしいって喜んでたわ！";
                }
                else if (okashi_totalscore >= 75 && okashi_totalscore < GameMgr.high_score) //75~85
                {
                    _getMoney = (int)(_buy_price * _kosu_default * 1.2f);
                    _kanso = "ありがとう！　お客さん、かなり喜んでくれたみたい！";
                }
                else if (okashi_totalscore >= GameMgr.high_score && okashi_totalscore < 100) //85~100
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (okashi_totalscore / GameMgr.high_score));
                    _kanso = "ありがとう！とても良い出来みたい！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 100 && okashi_totalscore < 120) //100~120
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (okashi_totalscore / GameMgr.high_score));
                    _kanso = "グレイトだわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 120 && okashi_totalscore < 150) //120~150
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (okashi_totalscore / GameMgr.high_score));
                    _kanso = "ほっぺたがとろけちゃうぐらい最高だって！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 150 && okashi_totalscore < 175) //150~175
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (okashi_totalscore / GameMgr.high_score));
                    _kanso = "まるで宝石のように美しい味らしいわ！！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 175 && okashi_totalscore < 200) //175~200
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (okashi_totalscore / GameMgr.high_score));
                    _kanso = "天使のような素晴らしい味らしいわ！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }
                else if (okashi_totalscore >= 200) //200~
                {
                    _getMoney = (int)(_buy_price * _kosu_default * (okashi_totalscore / GameMgr.high_score));
                    _kanso = "神の味だって、絶叫してたわ！ぜひまたお願いね！" + "\n" + "ちょっとだけど、報酬額を多めにあげるわね。";
                }

                _text.text = "報酬 " + GameMgr.ColorYellow + _getMoney + "</color>" + "G を受け取った！" + "\n" + _kanso;

                Debug.Log("納品完了！" + " 採点：" + okashi_totalscore + "点！");

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                //ジャキーンみたいな音を鳴らす。                
                //sc.PlaySe(4);
                sc.PlaySe(76);
                sc.PlaySe(31);

                //クエストリザルト画面をだす。
                questResultPanel.SetActive(true);
                questResultPanel.transform.Find("QuestResultImage/GetMoneyParam").GetComponent<Text>().text = _getMoney.ToString();
                keta = Digit(_getMoney);
                questResultPanel_tsukatext_pos.DOLocalMove(new Vector3(20f* (keta-1), 0f, 0), 0.0f).SetRelative();

                StartCoroutine("EndQuestResultButton");
                break;

            case 1: //そもそも違うお菓子を納品

                sc.PlaySe(6);

                _text.text = "これはちょっと違うお菓子みたいね。";

                Debug.Log("納品失敗..");

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                ResetQuestStatus();
                break;

            case 2: //ほしいトッピングが乗ってなかった場合。

                sc.PlaySe(6);

                if (_a != "")
                {
                    //_text.text = _a + "\n" + "う～ん。もうちょっと味を頑張ったほうがいいかも。";
                    _text.text = "う～ん。お客さん、あまり喜んでいないみたい..。";
                }
                else
                {
                    _text.text = "う～ん。お客さん、あまり喜んでいないみたい..";
                }

                Debug.Log("納品失敗..");

                //該当のクエストを削除
                quest_database.questTakeset.RemoveAt(_qitemID);

                ResetQuestStatus();
                break;
        }
        
    }

    public void OnEndResultButton() //クエストリザルトボタンおすと、フラグがONに。
    {
        sc.PlaySe(2);

        endresultbutton = true;
        questResultPanel.SetActive(false);

        //通貨のテキスト位置を元に戻しておく
        questResultPanel_tsukatext_pos.localPosition = questResultPanel_tsukatext_defpos;
    }

    IEnumerator EndQuestResultButton()
    {
        while (!endresultbutton)
        {
            yield return null;
        }

        endresultbutton = false;        

        //所持金をプラス
        moneyStatus_Controller.GetMoney(_getMoney); //アニメつき        

        ResetQuestStatus();
    }

    void ResetQuestStatus()
    {
        //リスト更新
        shopquestlistController.NouhinList_DrawView();
        shopquestlistController.nouhin_select_on = 0;

        yes.SetActive(false);
        no.SetActive(false);

        questListToggle.interactable = true;
        nouhinToggle.interactable = true;

        back_ShopFirst_btn.interactable = true;
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        shopMain.shop_status = 0;
    }


    //
    //パラメータのセットアップ
    //
    void SetInitQItem(int _count)
    {
        // 判定用に依頼のお菓子のパラメータを代入
        _Qid = quest_database.questTakeset[_count]._ID;              //基本判定のときは、使わない
        _questID = quest_database.questTakeset[_count].Quest_ID;    //基本判定のときは、使わない

        _itemname = quest_database.questTakeset[_count].Quest_itemName;
        _itemsubtype = quest_database.questTakeset[_count].Quest_itemSubtype;

        _kosu_min = quest_database.questTakeset[_count].Quest_kosu_min;
        _kosu_max = quest_database.questTakeset[_count].Quest_kosu_max;

        _kosu_default = quest_database.questTakeset[_count].Quest_kosu_default;
        _buy_price = quest_database.questTakeset[_count].Quest_buy_price;

        _rich = quest_database.questTakeset[_count].Quest_rich;
        _sweat = quest_database.questTakeset[_count].Quest_sweat;
        _bitter = quest_database.questTakeset[_count].Quest_bitter;
        _sour = quest_database.questTakeset[_count].Quest_sour;

        _crispy = quest_database.questTakeset[_count].Quest_crispy;
        _fluffy = quest_database.questTakeset[_count].Quest_fluffy;
        _smooth = quest_database.questTakeset[_count].Quest_smooth;
        _hardness = quest_database.questTakeset[_count].Quest_hardness;
        _jiggly = quest_database.questTakeset[_count].Quest_jiggly;
        _chewy = quest_database.questTakeset[_count].Quest_chewy;

        for (i = 0; i < _tp.Length; i++)
        {
            _tp[i] = quest_database.questTakeset[_count].Quest_topping[i];
        }


        //一回まず各スコアを初期化。
        for (i = 0; i < itemslot_NouhinScore.Count; i++)
        {
            itemslot_NouhinScore[i] = 0;
        }

        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _tp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_tp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslot_NouhinScore[count]++;
                }
                count++;
            }
        }
    }

    void SetInitNouhinItem(int _count_n)
    {
        // 判定用に依頼のお菓子のパラメータを代入
        itemType = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggleitem_type;

        switch (itemType)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                _id = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggle_originplist_ID;

                //各パラメータを取得
                _basename = database.items[_id].itemName;
                _basehp = database.items[_id].itemHP;
                _baseday = database.items[_id].item_day;
                _basequality = database.items[_id].Quality;
                _baseexp = 0; //元アイテムの経験値は影響なし。材料のみの経験値を加算する。
                _baseprobability = database.items[_id].Ex_Probability;
                _baserich = database.items[_id].Rich;
                _basesweat = database.items[_id].Sweat;
                _basebitter = database.items[_id].Bitter;
                _basesour = database.items[_id].Sour;
                _basecrispy = database.items[_id].Crispy;
                _basefluffy = database.items[_id].Fluffy;
                _basesmooth = database.items[_id].Smooth;
                _basehardness = database.items[_id].Hardness;
                _basejiggly = database.items[_id].Jiggly;
                _basechewy = database.items[_id].Chewy;
                _basepowdery = database.items[_id].Powdery;
                _baseoily = database.items[_id].Oily;
                _basewatery = database.items[_id].Watery;
                _basegirl1_like = database.items[_id].girl1_itemLike;
                _basecost = database.items[_id].cost_price;
                _basesell = database.items[_id].sell_price;
                _base_itemType = database.items[_id].itemType.ToString();
                _base_itemType_sub = database.items[_id].itemType_sub.ToString();
                _base_extreme_kaisu = database.items[_id].ExtremeKaisu;
                _base_item_hyouji = database.items[_id].item_Hyouji;

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = database.items[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = database.items[_id].koyu_toppingtype[i].ToString();
                }

                break;

            case 1: //オリジナルプレイヤーアイテムリストから選択している場合

                //さらに、オリジナルのプレイヤーアイテムリストの番号を参照する。

                _id = pitemlistController._listitem[_count_n].GetComponent<itemSelectToggle>().toggle_originplist_ID;

                //各パラメータを取得
                _basename = pitemlist.player_originalitemlist[_id].itemName;
                _basehp = pitemlist.player_originalitemlist[_id].itemHP;
                _baseday = pitemlist.player_originalitemlist[_id].item_day;
                _basequality = pitemlist.player_originalitemlist[_id].Quality;
                _baseexp = pitemlist.player_originalitemlist[_id].Exp;
                _baseprobability = pitemlist.player_originalitemlist[_id].Ex_Probability;
                _baserich = pitemlist.player_originalitemlist[_id].Rich;
                _basesweat = pitemlist.player_originalitemlist[_id].Sweat;
                _basebitter = pitemlist.player_originalitemlist[_id].Bitter;
                _basesour = pitemlist.player_originalitemlist[_id].Sour;
                _basecrispy = pitemlist.player_originalitemlist[_id].Crispy;
                _basefluffy = pitemlist.player_originalitemlist[_id].Fluffy;
                _basesmooth = pitemlist.player_originalitemlist[_id].Smooth;
                _basehardness = pitemlist.player_originalitemlist[_id].Hardness;
                _basejiggly = pitemlist.player_originalitemlist[_id].Jiggly;
                _basechewy = pitemlist.player_originalitemlist[_id].Chewy;
                _basepowdery = pitemlist.player_originalitemlist[_id].Powdery;
                _baseoily = pitemlist.player_originalitemlist[_id].Oily;
                _basewatery = pitemlist.player_originalitemlist[_id].Watery;
                _basegirl1_like = pitemlist.player_originalitemlist[_id].girl1_itemLike;
                _basecost = pitemlist.player_originalitemlist[_id].cost_price;
                _basesell = pitemlist.player_originalitemlist[_id].sell_price;
                _base_itemType = pitemlist.player_originalitemlist[_id].itemType.ToString();
                _base_itemType_sub = pitemlist.player_originalitemlist[_id].itemType_sub.ToString();
                _base_extreme_kaisu = pitemlist.player_originalitemlist[_id].ExtremeKaisu;
                _base_item_hyouji = pitemlist.player_originalitemlist[_id].item_Hyouji;

                for (i = 0; i < database.items[_id].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[_id].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[_id].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_originalitemlist[_id].koyu_toppingtype[i].ToString();
                }
                break;
        }

        //一回まず各スコアを初期化。とっぴんぐ・固有トッピングで、共通のリスト
        for (i = 0; i < itemslot_PitemScore.Count; i++)
        {
            itemslot_PitemScore[i] = 0;
        }


        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslot_PitemScore[count]++;
                }
                count++;
            }
        }

        //固有トッピングスロットも見る。一致する効果があれば、所持数+1
        for (i = 0; i < _koyutp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {
                //Debug.Log(key);
                if (_koyutp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log("_koyutp: " + _koyutp[i]);
                    itemslot_PitemScore[count]++;
                }
                count++;
            }
        }
    }

    void InitializeItemSlotDicts()
    {
        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslot_NouhinScore.Add(0); //納品アイテムの必要スロットパラメータ
            itemslot_PitemScore.Add(0); //選択したアイテムのスロットパラメータ
        }
    }

    //入れた数字の桁数を取得する
    public int Digit(int num)
    {
        int digit = 1;
        for (int i = num; i >= 10; i /= 10)
        {
            digit++;
        }
        return digit;
    }
}
