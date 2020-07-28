using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMatResult_Panel : MonoBehaviour
{
    private GameObject canvas;

    private ItemDataBase database;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    private int list_count;
    private int i, count;

    private string item_name;
    private int item_kosu;

    private Sprite texture2d;
    private Image _Img;

    private Text[] _text = new Text[3];

    private List<int> itemID = new List<int>();
    private List<int> itemKosu = new List<int>();

    public List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。

    private GetMatPlace_Panel getmatplace_panel;

    // Use this for initialization
    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Comp/Image/Scroll View/Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/itemResultToggle");

        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // リストビューの描画部分。重要。
    public void reset_and_DrawView()
    {
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _listitem.Clear();
        itemID.Clear();
        itemKosu.Clear();

        for (i=0;  i < getmatplace_panel.result_items.Count; i++)
        {
            if(getmatplace_panel.result_items[i] > 0)
            {
                itemID.Add(i);
                itemKosu.Add(getmatplace_panel.result_items[i]);
            }
        }
        

        //表示
        for (i = 0; i < itemID.Count; i++)
        {
            itemlist_hyouji();
        }
    }

    void itemlist_hyouji()
    {
        //Debug.Log(i);
        _listitem.Add(Instantiate(textPrefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _listitem[list_count].GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、さっき_listitem[i]に入れたインスタンスの中の、テキストコンポーネントを、_textにアタッチ。_text.textで、内容を変更可能。
        _Img = _listitem[list_count].transform.Find("ItemIcon").GetComponent<Image>(); //アイテムのアイコン


        item_name = database.items[itemID[i]].itemNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。

        _text[0].text = item_name;

        _text[1].text = itemKosu[i].ToString(); //獲得個数

        //画像を変更
        texture2d = database.items[itemID[i]].itemIcon_sprite;
        _Img.sprite = texture2d;

        ++list_count;
    }
}
