using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CatZairyoCheckPanel : MonoBehaviour
{
    private GameObject canvas;

    private ItemDataBase database;
    private CatDataBase catDataBase;

    private SoundController sc;

    private GameObject textPrefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。
    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数

    private GameObject getmatResult_Image_obj;

    private int list_count;
    private int i, count;
    private int CatID;

    private string item_name;
    private int item_kosu;

    private Sprite texture2d;
    private Image _Img;

    private Text[] _text = new Text[3];

    private List<int> itemID = new List<int>();
    private List<int> itemKosu = new List<int>();

    public List<GameObject> _listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitSetting()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //ねこデータベースの取得
        catDataBase = CatDataBase.Instance.GetComponent<CatDataBase>();

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Comp/CatDataPanel/GetMatPanel/Scroll View/Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/CatitemResultToggle");

        getmatResult_Image_obj = this.transform.Find("Comp/CatDataPanel").gameObject;
    }

    private void OnEnable()
    {
        InitSetting();        
    }

    public void OpenPanel(int _catid)
    {
        CatID = _catid;

        FinalCheck_CatDataKoushin(this.transform.Find("Comp").gameObject, CatID);

        reset_and_DrawView();
        OnStartAnim();
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

        i = 0;
        while(i < catDataBase.catdata_list[CatID].getmat_itemname_cat.Length)
        {
            //最初のNonがきたら、以降空なのでそこで表示終了
            if(catDataBase.catdata_list[CatID].getmat_itemname_cat[i] == "Non")
            {
                break;
            }
            else
            {
                itemID.Add(database.SearchItemIDString(catDataBase.catdata_list[CatID].getmat_itemname_cat[i]));
                itemKosu.Add(catDataBase.catdata_list[CatID].getmat_kosu_cat[i]);
            }            
            i++;
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

    void FinalCheck_CatDataKoushin(GameObject _obj, int _catid)
    {
        //アニメのほうを表示
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").gameObject.SetActive(false);
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.SetActive(true);

        //ねこの画像
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIcon").GetComponent<Image>().sprite = catDataBase.SetSprite(_catid, 0);

        //ねこの画像アニメ
        foreach (Transform child in _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim").gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
        _obj.transform.Find("CatDataPanel/CatIconBaseImg/CatIconAnim/" + catDataBase.SetCatAnimObj(_catid, 0)).gameObject.SetActive(true);

        //ねこの名前
        _obj.transform.Find("CatDataPanel/NameText").GetComponent<Text>().text = catDataBase.catdata_list[_catid].catnameHyouji;
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
