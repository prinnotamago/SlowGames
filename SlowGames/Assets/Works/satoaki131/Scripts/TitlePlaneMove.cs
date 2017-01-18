using UnityEngine;
using System.Collections;

public class TitlePlaneMove : MonoBehaviour {

    [SerializeField]
    private float endZ = 10.0f;

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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 一定距離を超えたら消す
        if (transform.position.z > endZ)
        {
            Destroy(gameObject);
        }
	}
}
