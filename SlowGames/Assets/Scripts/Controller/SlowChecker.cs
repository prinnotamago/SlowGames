using UnityEngine;
using System.Collections;

public class SlowChecker : MonoBehaviour
{
    public bool canSlow
    {
        get; private set;
    }

    public void OnTriggerEnter(Collider other)
    {
        canSlow = true;
    }

    public void OnTriggerExit(Collider other)
    {
        canSlow = false;
    }

    public void OnTriggerStay(Collider other)
    {
        canSlow = true;
    }

    void Update()
    {
        Debug.Log(canSlow);
    }
}
