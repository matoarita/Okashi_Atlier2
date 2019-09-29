using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// アイテムデータベースに登録した画像を、キー入力で順番に読むこむ処理。デバッグ用。
// check_itemdata_numを増減させて、itemdatabaseのitems[]のリストの数値を変化させる。

public class Check_ItemDataBase : MonoBehaviour {

    public int check_itemdata_num; //アイテムIDをそのまま指定している。

    private List<int> check_IDlist = new List<int>();

    //private GameObject Itemdatabase_object; // publicにして、ItemDataBaseゲームオブジェクトを、ヒエラルキー上でアタッチしても良い。今回は、FindWithTagで探し出して取得している。
    private ItemDataBase database;
    private int database_max;

    private int i,j;
    private int count_kisuu;
    private int count;

    // Use this for initialization
    void Start () {

        
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();
        check_itemdata_num = 0;

        count = 0;
        count_kisuu = 0;

        for (i = 0; i < database.sheet_topendID.Count; i++)
        {
            //Debug.Log("  " + database.sheet_topendID[i]);
            
            if (count_kisuu % 2 == 1)
            {
                for ( j = 0; j < database.sheet_topendID[i] - database.sheet_topendID[i-1] + 1; j++ )
                {
                    check_IDlist.Add(database.sheet_topendID[i - 1] + j);
                }
            }

            ++count_kisuu;
        }

        database_max = check_IDlist.Count - 1;
    }
	
	// Update is called once per frame
	void Update () {


        // ← キーが押された時
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           if ( count > 0)
            {
                count--;

                check_itemdata_num = check_IDlist[count];
            }
             
        }

        // → キーが押された時
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (count < database_max)
            {
                count++;

                check_itemdata_num = check_IDlist[count];
            }

        }
    }
}
