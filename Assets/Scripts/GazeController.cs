using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using DG.Tweening;

/// <summary>
/// 目線の追従を行うクラス
/// </summary>
public class GazeController : MonoBehaviour
{
    [SerializeField]
    Transform Anchor = null;
    Vector3 centerOnScreen;

    private Tween sequence_gaze;
    [SerializeField]
    private float gaze_inf; //ゲーズの影響値。なめらかに０にすれば、なめらかにゲーズをOFFにできるはず。

    void Start()
    {
        centerOnScreen = Camera.main.WorldToScreenPoint(Anchor.position);
        gaze_inf = 1.0f;
    }

    private void OnEnable()
    {
        gaze_inf = 1.0f;
        //FadeGazeOn();
    }

    void LateUpdate()
    {
        centerOnScreen = Camera.main.WorldToScreenPoint(Anchor.position);

        if (Input.GetMouseButton(0)) //マウス左クリックしてるときだけ、追従
        {
            var mousePos = Input.mousePosition - centerOnScreen;
            UpdateRotate(new Vector3(mousePos.x * gaze_inf, mousePos.y * gaze_inf, 0) * 0.2f);
        }
        else if (!Input.GetMouseButton(0))
        {
            //var mousePos = centerOnScreen;
            UpdateRotate(new Vector3(0, 0, 0) * 0.2f);
        }

    }

    Vector3 currentRotateion = Vector3.zero;
    Vector3 eulerVelocity = Vector3.zero;

    [SerializeField]
    CubismParameter HeadAngleX = null, HeadAngleY = null, HeadAngleZ = null;
    [SerializeField]
    CubismParameter EyeBallX = null, EyeBallY = null;
    [SerializeField]
    float EaseTime = 0.2f;
    [SerializeField]
    float EyeBallXRate = 0.05f;
    [SerializeField]
    float EyeBallYRate = 0.02f;
    [SerializeField]
    bool ReversedGazing = false;
    [SerializeField]
    bool GazeOn = false;

    void UpdateRotate(Vector3 targetEulerAngle)
    {
        //マウス座標からとってきた、現在の顔角度
        currentRotateion = Vector3.SmoothDamp(currentRotateion, targetEulerAngle, ref eulerVelocity, EaseTime);
        // 頭の角度
        SetParameter(HeadAngleX, currentRotateion.x);
        SetParameter(HeadAngleY, currentRotateion.y);
        SetParameter(HeadAngleZ, currentRotateion.z);
        // 眼球の向き
        SetParameter(EyeBallX, currentRotateion.x * EyeBallXRate * (ReversedGazing ? -1 : 1));
        SetParameter(EyeBallY, currentRotateion.y * EyeBallYRate * (ReversedGazing ? -1 : 1));
    }
    void SetParameter(CubismParameter parameter, float value)
    {
        if (parameter != null)
        {
            parameter.Value = Mathf.Clamp(value, parameter.MinimumValue, parameter.MaximumValue);
        }
    }

    public void FadeGazeOn() //切り替えオフの信号をもらったら、フェードで元の角度に戻す。
    {

        DOTween.Kill(sequence_gaze);
        sequence_gaze = DOTween.To(() => gaze_inf, (val) =>
        {
            gaze_inf = val;

        }, 1.0f, 0.5f).SetEase(Ease.InOutSine);
    }

    public void FadeGazeOff() //切り替えオフの信号をもらったら、フェードで元の角度に戻す。
    {

        DOTween.Kill(sequence_gaze);
        sequence_gaze = DOTween.To(() => gaze_inf, (val) =>
        {
            gaze_inf = val;

        }, 0.0f, 1f).SetEase(Ease.InOutSine)
        .OnComplete(() =>
        {
            this.GetComponent<GazeController>().enabled = false; 
        });
    }
}
