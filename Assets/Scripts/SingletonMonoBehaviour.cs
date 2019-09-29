using UnityEngine;
using System;


public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour

{

    private static T instance;

    public static T Instance
    {

        get
        {

            if (instance == null)
            {

                instance = (T)FindObjectOfType(typeof(T));



                if (instance == null)
                {

                    Debug.LogError(typeof(T) + "is nothing");

                }

            }



            return instance;

        }

    }



    protected void Awake()

    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();

    }



    protected bool CheckInstance()

    {

        if (this == Instance) { return true; }

        Destroy(this);

        return false;

    }

}

/*public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 他のゲームオブジェクトにアタッチされているか調べる
        // アタッチされている場合は破棄する。
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}*/
