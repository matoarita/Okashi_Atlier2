using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Panel_Init : SingletonMonoBehaviour<Debug_Panel_Init>
{

    private GameObject canvas;

    private GameObject debug_panel;
    private GameObject debug_panel_init;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //キャンバス上に存在させるため、ランタイムイニシャライザー上では、あえてデバッグパネルは生成していない。各シーンごとで生成する。
    public void DebugPanel_init()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        debug_panel_init = (GameObject)Resources.Load("Prefabs/Debug_Panel");

        if (debug_panel == null)
        {
            debug_panel = Instantiate(debug_panel_init, canvas.transform);
        }
    }
}
