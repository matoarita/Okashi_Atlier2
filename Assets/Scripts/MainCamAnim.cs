using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamAnim : MonoBehaviour {

    //カメラ関連
    private Camera main_cam;
    private Animator maincam_animator;
    private int trans; //トランジション用のパラメータ

                       // Use this for initialization
    void Start () {

        //カメラの取得
        main_cam = Camera.main;
        maincam_animator = main_cam.GetComponent<Animator>();
        trans = maincam_animator.GetInteger("trans");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ZoomOutEnd()
    {
        GameMgr.camerazoom_endflag = true;

        //アイドルに戻るときに0に戻す。
        trans = 0;

        //intパラメーターの値を設定する.
        maincam_animator.SetInteger("trans", trans);
    }
}
