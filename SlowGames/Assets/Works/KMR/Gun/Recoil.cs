using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Animator _animator;
    int _animHash;
    bool _isShot;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animHash = Animator.StringToHash("_isShot");
    }

    void Update()
    {
        
    }

    public void RecoilAnimation()
    {
        _isShot = true;
        _animator.SetBool(_animHash, _isShot);
        
              
    }

    void Move()
    {
        _animator.Play(_animHash, 0, 0);
    }

}
