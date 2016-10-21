using UnityEngine;
using System.Collections;

public class SwordSlow : MonoBehaviour {

    [SerializeField]
    float _slowGage;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == TagName.Float)
        {
            SlowMotion._instance.GameSpeed(0.1f);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == TagName.Float)
        {
            SlowMotion._instance.ResetSpeed();
        }
    }
}
