using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GirlEat_Judge : MonoBehaviour {

    private PlayerItemList pitemlist;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private Girl1_status girl1_status;

    private GameObject window_param_result_obj;
    private Text window_result_text;

    private GameObject hukidashiitem;
    private Text _text;

    private GameObject text_area;
    private Text _windowtext;

    private int i, count;

    private int kettei_item1; //女の子にあげるアイテムの、アイテムリスト番号。
    private int _toggle_type1; //店売りか、オリジナルのアイテムなのかの判定用

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

    //アイテムのパラメータ

    private string _basenameHyouji;
    private int _basequality;
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
    private string[] _basetp;

    private string _baseitemtype;
    private string _baseitemtype_sub;

    //private string _basename;
    //private int _basemp;
    //private int _baseday;
    private int _basecost;
    //private int _basesell;

    //女の子の計算用パラメータ

    private int _girlquality;
    private int _girlsweat;
    private int _girlbitter;
    private int _girlsour;
    private int _girlcrispy;
    private int _girlfluffy;
    private int _girlsmooth;
    private int _girlhardness;
    private int _girljiggly;
    private int _girlchewy;
    private int _girlpowdery;
    private int _girloily;
    private int _girlwatery;


    //女の子の採点用パラメータ

    private int quality_result;
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

    // スロットのデータを保持するリスト。点数とセット。
    List<string> itemslotInfo = new List<string>();

    // スロットの点数
    List<int> itemslotScore = new List<int>();


    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //比較値計算結果用のパネル　デバッグ用
        window_param_result_obj = GameObject.FindWithTag("Canvas").transform.Find("Window_Param_Result").gameObject;
        window_result_text = window_param_result_obj.transform.Find("Viewport/Content/Text").gameObject.GetComponent<Text>();

        window_param_result_obj.SetActive(false);

        //windowテキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _windowtext = text_area.GetComponentInChildren<Text>();

        audioSource = GetComponent<AudioSource>();

        kettei_item1 = 0;
        _toggle_type1 = 0;

        dislike_flag = true;

        //トッピングスロットの配列
        _basetp = new string[10];

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();

        //好感度バーの取得
        _slider = GameObject.FindWithTag("Girl_love_exp_bar").GetComponent<Slider>();
        _slider.value = girl1_status.girl1_Love_exp;
        _exp = 0;
        Getlove_exp = 0;
        GetMoney = 0;
        loveanim_on = false;

        //バーの最大値の設定。ステージによって変わる。
        _slider.maxValue = 100;
    }
	
	// Update is called once per frame
	void Update () {

        //好感度バーアニメーションの処理
        if (loveanim_on == true)
        {
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
                }
            }

            else if (Getlove_exp < 0)//減る場合は、こっちの処理
            {
                //好感度が0の場合、0が下限。
                if (girl1_status.girl1_Love_exp <= 0)
                {
                    Getlove_exp = 0;
                    _exp = 0;
                    loveanim_on = false;
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

                _basenameHyouji = database.items[kettei_item1].itemNameHyouji;
                _basequality = database.items[kettei_item1].Quality;
                _basesweat = database.items[kettei_item1].Sweat;
                _basebitter = database.items[kettei_item1].Bitter;
                _basesour = database.items[kettei_item1].Sour;
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

                break;

            case 1:

                _basenameHyouji = pitemlist.player_originalitemlist[kettei_item1].itemNameHyouji;
                _basequality = pitemlist.player_originalitemlist[kettei_item1].Quality;
                _basesweat = pitemlist.player_originalitemlist[kettei_item1].Sweat;
                _basebitter = pitemlist.player_originalitemlist[kettei_item1].Bitter;
                _basesour = pitemlist.player_originalitemlist[kettei_item1].Sour;
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

                break;

            default:
                break;
        }

        //女の子の計算パラメータを代入
        _girlquality = girl1_status.girl1_Quality;
        _girlsweat = girl1_status.girl1_Sweat;
        _girlbitter = girl1_status.girl1_Bitter;
        _girlsour = girl1_status.girl1_Sour;
        _girlcrispy = girl1_status.girl1_Crispy;
        _girlfluffy = girl1_status.girl1_Fluffy;
        _girlsmooth = girl1_status.girl1_Smooth;
        _girlhardness = girl1_status.girl1_Hardness;
        _girljiggly = girl1_status.girl1_Jiggly;
        _girlchewy = girl1_status.girl1_Chewy;
        _girlpowdery = girl1_status.girl1_Powdery;
        _girloily = girl1_status.girl1_Oily;
        _girlwatery = girl1_status.girl1_Watery;


        /* 古い処理、ここに入ってた。とりあえず、スクリプト下部へ避難 */



        /* 新しい処理ここから */

        //一回まず各スコアを初期化。
        for (i = 0; i < itemslotScore.Count; i++)
        {
            itemslotScore[i] = 0;
        }

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

                break;

            case 1: //お腹が空いた状態。吹き出しがでる。

                
                //女の子が食べたいものを満たしているか、比較する
                //OKだったら、正解し、さらに好感度の上昇値を計算する。

                dislike_flag = true;
                for (i = 0; i < itemslotScore.Count; i++)
                {
                    //0はNonなので、無視
                    if (i != 0)
                    {
                        //女の子のスコアより、生成したアイテムのスロットのスコアが大きい場合は、正解
                        if (itemslotScore[i] >= girl1_status.girl1_hungryScore[i])
                        {

                        }
                        //一つでも満たしてないものがある場合は、NGフラグがたつ
                        else
                        {
                            dislike_flag = false;
                        }
                    }
                }

                if (dislike_flag == true) //正解の場合
                {

                    hukidashiitem = GameObject.FindWithTag("Hukidashi");
                    _text = hukidashiitem.GetComponentInChildren<Text>();

                    _text.text = "お兄ちゃん！ありがとー！！";

                    //エクストリームの効果や、アイテム自体の得点をもとに、好感度とお金を計算
                    LoveScoreCal();

                    Debug.Log("GetMoney: " + GetMoney);
                    PlayerStatus.player_money += GetMoney;

                    //アイテムの削除
                    delete_Item();                    

                    //アニメーションをON
                    loveanim_on = true;

                    //音を鳴らす
                    audioSource.PlayOneShot(sound1);

                    //テキストウィンドウの更新
                    _windowtext.text = "お菓子をあげた！" + "\n" + "好感度が" + Getlove_exp + "アップ！　" + "お金を" + GetMoney + "G ゲットした！";

                    //お菓子をあげたあとの状態に移行する。
                    girl1_status.timeGirl_hungry_status = 2;
                    girl1_status.timeOut = 5.0f;
                }
                else //失敗の場合
                {
                    hukidashiitem = GameObject.FindWithTag("Hukidashi");
                    _text = hukidashiitem.GetComponentInChildren<Text>();

                    _text.text = "コレ嫌いー！";

                    //好感度取得
                    Getlove_exp = -10;

                    //アイテムの削除
                    delete_Item();

                    //アニメーションをON
                    loveanim_on = true;

                    //音を鳴らす
                    audioSource.PlayOneShot(sound2);

                    //テキストウィンドウの更新
                    _windowtext.text = "お菓子をあげた！" + "\n" + "好感度が" + Math.Abs(Getlove_exp) + "下がった..。";

                    //お菓子をあげたあとの状態に移行する。残り時間を、短く設定。
                    girl1_status.timeGirl_hungry_status = 2;
                    girl1_status.timeOut = 5.0f;
                }

                break;

            case 2:

                _windowtext.text = "お菓子をあげたばかりだ。";

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

    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for( i = 0; i < slotnamedatabase.slotname_lists.Count; i++ )
        {
            itemslotInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            itemslotScore.Add(0);
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
    }

    void LoveScoreCal()
    {
        //①アイテムそれぞれの固有の好感度: _basegirl1_like お金:_basecost + ②スロット単体の効果による得点 + ③スロットの役の組み合わせ（チョコバナナなど）

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

        /*
            Debug.Log("###  好みの比較　結果　###");

            //味の濃さ（さっぱりかコクがあるか）の比較。
            quality_result = _basequality - _girlquality;
            

            //甘さ・苦さ・酸味の比較。

            sweat_result = _basesweat - _girlsweat;
            Debug.Log(_basenameHyouji + " のあまさ: " + _basesweat + " 女の子の好みの甘さ: " + _girlsweat);
            Debug.Log("あまさの差: " + sweat_result);

            bitter_result = _basebitter - _girlbitter;
            Debug.Log(_basenameHyouji + " の苦さ: " + _basebitter + " 女の子の好みの苦さ: " + _girlbitter);
            Debug.Log("にがさの差: " + bitter_result);

            sour_result = _basesour - _girlsour;
            Debug.Log(_basenameHyouji + " の酸味: " + _basesour + " 女の子の好みの酸味: " + _girlsour);
            Debug.Log("酸味の差: " + sour_result);


            //食感の比較。事前に値に取得。

            crispy_result = _basecrispy - _girlcrispy;
            fluffy_result = _basefluffy - _girlfluffy;
            smooth_result = _basesmooth - _girlsmooth;
            hardness_result = _basehardness - _girlhardness;
            jiggly_result = _basejiggly - _girljiggly;
            chewy_result = _basechewy - _girlchewy;


            //マイナス要素の比較。粉っぽいか、油っぽいか、水っぽい。女の子の許容値を超えると、他のスコアにマイナス影響をあたえる。
            powdery_result = _basepowdery - _girlpowdery;
            oily_result = _baseoily - _girloily;
            watery_result = _basewatery - _girlwatery;




            //  ###  採点メソッド  ###  //

            itemLike_score = 0;

            quality_score = 0;
            sweat_score = 0;
            bitter_score = 0;
            sour_score = 0;

            crispy_score = 0;
            fluffy_score = 0;
            smooth_score = 0;
            hardness_score = 0;
            jiggly_score = 0;
            chewy_score = 0;

            subtype1_score = 0;
            subtype2_score = 0;

            total_score = 0;



            //
            //各スコアの計算
            //


            //アイテムごとに固有の満足度があるので、その値を取得。
            itemLike_score = _basegirl1_like;
            Debug.Log("基礎点数: " + itemLike_score);


            //あまみ・にがみ・さんみに対して、それぞれの評価。差の値により、6段階で評価する。

            //甘味
            if (Mathf.Abs(sweat_result) == 0)
            {
                Debug.Log("甘み: Perfect!!");
                sweat_score = 100;
            }
            else if (Mathf.Abs(sweat_result) < 5)
            {
                Debug.Log("甘み: Great!!");
                sweat_score = 30;
            }
            else if (Mathf.Abs(sweat_result) < 15)
            {
                Debug.Log("甘み: Good!");
                sweat_score = 10;
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
                bitter_score = 100;
            }
            else if (Mathf.Abs(bitter_result) < 5)
            {
                Debug.Log("苦味: Great!!");
                bitter_score = 30;
            }
            else if (Mathf.Abs(bitter_result) < 15)
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
                sour_score = 100;
            }
            else if (Mathf.Abs(sour_result) < 5)
            {
                Debug.Log("酸味: Great!!");
                sour_score = 30;
            }
            else if (Mathf.Abs(sour_result) < 15)
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
            */

        /*
        //食感の計算。基本的に、女の子の閾値を超えた分が、加算される。
        if( crispy_result >= 0 )
        {
            crispy_score = Mathf.Abs(crispy_result);
        }
        if (fluffy_result >= 0)
        {
            fluffy_score = Mathf.Abs(fluffy_result);
        }
        if (smooth_result >= 0)
        {
            smooth_score = Mathf.Abs(smooth_result);
        }
        if (hardness_result >= 0)
        {
            hardness_score = Mathf.Abs(hardness_result);
        }*/
        /*if (jiggly_result >= 0) 未使用
        {
            jiggly_score = Mathf.Abs(jiggly_result);
        }
        if (chewy_result >= 0) 未使用
        {
            chewy_score = Mathf.Abs(chewy_result);
        }*/



        /*
        //サブジャンルごとに、比較の対象が限定される。例えば、クッキーなら、さくさく度だけを見る。
        switch (_baseitemtype_sub)
        {
            case "Cookie":

                Debug.Log(database.items[kettei_item1].itemNameHyouji + " のさくさく度: " + database.items[kettei_item1].Crispy + " 女の子の好みのさくさく度: " + girl1_status.girl1_crispy);
                Debug.Log("サクサク度の差: " + crispy_result);

                if (crispy_result >= 0) //好みのしきい値を超えた
                {
                    crispy_score = crispy_result; //わかりやすく、サクサク度の数値がそのまま点数に。
                    Debug.Log("サクサク度の点: " + crispy_score);
                }
                break;

            case "Cake":

                Debug.Log(database.items[final_kettei_item1].itemNameHyouji + " のふわふわ度: " + database.items[final_kettei_item1].Fluffy + " 女の子の好みのふわふわ度: " + girl1_status.girl1_fluffy);
                Debug.Log("ふわふわ度の差: " + fluffy_result);

                if (fluffy_result >= 0) //好みのしきい値を超えた
                {
                    fluffy_score = fluffy_result;
                    Debug.Log("ふわふわ度の点: " + fluffy_score);
                }
                break;

            case "Chocolate":

                Debug.Log(database.items[final_kettei_item1].itemNameHyouji + " のとろとろ度: " + database.items[final_kettei_item1].Smooth + " 女の子の好みのとろとろ度: " + girl1_status.girl1_smooth);
                Debug.Log("とろとろ度の差: " + smooth_result);

                if (smooth_result >= 0) //好みのしきい値を超えた
                {
                    smooth_score = smooth_result;
                    Debug.Log("とろとろ度の点: " + smooth_score);
                }
                break;

            default:
                break;
        }
        */

        /*
        //クッキーが好き、ケーキが好きなどの、サブタイプの採点。一致していれば、加点。
        if (_baseitemtype_sub == girl1_status.girl1_Subtype1)
        {
            subtype1_score = girl1_status.girl1_Subtype1_p;
            Debug.Log(girl1_status.girl1_Subtype1 + "が好き: " + subtype1_score);
        }

        if (_baseitemtype_sub == girl1_status.girl1_Subtype2)
        {
            subtype2_score = girl1_status.girl1_Subtype2_p;
            Debug.Log(girl1_status.girl1_Subtype2 + "が好き: " + subtype2_score);
        }

        //トッピングの採点。複雑なので、一度置き。


        //以上、全ての点数を合計。
        total_score = itemLike_score + quality_score + sweat_score + bitter_score + sour_score + crispy_score + fluffy_score 
            + smooth_score + hardness_score + jiggly_score + chewy_score + subtype1_score + subtype2_score;

        Debug.Log("総合点: " + total_score);

        Debug.Log("###  ###");
        */
    
    }
