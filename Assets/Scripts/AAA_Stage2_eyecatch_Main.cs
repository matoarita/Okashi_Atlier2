using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AAA_Stage2_eyecatch_Main : MonoBehaviour {

    private Debug_Panel_Init debug_panel_init;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {

        //デバッグパネルの取得
        debug_panel_init = Debug_Panel_Init.Instance.GetComponent<Debug_Panel_Init>();
        debug_panel_init.DebugPanel_init(); //パネルの初期化
    }
	
	// Update is called once per frame
	void Update () {

        

    }    

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();

        //音を鳴らす
        audioSource.Play();

        StartCoroutine(Checking(() => {

            //Debug.Log("音終了");

            //音終わりにこの中の処理

            FadeManager.Instance.LoadScene("002_Stage2", 0.3f);
        }));
    }

    public delegate void functionType();
    private IEnumerator Checking(functionType callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying)
            {
                callback();
                break;
            }
        }
    }
}
