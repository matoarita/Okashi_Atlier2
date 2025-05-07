using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionToggle : MonoBehaviour
{
    public int _listnum;
    public int _no;
    public string _itemName;

    private GameObject canvas;
    private GameObject status_panel;

    // Start is called before the first frame update
    void Start()
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //ステータスパネルの取得
        status_panel = canvas.transform.Find("StatusPanel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnToggle()
    {
        status_panel.GetComponent<StatusPanel>().OnCollectionCaptionHyouji(_no, _itemName);
    }
}
