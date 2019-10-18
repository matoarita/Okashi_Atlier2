using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//シングルトン化しているので、ゲーム中ItemDataBaseは一個だけ。Findで探す必要もないので、itemDataBaseクラスを使うときは、その書き方にならうこと。
//できれば、ゲーム中のタイトル画面などで、一回だけ読むのがふさわしい。今は、mainで毎回読み込んでいるので、あとで修正が必要。

public class ItemCompoundDataBase : SingletonMonoBehaviour<ItemCompoundDataBase>
{
    private Entity_compoItemDataBase excel_compoitemdatabase;

    private int _id;
    private string cmpitem_1;
    private string cmpitem_2;
    private string cmpitem_3;
    private string cmpsubtype_1;
    private string cmpsubtype_2;
    private string cmpsubtype_3;
    private string result_item;

    private int cmp_kosu_1;
    private int cmp_kosu_2;
    private int cmp_kosu_3;
    private int cmp_flag;

    private int _cost_time;

    private int _srate;
    private int _renkin_bexp;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    //調合データベース。3つのアイテムの組み合わせを見て、一個のアイテムを決定する。
    //アイテム番号が低いものをベースに、残りの番号との組み合わせを見る。番号は、アイテムID。

    public List<ItemCompound> compoitems = new List<ItemCompound>(); //
    
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        compoitems.Clear();

        excel_compoitemdatabase = Resources.Load("Excel/Entity_compoItemDataBase") as Entity_compoItemDataBase;

        sheet_no = 0;

        count = 0;

        while (count < excel_compoitemdatabase.sheets[sheet_no].list.Count)
        {
            // 一旦代入
            _id = excel_compoitemdatabase.sheets[sheet_no].list[count].ItemID;
            cmpitem_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemID_1;
            cmpitem_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemID_2;
            cmpitem_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemID_3;
            cmpsubtype_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_subtype_1;
            cmpsubtype_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_subtype_2;
            cmpsubtype_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_subtype_3;
            result_item = excel_compoitemdatabase.sheets[sheet_no].list[count].result_itemID;

            cmp_kosu_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitem_kosu1;
            cmp_kosu_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitem_kosu2;
            cmp_kosu_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitem_kosu3;
            cmp_flag = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_flag;

            _cost_time = excel_compoitemdatabase.sheets[sheet_no].list[count].cost_time;

            _srate = excel_compoitemdatabase.sheets[sheet_no].list[count].success_rate;
            _renkin_bexp = excel_compoitemdatabase.sheets[sheet_no].list[count].renkin_Bexp;


            //ここでリストに追加している
            compoitems.Add(new ItemCompound(_id, cmpitem_1, cmpitem_2, cmpitem_3, cmpsubtype_1, cmpsubtype_2, cmpsubtype_3, result_item, cmp_kosu_1, cmp_kosu_2, cmp_kosu_3, cmp_flag, _cost_time, _srate, _renkin_bexp));

            ++count;
        }

        /*for (i = 0; i < compoitems.Count; i++)
        {
            Debug.Log(i + " " + compoitems[i].cmpitemID + " " + compoitems[i].cmpitemID_1 + " " + compoitems[i].cmpitemID_2 + " " + compoitems[i].cmpitemID_3 + " ");
        }*/
    }
}