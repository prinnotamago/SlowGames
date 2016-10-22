using UnityEngine;
using System.Collections;

public class AimAssist : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public  void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "EnemyBullet")
        {
                
        }
    }
}
