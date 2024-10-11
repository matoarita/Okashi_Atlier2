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

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        PlateImg = this.transform.Find("Plate").GetComponent<Image>();

        EatStartEffect = GameObject.FindWithTag("EatAnim_Effect").transform.Find("Comp").gameObject;
        EatStartEffect.SetActive(false);

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
