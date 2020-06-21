using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private Button questListButton;
    private Button nouhinButton;

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

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの点数
    List<int> itemslot_NouhinScore = new List<int>();
    List<int> itemslot_PitemScore = new List<int>();

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

    private int _getMoney;

    private int _id;
    private int _Qid;
    private int _questID;

    private string _filename;
    private string _itemname;
    private string _itemsubtype;

    private int _kosu_default;
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
    private int _basegirl1_like;
    private int _basecost;
    private int _basesell;
    private string[] _basetp;
    private string[] _koyutp;
    private string _base_itemType;
    private string _base_itemType_sub;
    private int _base_extreme_kaisu;
    private int _base_item_hyouji;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        shopMain_obj = GameObject.FindWithTag("Shop_Main");
        shopMain = shopMain_obj.GetComponent<Shop_Main>();

        shopquestlistController_obj = canvas.transform.Find("ShopQuestList_ScrollView").gameObject;
        shopquestlistController = shopquestlistController_obj.GetComponent<ShopQuestListController>();
        back_ShopFirst_obj = shopquestlistController_obj.transform.Find("Back_ShopFirst").gameObject;
        back_ShopFirst_btn = back_ShopFirst_obj.GetComponent<Button>();

        questListButton = shopquestlistController_obj.transform.Find("QuestListButton").GetComponent<Button>();
        nouhinButton = shopquestlistController_obj.transform.Find("NouhinButton").GetComponent<Button>();

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

        //初期化
        _basetp = new string[10];
        _koyutp = new string[3];
        _tp = new string[5];

        InitializeItemSlotDicts();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

   
    public void Quest_result(int _ID)
    {
        SetInitQItem(_ID);

        nouhinOK_flag = false;

        //プレイヤーのアイテムリストを検索
        for (i = 0; i < pitemlist.playeritemlist.Count; i++)
        {
            if (pitemlist.playeritemlist[i] > 0) //持っている個数が1以上のアイテムのみ、探索。
            {                

                //まず該当アイテムがあるかどうか調べる。
                if( _itemname == database.items[i].itemName)
                {

                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.playeritemlist[i] >= _kosu_default)
                    {
                        nouhinOK_flag = true;

                        //所持アイテムを削除
                        pitemlist.deletePlayerItem(i, _kosu_default);
                    }
                    else
                    {
                        nouhinOK_flag = false;
                    }
                }
            }
        }
        /*
        if (!nouhinOK_flag) //上の探索で納品OKがtrueなら、オリジナルアイテムリストは検索しない
        {
            //次にプレイヤーのオリジナルアイテムリストを検索。player_originalitemlistは個数が1以上のものしかセットされていない。
            for (i = 0; i < pitemlist.player_originalitemlist.Count; i++)
            {

                //まず該当アイテムがあるかどうか調べる。
                if (_itemname == pitemlist.player_originalitemlist[i].itemName)
                {
                    //一致したら、さらに個数が足りてるかどうかを調べる。
                    if (pitemlist.player_originalitemlist[i].ItemKosu >= _kosu_default)
                    {
                        nouhinOK_flag = true;

                        //所持アイテムを削除
                        pitemlist.deleteOriginalItem(i, _kosu_default);
                    }
                    else
                    {
                        nouhinOK_flag = false;
                    }
                }

            }
        }*/

        if (nouhinOK_flag)
        {
            _getMoney = _buy_price * _kosu_default;

            //足りてるので、納品完了の処理
            _text.text = "報酬 " + _getMoney + "G を受け取った！" + "\n" + "ありがとう！お客さんもとても喜んでいるわ！";

            //ジャキーンみたいな音を鳴らす。
            sc.PlaySe(31);

            //所持金をプラス
            //PlayerStatus.player_money += _getMoney;
            moneyStatus_Controller.GetMoney(_getMoney); //アニメつき

            //該当のクエストを削除
            quest_database.questTakeset.RemoveAt(_ID);

            //リスト更新
            shopquestlistController.NouhinList_DrawView();


            Debug.Log("納品完了！");
        }
        else
        { 

            _text.text = "まだ数が足りてないようね..。";

            //リスト更新
            shopquestlistController.NouhinList_DrawView();

        }

        yes.SetActive(false);
        no.SetActive(false);

        questListButton.interactable = true;
        nouhinButton.interactable = true;

        back_ShopFirst_btn.interactable = true;
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }




    public void Okashi_Judge(int _ID)
    {
        pitemlistController_obj = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        SetInitQItem(_ID); //依頼アイテムのパラメータを代入

        deleteOriginalList.Clear();

        //listcount分判定する。
        for (list_count = 0; list_count < pitemlistController._listcount.Count; list_count++)
        {
            //選択したアイテムのデータをセット
            SetInitNouhinItem(pitemlistController._listcount[list_count]);

            //
            //お菓子の正解判定。①タイプ　②味　③スロットを見る。点数とかだす？
            //

            nouhinOK_flag = true; //0なら正解

            //①トッピングスロットの計算

            for (i = 0; i < itemslot_NouhinScore.Count; i++)
            {
                //0はNonなので、無視
                if (i != 0)
                {
                    //納品スコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                    if (itemslot_PitemScore[i] >= itemslot_NouhinScore[i])
                    {

                    }
                    //一つでも満たしてないものがある場合は、NGフラグがたつ
                    else
                    {
                        nouhinOK_flag = false;
                    }
                }
            }

            //②味パラメータの計算
            if (_baserich >= _rich)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basesweat >= _sweat)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basebitter >= _bitter)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basesour >= _sour)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basecrispy >= _crispy)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basefluffy >= _fluffy)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basesmooth >= _smooth)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basehardness >= _hardness)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basejiggly >= _jiggly)
            {

            }
            else { nouhinOK_flag = false; }

            if (_basechewy >= _chewy)
            {

            }
            else { nouhinOK_flag = false; }

            //④特定のお菓子の判定。④が一致していない場合は、③は計算するまでもなく不正解となる。
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
                    //不正解
                    nouhinOK_flag = false;
                }
            }
            else if (_itemname == _basename) //お菓子の名前が一致している。
            {
                //サブは計算せず、特定のお菓子自体が正解なら、正解

            }
            else
            {
                //不正解
                nouhinOK_flag = false;
            }
            
            //アイテムを削除
            switch (itemType)
            {
                case 0:

                    //もし、エクストリームパネルにセットされているお菓子を納品し、個数が０になった場合。処理が必要。
                    if (exp_Controller._temp_extreme_id == _id)
                    {
                        if (pitemlist.playeritemlist[_id] == _kosu_default)
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
                        if (pitemlist.player_originalitemlist[_id].ItemKosu == _kosu_default)
                        {
                            exp_Controller._temp_extreme_id = 9999;
                        }
                    }

                    //所持アイテムをリストに追加し、あとで降順に削除
                    deleteOriginalList.Add(_id, _kosu_default);
                    break;
            }
        }
        
        //オリジナルアイテムリストからアイテムを選んでる場合の削除処理
        if (deleteOriginalList.Count > 0)
        {
            //Debug.Log("オリジナルアイテムを降順で削除");

            //オリジナルアイテムをトッピングに使用していた場合の削除処理。削除用リストに入れた分をもとに、削除の処理を行う。
            var newTable = deleteOriginalList.OrderByDescending(value => value.Key); //降順にする

            foreach (KeyValuePair<int, int> deletePair in newTable)
            {
                pitemlist.deleteOriginalItem(deletePair.Key, deletePair.Value);
                //Debug.Log("delete_originID: " + deletePair.Key + " 個数:" + deletePair.Value);
            }
        }

        


        //0なら正解
        if (nouhinOK_flag)
        {
            _getMoney = _buy_price * _kosu_default;

            //足りてるので、納品完了の処理
            _text.text = "報酬 " + _getMoney + "G を受け取った！" + "\n" + "ありがとう！特に○○がおいしいと評判よ！";

            //ジャキーンみたいな音を鳴らす。
            sc.PlaySe(31);

            //所持金をプラス
            //PlayerStatus.player_money += _getMoney;
            moneyStatus_Controller.GetMoney(_getMoney); //アニメつき

            Debug.Log("納品完了！");
        }
        else //1だと不正解
        {
            _text.text = "う～ん。もうちょっと味を頑張ったほうがいいかも。";

            Debug.Log("納品失敗..");
        }

        //該当のクエストを削除
        quest_database.questTakeset.RemoveAt(_ID);

        //リスト更新
        shopquestlistController.NouhinList_DrawView();       

        yes.SetActive(false);
        no.SetActive(false);

        questListButton.interactable = true;
        nouhinButton.interactable = true;

        back_ShopFirst_btn.interactable = true;
        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。
    }




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

        //一回まず各スコアを初期化。
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
}
