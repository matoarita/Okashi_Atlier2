﻿using System.Collections;
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
        /*switch (transform.name)
        {
            case "Particle_Compo2":

                break;

            case "Particle_Compo3":

                break;

            default:
                break;
        }*/

        StartCoroutine("DestroySelf_10");
    }

    IEnumerator DestroySelf_10()
    {
        yield return new WaitForSeconds(10f); //10秒待つ

        Destroy(this.gameObject);
    }
}
