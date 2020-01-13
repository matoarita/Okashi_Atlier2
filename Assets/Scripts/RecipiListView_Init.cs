using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipiListView_Init : SingletonMonoBehaviour<RecipiListView_Init>
{
    private GameObject recipilist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject recipilist_scrollview_init;

    private GameObject canvas;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecipiList_ScrollView_Init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        recipilist_scrollview_init = (GameObject)Resources.Load("Prefabs/RecipiList_ScrollView");
        recipilist_onoff = Instantiate(recipilist_scrollview_init, canvas.transform);

        recipilist_onoff.transform.localScale = new Vector3(0.85f, 0.85f, 1.0f);
        recipilist_onoff.transform.localPosition = new Vector3(-220, 90, 0);
        recipilist_onoff.name = "RecipiList_ScrollView";
    }
}
