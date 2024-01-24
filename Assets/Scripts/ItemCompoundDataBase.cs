using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//シングルトン化しているので、ゲーム中ItemDataBaseは一個だけ。Findで探す必要もないので、itemDataBaseクラスを使うときは、その書き方にならうこと。
//できれば、ゲーム中のタイトル画面などで、一回だけ読むのがふさわしい。今は、mainで毎回読み込んでいるので、あとで修正が必要。

public class ItemCompoundDataBase : SingletonMonoBehaviour<ItemCompoundDataBase>
{
    private Entity_compoItemDataBase excel_compoitemdatabase;

    private int _id;
    private string cmpitem_name;
    private string cmpitem_1;
    private string cmpitem_2;
    private string cmpitem_3;
    private string cmpsubtype_1;
    private string cmpsubtype_2;
    private string cmpsubtype_3;
    private string result_item;
    private int result_kosu;

    private int cmp_kosu_1;
    private int cmp_kosu_2;
    private int cmp_kosu_3;
    private float cmp_bestkosu_1;
    private float cmp_bestkosu_2;
    private float cmp_bestkosu_3;
    private int cmp_flag;

    private int _cost_time;

    private int _srate;
    private int _renkin_bexp;

    private string _keisan_method;
    private int _comp_count;

    private string release_recipi;
    private int recipi_count;
    private int buf_kouka_on;
    private int secretFlag;

    private int hikari_make_count;

    private int i, j;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    private int all_recipicount, cullent_recipi_count;
    private float recipi_archivement_rate;

    private int hikari_make_totalcount;

    //調合データベース。3つのアイテムの組み合わせを見て、一個のアイテムを決定する。
    //アイテム番号が低いものをベースに、残りの番号との組み合わせを見る。番号は、アイテムID。

    public List<ItemCompound> compoitems = new List<ItemCompound>(); //
    //public List<ItemCompound> magic_compoitems = new List<ItemCompound>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        ResetDefaultCompoExcel();

