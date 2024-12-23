using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NPC_Hiroba_Main_Or : MonoBehaviour {

    private NPC_Hiroba_Main_Controller npc_hiroba_main_Controller;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private int i;

    // Use this for initialization
    void Start () {

        npc_hiroba_main_Controller = this.GetComponent<NPC_Hiroba_Main_Controller>();
        npc_hiroba_main_Controller.InitSetup();

        BGImagePanel = GameObject.FindWithTag("BG");

        BGImg_List.Clear();
        i = 0;
        foreach (Transform child in BGImagePanel.transform)
        {
            //Debug.Log(child.name);           
            BGImg_List.Add(child.gameObject);
            BGImg_List[i].SetActive(false);
            i++;
        }

        switch (GameMgr.SceneSelectNum)
        {
            case 0: //春エリア

                GameMgr.Scene_Name = "Or_Hiroba_Summer_SweetsHouse";
                BGImagePanel.transform.Find("BG_sprite_1").gameObject.SetActive(true);
                GameMgr.Window_CharaName = "ノア";
                GameMgr.System_Shop_text1 = "やあ。我が店へようこそ！";
                break;

            default:

                break;
        }



        //ネームプレートの設定
        npc_hiroba_main_Controller.SceneNamePlateSetting();

        //お店の初期メッセージ
        npc_hiroba_main_Controller.SceneDefaultMessage();

        //シーン読み込み完了時のメソッド
        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        //SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド。自分自身のシーン読み込み時でも発動する。 
    }

    // Update is called once per frame
    void Update()
    {
        npc_hiroba_main_Controller.UpdateFarmScene();
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
