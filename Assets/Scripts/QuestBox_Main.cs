using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestBox_Main : MonoBehaviour {

    private GameObject text_area;
    private Text _text;

    private GameObject canvas;
    private GameObject qbox_select;
    private GameObject qbox_toggle_nouhin;
    private GameObject qbox_toggle_watch;

    private GameObject playeritemlist_onoff;
    private PlayerItemListController pitemlistController;
    private GameObject pitemlist_scrollview_init_obj;

    private GameObject backbutton_obj;

    public int qbox_status;

    // Use this for initialization
    void Start () {

        //宴オブジェクトの読み込み。
        SceneManager.LoadScene("Utage", LoadSceneMode.Additive); //宴のテキストシーンを読み込み

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        qbox_select = canvas.transform.Find("QuestBox_Select").gameObject;
        qbox_toggle_nouhin = qbox_select.transform.Find("Viewport/Content/QuestBox_Toggle_Nouhin").gameObject;
        qbox_toggle_watch = qbox_select.transform.Find("Viewport/Content/QuestBox_Toggle_Watch").gameObject;

        //戻るボタンを取得
        backbutton_obj = GameObject.FindWithTag("Canvas").transform.Find("Button_modoru").gameObject;
        backbutton_obj.SetActive(false);

        //プレイヤー所持アイテムリストパネルの取得
        pitemlist_scrollview_init_obj = GameObject.FindWithTag("PlayerItemListView_Init");
        pitemlist_scrollview_init_obj.GetComponent<PlayerItemListView_Init>().PlayerItemList_ScrollView_Init();
        playeritemlist_onoff = canvas.transform.Find("PlayeritemList_ScrollView").gameObject;
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
                qbox_select.SetActive(true);
                text_area.SetActive(true);

                //初期メッセージ
                _text.text = "おや？今日はどうしたんだい？";

                //StartCoroutine(QBox_Start());
                break;

            case 1: //クエストボックス、納品アイテム選択中シーンに入っている、というフラグ 

                break;

            case 10: //眺めている。
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

    public void OnCheck_1() //女の子にお菓子をあげる
    {
        if (qbox_toggle_nouhin.GetComponent<Toggle>().isOn == true)
        {
            qbox_toggle_nouhin.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            backbutton_obj.SetActive(false);
            playeritemlist_onoff.SetActive(true); //アイテム画面を表示。
                                                  //no.SetActive(true);
            qbox_select.SetActive(false);

            qbox_status = 1; //女の子にアイテムをあげるシーンに入っています、というフラグ。分岐があるわけではないが、0の繰り返しを避ける意味がある。

            _text.text = "作ったお菓子を納品してね。";

        }
    }

    public void OnCheck_2() //眺める（話かけて、噂を聞いたりする。）
    {
        if (qbox_toggle_watch.GetComponent<Toggle>().isOn == true)
        {
            qbox_toggle_watch.GetComponent<Toggle>().isOn = false; //isOnは元に戻しておく。

            //shop_select.SetActive(false);

            qbox_status = 10; //眺めるを押したときのフラグ

            _text.text = "あら？わたしをジックリと眺める気かい？";

        }
    }
}
