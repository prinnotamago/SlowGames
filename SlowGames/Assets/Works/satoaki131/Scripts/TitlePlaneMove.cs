using UnityEngine;
using System.Collections;

public class TitlePlaneMove : MonoBehaviour {

    //Planeが死ぬ座標
    public float endZ
    {
        get; set;
    } 
    

    // 動く速さ
    public float speed
    {
        get; set;
    }
	
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
