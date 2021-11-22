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
    private float total_costume_per;
    private Text player_rank_text;
    private Text player_shogo_text;
    private string player_shogo;

    private Sprite texture2d;
    private Image ClearImg;
    private Text ClearItemName;

    private Girl1_status girl1_status;

    private List<GameObject> ed_view_list = new List<GameObject>();
    private Dictionary<int, int> EDList;

    private int total_score;
    private Text total_score_text;

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

        foreach (Transform child in canvas.transform.Find("ResultGroup/ResultPanel_1/ED_View/Viewport/Content").transform) //
        {
            ed_view_list.Add(child.gameObject);
        }

        girllv_param_text = canvas.transform.Find("ResultGroup/ResultPanel_1/Hlv_Param").GetComponent<Text>();
        girl_exp_param_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalHeart_Param").GetComponent<Text>();
        total_recipi_count_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalRecipiCount").GetComponent<Text>();
        total_collection_count_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalCollection").GetComponent<Text>();
        total_costume_count_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalCostume").GetComponent<Text>();
        player_rank_text = canvas.transform.Find("ResultGroup/ResultPanel_1/PlayerRank").GetComponent<Text>();
        player_shogo_text = canvas.transform.Find("ResultGroup/ResultPanel_1/PlayerShogo").GetComponent<Text>();

        ClearImg = canvas.transform.Find("ResultGroup/ResultPanel_1/ClearItemIcon").GetComponent<Image>(); //アイテムの画像データ
        ClearItemName = canvas.transform.Find("ResultGroup/ResultPanel_1/ClearItemText").GetComponent<Text>();

        total_score_text = canvas.transform.Find("ResultGroup/ResultPanel_1/TotalScore").GetComponent<Text>();
        total_score = 0;

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
        //コンテストクリア時のアイテムと名前
        ClearItemName.text = GameMgr.contest_okashiSlotName + GameMgr.contest_okashiNameHyouji;
        ClearImg.sprite = database.items[GameMgr.contest_okashiID].itemIcon_sprite;

        //ハート総数とレベル
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
        total_costume_per = pitemlist.emeralditemlist_CostumeCount() / pitemlist.emeralditemlist_CostumeAllCount();

        //EDタイプ
        ChangeEDNumArray();
        ed_view_list[EDList[GameMgr.ending_number - 1]].transform.Find("Text1_on").gameObject.SetActive(true);

        //上記のパラメータをもとに、ゲームトータルスコアを計算
        //コンテストのスコア・ハート総数・コスチュームアイテム総数(＊100点）・見つけたレシピの総数(レシピ総数%＊20倍点）
        total_score = GameMgr.contest_TotalScore + PlayerStatus.girl1_Love_exp + (int)(100 * pitemlist.emeralditemlist_CostumeCount()) +
            ((int)(GameMgr.game_Recipi_archivement_rate * 20));
        total_score_text.text = total_score.ToString();

        //パティシエランク計算　トータルスコアをもとに、SS S A B C D E F 8段階
        player_rank_text.text = "";
        player_shogo = "-";
        if (total_score < 1000)
        {
            player_rank_text.text = "F";
            player_shogo = "パティシエ見習い";
        }
        else if (total_score >= 1000 && total_score < 1500)
        {
            player_rank_text.text = "D";
            player_shogo = "パティシエたまご";
        }
        else if (total_score >= 1500 && total_score < 2000)
        {
            player_rank_text.text = "C";
            player_shogo = "パティシエ半人前";
        }
        else if (total_score >= 2000 && total_score < 2500)
        {
            player_rank_text.text = "B";
            player_shogo = "パティシエ一人前";
        }
        else if (total_score >= 2500 && total_score < 3000)
        {
            player_rank_text.text = "B+";
            player_shogo = "一流パティシエ";
        }
        else if (total_score >= 3000 && total_score < 4000)
        {
            player_rank_text.text = "A";
            player_shogo = "グランド・パティシエ";
        }
        else if (total_score >= 4000 && total_score < 5000)
        {
            player_rank_text.text = "S";
            player_shogo = "パティシエ・マイスター";
        }
        else if (total_score >= 5000)
        {
            player_rank_text.text = "SS";
            player_shogo = "究極パティシエ";
        }

        //称号計算　通常は、パティシエランクに合わせて決める。特別な条件をクリアすると、特殊な称号がもらえるようにする。
        player_shogo_text.text = "";
        player_shogo_text.text = player_shogo;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    void ChangeEDNumArray() //EDnumが3~0の順なので、順番を逆に入れ替える
    {
        EDList = new Dictionary<int, int>();
        EDList.Add(3, 0);
        EDList.Add(2, 1);
        EDList.Add(1, 2);
        EDList.Add(0, 3);
    }
}
