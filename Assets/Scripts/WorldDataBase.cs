using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDataBase : SingletonMonoBehaviour<WorldDataBase>
{

    //街や採集場所の情報を登録しておくリスト

    public List<string> travel_name = new List<string>(); //staticにすると、シーン間でも変数が共通のものになる。呼び出すときは、クラス名（WorldDataBase）.travel_name～のようにする。
    public List<int> travel_cost = new List<int>();

    void Start()
    {
        DontDestroyOnLoad(this);
        Setup_WorldDataBase();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Setup_WorldDataBase()
    {

        //場所の初期化
        travel_name.Add("近くの森と平原");
        travel_cost.Add(30);

        travel_name.Add("エメラルドの森");
        travel_cost.Add(60);

        travel_name.Add("アクアマリンの海");
        travel_cost.Add(120);

        travel_name.Add("ルビーの古城");
        travel_cost.Add(240);

        travel_name.Add("シトリン平野");
        travel_cost.Add(480);

        travel_name.Add("アメジストの庭");
        travel_cost.Add(960);
    }
}
