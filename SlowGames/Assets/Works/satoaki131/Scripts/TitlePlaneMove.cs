using UnityEngine;
using System.Collections;

public class TitlePlaneMove : MonoBehaviour {

    // 動く速さ
    public float speed
    {
        get; set;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // 移動
        transform.Translate(Vector3.forward * speed);

        // 一定距離を超えたら消す
        if (transform.position.z > 100)
        {
            Destroy(gameObject);
        }
	}
}
