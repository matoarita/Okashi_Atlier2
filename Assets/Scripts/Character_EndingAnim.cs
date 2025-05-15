using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Character_EndingAnim : MonoBehaviour
{
    private Girl1_status girl1_status;

    private bool AnimStart;

    // Start is called before the first frame update
    void Start()
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子    

        AnimStart = false;
        GameMgr.Bend_FadeAnimStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMgr.ending_count >= 1) //一回でもEDクリア。トップ画面はLive2Dモードになる。
        {
            //エンディングの取得により、半透明でヒカリの残像になる。トゥルーエンドはクリアしてないが、Bエンドで終わってる場合。
            if (!GameMgr.ending_getflag[0]) //トゥルーエンドはまだ
            {
                if (GameMgr.ending_getflag[1]) //Bエンドはクリア
                {
                    GameMgr.Bend_FadeAnimStart = true;
                    girl1_status.GirlEat_Judge_on = false; //Girl1_Statusのほうのランダムアニメは止める。

                    if (AnimStart) { } //再生中はコルーチンに入らない。
                    else
                    {                       
                        StartCoroutine(FadeAnim());
                    }
                }
            }           
        }
    }

    IEnumerator FadeAnim()
    {
        AnimStart = true;

        //一回目アニメ　にいちゃ～～ん。吹き出しもここで出す。
        girl1_status.MotionChange(450);
        this.gameObject.GetComponent<CanvasGroup>().DOFade(0.7f, 3.0f); //

        yield return new WaitForSeconds(5.0f); //ワンテンポおく

        this.gameObject.GetComponent<CanvasGroup>().DOFade(0.0f, 3.0f); //消える

        yield return new WaitForSeconds(5.0f); //ワンテンポおく

        yield return new WaitForSeconds(2.0f); //ワンテンポおく

        //二回目登場　モーションのみ遊んでる感じ？
        girl1_status.MotionChange(450);
        this.gameObject.GetComponent<CanvasGroup>().DOFade(0.7f, 3.0f);

        yield return new WaitForSeconds(5.0f); //ワンテンポおく

        yield return new WaitForSeconds(2.0f); //ワンテンポおく

        //二回目消える
        this.gameObject.GetComponent<CanvasGroup>().DOFade(0.0f, 3.0f);

        yield return new WaitForSeconds(5.0f); //ワンテンポおく

        yield return new WaitForSeconds(2.0f); //ワンテンポおく

        //ループ
        AnimStart = false;
    }
}
