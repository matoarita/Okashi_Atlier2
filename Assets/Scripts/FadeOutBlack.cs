using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeOutBlack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FadeIn()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.5f));

    }

    public void FadeOut()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 1;
        sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 1.0f));
    }
}
