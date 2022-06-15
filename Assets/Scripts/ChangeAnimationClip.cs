using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimationClip : MonoBehaviour
{
    //微妙にバグるのと、結局更新されなかったので、使ってない。

    [SerializeField] AnimationClip clip1; // 
    [SerializeField] AnimationClip clip2; // 

    string overrideClipName = "Idle"; // 上書きするAnimationClip対象

    private AnimatorOverrideController overrideController;
    private Animator anim;

    void Start()
    {
        /*
        anim = GetComponent<Animator>();
        overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController;
        anim.runtimeAnimatorController = overrideController;
        */
    }

    public void ChangeClip(int _clipnum)
    {
        /*
        // ステートをキャッシュ
        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount];
        for (int i = 0; i < anim.layerCount; i++)
        {
            layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
        }

        // AnimationClipを差し替えて、強制的にアップデート
        // ステートがリセットされる
        if (_clipnum == 0)
        {
            overrideController[overrideClipName] = clip1;
        }
        else
        {
            overrideController[overrideClipName] = clip2;
        }
        anim.Update(0.0f);

        // ステートを戻す
        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
        }
        */
    }
}