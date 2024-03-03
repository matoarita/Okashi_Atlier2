using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Imageコンポーネントを必要とする
[RequireComponent(typeof(Image))]

public class Memo_Result : MonoBehaviour
{
    // ドラッグ前の位置
    private Vector3 prevPos;

    //基準点（マウスの基準は左下だが、オブジェクトの基準は画面中央になるので補正する。）
    private Vector2 rootPos;

    private GameObject canvas;
    private GameObject recipimemoController_obj;

    private PlayerItemList pitemlist;

    private int event_ID;

    private Text _text;
    private string text_recipi_memo;

    private SoundController sc;

    private GameObject content;
    private GameObject textPrefab;

    private GameObject compoBG_A;

    private List<GameObject> _memoList = new List<GameObject>();

    // Use this for initialization
    void Start () {

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スクロールビュー内の、コンテンツ要素を取得
        content = this.transform.Find("Viewport/Content").gameObject;
        textPrefab = (GameObject)Resources.Load("Prefabs/MemoText");

        //コンポBGパネルの取得
        compoBG_A = this.transform.parent.gameObject;

        recipimemoController_obj = compoBG_A.transform.Find("RecipiMemo_ScrollView").gameObject;
        recipimemoController_obj.SetActive(false);


        //テキストオブジェクトを生成
        foreach (Transform child in content.transform) // content内のゲームオブジェクトを一度全て削除。content以下に置いたオブジェクトが、リストに表示される
        {
            Destroy(child.gameObject);
        }
        _memoList.Clear();

        _memoList.Add(Instantiate(textPrefab, content.transform));

        //テキストエリアの読み込み
        _text = _memoList[0].GetComponent<Text>();

        //メモのデータの読み込み
        text_recipi_memo = pitemlist.eventitemlist[event_ID].ev_memo + "\n" + "\n" + "\n";
        _text.text = text_recipi_memo;

        //チュートリアル時
        if (GameMgr.tutorial_ON == true)
        {
            if (GameMgr.tutorial_Num == 30)
            {
                GameMgr.tutorial_Progress = true;
                GameMgr.tutorial_Num = 40;
            }
        }

        //音鳴らす
        sc.PlaySe(34);

        //rootPos = new Vector3(400f, 400f, 0f); //画面の半分（400, 300）+y方向に100

        //初期位置
        this.transform.localPosition = new Vector3(250f, 50f, 0f);
    }

    public void SeteventID(int _ev_id )
    {
        event_ID = _ev_id;
    }

    public void CloseMemo()
    {
        //自身のスクロールの位置を上に。
        this.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;

        recipimemoController_obj.SetActive(true);
        this.gameObject.SetActive(false);
    }

    /*
    //ドラッグ＆ドロップ関係

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置を記憶しておく
        prevPos = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中は位置を更新する
        transform.localPosition = eventData.position - rootPos;
        //Debug.Log("eventData.position: " + eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ドラッグ前の位置に戻す
        //transform.position = prevPos;
        transform.localPosition = eventData.position - rootPos;

        //画面外にでたら、端っこあたりにでるようにする。
    }*/

    //public void OnDrop(PointerEventData eventData)
    //{
        /*var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        
        foreach (var hit in raycastResults)
        {
            // もし DroppableField の上なら、その位置に固定する
            if (hit.gameObject.CompareTag("DroppableField"))
            {
                transform.position = hit.gameObject.transform.position;
                this.enabled = false;
            }
        }*/
    //}
}
