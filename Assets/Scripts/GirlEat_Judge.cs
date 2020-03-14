//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlEat_Judge : MonoBehaviour {

    private GameObject canvas;

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject MoneyStatus_Panel_obj;
    private MoneyStatus_Controller moneyStatus_Controller;

    private GameObject Extremepanel_obj;
    private ExtremePanel extreme_panel;

    private Exp_Controller exp_Controller;

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private Dictionary<int, int> girlLikeCompoScore = new Dictionary<int, int>();

    private Girl1_status girl1_status;

    private GameObject hukidashiitem;   
    private Text _hukidashitext;

    private GameObject eat_hukidashiPrefab;
    private GameObject eat_hukidashiitem;
    private Text eat_hukidashitext;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count;

    private int kettei_item1; //女の子にあげるアイテムの、アイテムリスト番号。
    private int _toggle_type1; //店売りか、オリジナルのアイテムなのかの判定用

    private GameObject _slider_obj;
    private Slider _slider; //好感度バーを取得
    private int _exp;
    private int slot_girlscore, slot_money;
    private int Getlove_exp;
    private int GetMoney;
    private bool loveanim_on;

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    //判定中の女の子のアニメ用
    private bool judge_anim_on;
    private bool judge_end;
    private int judge_anim_status;

    private SpriteRenderer s;

    //時間
    private float timeOut;

    //アイテムのパラメータ
    private int _baseID;
    private string _basename;
    private string _basenameHyouji;
    private int _basequality;
    private int _basesweat;
    private int _basebitter;
    private int _basesour;
    private int _baserich;
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
    private string[] _basetp = new string[10];
    private string[] _koyutp = new string[3];

    private string _baseitemtype;
    private string _baseitemtype_sub;

    //private string _basename;
    //private int _basemp;
    //private int _baseday;
    private int _basecost;
    //private int _basesell;

    //女の子の計算用パラメータ

    private int _girlquality;
    private int[] _girlsweat;
    private int[] _girlbitter;
    private int[] _girlsour;
    private int[] _girlrich;
    private int[] _girlcrispy;
    private int[] _girlfluffy;
    private int[] _girlsmooth;
    private int[] _girlhardness;
    private int[] _girljiggly;
    private int[] _girlchewy;

    private int _girlpowdery;
    private int _girloily;
    private int _girlwatery;

    private int _set_compID;

    private string[] _girl_subtype;
    private string[] _girl_likeokashi;


    //女の子の採点用パラメータ

    private int quality_result;
    private int rich_result;
    private int sweat_result;
    private int bitter_result;
    private int sour_result;

    private int crispy_result;
    private int fluffy_result;
    private int smooth_result;
    private int hardness_result;
    private int jiggly_result;
    private int chewy_result;

    private int powdery_result;
    private int oily_result;
    private int watery_result;


    //採点用パラメータを元に、さらに計算し、最終的に出されるスコア。
    //女の子が感想をいうときにも、使う。スコアは、girl1_statusにも共有し、宴と処理を絡める。

    public int itemLike_score;

    public int quality_score;
    public int rich_score;
    public int sweat_score;
    public int bitter_score;
    public int sour_score;

    public int crispy_score;
    public int fluffy_score;
    public int smooth_score;
    public int hardness_score;
    public int jiggly_score;
    public int chewy_score;

    public int subtype1_score;
    public int subtype2_score;

    public int total_score;

    private bool dislike_flag;
    private int dislike_status;

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの点数
    List<int> itemslotScore = new List<int>();

    private GameObject effect_Prefab;
    private List<GameObject> _listEffect = new List<GameObject>();

    private GameObject heart_Prefab;
    private List<GameObject> _listHeart = new List<GameObject>();

    private GameObject hearthit_Prefab;
    private List<GameObject> _listHeartHit = new List<GameObject>();

    private GameObject hearthit2_Prefab;
    private List<GameObject> _listHeartHit2 = new List<GameObject>();

    private Vector3 heartPos;

    private int rnd, rnd2;
    private int set_id;

    // Use this for initialization
    void Start () {

        canvas = GameObject.FindWithTag("Canvas");

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");

        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //エクストリームパネルの取得
        Extremepanel_obj = GameObject.FindWithTag("ExtremePanel");
        extreme_panel = Extremepanel_obj.GetComponentInChildren<ExtremePanel>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        //お金の増減用パネルの取得
        MoneyStatus_Panel_obj = GameObject.FindWithTag("Canvas").transform.Find("MoneyStatus_panel").gameObject;
        moneyStatus_Controller = MoneyStatus_Panel_obj.GetComponent<MoneyStatus_Controller>();



        //エフェクトプレファブの取得
        effect_Prefab = (GameObject)Resources.Load("Prefabs/Particle_Heart");

        //ハートプレファブの取得
        heart_Prefab = (GameObject)Resources.Load("Prefabs/HeartUpObj");
        hearthit_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect");
        hearthit2_Prefab = (GameObject)Resources.Load("Prefabs/HeartHitEffect2");

        //Prefab内の、コンテンツ要素を取得
        eat_hukidashiPrefab = (GameObject)Resources.Load("Prefabs/Eat_hukidashi");

        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _windowtext = text_area.GetComponentInChildren<Text>();

        audioSource = GetComponent<AudioSource>();

        kettei_item1 = 0;
        _toggle_type1 = 0;

        dislike_flag = true;

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //好感度バーの取得
        _slider_obj = GameObject.FindWithTag("Girl_love_exp_bar").gameObject;
        _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();
        _slider.value = girl1_status.girl1_Love_exp;
        _exp = 0;
        Getlove_exp = 0;
        GetMoney = 0;
        loveanim_on = false;

        //バーの最大値の設定。ステージによって変わる。
        //ステージクリア用の好感度数値
        switch (GameMgr.stage_number)
        {
            case 1:

                _slider.maxValue = GameMgr.stage1_clear_love;
                break;

            case 2:

                _slider.maxValue = GameMgr.stage2_clear_love;
                break;

            case 3:

                _slider.maxValue = GameMgr.stage3_clear_love;
                break;
        }

        //アニメーション用時間
        timeOut = 5.0f;

        judge_anim_on = false;
        judge_end = false;
        judge_anim_status = 0;

        //要素数の初期化
        _girlsweat = new int[girl1_status.youso_count];
        _girlbitter = new int[girl1_status.youso_count];
        _girlsour = new int[girl1_status.youso_count];
        _girlrich = new int[girl1_status.youso_count];
        _girlcrispy = new int[girl1_status.youso_count];
        _girlfluffy = new int[girl1_status.youso_count];
        _girlsmooth = new int[girl1_status.youso_count];
        _girlhardness = new int[girl1_status.youso_count];
        _girljiggly = new int[girl1_status.youso_count];
        _girlchewy = new int[girl1_status.youso_count];

        _girl_subtype = new string[girl1_status.youso_count];
        _girl_likeokashi = new string[girl1_status.youso_count];
    }
	
	// Update is called once per frame
	void Update () {

        //好感度バーアニメーションの処理
        if (loveanim_on == true)
        {
            /*
            if (Getlove_exp > 0) //増える場合は、こっちの処理
            {

                //１ずつ増加
                ++_exp;
                ++girl1_status.girl1_Love_exp;

                //スライダにも反映
                _slider.value = girl1_status.girl1_Love_exp;

                if (_exp >= Getlove_exp)
                {
                    Getlove_exp = 0;
                    _exp = 0;
                    loveanim_on = false;

                    compound_Main.check_GirlLoveEvent_flag = false;
                }
            }*/

            if (Getlove_exp < 0)//減る場合は、こっちの処理
            {
                //好感度が0の場合、0が下限。
                if (girl1_status.girl1_Love_exp <= 0)
                {
                    Getlove_exp = 0;
                    _exp = 0;
                    loveanim_on = false;

                    compound_Main.check_GirlLoveEvent_flag = false;
                }
                else
                {
                    //１ずつ減少
                    --_exp;
                    --girl1_status.girl1_Love_exp;

                    //スライダにも反映
                    _slider.value = girl1_status.girl1_Love_exp;

                    if (_exp <= Getlove_exp)
                    {
                        Getlove_exp = 0;
                        _exp = 0;
                        loveanim_on = false;

                        compound_Main.check_GirlLoveEvent_flag = false;
                    }
                }
            }
            else
            {
                //0の場合で、アニメーションが作動した場合は、特になにもしない。
                Getlove_exp = 0;
                _exp = 0;
                loveanim_on = false;
            }
        }

        if (judge_anim_on == true)
        {
            switch(judge_anim_status)
            {
                case 0: //初期化 状態１

                   
                    girl1_status.GirlEat_Judge_on = false;

                    Extremepanel_obj.SetActive(false);
                    MoneyStatus_Panel_obj.SetActive(false);
                    text_area.SetActive(false);

                    timeOut = 1.0f;
                    judge_anim_status = 1;
                    s.sprite = girl1_status.Girl1_img_eat_start;

                    //現在の吹き出しをオフ
                    girl1_status.Girl_hukidashi_Off();

                    //食べ中の表示用吹き出しを生成
                    eat_hukidashiitem = Instantiate(eat_hukidashiPrefab, canvas.transform);
                    eat_hukidashitext = eat_hukidashiitem.GetComponentInChildren<Text>();

                    eat_hukidashitext.text = ".";

                    //カメラ寄る。
                    trans++; //transが1を超えたときに、ズームするように設定されている。

                    //intパラメーターの値を設定する.
                    maincam_animator.SetInteger("trans", trans);

                    break;

                case 1: // 状態2

                    if( timeOut <= 0.0)
                    {
                        timeOut = 1.0f;
                        judge_anim_status = 2;

                        eat_hukidashitext.text = ". .";
                        
                    }
                    break;

                case 2:

                    if (timeOut <= 0.0)
                    {
                        timeOut = 2.0f;
                        judge_anim_status = 3;

                    }
                    break;

                case 3: //アニメ終了。判定する

                    Extremepanel_obj.SetActive(true);
                    MoneyStatus_Panel_obj.SetActive(true);
                    text_area.SetActive(true);

                    //食べ中吹き出しの削除
                    if (eat_hukidashiitem != null)
                    {
                        Destroy(eat_hukidashiitem);
                    }

                    //現在の吹き出しをオン
                    girl1_status.Girl_hukidashi_On();

                    judge_anim_on = false;
                    judge_end = true;
                    judge_anim_status = 0;

                    //カメラ寄る。
                    trans--; //transが0以下のときに、ズームアウトするように設定されている。

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

    //選んだアイテムと、女の子の好みを比較するメソッド

    public void Girleat_Judge_method(int value1, int value2)
    {

        //一度、決定したアイテムのリスト番号と、タイプを取得
        kettei_item1 = value1;
        _toggle_type1 = value2;        

        //アイテムパラメータの取得

        switch (_toggle_type1)
        {
            case 0:

                _baseID = database.items[kettei_item1].itemID;
                _basename = database.items[kettei_item1].itemName;
                _basenameHyouji = database.items[kettei_item1].itemNameHyouji;
                _basequality = database.items[kettei_item1].Quality;
                _basesweat = database.items[kettei_item1].Sweat;
                _basebitter = database.items[kettei_item1].Bitter;
                _basesour = database.items[kettei_item1].Sour;
                _baserich = database.items[kettei_item1].Rich;
                _basecrispy = database.items[kettei_item1].Crispy;
                _basefluffy = database.items[kettei_item1].Fluffy;
                _basesmooth = database.items[kettei_item1].Smooth;
                _basehardness = database.items[kettei_item1].Hardness;
                _basejiggly = database.items[kettei_item1].Jiggly;
                _basechewy = database.items[kettei_item1].Chewy;
                _basepowdery = database.items[kettei_item1].Powdery;
                _baseoily = database.items[kettei_item1].Oily;
                _basewatery = database.items[kettei_item1].Watery;
                _basegirl1_like = database.items[kettei_item1].girl1_itemLike;
                _baseitemtype = database.items[kettei_item1].itemType.ToString();
                _baseitemtype_sub = database.items[kettei_item1].itemType_sub.ToString();
                _basecost = database.items[kettei_item1].cost_price;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = database.items[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = database.items[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            case 1:

                _baseID = pitemlist.player_originalitemlist[kettei_item1].itemID;
                _basename = pitemlist.player_originalitemlist[kettei_item1].itemName;
                _basenameHyouji = pitemlist.player_originalitemlist[kettei_item1].itemNameHyouji;
                _basequality = pitemlist.player_originalitemlist[kettei_item1].Quality;
                _basesweat = pitemlist.player_originalitemlist[kettei_item1].Sweat;
                _basebitter = pitemlist.player_originalitemlist[kettei_item1].Bitter;
                _basesour = pitemlist.player_originalitemlist[kettei_item1].Sour;
                _baserich = pitemlist.player_originalitemlist[kettei_item1].Rich;
                _basecrispy = pitemlist.player_originalitemlist[kettei_item1].Crispy;
                _basefluffy = pitemlist.player_originalitemlist[kettei_item1].Fluffy;
                _basesmooth = pitemlist.player_originalitemlist[kettei_item1].Smooth;
                _basehardness = pitemlist.player_originalitemlist[kettei_item1].Hardness;
                _basejiggly = pitemlist.player_originalitemlist[kettei_item1].Jiggly;
                _basechewy = pitemlist.player_originalitemlist[kettei_item1].Chewy;
                _basepowdery = pitemlist.player_originalitemlist[kettei_item1].Powdery;
                _baseoily = pitemlist.player_originalitemlist[kettei_item1].Oily;
                _basewatery = pitemlist.player_originalitemlist[kettei_item1].Watery;
                _basegirl1_like = pitemlist.player_originalitemlist[kettei_item1].girl1_itemLike;
                _baseitemtype = pitemlist.player_originalitemlist[kettei_item1].itemType.ToString();
                _baseitemtype_sub = pitemlist.player_originalitemlist[kettei_item1].itemType_sub.ToString();
                _basecost = pitemlist.player_originalitemlist[kettei_item1].cost_price;

                for (i = 0; i < database.items[kettei_item1].toppingtype.Length; i++)
                {
                    _basetp[i] = pitemlist.player_originalitemlist[kettei_item1].toppingtype[i].ToString();
                }

                for (i = 0; i < database.items[kettei_item1].koyu_toppingtype.Length; i++)
                {
                    _koyutp[i] = pitemlist.player_originalitemlist[kettei_item1].koyu_toppingtype[i].ToString();
                }

                break;

            default:
                break;
        }

        for (i = 0; i < girl1_status.youso_count; i++)
        {
            //女の子の計算パラメータを代入
            _girlquality = girl1_status.girl1_Quality;
            _girlsweat[i] = girl1_status.girl1_Sweat[i];
            _girlbitter[i] = girl1_status.girl1_Bitter[i];
            _girlsour[i] = girl1_status.girl1_Sour[i];
            _girlrich[i] = girl1_status.girl1_Rich[i];
            _girlcrispy[i] = girl1_status.girl1_Crispy[i];
            _girlfluffy[i] = girl1_status.girl1_Fluffy[i];
            _girlsmooth[i] = girl1_status.girl1_Smooth[i];
            _girlhardness[i] = girl1_status.girl1_Hardness[i];
            _girljiggly[i] = girl1_status.girl1_Jiggly[i];
            _girlchewy[i] = girl1_status.girl1_Chewy[i];
            
            _girl_subtype[i] = girl1_status.girl1_likeSubtype[i];
            _girl_likeokashi[i] = girl1_status.girl1_likeOkashi[i];
        }

        //一回だけ代入すればよい。
        _girlpowdery = girl1_status.girl1_Powdery;
        _girloily = girl1_status.girl1_Oily;
        _girlwatery = girl1_status.girl1_Watery;

        _set_compID = girl1_status.Set_compID;

        /* 古い処理、ここに入ってた。とりあえず、スクリプト下部へ避難 */



        /* 新しい処理ここから */

        //一回まず各スコアを初期化。
        for (i = 0; i < itemslotScore.Count; i++)
        {
            itemslotScore[i] = 0;
        }

        //トッピングスロットをみて、一致する効果があれば、所持数+1
        for (i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            //itemslotInfoディクショナリのキーを全て取得
            foreach (string key in itemslotInfo)
            {   
                //Debug.Log(key);
                if ( _basetp[i] == key) //キーと一致するアイテムスロットがあれば、点数を+1
                {
                    //Debug.Log(key);
                    itemslotScore[count]++;                  
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
                    itemslotScore[count]++;
                }
                count++;
            }
        }

        //確認用
        /*count = 0;
        foreach (string key in itemslotInfo)
        {
            Debug.Log(key + ": " + itemslotScore[count].ToString());
            count++;
        }*/


        switch (girl1_status.timeGirl_hungry_status)
        {
            case 0: //お腹が特に減ってない状態。

                Debug.Log("今はあまりお腹が減ってないらしい");

                _windowtext.text = "今はあまりお腹が減ってないらしい";

                compound_Main.compound_status = 0;

                break;

            case 1: //お腹が空いた状態。吹き出しがでる。


                //女の子が食べたいものを満たしているか、比較する
                //OKだったら、正解し、さらに好感度の上昇値を計算する。

                //判定アニメ起動
                judge_anim_on = true;
                judge_end = false;
                StartCoroutine("Girl_Judge_anim_co");


                break;

            case 2:

                _windowtext.text = "お菓子をあげたばかりだ。";

                compound_Main.compound_status = 0;

                break;

            default:
                break;
        }
        


        //宴用にgirl1_statusにも、点数を共有
        girl1_status.girl_final_kettei_item = kettei_item1;

        girl1_status.itemLike_score_final = itemLike_score;

        girl1_status.quality_score_final = quality_score;
        girl1_status.sweat_score_final = sweat_score;
        girl1_status.bitter_score_final = bitter_score;
        girl1_status.sour_score_final = sour_score;

        girl1_status.crispy_score_final = crispy_score;
        girl1_status.fluffy_score_final = fluffy_score;
        girl1_status.smooth_score_final = smooth_score;
        girl1_status.hardness_score_final = hardness_score;
        girl1_status.jiggly_score_final = jiggly_score;
        girl1_status.chewy_score_final = chewy_score;

        girl1_status.subtype1_score_final = subtype1_score;
        girl1_status.subtype2_score_final = subtype2_score;

        girl1_status.total_score_final = total_score;


        // => Compound_Mainに戻る


        //window_param_result_obj.SetActive(true);

        //デバッグ用　計算結果の表示
        /*window_result_text.text = "###  好みの比較　結果　###"
            + "\n" + "\n" + "基礎点数: " + itemLike_score
            + "\n" + "\n" + _basenameHyouji + " のあまさ: " + _basesweat + "\n" + " 女の子の好みの甘さ: " + _girlsweat + "\n" + "あまさの差: " + sweat_result + " 点数: " + sweat_score
            + "\n" + "\n" + _basenameHyouji + " の苦さ: " + _basebitter + "\n" + " 女の子の好みの苦さ: " + _girlbitter + "\n" + "にがさの差: " + bitter_result + " 点数: " + bitter_score
            + "\n" + "\n" + _basenameHyouji + " の酸味: " + _basesour + "\n" + " 女の子の好みの酸味: " + _girlsour + "\n" + "酸味の差: " + sour_result + " 点数: " + sour_score
            + "\n" + "\n" + "さくさく度: " + _basecrispy + "\n" + "さくさく閾値: " + _girlcrispy + "\n" + "差: " + crispy_result + " 点数: " + crispy_score
            + "\n" + "\n" + "ふわふわ度: " + _basefluffy + "\n" + "ふわふわ閾値: " + _girlfluffy + "\n" + "差: " + fluffy_result + " 点数: " + fluffy_score
            + "\n" + "\n" + "なめらか度: " + _basesmooth + "\n" + "なめらか閾値: " + _girlsmooth + "\n" + "差: " + smooth_result + " 点数: " + smooth_score
            + "\n" + "\n" + "歯ごたえ度: " + _basehardness + "\n" + "歯ごたえ閾値: " + _girlhardness + "\n" + "差: " + hardness_result + " 点数: " + hardness_score
            + "\n" + "\n" + "ぷるぷる度: " + "-"
            + "\n" + "\n" + "噛み応え度: " + "-"
            + "\n" + "\n" + girl1_status.girl1_Subtype1 + "が好き " + "点数: " + subtype1_score
            + "\n" + "\n" + girl1_status.girl1_Subtype2 + "が好き " + "点数: " + subtype2_score
            + "\n" + "\n" + "総合得点: " + total_score;*/
    }

    IEnumerator Girl_Judge_anim_co()
    {
        judge_result(); //先に判定し、マズイなどがあったら、アニメにも反映する。

        judge_score(); //点数の判定。

        while (judge_end != true)
        {
            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        Girl_reaction();
    }

    void judge_result()
    {

        count = 0;

        //Debug.Log("girl1_status.Set_Count: " + girl1_status.Set_Count);
        while (count < girl1_status.Set_Count) //セットの組み合わせの数だけ判定。最大３。どれか一つのセットが条件をクリアしていれば、正解。
        {
            //パラメータ初期化し、判定処理
            dislike_flag = true;
            dislike_status = 1;

            //
            //判定処理　パターンC
            //

            //粉っぽさなどのマイナス判定。一番強い。ここであまりに粉っぽさなどが強い場合は、問答無用で嫌われる。

            if (_basepowdery > 50)
            {
                dislike_flag = false;
                dislike_status = 3;
            }
            if (_baseoily > 50)
            {
                dislike_flag = false;
                dislike_status = 3;
            }
            if (_basewatery > 50)
            {
                dislike_flag = false;
                dislike_status = 3;
            }

            //嫌いな材料の判定
            for (i = 0; i < itemslotScore.Count; i++)
            {
                //0はNonなので、無視
                if (i != 0)
                {
                    //あげたアイテムに、女の子の嫌いな材料が使われていた！
                    if (itemslotInfo[i] == "Shishamo" && itemslotScore[i] > 0)
                    {
                        dislike_flag = false;
                        dislike_status = 4;
                    }
                }
            }


            if (dislike_status == 3 || dislike_status == 4)
            {
                //パターンCのマイナスフラグがたってしまったので、以下の判定処理を無視
                break;
            }
            else
            {
                //
                //判定処理　パターンA
                //

                //①トッピングスロットの計算
                switch (count)
                {                   

                    case 0:

                        for (i = 0; i < itemslotScore.Count; i++)
                        {

                            //0はNonなので、無視
                            if (i != 0)
                            {
                                //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                                if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet1[i])
                                {
                                    //break;
                                }
                                //一つでも満たしてないものがある場合は、NGフラグがたつ
                                else
                                {
                                    dislike_flag = false;
                                }
                            }
                        }
                        break;

                    case 1:

                        for (i = 0; i < itemslotScore.Count; i++)
                        {
                            //①トッピングスロットの計算

                            //0はNonなので、無視
                            if (i != 0)
                            {
                                //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                                if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet2[i])
                                {
                                    //break;
                                }
                                //一つでも満たしてないものがある場合は、NGフラグがたつ
                                else
                                {
                                    dislike_flag = false;
                                }
                            }
                        }
                        break;

                    case 2:

                        for (i = 0; i < itemslotScore.Count; i++)
                        {
                            //①トッピングスロットの計算

                            //0はNonなので、無視
                            if (i != 0)
                            {
                                //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                                if (itemslotScore[i] >= girl1_status.girl1_hungryScoreSet3[i])
                                {
                                    //break;
                                }
                                //一つでも満たしてないものがある場合は、NGフラグがたつ
                                else
                                {
                                    dislike_flag = false;
                                }
                            }
                        }
                        break;

                    default:

                        break;
                }

                /*
                //②味の比較。
                if (_baserich >= _girlrich[count])
                {
                    //break;
                }
                else { dislike_flag = false; }

                if (_basesweat >= _girlsweat[count])
                {
                    //break;
                }
                else { dislike_flag = false; }

                if (_basebitter >= _girlbitter[count])
                {
                    //break;
                }
                else { dislike_flag = false; }

                if (_basesour >= _girlsour[count])
                {
                    //break;
                }
                else { dislike_flag = false; }*/


                //④特定のお菓子の判定。④が一致していない場合は、③は計算するまでもなく不正解となる。
                if (_girl_likeokashi[count] == "Non") //特に指定なし
                {
                    //③お菓子の種別の計算
                    if (_girl_subtype[count] == "Non") //特に指定なし
                    {
                        //break;
                    }
                    else if (_girl_subtype[count] == _baseitemtype_sub) //お菓子の種別が一致している。
                    {
                        //break;
                    }
                    else
                    {
                        dislike_flag = false;
                    }
                }
                else if (_girl_likeokashi[count] == _basename) //お菓子の名前が一致している。
                {
                    //サブは計算せず、特定のお菓子自体が正解なら、正解
                    //break;
                }
                else
                {
                    dislike_flag = false;
                }

                //Debug.Log("_girl_likeokashi[count]: " + _girl_likeokashi[count]);
                Debug.Log("あげたお菓子: " + _basename);

                //判定 嫌いなものがなければbreak。falseだった場合、次のセットを見る。
                if (dislike_flag) { break; }

                count++;
            }

            set_id = count;

            //この時点で、嫌いなもの（吹き出しと違うもの）であれば、dislike_flagがたっている。

            //
            //判定処理　パターンB
            //

            //次に、それを新しく食べるものかどうかを判定。dislike_flagがたっているときのみ判定
            if (dislike_flag == false)
            {
                if (database.items[_baseID].First_eat == 0) //新しい食べ物の場合
                {
                    dislike_flag = true;
                    dislike_status = 2;
                }
            }
        }
    }

    void judge_score()
    {
        if (dislike_flag == true) //正解の場合のみ、味を採点する。好感度とお金に反映される。
        {
            //クッキーの場合はさくさく感など。大きいパラメータをまず見る。次に甘さ・苦さ・酸味が、女の子の好みに近いかどうか。
            //新しいお菓子の場合、少し採点甘め。

            //お菓子そのものの好み。豪華、レアなお菓子ほど高得点。
            itemLike_score = _basegirl1_like;

            //未使用。
            quality_score = 0;
            rich_score = 0;
            jiggly_score = 0;
            chewy_score = 0;

            
            //味パラメータの計算。味は、GirlLikeSetの値で、理想値としている。
            //理想の値に近いほど高得点。超えすぎてもいけない。

            rich_result = _baserich - _girlrich[set_id];
            sweat_result = _basesweat - _girlsweat[set_id];
            bitter_result = _basebitter - _girlbitter[set_id];
            sour_result = _basesour - _girlsour[set_id];

            
            //あまみ・にがみ・さんみに対して、それぞれの評価。差の値により、6段階で評価する。

            //甘味
            if (Mathf.Abs(sweat_result) == 0)
            {
                Debug.Log("甘み: Perfect!!");
                sweat_score = (int)(_basesweat * 5.0f);
            }
            else if (Mathf.Abs(sweat_result) < 5)
            {
                Debug.Log("甘み: Great!!");
                sweat_score = (int)(_basesweat * 1.0f);
            }
            else if (Mathf.Abs(sweat_result) < 15)
            {
                Debug.Log("甘み: Good!");
                sweat_score = (int)(_basesweat * 0.5f);
            }
            else if (Mathf.Abs(sweat_result) < 30)
            {
                Debug.Log("甘み: Good!");
                sweat_score = 5;
            }
            else if (Mathf.Abs(sweat_result) < 50)
            {
                Debug.Log("甘み: Normal");
                sweat_score = 2;
            }
            else if (Mathf.Abs(sweat_result) < 80)
            {
                Debug.Log("甘み: poor");
                sweat_score = -35;
            }
            else if (Mathf.Abs(sweat_result) <= 100)
            {
                Debug.Log("甘み: death..");
                sweat_score = -80;
            }
            else
            {
                Debug.Log("100を超える場合はなし");
            }
            Debug.Log("甘み点: " + sweat_score);

            //苦味
            if (Mathf.Abs(bitter_result) == 0)
            {
                Debug.Log("苦味: Perfect!!");
                bitter_score = (int)(_basebitter * 5.0f);
            }
            else if (Mathf.Abs(bitter_result) < 5)
            {
                Debug.Log("苦味: Great!!");
                bitter_score = (int)(_basebitter * 3.0f);
            }
            else if (Mathf.Abs(bitter_result) < 15)
            {
                Debug.Log("苦味: Good!");
                bitter_score = (int)(_basebitter * 1.0f);
            }
            else if (Mathf.Abs(bitter_result) < 30)
            {
                Debug.Log("苦味: Good!");
                bitter_score = 10;
            }
            else if (Mathf.Abs(bitter_result) < 50)
            {
                Debug.Log("苦味: Normal");
                bitter_score = 2;
            }
            else if (Mathf.Abs(bitter_result) < 80)
            {
                Debug.Log("苦味: poor");
                bitter_score = -35;
            }
            else if (Mathf.Abs(bitter_result) <= 100)
            {
                Debug.Log("苦味: death..");
                bitter_score = -80;
            }
            else
            {
                Debug.Log("100を超える場合はなし");
            }
            Debug.Log("苦味点: " + bitter_score);

            //酸味
            if (Mathf.Abs(sour_result) == 0)
            {
                Debug.Log("酸味: Perfect!!");
                sour_score = (int)(_basesour * 5.0f);
            }
            else if (Mathf.Abs(sour_result) < 5)
            {
                Debug.Log("酸味: Great!!");
                sour_score = (int)(_basesour * 3.0f);
            }
            else if (Mathf.Abs(sour_result) < 15)
            {
                Debug.Log("酸味: Good!");
                sour_score = (int)(_basesour * 1.0f);
            }
            else if (Mathf.Abs(sour_result) < 30)
            {
                Debug.Log("酸味: Good!");
                sour_score = 10;
            }
            else if (Mathf.Abs(sour_result) < 50)
            {
                Debug.Log("酸味: Normal");
                sour_score = 2;
            }
            else if (Mathf.Abs(sour_result) < 80)
            {
                Debug.Log("酸味: poor");
                sour_score = -35;
            }
            else if (Mathf.Abs(sour_result) <= 100)
            {
                Debug.Log("酸味: death..");
                sour_score = -80;
            }
            else
            {
                Debug.Log("100を超える場合はなし");
            }
            Debug.Log("酸味点: " + sour_score);


            //食感パラメータは、大きければ大きいほど、そのまま得点に。
            //サブジャンルごとに、比較の対象が限定される。例えば、クッキーなら、さくさく度だけを見る。
            //またジャンルごとに、どのスコアの比重が大きくなるか、補正がかかる。アイスなら甘味が大事、とか。
            switch (_baseitemtype_sub)
            {
                case "Cookie":

                    crispy_score = _basecrispy; //わかりやすく、サクサク度の数値がそのまま点数に。
                    Debug.Log("サクサク度の点: " + crispy_score);

                    break;

                case "Cake":

                    fluffy_score = _basefluffy;
                    Debug.Log("ふわふわ度の点: " + fluffy_score);

                    break;

                case "Chocolate":

                    smooth_score = _basesmooth;
                    Debug.Log("とろとろ度の点: " + smooth_score);

                    break;

                case "Chocolate_Mat":

                    smooth_score = _basesmooth;
                    Debug.Log("とろとろ度の点: " + smooth_score);

                    break;

                case "Bread":

                    crispy_score = _basecrispy;
                    Debug.Log("サクサク度の点: " + crispy_score);

                    break;

                case "IceCream":

                    smooth_score = _basesmooth;
                    Debug.Log("とろとろ度の点: " + smooth_score);

                    sweat_score *= 2;

                    break;

                default:
                    break;
            }

            //トッピングの値も計算する。クッキーに合うかわいいトッピングや、チョコバナナなどの組み合わせとかは高得点など。

            //以上、全ての点数を合計。
            total_score = itemLike_score + quality_score + sweat_score + bitter_score + sour_score 
                + crispy_score + fluffy_score
                + smooth_score + hardness_score + jiggly_score + chewy_score;

            Debug.Log("###  ###");

            Debug.Log("総合点: " + total_score);

            Debug.Log("###  ###");


            //得点に応じて、好感度・お金に補正がかかる。
        }
        else
        {  }
    }

    void Girl_reaction()
    {
        if (dislike_flag == true) //正解の場合
        {
            switch (dislike_status)
            {
                //吹き出しのお菓子をあげた場合の処理
                case 1:

                    if (girl1_status.hukidashiitem != null)
                    {
                        hukidashiitem = GameObject.FindWithTag("Hukidashi");
                        //hukidashiitem.GetComponent<TextController>().SetText("お兄ちゃん！ありがとー！！");
                        _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                        _hukidashitext.text = "お兄ちゃん！ありがとー！！";
                    }

                    //そのお菓子セットをどれだけ食べたか。スコアを追加。
                    for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
                    {
                        if (girlLikeCompo_database.girllike_composet[i].set_ID == _set_compID)
                        {
                            ++girlLikeCompo_database.girllike_composet[i].set_score;
                        }
                    }

                    //3秒ほど表示したら、お菓子の感想を言ったり、なんか褒めてくれたりする。
                    StartCoroutine("WaitCommentDesc");

                    break;

                //新しいお菓子をあげた場合の処理
                case 2:

                    if (girl1_status.hukidashiitem != null)
                    {
                        hukidashiitem = GameObject.FindWithTag("Hukidashi");
                        //hukidashiitem.GetComponent<TextController>().SetText("む！今まで食べたことがないお菓子だ！！");
                        _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();
                        _hukidashitext.text = "む！今まで食べたことがないお菓子だ！！";
                    }
                    
                    //3秒ほど表示したら、お菓子の感想を言ったり、なんか褒めてくれたりする。
                    StartCoroutine("WaitCommentNewOkashiDesc");

                    break;

                default:
                    break;
            }
        
            
            //お菓子をたべたフラグをON
            if (database.items[_baseID].First_eat == 0) //新しい食べ物の場合
            { database.items[_baseID].First_eat = 1; }

            
            //エクストリームの効果や、アイテム自体の得点をもとに、好感度とお金を計算
            LoveScoreCal();

            //お金の取得
            moneyStatus_Controller.GetMoney(GetMoney);

            //アイテムの削除
            delete_Item();

            //アニメーションをON
            loveGetPlusAnimeON();

            //エフェクト生成＋アニメ開始
            _listEffect.Add(Instantiate(effect_Prefab));
            StartCoroutine("Love_effect");

            //音を鳴らす
            audioSource.PlayOneShot(sound1);

            //テキストウィンドウの更新
            exp_Controller.GirlLikeText(Getlove_exp, GetMoney);

            //お菓子をあげたあとの状態に移行する。
            girl1_status.timeGirl_hungry_status = 2;
            girl1_status.timeOut = 5.0f;

            //キャラクタ表情変更
            s.sprite = girl1_status.Girl1_img_smile;

            //リセット＋フラグチェック
            Getlove_exp = 0;
            compound_Main.check_GirlLoveEvent_flag = false;

            //次の課題を、進行度に応じて決める。イベントお菓子を作るか、ランダムセットのままか。また、ランダムのフラグ解放もこのメソッド。
            SelectNewOkashiSet();
        }
        else //失敗の場合
        {
            if (girl1_status.hukidashiitem != null)
            {
                hukidashiitem = GameObject.FindWithTag("Hukidashi");
                _hukidashitext = hukidashiitem.transform.Find("hukidashi_Text").GetComponent<Text>();

            }

            switch (dislike_status)
            {

                case 3:

                    hukidashiitem.GetComponent<TextController>().SetText("げろげろ..。ま、まずいよ..。兄ちゃん。");
                    //_hukidashitext.text = "げろげろ..。ま、まずいよ..。兄ちゃん。";

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_verysad;

                    break;

                case 4:

                    hukidashiitem.GetComponent<TextController>().SetText("ぐええ..。コレ嫌いー！..。");
                    //_hukidashitext.text = "ぐええ..。コレ嫌いー！..。";

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_verysad_close;

                    break;

                default:

                    hukidashiitem.GetComponent<TextController>().SetText("コレ嫌いー！");
                    //_hukidashitext.text = "コレ嫌いー！";

                    //キャラクタ表情変更
                    s.sprite = girl1_status.Girl1_img_gokigen;

                    break;
            }

            

            //好感度取得
            Getlove_exp = -10;

            //アイテムの削除
            delete_Item();

            //アニメーションをON
            loveanim_on = true;

            //音を鳴らす
            audioSource.PlayOneShot(sound2);

            //テキストウィンドウの更新
            exp_Controller.GirlDisLikeText(Getlove_exp);

            //お菓子をあげたあとの状態に移行する。残り時間を、短く設定。
            girl1_status.timeGirl_hungry_status = 2;
            girl1_status.timeOut = 5.0f;           

            //リセット＋フラグチェック
            //減る場合は、update内でちぇっく
        }
        
        compound_Main.compound_status = 0;

        girl1_status.GirlEat_Judge_on = true; //またカウントが進み始める

        //チュートリアルモードがONのときの処理。ボタンを押した、フラグをたてる。
        if (GameMgr.tutorial_ON == true)
        {

            StartCoroutine("WaitForSeconds");  //1秒まって次へ              

        }
    }

    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1.0f);

        if (GameMgr.tutorial_Num == 105)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 110;
        }
        if (GameMgr.tutorial_Num == 285)
        {
            GameMgr.tutorial_Progress = true;
            GameMgr.tutorial_Num = 290;
        }
    }


    //
    //お菓子をあげたあとのコメント・感想メソッド
    //
    IEnumerator WaitCommentDesc()
    {
        yield return new WaitForSeconds(3.0f);

        if (hukidashiitem != null)
        {
            hukidashiitem.GetComponent<TextController>().SetText("腕をあげたねぇ、お兄ちゃん。");
            //_hukidashitext.text = "腕をあげたねぇ、お兄ちゃん。";
        }

        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
    }

    IEnumerator WaitCommentNewOkashiDesc()
    {
        yield return new WaitForSeconds(3.0f);

        if (hukidashiitem != null)
        {
            hukidashiitem.GetComponent<TextController>().SetText(database.items[_baseID].itemNameHyouji + "うまいぞ！");
            //_hukidashitext.text = database.items[_baseID].itemNameHyouji + "うまいぞ！";
        }

        girl1_status.timeOut += 3.0f; //少し表示時間をのばす
    }




    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for( i = 0; i < slotnamedatabase.slotname_lists.Count; i++ )
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslotScore.Add(0);
        }
            
    }

    void InitializeGirlLikeCompoScore()
    {
        girlLikeCompoScore.Clear();

        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            girlLikeCompoScore.Add(girlLikeCompo_database.girllike_composet[i].set_ID, girlLikeCompo_database.girllike_composet[i].set_score);
        }
    }

    void delete_Item()
    {
        switch (_toggle_type1)
        {
            case 0: //プレイヤーアイテムリストから選択している。

                pitemlist.deletePlayerItem(kettei_item1, 1);
                break;

            case 1: //オリジナルアイテムリストから選択している。オリジナルの場合は、一度削除用リストにIDを追加し、降順にしてから、後の削除メソッドでまとめて削除する。

                pitemlist.deleteOriginalItem(kettei_item1, 1);
                break;

            default:
                break;
        }

        //エクストリームアイテムのほうも、空にする。
        extreme_panel.deleteExtreme_Item();

    }

    void LoveScoreCal()
    {
        //①アイテムそれぞれの固有の好感度: _basegirl1_like お金:_basecost + ②スロット単体の効果による得点 + ③スロットの役の組み合わせ（チョコバナナなど）
        // - 品質補正（お菓子のHPが低いと、それに応じて、好感度・お金も下がる）ただ、かなり低くならない限りは、そこまで影響しない

        slot_girlscore = 0;
        slot_money = 0;

        //スロットの計算。該当するスロット効果があれば、それを得点にする。
        for ( i = 0; i < _basetp.Length; i++)
        {
            count = 0;
            while (count < slotnamedatabase.slotname_lists.Count)
            {
                if(_basetp[i] == slotnamedatabase.slotname_lists[count].slotName)
                {
                    //Debug.Log("スロットの効果により、好感度・お金が加算: " + slotnamedatabase.slotname_lists[count].slotName + " 好感度: " + slotnamedatabase.slotname_lists[count].slot_girlScore + " お金: " + slotnamedatabase.slotname_lists[count].slot_Money);
                    slot_girlscore += slotnamedatabase.slotname_lists[count].slot_girlScore;
                    slot_money += slotnamedatabase.slotname_lists[count].slot_Money;
                    break;
                }
                count++;
            }
        }

        //好感度取得
        Getlove_exp = _basegirl1_like + slot_girlscore;

        //お金の取得
        GetMoney = _basecost + slot_money;
    }

    IEnumerator Love_effect()
    {
        //10秒待つ
        yield return new WaitForSeconds(10);

        //初期化しておく
        for (i = 0; i < _listEffect.Count; i++)
        {
            Destroy(_listEffect[i]);
        }
        _listEffect.Clear();
    }

    void loveGetPlusAnimeON()
    {
        _listHeart.Clear();

        //ハートのインスタンスを、獲得好感度分だけ生成する。
        for (i = 0; i < Getlove_exp; i++)
        {
            _listHeart.Add(Instantiate(heart_Prefab, _slider_obj.transform));

        }

        //好感度　取得分増加
        girl1_status.girl1_Love_exp += Getlove_exp;        
    }

    public void GetHeartValue()
    {

        //スライダにも反映
        _slider.value++;
        
        //エフェクト
        _listHeartHit.Add(Instantiate(hearthit_Prefab, _slider_obj.transform.Find("Panel").gameObject.transform));
        _listHeartHit2.Add(Instantiate(hearthit2_Prefab, _slider_obj.transform.Find("Panel").gameObject.transform));

    }


    //
    //次の食べたいお菓子を決めるメソッド。
    //
    void SelectNewOkashiSet()
    {
        
        //スペシャルお菓子クエストをクリアした場合の条件分岐

        //特定のお菓子をクリアした場合は、1に戻す。
        if (girl1_status.OkashiNew_Status == 0)
        {
            girl1_status.OkashiNew_Status = 1;

            //その際、クリアしたお菓子に応じて特別報酬やイベントなどが進む。
            switch(girl1_status.OkashiQuest_ID)
            {
                case 1000:

                    Debug.Log("Level2start: ラスクを欲しがる");
                    GameMgr.OkashiQuest01_flag = true;

                    //クッキー系をOFF
                    SetGirlSetFlag(0, 0);
                    SetGirlSetFlag(1, 0);
                    //SetGirlSetFlag(1000, 0);

                    //ラスクを欲しがる
                    SetGirlSetFlag(1010, 1);

                    //イベントお菓子フラグのON/OFF。
                    girl1_status.OkashiNew_Status = 0;
                    girl1_status.OkashiQuest_ID = 1010;

                    break;

                default:
                    break;
            }
        }



        //その他、通常の状態で、何らかの条件を満たした場合

        //点数をまず初期化
        InitializeGirlLikeCompoScore();

        //①ガールセットコンポのスコアを見る。or お菓子を食べたことがあるかどうかをみる。

        //クッキー　＋　オレンジねこクッキーをあげてる場合、
        //＜自由＞クッキー系が解放。IDは、「set_compID」のほうを選択。
        //ちなみに自由課題は、「スペシャルお菓子」。クリアすると、通常よりもより大きな好感度が入る！
        if (girlLikeCompoScore[0] >= 1 && girlLikeCompoScore[1] >= 1)
        {
            //set_compID=2を解放
            if (GameMgr.OkashiQuest01_flag != true)
            {
                Debug.Log("Set2: ＜自由＞クッキー系　解放");

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1000;
                girl1_status.ResetHukidashi();
                Debug.Log("お菓子Quest1: ＜自由＞オリジナルなクッキー　を作る");
            }

        }
        else if (database.GetItemFirstEat("neko_cookie") >= 1 && database.GetItemFirstEat("orange_neko_cookie") >= 1)
        {
            //set_compID=2を解放
            if (GameMgr.OkashiQuest01_flag != true)
            {
                Debug.Log("Set2: ＜自由＞クッキー系　解放");

                //イベントお菓子フラグのON/OFF。ONになると、特定のお菓子課題をクリアするまで、ランダムでなくなる。
                girl1_status.OkashiNew_Status = 0;
                girl1_status.OkashiQuest_ID = 1000;
                girl1_status.ResetHukidashi();
                Debug.Log("お菓子Quest1: ＜自由＞オリジナルなクッキー　を作る");
            }
        }

        //②好感度で発生するイベントがあるかどうか。
        //Compound_Main内で処理している。
    }



    //指定したコンプIDのフラグをONかOFFにする。
    void SetGirlSetFlag(int _compID, int _flag)
    {
        for (i = 0; i < girlLikeCompo_database.girllike_composet.Count; i++)
        {
            if (girlLikeCompo_database.girllike_composet[i].set_ID == _compID)
            {
                girlLikeCompo_database.girllike_composet[i].set_flag = _flag;
            }
        }
    }
       
    }
