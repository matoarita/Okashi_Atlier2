//Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
//Set your own Text in the Inspector window

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

//***  アイテムの調合処理、プレイヤーのアイテム所持リストの処理はここでやっています。
//***  プレファブにとりつけているスクリプト、なので、privateの値は、インスタンスごとに変わってくるため、バグに注意。

public class bgmSelectToggle : MonoBehaviour
{
    private GameObject canvas;

    private GameObject Option_panel;

    public int toggle_ID; //こっちは、自分自身のIDを保持する。OptionPanelから初期化する際に、決定される。

    private int i;


    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        Option_panel = transform.parent.parent.parent.parent.parent.gameObject;

    }


    void Update()
    {

    }


    /* ### ショップでアイテムを買うときのシーン ### */

    public void select_Music()
    {
        //アイテムを選択したときの処理（トグルの処理）
        Option_panel.GetComponent<OptionPanel>().SelectBGM(toggle_ID);
    }
}
