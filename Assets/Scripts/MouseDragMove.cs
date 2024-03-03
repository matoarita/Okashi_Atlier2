using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Imageコンポーネントを必要とする
[RequireComponent(typeof(Image))]

public class MouseDragMove : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    // ドラッグ前の位置
    private Vector3 prevPos;

    //基準点（マウスの基準は左下だが、オブジェクトの基準は画面中央になるので補正する。）
    private Vector2 rootPos;

    // Use this for initialization
    void Start () {

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        rootPos = new Vector3(400f, 400f, 0f); //画面の半分（400, 300）+y方向に100

        //初期位置
        //this.transform.localPosition = new Vector3(250f, 50f, 0f);
    }


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
    }

    public void OnDrop(PointerEventData eventData)
    {
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
    }
}
