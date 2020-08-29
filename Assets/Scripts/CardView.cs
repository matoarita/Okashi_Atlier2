using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardView : SingletonMonoBehaviour<CardView>
{

    private int i;

    private GameObject compound_Main_obj;
    private Compound_Main compound_Main;

    private List<GameObject> _cardImage_obj = new List<GameObject>(); //カード表示用のゲームオブジェクト

    private SetImage _cardImage; //カードの描画処理は、SetImageスクリプト
    private GameObject canvas;
    private GameObject cardPrefab;

    public int Pitem_or_Origin_judge; //店売りアイテムか、オリジナルアイテムの判定

    //SEを鳴らす
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    private Transform resulttransform;
    private Vector3 resultScale;
    private Vector3 resultPos;

    private float maxScale, maxPos;
    private float _Scale,  _Pos;
    private float _degscale, _degpos;
    private bool result_anim_on;
 
    private Vector3 _diff_pos;
    private Vector3 _temp_nowpos;
    private Vector3 _temp_nowrot;

    private float speed;
    private List<Vector3> _now_cardpos = new List<Vector3>();
    private List<Vector3> _now_cardrot = new List<Vector3>();
    private List<float> _diff_x = new List<float>();
    private List<float> _diff_y = new List<float>();
    private List<float> radius = new List<float>();
    private List<Vector3> _diff_rot = new List<Vector3>();
    private int _movetime;
    public bool cardcompo_anim_on;

    private GameObject zero_point;

    // Use this for initialization
    void Start () {

        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        Pitem_or_Origin_judge = 0;

        audioSource = GetComponent<AudioSource>();

        result_anim_on = false;

        speed = 2.0f;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                zero_point = canvas.transform.Find("ZeroPoint").gameObject;
                break;

            default://シナリオ系のシーンでは読み込まない。
                break;
        }

        //SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }
	
	// Update is called once per frame
	void Update () {

        if(canvas == null)
        {
            //カードのプレファブコンテンツ要素を取得
            canvas = GameObject.FindWithTag("Canvas");
            cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

            Pitem_or_Origin_judge = 0;

            switch (SceneManager.GetActiveScene().name)
            {
                case "Compound":

                    zero_point = canvas.transform.Find("ZeroPoint").gameObject;
                    break;

                default://シナリオ系のシーンでは読み込まない。
                    break;
            }
        }

        if (cardcompo_anim_on == true)
        {
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


        if (result_anim_on == true)
        {
            _Scale += _degscale;
            _Pos += _degpos;

            resultScale.x = _Scale;
            resultScale.y = _Scale;
            resulttransform.localScale = resultScale;

            resultPos.y = _Pos;
            if (resultPos.y >= 100)
            {
                resultPos.y = 100; //最大値 100の位置にしておく。
            }

            resulttransform.localPosition = resultPos;

            if (_Scale >= maxScale)
            {
                result_anim_on = false;
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

    public void DeleteCard_DrawView()
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

    }

    public void OKCard_DrawView(int _kosu)
    {
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);
            _cardImage_obj[0].GetComponent<SetImage>().Kosu_ON(_kosu);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(50, 100, 0);
        }
    }

    public void SelectCard_DrawView02(int _toggleType, int _kettei_item2)
    {
        Draw_Compound();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[1].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item2;
        _cardImage.SetInit();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, 80, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {

            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

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
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(150, 150, 0);
            _cardImage_obj[1].GetComponent<SetImage>().Kosu_ON(_kosu);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);
        }
    }

    public void SelectCard_DrawView03(int _toggleType, int _kettei_item3)
    {
        //Debug.Log("SelectCard_DrawView03 called");

        Draw_Compound();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[2].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item3;
        _cardImage.SetInit();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(150, 150, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(0, 80, 0);           
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

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
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(150, 150, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(300, 150, 0);
            _cardImage_obj[2].GetComponent<SetImage>().Kosu_ON(_kosu);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);
        }
    }

    public void SelectCard_DrawView04(int _toggleType, int _kettei_item4)
    {
        Draw_Compound();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[3].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item4;
        _cardImage.SetInit();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            //オリジナル調合では使わない。
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);

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
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            //オリジナル調合では使わない
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(75, 180, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, -15, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(140, -15, 0);

            _cardImage_obj[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[3].transform.localPosition = new Vector3(280, -15, 0);
        }
    }

    //リザルトカードの場合は、カード自体を押すと、消える
    public void ResultCard_DrawView(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        Result_animOn(); //スケールが小さいから大きくなるアニメーションをON
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

        //位置とスケール
        Draw1();

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

        _cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        //店売りかオリジナルか、アイテムID
        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();
        //_cardImage.SetYosokuInit(); //予測の場合は、Compound_Keisan.csで調合を事前に計算し、その数値を表示する。

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        Result_animOn(); //スケールが小さいから大きくなるアニメーションをON
    }


    


    //持ち物リストで、開いたときのカード表示処理
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

        //店売りかオリジナルか、アイテムID
        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item1;
        _cardImage.SetInit();

        //位置とスケール
        Draw1();

    }


    public void PresentGirl(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();

        //_cardImage_obj[0].transform.Find("CompoundResultButton").gameObject.SetActive(true);

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;
        _cardImage.SetInit();

        //位置とスケール
        Draw1();
    }

    void Draw1()
    {
        _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 80, 0);
    }

    public void CardCompo_Anim()
    {
        _movetime = 60; //移動にかかるフレーム数

        _diff_x.Clear();
        _diff_y.Clear();
        _now_cardpos.Clear();
        _now_cardrot.Clear();
        radius.Clear();

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
            _diff_rot.Add(new Vector3(Random.Range(0, 360)/30.0f, Random.Range(0, 360)/30.0f, Random.Range(0, 360)/30.0f));

            _cardImage_obj[i].GetComponent<SetImage>().CardParamOFF();
            _cardImage_obj[i].GetComponent<SetImage>().Kosu_OFF();
        }

        //アニメーション開始。
        cardcompo_anim_on = true;
    }


    void Result_animOn()
    {
        resulttransform = _cardImage_obj[0].transform;
        resultPos = resulttransform.localPosition;
        resultScale = resulttransform.localScale;

        maxScale = 0.85f;
        maxPos = 80.0f;
        _Scale = 0.0f;
        _Pos = 0.0f;

        _degscale = 0.1f;

        //スケールの変動量を位置の変動量に変換
        _degpos = SujiMap(_degscale, 0, maxScale, 0, maxPos);
        //Debug.Log("_degpos: " + _degpos);

        result_anim_on = true;
    }



    void Draw_Compound()
    {
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

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
