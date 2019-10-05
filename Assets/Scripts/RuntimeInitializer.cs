using UnityEngine;



public class RuntimeInitializer : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        Debug.Log("Before scene loaded: ALL Database & Setting OK");

        // ゲーム中に常に存在するオブジェクトを生成、およびシーンの変更時にも破棄されないようにする。
        // 「player_status」だけは、直接staticにして、イニシャライズして使いやすくしている。
        // 調整する際は、スクリプトから直接アクセスすること。（ゲームオブジェクトすら生成されないので、見つけづらいかも。）

        //世界・街のデータベースリスト
        var worlddatabase_init = new GameObject("WorldDataBase", typeof(WorldDataBase));
        GameObject.DontDestroyOnLoad(worlddatabase_init);

        //アイテムデータベースリスト
        var itemdatabase_init = new GameObject("ItemDataBase", typeof(ItemDataBase));
        GameObject.DontDestroyOnLoad(itemdatabase_init);

        //調合データベースリスト
        var itemcompdatabase_init = new GameObject("ItemCompoundDataBase", typeof(ItemCompoundDataBase));
        GameObject.DontDestroyOnLoad(itemcompdatabase_init);

        //焼くデータベースリスト
        var itemroastdatabase_init = new GameObject("ItemRoastDataBase", typeof(ItemRoastDataBase));
        GameObject.DontDestroyOnLoad(itemroastdatabase_init);

        //スロットネーム変換リスト
        var slotnamedatabase_init = new GameObject("SlotNameDataBase", typeof(SlotNameDataBase));
        GameObject.DontDestroyOnLoad(slotnamedatabase_init);

        //プレイヤーアイテムリスト
        var player_itemlist_init = new GameObject("PlayerItemList", typeof(PlayerItemList));
        GameObject.DontDestroyOnLoad(player_itemlist_init);
        player_itemlist_init.tag = "PlayerItemList";

        //プレイヤーアイテムリストスクロールビューの初期化用オブジェクト
        var player_itemscrollview_init = new GameObject("PlayerItemListView_Init", typeof(PlayerItemListView_Init));
        GameObject.DontDestroyOnLoad(player_itemscrollview_init);
        player_itemscrollview_init.tag = "PlayerItemListView_Init";

        //デバッグ用チェックアイテムデータベースリスト
        var chk_itemdatabase_init = new GameObject("Check_ItemDataBase", typeof(Check_ItemDataBase));
        GameObject.DontDestroyOnLoad(chk_itemdatabase_init);
        chk_itemdatabase_init.tag = "check_ItemDataBase_obj";

        //女の子１のステータスリスト
        var girl1_status_init = new GameObject("Girl1_status", typeof(Girl1_status));
        GameObject.DontDestroyOnLoad(girl1_status_init);

        //カード表示部分
        var cardview_init = new GameObject("CardView", typeof(CardView));
        GameObject.DontDestroyOnLoad(cardview_init);
        cardview_init.tag = "CardView";

        //ショップデータベースリスト
        var itemshopdatabase_init = new GameObject("ItemShopDataBase", typeof(ItemShopDataBase));
        GameObject.DontDestroyOnLoad(itemshopdatabase_init);

        //ゲームマネージャ
        var gamemgr_init = new GameObject("GameMgr", typeof(GameMgr));
        GameObject.DontDestroyOnLoad(gamemgr_init);

        //キー入力の処理マネージャ
        var keyinputmgr_init = new GameObject("KeyInputMgr", typeof(keyManager));
        GameObject.DontDestroyOnLoad(keyinputmgr_init);

        //EXPコントローラー（アイテム増減・経験の増減処理を行う）
        var expcontroller_init = new GameObject("Exp_Controller", typeof(Exp_Controller));
        GameObject.DontDestroyOnLoad(expcontroller_init);
        expcontroller_init.tag = "Exp_Controller";

        //フェードマネージャー
        var fadeMgr_init = new GameObject("FadeManager", typeof(FadeManager));
        GameObject.DontDestroyOnLoad(fadeMgr_init);
        fadeMgr_init.GetComponent<FadeManager>().DebugMode = false; //デバッグモード

        //アイテムリストのキャンセル待ちを監視するオブジェクト
        var itemSelect_cancel_init = new GameObject("ItemSelect_Cancel", typeof(ItemSelect_Cancel));
        GameObject.DontDestroyOnLoad(itemSelect_cancel_init);
        itemSelect_cancel_init.tag = "ItemSelect_Cancel";

        //サウンド・SE関連を統括するオブジェクト
        var soundcontroller_init = new GameObject("SoundController", typeof(SoundController));
        GameObject.DontDestroyOnLoad(soundcontroller_init);
        soundcontroller_init.tag = "SoundController";

    }

} // class RuntimeInitializer