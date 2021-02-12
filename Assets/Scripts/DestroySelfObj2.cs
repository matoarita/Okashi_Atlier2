using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfObj2 : MonoBehaviour
{

    private float time;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {

        StartCoroutine("DestroySelf_3");
    }

    IEnumerator DestroySelf_3()
    {
        yield return new WaitForSeconds(3f); //3秒待つ

        Destroy(this.gameObject);
    }

    public void KillNow()
    {
        Destroy(this.gameObject);
    }
}
