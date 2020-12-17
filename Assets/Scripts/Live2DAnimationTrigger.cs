using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Live2DAnimationTrigger : MonoBehaviour {

    private Girl1_status girl1_status;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    private Exp_Controller exp_Controller;

    // Use this for initialization
    void Start () {

        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        live2d_animator = this.GetComponent<Animator>();
       
        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                this.GetComponent<CubismRenderController>().SortingOrder = -500;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {        
    }

    //調合から戻ってきたときに、元の表情にもどる。
    public void OnEndReturnBackHome()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);

        //うまく調合できた場合は、「おいしそ～」って感じで、ワクワクした表情に。
        if(exp_Controller.ResultSuccess) //成功した場合
        {
            girl1_status.face_girl_Yodare(); //おいしそ～ よだれの表情

            //「おいしそ～」って吹き出しもだしていいかも。
            girl1_status.hukidashiReturnHome();
        } else //失敗した場合
        {
            girl1_status.DefaultFace(); //現在の機嫌に合わせた表情に戻す
        }

        //腹減りカウント再開
        girl1_status.GirlEat_Judge_on = true;
    }

    public void OnEndOriCompoPosition()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);
    }

    public void ResetGazeAnimEnd()
    {

    }
}
