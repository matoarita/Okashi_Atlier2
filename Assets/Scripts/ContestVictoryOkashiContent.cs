using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class ContestVictoryOkashiContent : MonoBehaviour
{
    private GameObject canvas;

    private GameObject contestvictory_okashiPanel;

    public int toggleitem_ID; //リストの要素自体に、アイテムIDを保持する。


    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        contestvictory_okashiPanel = canvas.transform.Find("ContestVictoryOkashiPanel").gameObject;
    }


    void Update()
    {

    }

    public void OnDataOpenButton()
    {
        ContestList_Open();
    }

    //
    void ContestList_Open()
    {
        contestvictory_okashiPanel.GetComponent<ContestVictoryOkashiPanel>().OnDataOpenButton(toggleitem_ID);
    }
}
