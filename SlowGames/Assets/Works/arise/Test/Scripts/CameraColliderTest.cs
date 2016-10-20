using UnityEngine;
using System.Collections;

public class CameraColliderTest : MonoBehaviour
{
    public bool trriger = false;

    public void OnTriggerEnter(Collider other)
    {
        trriger = true;
        Debug.Log("TrrigerEnter");
    }

    public void OnTriggerExit(Collider other)
    {
        trriger = false;
        Debug.Log("TrrigerExit");
    }

    public void OnTriggerStay(Collider other)
    {
        trriger = true;
        Debug.Log("TrrigerStay");
        Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
    }

    void Update()
    {
        Debug.Log(trriger);
    }
}
