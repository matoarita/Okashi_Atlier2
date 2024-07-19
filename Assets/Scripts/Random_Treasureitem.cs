using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Treasureitem : MonoBehaviour {

    private GameObject canvas;

    private SoundController sc;

    private GameObject hirobaTreasureget_Controller_obj;
    private HirobaTreasureGetController hirobaTreasureget_Controller;

    private int i;

    // Use this for initialization
    void Start () {

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        hirobaTreasureget_Controller_obj = canvas.transform.Find("HirobaTreasureGetController").gameObject;
        hirobaTreasureget_Controller = hirobaTreasureget_Controller_obj.GetComponent<HirobaTreasureGetController>();

        TresureHyoujiOnCheck();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void GetTreasureItem()
    {
        sc.PlaySe(1);
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;

        switch (this.transform.parent.gameObject.name)
        {
            case "MainList_ScrollView_05":

                GameMgr.Treature_getList[0] = 1; //1=取得したフラグ

                //青ジェム　MPが１上がる
                GameMgr.hiroba_treasureget_Num = 0; //宝箱番号
                GameMgr.hiroba_treasureget_Name = GameMgr.System_TreasureItem01; //クリスタルのこと
                
                GameMgr.hiroba_treasureget_Kosu = 1;
                PlayerStatus.player_maxmp++;

                hirobaTreasureget_Controller.EventReadingStart();
                break;

            case "MainList_ScrollView_104":

                GameMgr.Treature_getList[1] = 1; //1=取得したフラグ

                //青ジェム　MPが１上がる
                GameMgr.hiroba_treasureget_Num = 0; //宝箱番号
                GameMgr.hiroba_treasureget_Name = GameMgr.System_TreasureItem01;

                GameMgr.hiroba_treasureget_Kosu = 1;
                PlayerStatus.player_maxmp++;

                hirobaTreasureget_Controller.EventReadingStart();
                break;
        }
    }
    

    void TresureHyoujiOnCheck()
    {
        switch (this.transform.parent.gameObject.name)
        {
            case "MainList_ScrollView_05":

                if (GameMgr.Treature_getList[0] >= 1)
                {
                    this.gameObject.SetActive(false); //すでに取得済ということ
                }

                break;

            case "MainList_ScrollView_104":

                if (GameMgr.Treature_getList[1] >= 1)
                {
                    this.gameObject.SetActive(false); //すでに取得済ということ
                }

                break;
        }                
    }

}
