using UnityEngine;
using System.Collections;

public class SwordDecision : MonoBehaviour {

    // このゲームオブジェクトの当たり判定を入れる
    Collider collider = null;

    // 判定を取得するのに使う
    [SerializeField]
    SlashSword slash;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
        if (slash.IsAttack)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
	}
}
