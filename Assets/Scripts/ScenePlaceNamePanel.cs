using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ScenePlaceNamePanel : MonoBehaviour {

    private string _text, _subtext;
    private Text _paneltext;
    private Text _paneltext_sub;
    private int i;

    private ItemMatPlaceDataBase matplace_database;

    // Use this for initialization
    void Start () {

    }

    void InitSetting()
    {
        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        _paneltext = this.transform.Find("Image/Place_Name").GetComponent<Text>();
        _paneltext_sub = this.transform.Find("Image/Place_Name_Sub").GetComponent<Text>();

        _text = "";
        _subtext = "";

        switch (SceneManager.GetActiveScene().name) //初回に、広場シーンを読み込むと、こちらが読み込まれる。OnSceneLoadedは読まれない。
        {

            default:

                //オランジーナは、こっちが中心
                switch (GameMgr.Scene_Category_Num)
                {
                    /*case 20: //オランジーナショップ

                        _paneltext.text = "オランジーナのお菓子店";
                        break;

                    case 30: //オランジーナ酒場

                        _paneltext.text = "オランジーナの酒場";

                        break;*/

                    case 60: //オランジーナ広場系

                        switch (GameMgr.Scene_Name)
                        {
                            case "Or_Hiroba_CentralPark": //中央噴水

                                _text = "中央噴水" + "\n" + "～ミラージュファウンテン～";                             
                                _subtext = "Mirage Foutain";
                                
                                break;

                            case "Or_Hiroba_CentralPark2": //中央噴水のお散歩小道

                                _text = "お散歩小道";
                                _subtext = "Osanpo Campo";
                                break;

                            case "Or_Hiroba_Spring_Entrance": //春のエリア入口

                                _text = "スプリングガーデン入口";
                                _subtext = "Spring Garden Entrance";
                                break;

                            case "Or_Hiroba_Spring_Shoping_Moll": //春のエリア商店街

                                _text = "スプリングガーデン商店街";
                                _subtext = "Spring Garden Shopping Street";
                                break;

                            case "Or_Hiroba_Spring_Oku": //春のエリア奥

                                _text = "秘密の花園";
                                _subtext = "Secret of FlowerGarden";
                                break;

                            case "Or_Hiroba_Spring_UraStreet": //春のエリア裏通り

                                _text = "";
                                _subtext = "";
                                break;

                            case "Or_Hiroba_Summer_Entrance": //夏のエリア入口

                                _text = "サマー・ドリームス入口";
                                _subtext = "Summer Dreams Entrance";
                                break;

                            case "Or_Hiroba_Summer_Street": //夏のエリア入口　奥側

                                _text = "";
                                _subtext = "";
                                break;

                            case "Or_Hiroba_Summer_MainStreet": //夏のエリア　メインストリート

                                _text = "サマー・ドリームス" + "\n" + "メイン通り";
                                _subtext = "Summer Dreams MainStreet";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Map": //夏のエリア　遊園地全体マップ

                                _text = "ソーダ・アイランド";
                                _subtext = "Soda Island";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Enter": //夏のエリア　遊園地全体マップ

                                _text = "ソーダ・アイランド" + "\n" + "入口広場";
                                _subtext = "Soda Island Campo";
                                break;

                                case "Or_Hiroba_Summer_ThemePark_StreetA":

                                _text = "13番街通り";
                                _subtext = "Soda 13th Street";
                                break;

                            case "Or_Hiroba_Autumn_Entrance": //秋のエリア入口

                                _text = "オータム・リーブス入口";
                                _subtext = "Autumn Leaves Entrance";
                                break;

                            case "Or_Hiroba_Autumn_Entrance_bridge": //秋エリア　入口大橋

                                _text = "メイプル大橋";
                                _subtext = "Maple Big bridge";
                                break;

                            case "Or_Hiroba_Autumn_MainStreet": //秋エリア　メインストリート

                                _text = "オータム・リーブス" + "\n" + "メイン通り";
                                _subtext = "Autumn Leaves MainStreet";
                                break;

                            case "Or_Hiroba_Autumn_DepartMae": //秋エリア　百貨店前

                                _text = "オータム・リーブス" + "\n" + "百貨店前";
                                _subtext = "Autumn Leaves Depart";
                                break;

                            case "Or_Hiroba_Autumn_BarStreet": //秋エリア　酒場通り

                                _text = "オータム・リーブス" + "\n" + "酒場通り";
                                _subtext = "Autumn Leaves BarStreet";
                                break;

                            case "Or_Hiroba_Winter_Entrance": //冬のエリア入口

                                _text = "スノーマンズ・レスト";
                                _subtext = "SnowMan's Resting Place";
                                break;

                            default:

                                break;
                        }
                        break;


                    case 100: //コンテスト系


                        break;

                    case 110: //コンテスト会場前系


                        break;

                    default:

                        break;
                }
                break;
        }

        _paneltext.text = _text;
        _paneltext_sub.text = _subtext;
    }

    public void OnSceneNamePlate()
    {
        InitSetting();
    }
	
    void SetSceneName(string _name)
    {
        for (i = 0; i < matplace_database.matplace_lists.Count; i++)
        {
            if (matplace_database.matplace_lists[i].placeName == _name)
            {
                _text = matplace_database.matplace_lists[i].placeNameHyouji;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    
}
