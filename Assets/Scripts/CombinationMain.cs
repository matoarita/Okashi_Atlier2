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

    private string[] item;
    private string[] subtype;
    private int[] kosu;
    private int[] dictID; //アイテム名、個数それぞれ、ディクショナリー化する。組み合わせは、各ディクショナリーのIDで見る。

    private string[] item2;

    private Dictionary<int, string> itemset;
    private Dictionary<int, int> kosuset;

    public bool compFlag;
    public string resultitemName;
    public int result_compID;

    public bool kosu_Check;

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

    public void Combination(string[] args, int[] _kosu) //順列・組み合わせ計算プログラム
    {
        n = 3; //3個の入力から
        k = 3; //nの中から、3個を選ぶ

        item = new string[n]; //3個の入力から
        subtype = new string[n];
        kosu = new int[n];
        dictID = new int[n];

        for(i = 0; i < dictID.Length; i++)
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
                    List<int> youso = new List<int>(elem);

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
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
                                break;

                            case true:

                                //更に、個数も確認。
                                if (databaseCompo.compoitems[count].cmpitem_kosu1 == kosuset[youso[0]] &&
                                    databaseCompo.compoitems[count].cmpitem_kosu2 == kosuset[youso[1]] &&
                                    databaseCompo.compoitems[count].cmpitem_kosu3 == kosuset[youso[2]])
                                {
                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
        if (!compFlag) { Debug.Log("＜固有名称組み合わせ＞コンポDBに一致するもの無し"); }
    }


    public void Combination2(string[] args, string[] args2, int[] _kosu) //順列・組み合わせ計算プログラム
    {
        item = args;
        subtype = args2;
        kosu = _kosu;

        count = 0;

        while (count < databaseCompo.compoitems.Count)
        {
            //cmp_flag=9999の場合、その組み合わせは無視する。
            if (databaseCompo.compoitems[count].cmpitem_flag == 9999)
            {

            }
            else
            {
                //コンポDBの一個目のアイテム名と選んだ3アイテムの名前が一致するかどうかを見る。

                if (databaseCompo.compoitems[count].cmpitemID_1 == item[0]) //一個目に選択したのが一致してた
                {
                    //一致してた場合、次に、2個目のコンポDBのアイテム名と一致するものがあるか調べる
                    if (databaseCompo.compoitems[count].cmpitemID_2 == item[1])
                    {
                        //アイテム名＋アイテム名＋サブ(サブが空の場合もある。）
                        //3個目がサブ
                        if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2] && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                        {
                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
                                    }
                                    break;
                            }

                            break;
                        }
                    }
                    else if (item[2] != "empty" && databaseCompo.compoitems[count].cmpitemID_2 == item[2])
                    {
                        //アイテム名＋サブ＋アイテム名
                        //2個目がサブ
                        if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1] && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                        {
                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
                                    }
                                    break;
                            }


                            break;
                        }
                    }
                    //2・3個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                    else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[1] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2]
                        && databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                    {
                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
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
                                }
                                break;
                        }


                        break;
                    }
                    else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[2] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1]
                        && databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                    {
                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
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
                        if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2] && databaseCompo.compoitems[count].cmp_subtype_3 != "empty") //3個目のサブも一致していた
                        {
                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
                                    }
                                    break;
                            }


                            break;
                        }
                    }
                    else if (item[2] != "empty" && databaseCompo.compoitems[count].cmpitemID_2 == item[2])
                    {
                        //アイテム名＋サブ＋アイテム名
                        //2個目がサブ
                        if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0] && databaseCompo.compoitems[count].cmp_subtype_3 != "empty") //2個目のサブも一致していた
                        {
                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
                                    }
                                    break;
                            }


                            break;
                        }
                    }
                    //1・3個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                    else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[0] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[2]
                        && databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                    {
                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
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
                                }
                                break;
                        }

                        break;
                    }
                    else if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[2] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0]
                        && databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                    {
                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
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
                        if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1] && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                        {
                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
                        if (databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0] && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                        {
                            switch (kosu_Check)
                            {
                                case false:

                                    //一致していたら、true
                                    Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                    compFlag = true;
                                    resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                    result_compID = count;
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
                                    }
                                    break;
                            }


                            break;
                        }
                    }
                    //1・2個目のアイテム名が、DBと一致しなかったので、次はサブ＋サブの組み合わせを調べる。
                    if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[0] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[1]
                        && databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                    {
                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
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
                                }
                                break;
                        }

                        break;
                    }
                    //一致してた場合、残りの2個のサブタイプを見る
                    if (databaseCompo.compoitems[count].cmp_subtype_2 == subtype[1] && databaseCompo.compoitems[count].cmp_subtype_3 == subtype[0]
                        && databaseCompo.compoitems[count].cmp_subtype_2 != "empty" && databaseCompo.compoitems[count].cmp_subtype_3 != "empty")
                    {
                        switch (kosu_Check)
                        {
                            case false:

                                //一致していたら、true
                                Debug.Log("一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name);

                                compFlag = true;
                                resultitemName = databaseCompo.compoitems[count].cmpitemID_result;
                                result_compID = count;
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
                                }
                                break;
                        }

                        break;
                    }
                }
            }
            ++count;
        }

        //該当するものがなければ、compFlagはFalseのまま
        if (!compFlag) { Debug.Log("＜固有名＋サブタイプ＞コンポDBに一致するもの無し"); }
        else { Debug.Log("固有名＋サブで一致した。" + " アイテム名: " + databaseCompo.compoitems[count].cmpitem_Name); }
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
}
