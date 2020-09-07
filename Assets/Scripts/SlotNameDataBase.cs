using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//シングルトン化しているので、ゲーム中ItemDataBaseは一個だけ。Findで探す必要もないので、itemDataBaseクラスを使うときは、その書き方にならうこと。
//できれば、ゲーム中のタイトル画面などで、一回だけ読むのがふさわしい。今は、mainで毎回読み込んでいるので、あとで修正が必要。

public class SlotNameDataBase : SingletonMonoBehaviour<SlotNameDataBase>
{
    private Entity_slotNameDataBase excel_slot_namedatabase;

    private int _id;
    private string slotName;
    private string slot_Hyouki_1;
    private string slot_Hyouki_2;
    private int slot_totalscore;
    private int slot_getgirllove;
    private int slot_money;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<SlotName> slotname_lists = new List<SlotName>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        slotname_lists.Clear();

        excel_slot_namedatabase = Resources.Load("Excel/Entity_slotNameDataBase") as Entity_slotNameDataBase;


        sheet_no = 0;

        count = 0;

        while (count < excel_slot_namedatabase.sheets[sheet_no].list.Count)
        {
            // 一旦代入
            _id = excel_slot_namedatabase.sheets[sheet_no].list[count].ItemID;
            slotName = excel_slot_namedatabase.sheets[sheet_no].list[count].slot_Name;
            slot_Hyouki_1 = excel_slot_namedatabase.sheets[sheet_no].list[count].slot_Hyouki_1;
            slot_Hyouki_2 = excel_slot_namedatabase.sheets[sheet_no].list[count].slot_Hyouki_2;
            slot_totalscore = excel_slot_namedatabase.sheets[sheet_no].list[count].slot_totalscore;
            slot_getgirllove = excel_slot_namedatabase.sheets[sheet_no].list[count].slot_getgirllove;
            slot_money = excel_slot_namedatabase.sheets[sheet_no].list[count].slot_money;

            //ここでリストに追加している
            slotname_lists.Add(new SlotName(_id, slotName, slot_Hyouki_1, slot_Hyouki_2, slot_totalscore, slot_getgirllove, slot_money));

            ++count;
        }

    }
}