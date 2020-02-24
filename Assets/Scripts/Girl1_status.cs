using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Girl1_status : SingletonMonoBehaviour<Girl1_status>
{
    //スロットのトッピングDB。スロット名を取得。
    private SlotNameDataBase slotnamedatabase;

    //女の子のお菓子の好きセット
    private GirlLikeSetDataBase girlLikeSet_database;

    //女の子のお菓子の好きセットの組み合わせDB
    private GirlLikeCompoDataBase girlLikeCompo_database;

    private SpriteRenderer s;

    public float timeOut;
    public int timeGirl_hungry_status; //今、お腹が空いているか、空いてないかの状態

    public bool GirlEat_Judge_on;

    private GameObject text_area;

    private GameObject hukidashiPrefab;
    private GameObject canvas;

    private GameObject hukidashiitem;
    private Text _text;

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    //女の子の好み値。この値と、選択したアイテムの数値を比較し、近いほど得点があがる。
    public int girl1_Quality;

    public int[] girl1_Rich;
    public int[] girl1_Sweat;
    public int[] girl1_Sour;
    public int[] girl1_Bitter;

    public int[] girl1_Crispy;
    public int[] girl1_Fluffy;
    public int[] girl1_Smooth;
    public int[] girl1_Hardness;
    public int[] girl1_Chewy;
    public int[] girl1_Jiggly;

    //マイナスとなる要素。これは、お菓子の種類は関係なく、この数値を超えると、嫌がられる。
    public int girl1_Powdery;
    public int girl1_Oily;
    public int girl1_Watery;

    public string[] girl1_likeSubtype;
    private string girl1_Subtype1_hyouji;

    public string[] girl1_likeOkashi;

    private string[] girllike_desc;
    private string _desc;

    public int youso_count; //GirlEat_judgeでも、パラメータ初期化の際使う。
    public int Set_Count;

    public int girl1_Love_exp; //女の子の好感度値のこと。ゲーム中に、お菓子をあげることで変動する。

    public bool girl_comment_flag; //女の子が感想をいうときに、宴をON/OFFにするフラグ
    public bool girl_comment_endflag; //感想を全て言い終えたフラグ


    //採点結果　宴と共有する用のパラメータ。採点は、GirlEat_Judgeで行っている。
    public int girl_final_kettei_item;
    public int itemLike_score_final;

    public int quality_score_final;

    public int rich_score_final;
    public int sweat_score_final;
    public int bitter_score_final;
    public int sour_score_final;

    public int crispy_score_final;
    public int fluffy_score_final;
    public int smooth_score_final;
    public int hardness_score_final;
    public int jiggly_score_final;
    public int chewy_score_final;

    public int subtype1_score_final;
    public int subtype2_score_final;

    public int total_score_final;

    private int i, j, count;
    private int index;
    private int setID;

    private float rnd;
    private int random;

    //ランダムで変化する、女の子が今食べたいお菓子のテーブル
    public List<string> girl1_hungryInfo = new List<string>();
    public List<int> girl1_hungryScoreSet1 = new List<int>();
    public List<int> girl1_hungryScoreSet2 = new List<int>();
    public List<int> girl1_hungryScoreSet3 = new List<int>();

    public List<int> girl1_hungrySet = new List<int>();                //①食べたいトッピングスロットのリスト


    //女の子の好み組み合わせセットのデータ
    private int glike_compID;
    private int set1_ID;
    private int set2_ID;
    private int set3_ID;

    private List<int> set_ID = new List<int>();

    //女の子イラストデータ
    public Sprite Girl1_img_normal;
    public Sprite Girl1_img_gokigen;
    public Sprite Girl1_img_eat_start;
    public Sprite Girl1_img_smile;

    // Use this for initialization
    void Start () {

        DontDestroyOnLoad(this);

        girl_comment_flag = false;
        girl_comment_endflag = false;

        audioSource = GetComponent<AudioSource>();

        //Prefab内の、コンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");

        //スロットの日本語表示用リストの取得
        slotnamedatabase = SlotNameDataBase.Instance.GetComponent<SlotNameDataBase>();

        //女の子の好みのお菓子セットの取得
        girlLikeSet_database = GirlLikeSetDataBase.Instance.GetComponent<GirlLikeSetDataBase>();

        //女の子の好みのお菓子セット組み合わせの取得 ステージ中、メインで使うのはコチラ
        girlLikeCompo_database = GirlLikeCompoDataBase.Instance.GetComponent<GirlLikeCompoDataBase>();

        // スロットの効果と点数データベースの初期化
        InitializeItemSlotDicts();        

        //テキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;

        //この時間ごとに、女の子は、お菓子を欲しがり始める。
        timeOut = 5.0f;
        timeGirl_hungry_status = 0;

        girl1_Love_exp = 0;

        GirlEat_Judge_on = true;

        //女の子のイラストデータ
        Girl1_img_normal = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_normal");
        Girl1_img_gokigen = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_gokigen");
        Girl1_img_smile = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_yorokobi");
        Girl1_img_eat_start = Resources.Load<Sprite>("Utage_Scenario/Texture/Character/Hikari/Hikari_eat_start");

        // *** パラメータ初期設定 ***

        youso_count = 3; //配列3のサイズ
        Set_Count = 1;   //デフォルトで１。

        //女の子の好み。初期化。甘さ・苦さ・酸味は近いものほど高得点。
        girl1_Rich = new int[youso_count];
        girl1_Sweat = new int[youso_count];
        girl1_Sour = new int[youso_count];
        girl1_Bitter = new int[youso_count];

        girl1_Crispy = new int[youso_count];
        girl1_Fluffy = new int[youso_count];
        girl1_Smooth = new int[youso_count];
        girl1_Hardness = new int[youso_count];
        girl1_Chewy = new int[youso_count];
        girl1_Jiggly = new int[youso_count];

        girl1_likeSubtype = new string[youso_count];
        girl1_likeOkashi = new string[youso_count];
        girllike_desc = new string[youso_count];

        //ステージごとに、女の子が食べたいお菓子のセットを初期化
        InitializeStageGirlHungrySet(0, 0); //とりあえず0で初期化

        // *** ここまで *** 

    }

    // Update is called once per frame
    void Update () {

        //シーン移動の際、破壊されてしまうオブジェクトは、毎回初期化
        if( canvas == null )
        {
            canvas = GameObject.FindWithTag("Canvas");
        }

        if (hukidashiPrefab == null)
        {
            //Prefab内の、コンテンツ要素を取得       
            hukidashiPrefab = (GameObject)Resources.Load("Prefabs/hukidashi");
        }

        //trueだと腹減りカウントが進む。
        if (GirlEat_Judge_on == true)
        {
            timeOut -= Time.deltaTime;
        }

        if (GameMgr.scenario_ON == true) //宴シナリオを読み中は、腹減りカウントしない。
        {

        }
        else { 

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    s = GameObject.FindWithTag("Character").GetComponent<SpriteRenderer>();

                    //一定時間たつと、女の子はお腹がへって、お菓子を欲しがる。
                    if (timeOut <= 0.0)
                    {
                        switch (timeGirl_hungry_status)
                        {
                            case 0:

                                timeGirl_hungry_status = 1; //お腹が空いた状態に切り替え。吹き出しがでる。

                                rnd = Random.Range(30.0f, 60.0f);
                                timeOut = 5.0f + rnd;
                                Girl_Hungry();
                                break;

                            case 1:

                                timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                rnd = Random.Range(1.0f, 5.0f);
                                timeOut = 2.0f + rnd;
                                Girl_Full();
                                break;

                            case 2:

                                //お菓子をあげたあとの状態。

                                timeGirl_hungry_status = 0; //お腹がいっぱいの状態に切り替え。吹き出しが消え、しばらく何もなし。

                                rnd = Random.Range(1.0f, 5.0f);
                                timeOut = 1.0f + rnd;
                                Girl_Full();

                                //キャラクタ表情変更
                                s.sprite = Girl1_img_gokigen;
                                break;

                            default:

                                timeOut = 5.0f;
                                break;
                        }



                    }
                    break;

                default:
                    break;
            }
        }
        
    }

    //女の子が食べたいものの決定。ランダムでもいいし、ストーリーによっては、一つのイベントの感じで、同じものを合格するまで出し続けてもいい。
    public void Girl_Hungry()
    {
        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);
        }

        //デフォルトで１に設定。セット組み合わせの処理にいったときに、２や３に変わる。
        Set_Count = 1;

        //テーブルの決定
        if (GameMgr.tutorial_ON == true)
        {

        }
        else
        {
            Debug.Log("通常腹減りステータスON");

            /*
            //①イベントやチュートリアル、ある好感度をこえたときの条件によって、こちらの特定のアイテムを常に出すようにする。
            //ランダムで切り替わるテーブルセット。girlLikeSet_databaseのIDをランダムで抽選している。    
            random = Random.Range(0, girlLikeSet_database.girllikeset.Count);
            //番号を入れると、女の子の好みデータベースから、値を取得し、セット。とりあえずセット１(_set_num=0)に入れて、使いまわし。
            InitializeStageGirlHungrySet(random, 0);
            
             //テキストの設定。直接しているか、セット組み合わせエクセルにかかれたキャプションのどちらかが入る。
            _desc = girllike_desc[0];
            */

            //②その他、通常のステージ攻略時は、セット組み合わせからランダムに選ぶ。
            //例えば、セット1・4の組み合わせだと、1でも4でもどっちでも正解。カリっとしたお菓子を食べたい～、のような感じ。            
            //random = Random.Range(0, girlLikeCompo_database.girllike_composet.Count);

            glike_compID = 0;
            
            set1_ID = girlLikeCompo_database.girllike_composet[glike_compID].set1;
            set2_ID = girlLikeCompo_database.girllike_composet[glike_compID].set2;
            set3_ID = girlLikeCompo_database.girllike_composet[glike_compID].set3;
 

            set_ID.Clear();

            //set_idにリストの番号をセット
            if (set1_ID != 9999)
            {
                set_ID.Add(set1_ID);
            }
            if (set2_ID != 9999)
            {
                set_ID.Add(set2_ID);
            }
            if (set3_ID != 9999)
            {
                set_ID.Add(set3_ID);
            }

            Set_Count = set_ID.Count;
            //if(Set_Count == 0) { Set_Count = 1; } //例外処理。0ということは、基本無いが、なんらかのバグで0になっていたら、1を入れておく。

            Debug.Log("Set_Count: " + Set_Count);
            
            //さきほどのset_IDをもとに、好みの値を決定する。
            for (count = 0; count < Set_Count; count++)
            {
                InitializeStageGirlHungrySet(set_ID[count], count);

            }

            //テキストの設定。セット組み合わせのときは、セット組み合わせ用のメッセージになる。
            _desc = girlLikeCompo_database.girllike_composet[glike_compID].desc;

        }            
        

        //表示用吹き出しを生成
        hukidashiitem = Instantiate(hukidashiPrefab, canvas.transform);
        _text = hukidashiitem.GetComponentInChildren<Text>();

        //音を鳴らす
        audioSource.PlayOneShot(sound1);

        //吹き出しのテキスト決定
        _text.text = _desc;


    }

    public void Girl_Full()
    {
        //前の残りの吹き出しアイテムを削除。
        if (hukidashiitem != null)
        {
            Destroy(hukidashiitem);
        }

        //まず全ての値を0に初期化
        for (i = 0; i < girl1_hungryScoreSet1.Count; i++)
        {
            girl1_hungryScoreSet1[i] = 0;
            girl1_hungryScoreSet2[i] = 0;
            girl1_hungryScoreSet3[i] = 0;
        }

        //音を鳴らす
        audioSource.PlayOneShot(sound2);
    }

    public void Girl_hukidashi_Off()
    {
        //前の残りの吹き出しアイテムを一時的にオフ
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(false);
        }
    }

    public void Girl_hukidashi_On()
    {
        //前の残りの吹き出しアイテムを一時的にオフ
        if (hukidashiitem != null)
        {
            hukidashiitem.SetActive(true);
        }
    }

    public void Girl1_Status_Init()
    {
        timeOut = 5.0f;

        timeGirl_hungry_status = 0;
    }


    void InitializeItemSlotDicts()
    {

        //Itemスクリプトに登録されているトッピングスロットのデータを取得し、各スコアをつける
        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {
            girl1_hungryInfo.Add(slotnamedatabase.slotname_lists[i].slotName);
            girl1_hungryScoreSet1.Add(0);
            girl1_hungryScoreSet2.Add(0);
            girl1_hungryScoreSet3.Add(0);
        }
    }

    public void InitializeStageGirlHungrySet(int _id, int _set_num)
    {
        //IDをセット。「compNum」の値で指定する。

        //compNumの値で指定しているので、IDに変換する。
        j = 0;
        while (j < girlLikeSet_database.girllikeset.Count)
        {
            if (_id == girlLikeSet_database.girllikeset[j].girlLike_compNum)
            {
                
                setID = j;
                break;
            }
            j++;
        }

        //初期化
        girl1_hungrySet.Clear();


        //ステージごとに、女の子が欲しがるアイテムのセット

        //セット例
        //①スロット：　オレンジ・ナッツ・ぶどう

        for (i = 0; i < slotnamedatabase.slotname_lists.Count; i++)
        {

            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[0])
            {

                if(girlLikeSet_database.girllikeset[setID].girlLike_topping[0] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[1])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[1] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[2])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[2] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[3])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[3] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }

            }
            if (slotnamedatabase.slotname_lists[i].slotName == girlLikeSet_database.girllikeset[setID].girlLike_topping[4])
            {
                if (girlLikeSet_database.girllikeset[setID].girlLike_topping[4] != "Non")
                {
                    girl1_hungrySet.Add(i);
                }
            }
        }

        
        //以下、パラメータのセッティング

        //①女の子の食べたいトッピング

        switch(_set_num)
        {
            case 0:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet1.Count; i++)
                {
                    girl1_hungryScoreSet1[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet1[girl1_hungrySet[i]]++;
                }
                break;

            case 1:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet2.Count; i++)
                {
                    girl1_hungryScoreSet2[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet2[girl1_hungrySet[i]]++;
                }
                break;

            case 2:

                //まず全ての値を0に初期化
                for (i = 0; i < girl1_hungryScoreSet3.Count; i++)
                {
                    girl1_hungryScoreSet3[i] = 0;
                }

                //トッピングの値を加算
                for (i = 0; i < girl1_hungrySet.Count; i++)
                {
                    //該当のトッピングの値を、+1する。あとで、GirlEat_Judge内の判定スロットと比較する。
                    girl1_hungryScoreSet3[girl1_hungrySet[i]]++;
                }
                break;

            default:
                break;
        }
        

        //②味のパラメータ。現状は未実装。これに足りてないと、「甘さが足りない」といったコメントをもらえる。
        girl1_Rich[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_rich;
        girl1_Sweat[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sweat;
        girl1_Sour[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_sour;
        girl1_Bitter[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_bitter;

        //③お菓子の種類：　空＝お菓子はなんでもよい　か　クッキー
        girl1_likeSubtype[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_itemSubtype;

        //④特定のお菓子が食べたいかを決定。関係性は、④＞③。
        //④が決まった場合、③は無視し、①と②だけ計算する。④が空=Nonの場合、③を計算。④も③も空の場合、お菓子の種類は関係なくなる。
        girl1_likeOkashi[_set_num] = girlLikeSet_database.girllikeset[setID].girlLike_itemName;

        //コメントをセット
        girllike_desc[_set_num] = girlLikeSet_database.girllikeset[setID].set_kansou;

        //外部から直接指定されたとき用に、_descの中身も更新。
        _desc = girllike_desc[0];

        //Debug.Log("_desc: " + _desc);
    }

}
