using UnityEngine;
using System.Collections;

// 既存のキャラクターをラグドールで置換するためのスクリプト。
public class RagdollGenerator : MonoBehaviour
{

    public GameObject ragdollPrefab; // ラグドールのプレハブ。

    // ラグドールの生成。
    public void Generate(Transform originalRoot, Vector3 velocity)
    {
        GameObject ragdoll = (GameObject)Instantiate(ragdollPrefab);
        CopyTransformRecursively(originalRoot, ragdoll.transform, velocity);
    }

    // トランスフォームを再帰的にコピーする。
    // ついでに初速の設定も行う。
    private void CopyTransformRecursively(Transform src, Transform dst, Vector3 velocity)
    {
        dst.position = src.position;
        dst.localRotation = src.localRotation;

        var rigidbody = dst.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.velocity = velocity;
        }
        foreach (Transform child in src)
        {
            Transform srcChild = child;
            Transform dstChild = dst.Find(srcChild.name);
            if (dstChild) CopyTransformRecursively(srcChild, dstChild, velocity);
        }
    }
}