using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using DG.Tweening;

public class Live2DAnimSceneSet : MonoBehaviour {

    private CubismRenderController cubism_rendercontroller;
    private Animator live2d_animator;

    private int motion_layer_num;

    // Use this for initialization
    void Start () {

        live2d_animator = this.GetComponent<Animator>();        

        //シーンごとに。デフォルトの表情
        SetInitSceneExpression();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetInitSceneExpression()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Compound":
               
                break;

            case "999_Gameover":

                //live2d_animator.SetLayerWeight(3, 0.0f); //メインでは、最初宴用表情はオフにしておく。
                //live2d_animator.SetInteger("trans_expression", 60);

                live2d_animator.SetLayerWeight(3, 1.0f); //宴モーションをこのシーンでは再生
                motion_layer_num = 3;
                live2d_animator.Play("uta_face_60_crysunsun2", motion_layer_num, 0.0f);
                break;
        }
        
    }
}
