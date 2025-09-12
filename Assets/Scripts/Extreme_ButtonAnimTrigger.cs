using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extreme_ButtonAnimTrigger : MonoBehaviour
{
    private PlayerItemList pitemlist;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterAnimTrigger()
    {
        //Debug.Log("Enter エクストリームパネル");
        this.GetComponent<ButtonAnimTrigger>().OnImageEnterAnim();

        //おかしがセットされてたら、おさらは動かないバージョン
        /*if (pitemlist.player_extremepanel_itemlist.Count > 0)
        { }
        else
        {
            //Debug.Log("Enter エクストリームパネル");
            this.GetComponent<ButtonAnimTrigger>().OnImageEnterAnim();
        }  */         
    }
}
