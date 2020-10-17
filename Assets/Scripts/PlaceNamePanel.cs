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

                for (i = 0; i < matplace_database.matplace_lists.Count; i++)
                {
                    if (matplace_database.matplace_lists[i].placeName == "Shop")
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

                _text = "？？？";
                break;

            case "Contest":

                _text = "コンテスト会場";
                break;
        }

        _paneltext.text = _text;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
