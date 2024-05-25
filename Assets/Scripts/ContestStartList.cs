using UnityEngine;
using System.Collections;


[System.Serializable]//この属性を使ってインスペクター上で表示

public class ContestStartList
{
    public int ContestID;
    public int Contest_placeNumID; //
    public string ContestName;
    public string ContestNameHyouji;
    public string Contest_themeComment; //コンテストのテーマ
    public int Contest_PMonth; //開催月　0月なら月指定はなく、来た日をもとに日付を決める
    public int Contest_Pday; //開催日
    public int Contest_EndMonth; //終了月
    public int Contest_Endday; //終了日
    public int Contest_Cost; //出場費用
    public int Contest_Flag; //解禁フラグ
    public int Contest_PatissierRank; //必要なパティシエランク
    public int Contest_Lv; //コンテストの難易度
    public int Contest_BringType; //アイテム持ち込み可能か不可    
    public int Contest_BringMax; //アイテム持ち込み可能な場合の上限数 
    public int Contest_RankingType; //トーナメント形式かランキング形式
    public int Contest_Accepted; //そのコンテスト参加済かどうか
    public int GetPatissierPoint; //コンテストクリアした際の獲得ポイント
    public int ContestVictory; //そのコンテストの過去の記録
    public int ContestFightsCount; //そのコンテストの出場回数
    public string Contest_Comment_out; //ゲーム中では使わない　コンテストへのメモ
    public int read_endflag; //ここが1のところで、検索終了

    public Sprite ContestIcon_sprite;

    //ここでリスト化時に渡す引数をあてがいます   
    public ContestStartList(int id, int placenum, string fileName, string _name, string _name_Hyouji, string _theme, 
        int _pmonth, int _pday, int _endmonth, int _endday, int _cost, int _flag, int _Prank,
        int _lv, int _bring_type, int _bring_max, int _ranking_type, int _contest_Accepted, int _get_patissierpoint, 
        int _contestVictory, int _contestFightsCount, string _comment_out, int _read_endflag)
    {
        ContestID = id;
        Contest_placeNumID = placenum;

        ContestName = _name;
        ContestNameHyouji = _name_Hyouji;
        Contest_themeComment = _theme;

        Contest_PMonth = _pmonth;
        Contest_Pday = _pday;
        Contest_EndMonth = _endmonth;
        Contest_Endday = _endday;

        Contest_Cost = _cost;
        Contest_Flag = _flag;
        Contest_PatissierRank = _Prank;
        Contest_Lv = _lv;
        Contest_BringType = _bring_type;
        Contest_BringMax = _bring_max;
        Contest_RankingType = _ranking_type;

        Contest_Accepted = _contest_Accepted;
        GetPatissierPoint = _get_patissierpoint;
        ContestVictory = _contestVictory;
        ContestFightsCount = _contestFightsCount;

        Contest_Comment_out = _comment_out;
        read_endflag = _read_endflag;

        ContestIcon_sprite = Resources.Load<Sprite>("Sprites/Skill_Icon/" + fileName);
    }

}