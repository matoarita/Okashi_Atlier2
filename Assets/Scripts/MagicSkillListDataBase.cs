using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicSkillListDataBase : SingletonMonoBehaviour<MagicSkillListDataBase>
{
    private Entity_magicSkillListDataBase excel_magicskill_itemdatabase;

    private int _id;
    private int _koyuid;
    private string skillFileName;
    private string skillName;
    private string skillName_Hyouji;
    private string skillComment; //スキルの説明文
    private int skill_day;
    private int skill_cost;
    private int skill_flag;
    private int skill_lv;
    private int skill_maxlv;
    private int skill_uselv;
    private string skill_lvselect;
    private int skill_type;
    private int skill_category;
    private int success_rate;
    private int cost_time;
    private string skillComment_Full;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    public List<MagicSkillList> magicskill_lists = new List<MagicSkillList>(); //

    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        ResetDefaultMapExcel();
        
    }

    public void ResetDefaultMapExcel()
    {
        magicskill_lists.Clear();

        excel_magicskill_itemdatabase = Resources.Load("Excel/Entity_magicSkillListDataBase") as Entity_magicSkillListDataBase;


        sheet_no = 0;

        while (sheet_no < excel_magicskill_itemdatabase.sheets.Count)
        {           
            count = 0;

            while (count < excel_magicskill_itemdatabase.sheets[sheet_no].list.Count)
            {
                // 一旦代入
                _id = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skillID;
                _koyuid = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_koyuID;
                skillFileName = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].file_name;
                skillName = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_Name;
                skillName_Hyouji = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_Name_Hyouji;
                skillComment = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].comment;
                skill_day = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_day;
                skill_cost = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_cost;
                skill_flag = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_flag;
                skill_lv = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_lv;
                skill_maxlv = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_maxlv;
                skill_uselv = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_uselv;
                skill_lvselect = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_lvSelect;
                skill_type = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_type;
                skill_category = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_category;
                success_rate = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].success_rate;
                cost_time = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].cost_time;
                skillComment_Full = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].comment_full;


                //ここでリストに追加している
                if (sheet_no == 0)
                {
                    magicskill_lists.Add(new MagicSkillList(_id, _koyuid, skillFileName, skillName, skillName_Hyouji, skillComment, 
                        skill_day, skill_cost, skill_flag, skill_lv, skill_maxlv, skill_uselv, skill_lvselect,
                        skill_type, skill_category, success_rate, cost_time, skillComment_Full));
                }
                ++count;
            }

            ++sheet_no;
        }
    }

    //スキル名をいれると、そのスキルを解禁する（習得はしないが、表示はされるようになる。）
    public void skillHyoujiKaikin(string _name)
    {
        for (i = 0; i < magicskill_lists.Count; i++)
        {
            if (magicskill_lists[i].skillName == _name)
            {
                magicskill_lists[i].skillFlag = 1;
            }
        }
    }

    //スキル名とレベルをいれると、そのスキルをそのレベルまで習得する
    public void skillLearnLv_Name(string _name, int _lv)
    {
        for (i = 0; i < magicskill_lists.Count; i++)
        {
            if (magicskill_lists[i].skillName == _name)
            {
                magicskill_lists[i].skillFlag = 1;

                if(magicskill_lists[i].skillMaxLv <= _lv)
                {
                    magicskill_lists[i].skillLv = magicskill_lists[i].skillMaxLv;
                }
                else
                {
                    magicskill_lists[i].skillLv = _lv;
                }
                
                magicskill_lists[i].skillUseLv = magicskill_lists[i].skillLv;
            }
        }
    }

    //スキル名をいれると、そのスキルの習得レベルを返す
    public int skillName_SearchLearnLevel(string _name)
    {
        i = 0;
        while (i < magicskill_lists.Count)
        {
            if (magicskill_lists[i].skillName == _name)
            { 
                return magicskill_lists[i].skillLv;
            }
            i++;
        }

        //一致しなかった場合はエラー　ひとまず0を返す
        return 0;
    }

    //スキル名をいれると、そのスキルのIDを返すメソッド
    public int SearchSkillString(string Name)
    {
        if (Name == "Non")
        {
            return 9999;
        }
        else
        {
            i = 0;
            while (i <= magicskill_lists.Count)
            {
                if (magicskill_lists[i].skillName == Name)
                {
                    return i;
                }
                i++;
            }

            return 9999; //見つからなかった場合、9999
        }
    }

    //スキルのパラメータセット　アイテム名＋パラメータで、指定したパラムに置き換える。
    public void ReSetSkillParamString(string skillName, int param, int param2, int param3)
    {
        i = 0;
        while (i < magicskill_lists.Count)
        {
            if (magicskill_lists[i].skillName == skillName)
            {
                magicskill_lists[i].skillFlag = param;
                magicskill_lists[i].skillLv = param2;
                magicskill_lists[i].skillUseLv = param3;
                break;
            }
            i++;
        }
    }

    //デバッグ用　全てのスキルの表示フラグをONにする
    public void DebugAllSkillFlagKaikin()
    {
        for (i = 0; i < magicskill_lists.Count; i++)
        {
            magicskill_lists[i].skillFlag = 1;
        }
    }
}