using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CombinationMain : SingletonMonoBehaviour<CombinationMain>
{
    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private int i, count;
    private int set_count;
    private int n, k;
    private string s;

    private int mstatus;

    private string[] item;
    private string[] subtype;
    private int[] kosu;
    private int[] dictID; //アイテム名、個数それぞれ、ディクショナリー化する。組み合わせは、各ディクショナリーのIDで見る。

    private string[] item2;

    private Dictionary<int, string> itemset;
    private Dictionary<int, int> kosuset;

    public List<int> result_kosuset = new List<int>(); //成功時の個数組み合わせを保存しておく。

    public bool compFlag;
    public string resultitemName;
    public int result_compID;

    public bool kosu_Check;

    private float kyori1;
    private float kyori2;
    private float kyori3;
    public float totalkyori;

    private List<int> youso = new List<int>();

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //個数も組み合わせ時判定するか
        kosu_Check = false; //trueで個数判定あり 

        //CombinationSubSample();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Combination(string[] args, int[] _kosu, int _mstatus) //順列・組み合わせ計算プログラム
    {
        n = 3; //3個の入力から
        k = 3; //nの中から、3個を選ぶ

        item = new string[n]; //3個の入力から
        subtype = new string[n];
        kosu = new int[n];
        dictID = new int[n];

        mstatus = _mstatus;

        for (i = 0; i < dictID.Length; i++)
        {
            dictID[i] = i; // 0, 1, 2のIDが入ったリスト。
        }

        item = args;       
        kosu = _kosu;

        //アイテム名・個数をディクショナリー化。dictIDと結びつける。
        Init_ItemSet();
        Init_KosuSet();


        //var nums = Enumerable.Range(1, n).ToArray();
        var combinations = CombinationMethod.Enumerate(dictID, k); //, withRepetition: true withRepetitionがfalseだと、重複しなくなる。trueで、重複する。


        //CompoDBの１行目から見ていく。
        count = 0;
        compFlag = false;

        while (count < databaseCompo.compoitems.Count)
        {
            //cmp_flag=9999の場合、その組み合わせは無視する。
            if (databaseCompo.compoitems[count].cmpitem_flag == 9999) {

            }
            else
            {
                set_count = 1;

                //入力した要素を、順番に全て出力する。3個入力なら、6個出力
                foreach (var elem in combinations)
                {
                    youso = new List<int>(elem);

                    //デバッグ用
                    //s = "(" + string.Join(",", elem.Select(x => x.ToString()).ToArray()) + ")";
                    //Debug.Log("set" + set_count + ": " + s);

                    //0,1,2とか2,0,1といった、IDの組み合わせがyousoに入っている。それを取り出し比較する。
                    if (databaseCompo.compoitems[count].cmpitemID_1 == itemset[youso[0]] &&
                        databaseCompo.compoitems[count].cmpitemID_2 == itemset[youso[1]] &&
                        databaseCompo.compoitems[count].cmpitemID_3 == itemset[youso[2]])
                    {

                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                if (_mstatus != 99)
                                {
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                }

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;

                                Kyori_Keisan();
                                break;

                            case true:

                                //更に、個数も確認。
                                if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosuset[youso[0]] &&
                                    databaseCompo.compoitems[count].cmpitem_kosu2 == kosuset[youso[1]] &&
                                    databaseCompo.compoitems[count].cmpitem_kosu3 == kosuset[youso[2]])
                                {
                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan();
                                }
                                break;
                        }

                        
                    }

                    /*for (i = 0; i < youso.Count; i++) 
                    {
                        Debug.Log(youso[i] + " Item: " + itemset[youso[i]] + " Kosu: " + kosuset[youso[i]]);                    
                    }*/
                    set_count++;
                }               
            }
            if (compFlag) { break; }
            count++;
        }

        //該当するものがなければ、compFlagはFalseのまま
        if (!compFlag) {
            if (_mstatus != 99)
            { Debug.Log("＜固有名称組み合わせ＞コンポDBに一致するもの無し"); }
        }
    }


    public void Combination2(string[] args, string[] args2, int[] _kosu, int _mstatus) //順列・組み合わせ計算プログラム サブタイプも含めて計算
    {
        item = args;
        subtype = args2;
        kosu = _kosu;

        count = 0;

        mstatus = _mstatus;

        while (count < databaseCompo.compoitems.Count)
        {
            //cmp_flag=9999の場合、その組み合わせは無視する。
            if (databaseCompo.compoitems[count].cmpitem_flag == 9999)
            {

            }
            else
            {
                if(_kosu[2] == 9999) //2個選んでた場合
                {
                    //3個目の個数が9999になってない組み合わせは無視する。
                    if (databaseCompo.compoitems[count].cmpitem_kosu3 != 9999)
                    {
                    }
                    else
                    {
                        //アイテム名＋サブ　か　サブ＋アイテム名が一致するかを確認する。 例：パン　＋　エメラルド　か　エメラルド　＋　パン　か。コンポでは、ラスクフリーに一致する。

                        if (databaseCompo.compoitems[count].cmpitemID_1 == item[0]) //一個目に選択したのが一致してた
                        {
                            //二個目の選択したサブが一致するかどうか
                            if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[1])
                            {
                                //[0][1]
                                youso.Clear();
                                youso.Add(0);
                                youso.Add(1);
                                youso.Add(2);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();

                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[0] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[1])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }

                                break;
                            }
                        }
                        else if (databaseCompo.compoitems[count].cmpitemID_1 == item[1]) //二個目に選択したのが一致してた
                        {
                            //二個目の選択したサブが一致するかどうか
                            if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[0])
                            {
                                //[1][0]
                                youso.Clear();
                                youso.Add(1);
                                youso.Add(0);
                                youso.Add(2);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();

                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[1] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[0])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }

                                break;
                            }
                        }
                    }
                }
                else if (_kosu[2] != 9999) //3個選んでた場合
                {
                    //コンポDBの一個目のアイテム名と選んだ3アイテムの名前が一致するかどうかを見る。

                    if (databaseCompo.compoitems[count].cmpitemID_1 == item[0]) //一個目に選択したのが一致してた
                    {
                        //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                        if (databaseCompo.compoitems[count].cmpitemID_2 == item[1])
                        {
                            //アイテム名＋アイテム名＋サブ(サブが空の場合もある。）
                            //3個目がサブ
                            if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2]) //&& databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                            {
                                //[0][1][2]
                                youso.Clear();
                                youso.Add(0);
                                youso.Add(1);
                                youso.Add(2);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();

                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[0] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[1] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[2])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }

                                break;
                            }
                        }
                        else if (databaseCompo.compoitems[count].cmpitemID_2 == item[2]) //item[2] != "empty" && 
                        {
                            //アイテム名＋サブ＋アイテム名
                            //2個目がサブ
                            if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1]) //&& databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                            {
                                //[0][2][1]
                                youso.Clear();
                                youso.Add(0);
                                youso.Add(2);
                                youso.Add(1);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[0] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[2] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[1])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }


                                break;
                            }
                        }
                        //2・3個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                        else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[1] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2])
                        //&& databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                        {
                            //[0][1][2]
                            youso.Clear();
                            youso.Add(0);
                            youso.Add(1);
                            youso.Add(2);

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan2();
                                    break;

                                case true:

                                    //個数もチェック
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[0] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[1] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[2])
                                    {
                                        //アイテム名＋サブ①＋サブ②。
                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                    }
                                    break;
                            }


                            break;
                        }
                        else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[2] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1])
                        //&& databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                        {
                            //[0][2][1]
                            youso.Clear();
                            youso.Add(0);
                            youso.Add(2);
                            youso.Add(1);

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan2();
                                    break;

                                case true:

                                    //個数もチェック
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[0] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[2] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[1])
                                    {
                                        //アイテム名＋サブ②＋サブ①。
                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                    }
                                    break;
                            }


                            break;
                        }
                    }

                    else if (databaseCompo.compoitems[count].cmpitemID_1 == item[1]) //二個目に選択したのが一致してた
                    {
                        //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                        if (databaseCompo.compoitems[count].cmpitemID_2 == item[0])
                        {
                            //アイテム名＋アイテム名＋サブ
                            //3個目がサブ
                            if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2]) //3個目のサブも一致していた // && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                            {
                                //[1][0][2]
                                youso.Clear();
                                youso.Add(1);
                                youso.Add(0);
                                youso.Add(2);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[1] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[0] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[2])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }


                                break;
                            }
                        }
                        else if (databaseCompo.compoitems[count].cmpitemID_2 == item[2]) //item[2] != "empty" && 
                        {
                            //アイテム名＋サブ＋アイテム名
                            //2個目がサブ
                            if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0]) //2個目のサブも一致していた //&& databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                            {
                                //[1][2][0]
                                youso.Clear();
                                youso.Add(1);
                                youso.Add(2);
                                youso.Add(0);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[1] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[2] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[0])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }


                                break;
                            }
                        }
                        //1・3個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                        else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[0] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2])
                        //&& databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                        {
                            //[1][0][2]
                            youso.Clear();
                            youso.Add(1);
                            youso.Add(0);
                            youso.Add(2);

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan2();
                                    break;

                                case true:

                                    //個数もチェック
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[1] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[0] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[2])
                                    {
                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                    }
                                    break;
                            }

                            break;
                        }
                        else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[2] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0])
                        //&& databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                        {
                            //[1][2][0]
                            youso.Clear();
                            youso.Add(1);
                            youso.Add(2);
                            youso.Add(0);

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan2();
                                    break;

                                case true:

                                    //個数もチェック
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[1] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[2] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[0])
                                    {
                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                    }
                                    break;
                            }


                            break;
                        }
                    }

                    else if (databaseCompo.compoitems[count].cmpitemID_1 == item[2]) //三個目に選択したのが一致してた
                    {
                        //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                        if (databaseCompo.compoitems[count].cmpitemID_2 == item[0])
                        {
                            //アイテム名＋アイテム名＋サブ
                            //3個目がサブ
                            if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1]) //&& databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                            {
                                //[2][0][1]
                                youso.Clear();
                                youso.Add(2);
                                youso.Add(0);
                                youso.Add(1);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[2] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[0] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[1])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }

                                break;
                            }
                        }
                        else if (databaseCompo.compoitems[count].cmpitemID_2 == item[1])
                        {
                            //アイテム名＋サブ＋アイテム名
                            //2個目がサブ
                            if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0]) //&& databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                            {
                                //[2][1][0]
                                youso.Clear();
                                youso.Add(2);
                                youso.Add(1);
                                youso.Add(0);

                                switch (kosu_Check)
                                {
                                    case false:

                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                        break;

                                    case true:

                                        //個数もチェック
                                        if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[2] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[1] &&
                                            databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[0])
                                        {
                                            compFlag = true;
                                            resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                            result_compID = count;

                                            Kyori_Keisan2();
                                        }
                                        break;
                                }


                                break;
                            }
                        }
                        //1・2個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                        if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[0] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1])
                        //&& databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                        {
                            //[2][0][1]
                            youso.Clear();
                            youso.Add(2);
                            youso.Add(0);
                            youso.Add(1);

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan2();
                                    break;

                                case true:

                                    //個数もチェック
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[2] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[0] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[1])
                                    {
                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                    }
                                    break;
                            }

                            break;
                        }
                        //一致してた場合、残りの2個のサブタイプを見る
                        if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[1] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0])
                        //&& databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty"
                        {
                            //[2][1][0]
                            youso.Clear();
                            youso.Add(2);
                            youso.Add(1);
                            youso.Add(0);

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan2();
                                    break;

                                case true:

                                    //個数もチェック
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosu[2] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosu[1] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosu[0])
                                    {
                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan2();
                                    }
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
            ++count;
        }

        //該当するものがなければ、compFlagはFalseのまま
        if (!compFlag) {
            if (_mstatus != 99)
            {
                Debug.Log("＜固有名＋サブタイプ＞コンポDBに一致するもの無し");
            }
        }
        else {
            if (_mstatus != 99)
            {
                Debug.Log("固有名＋サブで一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
            }
        }
    }

    public void Combination3(string[] args, int[] _kosu, int _mstatus) //順列・組み合わせ計算プログラム
    {
        n = 3; //3個の入力から
        k = 3; //nの中から、3個を選ぶ

        item = new string[n]; //3個の入力から
        subtype = new string[n];
        kosu = new int[n];
        dictID = new int[n];

        mstatus = _mstatus;

        for (i = 0; i < dictID.Length; i++)
        {
            dictID[i] = i; // 0, 1, 2のIDが入ったリスト。
        }

        item = args;
        kosu = _kosu;

        //アイテム名・個数をディクショナリー化。dictIDと結びつける。
        Init_ItemSet();
        Init_KosuSet();


        //var nums = Enumerable.Range(1, n).ToArray();
        var combinations = CombinationMethod.Enumerate(dictID, k); //, withRepetition: true withRepetitionがfalseだと、重複しなくなる。trueで、重複する。


        //CompoDBの１行目から見ていく。
        count = 0;
        compFlag = false;

        while (count < databaseCompo.compoitems.Count)
        {
            //cmp_flag=9999の場合、その組み合わせは無視する。
            if (databaseCompo.compoitems[count].cmpitem_flag == 9999)
            {

            }
            else
            {
                set_count = 1;

                //入力した要素を、順番に全て出力する。3個入力なら、6個出力
                foreach (var elem in combinations)
                {
                    youso = new List<int>(elem);

                    //デバッグ用
                    //s = "(" + string.Join(",", elem.Select(x => x.ToString()).ToArray()) + ")";
                    //Debug.Log("set" + set_count + ": " + s);

                    //0,1,2とか2,0,1といった、IDの組み合わせがyousoに入っている。それを取り出し比較する。
                    //Excelルールで、サブの頭がemptyの場合は、固有＋固有か固有＋サブの場合しかないので、サブ同士の組み合わせは計算しない
                    if (databaseCompo.compoitems[count].cmp_subtype_1 != "empty") 
                    {
                        if (databaseCompo.compoitems[count].cmp_subtype_1 == itemset[youso[0]] &&
                            databaseCompo.compoitems[count].cmp_subtype_2 == itemset[youso[1]] &&
                            databaseCompo.compoitems[count].cmp_subtype_3 == itemset[youso[2]])
                        {

                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    if (_mstatus != 99)
                                    {
                                        Debug.Log("サブ同士で一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                    }

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;

                                    Kyori_Keisan();
                                    break;

                                case true:

                                    //更に、個数も確認。
                                    if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosuset[youso[0]] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu2 == kosuset[youso[1]] &&
                                        databaseCompo.compoitems[count].cmpitem_kosu3 == kosuset[youso[2]])
                                    {
                                        //一致していたら、true
                                        if (_mstatus != 99)
                                        {
                                            Debug.Log("サブ同士で一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);
                                        }

                                        compFlag = true;
                                        resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                        result_compID = count;

                                        Kyori_Keisan();
                                    }
                                    break;
                            }


                        }
                    }

                    /*for (i = 0; i < youso.Count; i++) 
                    {
                        Debug.Log(youso[i] + " Item: " + itemset[youso[i]] + " Kosu: " + kosuset[youso[i]]);                    
                    }*/
                    set_count++;
                }
            }
            if (compFlag) { break; }
            count++;
        }

        //該当するものがなければ、compFlagはFalseのまま
        if (!compFlag)
        {
            if (_mstatus != 99)
            { Debug.Log("＜サブ名称組み合わせ＞コンポDBに一致するもの無し"); }
        }
    }

    void CombinationSubSample() //アイテム・サブの組み合わせをみる
    {
        n = 6; //3個の入力から
        k = 3; //nの中から、3個を選ぶ

        item2 = new string[n]; //3個の入力から

        item2[0] = "item1";
        item2[1] = "item2";
        item2[2] = "item3";
        item2[3] = "sub1";
        item2[4] = "sub2";
        item2[5] = "sub3";


        //var nums = Enumerable.Range(1, n).ToArray();
        var combinations2 = CombinationMethod.Enumerate(item2, k); //, withRepetition: true withRepetitionがfalseだと、重複しなくなる。trueで、重複する。


        //CompoDBの１行目から見ていく。
        count = 0;
        compFlag = false;


            set_count = 1;

            //入力した要素を、順番に全て出力する。3個入力なら、6個出力
            foreach (var elem2 in combinations2)
            {
                List<string> youso2 = new List<string>(elem2);

                //デバッグ用
                s = "(" + string.Join(",", elem2.Select(x => x.ToString()).ToArray()) + ")";
                Debug.Log("set" + set_count + ": " + s);

                //0,1,2とか2,0,1といった、IDの組み合わせがyousoに入っている。それを取り出し比較する。

                /*for (i = 0; i < youso.Count; i++) 
                {
                    Debug.Log(youso[i] + " Item: " + itemset[youso[i]] + " Kosu: " + kosuset[youso[i]]);                    
                }*/
                set_count++;
            }



    }

    void Init_ItemSet() //名前が重複しない前提で、このセットが使える。
    {
        itemset = new Dictionary<int, string>();
        for (i = 0; i < n; i++)
        {
            itemset.Add(dictID[i], item[i]);
        }
    }


    void Init_KosuSet() //名前が重複しない前提で、このセットが使える。
    {
        kosuset = new Dictionary<int, int>();
        for (i = 0; i < n; i++)
        {
            kosuset.Add(dictID[i], kosu[i]);
        }
    }

    void Kyori_Keisan()
    {
        kyori1 = Mathf.Abs(kosuset[youso[0]] - databaseCompo.compoitems[count].cmpitem_bestkosu1);
        kyori2 = Mathf.Abs(kosuset[youso[1]] - databaseCompo.compoitems[count].cmpitem_bestkosu2);
        kyori3 = Mathf.Abs(kosuset[youso[2]] - databaseCompo.compoitems[count].cmpitem_bestkosu3);

        if(kosuset[youso[0]] == 9999)
        {
            kyori1 = 0;
        }
        if (kosuset[youso[1]] == 9999)
        {
            kyori2 = 0;
        }
        if (kosuset[youso[2]] == 9999)
        {
            kyori3 = 0;
        }

        totalkyori = kyori1 + kyori2 + kyori3;
        GameMgr.hikari_make_okashi_totalkyori = totalkyori; //ヒカリが作るお菓子のときに使用する。
        //Debug.Log("ベスト配合との距離: " + totalkyori);

        if (mstatus != 99)
        {
            if (compFlag)
            {
                Debug.Log("一致したときの個数");
                Debug.Log("アイテム名１:" + databaseCompo.compoitems[count].cmpitemID_1 + " 個数: " + kosuset[youso[0]]);
                Debug.Log("アイテム名２:" + databaseCompo.compoitems[count].cmpitemID_2 + " 個数: " + kosuset[youso[1]]);
                Debug.Log("アイテム名３:" + databaseCompo.compoitems[count].cmpitemID_3 + " 個数: " + kosuset[youso[2]]);

                result_kosuset.Clear();

                for (i = 0; i < k; i++)
                {
                    result_kosuset.Add(kosuset[youso[i]]); //頭から順番に、youso[0], youso[1], youso[2]を保存
                }
            }
        }
    }

    void Kyori_Keisan2()
    {
        kyori1 = Mathf.Abs(kosu[youso[0]] - databaseCompo.compoitems[count].cmpitem_bestkosu1);
        kyori2 = Mathf.Abs(kosu[youso[1]] - databaseCompo.compoitems[count].cmpitem_bestkosu2);
        kyori3 = Mathf.Abs(kosu[youso[2]] - databaseCompo.compoitems[count].cmpitem_bestkosu3);

        if (kosu[0] == 9999)
        {
            kyori1 = 0;
        }
        if (kosu[1] == 9999)
        {
            kyori2 = 0;
        }
        if (kosu[2] == 9999)
        {
            kyori3 = 0;
        }

        totalkyori = kyori1 + kyori2 + kyori3;
        GameMgr.hikari_make_okashi_totalkyori = totalkyori; //ヒカリが作るお菓子のときに使用する。
        //Debug.Log("ベスト配合との距離: " + totalkyori);

        if (mstatus != 99)
        {
            if (compFlag)
            {
                Debug.Log("一致したときの個数");
                Debug.Log("アイテム名１:" + databaseCompo.compoitems[count].cmpitemID_1 + " 個数: " + kosu[youso[0]]);
                Debug.Log("アイテム名２:" + databaseCompo.compoitems[count].cmpitemID_2 + " 個数: " + kosu[youso[1]]);
                Debug.Log("アイテム名３:" + databaseCompo.compoitems[count].cmpitemID_3 + " 個数: " + kosu[youso[2]]);

                result_kosuset.Clear();

                for (i = 0; i < k; i++)
                {
                    result_kosuset.Add(kosu[youso[i]]); //頭から順番に、youso[0], youso[1], youso[2]を保存
                }
            }
        }
    }
}
