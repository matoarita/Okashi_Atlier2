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

        //採取地のデータベースリスト
        var matplacedatabase_init = new GameObject("ItemMatPlaceDataBase", typeof(ItemMatPlaceDataBase));
        GameObject.DontDestroyOnLoad(matplacedatabase_init);

        //スロットネーム変換リスト
        var slotnamedatabase_init = new GameObject("SlotNameDataBase", typeof(SlotNameDataBase));
        GameObject.DontDestroyOnLoad(slotnamedatabase_init);

        //アイテムデータベースリスト
        var itemdatabase_init = new GameObject("ItemDataBase", typeof(ItemDataBase));
        GameObject.DontDestroyOnLoad(itemdatabase_init);

        //調合データベースリスト
        var itemcompdatabase_init = new GameObject("ItemCompoundDataBase", typeof(ItemCompoundDataBase));
        GameObject.DontDestroyOnLoad(itemcompdatabase_init);

        //焼くデータベースリスト
        var itemroastdatabase_init = new GameObject("ItemRoastDataBase", typeof(ItemRoastDataBase));
        GameObject.DontDestroyOnLoad(itemroastdatabase_init);       

        //プレイヤーアイテムリスト
        var player_itemlist_init = new GameObject("PlayerItemList", typeof(PlayerItemList));
        GameObject.DontDestroyOnLoad(player_itemlist_init);
        player_itemlist_init.tag = "PlayerItemList";       

        //プレイヤーアイテムリスト・レシピリストの初期化・生成。リストスクロールビューの初期位置などの設定用オブジェクト
        var player_itemscrollview_init = new GameObject("PlayerItemListView_Init", typeof(PlayerItemListView_Init));
        GameObject.DontDestroyOnLoad(player_itemscrollview_init);
        player_itemscrollview_init.tag = "PlayerItemListView_Init";

        //デバッグパネルの生成
        var debugPanel_init = new GameObject("Debug_Panel_Init", typeof(Debug_Panel_Init));
        GameObject.DontDestroyOnLoad(debugPanel_init);

        //女の子の好みのお菓子セットデータベースリスト
        var girllikeset_database_init = new GameObject("GirlLikeSetDataBase", typeof(GirlLikeSetDataBase));
        GameObject.DontDestroyOnLoad(girllikeset_database_init);

        //女の子の好みのお菓子組み合わせセットデータベースリスト
        var girllikecompo_database_init = new GameObject("GirlLikeCompoDataBase", typeof(GirlLikeCompoDataBase));
        GameObject.DontDestroyOnLoad(girllikecompo_database_init);

        //女の子１のステータスリスト
        var girl1_status_init = new GameObject("Girl1_status", typeof(Girl1_status));
        girl1_status_init.AddComponent<AudioSource>();
        girl1_status_init.GetComponent<AudioSource>().volume = 0.5f;
        GameObject.DontDestroyOnLoad(girl1_status_init);

        //女の子１の判定オブジェクト
        var GirlEat_Judge_init = new GameObject("GirlEat_Judge", typeof(GirlEat_Judge));
        GirlEat_Judge_init.AddComponent<AudioSource>();
        //girl1_status_init.GetComponent<AudioSource>().volume = 0.5f;
        GameObject.DontDestroyOnLoad(GirlEat_Judge_init);
        GirlEat_Judge_init.tag = "GirlEat_Judge";

        //カード表示部分
        var cardview_init = new GameObject("CardView", typeof(CardView));
        cardview_init.AddComponent<AudioSource>();
        cardview_init.GetComponent<AudioSource>().volume = 0.5f;
        GameObject.DontDestroyOnLoad(cardview_init);
        cardview_init.tag = "CardView";

        //ショップデータベースリスト
        var itemshopdatabase_init = new GameObject("ItemShopDataBase", typeof(ItemShopDataBase));
        GameObject.DontDestroyOnLoad(itemshopdatabase_init);

        //クエストセットデータベースリスト
        var questset_database_init = new GameObject("QuestSetDataBase", typeof(QuestSetDataBase));
        GameObject.DontDestroyOnLoad(questset_database_init);
       
        //コンテスト感想データベースリスト
        var contestcomment_database_init = new GameObject("ContestCommentDataBase", typeof(ContestCommentDataBase));
        GameObject.DontDestroyOnLoad(contestcomment_database_init);

        //スペシャルお菓子クエストデータベースリスト
        var specialquestset_database_init = new GameObject("Special_Quest", typeof(Special_Quest));
        GameObject.DontDestroyOnLoad(specialquestset_database_init);

        //ゲームマネージャ
        var gamemgr_init = new GameObject("GameMgr", typeof(GameMgr));
        GameObject.DontDestroyOnLoad(gamemgr_init);

        //キー入力の処理マネージャ
        var keyinputmgr_init = new GameObject("KeyInputMgr", typeof(keyManager));
        GameObject.DontDestroyOnLoad(keyinputmgr_init);

        //EXPコントローラー（アイテム増減・経験の増減処理を行う）
        var expcontroller_init = new GameObject("Exp_Controller", typeof(Exp_Controller));
        expcontroller_init.AddComponent<AudioSource>();
        expcontroller_init.GetComponent<AudioSource>().volume = 0.5f;
        GameObject.DontDestroyOnLoad(expcontroller_init);
        expcontroller_init.tag = "Exp_Controller";

        //Expテーブル
        var exp_table_init = new GameObject("ExpTable", typeof(ExpTable));
        GameObject.DontDestroyOnLoad(exp_table_init);
        exp_table_init.tag = "ExpTable";

        //ヒカリお菓子Expテーブル
        var hikariOkashiExpTable_init = new GameObject("HikariOkashiExpTable", typeof(HikariOkashiExpTable));
        GameObject.DontDestroyOnLoad(hikariOkashiExpTable_init);
        hikariOkashiExpTable_init.tag = "HikariOkashiExpTable";

        //スロット名前変更
        var slotchangename_init = new GameObject("SlotChangeName", typeof(SlotChangeName));
        GameObject.DontDestroyOnLoad(slotchangename_init);
        slotchangename_init.tag = "SlotChangeName";

        //フェードマネージャー
        var fadeMgr_init = new GameObject("FadeManager", typeof(FadeManager));
        GameObject.DontDestroyOnLoad(fadeMgr_init);
        fadeMgr_init.GetComponent<FadeManager>().DebugMode = false; //デバッグモード

        //アイテムリストのキャンセル待ちを監視するオブジェクト
        var itemSelect_cancel_init = new GameObject("ItemSelect_Cancel", typeof(ItemSelect_Cancel));
        GameObject.DontDestroyOnLoad(itemSelect_cancel_init);
        itemSelect_cancel_init.tag = "ItemSelect_Cancel";

        //SelectItemKetteiオブジェクト イエス、ノーを監視する
        var SelectItem_kettei_init = new GameObject("SelectItem_kettei", typeof(SelectItem_kettei));
        GameObject.DontDestroyOnLoad(SelectItem_kettei_init);
        SelectItem_kettei_init.tag = "SelectItem_kettei";

        //サウンド・SE関連を統括するオブジェクト
        var soundcontroller_init = new GameObject("SoundController", typeof(SoundController));
        GameObject.DontDestroyOnLoad(soundcontroller_init);
        soundcontroller_init.tag = "SoundController";        

        //調合計算用メソッドオブジェクト
        var combination_init = new GameObject("CombinationMain", typeof(CombinationMain));
        GameObject.DontDestroyOnLoad(combination_init);

        //調合計算用オブジェクト２　主に、調合時のパラメータとトッピングの加算処理。お菓子の味の初期化処理もここで行っている。
        var compound_keisan_init = new GameObject("Compound_Keisan", typeof(Compound_Keisan));
        GameObject.DontDestroyOnLoad(compound_keisan_init);
        compound_keisan_init.tag = "Compound_Keisan";

        //セーブコントローラー
        var savecontroller_init = new GameObject("SaveController", typeof(SaveController));
        GameObject.DontDestroyOnLoad(savecontroller_init);

        //バフ効果計算メソッド
        var buf_Power_Keisan_init = new GameObject("Buf_Power_Keisan", typeof(Buf_Power_Keisan));
        GameObject.DontDestroyOnLoad(buf_Power_Keisan_init);

        //イベントデータベースリスト
        var event_database_init = new GameObject("EventDataBase", typeof(EventDataBase));
        GameObject.DontDestroyOnLoad(event_database_init);

        //FPSカウンタ
        var fpscounter_init = new GameObject("FPSCounter", typeof(FPSCounter));
        GameObject.DontDestroyOnLoad(fpscounter_init);

        //時間を統括するオブジェクト
        var timecontroller_init = new GameObject("TimeController", typeof(TimeController));
        GameObject.DontDestroyOnLoad(timecontroller_init);
        timecontroller_init.tag = "TimeController";

    }

} // class RuntimeInitializer