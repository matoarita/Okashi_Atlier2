using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaceNamePanel : MonoBehaviour {

    private string _text;
    private Text _paneltext;
    private int i;

    private ItemMatPlaceDataBase matplace_database;

    // Use this for initialization
    void Start () {

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        _paneltext = this.transform.Find("Image/Place_Name").GetComponent<Text>();

        switch (SceneManager.GetActiveScene().name) //初回に、広場シーンを読み込むと、こちらが読み込まれる。OnSceneLoadedは読まれない。
        {
            case "Shop":

                _text = "プリンのお菓子店";
                break;

            case "Bar":

                for (i = 0; i < matplace_database.matplace_lists.Count; i++)
                {
                    if (matplace_database.matplace_lists[i].placeName == "Bar")
                    {
                        _text = matplace_database.matplace_lists[i].placeNameHyouji;
                    }
                }
                break;

            case "Farm":

                for (i = 0; i < matplace_database.matplace_lists.Count; i++)
                {
                    if (matplace_database.matplace_lists[i].placeName == "Farm")
                    {
                        _text = matplace_database.matplace_lists[i].placeNameHyouji;
                    }
                }
                break;

            case "Emerald_Shop":

                _text = "エメラルショップ";
                break;

            case "Contest":

                _text = "コンテスト会場";
                break;

            default:

                //オランジーナは、こっちが中心
                switch (GameMgr.Scene_Category_Num)
                {
                    case 20: //オランジーナショップ

                        _text = "オランジーナのお菓子店";
                        break;

                    case 30: //オランジーナ酒場

                        _text = "オランジーナの酒場";
                        
                        break;

                    case 60: //オランジーナ広場系

                        switch (GameMgr.Scene_Name)
                        {
                            case "Or_Hiroba_CentralPark": //中央噴水


                                break;

                            case "Or_Hiroba_CentralPark2": //中央噴水のお散歩小道


                                break;

                            case "Or_Hiroba_Spring_Entrance": //春のエリア入口


                                break;

                            case "Or_Hiroba_Spring_Shoping_Moll": //春のエリア商店街


                                break;

                            case "Or_Hiroba_Spring_Oku": //春のエリア商店街


                                break;

                            case "Or_Hiroba_Spring_UraStreet": //春のエリア商店街


                                break;

                            case "Or_Hiroba_Summer_Entrance": //夏のエリア入口


                                break;

                            case "Or_Hiroba_Autumn_Entrance": //秋のエリア入口


                                break;

                            case "Or_Hiroba_Winter_Entrance": //冬のエリア入口

                                _text = "スノーマンズ・レスト";
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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
