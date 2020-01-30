﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMaterial : MonoBehaviour {

    private GameObject canvas;

    private GameObject text_area;
    private Text _text;

    private GameObject tansaku_panel;
    private Button tansaku_yes;
    private Button tansaku_no;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private ItemMatPlaceDataBase matplace_database;

    private FadeImage slot_view_fade;

    // アイテムのデータを保持する辞書
    Dictionary<int, string> itemInfo;

    // 材料をドロップするアイテムの辞書
    Dictionary<int, float> itemDropDict;

    // ドロップする個数の辞書
    Dictionary<int, float> itemDropKosuDict;

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    private int itemId, itemKosu;
    private string itemName;

    private int random;
    private int i, count, empty;
    private int index;

    private int cullent_total_mat;

    private string[] _a = new string[3];
    private string[] _a_final = new string[3];
    private string _a_zairyomax;
    private int[] kettei_item = new int[3];
    private int[] kettei_kosu = new int[3];

    private int mat_cost;
    private float total;

    private bool mat_anim_on;
    private bool mat_anim_end;
    private int mat_anim_status;
    private float timeOut;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();        

        //テキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //材料採取のための、消費コスト
        mat_cost = 0;

        audioSource = GetComponent<AudioSource>();

        mat_anim_status = 0;
        mat_anim_on = false;
        mat_anim_end = false;

        cullent_total_mat = 0;

        slot_view_fade = canvas.transform.Find("GetMatPlace_Panel/Slot_View/Image").gameObject.GetComponent<FadeImage>();

        tansaku_panel = canvas.transform.Find("GetMatPlace_Panel/Slot_View/Tansaku_panel").gameObject;
        tansaku_yes = tansaku_panel.transform.Find("Yes").GetComponent<Button>();
        tansaku_no = tansaku_panel.transform.Find("No").GetComponent<Button>();
    }
	
	// Update is called once per frame
	void Update () {

        if (mat_anim_on == true)
        {
            switch (mat_anim_status)
            {
                case 0: //初期化 状態１

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

        //お金のチェック       
        if (PlayerStatus.player_money < mat_cost)
        {
            _text.text = "お金が足らない。";
        }
        else
        {
            //カゴの大きさのチェック。取った数の総量がMAXを超えると、これ以上取れない。
            if (PlayerStatus.player_zairyobox >= cullent_total_mat)
            { 

              //お金の消費
                PlayerStatus.player_money = PlayerStatus.player_money - mat_cost;

                //ウェイトアニメ
                mat_anim_on = true;
                mat_anim_end = false;
                slot_view_fade.FadeImageOff(); //ビュー画面を暗くフェード

                tansaku_panel.SetActive(false);
                //tansaku_yes.interactable = false;
                //tansaku_no.interactable = false;
                StartCoroutine("Mat_Judge_anim_co");

            }
            else
            {
                _text.text = "もうカゴがいっぱいだよ～。";
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
        //tansaku_yes.interactable = true;
        //tansaku_no.interactable = true;
        slot_view_fade.FadeImageOn(); //ビュー画面を戻す

        mat_result();
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
            }           
        }


        

        //テキストに結果反映

        //まず初期化
        count = 0;
        empty = 0;

        for (i = 0; i < _a_final.Length; i++)
        {
            _a_final[i] = "";
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

        if (_a_final.Length == empty) //何もなかったとき
        {
            _text.text = "特に何も見つからなかった。";

            //音を鳴らす
            audioSource.PlayOneShot(sound2);
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
            }

            _text.text = _a_final[0] + _a_final[1] + _a_final[2] + _a_zairyomax;

            //音を鳴らす
            audioSource.PlayOneShot(sound1);
        }
    }


    void InitializeDicts()
    {
        itemInfo = new Dictionary<int, string>();

        itemInfo.Add(0, matplace_database.matplace_lists[index].dropItem1); //アイテムデータベースに登録されているアイテム名と同じにする
        itemInfo.Add(1, matplace_database.matplace_lists[index].dropItem2); 
        itemInfo.Add(2, matplace_database.matplace_lists[index].dropItem3);
        itemInfo.Add(3, matplace_database.matplace_lists[index].dropItem4);
        itemInfo.Add(4, matplace_database.matplace_lists[index].dropItem5);
        itemInfo.Add(5, matplace_database.matplace_lists[index].dropRare1);
        itemInfo.Add(6, matplace_database.matplace_lists[index].dropRare2);

        itemDropDict = new Dictionary<int, float>();
        itemDropDict.Add(0, matplace_database.matplace_lists[index].dropProb1); //こっちは確率テーブル
        itemDropDict.Add(1, matplace_database.matplace_lists[index].dropProb2); 
        itemDropDict.Add(2, matplace_database.matplace_lists[index].dropProb3); 
        itemDropDict.Add(3, matplace_database.matplace_lists[index].dropProb4);  
        itemDropDict.Add(4, matplace_database.matplace_lists[index].dropProb5); 
        itemDropDict.Add(5, matplace_database.matplace_lists[index].dropRareProb1); 
        itemDropDict.Add(6, matplace_database.matplace_lists[index].dropRareProb2); 

        itemDropKosuDict = new Dictionary<int, float>();
        itemDropKosuDict.Add(1, 60.0f); //1個　60%
        itemDropKosuDict.Add(2, 25.0f); //2個　25%
        itemDropKosuDict.Add(3, 12.0f); //3個　12%
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
        float randomPoint = Random.value * total;

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
        float randomPoint = Random.value * total;

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

    public void SetInit()
    {
        cullent_total_mat = 0;
        _a_zairyomax = "";
    }
}
