using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contest_VictoryItemCheckPanel : MonoBehaviour
{
    private GameObject card_view_obj;
    private CardView card_view;

    private ContestStartListDataBase conteststartList_database;

    private int _list;
    private int data_changeflag;
    private Text contest_Itemname;

    private GameObject contestscore_panel;
    private Text contestscore_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitSetting(int _list_id)
    {
        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        contest_Itemname = this.gameObject.transform.Find("NamePlate/NameText").GetComponent<Text>();
        contestscore_panel = this.gameObject.transform.Find("ScorePanel").gameObject;
        contestscore_panel.SetActive(false);
        contestscore_text = contestscore_panel.transform.Find("ScoreText").GetComponent<Text>();

        GameMgr.common_itemdatahyouji_list.Clear();

        _list = _list_id;

        //Debug.Log("リスト選択番号: " + _list + " " + conteststartList_database.conteststart_lists[_list].ContestName);
        GameMgr.common_itemdatahyouji_list.Add(conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData);
        card_view.ContestVictoryItemDataHyouji(0, 0); //1番目はresult_itemの配列, 2番目はステータス

        if (conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData.user_customname != "")
        {
            contest_Itemname.text = conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData.user_customname;
        }
        else
        {
            contest_Itemname.text = conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData.item_FullName;
        }
        Debug.Log("作品名（ユーザー入力）: " + conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData.user_customname);

        if (conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData.ContestVictory_Score != 0)
        {
            contestscore_text.text = conteststartList_database.conteststart_lists[_list].Contest_VictoryItemData.ContestVictory_Score.ToString();
        }
        else
        {
            contestscore_text.text = ""; //数字が0のときは表示しない　0で優勝できることもないので、0のときはデータ自体が空のとき。
        }

        contestscore_panel.SetActive(false);

        data_changeflag = 0;
    }

    //おかしのデータの画像と詳細を切り替える
    public void OnOkashiData_ChangeButton()
    {
        switch (data_changeflag)
        {
            case 0:

                data_changeflag = 1;

                card_view.ContestVictoryItemDataHyouji(0, 1); //1番目はresult_itemの配列, 2番目はステータス
                contestscore_panel.SetActive(true);
                break;

            case 1:

                data_changeflag = 0;

                card_view.ContestVictoryItemDataHyouji(0, 0); //1番目はresult_itemの配列, 2番目はステータス
                contestscore_panel.SetActive(false);
                break;
        }
    }

    public void CloseVictory_ItemDataPanel() //閉じるをおす
    {
        card_view.DeleteCard_DrawView();
        this.gameObject.SetActive(false);
    }
}
