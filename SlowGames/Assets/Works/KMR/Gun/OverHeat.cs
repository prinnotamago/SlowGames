using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeat : MonoBehaviour
{

    [SerializeField]
    private Animator _overHeatAnim = null;

    int _isFinishHash;
    bool _isFinish = false;

    PlayerShot _playerShot;

    void Start()
    {
        _playerShot = GetComponent<PlayerShot>();
        _isFinishHash = Animator.StringToHash("isFinish");
    }

    void Update()
    {

    }

    //ブローバックアニメーションしてからその状態を維持する
    public void FinishProduction()
    {
        _playerShot.isStart = false;
        _isFinish = true;
        _overHeatAnim.SetBool(_isFinishHash, _isFinish);
    }

    //最初からブローバックしている状態にする
    public void BlowbackCompleted()
    {
        _playerShot.isStart = false;
        _isFinish = true;
        //_overHeatAnim.SetBool(_isFinishHash, _isFinish);
        var animationHash = _overHeatAnim.GetCurrentAnimatorStateInfo(0).shortNameHash;
        _overHeatAnim.Play(animationHash,0,9);
    }

}
