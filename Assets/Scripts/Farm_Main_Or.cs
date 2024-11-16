using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Farm_Main_Or : MonoBehaviour {

    private Farm_Main_Controller farmmain_Controller;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private int i;

    // Use this for initialization
    void Start () {

        farmmain_Controller = this.GetComponent<Farm_Main_Controller>();
        farmmain_Controller.InitSetup();

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

                GameMgr.Scene_Name = "Or_Farm";
                BGImagePanel.transform.Find("BG_sprite_1").gameObject.SetActive(true);
                GameMgr.Window_CharaName = "ポム";
                GameMgr.System_Shop_text1 = "いらっしゃ～い。";
                GameMgr.System_Shop_text2 = "朝とれたものばかりよ～！";
                GameMgr.System_Shop_text3 = "フルーツや材料は、買い取りをしてるわよ。" + "\n" + "何か売る？";
                GameMgr.System_Shop_text4 = "買う？";
                GameMgr.System_Shop_text5 = "かかります。";
                GameMgr.System_Shop_text6 = "何にしますか？";
                GameMgr.System_Shop_text7 = "売るアイテムを選択してね。";
                GameMgr.System_Shop_text8 = "売りますか？";
                GameMgr.System_Shop_text9 = "で買い取るわ。";
                break;

            case 10: //夏エリア

                GameMgr.Scene_Name = "Or_Farm_B1";
                BGImagePanel.transform.Find("BG_sprite_2").gameObject.SetActive(true);
                GameMgr.Window_CharaName = "モタリケ";
                GameMgr.System_Shop_text1 = "いらっしゃ～い。";
                GameMgr.System_Shop_text2 = "ぎょ～さん買っていきぃ！";
                GameMgr.System_Shop_text3 = "フルーツや材料は、買い取りをしてるわよ。" + "\n" + "何を売るの？";
                GameMgr.System_Shop_text4 = "買う？";
                GameMgr.System_Shop_text5 = "かかります。";
                GameMgr.System_Shop_text6 = "何にしますか？";
                GameMgr.System_Shop_text7 = "売るアイテムを選択してね。";
                GameMgr.System_Shop_text8 = "売りますか？";
                GameMgr.System_Shop_text9 = "で買い取るわ。";
                break;

            default:

                GameMgr.Scene_Name = "Or_Farm";
                BGImagePanel.transform.Find("BG_sprite_1").gameObject.SetActive(true);
                GameMgr.Window_CharaName = "モタリケ";
                GameMgr.System_Shop_text1 = "いらっしゃ～い。";
                GameMgr.System_Shop_text2 = "ぎょ～さん買っていきぃ！";
                GameMgr.System_Shop_text3 = "フルーツや材料は、買い取りをしてるわよ。" + "\n" + "何を売るの？";
                GameMgr.System_Shop_text4 = "買う？";
                GameMgr.System_Shop_text5 = "かかります。";
                GameMgr.System_Shop_text6 = "何にしますか？";
                GameMgr.System_Shop_text7 = "売るアイテムを選択してね。";
                GameMgr.System_Shop_text8 = "売りますか？";
                GameMgr.System_Shop_text9 = "で買い取るわ。";
                break;
        }

        

        //ネームプレートの設定
        farmmain_Controller.SceneNamePlateSetting();

        //お店の初期メッセージ
        farmmain_Controller.SceneDefaultMessage();

        //シーン読み込み完了時のメソッド
        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        //SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド。自分自身のシーン読み込み時でも発動する。 
    }

    // Update is called once per frame
    void Update()
    {
        farmmain_Controller.UpdateFarmScene();
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
