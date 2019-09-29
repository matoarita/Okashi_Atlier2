using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//シングルトン化しているので、ゲーム中ItemDataBaseは一個だけ。Findで探す必要もないので、itemDataBaseクラスを使うときは、その書き方にならうこと。
//できれば、ゲーム中のタイトル画面などで、一回だけ読むのがふさわしい。今は、mainで毎回読み込んでいるので、あとで修正が必要。

public class ItemRoastDataBase : SingletonMonoBehaviour<ItemRoastDataBase>
{
    private Entity_roastItemDataBase excel_roastitemdatabase;

    private ItemDataBase database;

    private int _id;
    private string roast_itemName;
    private string result_itemName;
    private int roast_itemID;
    private int result_itemID;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    //「焼く」ときの、アイテム変換データベース。

    public List<ItemRoast> roastitems = new List<ItemRoast>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        roastitems.Clear();

        excel_roastitemdatabase = Resources.Load("Excel/Entity_roastItemDataBase") as Entity_roastItemDataBase;

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        sheet_no = 0;

        count = 0;

        while (count < excel_roastitemdatabase.sheets[sheet_no].list.Count)
        {
            // 一旦代入
            _id = excel_roastitemdatabase.sheets[sheet_no].list[count].ItemID;
            roast_itemName = excel_roastitemdatabase.sheets[sheet_no].list[count].roast_itemName;
            result_itemName = excel_roastitemdatabase.sheets[sheet_no].list[count].result_itemName;

            //名前をもとに、データベースのアイテムIDに変換
            i = 0;
            while(i< database.items.Count)
            {
                if(roast_itemName == database.items[i].itemName)
                {
                    roast_itemID = i;
                    break;
                }
                i++;
            }

            i = 0;
            while (i < database.items.Count)
            {
                if (result_itemName == database.items[i].itemName)
                {
                    result_itemID = i;
                    break;
                }
                i++;
            }

            //ここでリストに追加している
            roastitems.Add(new ItemRoast(_id, roast_itemName, result_itemName, roast_itemID, result_itemID));

            ++count;
        }

    }
}