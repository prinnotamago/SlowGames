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
        _isShot = false;
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
        Move();
    }

    void Move()
    {
        if (!_isShot) return;
        _animator.Play(_animHash, 0, 0);
        _isShot = false;
        _animator.SetBool(_animHash, _isShot);

    }

}
