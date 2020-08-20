using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveExample : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //セーブデータを管理するデータバンクのインスタンスを取得します(シングルトン)
        DataBank bank = DataBank.Open();

        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        //セーブ保存用のクラスを新規作成。
        PlayerData playerData = new PlayerData()
        {
            /*name = "Tokoroten",
            level = 1,
            statusList = new List<int>
            {
                10, 20, 30, 40, 50
            }*/
        };
        Debug.Log(playerData);

        //データの一時保存。bankに、「player」という名前で現在のデータを保存。
        bank.Store("player", playerData);
        Debug.Log("bank.Store()");

        //一時データを永続的に保存。永続保存するときは、一度、一時データに保存しておく。
        bank.SaveAll();
        Debug.Log("bank.SaveAll()");

        //初期化
        playerData = new PlayerData();
        Debug.Log(playerData);

        //一時データを取得。「player」という名前のデータを取得。
        playerData = bank.Get<PlayerData>("player");
        Debug.Log(playerData);

        //一時データをすべて破棄。
        bank.Clear();
        Debug.Log("bank.Clear()");

        playerData = bank.Get<PlayerData>("player"); //上でクリアしたあとなので、Nullがかえってくる。
        Debug.Log(playerData);

        //永続的に保存しておいたデータを、一時データに読み込む。bankに読み込まれる。
        bank.Load<PlayerData>("player");
        Debug.Log("bank.Load()");

        //一時データに再度読み込んだので、Getすると、再びパラメータを取得できる。
        playerData = bank.Get<PlayerData>("player"); 
        Debug.Log(playerData);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
