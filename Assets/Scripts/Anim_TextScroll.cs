using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Anim_TextScroll : MonoBehaviour {

    //Sequence sequence;

    private int counter;

    private Vector3 EndX;
    private Vector3 StartX;
    private Vector3 defaultPos;

    private bool start_read;

    // Use this for initialization
    void Start () {

        //sequence = DOTween.Sequence();
        EndX = new Vector3(-this.GetComponent<RectTransform>().sizeDelta.x, 0, 0); //終端の座標を直接入力
        StartX = new Vector3(this.GetComponent<RectTransform>().sizeDelta.x*2, 0, 0); //開始の座標を直接入力
        
        //StartUpAnim();

        //Debug.Log("-this.GetComponent<RectTransform>().sizeDelta.x: " + -this.GetComponent<RectTransform>().sizeDelta.x); //オブジェクト幅を取得
    }
	
	// Update is called once per frame
	void Update () {


        this.transform.localPosition += new Vector3(-0.2f, 0, 0);
        //Debug.Log("this.transform.localPosition.x: " + this.transform.localPosition.x);

        if(this.transform.localPosition.x <= EndX.x)
        {
            this.transform.localPosition = StartX;
        }
    }

    public void SetInit()
    {
        start_read = false;
        defaultPos = this.transform.localPosition;
        //Debug.Log("defaultPos: " + defaultPos);
    }

    //初期配置に戻す
    public void ResetStartPos()
    {   
        this.transform.localPosition = defaultPos;
    }

    void StartUpAnim()
    {
        //ずっと左に進み続ける 一定時間たったら、また右に戻る
        LoopMotion03();
    }

    public void EnterAnimLoop()
    {
        //Debug.Log("ポイント入った");

        //LoopMotion01();

        //LoopMotion02();
    }

    public void EnterAnimLoopStop()
    {
        //Debug.Log("ポイント外れた");
        //AnimResetStart();
    }

    //アニメーションをリスタートする
    void AnimResetStart()
    {
        this.transform.DORewind();
        this.DOKill();

        StartUpAnim();
    }

    //矢印チョコチョコっと早めに移動
    void LoopMotion01()
    {
        this.transform.DOLocalMove(new Vector3(0f, 5f, 0), 0.5f)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InQuad);
    }

    //縦3dにクルクル回転
    void LoopMotion02()
    {
        this.transform.DOLocalRotate(new Vector3(0f, 180f, 0), 0.2f)
        .SetLoops(-1, LoopType.Incremental)
        .SetEase(Ease.Linear);
    }

    //ずっと左に進み続ける一定以上いったら、また最初に戻る
    void LoopMotion03()
    {
        this.transform.DOLocalMove(new Vector3(-10f, 0f, 0), 1.0f)
        .SetRelative()
        .SetLoops(-1, LoopType.Incremental)
        .SetEase(Ease.Linear);
        
    }
}
