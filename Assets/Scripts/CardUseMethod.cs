using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUseMethod : MonoBehaviour
{
    private GameObject canvas;

    private PlayerItemList pitemlist;

    private Compound_Main compound_Main;
    private ItemDataBase database;

    private GameObject itemselect_cancel_obj;
    private ItemSelect_Cancel itemselect_cancel;

    private GameObject pitemlistController_obj;
    private PlayerItemListController pitemlistController;

    private MagicSkillListDataBase magicskill_database;

    private GameObject bgpanelmatome;
    private BGAcceTrigger BGAccetrigger;
    private CardView card_view;

    private SelectItem_kettei yes_selectitem_kettei;//yesボタン内のSelectItem_ketteiスクリプト

    private SoundController sc;

    private int itemID;
    private int itemType;
    private int i;

    private string itemName;

    private Text kakunin_msg;

    // Use this for initialization
    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //アイテムデータベースの取得
        database = ItemDataBase.Instance.GetComponent<ItemDataBase>();

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    //消費アイテムの処理
    public void OnUseAction()
    {
        itemID = this.GetComponent<SetImage>().itemID;
        itemName = database.items[database.SearchItemID(itemID)].itemName;

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        //調合メイン取得
        card_view = GameObject.FindWithTag("CardView").GetComponent<CardView>();
        yes_selectitem_kettei = GameObject.FindWithTag("SelectItem_kettei").GetComponent<SelectItem_kettei>();

        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        //各アイテムごとの確認メッセージ更新
        kakunin_msg = canvas.transform.Find("ItemUseKakuninPanel/MessageWindowItemUse/Text").GetComponent<Text>();
        KakuninMessageKoushin(itemName);

        canvas.transform.Find("ItemUseKakuninPanel").gameObject.SetActive(true);
        GameMgr.compound_status = 999;
        StartCoroutine("ItemUse_kakunin");
    }

    //飾るアイテム
    public void OnDecoAction()
    {
        itemID = this.GetComponent<SetImage>().itemID;

        bgpanelmatome = GameObject.FindWithTag("BG");
        BGAccetrigger = bgpanelmatome.transform.Find("BGAccessory").GetComponent<BGAcceTrigger>();
        BGAccetrigger.BGAcceOn(database.items[database.SearchItemID(itemID)].itemName); //ヒンメリだったら、himmeliを入力している。
    }

    //お皿を変える
    public void OnPlateSetAction()
    {
        itemID = this.GetComponent<SetImage>().itemID;

        foreach (string key in GameMgr.PlateSetItemsName.Keys)
        {
            if (key == database.items[database.SearchItemID(itemID)].itemName)
            {
                //this.transform.Find("CardUseSelect_ScrollView").gameObject.SetActive(true);
                this.transform.Find("CardUseSelect_ScrollView/Viewport/Content/CardPlate_Toggle").gameObject.SetActive(true);
                GameMgr.PlateSetNum = GameMgr.PlateSetItemsName[key];
                break;
            }           
        }

        UseEnd(); //カードを閉じる
    }

    /*public void OnCollectAction() //コレクションに登録する
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
                    if (GameMgr.CollectionItemsName[i] == database.items[database.SearchItemID(itemID)].itemName)
                    {
                        //GameMgr.CollectionItems[i] = true;
                    }
                }

                //登録したアイテムは削除
                deleteItem(itemID);

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
    }*/

    IEnumerator ItemUse_kakunin()
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

                //登録したアイテムは削除
                deleteItem(itemID);

                //各アイテムごとの処理
                ItemUseEffort(itemName);

                canvas.transform.Find("ItemUseKakuninPanel").gameObject.SetActive(false);                             
                card_view.DeleteCard_DrawView();

                GameMgr.compound_status = 0;
                yes_selectitem_kettei.kettei1 = false;

                itemselect_cancel.All_cancel();
                GameMgr.List_count1 = 9999;
                break;

            case false:

                GameMgr.compound_status = 99; //アイテム選択中の画面に。
                yes_selectitem_kettei.kettei1 = false;

                canvas.transform.Find("ItemUseKakuninPanel").gameObject.SetActive(false);
                break;
        }
    }

    void UseEnd()
    {
        itemselect_cancel_obj = GameObject.FindWithTag("ItemSelect_Cancel");
        itemselect_cancel = itemselect_cancel_obj.GetComponent<ItemSelect_Cancel>();

        pitemlistController_obj = GameObject.FindWithTag("PlayeritemList_ScrollView");
        pitemlistController = pitemlistController_obj.GetComponent<PlayerItemListController>();

        //Debug.Log("一個目はcancel");

        itemselect_cancel.All_cancel();

        GameMgr.List_count1 = 9999;
        GameMgr.compound_status = 99; //何も選択していない状態にもどる。

        pitemlistController.transform.Find("BlackImg").gameObject.SetActive(false);
    }

    void deleteItem(int _itemID)
    {
        if (itemType == 0)
        {
            pitemlist.deletePlayerItem(database.items[database.SearchItemID(_itemID)].itemName, 1);
        }
        else if (itemType == 1)
        {
            pitemlist.deleteOriginalItem(_itemID, 1);
        }
        else
        {
            pitemlist.deleteExtremePanelItem(_itemID, 1);
        }
    }

    //各アイテムごとの確認メッセージ表示
    void KakuninMessageKoushin(string _itemName)
    {
        switch(_itemName)
        {
            case "skillreset_ticket":

                kakunin_msg.text = "スキルリセットをする？　にいちゃん" + "\n" + "（アイテムはなくなっちゃうよ！）";
                break;
        }
    }

    //各アイテムごとの処理をかく
    void ItemUseEffort(string _itemName)
    {
        switch (_itemName)
        {
            case "skillreset_ticket":

                sc.PlaySe(175); //5

                //リセットして、魔法ポイントに還元
                magicskill_database.SkillAllResetAndRegainPoint();

                GameMgr.ItemUse_AfterMessage = true;
                GameMgr.ItemUse_AfterMessageNum = 0;
                break;
        }
    }
}
