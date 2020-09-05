using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTouchArea : MonoBehaviour {

    private GameObject _model_obj;

    // Use this for initialization
    void Start () {

        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;

    }
	
	// Update is called once per frame
	void Update () {

        //キャラクタの位置に合わせて、位置を更新
        this.transform.localPosition = _model_obj.transform.localPosition;
    }
}
