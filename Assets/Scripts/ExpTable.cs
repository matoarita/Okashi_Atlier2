using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//
//経験値テーブル。ついでに、レベルアップのチェックもここで行っている。
//



public class ExpTable : SingletonMonoBehaviour<ExpTable>
{
    
    //経験値テーブル
    [SerializeField]
    public int[] exp_table_setting = new int[30];

    public Dictionary<int, int> exp_table;

    private int i, count;

    private int now_level, before_level;

    private GameObject canvas;
    private GameObject text_area;
    private Text _text;

    //SEを鳴らす
    public AudioClip sound1;
    AudioSource audioSource;

    IEnumerator routine;

    public bool check_on;
    private bool check_Lclick;


    // Use this for initialization
    void Start () {

        audioSource = GetComponent<AudioSource>();

        //キャンバスの読み込み
        canvas = GameObject.FindWithTag("Canvas");

        SetInit_ExpTable();

        check_on = false;
        check_Lclick = false;

    }
	
	// Update is called once per frame
	void Update () {
		
        if ( check_on == true )
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                check_Lclick = true;
            }
        }
	}

    public void SetInit_ExpTable()
    {
        exp_table = new Dictionary<int, int>();

        for ( i = 0; i < exp_table_setting.Length; i++)
        {
            exp_table.Add(i+1, exp_table_setting[i]);
            //Debug.Log( i+1 + " " + exp_table[i+1]);
        }        
    }

    public void Check_LevelUp()
    {
        //windowテキストエリアの取得
        text_area = canvas.transform.Find("MessageWindow").gameObject;
        _text = text_area.GetComponentInChildren<Text>();

        check_on = true; //チェック中

        for (i = 1; i < exp_table_setting.Length - 1; i++)
        {
            //Console.WriteLine("[{0}:{1}]", table.Key, table.Value);
            if (PlayerStatus.player_renkin_exp >= exp_table[i] && PlayerStatus.player_renkin_exp < exp_table[i+1])
            {
                now_level = i;
            }
        }

        before_level = PlayerStatus.player_renkin_lv; //現在のレベル（レベルアップ前）

        //現在のレベルより、ナウレベルのほうが高い場合は、レベルアップ
        if (now_level > before_level)
        {

            //音を鳴らす。音終わりに次の処理。
            audioSource.PlayOneShot(sound1);

            routine = WaitTime();
            StartCoroutine("WaitTime");
            StartCoroutine("Skip_LevelUp");


            /*音終わりに次の処理をする書き方。メモ。
            //音を鳴らす
            audioSource.Play();

            StartCoroutine(Checking(() => {

                Debug.Log("レベルが上がった！！");                 

                //１ずつあげて、その時に覚えるスキルなどがあれば、それをチェックする。

                PlayerStatus.player_renkin_lv++;

                //○○を覚えた！など

                //最後にテキスト表示
                _text.text = "レベルが上がった！" + "\n" + "錬金レベルが" + PlayerStatus.player_renkin_lv + "になった！";
            }));
            */


        }
        else //全て上がりきったら処理を抜ける。それまで、他の操作は出来なくする。
        {
            check_Lclick = false;
            check_on = false;
            StopCoroutine("Skip_LevelUp");
        }
    }

    public delegate void functionType();
    private IEnumerator Checking(functionType callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying)
            {
                callback();
                break;
            }
        }
    }

    IEnumerator WaitTime()
    {
        //3秒待つ
        yield return new WaitForSeconds(3);

        //左クリックが押されたら、強制終了
        if (check_Lclick == true)
        {
            check_Lclick = false;
            check_on = false;

            routine = null;
            yield break;
        }


        //レベルが上がった以降の処理を書く。
        Debug.Log("レベルが上がった！！");
        
        //１ずつあげて、その時に覚えるスキルなどがあれば、それをチェックする。
        PlayerStatus.player_renkin_lv++;
        
        //○○を覚えた！など

        //最後にテキスト表示
        _text.text = "レベルが上がった！" + "\n" + "パティシエレベルが" + PlayerStatus.player_renkin_lv + "になった！";

        Check_LevelUp(); //もう一回繰り返し
    }

    IEnumerator Skip_LevelUp()
    {
        while (routine != null)
        {

            yield return null; // オンクリックがtrueになるまでは、とりあえず待機
        }

        PlayerStatus.player_renkin_lv = now_level;

        //最後にテキスト表示
        _text.text = "レベルが上がった！" + "\n" + "パティシエレベルが" + PlayerStatus.player_renkin_lv + "になった！";
    }


}
