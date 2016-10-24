using UnityEngine;
using System.Collections;

public class TestNavimesh : MonoBehaviour {


    [SerializeField]
    GameObject Target;

    UnityEngine.AI.NavMeshAgent _navimesh;

    void Start()
    {
        _navimesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
 

    void Update()
    {
        _navimesh.SetDestination(Target.transform.position);
    }

}
