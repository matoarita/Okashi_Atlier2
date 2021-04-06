using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AAA_TotalResult : MonoBehaviour {

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private PlayerItemList pitemlist;

    private GameObject canvas;

    private Text girllv_param_text;
    private Text girl_exp_param_text;
    private Text total_recipi_count_text;
    private Text total_collection_count_text;
    private Text total_costume_count_text;

    private Sprite texture2d;
    private Image _Img;

    private Girl1_status girl1_status;

    private List<GameObject> ed_view_list = new List<GameObject>();

    private int _collection_count;
    private int i;

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        foreach (Transform child in canvas.transform.Find("ResultGroup/ResultPanel_1/ED_View/Viewport/Content").transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            ed_view_list.Add(child.gameObject);
        }

        girllv_param_text = canvas.transform.Find("ResultGroup/ResultPanel_1/Hlv_Param").GetComponent<Text>();
        girl_exp_param_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalHeart_Param").GetComponent<Text>();
        total_recipi_count_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalRecipiCount").GetComponent<Text>();
        total_collection_count_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalCollection").GetComponent<Text>();
        total_costume_count_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalCostume").GetComponent<Text>();

        //_Img = canvas.transform.Find("ResultPanel_1/ClearItemIcon").GetComponent<Image>(); //アイテムの画像データ

        DrawParam();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnNextButton()
    {
        FadeManager.Instance.LoadScene("120_AutoSave", 0.3f);
    }
    

    void DrawParam()
    {
        

        girllv_param_text.text = PlayerStatus.girl1_Love_lv.ToString();
        girl_exp_param_text.text = PlayerStatus.girl1_Love_exp.ToString();

        //レシピパーセント表示
        databaseCompo.RecipiCount_database();
        total_recipi_count_text.text = GameMgr.game_Recipi_archivement_rate.ToString("f2") + "%";
        //GameMgr.game_Cullent_recipi_count + " / " + GameMgr.game_All_recipi_count

        //コレクションアイテムの総数を計算
        _collection_count = 0;
        for (i = 0; i < GameMgr.CollectionItems.Count; i++)
        {
            if (GameMgr.CollectionItems[i])
            {
                _collection_count++;
            }
        }
        total_collection_count_text.text = _collection_count.ToString() + " / " + GameMgr.CollectionItems.Count.ToString();

        //衣装総数を計算
        total_costume_count_text.text = pitemlist.emeralditemlist_CostumeCount().ToString() + " / " + pitemlist.emeralditemlist_CostumeAllCount().ToString();

        //EDタイプ
        ed_view_list[GameMgr.ending_number - 1].transform.Find("Text1_on").gameObject.SetActive(true);
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
