using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTouchArea : MonoBehaviour {

    //Live2Dモデルの取得    
    private GameObject _model_root_obj;
    private GameObject _model;

    // Use this for initialization
    void Start () {

        //Live2Dモデルの取得
        _model_root_obj = GameObject.FindWithTag("CharacterRoot").gameObject;
        _model = _model_root_obj.transform.Find("CharacterMove/Hikari_Live2D_3").gameObject;

    }
	
	// Update is called once per frame
	void Update () {

        switch(GameMgr.Scene_Category_Num)
        {
            case 1000: //タイトルシーンのみ　レンダーテクスチャ使ってるので位置調整

                //キャラクタの位置に合わせて、位置を更新 レンダーカメラの位置は+30ほど右だが、その中のローカルポジション自体は、元のcanvasでの位置座標と数値は一緒なので、この描き方でだいじょうぶ
                this.transform.localPosition = _model.transform.localPosition + new Vector3(0, 0.5f, 0);
                break;

            default:
                //キャラクタの位置に合わせて、位置を更新
                this.transform.localPosition = _model.transform.localPosition;
                break;
        }
        
    }
}
