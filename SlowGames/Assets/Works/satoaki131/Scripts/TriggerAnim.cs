using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnim : MonoBehaviour
{

    private Animator _animator = null;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
    }

    public void SetAnimFrame(float frame)
    {
        //var clip = _animator.GetCurrentAnimatorClipInfo(0)[0].clip;

        //float time = (float)frame / clip.frameRate;

        var animationHash = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        _animator.Play(animationHash, 0, frame);
    }

}
