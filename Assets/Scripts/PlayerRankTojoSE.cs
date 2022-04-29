using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRankTojoSE : MonoBehaviour {

    private SoundController sc;

    public int _setrigger;

    // Use this for initialization
    void Start () {

        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        switch (transform.name)
        {
            case "SPClearTojoSe":

                if (_setrigger == 0)
                {
                    _setrigger = 1;
                    sc.PlaySe(12);
                }
                break;

            case "PlayerRank":

                sc.PlaySe(27);
                sc.PlaySe(4);
                break;
        }
                
    }

    public void OnTriggerFlag(int _set)
    {
        _setrigger = _set;
    }
}
