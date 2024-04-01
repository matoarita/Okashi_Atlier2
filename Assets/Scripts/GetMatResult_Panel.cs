using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GetMatResult_Panel : MonoBehaviour
{
    private GameObject canvas;

    private ItemDataBase database;

    private SoundController sc;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    private GameObject getmatResult_panel_obj;
    private GameObject getmatResult_Image_obj;

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
        InitSetting();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (getmatResult_panel_obj.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Return)) //Enterでオフ
            {
                getmatplace_panel.GetMatResultPanelOff();
            }
        }
    }

    void InitSetting()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Comp/Image/Scroll View/Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/itemResultToggle");

        getmatplace_panel = canvas.transform.Find("GetMatPlace_Panel").GetComponent<GetMatPlace_Panel>();

        getmatResult_panel_obj = this.transform.Find("Comp").gameObject;
        getmatResult_Image_obj = this.transform.Find("Comp/Image").gameObject;
    }

    private void OnEnable()
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

        foreach (KeyValuePair<string, int> item in GameMgr.GetMat_ResultList)
        {
            itemID.Add(database.SearchItemIDString(item.Key));
            itemKosu.Add(item.Value);
            
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

    public void OnStartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        sc.PlaySe(30); //ポコ

        //まず、初期値。
        getmatResult_Image_obj.GetComponent<CanvasGroup>().alpha = 0;
        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(getmatResult_Image_obj.transform.DOLocalMove(new Vector3(0f, 50f, 0), 0.0f)
            .SetRelative()); //元の位置から30px右に置いておく。
                               //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(getmatResult_Image_obj.transform.DOLocalMove(new Vector3(0f, -50f, 0), 0.5f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px右から、元の位置に戻る。
        sequence.Join(getmatResult_Image_obj.GetComponent<CanvasGroup>().DOFade(1, 0.3f));
    }
}
