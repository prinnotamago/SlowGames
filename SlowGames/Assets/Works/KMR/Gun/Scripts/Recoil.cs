using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Animator _animator;
    int _animHash;
    bool _isShot;
    bool _isshot;
    Reload _reload;


    void Start()
    {
        _isShot = false;
        _animator = GetComponent<Animator>();
        _animHash = Animator.StringToHash("_isShot");
        _reload = GetComponent<Reload>();
    }

    void Update()
    {
    }

    Quaternion a;
    Vector3 b;

    public void RecoilAnimation()
    {
        
        if (!_isShot)
        {
            
            _isShot = true;
            _animator.SetBool(_animHash, _isShot);
        }

        if(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || _reload.isReload)
        {
            transform.position = b;
            transform.rotation = a;
            //_isShot = false;
        }
        //Debug.Log("homo");
        //Move();
        b = transform.position;
        a = transform.rotation;
        _animator.Play(0,0,0);

    }

}
