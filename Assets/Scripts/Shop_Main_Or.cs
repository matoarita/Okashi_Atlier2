using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop_Main_Or : MonoBehaviour {
    

    private Shop_Main_Controller shopmain_Controller;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private int i;

    private GameObject CharacterPanel;

    // Use this for initialization
    void Start () {

        shopmain_Controller = this.GetComponent<Shop_Main_Controller>();
        shopmain_Controller.InitSetup();

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

        //キャラの設定　複数いる場合
        CharacterPanel = GameObject.FindWithTag("Character");
        i = 0;
        foreach (Transform child in CharacterPanel.transform.Find("CharacterImage").transform)　//
        {
            //Debug.Log(child.name);           
            child.gameObject.SetActive(false);
            i++;
        }

        switch (GameMgr.SceneSelectNum)
        {
            case 0: //春エリア

                GameMgr.Scene_Name = "Or_Shop_A1";
                BGImagePanel.transform.Find("BG_sprite_1").gameObject.SetActive(true);
                SettingCharacterPanel(0);
                GameMgr.Window_CharaName = "プリン";
                break;

            case 10: //夏エリア

                GameMgr.Scene_Name = "Or_Shop_B1";
                BGImagePanel.transform.Find("BG_sprite_2").gameObject.SetActive(true);
                SettingCharacterPanel(1);
                GameMgr.Window_CharaName = "プリン";
                break;

            case 20: //秋エリア

                GameMgr.Scene_Name = "Or_Shop_C1";
                BGImagePanel.transform.Find("BG_sprite_3").gameObject.SetActive(true);
                SettingCharacterPanel(2);
                GameMgr.Window_CharaName = "プリン";
                break;

            case 30: //冬エリア

                GameMgr.Scene_Name = "Or_Shop_D1";
                BGImagePanel.transform.Find("BG_sprite_4").gameObject.SetActive(true);
                SettingCharacterPanel(3);
                GameMgr.Window_CharaName = "プリン";
                break;

            default:

                GameMgr.Scene_Name = "Or_Shop_A1";
                BGImagePanel.transform.Find("BG_sprite_1").gameObject.SetActive(true);
                SettingCharacterPanel(0);
                GameMgr.Window_CharaName = "プリン";
                break;
        }

        //ネームプレートの設定
        shopmain_Controller.SceneNamePlateSetting();

        //シーン読み込み完了時のメソッド
        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        //SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    // Update is called once per frame
    void Update()
    {
        shopmain_Controller.UpdateShopScene();
    }

    void SettingCharacterPanel(int _num)
    {
        switch (_num)
        {
            case 0: //

                CharacterPanel.transform.Find("CharacterImage/CharacterImage01").gameObject.SetActive(true);
                break;

            case 1: //

                CharacterPanel.transform.Find("CharacterImage/CharacterImage02").gameObject.SetActive(true);
                break;

            case 2: //

                CharacterPanel.transform.Find("CharacterImage/CharacterImage03").gameObject.SetActive(true);
                break;

            case 3: //

                CharacterPanel.transform.Find("CharacterImage/CharacterImage04").gameObject.SetActive(true);
                break;
        }
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
