using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMaterial : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    private ItemMatPlaceDataBase matplace_database;

    // アイテムのデータを保持する辞書
    Dictionary<int, string> itemInfo;

    // 材料をドロップするアイテムの辞書
    Dictionary<int, float> itemDropDict;

    // ドロップする個数の辞書
    Dictionary<int, float> itemDropKosuDict;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    private int itemId, itemKosu;
    private string itemName;

    private int random;
    private int i, count, empty;
    private int index;

    private string[] _a = new string[3];
    private string[] _a_final = new string[3];
    private int[] kettei_item = new int[3];
    private int[] kettei_kosu = new int[3];

    private int mat_cost;
    private float total;

    // Use this for initialization
    void Start () {

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
    }
	
	// Update is called once per frame
	void Update () {
		
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
            //お金の消費
            PlayerStatus.player_money = PlayerStatus.player_money - mat_cost;

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


                    _a[count] = database.items[kettei_item[count]].itemNameHyouji + " を" + kettei_kosu[count] + "個　手に入れた！";

                    //アイテムの取得処理
                    pitemlist.addPlayerItem(kettei_item[count], kettei_kosu[count]);
                }


            }


            //音を鳴らす
            audioSource.PlayOneShot(sound1);

            //テキストに結果反映

            //まず初期化
            count = 0;
            empty = 0;

            for (i = 0; i < _a_final.Length; i++)
            {
                _a_final[i] = "";
            }

            //空白は無視するように調整
            for ( i = 0; i < _a.Length; i++)
            {
                if ( _a[i] == "") //空白は無視
                {
                    empty++;
                }
                else
                {
                    if ( count == 0 )
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

            if (_a_final.Length == empty)
            {
                _text.text = "特に何も見つからなかった。";
            }
            else
            {
                _text.text = _a_final[0] + _a_final[1] + _a_final[2];
            }       

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
}
