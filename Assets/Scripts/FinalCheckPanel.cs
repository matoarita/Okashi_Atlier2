using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalCheckPanel : MonoBehaviour {

    private GameObject canvas;
    private GameObject _comp;

    private GameObject updown_counter;

	// Use this for initialization
	void Start () {

        InitStart();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitStart()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        _comp = this.transform.Find("Comp").gameObject;

        updown_counter = canvas.transform.Find("updown_counter(Clone)").gameObject;
    }

    private void OnEnable()
    {
        InitStart();
        StartAnim();
    }

    void StartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;
        updown_counter.GetComponent<CanvasGroup>().alpha = 0;

        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(0f, 50f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
        sequence.Join(updown_counter.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(0f, -50f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(updown_counter.transform.DOLocalMove(new Vector3(50f, 0f, 0), 1.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。

        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
        sequence.Join(updown_counter.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }
}
