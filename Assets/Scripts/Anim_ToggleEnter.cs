using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Anim_ToggleEnter : MonoBehaviour {

    //Sequence sequence;

    // Use this for initialization
    void Start () {

        //sequence = DOTween.Sequence();

        StartUpAnim();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartUpAnim()
    {
        //透明度を点滅
        /*this.transform.GetComponent<Image>().DOFade(0.5f, 1.0f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InQuad);*/

        //矢印進んで、最初に戻るの繰り返し
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

    //矢印進んで、最初に戻るの繰り返し
    void LoopMotion03()
    {
        this.transform.DOLocalMove(new Vector3(0f, 10f, 0), 1.0f)
        .SetRelative()
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }
}
