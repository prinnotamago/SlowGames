using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDamageEffect : MonoBehaviour {

    float damage = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            damage += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SteamVR_Fade.Start(new Color(damage, 0.5f, 0.5f, 0.5f), 0.0f, true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SteamVR_Fade.Start(Color.clear, 1.0f, true);
            damage = 0;
        }
    }
}
