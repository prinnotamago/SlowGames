using UnityEngine;
using System.Collections;

public class SlowChecker : MonoBehaviour
{
    public bool isHit
    {
        get; private set;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerName.SlowCheck) return;
        isHit = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerName.SlowCheck) return;
        isHit = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerName.SlowCheck) return;
        isHit = true;
    }

    void Update()
    {
        //Debug.Log(isHit);
    }
}
