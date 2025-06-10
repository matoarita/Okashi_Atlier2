using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CatDataBase : SingletonMonoBehaviour<CatDataBase>
{
    private DateTime TodayNow;

    private ItemMatPlaceDataBase matplace_database;

    private string catid;
    private string _original_id_string;

    private string catnameHyouji;         //名前、画像ファイル名

    private int caticon_num;
    private int cattype; //猫の種族 現在４つ

    private int catcost; //エサ代　一日たつとこの費用が減っていく
    private int catcostlv;
    private int cattansaku_Speed; //探索にかかる時間。分単位。
    private int cattansaku_Kaisu; //探索一回あたりの試行回数
    private int catexp;
    private int cathp; //ねこの好感度のこと　体力ではない 100が上限。
    private int catlv; //LV9あたりまでがマックス？

    private string cattansaku_MapName;
    private int catstatus; //0=ひまで何もしてない。 100=探索に出かけ中。
    private bool catesa_nogive;

    public List<CatData> catdata_list = new List<CatData>();
    public List<CatData> catdata_randomlistLibrary = new List<CatData>(); //ねこが家にくるときのランダムのねこリスト
    public List<CatData> catdata_checklist = new List<CatData>(); //ランダムから一匹をえらび表示用としてここに入れる。必ず一匹のみデータを入れる。

    private int i, _rnd;
    private int _dcatcount;
    private int _catstatus;
    private int _cathp;
    private bool _catesa_nogive;
    private bool _playflag;
    private string _status_text, _mapnamehyouji, _cattansaku_txt;
    private int _type, _icon_num;

    private List<Sprite> catIcon_sprite = new List<Sprite>();
    private List<Sprite> catIcon_sprite2 = new List<Sprite>();
    private List<Sprite> catIcon_sprite3 = new List<Sprite>();
    private List<Sprite> catIcon_sprite4 = new List<Sprite>();

    private List<int> catIcon_voice = new List<int>();
    private List<int> catIcon_voice2 = new List<int>();
    private List<int> catIcon_voice3 = new List<int>();
    private List<int> catIcon_voice4 = new List<int>();

    private List<string> catType_name = new List<string>();
    private List<string> catIcon_anim = new List<string>();
    private List<string> catIcon_anim_sleep = new List<string>();

    private List<int> deleteCatList = new List<int>();

    private Sprite _return_sprite;
    private string _return_catanim;
    private int _return_voice;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this); //ゲーム中のアイテムリスト情報は、ゲーム中で全て共通のデータベースで管理したい。なので、破壊されないようにしておく。

        catdata_list.Clear();
        catdata_randomlistLibrary.Clear();
        catdata_checklist.Clear();

        CatType_InitLibrary();
        CatRandom_InitLibrary();
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
        caticon_num = 0;
        cattype = 0;
        catcost = 250;
        catcostlv = 2;
        cattansaku_Speed = 480; //8hで一回探索
        cattansaku_Kaisu = 6;

        catexp = 0;
        cathp = 30;
        catlv = 1;

        cattansaku_MapName = "Non";
        catstatus = 0;
        catesa_nogive = false;

        //ここでリストに追加している
        ListAdd();       
    }

    public void SetInit_CustomCatData(string _name, int _icon_num, int _type, int _cost, int _speed, int _kaisu, int _lv, int _chk_list)
    {
        KoyuID_Set();
        catid = _original_id_string;

        catnameHyouji = _name;
        caticon_num = _icon_num;
        cattype = _type;
        catcost = _cost;
        catcostlv = 1;
        cattansaku_Speed = _speed; //8hで一回探索
        cattansaku_Kaisu = _kaisu;

        catexp = 0;
        cathp = 30;
        catlv = _lv;

        cattansaku_MapName = "Non";
        catstatus = 0;
        catesa_nogive = false;

        //ここでリストに追加している
        if (_chk_list == 0)
        {
            ListAdd();
        }
        else if (_chk_list == 1)//チェック表示用の場合 1
        {
            ListCheckAdd();
        }
        else //ランダム用
{
            ListRandomAdd();
        }
    }

    void ListAdd()
    {
        //ここでリストに追加している
        catdata_list.Add(new CatData(catid, catnameHyouji, caticon_num, cattype, catcost, catcostlv, cattansaku_Speed, cattansaku_Kaisu, catexp, 
            cathp, catlv, cattansaku_MapName, catstatus, catesa_nogive));
        Debug.Log("ねこ追加: " + catdata_list[catdata_list.Count - 1].catnameHyouji);
    }

    void ListCheckAdd()
    {
        //ここでリストに追加している
        catdata_checklist.Add(new CatData(catid, catnameHyouji, caticon_num, cattype, catcost, catcostlv, cattansaku_Speed, cattansaku_Kaisu, catexp,
            cathp, catlv, cattansaku_MapName, catstatus, catesa_nogive));
        Debug.Log("ねこ表示用: " + catdata_checklist[catdata_checklist.Count - 1].catnameHyouji);
    }

    void ListRandomAdd()
    {
        //ここでリストに追加している
        catdata_randomlistLibrary.Add(new CatData(catid, catnameHyouji, caticon_num, cattype, catcost, catcostlv, cattansaku_Speed, cattansaku_Kaisu, catexp,
            cathp, catlv, cattansaku_MapName, catstatus, catesa_nogive));
        Debug.Log("ねこランダムセット用: " + catdata_randomlistLibrary[catdata_randomlistLibrary.Count - 1].catnameHyouji);
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

    public Sprite SetSprite(int _listid, int _chklist) //ねこの画像をかえす 種族やLVなどで画像はかわるので、ここで判定
    {
        
        if (_chklist == 0)
        {
            _type = catdata_list[_listid].catType;
            _icon_num = catdata_list[_listid].caticon_Num;

            TypeDataSetting(_type, _icon_num);                       
        }
        else //チェック用を参照
        {
            _type = catdata_checklist[_listid].catType;
            _icon_num = catdata_checklist[_listid].caticon_Num;

            TypeDataSetting(_type, _icon_num);
        }

        return _return_sprite;
    }

    public int SetVoice(int _listid, int _chklist) //ねこの画像をかえす 種族やLVなどで画像はかわるので、ここで判定
    {

        if (_chklist == 0)
        {
            _type = catdata_list[_listid].catType;
            _icon_num = catdata_list[_listid].caticon_Num;

            TypeDataSetting(_type, _icon_num);
        }
        else //チェック用を参照
        {
            _type = catdata_checklist[_listid].catType;
            _icon_num = catdata_checklist[_listid].caticon_Num;

            TypeDataSetting(_type, _icon_num);
        }

        return _return_voice;
    }

    void TypeDataSetting(int _Type, int _IconNum)
    {
        switch (_Type)
        {
            case 0:

                _return_sprite = catIcon_sprite[_IconNum];
                _return_voice = catIcon_voice[_IconNum];
                break;

            case 1:

                _return_sprite = catIcon_sprite2[_IconNum];
                _return_voice = catIcon_voice2[_IconNum];
                break;

            case 2:

                _return_sprite = catIcon_sprite3[_IconNum];
                _return_voice = catIcon_voice3[_IconNum];
                break;

            case 3:

                _return_sprite = catIcon_sprite4[_IconNum];
                _return_voice = catIcon_voice4[_IconNum];
                break;
        }
    }

    public string SetCatAnimObj(int _listid, int _chklist)
    {
        if (_chklist == 0)
        {
            if (catdata_list[_listid].catStatus == 0) //ねそべりモーション
            {
                _return_catanim = catIcon_anim_sleep[catdata_list[_listid].catType];
            }
            else if (catdata_list[_listid].catStatus == 100) //採取中モーション
            {
                _return_catanim = catIcon_anim[catdata_list[_listid].catType];
            }
        }
        else
        {
            if (catdata_checklist[_listid].catStatus == 0) //ねそべりモーション
            {
                _return_catanim = catIcon_anim_sleep[catdata_checklist[_listid].catType];
            }
            else if (catdata_checklist[_listid].catStatus == 100) //採取中モーション
            {
                _return_catanim = catIcon_anim[catdata_checklist[_listid].catType];
            }
        }

        return _return_catanim;
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

    //ねこリストからねこを削除する
    public void Sayonara_Cat(int _num)
    {
        catdata_list.RemoveAt(_num);
    }

    public string CatStatusTextLibrary(int _listid)
    {
        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        _cathp = catdata_list[_listid].catHP;
        _catstatus = catdata_list[_listid].catStatus;
        _catesa_nogive = catdata_list[_listid].catEsaNoGive;
        _status_text = "ぼ～っとしている。";

        if (_catesa_nogive)
        {
            _status_text = "エサをもらえてないので、不機嫌になっている。";
        }
        else
        {
            if (_catstatus == 0) //ひまの状態
            {
                if (_cathp <= 11) //好感度が低く逃亡寸前
                {
                    _status_text = "家が気に入らないので、逃亡を企てている。";
                }
                else
                {
                    _status_text = "ぼ～っとしている。";
                }
            }

            if (_catstatus == 100) //採取中の状態
            {
                _mapnamehyouji = matplace_database.matplace_lists[matplace_database.SearchMapString(catdata_list[_listid].catTansaku_MapName)].placeNameHyouji;
                _status_text = "採取中 " + _mapnamehyouji + "\n" + "今日はやる気いっぱい！";
            }
        }

        return _status_text;
    }

    public void GetOutCatCheck() //逃亡するかどうかをチェックする
    {
        deleteCatList.Clear();
        for (i = 0; i < catdata_list.Count; i++)
        {
            if (catdata_list[i].catHP <= 0)
            {
                deleteCatList.Add(i);
            }
        }

        for (i = 0; i < deleteCatList.Count; i++)
        {
            Debug.Log(deleteCatList[i]);
        }

        //降順にして後ろから削除
        if (deleteCatList.Count > 0)
        {
            Debug.Log("HP=0のねこがいたので逃亡");

            _dcatcount = deleteCatList.Count;
            for (i = 0; i < _dcatcount; i++)
            {
                catdata_list.RemoveAt(deleteCatList[_dcatcount - 1 - i]);
            }

            //逃亡フラグもたち、ヒカリがびっくりする
            GameMgr.CatEscapeFlag = true;
        }
    }

    public string CatTansakuTextLibrary(int _tansakusp)
    {
        _cattansaku_txt = "並";

        if (_tansakusp < 30)
        {
            _cattansaku_txt = "神速";
        }
        else if (_tansakusp >= 30 && _tansakusp < 60)
        {
            _cattansaku_txt = "撃速";
        }
        else if (_tansakusp >= 60 && _tansakusp < 120)
        {
            _cattansaku_txt = "速";
        }
        else if (_tansakusp >= 120 && _tansakusp < 200)
        {
            _cattansaku_txt = "良";
        }
        else if (_tansakusp >= 200 && _tansakusp < 400)
        {
            _cattansaku_txt = "並";
        }
        else if (_tansakusp >= 400 && _tansakusp < 600)
        {
            _cattansaku_txt = "遅い";
        }
        else if (_tansakusp >= 600)
        {
            _cattansaku_txt = "鈍亀";
        }

        return _cattansaku_txt;
    }

    public void RandomCatSelect() //EventDatabaseからよびだし。家にくる猫のデータをランダムでセット。
    {
        catdata_checklist.Clear();

        //ランダムライブラリーからデータをとってくる。
        _rnd = UnityEngine.Random.Range(0, catdata_randomlistLibrary.Count);

        SetInit_CustomCatData(catdata_randomlistLibrary[_rnd].catnameHyouji, catdata_randomlistLibrary[_rnd].caticon_Num,
            catdata_randomlistLibrary[_rnd].catType, 
            250, catdata_randomlistLibrary[_rnd].catTansaku_DefaultSpeed + UnityEngine.Random.Range(-50, 50),
            catdata_randomlistLibrary[_rnd].catTansaku_Kaisu, UnityEngine.Random.Range(1, 4), 1);
    }

    //チェックに入っていたデータをオリジナルデータへAddする
    public void CatCopyCheckToOrigin()
    {
        SetInit_CustomCatData(catdata_checklist[0].catnameHyouji, catdata_checklist[0].caticon_Num, catdata_checklist[0].catType, catdata_checklist[0].catCost,
            catdata_checklist[0].catTansaku_DefaultSpeed, catdata_checklist[0].catTansaku_Kaisu, catdata_checklist[0].catLv, 0);
    }

    //種族ごとのねこ画像　上から順番にtype=0, 1.. と対応
    void CatType_InitLibrary()
    {
        catIcon_sprite.Clear();
        catIcon_sprite2.Clear();
        catIcon_sprite3.Clear();
        catIcon_sprite4.Clear();
        catIcon_voice.Clear();
        catIcon_voice2.Clear();
        catIcon_voice3.Clear();
        catIcon_voice4.Clear();
        catType_name.Clear();
        catIcon_anim.Clear();
        catIcon_anim_sleep.Clear();

        //
        catIcon_sprite.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_01"));
        catIcon_sprite.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_01b"));
        catIcon_voice.Add(241);
        catIcon_voice.Add(241);

        catType_name.Add("灰猫");
        catIcon_anim.Add("tc_cat_type02_anim01"); //ファイル名を記述
        catIcon_anim_sleep.Add("tc_cat_type02_anim02"); //寝そべりモーション

        //
        catIcon_sprite2.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_02"));
        catIcon_sprite2.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_02b"));
        catIcon_voice2.Add(239);
        catIcon_voice2.Add(239);

        catType_name.Add("茶猫");
        catIcon_anim.Add("tc_cat_type01_anim01"); //
        catIcon_anim_sleep.Add("tc_cat_type01_anim02"); //寝そべりモーション

        //
        catIcon_sprite3.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_03"));
        catIcon_sprite3.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_03b"));
        catIcon_voice3.Add(242);
        catIcon_voice3.Add(242);

        catType_name.Add("くろ");
        catIcon_anim.Add("tc_cat_type04_anim01"); //
        catIcon_anim_sleep.Add("tc_cat_type04_anim02"); //寝そべりモーション

        //
        catIcon_sprite4.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_04"));
        catIcon_sprite4.Add(Resources.Load<Sprite>("Sprites/CatIcon/" + "CatIcon_04b"));
        catIcon_voice4.Add(243);
        catIcon_voice4.Add(243);

        catType_name.Add("しろ");
        catIcon_anim.Add("tc_cat_type03_anim01"); //
        catIcon_anim_sleep.Add("tc_cat_type03_anim02"); //寝そべりモーション
    }

    void CatRandom_InitLibrary() //ランダム猫用のデータセット　タイプでちょっと差をつけてもいいかも？
    {
        SetInit_CustomCatData("ピサロ", 0, 0, 250, UnityEngine.Random.Range(400, 800), UnityEngine.Random.Range(4, 6), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("ノブナガ", 1, 0, 250, UnityEngine.Random.Range(400, 800), UnityEngine.Random.Range(4, 6), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("ロドリゲス", 0, 0, 250, UnityEngine.Random.Range(400, 800), UnityEngine.Random.Range(4, 6), UnityEngine.Random.Range(1, 4), 2);

        SetInit_CustomCatData("じろきち", 0, 1, 250, UnityEngine.Random.Range(300, 500), UnityEngine.Random.Range(3, 4), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("マロリー", 1, 1, 250, UnityEngine.Random.Range(300, 500), UnityEngine.Random.Range(3, 4), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("クルス", 0, 1, 250, UnityEngine.Random.Range(300, 500), UnityEngine.Random.Range(3, 4), UnityEngine.Random.Range(1, 4), 2);        

        SetInit_CustomCatData("ボコ", 0, 2, 250, UnityEngine.Random.Range(200, 800), UnityEngine.Random.Range(2, 6), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("かにぱん", 1, 2, 250, UnityEngine.Random.Range(200, 800), UnityEngine.Random.Range(2, 6), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("えびふらい", 0, 2, 250, UnityEngine.Random.Range(200, 800), UnityEngine.Random.Range(2, 6), UnityEngine.Random.Range(1, 4), 2);

        SetInit_CustomCatData("みこ", 0, 3, 250, UnityEngine.Random.Range(200, 400), UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("メリー", 1, 3, 250, UnityEngine.Random.Range(200, 400), UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(1, 4), 2);
        SetInit_CustomCatData("シヴァ", 0, 3, 250, UnityEngine.Random.Range(200, 400), UnityEngine.Random.Range(2, 3), UnityEngine.Random.Range(1, 4), 2);
    }
}
