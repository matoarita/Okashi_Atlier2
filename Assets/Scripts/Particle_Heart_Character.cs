using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Heart_Character : MonoBehaviour {

    private Girl1_status girl1_status;

    private ParticleSystem particle;
    private ParticleSystem.EmissionModule particleEm;

    private float _love;
    private int _setlove;

    private GameObject _model_obj;

    // Use this for initialization
    void Start () {
              
        LoveRateChange();

        _model_obj = GameObject.FindWithTag("CharacterLive2D").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        //キャラクタの位置に合わせて、位置を更新
        this.transform.localPosition = _model_obj.transform.localPosition;

    }

    public void LoveRateChange()
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子

        particle = this.GetComponent<ParticleSystem>();
        particleEm = particle.emission;

        _love = girl1_status.girl1_Love_exp;
        _love = _love * 0.1f;

        _setlove = (int)_love;

        particleEm.rate = new ParticleSystem.MinMaxCurve(_setlove);
    }
}
