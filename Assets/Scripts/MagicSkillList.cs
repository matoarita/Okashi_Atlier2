using UnityEngine;
using System.Collections;


[System.Serializable]//この属性を使ってインスペクター上で表示

public class MagicSkillList
{
    public int magicskillID;
    public int magicskill_koyuID; //セーブなどをする際、この固有IDと各フラグを紐づける。あとでデータが増えたり行を入れ変えたりしても、固有IDは変わることはない。
    public string skillName;
    public string skillNameHyouji;
    public string skillComment; //スキルの説明
    public int skillDay; //消費時間　調合時
    public int skillCost; //消費MP
    public int skillFlag; //スキルを解放した状態　ただし、習得はしていない（スキルリストに表示はされる）
    public int skillLv; //今習得しているLv　0だと未習得
    public int skillMaxLv; //スキルの最大LV
    public int skillUseLv; //スキルを使うLV　習得LVよりも下のLVをあえて使いたい場合などで使用
    public string skill_LvSelect; //スキルを使うとき、レベルを指定するかどうか。Nonのときは固定レベルで、今覚えているLVの最大で使用する。調合時は、レベル数値を無視する。
    public int skillType;   //パッシヴかアクティブスキルか
    public int skillCategory; //スキルの属性　基本、火、氷、光、風、心
    public int success_rate; //スキルの成功率　だが、今のとこcompoDBで決定するので使用してない
    public int cost_time;
    public string skillComment_Full; //スキルの詳細な説明

    public Sprite skillIcon_sprite;

    //ここでリスト化時に渡す引数をあてがいます   
    public MagicSkillList(int id, int koyuid, string fileName, string skill_name, string skill_name_Hyouji, string skill_comment, int skill_day, int skill_cost, int skill_flag,
        int skill_lv, int skill_maxlv, int skill_uselv, string skill_lvselect, int skill_type, int skill_category, int successRate, int costTime, string skill_comment_full)
    {
        magicskillID = id;
        magicskill_koyuID = koyuid;

        skillName = skill_name;
        skillNameHyouji = skill_name_Hyouji;
        skillComment = skill_comment;

        skillDay = skill_day;
        skillCost = skill_cost;
        skillFlag = skill_flag;
        skillLv = skill_lv;
        skillMaxLv = skill_maxlv;
        skillUseLv = skill_uselv;
        skill_LvSelect = skill_lvselect;
        skillType = skill_type;
        skillCategory = skill_category;
        skillComment_Full = skill_comment_full;

        success_rate = successRate;
        cost_time = costTime;

        skillIcon_sprite = Resources.Load<Sprite>("Sprites/Skill_Icon/" + fileName);
    }

}