using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EmeraldShop_Main : MonoBehaviour {

    private EmeraldShop_Main_Controller emeraldmain_Controller;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private int i;

    // Use this for initialization
    void Start () {

        emeraldmain_Controller = this.GetComponent<EmeraldShop_Main_Controller>();
        emeraldmain_Controller.InitSetup();

        GameMgr.Scene_Name = "EmeraldShop_Grt";
        GameMgr.Window_CharaName = "ドルチェ";

        //ネームプレートの設定
        emeraldmain_Controller.SceneNamePlateSetting();

        //シーン読み込み完了時のメソッド
        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        //SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド。自分自身のシーン読み込み時でも発動する。      
    }
	
	// Update is called once per frame
	void Update () {
        emeraldmain_Controller.UpdateEmeraldScene();
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
