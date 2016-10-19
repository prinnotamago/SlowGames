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
        isHit = true;
    }

    public void OnTriggerExit(Collider other)
    {
        isHit = false;
    }

    public void OnTriggerStay(Collider other)
    {
        isHit = true;
    }

    void Update()
    {
        //Debug.Log(isHit);
    }
}
