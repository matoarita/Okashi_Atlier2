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
    AudioSource audioSource;

    Transform resulttransform;
    Vector3 resultScale;
    Vector3 resultPos;

    private float maxScale, maxPos;
    private float _Scale,  _Pos;
    private float _degscale, _degpos;
    private bool result_anim_on;

    // Use this for initialization
    void Start () {

        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        Pitem_or_Origin_judge = 0;

        audioSource = GetComponent<AudioSource>();

        result_anim_on = false;

        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }
	
	// Update is called once per frame
	void Update () {

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

        //位置とスケール
        _cardImage_obj[0].transform.localScale = new Vector3(0.85f, 0.85f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

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

    public void OKCard_DrawView()
    {
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-36, 108, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.9f, 0.9f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);
        }
    }

    public void SelectCard_DrawView02(int _toggleType, int _kettei_item2)
    {
        Draw_Compound();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[1].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item2;

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-36, 108, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, 100, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {

            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(0, 100, 0);
        }

    }

    public void DeleteCard_DrawView02()
    {
        Destroy(_cardImage_obj[1]);

        _cardImage_obj.RemoveAt(1);
    }

    public void OKCard_DrawView02()
    {
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-36, 108, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(132, 108, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(50, 0, 0);
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

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-36, 108, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(132, 108, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(0, 100, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(50, 0, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(0, 100, 0);
        }       
    }

    public void DeleteCard_DrawView03()
    {
        Destroy(_cardImage_obj[2]);

        _cardImage_obj.RemoveAt(2);
    }

    public void OKCard_DrawView03()
    {
        Draw_Compound();

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-36, 108, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(132, 108, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.6f, 0.6f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(300, 108, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(50, 0, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(100, 0, 0);
        }
    }

    public void SelectCard_DrawView04(int _toggleType, int _kettei_item4)
    {
        Draw_Compound();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[3].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item4;

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            //オリジナル調合では使わない。
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(50, 0, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(100, 0, 0);

            _cardImage_obj[3].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[3].transform.localPosition = new Vector3(0, 100, 0);
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
            _cardImage_obj[0].transform.localPosition = new Vector3(0, 100, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(50, 0, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(100, 0, 0);

            _cardImage_obj[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[3].transform.localPosition = new Vector3(150, 0, 0);
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

        _cardImage_obj[0].transform.localScale = new Vector3(0.0f, 0.0f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(0, 0, 0);

        Result_animOn(); //スケールが小さいから大きくなるアニメーションをON
        audioSource.PlayOneShot(sound1);

    }

    void Result_animOn()
    {
        resulttransform = _cardImage_obj[0].transform;
        resultPos = resulttransform.localPosition;
        resultScale = resulttransform.localScale;

        maxScale = 0.85f;
        maxPos = 100.0f;
        _Scale = 0.0f;
        _Pos = 0.0f;

        _degscale = 0.1f;

        //スケールの変動量を位置の変動量に変換
        _degpos = SujiMap(_degscale, 0, maxScale, 0, maxPos);
        Debug.Log("_degpos: " + _degpos);

        result_anim_on = true;
    }


    public void All_DeleteCard()
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();
    }


    void Draw_Compound()
    {
        compound_Main_obj = GameObject.FindWithTag("Compound_Main");
        compound_Main = compound_Main_obj.GetComponent<Compound_Main>();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        Pitem_or_Origin_judge = 0;
    }

    //(val1, val2)の値を、(val3, val4)の範囲の値に変換する数式
    float SujiMap(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }
}
