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

    private SetImage _cardImage;
    private GameObject canvas;
    private GameObject cardPrefab;

    public int Pitem_or_Origin_judge; //店売りアイテムか、オリジナルアイテムの判定

    // Use this for initialization
    void Start () {

        //カードのプレファブコンテンツ要素を取得
        canvas = GameObject.FindWithTag("Canvas");
        cardPrefab = (GameObject)Resources.Load("Prefabs/Item_card_base");

        Pitem_or_Origin_judge = 0;

        SceneManager.sceneLoaded += OnSceneLoaded; //別シーンから、このシーンが読み込まれたときに、処理するメソッド
    }
	
	// Update is called once per frame
	void Update () {
		
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

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item1;

        _cardImage_obj[0].transform.localScale = new Vector3(1.0f, 1.0f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(-170, 80, 0);

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
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-250, 200, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-170, 90, 0);
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
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-250, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(-120, 50, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {

            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.position = new Vector3(160, 320, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[1].transform.position = new Vector3(200, 250, 0);
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
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-200, 50, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(-100, 50, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.position = new Vector3(160, 320, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.position = new Vector3(100, 220, 0);
        }
    }

    public void SelectCard_DrawView03(int _toggleType, int _kettei_item3)
    {
        Draw_Compound();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[2].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _kettei_item3;

        // オリジナル調合を選択した場合の処理
        if (compound_Main.compound_select == 3)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-250, 150, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(-150, 150, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(-120, 50, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.position = new Vector3(160, 320, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.position = new Vector3(100, 220, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[2].transform.position = new Vector3(200, 250, 0);
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
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.localPosition = new Vector3(-250, 50, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[1].transform.localPosition = new Vector3(-150, 50, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[2].transform.localPosition = new Vector3(-50, 50, 0);
        }

        // トッピング調合を選択した場合の処理
        if (compound_Main.compound_select == 2)
        {
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.position = new Vector3(160, 320, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.position = new Vector3(100, 220, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.position = new Vector3(170, 220, 0);
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
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.position = new Vector3(160, 320, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.position = new Vector3(100, 220, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.position = new Vector3(170, 220, 0);

            _cardImage_obj[3].transform.localScale = new Vector3(0.85f, 0.85f, 1);
            _cardImage_obj[3].transform.position = new Vector3(200, 250, 0);
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
            _cardImage_obj[0].transform.localScale = new Vector3(0.65f, 0.65f, 1);
            _cardImage_obj[0].transform.position = new Vector3(160, 320, 0);

            _cardImage_obj[1].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[1].transform.position = new Vector3(100, 220, 0);

            _cardImage_obj[2].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[2].transform.position = new Vector3(170, 220, 0);

            _cardImage_obj[3].transform.localScale = new Vector3(0.5f, 0.5f, 1);
            _cardImage_obj[3].transform.position = new Vector3(240, 220, 0);
        }
    }

    public void ResultCard_DrawView(int _toggleType, int _result_item)
    {
        for (i = 0; i < _cardImage_obj.Count; i++)
        {
            Destroy(_cardImage_obj[i]);
        }

        _cardImage_obj.Clear();

        _cardImage_obj.Add(Instantiate(cardPrefab, canvas.transform));
        _cardImage = _cardImage_obj[0].GetComponent<SetImage>();

        _cardImage.Pitem_or_Origin = _toggleType;
        _cardImage.check_counter = _result_item;

        _cardImage_obj[0].transform.localScale = new Vector3(1.15f, 1.15f, 1);
        _cardImage_obj[0].transform.localPosition = new Vector3(-170, 60, 0);

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
}
