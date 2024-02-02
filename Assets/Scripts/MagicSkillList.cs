using UnityEngine;
using System.Collections;


[System.Serializable]//この属性を使ってインスペクター上で表示

public class MagicSkillList
{
    public int magicskillID;
    public string skillName;
    public string skillNameHyouji;
    public string skillComment; //スキルの説明
    public int skillDay; //消費時間　調合時
    public int skillCost; //消費MP
    public int skillFlag; //スキルを解放した状態　ただし、習得はしていない（スキルリストに表示はされる）
    public int skillLv; //今習得しているLv　0だと未習得
    public int skillMaxLv; //スキルの最大LV
    public int skillUseLv; //スキルを使うLV　習得LVよりも下のLVをあえて使いたい場合などで使用
    public int skillType;   //パッシヴかアクティブスキルか
    public int skillCategory; //スキルの属性　基本、火、氷、光、風、心
    public int success_rate;
    public string skillComment_Full; //スキルの詳細な説明

    public Sprite skillIcon_sprite;

    //ここでリスト化時に渡す引数をあてがいます   
    public MagicSkillList(int id, string fileName, string skill_name, string skill_name_Hyouji, string skill_comment, int skill_day, int skill_cost, int skill_flag,
        int skill_lv, int skill_maxlv, int skill_uselv, int skill_type, int skill_category, int successRate, string skill_comment_full)
    {
        magicskillID = id;

        skillName = skill_name;
        skillNameHyouji = skill_name_Hyouji;
        skillComment = skill_comment;

        skillDay = skill_day;
        skillCost = skill_cost;
        skillFlag = skill_flag;
        skillLv = skill_lv;
        skillMaxLv = skill_maxlv;
        skillUseLv = skill_uselv;
        skillType = skill_type;
        skillCategory = skill_category;
        skillComment_Full = skill_comment_full;

        success_rate = successRate;

        skillIcon_sprite = Resources.Load<Sprite>("Sprites/Skill_Icon/" + fileName);
    }

}