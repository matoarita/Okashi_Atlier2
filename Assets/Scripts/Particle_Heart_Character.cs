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

        /*if (transform.name == "Particle_Heart_Character_yellow")
        {
            if(PlayerStatus.girl1_Love_lv >= 15)
            {
                _love = PlayerStatus.girl1_Love_exp - girl1_status.stage1_lvTable[14];
                _love = _love * 0.05f;

                _setlove = (int)_love;

                if (_setlove >= 200) //MAX 200
                {
                    _setlove = 200;
                }

                particleEm.rate = new ParticleSystem.MinMaxCurve(_setlove);
            }
            else
            {
                _setlove = 0;
                particleEm.rate = new ParticleSystem.MinMaxCurve(_setlove);
            }
        }
        else
        {*/

            _love = PlayerStatus.girl1_Love_exp;
            _love = _love * 0.05f;

            _setlove = (int)_love;

            if (_setlove >= 200) //MAX 200
            {
                _setlove = 200;
            }

            particleEm.rate = new ParticleSystem.MinMaxCurve(_setlove);
        //}
    }
}
