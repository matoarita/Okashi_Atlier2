using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Travel_Map01_Main : MonoBehaviour {

    private GameObject text_area; //
    private Text _text; //

    private WorldDataBase worlddatabase;

    private PlayerItemList pitemlist;
    private ItemDataBase database;

    private int travel_no; //選んだマップの番号　近くの森＝0 エメラルドの森＝1 ..  ワールドデータベースのリストの配列番号と一緒

    private int i, count;
    private List<int> kettei_item = new List<int>();

    private bool click_on; //押されたよ、というフラグ
    private int[] rand = new int[3];

    private string[] _get_itemtext = new string[3];

    public List<string> map_itemList = new List<string>(); //その場所で拾えるアイテムリスト。この中からランダムで選ばれる。シーンごとに、内容を変える。

    // Use this for initialization
    void Start () {

        map_itemList.Clear();

        //近くの森で拾えるアイテムリスト初期化。他のマップでも、このスクリプトを使いまわし。拾えるアイテムの中身だけ、変える。
        if (SceneManager.GetActiveScene().name == "Travel_Map01")
        {
            travel_no = 0;

            map_itemList.Add("orange");
            map_itemList.Add("grape");
            map_itemList.Add("nuts");
        }


        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //世界・街情報の取得
        worlddatabase = WorldDataBase.Instance.GetComponent<WorldDataBase>();

        text_area = GameObject.FindWithTag("Message_Window"); //調合シーン移動し、そのシーン内にあるCompundSelectというオブジェクトを検出
        _text = text_area.GetComponentInChildren<Text>();

        click_on = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        rand[0] = Random.Range(0, 3); // 個数　０～３個
        rand[1] = Random.Range(0, 3); // 個数　０～３個
        rand[2] = Random.Range(0, 3); // 個数　０～３個

        kettei_item.Clear();

        //プレイヤーのアイテム増減やステータス処理
        count = 0;

        PlayerStatus.player_money -= worlddatabase.travel_cost[travel_no];

        while (count < map_itemList.Count)
        {
            i = 0;
            while (i < database.items.Count)
            {
                if (database.items[i].itemName == map_itemList[count])
                {
                    kettei_item.Add(i); //kettei_itemというリストに、アイテムIDを入れる。stringから変換。
                    break;
                }
                ++i;
            }
            ++count;
        }

        for (i = 0; i < map_itemList.Count; i++)
        {
            pitemlist.addPlayerItem(kettei_item[i], rand[i]);
        }

        //日数・時間の増減
        PlayerStatus.player_day += 1; //とりあえず探索に一日を使う設定。

        //　以下　表示の更新　//

        count = 0;

        while (count < map_itemList.Count)
        {
            if (rand[count] <= 0)
            {
                _get_itemtext[count] = ""; //0個のときは、表示しない
            }
            else
            {
                _get_itemtext[count] = database.items[kettei_item[count]].itemNameHyouji + "を" + rand[count] + "個　拾った！" + "\n"; //0個のときは、表示しない
            }
            ++count;
        }

        _text.text = _get_itemtext[0] + _get_itemtext[1] + _get_itemtext[2] + "探索費用" + worlddatabase.travel_cost[0] + "かかった";
    }

}
