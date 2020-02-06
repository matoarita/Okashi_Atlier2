using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//主に、ウィンドウやその他のゲームオブジェクトのアクティブ／非アクティブを切り替え。
//オブジェクト自体にスクリプトをつけると、非アクティブにしたときに、入力も効かなくなってしまうため、スクリプトを分けている。

    //今は未使用。

public class keyManager : SingletonMonoBehaviour<keyManager>
{
    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    //private GameObject cardImage_obj;
    private GameObject moneystatus_onoff;
    private SetImage cardImage;

    private GameObject check_ItemDataBase_obj;
    private Check_ItemDataBase check_IDB;

    private GameObject card_view_obj;
    private CardView card_view;

    private GameObject canvas;

    private bool playeritemlist_sw;

    // Use this for initialization
    void Start () {

        if (SceneManager.GetActiveScene().name == "Hiroba") //初回に、広場シーンを読み込むと、こちらが読み込まれる。OnSceneLoadedは読まれない。
        {           
            Setup_Hiroba();          
        }

        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }
	
	// Update is called once per frame
	void Update ()
    {
        

        if (SceneManager.GetActiveScene().name == "Hiroba")
        {
            if (Input.GetKeyDown(KeyCode.Z)) //Zキーでアイテムメニューを開く。デバッグ用
            {
                
                if (playeritemlist_sw == true)
                {
                    pitemlistController_obj.SetActive(false);
                    playeritemlist_sw = false;

                    card_view.DeleteCard_DrawView();

                }
                else
                {
                    pitemlistController_obj.SetActive(true);

                    playeritemlist_sw = true;

                    pitemlistController.ResetKettei_item();


                }
            }
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(scene.name + " scene loaded");

        if (scene.name == "Hiroba")
        {
            Setup_Hiroba();
        }
    }

    void Setup_Hiroba()
    {
        //Debug.Log(scene.name + "main scene loaded");


        //カード表示用オブジェクトの取得
        card_view_obj = GameObject.FindWithTag("CardView");
        card_view = card_view_obj.GetComponent<CardView>();

        moneystatus_onoff = GameObject.FindWithTag("MoneyStatus_panel");
        //moneystatus_onoff.SetActive(false);

        playeritemlist_sw = false;
    }
}
