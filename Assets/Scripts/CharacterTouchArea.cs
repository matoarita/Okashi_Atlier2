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

        //キャラクタの位置に合わせて、位置を更新
        this.transform.localPosition = _model.transform.localPosition;
    }
}
