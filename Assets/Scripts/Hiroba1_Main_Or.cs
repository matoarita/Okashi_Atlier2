﻿using System.Collections;
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

            case 1: //中央噴水２　散歩道

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

            case 103: //夏エリア  メインストリート　ショップ前

                GameMgr.Scene_Name = "Or_Hiroba_Summer_MainStreet_Shop";
                SettingBGPanel("Map103"); //Map〇〇のリスト番号を指定
                break;

            case 104: //夏エリア  メインストリート2　奥

                GameMgr.Scene_Name = "Or_Hiroba_Summer_MainStreet_Oku";
                SettingBGPanel("Map104"); //Map〇〇のリスト番号を指定
                break;

            case 105: //夏エリア  メインストリート3　ゴンドラ乗り場前

                GameMgr.Scene_Name = "Or_Hiroba_Summer_MainStreet_Gondora";
                SettingBGPanel("Map105"); //Map〇〇のリスト番号を指定
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

            case 153: //夏エリア  遊園地　観覧車広場

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_KanranShaHiroba";
                SettingBGPanel("Map153"); //Map〇〇のリスト番号を指定
                break;

            case 154: //夏エリア  遊園地　観覧車乗り場

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_KanranShaMae";
                SettingBGPanel("Map154"); //Map〇〇のリスト番号を指定
                break;

            case 155: //夏エリア  遊園地　水族館前

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_AquariumMae";
                SettingBGPanel("Map155"); //Map〇〇のリスト番号を指定
                break;

            case 156: //夏エリア  遊園地　水族館入口

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_AquariumEntrance";
                SettingBGPanel("Map156"); //Map〇〇のリスト番号を指定
                break;

            case 157: //夏エリア  遊園地　水族館メイン広場

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_AquariumMainHall";
                SettingBGPanel("Map157"); //Map〇〇のリスト番号を指定
                break;

            case 158: //夏エリア  遊園地　水族館メイン2F

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_AquariumMain2F";
                SettingBGPanel("Map158"); //Map〇〇のリスト番号を指定
                break;

            case 159: //夏エリア  遊園地　水族館ミニホール

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_AquariumMiniHall";
                SettingBGPanel("Map159"); //Map〇〇のリスト番号を指定
                break;

            case 160: //夏エリア  遊園地　水族館　大水槽

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_AquariumBigWhale";
                SettingBGPanel("Map160"); //Map〇〇のリスト番号を指定
                break;

            case 170: //夏エリア  遊園地　プール

                GameMgr.Scene_Name = "Or_Hiroba_Summer_ThemePark_Pool";
                SettingBGPanel("Map170"); //Map〇〇のリスト番号を指定
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

            case 205: //秋エリア　裏通り

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_UraStreet";
                SettingBGPanel("Map205"); //Map〇〇のリスト番号を指定
                break;

            case 206: //秋エリア　裏通りのさらに奥

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_UraStreet2";
                SettingBGPanel("Map206"); //Map〇〇のリスト番号を指定
                break;

            case 207: //秋エリア 途中の川のほとり

                GameMgr.Scene_Name = "Or_Hiroba_Autumn_Riverside";
                SettingBGPanel("Map207"); //Map〇〇のリスト番号を指定
                break;

            case 300: //冬エリア　入口

                GameMgr.Scene_Name = "Or_Hiroba_Winter_Entrance";
                SettingBGPanel("Map300"); //Map〇〇のリスト番号を指定
                
                break;

            case 301: //冬エリア　街前広場

                GameMgr.Scene_Name = "Or_Hiroba_Winter_EntranceHiroba";
                SettingBGPanel("Map301"); //Map〇〇のリスト番号を指定

                break;

            case 302: //冬エリア　広場からの通り

                GameMgr.Scene_Name = "Or_Hiroba_Winter_Street1";
                SettingBGPanel("Map302"); //Map〇〇のリスト番号を指定

                break;

            case 303: //冬エリア　メインストリート

                GameMgr.Scene_Name = "Or_Hiroba_Winter_MainStreet";
                SettingBGPanel("Map303"); //Map〇〇のリスト番号を指定

                break;

            case 304: //冬エリア　大広場

                GameMgr.Scene_Name = "Or_Hiroba_Winter_MainHiroba";
                SettingBGPanel("Map304"); //Map〇〇のリスト番号を指定

                break;

            case 305: //冬エリア　右奥の細道

                GameMgr.Scene_Name = "Or_Hiroba_Winter_Street2";
                SettingBGPanel("Map305"); //Map〇〇のリスト番号を指定

                break;

            case 306: //冬エリア　コンテスト会場前の橋

                GameMgr.Scene_Name = "Or_Hiroba_Winter_ContestBridge";
                SettingBGPanel("Map306"); //Map〇〇のリスト番号を指定

                break;

            case 320: //冬エリア　左奥の通り

                GameMgr.Scene_Name = "Or_Hiroba_Winter_Street3";
                SettingBGPanel("Map320"); //Map〇〇のリスト番号を指定

                break;

            case 321: //冬エリア　パティシエ家前

                GameMgr.Scene_Name = "Or_Hiroba_Winter_PatissierHouseMae";
                SettingBGPanel("Map321"); //Map〇〇のリスト番号を指定

                break;

            case 400: //正門前ストリート

                GameMgr.Scene_Name = "Or_Hiroba_MainGate_Street";
                SettingBGPanel("Map400"); //Map〇〇のリスト番号を指定

                break;

            case 401: //正門前ストリート２　お菓子街道　パティシエたちの屋台や魔法のお菓子がいっぱいある

                GameMgr.Scene_Name = "Or_Hiroba_MainGate_Street2_hiroba";
                SettingBGPanel("Map401"); //Map〇〇のリスト番号を指定

                break;

            case 402: //正門前ゲート

                GameMgr.Scene_Name = "Or_Hiroba_MainGate_Entrance";
                SettingBGPanel("Map402"); //Map〇〇のリスト番号を指定

                break;

            case 500: //城エリア　ストリート前の庭

                GameMgr.Scene_Name = "Or_Hiroba_Catsle_Garden";
                SettingBGPanel("Map500"); //Map〇〇のリスト番号を指定

                break;

            case 501: //城エリア　メインストリート

                GameMgr.Scene_Name = "Or_Hiroba_Catsle_MainStreet";
                SettingBGPanel("Map501"); //Map〇〇のリスト番号を指定

                break;

            case 502: //城エリア　入口受付

                GameMgr.Scene_Name = "Or_Hiroba_Catsle_MainEntrance";
                SettingBGPanel("Map502"); //Map〇〇のリスト番号を指定

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

        //ネームプレートを表示
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
