using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Anim_EnterAndFadeOut : MonoBehaviour {

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

        StartCoroutine("StartAnim");
        StartCoroutine("EndAnim");
    }

    IEnumerator StartAnim()
    {
        //まず、初期値。
        _comp.GetComponent<CanvasGroup>().alpha = 0;

        yield return new WaitForSeconds(0.5f); //ワンテンポおく

        //sc.PlaySe(19); //ファンファーレ

        Sequence sequence = DOTween.Sequence();

        //sequence.Append(transform.DOScale(new Vector3(0.65f, 0.65f, 0.65f), 0.0f));
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(-50f, 0f, 0), 0.0f)
            .SetRelative()); //元の位置から30px上に置いておく。
                             //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        /*sequence.Append(transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f)
            .SetEase(Ease.OutExpo));*/
        sequence.Append(_comp.transform.DOLocalMove(new Vector3(50f, 0f, 0), 3.0f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); //30px上から、元の位置に戻る。

        sequence.Join(_comp.GetComponent<CanvasGroup>().DOFade(1, 1.5f));
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
