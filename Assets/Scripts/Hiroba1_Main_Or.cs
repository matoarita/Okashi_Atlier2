using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hiroba1_Main_Or : MonoBehaviour
{ 

    private Hiroba1_Main_Controller hiroba1_mainController;

    private GameObject BGImagePanel;
    private List<GameObject> BGImg_List = new List<GameObject>();
    private List<GameObject> BGImg_List_mago = new List<GameObject>();
    private int i, _num;   

    // Use this for initialization
    void Start()
    {
        hiroba1_mainController = this.GetComponent<Hiroba1_Main_Controller>();
        hiroba1_mainController.InitSetup(); //先にコントローラーのstartは起動        

        //
        //背景と場所名の設定 最初にこれを行う
        //
        BGImagePanel = GameObject.FindWithTag("BG");
        BGImg_List.Clear();
        BGImg_List_mago.Clear();
        i = 0;
        foreach (Transform child in BGImagePanel.transform)　//子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            BGImg_List.Add(child.gameObject);
            BGImg_List[i].SetActive(false);
            i++;
        }

        switch (GameMgr.SceneSelectNum)
        {
            case 0: //中央噴水 メイン
                
                GameMgr.Scene_Name = "Or_Hiroba_CentralPark";
                SettingBGPanel("Map03"); //Map〇〇のリスト番号を指定
                break;

            case 1: //中央噴水２　冬エリアへ繋がる散歩道

                GameMgr.Scene_Name = "Or_Hiroba_CentralPark2";
                SettingBGPanel("Map02"); //Map〇〇のリスト番号を指定
                break;

            case 10: //春エリア

                GameMgr.Scene_Name = "Or_Hiroba_Spring_Entrance";
                SettingBGPanel("Map04"); //Map〇〇のリスト番号を指定
                break;

            case 11: //春エリア　商店街

                GameMgr.Scene_Name = "Or_Hiroba_Spring_Shoping_Moll";
                SettingBGPanel("Map05"); //Map〇〇のリスト番号を指定
                break;

            case 12: //春エリア　奥側

                GameMgr.Scene_Name = "Or_Hiroba_Spring_Oku";
                SettingBGPanel("Map06"); //Map〇〇のリスト番号を指定
                break;

            case 13: //春エリア　裏通り

                GameMgr.Scene_Name = "Or_Hiroba_Spring_UraStreet";
                SettingBGPanel("Map07"); //Map〇〇のリスト番号を指定
                break;

            case 100: //夏エリア

                GameMgr.Scene_Name = "Or_Hiroba_Summer_Entrance";
                SettingBGPanel("Map100"); //Map〇〇のリスト番号を指定
                break;

            case 101: //夏エリア  奥側

                GameMgr.Scene_Name = "Or_Hiroba_Summer_Street";
                SettingBGPanel("Map101"); //Map〇〇のリスト番号を指定
                break;

            case 102: //夏エリア  メインストリート

                GameMgr.Scene_Name = "Or_Hiroba_Summer_MainStreet";
                SettingBGPanel("Map102"); //Map〇〇のリスト番号を指定
                break;

            case 103: //夏エリア  メインストリート

                GameMgr.Scene_Name = "Or_Hiroba_Summer_MainStreet_Shop";
                SettingBGPanel("Map103"); //Map〇〇のリスト番号を指定
                break;

            case 150: //夏エリア  遊園地入口全体マップ

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_Map";
                SettingBGPanel("Map150"); //Map〇〇のリスト番号を指定
                break;

            case 151: //夏エリア  遊園地入口

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_Enter";
                SettingBGPanel("Map151"); //Map〇〇のリスト番号を指定
                break;

            case 152: //夏エリア  遊園地　右の通り

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_StreetA";
                SettingBGPanel("Map152"); //Map〇〇のリスト番号を指定
                break;

            case 200: //秋エリア

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_Entrance";
                SettingBGPanel("Map200"); //Map〇〇のリスト番号を指定
                break;

            case 201: //秋エリア入口　大橋

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_Entrance_bridge";
                SettingBGPanel("Map201"); //Map〇〇のリスト番号を指定
                break;

            case 202: //秋エリア　メインストリート

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_MainStreet";
                SettingBGPanel("Map202"); //Map〇〇のリスト番号を指定
                break;

            case 203: //秋エリア　百貨店前

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_DepartMae";
                SettingBGPanel("Map203"); //Map〇〇のリスト番号を指定
                break;

            case 204: //秋エリア　酒場通り

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_BarStreet";
                SettingBGPanel("Map204"); //Map〇〇のリスト番号を指定
                break;

            case 300: //冬エリア

                GameMgr.Scene_Name = "Or_Hiroba_Winter_Entrance";
                SettingBGPanel("Map300"); //Map〇〇のリスト番号を指定
                
                break;

            default:

                GameMgr.Scene_Name = "Or_Hiroba_CentralPark";
                SettingBGPanel("Map03"); //Map〇〇のリスト番号を指定
                break;
        }

        //天気対応
        if (GameMgr.WEATHER_TIMEMODE_ON)
        {
            if (GameMgr.Story_Mode != 0)
            {

                switch (GameMgr.BG_cullent_weather) //TimeControllerで変更
                {
                    case 1:

                        break;

                    case 2: //深夜→朝

                        break;

                    case 3: //朝

                        break;

                    case 4: //昼

                        break;

                    case 5: //夕方

                        BGImg_List_mago[1].gameObject.SetActive(true);
                        break;

                    case 6: //夜

                        BGImg_List_mago[2].gameObject.SetActive(true);
                        hiroba1_mainController.ToggleAllOff();
                        break;
                }
            }
        }
        //** 場所名設定ここまで **//


        //そのエリアの初期トグルをonにする
        hiroba1_mainController.SceneToggleDefaultSetup();

        //フラグをチェックし、必要ならtoggleON/OFFの追加
        hiroba1_mainController.ToggleFlagCheck();

        //テキスト設定
        hiroba1_mainController.text_scenario();

        //ネムプレートを表示
        hiroba1_mainController.SceneNamePlateSetting();

        //シーン読み込み完了時のメソッド
        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド。自分自身のシーン読み込み時でも発動する。      
        //SceneManager.sceneUnloaded += OnSceneUnloaded;  //アンロードされるタイミングで呼び出しされるメソッド
    }

    

    // Update is called once per frame
    void Update()
    {
        hiroba1_mainController.UpdateHiroba1MainScene();        
    }

    void SettingBGPanel(string _name)
    {
        i = 0;
        while(i < BGImg_List.Count)
        {
            if(BGImg_List[i].name == _name)
            {
                _num = i;
                break;
            }
             //マップオブジェクト名に一致するもの探す
            i++;
        }

        BGImg_List[_num].gameObject.SetActive(true);

        i = 0;
        foreach (Transform child in BGImg_List[_num].transform) //子要素（孫は取得しない）までなら、childでOK
        {
            //Debug.Log(child.name);           
            BGImg_List_mago.Add(child.gameObject);
            BGImg_List_mago[i].SetActive(false);
            i++;
        }
        BGImg_List_mago[0].gameObject.SetActive(true); //朝の画像一番上オブジェクトをON
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
