using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{
    private Girl1_status girl1_status;

    void Start()
    {
        //女の子データの取得
        girl1_status = Girl1_status.Instance.GetComponent<Girl1_status>(); //メガネっ子
    }
        

    public Vector3 GetPosition()
    {
        if (!girl1_status.CubismLookFlag)
        {
            return Vector3.zero;
        }
        else
        {
            if (!Input.GetMouseButton(0))
            {
                return Vector3.zero;
            }
        }

        var targetPosition = Input.mousePosition;

        targetPosition = (Camera.main.ScreenToViewportPoint(targetPosition) * 2) - Vector3.one;

        return targetPosition;
    }


    public bool IsActive()
    {
        return true;
    }
}