        RecipiCount_database(); //初期値設定。
    }

    public void ResetDefaultCompoExcel()
    {
        compoitems.Clear();
        //magic_compoitems.Clear();

        excel_compoitemdatabase = Resources.Load("Excel/Entity_compoItemDataBase") as Entity_compoItemDataBase;

        sheet_no = 0;

        while (sheet_no < excel_compoitemdatabase.sheets.Count)
        {
            //通常の調合データ
            count = 0;
            while (count < excel_compoitemdatabase.sheets[sheet_no].list.Count)
            {
                SetParam();

                //ここでリストに追加している
                compoitems.Add(new ItemCompound(_id, cmpitem_name, cmpitem_1, cmpitem_2, cmpitem_3, cmpsubtype_1, cmpsubtype_2, cmpsubtype_3, result_item, result_kosu,
                    cmp_kosu_1, cmp_kosu_2, cmp_kosu_3, cmp_bestkosu_1, cmp_bestkosu_2, cmp_bestkosu_3,
                    cmp_flag, _cost_time, _srate, _renkin_bexp, _keisan_method, _comp_count, release_recipi, recipi_count, buf_kouka_on, secretFlag, hikari_make_count));

                ++count;
            }

            sheet_no++;


            //魔法調合用のDBデータ
            count = 0;
            while (count < excel_compoitemdatabase.sheets[sheet_no].list.Count)
            {
                SetParam();

                //ここでリストに追加している
                compoitems.Add(new ItemCompound(_id, cmpitem_name, cmpitem_1, cmpitem_2, cmpitem_3, cmpsubtype_1, cmpsubtype_2, cmpsubtype_3, result_item, result_kosu,
                    cmp_kosu_1, cmp_kosu_2, cmp_kosu_3, cmp_bestkosu_1, cmp_bestkosu_2, cmp_bestkosu_3,
                    cmp_flag, _cost_time, _srate, _renkin_bexp, _keisan_method, _comp_count, release_recipi, recipi_count, buf_kouka_on, secretFlag, hikari_make_count));

                //Debug.Log("CompoID: " + magic_compoitems[count].cmpitemID);

                ++count;
            }

            sheet_no++;
        }

        /*for (i = 0; i < compoitems.Count; i++)
        {
            Debug.Log(i + " " + compoitems[i].cmpitem_Name + " " + compoitems[i].cmpitemID + " cmp_flag: " + compoitems[i].cmpitem_flag);
        }*/
    }

    void SetParam()
    {
        // 一旦代入
        _id = excel_compoitemdatabase.sheets[sheet_no].list[count].ItemID;
        cmpitem_name = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemName;
        cmpitem_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemID_1;
        cmpitem_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemID_2;
        cmpitem_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitemID_3;
        cmpsubtype_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_subtype_1;
        cmpsubtype_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_subtype_2;
        cmpsubtype_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_subtype_3;
        result_item = excel_compoitemdatabase.sheets[sheet_no].list[count].result_itemID;
        result_kosu = excel_compoitemdatabase.sheets[sheet_no].list[count].result_kosu;

        cmp_kosu_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitem_kosu1;
        cmp_kosu_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitem_kosu2;
        cmp_kosu_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].cmpitem_kosu3;
        cmp_bestkosu_1 = excel_compoitemdatabase.sheets[sheet_no].list[count].best_kosu1;
        cmp_bestkosu_2 = excel_compoitemdatabase.sheets[sheet_no].list[count].best_kosu2;
        cmp_bestkosu_3 = excel_compoitemdatabase.sheets[sheet_no].list[count].best_kosu3;
        cmp_flag = excel_compoitemdatabase.sheets[sheet_no].list[count].cmp_flag;

        _cost_time = excel_compoitemdatabase.sheets[sheet_no].list[count].cost_time;

        _srate = excel_compoitemdatabase.sheets[sheet_no].list[count].success_rate;
        _renkin_bexp = excel_compoitemdatabase.sheets[sheet_no].list[count].renkin_Bexp;

        _keisan_method = excel_compoitemdatabase.sheets[sheet_no].list[count].KeisanMethod;
        _comp_count = excel_compoitemdatabase.sheets[sheet_no].list[count].comp_count;

        release_recipi = excel_compoitemdatabase.sheets[sheet_no].list[count].release_recipi;
        recipi_count = excel_compoitemdatabase.sheets[sheet_no].list[count].recipi_count;
        buf_kouka_on = excel_compoitemdatabase.sheets[sheet_no].list[count].buf_kouka_on;
        secretFlag = excel_compoitemdatabase.sheets[sheet_no].list[count].seacretFlag;

        //Excelにのってない変数
        hikari_make_count = 0;
    }

    //アイテム名を入力すると、該当するcompoIDをOnにする
    public void CompoON_compoitemdatabase(string compo_itemname)
    {
        j = 0;
        while (j < compoitems.Count)
        {
            if (compo_itemname == compoitems[j].cmpitem_Name)
            {
                compoitems[j].cmpitem_flag = 1;
                break;
            }
            j++;
        }      
    }

    //レシピの名前を入力すると、該当のレシピIDを返す。
    public int SearchCompoIDString(string itemName)
    {
        if (itemName == "Non")
        {
            return 9999;
        }
        else
        {
            i = 0;
            while (i <= compoitems.Count)
            {
                if (compoitems[i].cmpitem_Name == itemName)
                {
                    return i;
                }
                i++;
            }

            return 9999; //見つからなかった場合、9999
        }
    }

    //レシピの名前を入力すると、該当のレシピを解禁してるかどうかチェックする
    public int SearchCompoFlagString(string itemName)
    {
        if (itemName == "Non")
        {
            return 0;
        }
        else
        {
            i = 0;
            while (i <= compoitems.Count)
            {
                if (compoitems[i].cmpitem_Name == itemName)
                {
                    return compoitems[i].cmpitem_flag;
                }
                i++;
            }

            return 0; //見つからなかった場合、0
        }
    }

    //ゲーム中に表示される全てのレシピ数をカウントする.また現在のレシピ達成率も計算する。
    public void RecipiCount_database()
    {
        all_recipicount = 0;
        cullent_recipi_count = 0;

        for (i = 0; i < compoitems.Count; i++)
        {
            if (compoitems[i].cmpitem_Name != "" && compoitems[i].recipi_count == 1)
            {
                all_recipicount++;

                if (compoitems[i].cmpitem_flag >= 1 && compoitems[i].cmpitem_flag != 9999)
                {
                    cullent_recipi_count++;
                }
            }
        }

        recipi_archivement_rate = ((float)cullent_recipi_count / (float)all_recipicount) * 100.0f;

        GameMgr.game_Cullent_recipi_count = cullent_recipi_count;
        GameMgr.game_All_recipi_count = all_recipicount;
        GameMgr.game_Recipi_archivement_rate = recipi_archivement_rate;
        GameMgr.game_Exup_rate = (int)(recipi_archivement_rate / 2);
        //Debug.Log("総レシピ数: " + all_recipicount);
        //Debug.Log("現在覚えているレシピ数: " + cullent_recipi_count);
        //Debug.Log("達成率: " + recipi_archivement_rate);
    }

    //ヒカリが現在作れるお菓子の総数をカウント
    public int Hikarimake_Totalcount()
    {
        hikari_make_totalcount = 0;
        for (i = 0; i < compoitems.Count; i++)
        {
            if (compoitems[i].hikari_make_count >= 1)
            {
                hikari_make_totalcount++;
            }
        }

        return hikari_make_totalcount;
    }
}