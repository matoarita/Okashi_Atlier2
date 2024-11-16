using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop_Main : MonoBehaviour {

    private Shop_Main_Controller shopmain_Controller;

    // Use this for initialization
    void Start () {

        shopmain_Controller = this.GetComponent<Shop_Main_Controller>();
        shopmain_Controller.InitSetup();

        GameMgr.Scene_Name = "Shop_Grt";
        GameMgr.Window_CharaName = "プリン";

        GameMgr.System_Shop_text1 = "いらっしゃい～。";
        GameMgr.System_Shop_text2 = "何を買うの？";
        GameMgr.System_Shop_text3 = "フルーツや材料は、買い取りをしてるわよ。" + "\n" + "何を売るの？";
        GameMgr.System_Shop_text4 = "買いますか？";
        GameMgr.System_Shop_text5 = "かかるわよ。";
        GameMgr.System_Shop_text6 = "何にしますか？";
        GameMgr.System_Shop_text7 = "売るアイテムを選択してね。";
        GameMgr.System_Shop_text8 = "売りますか？";
        GameMgr.System_Shop_text9 = "で買い取るわよ。";

        //ネームプレートの設定
        shopmain_Controller.SceneNamePlateSetting();

        //お店の初期メッセージ
        shopmain_Controller.SceneDefaultMessage();

        //シーン読み込み完了時のメソッド
        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        //SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    // Update is called once per frame
    void Update()
    {
        shopmain_Controller.UpdateShopScene();
    }    

    //別シーンからこのシーンが読み込まれたときに、読み込む
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameMgr.Scene_LoadedOn_End = true;
    }

    //シーンがアンロードされたタイミングで呼び出しされる
    void OnSceneUnloaded(Scene current)
    {
        Debug.Log("OnSceneUnloaded: " + current);
        GameMgr.Scene_LoadedOn_End = false;
    }
}
