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

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnterAnimLoop()
    {
        Debug.Log("ポイント入った");

        this.transform.DOLocalMove(new Vector3(0f, 5f, 0), 0.5f)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InQuad);
        /*this.transform.DOLocalRotate(new Vector3(0f, 180f, 0), 0.2f)
        .SetLoops(-1, LoopType.Incremental)
        .SetEase(Ease.Linear);*/
    }

    public void EnterAnimLoopStop()
    {
        Debug.Log("ポイント外れた");
        this.transform.DORewind();
        this.DOKill();
    }
}
