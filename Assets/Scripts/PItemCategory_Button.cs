using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PItemCategory_Button : MonoBehaviour
{
    public int pid;
    private int p_catenum;

    private SoundController sc;

    // Start is called before the first frame update
    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PitemList_Draw()
    {
        sc.PlaySe(34); //30 ポコ音

        switch (this.gameObject.name)
        {
            case "PItemCategory_Button_all":

                p_catenum = 0;
                break;

            case "PItemCategory_Button_suger":

                p_catenum = 10; //砂糖
                break;

            case "PItemCategory_Button_komugiko":

                p_catenum = 20; //小麦粉
                break;

            case "PItemCategory_Button_butter":

                p_catenum = 30; //バター
                break;

            case "PItemCategory_Button_water":

                p_catenum = 40; //水
                break;

            case "PItemCategory_Button_basic":

                p_catenum = 50; //小麦粉・砂糖・バター
                break;

            case "PItemCategory_Button_basic2":

                p_catenum = 60; //小麦粉・砂糖・バター以外の基本材料
                break;

            case "PItemCategory_Button_fruits":

                p_catenum = 70; //フルーツや花
                break;

            case "PItemCategory_Button_appaleil":

                p_catenum = 80; //クリームや生地系
                break;
                
            case "PItemCategory_Button_tools":

                p_catenum = 100; //器材
                break;

            case "PItemCategory_Button_original":

                p_catenum = 200; //オリジナルとエクストリームパネルアイテム
                break;

            case "PItemCategory_Button_etc":

                p_catenum = 300; //オリジナルとエクストリームパネルアイテム
                break;
        }

        this.transform.root.Find("PlayeritemList_ScrollView").GetComponent<PlayerItemListController>().ItemList_CategoryDraw(p_catenum);
    }
}
