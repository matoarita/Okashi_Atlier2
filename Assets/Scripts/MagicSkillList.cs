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
    public string skill_CompSelect; //スキルを使うとき、調合Compoチェックをするかどうか　CompNo, MS, Bufが入ってると、CompoDBに登録がなくても失敗にならない
    public string skill_KosuSelect; //生成時の個数の処理指定
    public string skill_AddOriginalSelect; //アイテムにバフをかけるときに、生成アイテムを必ずオリジナルアイテムとして登録　おもに、ブドウやシュガーを変質させるとき
    public int skillType;   //パッシヴかアクティブスキルか
    public int skillCategory; //スキルの属性　基本、火、氷、光、風、心
    public int skillEnshutuType;   //演出魔法かそうでないか
    public int success_rate; //スキルの成功率　だが、今のとこcompoDBで決定するので使用してない
    public int cost_time;
    public int status_time; //プレイヤーにバフかけるときのその魔法の持続時間基準
    public string skillComment_Full; //スキルの詳細な説明
    public string skill_Jouken_name1; //取得に必要な前提スキル名
    public int skill_Jouken_lv1; //それの必要レベル

    public Sprite skillIcon_sprite;

    //excelには記載してない
    public int skill_usecount; //その魔法の使用回数　使えば使うほど、成功率があがるなど

    //ここでリスト化時に渡す引数をあてがいます   
    public MagicSkillList(int id, int koyuid, string fileName, string skill_name, string skill_name_Hyouji, string skill_comment, int skill_day, int skill_cost, int skill_flag,
        int skill_lv, int skill_maxlv, int skill_uselv, string skill_compselect, string skill_kosuselect, string skill_addoriginal_select, 
        int skill_type, int skill_category, int skill_enshutuType, 
        int successRate, int costTime, int _statusTime, string skill_comment_full,
        string skill_jouken_name1, int skill_jouken_lv1)
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
        skill_CompSelect = skill_compselect;
        skill_KosuSelect = skill_kosuselect;
        skill_AddOriginalSelect = skill_addoriginal_select;
        skillType = skill_type;
        skillCategory = skill_category;
        skillEnshutuType = skill_enshutuType;
        skillComment_Full = skill_comment_full;

        skill_Jouken_name1 = skill_jouken_name1;
        skill_Jouken_lv1 = skill_jouken_lv1;

        success_rate = successRate;
        cost_time = costTime;
        status_time = _statusTime;

        skillIcon_sprite = Resources.Load<Sprite>("Sprites/Skill_Icon/" + fileName);

        //Excel記載してない
        skill_usecount = 0;
    }

}