using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DAnimationTrigger : MonoBehaviour {

    private Girl1_status girl1_status;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    // Use this for initialization
    void Start () {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        live2d_animator = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnEndReturnBackHome()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);

        girl1_status.DefaultFace(); //現在の機嫌に合わせた表情に戻す
    }

    public void OnEndOriCompoPosition()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);
    }
}
