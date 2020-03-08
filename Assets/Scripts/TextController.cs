using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    //public string[] scenarios;
    public List<string> scenarios = new List<string>();
    [SerializeField] Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.07f;  // 1文字の表示にかかる時間

    private int currentLine = 0;
    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;       // 表示中の文字数

    private bool OnReadFlag = false;

    private SoundController sc;

    void Start()
    {
        //サウンドコントローラーの取得
        sc = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();

        //SetNextLine();
    }

    void Update()
    {
        /*
        if (currentLine < scenarios.Length && Input.GetMouseButtonDown(0))
        {
            SetNextLine();
        }*/

        if (OnReadFlag)
        {
            // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
            //Debug.Log("displayCharacterCount: " + displayCharacterCount);

            // 表示文字数が前回の表示文字数と異なるならテキストを更新する
            if (displayCharacterCount != lastUpdateCharacter)
            {
                uiText.text = currentText.Substring(0, displayCharacterCount);
                lastUpdateCharacter = displayCharacterCount;

                //音を鳴らす。
                //sc.PlaySe(37);
            }
        }
    }


    void SetNextLine()
    {
        currentText = scenarios[currentLine];
        currentLine++;

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }

    public void SetText(string _contents)
    {
        //初期化
        scenarios.Clear();
        currentLine = 0;
        currentText = string.Empty;
        uiText.text = "";

        Debug.Log("_contents: " + _contents);
        scenarios.Add(_contents);

        SetNextLine();
        OnReadFlag = true; //読み込み開始
    }
}