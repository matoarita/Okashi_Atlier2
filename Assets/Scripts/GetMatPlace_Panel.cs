using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetMatPlace_Panel : MonoBehaviour {

    private ItemMatPlaceDataBase matplace_database;

    private GameObject canvas;
    private Texture2D texture2d;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private GameObject getmatplace_view;
    private GameObject slot_view;
    private GameObject slot_yes, slot_no;
    private Image slot_view_image;

    private GameObject matplace_toggle1;
    private GameObject matplace_toggle2;

    private GameObject text_area;
    private Text _text;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject yes_no_panel; //通常時のYes, noボタン

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject get_material_obj;
    private GetMaterial get_material;

    private int select_place_num;

    private bool Slot_view_on;
    private int slot_view_status;

    private int i;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {

        Setup();
        
    }

    void Setup()
    {
        audioSource = GetComponent<AudioSource>();

        //採取地データベースの取得
        matplace_database = ItemMatPlaceDataBase.Instance.GetComponent<ItemMatPlaceDataBase>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //調合シーンメインオブジェクトの取得
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();

        //windowテキストエリアの取得
        text_area = GameObject.FindWithTag("Message_Window");
        _text = text_area.GetComponentInChildren<Text>();

        //Yes no パネルの取得
        yes_no_panel = canvas.transform.Find("Yes_no_Panel").gameObject;

        //Yes no を判別する用のオブジェクトの取得
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        //アイテムセレクトキャンセルオブジェクトの取得
        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //材料ランダムで３つ手に入るオブジェクトの取得
        get_material_obj = GameObject.FindWithTag("GetMaterial");
        get_material = get_material_obj.GetComponent<GetMaterial>();

        getmatplace_view = this.transform.Find("GetMatPlace_View").gameObject;
        matplace_toggle1 = this.transform.Find("GetMatPlace_View/Viewport/Content/MatPlace_toggle1").gameObject;
        matplace_toggle2 = this.transform.Find("GetMatPlace_View/Viewport/Content/MatPlace_toggle2").gameObject;

        matplace_toggle1.GetComponentInChildren<Text>().text = matplace_database.matplace_lists[0].placeNameHyouji;
        matplace_toggle2.GetComponentInChildren<Text>().text = matplace_database.matplace_lists[1].placeNameHyouji;

        //採取地画面の取得
        slot_view = this.transform.Find("Slot_View").gameObject;

        slot_view_image = this.transform.Find("Slot_View/Image").gameObject.GetComponent<Image>();
        slot_yes = slot_view.transform.Find("Tansaku_panel/Yes").gameObject;
        slot_no = slot_view.transform.Find("Tansaku_panel/No").gameObject;

        select_place_num = 0;

        Slot_view_on = false;
        slot_view_status = 0;
    }

    // Update is called once per frame
    void Update () {
		
        if ( Slot_view_on == true )
        {
            //移動中のウェイトアニメ

            //採取地表示
            Slot_View();
        }
	}

    public void OnClick_Place1()
    {
        if (matplace_toggle1.GetComponent<Toggle>().isOn == true)
        {
            _text.text = matplace_database.matplace_lists[0].placeNameHyouji + "へ行きますか？" + "\n" + "探索費用：" + matplace_database.matplace_lists[0].placeCost.ToString() + "G";
            select_place_num = 0;

            Select_Pause();
        }
        
    }

    public void OnClick_Place2()
    {
        if (matplace_toggle2.GetComponent<Toggle>().isOn == true)
        {
            _text.text = matplace_database.matplace_lists[1].placeNameHyouji + "へ行きますか？" + "\n" + "探索費用：" + matplace_database.matplace_lists[1].placeCost.ToString() + "G";
            select_place_num = 1;

            Select_Pause();
        }
    }

    void Select_Pause()
    {
        itemselect_cancel.kettei_on_waiting = true; //今、トグルをおして、選択中の状態

        matplace_toggle1.GetComponent<Toggle>().interactable = false; //一時的に触れなくする
        matplace_toggle2.GetComponent<Toggle>().interactable = false;

        yes_no_panel.transform.Find("Yes").gameObject.SetActive(true);

        StartCoroutine("Place_kakunin");
    }


    IEnumerator Place_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除

                itemselect_cancel.kettei_on_waiting = false;

                yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

                //採取地確定したので、採取地の番号に従って、ランダムで３つアイテム取得＋金額を消費するメソッドへいく。
                Slot_view_on = true;
                //getmatplace_view.SetActive(false);

                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                All_Off();

                _text.text = "行き先を選んでね。";
                break;
        }

    }

    void Slot_View()
    {
        switch (slot_view_status)
        {
            case 0: //初期化

                slot_view.SetActive(true);
                yes_no_panel.SetActive(false);
                slot_view_status = 1;
                compound_Main.compound_status = 21;

                switch (select_place_num)
                {
                    case 0:

                        texture2d = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/ID003_Western-Castle_noon-1024x576");
                        // texture2dを使い、Spriteを作って、反映させる
                        slot_view_image.sprite = Sprite.Create(texture2d,
                                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                                   Vector2.zero);

                        _text.text = "わ～～！市場だーー！";
                        break;

                    case 1:

                        texture2d = Resources.Load<Texture2D>("Utage_Scenario/Texture/Bg/1_forest_a");
                        // texture2dを使い、Spriteを作って、反映させる
                        slot_view_image.sprite = Sprite.Create(texture2d,
                                                   new Rect(0, 0, texture2d.width, texture2d.height),
                                                   Vector2.zero);

                        _text.text = "すげぇ～～！森だー！";
                        break;

                    default:
                        break;
                }
                
                break;

            case 1: //入力まち

                if (yes_selectitem_kettei.onclick == true) //Yes, No ボタンが押された
                {
                    if (yes_selectitem_kettei.kettei1 == true) //探索ボタンをおした
                    {

                        get_material.GetRandomMaterials(select_place_num);

                        yes_selectitem_kettei.onclick = false;
                    }
                    else //街へ戻るをおした
                    {
                        slot_view_status = 2;

                        yes_selectitem_kettei.onclick = false;

                        _text.text = "家に戻る？";

                        slot_yes.GetComponent<Button>().interactable = false;
                        slot_no.GetComponent<Button>().interactable = false;

                        yes_no_panel.SetActive(true);

                        StartCoroutine("modoru_kakunin");
                       
                    }
                }
                break;

            case 2: //戻るかどうかの入力まち
                break;

            default:
                break;
        }
        
    }

    IEnumerator modoru_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                //Debug.Log("ok");
                //解除

                //全ての処理が完了し、家に戻るときに、以下の処理でリセット
                All_Off();

                compound_Main.compound_status = 0;

                slot_yes.GetComponent<Button>().interactable = true;
                slot_no.GetComponent<Button>().interactable = true;
                yes_no_panel.SetActive(false);

                slot_view_status = 0;
                Slot_view_on = false;
                slot_view.SetActive(false);

                yes_selectitem_kettei.onclick = false;

                _text.text = "家に戻ってきた。";

                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");
                slot_view_status = 1;

                slot_yes.GetComponent<Button>().interactable = true;
                slot_no.GetComponent<Button>().interactable = true;
                yes_no_panel.SetActive(false);

                _text.text = "";

                yes_selectitem_kettei.onclick = false;
                break;
        }

    }

    void All_Off()
    {
        //再度、セレクトできるようにする
        matplace_toggle1.GetComponent<Toggle>().isOn = false;
        matplace_toggle1.GetComponent<Toggle>().interactable = true;

        matplace_toggle2.GetComponent<Toggle>().isOn = false;
        matplace_toggle2.GetComponent<Toggle>().interactable = true;


        itemselect_cancel.kettei_on_waiting = false;

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        yes_no_panel.transform.Find("Yes").gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Setup();

        //音を鳴らす
        audioSource.PlayOneShot(sound1);

        //スロットビューは最初Off
        slot_view.SetActive(false);

        //最初は、採取地選択画面をonに。
        getmatplace_view.SetActive(true);

        //表示フラグにそって、採取地の表示/非表示の決定

        if ( matplace_database.matplace_lists[0].placeFlag == 1)
        {
            matplace_toggle1.SetActive(true);
            
        }
        else
        {
            matplace_toggle1.SetActive(false);
        }

        if (matplace_database.matplace_lists[1].placeFlag == 1)
        {
            matplace_toggle2.SetActive(true);
            
        }
        else
        {
            matplace_toggle2.SetActive(false);
        }

    }

    
}
