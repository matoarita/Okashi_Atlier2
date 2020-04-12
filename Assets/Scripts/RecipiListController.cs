﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecipiListController : MonoBehaviour {

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _recipi_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text _text;
    private recipiitemSelectToggle _toggle_itemID;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private PlayerItemList pitemlist;

    public List<GameObject> _cardImage_obj = new List<GameObject>(); //カード表示用のゲームオブジェクト
    private GameObject canvas;
    private GameObject cardPrefab;

    private Sprite texture2d;
    private Image _Img;

    private ItemDataBase database;
    private ItemCompoundDataBase databaseCompo;
    private string item_name;
    private int item_kosu;

    private Girl1_status girl1_status;

    private GameObject comp_text_area; //Scene「Compund」の、テキスト表示エリアのこと。Mainにはありません。初期化も、Compoundでメニューが開かれたときに、リセットされるようになっています。
    private Text _comp_text; //同じく、Scene「Compund」用。

    private int max;
    private int count;
    private int i, j;

    public int _count1;

    public int final_kettei_recipiitemID_1;

    public int kettei_recipiitem1; //最終的に確定したアイテムのID（アイテムデータベースのIDを同一）
    public int kettei_recipiitem2;
    public int kettei_recipiitem3;

    public int result_recipiitem;
    public int result_recipicompID; //最終的に確定したときの、コンポDBのアイテムID

    public int final_kettei_recipikosu1;
    public int final_kettei_recipikosu2;
    public int final_kettei_recipikosu3;
    public int final_select_kosu;

    public bool final_recipiselect_flag;

    private GameObject yes_button;
    private GameObject no_button;

    // Use this for initialization
    void Awake () {

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //調合組み合わせデータベースの取得
        databaseCompo = ItemCompoundDataBase.Instance.GetComponent<ItemCompoundDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = GameObject.FindWithTag("RecipiListContent");
        textPrefab = (GameObject)Resources.Load("Prefabs/recipiitemSelectToggle");

        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        i = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");

        yes_button = this.transform.Find("Yes").gameObject;
        no_button = this.transform.Find("No").gameObject;

        yes_button.SetActive(false);

        if (GameMgr.tutorial_ON == true)
        {
            no_button.SetActive(false);
        }
        else
        {
            no_button.SetActive(true);
        }
            


        final_recipiselect_flag = false;
        reset_and_DrawView();


    }

    public void RecipiList_Draw()
    {
        //アイテムリストを表示中に、アイテムを追加した場合、リアルタイムに表示を更新する

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

        
        if (GameMgr.tutorial_ON == true)
        {
            //チュートリアル中は、イベントレシピは見れないようにする。  
        }
        else
        {
            //イベント用レシピのフラグをチェック。レシピリストから、さらに読めるものを表示。章クリア用のメモなど。
            for (i = 0; i < pitemlist.eventitemlist.Count; i++)
            {
                if (pitemlist.eventitemlist[i].ev_itemKosu > 0 && pitemlist.eventitemlist[i].ev_ListOn == 1) //イベントレシピを所持してる　かつ　リスト表示がONのものを表示
                {
                    //Debug.Log(i);

                    _recipi_listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
                    _text = _recipi_listitem[list_count].GetComponentInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
                    _Img = _recipi_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ

                    _toggle_itemID = _recipi_listitem[list_count].GetComponent<recipiitemSelectToggle>();
                    _toggle_itemID.recipi_toggleEventitem_ID = i; //イベントアイテムIDを、リストビューのトグル自体にも記録させておく。
                    _toggle_itemID.recipi_toggleitemType = 0; //イベントアイテムタイプなので、0


                    j = 0;

                    //調合DBの生成アイテムはローマ字表記なので、データベースから、日本語表記をひっぱってくる。
                    item_name = pitemlist.eventitemlist[i].event_itemNameHyouji;

                    _toggle_itemID.recipi_itemNameHyouji = item_name;

                    _text.text = item_name;
                    //_text.color = new Color(153f / 255f, 89f / 255f, 201f / 255f); //9959C980

                    //画像を変更
                    texture2d = Resources.Load<Sprite>("Sprites/Icon/Book01");
                    _Img.sprite = texture2d;

                    ++list_count;
                }
            }
        }

        //調合DBのフラグをチェック
        for (i = 0; i < databaseCompo.compoitems.Count; i++)
        {
            if (databaseCompo.compoitems[i].cmpitem_flag == 1) //調合DBのフラグが1のアイテムのみ、表示。そのときに、格納されてる配列番号=iをtoggleに保持する。
            {

                //Debug.Log(i);

                _recipi_listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
                _text = _recipi_listitem[list_count].GetComponentInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
                _Img = _recipi_listitem[list_count].transform.Find("Background/Image").GetComponent<Image>(); //アイテムの画像データ

                _toggle_itemID = _recipi_listitem[list_count].GetComponent<recipiitemSelectToggle>();
                _toggle_itemID.recipi_toggleCompoitem_ID = i; //コンポアイテムIDを、リストビューのトグル自体にも記録させておく。
                _toggle_itemID.recipi_toggleitemType = 1; //コンポ調合アイテムタイプなので、1


                j = 0;

                //調合DBの生成アイテムはローマ字表記なので、アイテムデータベースから、日本語表記をひっぱってくる。
                while( j < database.items.Count )
                {
                    if (database.items[j].itemName == databaseCompo.compoitems[i].cmpitemID_result)
                    {
                        item_name = database.items[j].itemNameHyouji;
                        texture2d = database.items[j].itemIcon_sprite;
                        _toggle_itemID.recipi_itemID = j; //アイテムデータベース上の、アイテムID（コンポデータベースではない。）
                        break;
                    }
                    ++j;
                }

                _toggle_itemID.recipi_itemNameHyouji = item_name;

                _text.text = item_name;

                //画像を変更              
                _Img.sprite = texture2d;

                ++list_count;
            }
        }

    }

}
