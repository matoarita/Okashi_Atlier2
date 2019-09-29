using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestBox_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;

    private GameObject backbutton_obj;

    public int qbox_status;

    // Use this for initialization
    void Start () {

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;
        backbutton_obj.SetActive(false);

        //所持アイテム画面を開く。初期設定で最初はOFF。
        playeritemlist_onoff = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = playeritemlist_onoff.GetComponent<PlayerItemListController>();

        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        qbox_status = 0;
    }
	
	// Update is called once per frame
	void Update () {

        switch (qbox_status)
        {
            case 0:

                playeritemlist_onoff.SetActive(false);
                backbutton_obj.SetActive(true);
                text_area.SetActive(true);

                StartCoroutine(QBox_Start());
                break;

            case 1: //クエストボックス、納品アイテム選択中シーンに入っている、というフラグ 

                break;

            default:

                break;
        }
    }

    IEnumerator QBox_Start()
    {
        //初期メッセージ
        _text.text = "作ったお菓子を納品してね。";

        qbox_status = 1;

        while (!Input.GetMouseButtonDown(0)) yield return null; //マウス左クリックが押されるまで待機する。コルーチンが動いていても、Updateは、常に更新されている。

        yield return new WaitForSeconds(0.2f); //○○秒待つ        

        backbutton_obj.SetActive(false);
        playeritemlist_onoff.SetActive(true); //アイテム画面を表示。


    }
}
