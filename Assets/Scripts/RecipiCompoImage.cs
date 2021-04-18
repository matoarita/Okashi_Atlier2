using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipiCompoImage : MonoBehaviour {

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;

    private Text recipi_tassei_text;

    private int i, count;
    private float recipi_archivement_rate;

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

        recipi_tassei_text = this.transform.Find("Panel/RecipiPercent").GetComponent<Text>();

        this.transform.Find("Panel").gameObject.SetActive(true);

        //現在のレシピ数を更新
        databaseCompo.RecipiCount_database();

        recipi_tassei_text.text = GameMgr.game_Cullent_recipi_count + " / " + GameMgr.game_All_recipi_count + " " 
            + GameMgr.game_Recipi_archivement_rate.ToString("f2") + "%";
    }

}
