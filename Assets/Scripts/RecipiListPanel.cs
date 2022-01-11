using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipiListPanel : MonoBehaviour {

    private GameObject recipilist_onoff;

    private Text RecipiCount;
    private Text RecipiCountPer;

    private ItemCompoundDataBase databaseCompo;

    // Use this for initialization
    void Start () {

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        recipilist_onoff = GameObject.FindWithTag("RecipiList_ScrollView");

        RecipiCount = this.transform.Find("PageParam").GetComponent<Text>();
        RecipiCountPer = this.transform.Find("PageParamPercent").GetComponent<Text>();

        //レシピパーセント表示
        databaseCompo.RecipiCount_database();
        //total_recipi_count_text.text = GameMgr.game_Recipi_archivement_rate.ToString("f2") + "%";
        RecipiCountPer.text = GameMgr.game_Recipi_archivement_rate.ToString("f2") + "%";
        RecipiCount.text = GameMgr.game_Cullent_recipi_count + " / " + GameMgr.game_All_recipi_count;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void backButton()
    {
        recipilist_onoff.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
