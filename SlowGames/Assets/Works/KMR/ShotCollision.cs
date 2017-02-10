using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision col) //子供のあたり判定のときも呼んでくれる
    {

        if (col.gameObject.tag == "Weapon" || col.gameObject.tag == "Bullet" || col.gameObject.tag == "Player" || col.gameObject.tag == "Floor") return;
        Destroy(gameObject);
    }

}
