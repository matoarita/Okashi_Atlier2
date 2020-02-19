using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetmoneyObj_text : MonoBehaviour {

    private Transform myTransform;
    Vector3 my_localPos;

    private Vector3 _tempPos;

    private float life; //自分の表示時間
    private float _deg;
    private float _posdeg;

    // Use this for initialization
    void Start () {
        myTransform = this.transform;
        my_localPos = this.transform.localPosition;
        _tempPos = my_localPos;
        //Debug.Log("GetmoneyObj_text_localPos: " + my_localPos);
        life = 5;
        _deg = 0.1f;
        _posdeg = 0.05f;
    }
	
	// Update is called once per frame
	void Update () {

        myTransform.Translate( 0.0f, _posdeg, 0.0f );

        life -= _deg;

        //Debug.Log("life: " + life);
        //Debug.Log("my_localPos.y: " + my_localPos.y);
        if ( life <= 0 )
        {
            Destroy(this.gameObject);
        }
	}
}
