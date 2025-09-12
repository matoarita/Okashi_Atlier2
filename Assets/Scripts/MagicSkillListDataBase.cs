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
    private string skill_compselect;
    private string skill_kosuselect;
    private string skill_addoriginal_select;
    private int skill_type;
    private int skill_category;
    private int skill_enshututype;
    private int success_rate;
    private int cost_time;
    private int status_time;
    private string skillComment_Full;
    private string skillJouken_name1;
    private int skillJouken_lv1;

    private int i;
    private int count;
    private int sheet_count;
    private int sheet_no; //アイテムが格納されているシート番号

    private int skillcount_max, _skillmax_id;
    private string _skillmax_namehyouji;

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
                skill_compselect = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_CompSelect;
                skill_kosuselect = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_KosuSelect;
                skill_addoriginal_select = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_AddOriginalSelect;
                skill_type = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_type;
                skill_category = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_category;
                skill_enshututype = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_enshututype;
                success_rate = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].success_rate;
                cost_time = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].cost_time;
                status_time = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].status_time;
                skillComment_Full = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].comment_full;
                skillJouken_name1 = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_Jouken_name1;
                skillJouken_lv1 = excel_magicskill_itemdatabase.sheets[sheet_no].list[count].skill_Jouken_lv1;

                //ここでリストに追加している
                if (sheet_no == 0)
                {
                    magicskill_lists.Add(new MagicSkillList(_id, _koyuid, skillFileName, skillName, skillName_Hyouji, skillComment, 
                        skill_day, skill_cost, skill_flag, skill_lv, skill_maxlv, skill_uselv, skill_compselect, skill_kosuselect, skill_addoriginal_select,
                        skill_type, skill_category, skill_enshututype, success_rate, cost_time, status_time, skillComment_Full,
                        skillJouken_name1, skillJouken_lv1));
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

    //スキル名をいれると、そのスキルを未習得にする。
    public void skillHyoujiDelete(string _name)
    {
        for (i = 0; i < magicskill_lists.Count; i++)
        {
            if (magicskill_lists[i].skillName == _name)
            {
                magicskill_lists[i].skillFlag = 0;
                magicskill_lists[i].skillLv = 0;
                magicskill_lists[i].skillUseLv = 0;
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

    //スキル名をいれると、そのスキルの属性を返す
    public int SearchSkillCategory(string _name)
    {
        i = 0;
        while (i < magicskill_lists.Count)
        {
            if (magicskill_lists[i].skillName == _name)
            {
                return magicskill_lists[i].skillCategory;
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

    //スキルのカテゴリー番号をいれると、そのカテゴリーのスキルの現在の習得数を返す　カテゴリービュー表示用に使う
    public int skillType_SearchAllLearnCount(int _cate)
    {
        i = 0;
        count = 0;
        while (i < magicskill_lists.Count)
        {
            if (magicskill_lists[i].skillCategory == _cate)
            {
                if(magicskill_lists[i].skillFlag >= 1)
                {
                    count++;
                }
            }
            i++;
        }

        return count;
    }

    //スキルのパラメータセット　アイテム名＋パラメータで、指定したパラムに置き換える。
    public void ReSetSkillParamString(string skillName, int param, int param2, int param3, int param4, int param5)
    {
        i = 0;
        while (i < magicskill_lists.Count)
        {
            if (magicskill_lists[i].skillName == skillName)
            {
                magicskill_lists[i].skillFlag = param;
                magicskill_lists[i].skillLv = param2;
                magicskill_lists[i].skillUseLv = param3;
                magicskill_lists[i].skill_usecount = param4;
                //param5 空
                break;
            }
            i++;
        }
    }

    //現在一番使ってる魔法の魔法表示名を返す　すべて等しいときはルミナスシュガーをかえす
    public string Count_TopUseMagicSkill()
    {
        skillcount_max = 0;
        _skillmax_id = 0;

        for (i = 0; i < magicskill_lists.Count; i++)
        {
            if (magicskill_lists[i].skillFlag == 1 && magicskill_lists[i].skillType == 1) //習得すみ　かつ　アクティブな魔法のみ
            {
                if (magicskill_lists[i].skill_usecount > skillcount_max)
                {
                    skillcount_max = magicskill_lists[i].skill_usecount;
                    _skillmax_id = i;                   
                }
            }
        }
        
        if ( skillcount_max == 0 )
        {
            _id = SearchSkillString("Luminous_Suger");
            _skillmax_namehyouji = magicskill_lists[_id].skillNameHyouji;
        }
        else
        {
            _skillmax_namehyouji = magicskill_lists[_skillmax_id].skillNameHyouji;
        }
        Debug.Log("一番使ってるスキルの使用回数: " + _skillmax_namehyouji + " " + skillcount_max);

        return _skillmax_namehyouji;
    }

    //デバッグ用　全てのスキルの表示フラグをONにする
    public void DebugAllSkillFlagKaikin()
    {
        for (i = 0; i < magicskill_lists.Count; i++)
        {
            if (magicskill_lists[i].skillFlag != 9999)
            {
                magicskill_lists[i].skillFlag = 1;
            }
        }
    }

    //デバッグ用　全てのスキルの表示フラグをOFFにする
    public void DebugAllSkillFlagOFF()
    {
        for (i = 0; i < magicskill_lists.Count; i++)
        {
            if (magicskill_lists[i].skillFlag != 9999)
            {
                magicskill_lists[i].skillFlag = 0;
            }
        }
    }
}