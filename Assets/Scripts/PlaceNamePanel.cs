using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaceNamePanel : MonoBehaviour {

    private string _text;
    private Text _paneltext;
    private int i;
    private string _placename;

    private ItemMatPlaceDataBase matplace_database;

    // Use this for initialization
    void Start () {

        InitSetting();
        
    }

    void InitSetting()
    {
        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        _paneltext = this.transform.Find("Image/Place_Name").GetComponent<Text>();

        switch (SceneManager.GetActiveScene().name) //初回に、広場シーンを読み込むと、こちらが読み込まれる。OnSceneLoadedは読まれない。
        {
            case "Shop":

                _text = "プリンのお菓子店";
                break;

            case "Bar":

                SetSceneName("Bar"); //マップDBに登録されているmap_Nameをもとに、マップ名をテキストに入れる
                break;

            case "Farm":

                SetSceneName("Farm"); //マップDBに登録されているmap_Nameをもとに、マップ名をテキストに入れる
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

                        SetSceneName(GameMgr.Scene_Name);

                        break;

                    case 30: //オランジーナ酒場

                        SetSceneName(GameMgr.Scene_Name);

                        break;

                    case 60: //オランジーナ広場系 広場系は、ScenePlaceNamePanelで設定

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

    public void OnSceneNamePlate()
    {
        InitSetting();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
