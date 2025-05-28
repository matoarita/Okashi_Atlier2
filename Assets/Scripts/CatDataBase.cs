using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CatDataBase : SingletonMonoBehaviour<CatDataBase>
{
    private DateTime TodayNow;

    private ItemMatPlaceDataBase matplace_database;

    private string catid;
    private string _original_id_string;

    private string catnameHyouji;         //名前、画像ファイル名

    private int cattype; //猫の種族

    private int catcost; //エサ代　一日たつとこの費用が減っていく
    private int cattansaku_Speed; //探索にかかる時間。分単位。
    private int cattansaku_Kaisu; //探索一回あたりの試行回数
    private int catexp;
    private int cathp; //ねこの好感度のこと　体力ではない 100が上限。
    private int catlv; //LV9あたりまでがマックス？

    private string cattansaku_MapName;
    private int catstatus; //0=ひまで何もしてない。 100=探索に出かけ中。

    public List<CatData> catdata_list = new List<CatData>();

    private int i;
    private int _catstatus;
    private bool _playflag;
    private string _status_text, _mapnamehyouji;

    private List<Sprite> catIcon_sprite = new List<Sprite>();
    private List<String> catType_name = new List<String>();

    private Sprite _return_sprite;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        catdata_list.Clear();

        SetInit_CustomCatData("みゅ～", 1, 250, 20, 6, 1);
        SetInit_CustomCatData("モルダウ", 0, 500, 30, 6, 3);
        Debug.Log("ねこリストカウント: " + catdata_list.Count);

        CatType_InitLibrary();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInit_DefaultCatData()
    {
        KoyuID_Set();
        catid = _original_id_string;

        catnameHyouji = "";
        cattype = 0;
        catcost = 200;
        cattansaku_Speed = 480; //8hで一回探索
        cattansaku_Kaisu = 6;

        catexp = 0;
        cathp = 30;
        catlv = 1;

        cattansaku_MapName = "Non";
        catstatus = 0;

        //ここでリストに追加している
        ListAdd();       
    }

    public void SetInit_CustomCatData(string _name, int _type, int _cost, int _speed, int _kaisu, int _lv)
    {
        KoyuID_Set();
        catid = _original_id_string;

        catnameHyouji = _name;
        cattype = _type;
        catcost = _cost;
        cattansaku_Speed = _speed; //8hで一回探索
        cattansaku_Kaisu = _kaisu;

        catexp = 0;
        cathp = 30;
        catlv = _lv;

        cattansaku_MapName = "Non";
        catstatus = 0;

        //ここでリストに追加している
        ListAdd();
    }

    void ListAdd()
    {
        //ここでリストに追加している
        catdata_list.Add(new CatData(catid, catnameHyouji, cattype, catcost, cattansaku_Speed, cattansaku_Kaisu, catexp, cathp, catlv, cattansaku_MapName, catstatus));
        Debug.Log("ねこ追加: " + catdata_list[catdata_list.Count - 1].catnameHyouji);
    }

    void KoyuID_Set()
    {
        TodayNow = DateTime.Now;
        _original_id_string = TodayNow.Year.ToString("0000") + TodayNow.Month.ToString("00") + TodayNow.Day.ToString("00") + TodayNow.Hour.ToString("00") + TodayNow.Minute.ToString("00") + TodayNow.Second.ToString("00");
        //Debug.Log("OriginalItemID: " + _original_id_string);
    }

    //固有IDをもとにねこのリスト番号を探す
    public int SearchCatKoyuID(string _id)
    {
        for(i = 0; i < catdata_list.Count; i++)
        {
            if( catdata_list[i].catID == _id)
            {
                return i;
            }
        }

        return 0; //無い場合は0
    }

    //リスト番号と採取地名をいれると、そこのマップに採取にいかせる　状態もいってる状態に。
    public void SetCatGotoMap(int _listid, string _mapname)
    {
        catdata_list[_listid].catTansaku_MapName = _mapname;
        catdata_list[_listid].catStatus = 100;
    }

    public Sprite SetSprite(int _listid) //ねこの画像をかえす 種族やLVなどで画像はかわるので、ここで判定
    {
        _return_sprite = catIcon_sprite[catdata_list[_listid].catType];

        return _return_sprite;
    }

    //ねこリストを全部チェックし、採取のチェックが必要か不要かを判定する
    public bool Check_CatGotoFlag()
    {
        _playflag = false;
        for (i = 0; i < catdata_list.Count; i++)
        {
            if (catdata_list[i].catStatus == 100) //採取中のねこが一匹でもいればフラグON
            {
                _playflag = true;
                return _playflag;
            }
        }

        return _playflag;
    }

    public string CatStatusTextLibrary(int _listid)
    {
        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        _catstatus = catdata_list[_listid].catStatus;
        _status_text = "ぼ～っとしている。";

        if(_catstatus == 0) //ひまの状態
        {
            _status_text = "ぼ～っとしている。";
        }

        if (_catstatus == 100) //採取中の状態
        {
            _mapnamehyouji = matplace_database.matplace_lists[matplace_database.SearchMapString(catdata_list[_listid].catTansaku_MapName)].placeNameHyouji;
            _status_text = "採取中 " + _mapnamehyouji + "\n" + "今日はやる気いっぱい！";
        }

        return _status_text;
    }

    //種族ごとのねこ画像　上から順番にtype=0, 1.. と対応
    void CatType_InitLibrary()
    {
        catIcon_sprite.Clear();
        catType_name.Clear();

        catIcon_sprite.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_01"));
        catType_name.Add("茶猫");
        catIcon_sprite.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_02"));
        catType_name.Add("灰色雑種");
    }

    
}
