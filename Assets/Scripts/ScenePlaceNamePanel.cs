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

                                _text = "中央噴水" + "\n" + "～レインボーファウンテン～";                             
                                _subtext = "Rainbow Foutain";
                                
                                break;

                            case "Or_Hiroba_CentralPark_Castle_Street": //中央噴水

                                _text = "オランジーナキャッスル前";
                                _subtext = "Orangina Castle Street";

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

                            case "Or_Hiroba_Spring_RotenStreet": //春のエリア露店通り

                                _text = "スプリングガーデン　露店通り";
                                _subtext = "Spring Garden Festival Street";
                                break;

                            case "Or_Hiroba_Spring_RotenStreet2": //春のエリア露店通り2

                                _text = "スプリングガーデン　喫茶店前";
                                _subtext = "Spring Garden Cafe Street";
                                break;

                            case "Or_Hiroba_Spring_Oku_Garden": //春のエリア裏通り　奥の庭

                                _text = "裏通りの庭";
                                _subtext = "Backstreet Garden";
                                break;

                            case "Or_Hiroba_Spring_Out_Plain": //春のエリア　離れの草原

                                _text = "離れの草原";
                                _subtext = "Grassland outside the area";
                                break;

                            case "Or_Hiroba_Spring_Out_MagicHouseLake": //春のエリア　静けさの湖　ミラボー先生の家前

                                _text = "静けさの湖";
                                _subtext = "Lake of Tranquility";
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

                            case "Or_Hiroba_Summer_MainStreet_Gondora": //夏のエリア　ゴンドラ乗り場

                                _text = "ゴンドラ乗り場";
                                _subtext = "Summer Dreams Boat-Pier";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Map": //夏のエリア　遊園地全体マップ

                                _text = "ソーダ・アイランド";
                                _subtext = "Soda Island";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Enter": //夏のエリア　遊園地全体マップ

                                _text = "ソーダ・アイランド" + "\n" + "入口広場";
                                _subtext = "Soda Island Campo";
                                break;

                                case "Or_Hiroba_Summer_ThemePark_StreetA": //夏エリア　遊園地　右の通り

                                _text = "13番街通り";
                                _subtext = "Soda 13th Street";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_KanranShaHiroba": //夏エリア　観覧車広場

                                _text = "観覧車エリア";
                                _subtext = "Ferris wheel Area";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_KanranShaMae": //夏エリア  遊園地　観覧車乗り場

                                _text = "観覧車乗り場";
                                _subtext = "Ferris wheel Plarform";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_AquariumMae": //夏エリア  遊園地　水族館前

                                _text = "水族館エリア";
                                _subtext = "Aquarium Area";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_AquariumEntrance": //夏エリア  遊園地　水族館入口

                                _text = "水族館入口";
                                _subtext = "Aquarium Entrance";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_AquariumMainHall": //夏エリア  遊園地　水族館メイン広場

                                _text = "水族館1F - 「湖底の森」";
                                _subtext = "Forest of Lake bottom";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_AquariumMain2F": //夏エリア  遊園地　水族館メイン2F

                                _text = "水族館2F - 「翡翠の大広場」";
                                _subtext = "Jade Square";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_AquariumMiniHall": //夏エリア  遊園地　水族館ミニホール

                                _text = "暗闇のミニホール";
                                _subtext = "Mini Hall of darkness";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_AquariumBigWhale": //夏エリア  遊園地　水族館　大水槽

                                _text = "夢見る白くじら";
                                _subtext = "Aquarium for white whale";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_Pool": //夏エリア  プール入口

                                _text = "プールエリア入口";
                                _subtext = "Poolside Entrance";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_StreetA_2": //夏エリア　13番街　奥

                                //_text = "プールエリア入口";
                                //_subtext = "Soda 13th Street";
                                break;

                            case "Or_Hiroba_Summer_ThemePark_beachMae": //夏エリア　13番街　ビーチ

                                _text = "13番街　オレンジビーチ前";
                                _subtext = "Soda 13th OrangeBeach";
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

                            case "Or_Hiroba_Autumn_UraStreet": //秋エリア　裏通り

                                _text = "ミルフィーユ通り";
                                _subtext = "Millefeuille Street";
                                break;

                            case "Or_Hiroba_Autumn_UraStreet2": //秋エリア　裏通り奥

                                _text = "木枯らし小道";
                                _subtext = "Kogarashi Campiello"; //路地裏はカンピエーロになるらしい
                                break;

                            case "Or_Hiroba_Autumn_Riverside": //秋エリア 途中の川のほとり

                                _text = "黄金色の川のほとり";
                                _subtext = "Golden RiverSide";
                                break;

                            case "Or_Hiroba_Winter_Entrance": //冬のエリア入口　雪の通り

                                _text = "スノーマンズ・レスト";
                                _subtext = "SnowMan's Resting Place";
                                break;

                            case "Or_Hiroba_Winter_EntranceHiroba": //冬のエリア入口

                                _text = "スノーマンズ・レスト" + "\n" + "入口広場";
                                _subtext = "SnowMan's Resting Entrance";
                                break;

                            case "Or_Hiroba_Winter_Street1": //冬のエリア入口から奥通り

                                _text = "ランプ街道";
                                _subtext = "Lamp Street";
                                break;

                            case "Or_Hiroba_Winter_MainStreet": //冬のエリア入口から奥の広場通り

                                _text = "小街道";
                                _subtext = "SnowMan's Main Street";
                                break;

                            case "Or_Hiroba_Winter_MainHiroba": //冬のエリア入口から奥の広場通り

                                _text = "雪うさぎの大広場";
                                _subtext = "SnowMan's Rabbits Campo";
                                break;

                            case "Or_Hiroba_Winter_Street2": //冬のエリア入口から奥の広場通り

                                _text = "";
                                _subtext = "";
                                break;

                            case "Or_Hiroba_Winter_ContestBridge": //冬のエリア入口から奥の広場通り

                                _text = "ミルキー・ブリッジ";
                                _subtext = "Milky Bridge";
                                break;

                            case "Or_Hiroba_Winter_Street3": //冬のエリア入口から奥の広場通り

                                _text = "";
                                _subtext = "";
                                break;

                            case "Or_Hiroba_Winter_PatissierHouseMae": //冬のエリア入口から奥の広場通り

                                _text = "星降る夜の小広場";
                                _subtext = "Star Nights Campo";
                                break;

                            case "Or_Hiroba_MainGate_Street": //正門前ストリート

                                _text = "正門前ストリート";
                                _subtext = "MainGate Street";
                                break;

                            case "Or_Hiroba_MainGate_Street2_hiroba": //お菓子街道

                                _text = "お菓子街道";
                                _subtext = "Patissier's Street";
                                break;

                            case "Or_Hiroba_MainGate_Entrance": //正門前ゲート

                                _text = "正門前ゲート";
                                _subtext = "Orangina Town MainGate";
                                break;

                            case "Or_Hiroba_Catsle_Garden": //城エリア　前の庭

                                _text = "バラ園";
                                _subtext = "Orangina Rose Garden";
                                break;

                            case "Or_Hiroba_Catsle_MainStreet": //城エリア　メインストリート

                                _text = "オランジーナ城門";
                                _subtext = "Orangina Castle MainGate";
                                break;

                            default:

                                break;
                        }
                        break;


                    case 100: //コンテスト系


                        break;

                    case 110: //コンテスト会場前系

                        switch (GameMgr.Scene_Name)
                        {
                            case "Or_Contest_Out_Spring":

                                _text = "春のコンテスト会場";
                                _subtext = "Spring Contest Hall";
                                break;

                            case "Or_Contest_Out_Summer":

                                _text = "夏のコンテスト会場";
                                _subtext = "Summer Contest Hall";
                                break;

                            case "Or_Contest_Out_Autumn":

                                _text = "秋のコンテスト会場";
                                _subtext = "Autumn Contest Hall";
                                break;

                            case "Or_Contest_Out_Winter":

                                _text = "冬のコンテスト会場";
                                _subtext = "Winter Contest Hall";
                                break;

                        }
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
