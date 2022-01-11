using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CGGalleryPanel : MonoBehaviour {

    private GameObject event_list_view;
    private GameObject eventlist_obj;
    private List<GameObject> eventlist_List = new List<GameObject>();

    private GameObject RButton_obj;
    private GameObject LButton_obj;
    private GameObject CGSelect_panel;
    private GameObject backbutton_obj;

    private Text count_param;

    private int i;
    private int _count;

    private int page_stillcount = 4; //１ページの表示枚数
    private int max_page;
    private int max_page_nokori;

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
        eventlist_obj = (GameObject)Resources.Load("Prefabs/GalleryPanel");
        count_param = this.transform.Find("PageParam").GetComponent<Text>();       

        CGSelect_panel = this.transform.Find("CGSelectPanel").gameObject;
        backbutton_obj = this.transform.Find("No").gameObject;
        event_list_view = this.transform.Find("CGSelectPanel/Scroll View/Viewport/Content").gameObject;
        RButton_obj = this.transform.Find("CGSelectPanel/RButton").gameObject;
        LButton_obj = this.transform.Find("CGSelectPanel/LButton").gameObject;       

        //ページ数表示の設定　１ページ4枚ずつ表示
        max_page_nokori = GameMgr.event_collection_list.Count % page_stillcount;
        if(max_page_nokori > 0)
        {
            max_page = (GameMgr.event_collection_list.Count / page_stillcount) + 1;
        }
        else
        {
            max_page = GameMgr.event_collection_list.Count / page_stillcount;
        }

        //1ページ目から表示
        Page_Koushin();
    }

    void Page_Koushin()
    {
        //まず全て消す。
        foreach (Transform child in event_list_view.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        eventlist_List.Clear();

        //もし、現在ページが最後のページでなければ、4枚ずつ表示してOK
        if (_count != max_page)
        {
            for (i = 0; i < page_stillcount; i++)
            {
                eventlist_List.Add(Instantiate(eventlist_obj, event_list_view.transform));
            }         
        }
        else //もし最後のページで..
        {
            //さらに端数がある場合は、残り枚数を表示
            if (max_page_nokori > 0) 
            {
                for (i = 0; i < max_page_nokori; i++)
                {
                    eventlist_List.Add(Instantiate(eventlist_obj, event_list_view.transform));
                }
            }
            else //端数がない場合は、ぴったりなので、4枚表示
            {
                for (i = 0; i < page_stillcount; i++)
                {
                    eventlist_List.Add(Instantiate(eventlist_obj, event_list_view.transform));
                }
            }
        }

        for (i = 0; i < eventlist_List.Count; i++)
        {
            if (GameMgr.event_collection_list[(_count - 1) * page_stillcount + i].Flag) //現在のページ-1で配列になおし、そこから各1~4枚を設定
            {
                eventlist_List[i].transform.Find("Text").GetComponent<Text>().text = GameMgr.event_collection_list[(_count - 1) * page_stillcount + i].titleNameHyouji;
                eventlist_List[i].transform.Find("Img").GetComponent<Image>().sprite = GameMgr.event_collection_list[(_count - 1) * page_stillcount + i].imgIcon_sprite;
                eventlist_List[i].GetComponent<Button>().interactable = true;
                eventlist_List[i].GetComponent<GalleryPanel>()._id = (_count - 1) * page_stillcount + i; //IDも振っておく。GameMgr.event_collection_listの配列と一緒。
            }
        }

        count_param.text = _count.ToString() + " / " + max_page.ToString();

        //ボタンの表示判定
        LButton_obj.SetActive(true);
        RButton_obj.SetActive(true);
        if (_count <= 1)
        {
            LButton_obj.SetActive(false);
        }

        if(_count >= max_page)
        {
            RButton_obj.SetActive(false);
        }
    }

    public void RPageButton()
    {
        _count++;
        if (_count >= max_page)
        {
            _count = max_page;
        }
        Page_Koushin();
    }

    public void LPageButton()
    {
        _count--;
        if(_count <= 1)
        {
            _count = 1;
        }
        Page_Koushin();
    }

    //一時的にパネルの入力をオフ
    public void OffInteractPanel()
    {
        RButton_obj.GetComponent<Button>().interactable = false;
        LButton_obj.GetComponent<Button>().interactable = false;

        for (i = 0; i < eventlist_List.Count; i++)
        {
            if (GameMgr.event_collection_list[_count - 1 + i].Flag) //現在のページ-1で配列になおし、そこから各1~4枚を設定
            {
                eventlist_List[i].GetComponent<Button>().interactable = false;
            }
        }

    }

    //パネル入力を基に戻す
    public void OnInteractPanel()
    {
        RButton_obj.GetComponent<Button>().interactable = true;
        LButton_obj.GetComponent<Button>().interactable = true;

        for (i = 0; i < eventlist_List.Count; i++)
        {
            if (GameMgr.event_collection_list[_count - 1 + i].Flag) //現在のページ-1で配列になおし、そこから各1~4枚を設定
            {
                eventlist_List[i].GetComponent<Button>().interactable = true;
            }
        }

    }

    public void PageReset() //パネルを開くときだけ、リセット
    {
        _count = 1; //現在のページ数
    }

    public void backButton()
    {
        this.gameObject.SetActive(false);
    }
}
