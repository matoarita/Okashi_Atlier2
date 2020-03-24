using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemMatPlaceDataBase : SingletonMonoBehaviour<ItemMatPlaceDataBase>
{
    private Entity_matplaceItemDataBase excel_matplace_itemdatabase;

    private int _id;
    private string placeFileName;
    private string placeName;
    private string placeName_Hyouji;
    private int place_cost;
    private int place_flag;
    private string drop_item1;
    private string drop_item2;
    private string drop_item3;
    private string drop_item4;
    private string drop_item5;
    private string drop_rare1;
    private string drop_rare2;
    private string drop_rare3;
    private float drop_prob1;
    private float drop_prob2;
    private float drop_prob3;
    private float drop_prob4;
    private float drop_prob5;
    private float drop_rare_prob1;
    private float drop_rare_prob2;
    private float drop_rare_prob3;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<ItemMatPlace> matplace_lists = new List<ItemMatPlace>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        matplace_lists.Clear();

        excel_matplace_itemdatabase = Resources.Load("Excel/Entity_matplaceItemDataBase") as Entity_matplaceItemDataBase;


        sheet_no = 0;

        count = 0;

        while (count < excel_matplace_itemdatabase.sheets[sheet_no].list.Count)
        {
            // 一旦代入
            _id = excel_matplace_itemdatabase.sheets[sheet_no].list[count].ItemID;
            placeFileName = excel_matplace_itemdatabase.sheets[sheet_no].list[count].file_name;
            placeName = excel_matplace_itemdatabase.sheets[sheet_no].list[count].place_Name;
            placeName_Hyouji = excel_matplace_itemdatabase.sheets[sheet_no].list[count].place_Name_Hyouji;
            place_cost = excel_matplace_itemdatabase.sheets[sheet_no].list[count].place_cost;
            place_flag = excel_matplace_itemdatabase.sheets[sheet_no].list[count].place_flag;
            drop_item1 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_item1;
            drop_item2 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_item2;
            drop_item3 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_item3;
            drop_item4 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_item4;
            drop_item5 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_item5;
            drop_rare1 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_rare1;
            drop_rare2 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_rare2;
            drop_rare3 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_rare3;
            drop_prob1 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_prob1;
            drop_prob2 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_prob2;
            drop_prob3 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_prob3;
            drop_prob4 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_prob4;
            drop_prob5 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_prob5;
            drop_rare_prob1 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_rare_prob1;
            drop_rare_prob2 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_rare_prob2;
            drop_rare_prob3 = excel_matplace_itemdatabase.sheets[sheet_no].list[count].drop_rare_prob3;


            //ここでリストに追加している
            matplace_lists.Add(new ItemMatPlace(_id, placeFileName, placeName, placeName_Hyouji, place_cost, place_flag, drop_item1, drop_item2, drop_item3, drop_item4, drop_item5, drop_rare1, drop_rare2, drop_rare3, drop_prob1, drop_prob2, drop_prob3, drop_prob4, drop_prob5, drop_rare_prob1, drop_rare_prob2, drop_rare_prob3));

            ++count;
        }

    }

    public void matPlaceKaikin(string _name)
    {
        for (i = 0; i < matplace_lists.Count; i++)
        {
            if (matplace_lists[i].placeName == _name)
            {
                matplace_lists[i].placeFlag = 1;
            }
        }
    }
}