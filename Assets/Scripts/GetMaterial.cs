using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMaterial : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private PlayerItemList pitemlist;

    private ItemDataBase database;

    // アイテムのデータを保持する辞書
    Dictionary<int, string> itemInfo;

    // 材料をドロップするアイテムの辞書
    Dictionary<int, float> itemDropDict;

    // ドロップする個数の辞書
    Dictionary<int, float> itemDropKosuDict;

    private int itemId, itemKosu;
    private string itemName;

    private int random;
    private int i, count;

    private string[] _a = new string[3];
    private int[] kettei_item = new int[3];
    private int[] kettei_kosu = new int[3];

    // Use this for initialization
    void Start () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        // 入手できるアイテムのデータベースの初期化後に、エクセルで設定できるようにする。
        InitializeDicts();

        //テキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetRandomMaterials() //材料を３つランダムでゲットする処理
    {

        for (count = 0; count < 3; count++) //3回繰り返す
        {
            // ドロップアイテムの抽選
            itemId = Choose();
            itemName = itemInfo[itemId];

            //  個数の抽選
            itemKosu = ChooseKosu();
            kettei_kosu[count] = itemKosu;

            //今回のゲームでは、必ず何かアイテムが入手できる。

            if (itemId == 0) //id = 0 のときは、何もなし
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
            }

        }
        

        //アイテムの取得処理
        pitemlist.addPlayerItem(kettei_item[0], kettei_kosu[0]);
        pitemlist.addPlayerItem(kettei_item[1], kettei_kosu[1]);
        pitemlist.addPlayerItem(kettei_item[2], kettei_kosu[2]);

        //テキストに結果反映
        _text.text = _a[0] + "\n" + _a[1] + "\n" + _a[2];

    }

    void InitializeDicts()
    {
        itemInfo = new Dictionary<int, string>();
        itemInfo.Add(0, "なし");
        itemInfo.Add(1, "orange"); //アイテムデータベースに登録されているアイテム名と同じにする
        itemInfo.Add(2, "grape");
        itemInfo.Add(3, "nuts");
        itemInfo.Add(4, "竜の翼");
        itemInfo.Add(5, "竜の逆鱗");
        itemInfo.Add(6, "竜の紅玉");

        itemDropDict = new Dictionary<int, float>();
        //itemDropDict.Add(0, 60.0f); //0 なしが　60%
        itemDropDict.Add(1, 25.0f); //1 が　25%
        itemDropDict.Add(2, 12.0f); //2 が　12%
        itemDropDict.Add(3, 3.0f);  //3 が　3%

        itemDropKosuDict = new Dictionary<int, float>();
        itemDropKosuDict.Add(1, 60.0f); //1個　60%
        itemDropKosuDict.Add(2, 25.0f); //2個　25%
        itemDropKosuDict.Add(3, 12.0f); //3個　12%
    }

    int Choose()
    {
        // 確率の合計値を格納
        float total = 0;

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
        float total = 0;

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
