﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RecipiMemoController : MonoBehaviour
{

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _recipi_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text _text;
    private recipimemoSelectToggle _toggle_itemID;

    private GameObject recipiMemoButton;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;

    private GameObject canvas;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject compoBG_A;

    private string item_name;
    private int item_kosu;

    private GameObject comp_text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _comp_text; //同じく、Scene「Compund」用。

    private int max;
    private int count;
    private int i, j;

    public int _count1;

    // ドラッグ前の位置
    private Vector3 prevPos;

    //基準点（マウスの基準は左下だが、オブジェクトの基準は画面中央になるので補正する。）
    private Vector2 rootPos;

    // Use this for initialization
    void Awake()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/recipiMemoSelectToggle");

        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");

        //コンポBGパネルの取得
        compoBG_A = this.transform.parent.gameObject;

        //レシピメモボタンを取得
        recipiMemoButton = compoBG_A.transform.Find("RecipiMemoButton").gameObject;

        i = 0;

        //this.gameObject.SetActive(false);

        rootPos = new Vector3(400f, 400f, 0f); //画面の半分（400, 300）+y方向に100

        //初期位置
        this.transform.localPosition = new Vector3(250f, 50f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");

        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        reset_and_DrawView();

    }

    // リストビューの描画部分。重要。
    void reset_and_DrawView()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _recipi_listitem.Clear();

        //イベント用レシピのフラグをチェック。レシピリストから、さらに読めるものを表示。章クリア用のメモなど。
        for (i = 0; i < pitemlist.eventitemlist.Count; i++)
        {
            if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ListOn == 1 && pitemlist.eventitemlist[i].ev_memo != "non") //イベントレシピを所持してる　かつ　リスト表示がONのもの かつ　内容がnonじゃないものを表示
            {
                //Debug.Log(i);

                _recipi_listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
                _text = _recipi_listitem[list_count].GetComponentInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。

                _toggle_itemID = _recipi_listitem[list_count].transform.Find("recipiMemoSelectToggle_content").GetComponent<recipimemoSelectToggle>();
                _toggle_itemID.recipi_toggleEventitem_ID = i; //イベントアイテムIDを、リストビューのトグル自体にも記録させておく。
                _toggle_itemID.recipi_toggleitemType = 0; //イベントアイテムタイプなので、0


                j = 0;

                //調合DBの生成アイテムはローマ字表記なので、データベースから、日本語表記をひっぱってくる。
                item_name = pitemlist.eventitemlist[i].event_itemNameHyouji;

                _toggle_itemID.recipi_itemNameHyouji = item_name;

                _text.text = item_name;
                //_text.color = new Color(240f / 255f, 168f / 255f, 255f / 255f);


                ++list_count;
            }
        }

    }

    public void Cancel_Memo()
    {
        recipiMemoButton.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //ドラッグ＆ドロップ関係

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく
        prevPos = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中は位置を更新する
        transform.localPosition = eventData.position - rootPos;
        //Debug.Log("eventData.position: " + eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置に戻す
        //transform.position = prevPos;
        transform.localPosition = eventData.position - rootPos;

        //画面外にでたら、端っこあたりにでるようにする。
    }
}
