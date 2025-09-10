using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CardView : SingletonMonoBehaviour<CardView>
{

    private int i;

    private ItemDataBase database;

    private GameObject ResultCardView_content_obj;

    private List<GameObject> _cardImage_obj = new List<GameObject>(); //カード表示用のゲームオブジェクト

    private SetImage _cardImage; //カードの描画処理は、SetImageスクリプト
    private GameObject canvas;
    private GameObject cardPrefab;

    private GameObject selectitem_kettei_obj;
    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    public int Pitem_or_Origin_judge; //店売りアイテムか、オリジナルアイテムの判定
    public int DrawStatus; //店売りアイテムをひらいたとき、そのカードの位置の状態を保持　shopitemSelectToggleから読む

    private Transform resulttransform;
    private Vector3 resultScale;
    private Vector3 resultPos;

    private GameObject toppingItemRoot;

    private float _Scale,  _Pos;
 
    private Vector3 _diff_pos;
    private Vector3 _temp_nowpos;
    private Vector3 _temp_nowrot;

    private float timeOut;

    private float speed;
    private List<Vector3> _now_cardpos = new List<Vector3>();
    private List<Vector3> _now_cardrot = new List<Vector3>();
    private List<float> _diff_x = new List<float>();
    private List<float> _diff_y = new List<float>();
    private List<float> radius = new List<float>();
    private List<Vector3> _diff_rot = new List<Vector3>();
    private float rot_speed_x;
    private float rot_speed_y;
    private float rot_speed_z;
    private int _movetime;
    public bool cardcompo_anim_on;

    private int Select_SortingNum = 6000;

    // Use this for initialization
    void Start () {

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        InitSetting();

        //timeOut = 1.0f / 60.0f;

        /*switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                ResultCardView_content_obj = canvas.transform.Find("ResultCardView/Viewport/Content").gameObject;
                break;

            default://シナリオ系のシーンでは読み込まない。
                break;
        }*/
    }

    void InitSetting()
    {
        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        toppingItemRoot = canvas.transform.Find("ToppingItemRoot/Obj/ScrollView/Viewport/Content").gameObject;

        Pitem_or_Origin_judge = 0;
        DrawStatus = 0;

        speed = 2.0f;
    }

    // Update is called once per frame
    void Update () {

        if(canvas == null)
        {
            InitSetting();

        }

        if (cardcompo_anim_on == true)
        {
            timeOut -= Time.deltaTime;

            if (timeOut <= 0.0f)
            {
                timeOut = 1.0f / 60.0f; //60Fで割る

                for (i = 0; i < _cardImage_obj.Count; i++)
                {
                    //位置の計算
                    _temp_nowpos = _now_cardpos[i];


                    if (_diff_x[i] > 0) //diffが正か負かをみる　正なら0より右に位置　負なら0より左に位置
                    {
                        if (_temp_nowpos.x <= 0)
                        {
                            _temp_nowpos.x = 0;
                        }
                        else
                        {
                            _temp_nowpos.x += (_diff_x[i] * (-1.0f));
                        }
                    }
                    else //こっちは負の場合
                    {
                        if (_temp_nowpos.x >= 0)
                        {
                            _temp_nowpos.x = 0;
                        }
                        else
                        {
                            _temp_nowpos.x += (_diff_x[i] * (-1.0f));
                        }
                    }

                    if (_diff_y[i] > 0) //diffが正か負かをみる　正なら0より右に位置　負なら0より左に位置
                    {
                        if (_temp_nowpos.y <= 0)
                        {
                            _temp_nowpos.y = 0;
                        }
                        else
                        {
                            _temp_nowpos.y += (_diff_y[i] * (-1.0f));
                            //_temp_nowpos.y = (_temp_nowpos.y - radius[i] * Mathf.Cos(Time.time * speed));
                        }
                    }
                    else //こっちは負の場合
                    {
                        if (_temp_nowpos.y >= 0)
                        {
                            _temp_nowpos.y = 0;
                        }
                        else
                        {
                            _temp_nowpos.y += (_diff_y[i] * (-1.0f));
                            //_temp_nowpos.y = (_temp_nowpos.y - radius[i] * Mathf.Cos(Time.time * speed));
                        }
                    }

                    //位置の更新
                    _now_cardpos[i] = _temp_nowpos;
                    _cardImage_obj[i].transform.localPosition = _now_cardpos[i];


                    //回転の更新
                    _temp_nowrot = _now_cardrot[i];
                    _temp_nowrot += _diff_rot[i];
                    _now_cardrot[i] = _temp_nowrot;

                    _cardImage_obj[i].transform.localEulerAngles = _now_cardrot[i];

                }
            }
        }

    }

    //カード表示処理
    public void SelectCard_DrawView(int _toggleType, int _kettei_item1)
    {
        //初期化しておく
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }
        _cardImage_obj.Clear();


        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();

        //店売りかオリジナルか、アイテムID
        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item1;
        _cardImage.SetInit();
        _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();
        //_cardImage_obj[0].GetComponent<SetImage>().SlotChangeButtonON();

        //位置とスケール
        _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 80, 0);

        //デバッグ用
        if (SceneManager.GetActiveScene().name == "Hiroba")
        {
            _cardImage_obj[0].transform.localScale = new Vector3(1.0f, 1.0f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-170, 80, 0);
        }

    }

    //全てのカードを削除する。
    public void DeleteCard_DrawView()
    {
        for (i = 0; i < _cardImage_obj.Count; i++) //最後から削除していく。
        {
            Destroy(_cardImage_obj[(_cardImage_obj.Count-1) - i]);
        }

        _cardImage_obj.Clear();

    }

    public void OKCard_DrawView(int _kosu)
    {

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);
            _cardImage_obj[0].GetComponent<SetImage>().Kosu_ON(_kosu);
            _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF();
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(50, 100, 0);
            _cardImage_obj[0].transform.localPosition = new Vector3(135, -15, 0);

            _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();
            _cardImage_obj[0].GetComponent<SetImage>().CardOFF_ItemOnlyHyouji();
            _cardImage_obj[0].GetComponent<SetImage>().CardOFF_PlateHyouji();
        }

        // 魔法使用時、スキルレベル選択時の場合の処理
        if (GameMgr.compound_select == 21 || GameMgr.compound_select == 10)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.75f, 0.75f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 65, 0);
            _cardImage_obj[0].GetComponent<SetImage>().Kosu_ON(_kosu);
            _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF();

        }
    }

    
    public void SelectCard_DrawView02(int _toggleType, int _kettei_item2)
    {

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[1].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item2;
        _cardImage.SetInit();
        _cardImage_obj[1].GetComponent<SetImage>().CardParamOFF_2();
        _cardImage_obj[1].GetComponent<Canvas>().sortingOrder = Select_SortingNum;
        //_cardImage_obj[1].GetComponent<SetImage>().SlotChangeButtonON();

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {           
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, 80, 0);
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            //_cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(50, 100, 0);           
        }

    }

    public void DeleteCard_DrawView02()
    {
        Destroy(_cardImage_obj[1]);

        _cardImage_obj.RemoveAt(1);
    }

    public void OKCard_DrawView02(int _kosu)
    {

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(150, 150, 0);
            _cardImage_obj[1].GetComponent<SetImage>().Kosu_ON(_kosu);
            _cardImage_obj[1].GetComponent<SetImage>().CardParamOFF();
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            //_cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            SetTopping_PositionA();
            _cardImage_obj[1].GetComponent<SetImage>().CardOFF_ItemOnlyHyouji();
            _cardImage_obj[1].GetComponent<SetImage>().CardOFF_ItemAnimON();
            _cardImage_obj[1].transform.parent = toppingItemRoot.transform;
            //_cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

        }
    }

    

    public void SelectCard_DrawView03(int _toggleType, int _kettei_item3)
    {
        //Debug.Log("SelectCard_DrawView03 called");

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[2].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item3;
        _cardImage.SetInit();
        _cardImage_obj[2].GetComponent<SetImage>().CardParamOFF_2();
        _cardImage_obj[2].GetComponent<Canvas>().sortingOrder = Select_SortingNum + 1000;
        //_cardImage_obj[2].GetComponent<SetImage>().SlotChangeButtonON();

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(150, 150, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(0, 80, 0);           
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            //_cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            SetTopping_PositionA();
            //_cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);          

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(50, 100, 0);
        }       
    }

    public void DeleteCard_DrawView03()
    {
        Destroy(_cardImage_obj[2]);

        _cardImage_obj.RemoveAt(2);
    }

    public void OKCard_DrawView03(int _kosu)
    {

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(150, 150, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(300, 150, 0);
            _cardImage_obj[2].GetComponent<SetImage>().Kosu_ON(_kosu);
            _cardImage_obj[2].GetComponent<SetImage>().CardParamOFF();
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            //_cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            SetTopping_PositionA();
            //_cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

            SetTopping_PositionB();
            //_cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);
            _cardImage_obj[2].GetComponent<SetImage>().CardOFF_ItemOnlyHyouji();
            _cardImage_obj[2].GetComponent<SetImage>().CardOFF_ItemAnimON();
            _cardImage_obj[2].transform.parent = toppingItemRoot.transform;
        }
    }

    public void SelectCard_DrawView04(int _toggleType, int _kettei_item4)
    {

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[3].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item4;
        _cardImage.SetInit();
        _cardImage_obj[3].GetComponent<SetImage>().CardParamOFF_2();
        _cardImage_obj[3].GetComponent<Canvas>().sortingOrder = Select_SortingNum + 2000;
        //_cardImage_obj[3].GetComponent<SetImage>().SlotChangeButtonON();

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {
            //オリジナル調合では使わない。
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            //_cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            SetTopping_PositionA();
            //_cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

            SetTopping_PositionB();
            //_cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);

            _cardImage_obj[3].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[3].transform.localPosition = new Vector3(50, 100, 0);
        }       
    }

    public void DeleteCard_DrawView04()
    {
        Destroy(_cardImage_obj[3]);

        _cardImage_obj.RemoveAt(3);
    }

    public void OKCard_DrawView04()
    {

        // オリジナル調合を選択した場合の処理
        if (GameMgr.compound_select == 3 || GameMgr.compound_select == 7)
        {
            //オリジナル調合では使わない
        }

        // トッピング調合を選択した場合の処理
        if (GameMgr.compound_select == 2)
        {
            SetTopping_BasePosition();
            //_cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            SetTopping_PositionA();
            //_cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

            SetTopping_PositionB();
            //_cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);

            SetTopping_PositionC();
            //_cardImage_obj[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            //_cardImage_obj[3].transform.localPosition = new Vector3(280, -15, 0);
            _cardImage_obj[3].GetComponent<SetImage>().CardOFF_ItemOnlyHyouji();
            _cardImage_obj[3].GetComponent<SetImage>().CardOFF_ItemAnimON();
            _cardImage_obj[3].transform.parent = toppingItemRoot.transform;
        }
    }

    void SetTopping_BasePosition()
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        //_cardImage_obj[0].transform.localPosition = new Vector3(50, 100, 0);
        _cardImage_obj[0].transform.localPosition = new Vector3(135, -15, 0);
    }

    void SetTopping_PositionA()
    {
        _cardImage_obj[1].transform.localScale = new Vector3(0.75f, 0.75f, 1);
        //_cardImage_obj[1].transform.localPosition = new Vector3(40, 140, 0);
    }

    void SetTopping_PositionB()
    {
        _cardImage_obj[2].transform.localScale = new Vector3(0.75f, 0.75f, 1);
        //_cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);
    }

    void SetTopping_PositionC()
    {
        _cardImage_obj[3].transform.localScale = new Vector3(0.75f, 0.75f, 1);
        //_cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);
    }





    //
    //前提として、「CompoundMainController」を使用するときの処理。
    //
    //リザルトカードの場合は、カード自体を押すと、消える
    public void ResultCard_DrawView(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        SetResultCardViewPanel();
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();

        if(PlayerStatus.girl1_Love_lv >= 99)
        {
            //Hlvのときは食感があがるボーナスがつくので、それの効果表示
            _cardImage.HLVBonus_Hyouji();
        }

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        //_cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        _cardImage.CardParamOFF_2();

        Result_animOn(0); //スケールが小さいから大きくなるアニメーションをON
    }

    public void ResultCard_DrawView2(int _toggleType, int _result_item1, int _result_item2) //リザルト　２つ同時にできる場合の表示
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        //1枚目
        SetResultCardViewPanel();
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item1;
        _cardImage.SetInit();

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        //_cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();

        Result_animOn(0); //スケールが小さいから大きくなるアニメーションをON

        //2枚目
        SetResultCardViewPanel();
        _cardImage = _cardImage_obj[1].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[1].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item2;
        _cardImage.SetInit();

        _cardImage_obj[1].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        //_cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        _cardImage_obj[1].GetComponent<SetImage>().CardParamOFF_2();

        Result_animOn(1); //スケールが小さいから大きくなるアニメーションをON
    }

    public void MagicResultCard_DrawView(int _toggleType, int _result_item, int _mstatus, string _magicname)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        SetResultCardViewPanel();
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        //_cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        //Debug.Log("ログ");
        _cardImage.CardParamOFF_2();
        //_cardImage.CardOFF_ItemOnlyHyouji(); //カード表示をオフにし、アイテムアイコンだけ表示する。
        //_cardImage.CardALLParamOFF(); //魔法調合時、カードそのものの表示はオフ。ただし、調合リザルトボタンは流用したい。

        if (_mstatus == 1) //プレイヤー状態変化をする魔法の場合　名前やテキスト説明を変更　テンプレートも変えてもいいかも？
        {
            _cardImage.CardParamMagic_Hyouji(_magicname);
        }

        Result_animOn(0); //スケールが小さいから大きくなるアニメーションをON
    }




    //予測表示するときのカード表示
    public void ResultCardYosoku_DrawView(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        
        SetResultCardViewPanel();
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        //_cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInitYosoku();

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        //_cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();

        Result_animOn(0); //スケールが小さいから大きくなるアニメーションをON
    }




    //
    void SetResultCardViewPanel()
    {
        if (GameMgr.CompoundSceneStartON)
        {
            ResultCardView_content_obj = canvas.transform.Find("CompoundMainController/Compound_BGPanel_A/ResultCardView/Viewport/Content").gameObject;
            _cardImage_obj.Add(Instantiate(cardPrefab, ResultCardView_content_obj.transform));
        }
        else
        {
            _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        }
    }



    //レシピリストで、開いたときのカード表示処理
    public void RecipiCard_DrawView(int _toggleType, int _kettei_item1)
    {
        //初期化しておく
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }
        _cardImage_obj.Clear();


        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();

        //店売りかオリジナルか、アイテムID
        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item1;
        _cardImage.SetInit();
        //_cardImage.SetYosokuInit();

        _cardImage.CardParamSpScoreOFF(); //SPスコア表示はオフにする。

        //位置とスケール
        if (_cardImage.item_type == "Okashi")
        {
            //前回スコアも表示するので、左寄りに。
            Draw2();
        }
        else
        {
            Draw1();
        }

    }

    //レシピの場合の、リザルトカード表示
    public void RecipiResultCard_DrawView(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage.anim_status = 99;

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        //店売りかオリジナルか、アイテムID
        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();
        //_cardImage.SetYosokuInit(); //予測の場合は、Compound_Keisan.csで調合を事前に計算し、その数値を表示する。

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();

        Result_animOn(0); //スケールが小さいから大きくなるアニメーションをON
    }
   

    //
    //持ち物リストで、開いたときのカード表示処理
    //
    public void ItemListCard_DrawView(int _toggleType, int _kettei_item1)
    {
        //初期化しておく
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }
        _cardImage_obj.Clear();


        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        //_cardImage.SlotChangeButtonON();

        //店売りかオリジナルか、アイテムID
        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item1;
        _cardImage.SetInitPitemList();

        //位置とスケール
        Draw1();

        //アイテムによって、使用するかどうかのビューも表示する。
        _cardImage.UseToggleSetInit(_toggleType, _kettei_item1); //まず初期化

    }

    //
    //あげるをおしたときのカード表示処理
    //
    public void PresentGirl(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        //_cardImage.SlotChangeButtonON();

        //_cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();

        _cardImage_obj[0].GetComponent<SetImage>().CardParamOFF_2();

        //位置とスケール
        Draw1();
    }

    //
    //コンテストロフィーのカード表示処理
    //
    public void ContestClearOkashi(int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();
        _cardImage_obj[0].GetComponent<Canvas>().sortingOrder = 1500;

        _cardImage.Pitem_or_Origin = 0;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInitContestClear();

        //位置とスケール
        Draw5();
    }

    //ショップで、選択したときのカード表示処理
    public void ShopSelectCard_DrawView(int _toggleType, int _kettei_item1)
    {
        //初期化しておく
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }
        _cardImage_obj.Clear();


        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();

        //通常、もしくはレシピ系
        if (_toggleType == 0)
        {
            _cardImage.Pitem_or_Origin = 0;
            _cardImage.check_counter = _kettei_item1; //アイテムID
            _cardImage.SetInitPitemList();
        }
        else if (_toggleType == 1) //レシピ
        {
            //_cardImage.Pitem_or_Origin = _toggleType;
            _cardImage.check_counter = _kettei_item1; //イベントアイテムID
            _cardImage.SetInitEventItem();
        }

        //位置とスケール
        if (_cardImage.CardTasteView_flag1)
        {
            Draw4();
        }
        else
        {
            Draw3();
        }
    }


    void Draw1() //調合時など。センター表示
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 80, 0);
        _cardImage.def_scale = new Vector3(0.85f, 0.85f, 1);
        _cardImage.CardHyoujiAnim();
    }

    void Draw2() //レシピ時。Window２まで表示可能
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(-200, 80, 0);
        _cardImage.def_scale = new Vector3(0.85f, 0.85f, 1);
        _cardImage.CardHyoujiAnim();
    }

    void Draw3() //お店用
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 80, 0);
        _cardImage.def_scale = new Vector3(0.95f, 0.95f, 1);
        _cardImage.CardHyoujiAnim();
        DrawStatus = 0;
    }

    void Draw4() //お店用 味ビューが一つ表示されてるので左よりに位置
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(-180, 80, 0);
        _cardImage.def_scale = new Vector3(0.95f, 0.95f, 1);
        _cardImage.CardHyoujiAnim();
        DrawStatus = 1;
    }

    void Draw5() //コンテストクリア用
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.95f, 0.95f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(-100, 50, 0);
        _cardImage.def_scale = new Vector3(0.95f, 0.95f, 1);
        _cardImage.CardHyoujiAnim();
    }

    //itemSelectToggleから読み込み　使う場合の処理待ち　キャンセル待ち
    public void ItemUseWait()
    {
        selectitem_kettei_obj = GameObject.FindWithTag("SelectItem_kettei");
        yes_selectitem_kettei = selectitem_kettei_obj.GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        StartCoroutine("itemselect_kakunin_PitemList");
    }

    IEnumerator itemselect_kakunin_PitemList()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。
        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された アイテムを飾る、使う場合の処理

                
                break;

            case false: //キャンセルが押された

                //Debug.Log("一個目はcancel");

                itemselect_cancel.All_cancel();

                GameMgr.List_count1 = 9999;
                GameMgr.compound_status = 99; //何も選択していない状態にもどる。

                pitemlistController.transform.Find("BlackImg").gameObject.SetActive(false);
                break;
        }

    }

    //調合時のカードアニメーション
    public void CardCompo_Anim(int _animstatus)
    {
        _movetime = 60; //移動にかかるフレーム数

        _diff_x.Clear();
        _diff_y.Clear();
        _now_cardpos.Clear();
        _now_cardrot.Clear();
        radius.Clear();

        if (_animstatus == 0)
        {
            //今存在している全てのカードに対して、アニメーション
            for (i = 0; i < _cardImage_obj.Count; i++)
            {
                _now_cardpos.Add(_cardImage_obj[i].transform.localPosition);
                _now_cardrot.Add(_cardImage_obj[i].transform.localEulerAngles);

                //今カードがある位置と、原点の差をだす。
                _diff_pos = _cardImage_obj[i].transform.localPosition - Vector3.zero;

                //移動にかかる秒数で割り、１フレームあたりの移動量をだす。各カードのリストに記録
                _diff_x.Add(_diff_pos.x / _movetime);
                _diff_y.Add(_diff_pos.y / _movetime);

                //半径
                //radius.Add(Mathf.Sqrt(_diff_pos.x * _diff_pos.x + _diff_pos.y * _diff_pos.y));

                //回転ランダム
                rot_speed_x = Random.Range(36, 360) / Random.Range(6f, 36f);
                rot_speed_y = Random.Range(36, 360) / Random.Range(6f, 36f);
                rot_speed_z = Random.Range(36, 360) / Random.Range(6f, 36f);
                _diff_rot.Add(new Vector3(rot_speed_x, rot_speed_y, rot_speed_z));
                //Debug.Log("rot_speed_x, y, z: " + rot_speed_x + " " + rot_speed_y + " " + rot_speed_z);

                _cardImage_obj[i].GetComponent<SetImage>().CardParamOFF();
                _cardImage_obj[i].GetComponent<SetImage>().Kosu_OFF();
            }
        }
        else if (_animstatus == 1) //トッピング時の調合アニメーション
        {
            //今存在している全てのカードに対して、アニメーション
            for (i = 0; i < _cardImage_obj.Count; i++)
            {
                if (i != 0) //トッピングアイテムはまたcanvasの座標に戻す
                {
                    _cardImage_obj[i].transform.parent = canvas.transform;
                }

                _now_cardpos.Add(_cardImage_obj[i].transform.localPosition);
                _now_cardrot.Add(_cardImage_obj[i].transform.localEulerAngles);

                //今カードがある位置と、原点の差をだす。
                _diff_pos = _cardImage_obj[i].transform.localPosition - Vector3.zero;

                //移動にかかる秒数で割り、１フレームあたりの移動量をだす。各カードのリストに記録
                _diff_x.Add(_diff_pos.x / _movetime);
                _diff_y.Add(_diff_pos.y / _movetime);

                //半径
                //radius.Add(Mathf.Sqrt(_diff_pos.x * _diff_pos.x + _diff_pos.y * _diff_pos.y));

                //回転ランダム
                rot_speed_x = Random.Range(36, 360) / Random.Range(6f, 36f);
                rot_speed_y = Random.Range(36, 360) / Random.Range(6f, 36f);
                rot_speed_z = Random.Range(36, 360) / Random.Range(6f, 36f);
                _diff_rot.Add(new Vector3(rot_speed_x, rot_speed_y, rot_speed_z));
                //Debug.Log("rot_speed_x, y, z: " + rot_speed_x + " " + rot_speed_y + " " + rot_speed_z);

                _cardImage_obj[i].GetComponent<SetImage>().CardParamOFF();
                _cardImage_obj[i].GetComponent<SetImage>().Kosu_OFF();
                _cardImage_obj[i].GetComponent<SetImage>().CardOFF_PlateHyoujiOFF();
            }
        }

        //アニメーション開始。
        cardcompo_anim_on = true;
        timeOut = 1.0f / 60.0f;

        //StartCoroutine("WaitScaleAnim"); //2秒ほどたってから、だんだんスケールもちっちゃくなるアニメ
    }

    //ボインとはじくようなアニメ
    void Result_animOn(int _card_num)
    {
        resulttransform = _cardImage_obj[_card_num].transform;
        //resultPos = resulttransform.localPosition;
        //resultScale = resulttransform.localScale;

        {
            Sequence sequence = DOTween.Sequence();

            //まず、初期値。
            _cardImage_obj[0].GetComponent<CanvasGroup>().alpha = 0;
            sequence.Append(resulttransform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.0f));
            //sequence.Join(resulttransform.DOLocalMove(new Vector3(0, 0, 0), 0.0f)
                //); //
                                   //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

            //移動のアニメ
            sequence.Append(resulttransform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.75f)
                .SetEase(Ease.OutElastic));
            /*sequence.Join(resulttransform.DOLocalMove(new Vector3(0f, 80f, 0), 0.75f)
                .SetRelative()
                .SetEase(Ease.OutExpo)); //元の位置に戻る。*/
            sequence.Join(_cardImage_obj[0].GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        }

    }

    IEnumerator WaitScaleAnim()
    {
        yield return new WaitForSeconds(2.3f); //2秒待つ

        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            _cardImage_obj[i].transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 2.0f);
        }
    }



    //一時的にカードのインタラクトをOFF
    public void SetinteractiveOFF()
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            _cardImage_obj[i].transform.Find("CompoundResultButton").gameObject.SetActive(false);
        }
    }

    //カードのインタラクトをOn
    public void SetinteractiveOn()
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            _cardImage_obj[i].transform.Find("CompoundResultButton").gameObject.SetActive(true);
        }
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
