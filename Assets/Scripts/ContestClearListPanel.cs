using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContestClearListPanel : MonoBehaviour {

    private GameObject specialtitle_list_view;
    private GameObject sp_titlelist_obj;
    private List<GameObject> contestlist_List = new List<GameObject>();

    private Text count_param;

    private int i;
    private int _count;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        InitSet();
    }

    void InitSet()
    {
        sp_titlelist_obj = (GameObject)Resources.Load("Prefabs/ContestClearList");
        count_param = this.transform.Find("PageParam").GetComponent<Text>();
        _count = 0;

        specialtitle_list_view = this.transform.Find("CGSelectPanel/Scroll View/Viewport/Content").gameObject;
        foreach (Transform child in specialtitle_list_view.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        contestlist_List.Clear();
        for (i = 0; i < GameMgr.contestclear_collection_list.Count; i++)
        {
            contestlist_List.Add(Instantiate(sp_titlelist_obj, specialtitle_list_view.transform));
        }

        for (i = 0; i < GameMgr.contestclear_collection_list.Count; i++)
        {
            if (GameMgr.contestclear_collection_list[i].Flag)
            {
                _count++;
                
                if(GameMgr.contestclear_collection_list[i].ItemData.itemID == 9999)
                {
                    contestlist_List[i].transform.Find("ItemImg").GetComponent<Image>().sprite = GameMgr.contestclear_collection_list[i].imgIcon_sprite;
                }
                else
                {
                    contestlist_List[i].transform.Find("ItemImg").GetComponent<Image>().sprite = GameMgr.contestclear_collection_list[i].ItemData.itemIcon_sprite;
                }
                
                contestlist_List[i].transform.Find("Text").GetComponent<Text>().text = GameMgr.contestclear_collection_list[i].titleNameHyouji;
                contestlist_List[i].transform.Find("Score").GetComponent<Text>().text = GameMgr.contestclear_collection_list[i].Score.ToString();
            }
        }

        count_param.text = _count.ToString() + " / " + GameMgr.contestclear_collection_list.Count.ToString();
    }

    public void backButton()
    {
        this.gameObject.SetActive(false);
    }
}
