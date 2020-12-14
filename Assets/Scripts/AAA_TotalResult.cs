using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AAA_TotalResult : MonoBehaviour {

    private GameObject canvas;

    private int player_Okashi_score;
    private int player_Okashi_lv;

    private string player_itemRank;

    private Text score_param_text;
    private Text clear_item_text;

    private Sprite texture2d;
    private Image _Img;

    private Girl1_status girl1_status;

    private float girl_love_score;
    private float recipi_archivement_score;
    private float contest_score;
    private float player_lv_score;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        score_param_text = canvas.transform.Find("ResultPanel_1/ScoreParam").GetComponent<Text>();
        clear_item_text = canvas.transform.Find("ResultPanel_1/ClearItemText").GetComponent<Text>();

        _Img = canvas.transform.Find("ResultPanel_1/ClearItemIcon").GetComponent<Image>(); //アイテムの画像データ

        ScoreKeisan();

        DrawParam();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnNextButton()
    {
        FadeManager.Instance.LoadScene("120_AutoSave", 0.3f);
    }

    void ScoreKeisan()
    {
        //トータルスコアを、HP、レシピ達成率、コンテストの採点結果などのパラメータをもとに、算出する。
        //その他、各クエストクリア時の最高得点、各お菓子の最高得点、パティシエレベル
        //アイテム発見力（見つけたレシピの本の数）

        girl_love_score = SujiMap(PlayerStatus.girl1_Love_exp, 0f, 999f, 0f, 100f);
        recipi_archivement_score = GameMgr.game_Recipi_archivement_rate;
        contest_score = SujiMap(GameMgr.contest_TotalScore, 0f, 300f, 0f, 100f);

        player_lv_score = PlayerStatus.player_renkin_lv * 10;

        player_Okashi_score = (int)(girl_love_score + recipi_archivement_score + contest_score + player_lv_score);

        //お菓子スコア　段階ごとに、パティシエレベルが決定
        if(player_Okashi_score < 100)
        {
            player_Okashi_lv = 1;
            player_itemRank = "ねこクッキー";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "neko_cookie");

        }
        else if (player_Okashi_score >= 100 && player_Okashi_score < 150)
        {
            player_Okashi_lv = 2;
            player_itemRank = "メロンパン";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "bugget");
        }
        else if (player_Okashi_score >= 150 && player_Okashi_score < 200)
        {
            player_Okashi_lv = 3;
            player_itemRank = "ジェラート";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "icecream");
        }
        else if (player_Okashi_score >= 200 && player_Okashi_score < 300)
        {
            player_Okashi_lv = 4;
            player_itemRank = "チョコレイト";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "chocolate");
        }
        else if (player_Okashi_score >= 300 && player_Okashi_score < 350)
        {
            player_Okashi_lv = 5;
            player_itemRank = "チョコレート・パフェ";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "chocolate_parfe");
        }
        else if (player_Okashi_score >= 350 && player_Okashi_score < 400)
        {
            player_Okashi_lv = 6;
            player_itemRank = "フォー・ゲット・ミー・ノット";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "house01_01");
        }
        else if (player_Okashi_score >= 400 && player_Okashi_score < 450)
        {
            player_Okashi_lv = 7;
            player_itemRank = "ティラミス";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "house01_01");
        }
        else if (player_Okashi_score >= 450 && player_Okashi_score < 500)
        {
            player_Okashi_lv = 8;
            player_itemRank = "プリンセストータ";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "house01_01");
        }
        else if (player_Okashi_score >= 500 && player_Okashi_score < 650)
        {
            player_Okashi_lv = 9;
            player_itemRank = "シュヴァルツベルダー・キルシュトルテ";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "house01_01");
        }
        else if (player_Okashi_score >= 650 && player_Okashi_score < 999)
        {
            player_Okashi_lv = 10;
            player_itemRank = "ブッシュ・ド・ノエル";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "house01_01");
        }
        else if (player_Okashi_score >= 999)
        {
            //神
            player_Okashi_lv = 11;
            player_itemRank = "オペラ";
            texture2d = Resources.Load<Sprite>("Sprites/Items/" + "house01_01");
        }
    }

    void DrawParam()
    {
        score_param_text.text = player_Okashi_score.ToString();
        clear_item_text.text = player_itemRank;
        
        _Img.sprite = texture2d;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
