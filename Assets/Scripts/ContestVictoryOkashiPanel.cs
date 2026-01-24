using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContestVictoryOkashiPanel : MonoBehaviour
{
    private GameObject canvas;

    private ContestStartListDataBase conteststartList_database;
    private ItemDataBase database;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject contestvictory_list_view;
    private GameObject contest_victorycontent_obj;
    private List<GameObject> contestlist_List = new List<GameObject>();

    private GameObject contest_Contest_VictoryItemCheckPanel_obj;

    private int i;
    private int count;

    private int _id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        InitSet();
    }

    void InitSet()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //コンテスト全般データベースの取得
        conteststartList_database = ContestStartListDataBase.Instance.GetComponent<ContestStartListDataBase>();

        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        contestvictory_list_view = this.transform.Find("MainPanel/ContestScrollView/Viewport/Content").gameObject;
        contest_victorycontent_obj = (GameObject)Resources.Load("Prefabs/ContestVictoryOkashiContent");

        contest_Contest_VictoryItemCheckPanel_obj = this.transform.Find("Contest_VictoryItemCheckPanel").gameObject;
        contest_Contest_VictoryItemCheckPanel_obj.SetActive(false);

        count = 0;
        
        foreach (Transform child in contestvictory_list_view.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        contestlist_List.Clear();
        for (i = 0; i < conteststartList_database.conteststart_lists.Count; i++)
        {
            
            if (conteststartList_database.conteststart_lists[i].ContestVictory == 1 && conteststartList_database.conteststart_lists[i].Contest_Flag != 9999) //1位のやつを表示
            {
                //Debug.Log("一位コンテスト名: " + conteststartList_database.conteststart_lists[i].ContestNameHyouji);

                contestlist_List.Add(Instantiate(contest_victorycontent_obj, contestvictory_list_view.transform));
                contestlist_List[count].GetComponent<ContestVictoryOkashiContent>().toggleitem_ID = i; //リストIDをいれる

                _id = database.SearchItemIDString(conteststartList_database.conteststart_lists[i].Contest_VictoryItemData.itemName);
                contestlist_List[count].transform.Find("ContestVictory_CheckButton/ItemIcon").GetComponent<Image>().sprite = database.items[_id].itemIcon_sprite;

                contestlist_List[count].transform.Find("ContestVictory_CheckButton/ContestName").GetComponent<Text>().text = conteststartList_database.conteststart_lists[i].ContestNameHyouji;

                if (conteststartList_database.conteststart_lists[i].Contest_VictoryItemData.user_customname != "")
                {
                    contestlist_List[count].transform.Find("ContestVictory_CheckButton/OkashiName").GetComponent<Text>().text = conteststartList_database.conteststart_lists[i].Contest_VictoryItemData.user_customname;
                }
                else
                {
                    contestlist_List[count].transform.Find("ContestVictory_CheckButton/OkashiName").GetComponent<Text>().text = conteststartList_database.conteststart_lists[i].Contest_VictoryItemData.item_FullName;
                }
                

                if(conteststartList_database.conteststart_lists[i].Contest_VictoryItemData.ContestVictory_Score != 0)
                {
                    contestlist_List[count].transform.Find("ContestVictory_CheckButton/ScoreText").GetComponent<Text>().text = conteststartList_database.conteststart_lists[i].Contest_VictoryItemData.ContestVictory_Score.ToString();
                }
                else
                {
                    contestlist_List[count].transform.Find("ContestVictory_CheckButton/ScoreText").GetComponent<Text>().text = "";
                }

                count++;
            }
        }

    }

    public void backButton()
    {
        this.gameObject.SetActive(false);
    }

    public void Cancel_ContestClearCard()
    {

    }

    //コンテスト一覧画面からボタンをおされた
    public void OnDataOpenButton(int _listID)
    {
        contest_Contest_VictoryItemCheckPanel_obj.SetActive(true);
        contest_Contest_VictoryItemCheckPanel_obj.GetComponent<Contest_VictoryItemCheckPanel>().InitSetting(_listID);
    }
}
