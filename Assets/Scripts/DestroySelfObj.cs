using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfObj : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        switch (transform.name)
        {
            case "Particle_Compo2":

                break;

            case "Particle_Compo3":

                break;

            default:
                break;
        }

        StartCoroutine("DestroySelf");
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(10); //10秒待つ

        Destroy(this.gameObject);
    }
}
