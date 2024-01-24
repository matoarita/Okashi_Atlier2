using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MagicSkillListController : MonoBehaviour
{
    //
    //ショップの品揃えのコントローラー
    //

    private GameObject content; //Scroll viewのcontentを取得するための、一時的な変数
    public List<GameObject> _skill_listitem = new List<GameObject>(); //リストビューの個数　テキスト表示用のプレファブのインスタンスを格納する。
    private int list_count; //リストビューに現在表示するリストの個数をカウント

    private Text[] _text = new Text[2];
    private Sprite texture2d;
    private Sprite touchon, touchoff;
    private Image _Img;
    private Image _togglebg;
    private magicskillSelectToggle _toggle_itemID;

    private Girl1_status girl1_status;

    private GameObject skill_Prefab; //ItemPanelのプレファブの内容を取得しておくための変数。プレファブをスクリプトで制御する場合は、一度ゲームオブジェクトに読み込んでおく。

    private MagicSkillListDataBase magicskill_database;

    private int max;
    private int count;
    private int i;
    private int rnd;
    private int shop_hyouji_flag;

    //一時保存用変数
    public int skill_count; //選択したリスト番号が入る。
    public int skill_kettei_ID; //スキルデータベースIDが入る。
    public int skill_Type;
    public int skill_cost; //消費MP
    public string skill_Name;
    public string skill_itemName_Hyouji; //最終的なスキル名がはいる。

    public bool skill_final_select_flag;

    public List<GameObject> category_toggle = new List<GameObject>();
    private int category_status;


    void Awake() //Startより手前で先に読みこんで、OnEnableの挙動のエラー回避
    {

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        skill_Prefab = (GameObject)Resources.Load("Prefabs/magicskillSelectToggle");

        //アイコン背景画像データの取得
        touchon = Resources.Load<Sprite>("Sprites/Window/sabwindowB");
        touchoff = Resources.Load<Sprite>("Sprites/Window/checkbox");

        foreach (Transform child in this.transform.Find("CategoryView/Viewport/Content/").transform)
        {
            //Debug.Log(child.name);           
            category_toggle.Add(child.gameObject);
        }

        i = 0;
        category_status = 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        //ウィンドウがアクティヴになった瞬間だけ読み出される
        //Debug.Log("OnEnable");

        for (i = 0; i < category_toggle.Count; i++)
        {
            category_toggle[i].GetComponent<Toggle>().isOn = false;
        }
        category_toggle[0].GetComponent<Toggle>().isOn = true;
        reset_and_DrawView(0);
        
    }

    public void SkillList_DrawView()
    {
        if (category_toggle[0].GetComponent<Toggle>().isOn == true)
        {
            category_status = 0;
            reset_and_DrawView(0);
        }
    }

    public void SkillList_DrawView2()
    {
        if (category_toggle[1].GetComponent<Toggle>().isOn == true)
        {
            category_status = 1;
            reset_and_DrawView(1);
        }
    }

    public void SkillList_DrawView3()
    {
        if (category_toggle[2].GetComponent<Toggle>().isOn == true)
        {
            category_status = 2;
            reset_and_DrawView(2);
        }
    }

    public void SkillList_DrawView4()
    {
        if (category_toggle[3].GetComponent<Toggle>().isOn == true)
        {
            category_status = 3;
            reset_and_DrawView(3);
        }
    }

    public void SkillList_DrawView5()
    {
        if (category_toggle[4].GetComponent<Toggle>().isOn == true)
        {
            category_status = 4;
            reset_and_DrawView(4);
        }
    }

    public void ReDraw()
    {       

        switch(category_status)
        {
            case 0:

                reset_and_DrawView(0);
                break;

            case 1:

                reset_and_DrawView(1);
                break;

            case 2:

                reset_and_DrawView(2);
                break;

            case 3:

                reset_and_DrawView(3);
                break;

            case 4:

                reset_and_DrawView(4);
                break;

        }
    }

    // リストビューの描画部分。重要。
    void reset_and_DrawView(int _cate_status)
    {

        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }

        list_count = 0;
        _skill_listitem.Clear();

        switch(_cate_status)
        {
            case 0:

                for (i = 0; i < magicskill_database.magicskill_lists.Count; i++)
                {
                    //1～だと表示する。章によって、品ぞろえを追加する場合などに、フラグとして使用する。+ itemType=0は基本の材料系
                    if (magicskill_database.magicskill_lists[i].skillFlag == 1)
                    {
                        drawSkill();
                    }
                }
                break;


            default:

                break;
        }
        
    }


    void drawSkill()
    {
        _skill_listitem.Add(Instantiate(skill_Prefab, content.transform)); //Instantiateで、プレファブのオブジェクトのインスタンスを生成。名前を_listitem配列に順番にいれる。2つ目は、contentの子の位置に作る？という意味かも。
        _text = _skill_listitem[list_count].transform.Find("Background").GetComponentsInChildren<Text>(); //GetComponentInChildren<Text>()で、３つのテキストコンポを格納する。
        _Img = _skill_listitem[list_count].transform.Find("Background/Icon").GetComponent<Image>(); //アイテムの画像データ
        _togglebg = _skill_listitem[list_count].transform.Find("Background").GetComponent<Image>(); //アイコン背景データ

        _toggle_itemID = _skill_listitem[list_count].GetComponent<magicskillSelectToggle>();
        _toggle_itemID.toggle_skill_ID = magicskill_database.magicskill_lists[i].magicskillID; //スキルデータベース上のアイテムID。iと同じ値になる。
        _toggle_itemID.toggle_skill_type = magicskill_database.magicskill_lists[i].skillType; //スキルがパッシヴかアクティブか
        _toggle_itemID.toggle_skill_name = magicskill_database.magicskill_lists[i].skillName; //データ上のスキル名
        _toggle_itemID.toggle_skill_nameHyouji = magicskill_database.magicskill_lists[i].skillNameHyouji; //表示用の名前

        _text[0].text = magicskill_database.magicskill_lists[i].skillNameHyouji; //i = itemIDと一致する。NameHyoujiで、日本語表記で表示。;

        _text[1].text = magicskill_database.magicskill_lists[i].skillComment; //i = itemIDと一致する。スキルの説明文。

        texture2d = magicskill_database.magicskill_lists[i].skillIcon_sprite;
        _Img.sprite = texture2d;

        //お金が足りない場合は、選択できないようにする。
        /*if (PlayerStatus.player_money < shop_database.shopitems[i].shop_costprice)
        {
            _shop_listitem[list_count].GetComponent<Toggle>().interactable = false;
            //_togglebg.sprite = touchoff;
        }
        else
        {
            _shop_listitem[list_count].GetComponent<Toggle>().interactable = true;
            //_togglebg.sprite = touchon;
        }*/
        //Debug.Log("i: " + i + " list_count: " + list_count + " _toggle_itemID.toggle_shopitem_ID: " + _toggle_itemID.toggle_shopitem_ID);

        ++list_count;
    }

    
}
