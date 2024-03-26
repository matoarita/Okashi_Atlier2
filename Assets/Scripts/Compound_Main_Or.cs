using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Rendering;
using DG.Tweening;

public class Compound_Main_Or : MonoBehaviour {

    private MagicSkillListDataBase magicskill_database;
    private PlayerItemList pitemlist;
    private PlayerDefaultStartItemGet playerDefaultStart_ItemGet;

    private void Awake()
    {
        //メインオブジェクト　シーンの読み込み。
        SceneManager.LoadSceneAsync("Hikari_CompMain", LoadSceneMode.Additive); //
    }

    // Use this for initialization
    void Start () {

        //今いるシーン番号を指定
        //GameMgr.Scene_Category_Num = 10;
        //Debug.Log("(GameMgr.Scene_Category_Num): " + GameMgr.Scene_Category_Num); 

        GameMgr.Scene_Name = "Or_Compound";

        //DebugStartItem();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //デバッグ用
    void DebugStartItem()
    {
        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        //ゲーム最初に所持するアイテムを決定するスクリプト
        playerDefaultStart_ItemGet = PlayerDefaultStartItemGet.Instance.GetComponent<PlayerDefaultStartItemGet>();

        //スキルデータベースの取得
        magicskill_database = MagicSkillListDataBase.Instance.GetComponent<MagicSkillListDataBase>();

        //pitemlist.addPlayerItemString("komugiko", 10);

        //デバッグ用　全てのアイテムを追加する
        playerDefaultStart_ItemGet.AddAllItem_NoAcce();

        magicskill_database.skillLearnLv_Name("Bake_Beans", 10);
        magicskill_database.skillLearnLv_Name("Freezing_Spell", 10);
        magicskill_database.skillLearnLv_Name("Removing_Shells", 10);
        magicskill_database.skillLearnLv_Name("Chocolate_Tempering", 10);
    }
}
