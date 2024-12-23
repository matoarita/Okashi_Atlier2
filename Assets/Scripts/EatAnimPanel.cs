using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatAnimPanel : MonoBehaviour {

    private SoundController sc;
    private GameObject EatStartEffect;

    private Image PlateImg;

    private Sprite _plate_sprite1;
    private Sprite _plate_sprite2;
    private Sprite _plate_sprite3;
    private Sprite _plate_sprite4;
    private Sprite _plate_sprite5;

    // Use this for initialization
    void Start () {

        InitSetting();
    }
	
    void InitSetting()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        PlateImg = this.transform.Find("Plate/plate_img1").GetComponent<Image>();

        EatStartEffect = GameObject.FindWithTag("EatAnim_Effect").transform.Find("Comp").gameObject;
        EatStartEffect.SetActive(false);

        _plate_sprite1 = Resources.Load<Sprite>("Sprites/Icon/PlateImg_01");
        _plate_sprite2 = Resources.Load<Sprite>("Sprites/Icon/PlateImg_02");
        _plate_sprite3 = Resources.Load<Sprite>("Sprites/Icon/PlateImg_03");

        switch (GameMgr.PlateSetNum)
        {
            case 0:

                PlateImg.sprite = _plate_sprite1;
                break;

            case 1:

                PlateImg.sprite = _plate_sprite2;
                break;

            case 2:

                PlateImg.sprite = _plate_sprite3;
                break;
        }
    }

	// Update is called once per frame
	void Update () {
		
        if(GameMgr.EatAnim_End)
        {
            GameMgr.EatAnim_End = false;
            Effect_End();
            this.gameObject.SetActive(false);
        }
	}

    public void Effect_Start()
    {
        InitSetting();

        //食べ始めアニメエフェクト
        EatStartEffect.SetActive(true);       
    }


    public void OnSound_Hit()
    {
        sc.PlaySe(71);
        sc.PlaySe(72);
    }

    public void Effect_End()
    {
        EatStartEffect.SetActive(false);
    }
}
