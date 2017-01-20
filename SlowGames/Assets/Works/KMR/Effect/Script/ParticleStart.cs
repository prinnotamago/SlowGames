using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStart : MonoBehaviour {

	void Start ()
    {
        foreach(var test in GetComponentsInChildren<ParticleSystem>())
        {
            test.Play();
        }
	}
	
	void Update () {
		
	}
}
