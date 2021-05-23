﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Compound_BGPanel_A : MonoBehaviour {

    private SoundController sc;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //音ならす
        //sc.PlaySe(25); //25 鐘の音:50 キラリン:17

        //アニメーションスタート
        OnStartAnim();

    }

    void OnStartAnim()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.transform.Find("SelectPanel_1").GetComponent<CanvasGroup>().alpha = 0;
        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(this.transform.Find("SelectPanel_1").transform.DOLocalMove(new Vector3(0f, 20f, 0), 0.0f)
            .SetRelative()); //元の位置から30px右に置いておく。

        //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/

        //まず画面をフェードイン
        sequence.Append(this.GetComponent<CanvasGroup>().DOFade(1, 0.3f)
            .SetEase(Ease.OutQuart));

        //次にクエストタイトルを登場させる
        sequence.Join(this.transform.Find("SelectPanel_1").transform.DOLocalMove(new Vector3(0.0f, 0f, 0.0f), 0.7f)
            .SetEase(Ease.OutQuart)); //30px右から、元の位置に戻る。
        sequence.Join(this.transform.Find("SelectPanel_1").GetComponent<CanvasGroup>().DOFade(1, 0.3f)
             .SetEase(Ease.OutQuart));
    }
}