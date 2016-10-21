using UnityEngine;
using System.Collections;

public class TestNavimesh : MonoBehaviour {


    [SerializeField]
    GameObject Target;

    NavMeshAgent _navimesh;

    void Start()
    {
        _navimesh = GetComponent<NavMeshAgent>();
    }
 

    void Update()
    {
        _navimesh.SetDestination(Target.transform.position);
    }

}
