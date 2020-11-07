using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DAnimationTrigger : MonoBehaviour {

    private Girl1_status girl1_status;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    private Compound_Main compound_Main;
    private Exp_Controller exp_Controller;

    // Use this for initialization
    void Start () {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        live2d_animator = this.GetComponent<Animator>();
       
        //Mainオブジェクトの取得
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //調合から戻ってきたときに、元の表情にもどる。
    public void OnEndReturnBackHome()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);

        //うまく調合できた場合は、「おいしそ～」って感じで、ワクワクした表情に。
        if(exp_Controller.ResultSuccess) //成功した場合
        {
            girl1_status.face_girl_Surprise(); //おいしそ～
        } else //失敗した場合
        {
            girl1_status.DefaultFace(); //現在の機嫌に合わせた表情に戻す
        }      
    }

    public void OnEndOriCompoPosition()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);
    }
}
