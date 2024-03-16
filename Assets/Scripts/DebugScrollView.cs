using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScrollView : MonoBehaviour
{
    public GameObject DebugText;
    public GameObject DebugWindow;
    public int logcnt;

    private Text _logText;

    void Awake()
    {
    }

    private void OnEnable()
    {
        // デバッグログを表示するためのイベントハンドラを登録
        Application.logMessageReceived += LoggedCb;

        _logText = DebugText.GetComponent<Text>();

        logcnt = 0;
    }

    private void OnDisable()
    {
        // イベントハンドラを解除
        Application.logMessageReceived -= LoggedCb;
    }

    // Start と Updateは省略

    public void LoggedCb(string logstr, string stacktrace, LogType type)
    {
        if (logcnt > 20)
        {
            _logText.text = "";
            logcnt = 0;
            int index = _logText.text.IndexOf("\n");
            _logText.text = _logText.text.Substring(index + 1);
        }
        else
        {
            logcnt++;
        }

        _logText.text += logstr;
        _logText.text += "\n";
        // 常にTextの最下部（最新）を表示するように強制スクロール
        DebugWindow.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}