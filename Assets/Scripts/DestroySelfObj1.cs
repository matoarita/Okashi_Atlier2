using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfObj1 : MonoBehaviour {

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

        StartCoroutine("DestroySelf_5");
    }

    IEnumerator DestroySelf_5()
    {
        yield return new WaitForSeconds(5.0f); //5秒待つ

        Destroy(this.gameObject);
    }

    public void KillNow()
    {
        Destroy(this.gameObject);
    }
}
