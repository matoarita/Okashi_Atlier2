using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonAnimTrigger : MonoBehaviour {

    private bool First_Load = false;

    private Vector3 myscale;

    private int canvas_layer_order;

	// Use this for initialization
	void Start () {
        myscale = this.transform.localScale;

        if(this.GetComponent<Canvas>())
        {
            canvas_layer_order = this.GetComponent<Canvas>().sortingOrder;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        if(!First_Load)
        {
            First_Load = true;
            myscale = this.transform.localScale;
        }
        this.transform.localScale = myscale;
    }

    public void OnEnterAnim() //一回小さくなってすぐ戻る
    {
        if (this.GetComponent<Toggle>())
        {
            if (this.GetComponent<Toggle>().IsInteractable() == false)
            {

            }
            else
            {
                Animation1();                
            }
        }

        if (this.GetComponent<Button>())
        {
            if (this.GetComponent<Button>().IsInteractable() == false)
            {

            }
            else
            {
                Animation1();                
            }
        }
    }

    public void OnImageEnterAnim() //一回小さくなってすぐ戻る
    {

        if (this.GetComponent<Image>())
        {
            Animation1();
        }
    }

    public void OnEnterAnimScaleUp() //Enter時でかくなり、ボタン外へいくと元に戻る。
    {
        if (this.GetComponent<Toggle>())
        {
            if (this.GetComponent<Toggle>().IsInteractable() == false)
            {

            }
            else
            {
                Animation2();
            }
        }

        if (this.GetComponent<Button>())
        {
            if (this.GetComponent<Button>().IsInteractable() == false)
            {

            }
            else
            {
                Animation2();
            }
        }
    }

    public void OnImageEnterAnimScaleUp() //Enter時でかくなり、ボタン外へいくと元に戻る。
    {
        if (this.GetComponent<Image>())
        {
            Animation2();
        }
    }

    public void OnEnterAnimScaleDown() //Enter時でかくなり、ボタン外へいくと元に戻る。
    {
        if (this.GetComponent<Toggle>())
        {
            if (this.GetComponent<Toggle>().IsInteractable() == false)
            {

            }
            else
            {
                Animation3();
            }
        }

        if (this.GetComponent<Button>())
        {
            if (this.GetComponent<Button>().IsInteractable() == false)
            {

            }
            else
            {
                Animation3();
            }
        }
    }

    void Animation1()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        //this.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(transform.DOScale(new Vector3(-0.1f, -0.1f, -0.1f), 0.1f)
            .SetRelative());
        /*sequence.Append(transform.DOLocalMove(new Vector3(30f, 0, 0), 0.0f)
            .SetRelative());*/ //元の位置から30px右に置いておく。
                               //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));

        //移動のアニメ
        sequence.Append(transform.DOScale(myscale, 0.1f)

            .SetEase(Ease.OutExpo));
        /*sequence.Append(transform.DOLocalMove(new Vector3(-30f, 0, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); *///30px右から、元の位置に戻る。
                                       //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));
    }

    void Animation2()
    {
        Sequence sequence = DOTween.Sequence();

        //まず、初期値。
        //this.GetComponent<CanvasGroup>().alpha = 0;
        sequence.Append(transform.DOScale(new Vector3(0.15f, 0.15f, 0.15f), 0.1f)
            .SetRelative());
        /*sequence.Append(transform.DOLocalMove(new Vector3(30f, 0, 0), 0.0f)
            .SetRelative());*/ //元の位置から30px右に置いておく。
                               //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(0, 0.0f));  

        if (this.GetComponent<Canvas>())
        {
            this.GetComponent<Canvas>().sortingOrder = canvas_layer_order + 10;
        }
    }

    void Animation3()
    {
        Sequence sequence = DOTween.Sequence();

        //移動のアニメ
        sequence.Append(transform.DOScale(myscale, 0.1f)

            .SetEase(Ease.OutExpo));
        /*sequence.Append(transform.DOLocalMove(new Vector3(-30f, 0, 0), 0.3f)
            .SetRelative()
            .SetEase(Ease.OutExpo)); *///30px右から、元の位置に戻る。
                                       //sequence.Join(this.GetComponent<CanvasGroup>().DOFade(1, 0.2f));

        if (this.GetComponent<Canvas>())
        {
            this.GetComponent<Canvas>().sortingOrder = canvas_layer_order;
        }
    }
}
