using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpecialTitleListPanel : MonoBehaviour {

    private GameObject specialtitle_list_view;
    private GameObject sp_titlelist_obj;
    private List<GameObject> sp_titlelist_List = new List<GameObject>();
    private GameObject specialtitle_badgelist_view;
    private GameObject sp_titlebadge_obj;
    private List<GameObject> sp_titlebadge_List = new List<GameObject>();

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
        sp_titlelist_obj = (GameObject)Resources.Load("Prefabs/SpecialTitleList");
        sp_titlebadge_obj = (GameObject)Resources.Load("Prefabs/titlebadge");
        specialtitle_list_view = this.transform.Find("CGSelectPanel/Scroll View/Viewport/Content").gameObject;
        specialtitle_badgelist_view = this.transform.Find("TitleBadgeList/Viewport/Content").gameObject;

        count_param = this.transform.Find("PageParam").GetComponent<Text>();
        _count = 0;

        
        foreach (Transform child in specialtitle_list_view.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in specialtitle_badgelist_view.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        //称号リスト更新
        sp_titlelist_List.Clear();
        for (i = 0; i < GameMgr.title_collection_list.Count; i++)
        {
            sp_titlelist_List.Add(Instantiate(sp_titlelist_obj, specialtitle_list_view.transform));
            sp_titlelist_List[sp_titlelist_List.Count - 1].transform.Find("Icon").gameObject.SetActive(false);
        }

        for (i = 0; i < GameMgr.title_collection_list.Count; i++)
        {
            if (GameMgr.title_collection_list[i].Flag)
            {
                _count++;
                sp_titlelist_List[i].transform.Find("Text").GetComponent<Text>().text = GameMgr.title_collection_list[i].titleNameHyouji;
                if (GameMgr.title_collection_list[i].imgIcon_sprite != null)
                {
                    sp_titlelist_List[i].transform.Find("Icon").gameObject.SetActive(true);
                    sp_titlelist_List[i].transform.Find("Icon").GetComponent<Image>().sprite = GameMgr.title_collection_list[i].imgIcon_sprite;
                }
                else
                {
                    sp_titlelist_List[i].transform.Find("Icon").gameObject.SetActive(false);
                }
            }
        }

        //称号バッチ更新
        sp_titlebadge_List.Clear();
        for (i = 0; i < GameMgr.title_collection_list.Count; i++)
        {
            if (GameMgr.title_collection_list[i].Flag)
            {
                if (GameMgr.title_collection_list[i].imgIcon_sprite != null)
                {
                    sp_titlebadge_List.Add(Instantiate(sp_titlebadge_obj, specialtitle_badgelist_view.transform));
                    sp_titlebadge_List[sp_titlebadge_List.Count - 1].transform.Find("Image").GetComponent<Image>().sprite = GameMgr.title_collection_list[i].imgIcon_sprite;
                }
            }
        }

        count_param.text = _count.ToString() + " / " + GameMgr.title_collection_list.Count.ToString();
    }


    public void backButton()
    {
        this.gameObject.SetActive(false);
    }
}
