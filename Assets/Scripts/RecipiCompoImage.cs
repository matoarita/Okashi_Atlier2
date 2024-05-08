using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipiCompoImage : MonoBehaviour {

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private Buf_Power_Keisan bufpower_keisan;

    private Text recipi_tassei_text;
    private Text exup_text;

    private int i, count;
    private float recipi_archivement_rate;

    private int cullent_exup;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {       
        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //バフ効果計算メソッドの取得
        bufpower_keisan = Buf_Power_Keisan.Instance.GetComponent<Buf_Power_Keisan>();

        recipi_tassei_text = this.transform.Find("Panel/RecipiPercent").GetComponent<Text>();
        exup_text = this.transform.Find("Panel/exup_param").GetComponent<Text>();

        this.transform.Find("Panel").gameObject.SetActive(true);

        //現在のレシピ数を更新
        databaseCompo.RecipiCount_database();

        recipi_tassei_text.text = GameMgr.game_Cullent_recipi_count + " / " + GameMgr.game_All_recipi_count + " " 
            + GameMgr.game_Recipi_archivement_rate.ToString("f2") + "%";

        //調合成功率アップパーセント表示も更新
        databaseCompo.RecipiCount_database();
        //cullent_exup = GameMgr.game_Exup_rate + bufpower_keisan.Buf_CompKakuritsu_Keisan();
        cullent_exup = GameMgr.game_Exup_rate;
        exup_text.text = "+" + cullent_exup.ToString() + "%";
    }

}
