using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;

public class Live2DAnimationTrigger : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

    private Girl1_status girl1_status;
    private Animator live2d_animator;
    private int trans_expression;
    private int trans_motion;

    private Exp_Controller exp_Controller;

    // Use this for initialization
    void Start () {

        SetInit();
    }

    void SetInit()
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        live2d_animator = this.GetComponent<Animator>();
        //expressionは、ノーマルにセットしておく。宴でバグらなくなる。
        trans_expression = 1; //リセット
        live2d_animator.SetInteger("trans_expression", trans_expression);

        //Expコントローラーの取得
        exp_Controller = Exp_Controller.Instance.GetComponent<Exp_Controller>();

       

        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":

                //カメラの取得
                main_cam = Camera.main;
                maincam_animator = main_cam.GetComponent<Animator>();
                trans = maincam_animator.GetInteger("trans");

                this.GetComponent<CubismRenderController>().SortingOrder = -500;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        SetInit();
    }

    //調合終了後、元の位置まで戻ってきたときに、下のメソッドを呼び出し、元の表情にもどる。
    public void OnEndReturnBackHome()
    {
        if (girl1_status.gireleat_start_flag) //食べ始めアニメが入ったら、trans_motionは触らない
        { }
        else
        { 
            trans_motion = 101; //念の為、100を繰り返すのを止めておく。
            live2d_animator.SetInteger("trans_motion", trans_motion);           

            //うまく調合できた場合は、「おいしそ～」って感じで、ワクワクした表情に。
            if (exp_Controller.ResultSuccess) //成功した場合
            {
                girl1_status.face_girl_Yodare2(); //おいしそ～ よだれの表情

                //「おいしそ～」って吹き出しもだしていいかも。
                if (girl1_status.HukidashiFlag)
                {
                    girl1_status.hukidashiReturnHome();
                }

                //「おいしそ～」状態に変化する
                girl1_status.GirlOishiso_Status = 1;
            }
            else //失敗した場合
            {
                girl1_status.face_girl_Mazui(); //失敗した表情

                //「失敗しちゃった..」って吹き出しもだしていいかも。
                if (girl1_status.HukidashiFlag)
                {
                    girl1_status.hukidashiOkashiFailedReturnHome();
                }

                girl1_status.GirlOishiso_Status = 2;
            }

            //腹減りカウント再開は、吹き出しが消えたあと。
            //girl1_status.GirlEat_Judge_on = true;
            girl1_status.ResetHukidashiYodare();
        }
    }

    public void OnResetMotion()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);
    }

    public void OnEndOriCompoPosition()
    {
        trans_motion = 0; //リセット
        live2d_animator.SetInteger("trans_motion", trans_motion);
    }

    public void ResetGazeAnimEnd()
    {

    }

    public void FaceMotionEndSignal() //アニメーションをフェードで終了し切り替えるためのフラグ
    {

    }

    public void TapMotionEndSignal() //タップアニメーションをフェードで終了し切り替えるためのフラグ。タッチフラグもオフにする。
    {

        girl1_status.Girl1_touchtwintail_start = false;
        girl1_status.Girl1_touchchest_start = false;
        //girl1_status.touchanim_start = false;
    }

    public void OishisoEndSignal() //アニメーションをフェードで終了し切り替えるためのフラグ
    {
        girl1_status.GirlOishiso_Status = 0; //０に戻す。
        girl1_status.Walk_Start = true;
    }

    IEnumerator Waitalittle() //使用していない
    {
        yield return new WaitForSeconds(0.1f);

    }
}
