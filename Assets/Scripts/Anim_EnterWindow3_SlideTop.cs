using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Anim_EnterWindow3_SlideTop : MonoBehaviour {

    private GameObject _comp;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        _comp = this.gameObject;

        OpenAnim();
        //StartCoroutine("StartAnim");
        //StartCoroutine("EndAnim");
    }

    void OpenAnim()
    {
        //まず、初期値。
        this.GetComponent<CanvasGroup>().alpha = 0;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_comp.transform.DOLocalMove(new Vector3(0f, 50f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。

        sequence.Append(_comp.transform.DOLocalMove(new Vector3(0f, -50f, 0), 0.5f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。
        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 0.3f));
    }

    IEnumerator StartAnim()
    {
        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;

        yield return new WaitForSeconds(0.1f); //ワンテンポおく

        //sc.PlaySe(19); //ファンファーレ

        OpenAnim();
    }

    IEnumerator EndAnim()
    {
        yield return new WaitForSeconds(4.0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_comp.GetComponent<CanvasGroup>().DOFade(0, 1.0f)
            .OnComplete(OffObj));
    }

    void OffObj()
    {
        this.gameObject.SetActive(false);
    }
}
