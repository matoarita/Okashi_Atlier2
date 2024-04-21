using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroineLifePanel : MonoBehaviour {

    private Text HeroineLifeText;
    // Use this for initialization
    void Start () {

        HeroineLifeText = this.transform.Find("HPguage/HPparam").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {

        HPKoushin();

    }

    void HPKoushin()
    {
        //妹の体力（HP)を表示
        HeroineLifeText.text = PlayerStatus.player_girl_lifepoint.ToString();
    }
}
