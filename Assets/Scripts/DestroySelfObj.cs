using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfObj : MonoBehaviour {

    private float time;

    // Use this for initialization
    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {

        StartCoroutine("DestroySelf_10");
    }

    IEnumerator DestroySelf_10()
    {
        yield return new WaitForSeconds(10f); //10秒待つ

        Destroy(this.gameObject);
    }

    public void KillNow()
    {
        Destroy(this.gameObject);
    }
}
