using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultChangeModel : MonoBehaviour {

    [SerializeField]
    private GameObject _handModel = null;

    public bool isGunPut
    {
        get; set;
    }

    void Start()
    {
        
    }

    void Update()
    {

        if (!isGunPut) return;
        _handModel.SetActive(true);
        gameObject.SetActive(false);
    }
}
