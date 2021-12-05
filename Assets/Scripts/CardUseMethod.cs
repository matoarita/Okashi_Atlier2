using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUseMethod : MonoBehaviour
{
    private GameObject canvas;

    private PlayerItemList pitemlist;

    private Compound_Main compound_Main;
    private ItemDataBase database;

    private BGAcceTrigger BGAccetrigger;
    private CardView card_view;

    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private SoundController sc;

    private int itemID;
    private int itemType;
    private int i;

    // Use this for initialization
    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnUseAction()
    {
        itemID = this.GetComponent<SetImage>().itemID;
    }

    public void OnDecoAction()
    {
        itemID = this.GetComponent<SetImage>().itemID;
        BGAccetrigger = GameObject.FindWithTag("BGAccessory").GetComponent<BGAcceTrigger>();

        BGAccetrigger.BGAcceOn(database.items[itemID].itemName); //ヒンメリだったら、himmeliを入力している。

    }

    public void OnCollectAction() //コレクションに登録する
    {
        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //調合メイン取得
        compound_Main = GameObject.FindWithTag("Compound_Main").GetComponent<Compound_Main>();

        card_view = GameObject.FindWithTag("CardView").GetComponent<CardView>();

        yes_selectitem_kettei = GameObject.FindWithTag("SelectItem_kettei").GetComponent<SelectItem_kettei>();

        canvas.transform.Find("CollectionKakunin").gameObject.SetActive(true);
        GameMgr.compound_status = 999;
        StartCoroutine("collect_kakunin");

    }

    IEnumerator collect_kakunin()
    {

        // 一時的にここでコルーチンの処理を止める。別オブジェクトで、はいかいいえを押すと、再開する。

        while (yes_selectitem_kettei.onclick != true)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        yes_selectitem_kettei.onclick = false; //オンクリックのフラグはオフにしておく。

        switch (yes_selectitem_kettei.kettei1)
        {

            case true: //決定が押された

                itemID = this.GetComponent<SetImage>().itemID;
                itemType = this.GetComponent<SetImage>().Pitem_or_Origin;

                for (i = 0; i < GameMgr.CollectionItemsName.Count; i++)
                {
                    if (GameMgr.CollectionItemsName[i] == database.items[itemID].itemName)
                    {
                        GameMgr.CollectionItems[i] = true;
                    }
                }

                //登録したアイテムは削除
                if (itemType == 0)
                {
                    pitemlist.deletePlayerItem(database.items[itemID].itemName, 1);
                }
                else
                {
                    pitemlist.deleteOriginalItem(itemID, i);
                }

                canvas.transform.Find("CollectionKakunin").gameObject.SetActive(false);
                sc.PlaySe(5);
                GameMgr.compound_status = 0;
                card_view.DeleteCard_DrawView();
                break;

            case false:

                GameMgr.compound_status = 99;
                canvas.transform.Find("CollectionKakunin").gameObject.SetActive(false);
                break;
        }
    }
}
