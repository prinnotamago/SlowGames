using UnityEngine;
using System.Collections;

public class HitDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Weapon" || collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Player") return;
        Destroy(gameObject.transform.parent.gameObject);
    }
}
