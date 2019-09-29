using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TravelListController : MonoBehaviour {

    public List<bool> travelSelected = new List<bool>(); //現在選択しているアイテム用のリスト。

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    private GameObject travelpanel_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    public List<GameObject> _travellistitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。

    private WorldDataBase worlddatabase;

    private int list_count;
    private int i, max;

    private Text _text;

    public int kettei_travel;

    // Use this for initialization
    void Start () {

        //世界・街情報の取得
        worlddatabase = WorldDataBase.Instance.GetComponent<WorldDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("TravelListContent");
        travelpanel_Prefab = (GameObject)Resources.Load("Prefabs/travelSelectToggle");

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        _travellistitem.Clear();
        list_count = 0;

        max = PlayerStatus.player_travelList.Count;

        //描画部分
        for (i = 0; i < max; i++)
        {
            _travellistitem.Add(Instantiate(travelpanel_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
            _text = _travellistitem[list_count].GetComponentInChildren<Text>();

            _text.text = worlddatabase.travel_name[list_count];

            ++list_count;
        }

        travelSelected.Clear();

        for (i = 0; i < max; i++)
        {
            travelSelected.Add(false); //未選択状態のもので埋めておく。
            //Debug.Log(itemSelected[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
