using UnityEngine;
using System.Collections;

public class EquivalenceObjectTest : MonoBehaviour
{
    void Start()
    {
        var a = new AudioSource();
        if (GetComponent<AudioSource>() == a)
        {

        }
    }
}
