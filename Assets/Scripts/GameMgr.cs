using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : SingletonMonoBehaviour<GameMgr>
{
    //↑シングルトンにすることで、ゲーム中、GameManagerオブジェクトは、必ず一つのみになる。
    //DontDestroyOnLoad（シーン間で移動してもオブジェクトが削除されない）と併用することで、シーンをまたいでも、常にそのゲームオブジェクトは生き残る。

    public static int scenario_flag; //全シーンで共通。今、どのシナリオまできているか。
    public static bool scenario_ON; //全シーンで共通。宴・シナリオを優先するフラグ。これがONのときは、調合シーンなどでも、宴の表示をまず優先する。宴を読み終えたらOFFにする。
    public int scenario_flag_input; //デバッグ用。シナリオフラグをインスペクタから入力
    public int scenario_flag_cullent; //デバッグ用。現在のシナリオフラグを確認用

    public static bool event_recipi_flag; //イベントレシピを見たときに、宴を表示する用のフラグ
    public static int event_recipiID; //その時のイベント番号
    public static bool event_recipi_endflag; //レシピを読み終えたときのフラグ

    public static bool recipi_read_flag; //入手したレシピを読むときの、宴を表示する用のフラグ
    public static int recipi_read_ID; //その時のイベント番号
    public static bool recipi_read_endflag; //レシピを読み終えたときのフラグ

    public static bool talk_flag; //ショップの「話す」コマンドをONにしたとき、これがONになり、宴の会話が優先される。NPCなどでも使う。
    public static int talk_number; //その時の会話番号。

    private PlayerItemList pitemlist;

    //イベントフラグ管理用
    [SerializeField]
    private bool orange_recipi_get;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);

        //プレイヤー所持アイテムリストの取得
        pitemlist = PlayerItemList.Instance.GetComponent<PlayerItemList>();

        scenario_flag = 0; //シナリオの進み具合を管理するフラグ。GameMgr.scenario_flagでアクセス可能。
        scenario_ON = false;
        scenario_flag_input = 0;
        scenario_flag_cullent = scenario_flag;

        event_recipi_flag = false;
        event_recipi_endflag = false;

        recipi_read_flag = false;
        recipi_read_endflag = false;

        orange_recipi_get = false;

        talk_flag = false;
        talk_number = 0;

    }
	
	// Update is called once per frame
	void Update () {

        //デバッグ用
        scenario_flag_cullent = scenario_flag;

        if (Input.GetKeyDown(KeyCode.Space)) //Spaceキーをおすと、シナリオフラグの入力を手動で設定する。
        {
            scenario_flag = scenario_flag_input;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) //１キーでMain
        {
            //SceneManager.LoadScene("Main");
            FadeManager.Instance.LoadScene("Hiroba", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //２キーでCompound 調合シーン
        {
            //SceneManager.LoadScene("Compound");
            FadeManager.Instance.LoadScene("Compound", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //３キーでGirlEat 試食シーン
        {
            //SceneManager.LoadScene("GirlEat");
            FadeManager.Instance.LoadScene("GirlEat", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //４キーでTravel 採取シーン
        {
            //SceneManager.LoadScene("Travel");
            FadeManager.Instance.LoadScene("Travel", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) //５キーでShop ショップシーン
        {
            //SceneManager.LoadScene("Shop");
            FadeManager.Instance.LoadScene("Shop", 0.3f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) //６キーでQuestBox ショップシーン
        {
            //SceneManager.LoadScene("Shop");
            FadeManager.Instance.LoadScene("QuestBox", 0.3f);
        }

        //ここまで
        if (orange_recipi_get != true)
        {
            if (scenario_flag == 110)
            {
                pitemlist.add_eventPlayerItem(0, 1); //オレンジクッキーのレシピを追加
                orange_recipi_get = true; //ゲットしたよフラグをONに。
            }
        }
    }
}
