using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryPanel : MonoBehaviour {

    public int _id; //自分自身の番号

    private GameObject Omake_Main;
    private GameObject CGGallery_panel;

    private GameObject canvas;
    private SoundController sc;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        Omake_Main = GameObject.FindWithTag("Omake_Main");
        CGGallery_panel = canvas.transform.Find("CGGalleryPanel").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnEventSceneWatch()
    {
        //宴を呼び出し。
        GameMgr.scenario_ON = true;
        GameMgr.CGGallery_readflag = true;
        GameMgr.CGGallery_num = GameMgr.event_collection_list[_id].ID; //イベント番号ごとに固有のIDがふられている。

        Omake_Main.GetComponent<Omake_Main>().ReadCGGallery();
        CGGallery_panel.GetComponent<CGGalleryPanel>().OffInteractPanel();

        sc.PlaySe(2);
    }  
}
