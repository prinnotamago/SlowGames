using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RemainingBulletTexter : MonoBehaviour
{
    Text _text = null;
    PlayerShot _playerShot = null;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    void Start()
    {
        _playerShot = GetComponentInParent<PlayerShot>();
    }

    void Update()
    {
        _text.text = _playerShot.bulletsNumber.ToString() + "/" + _playerShot.maxBulletsNumbers.ToString();
    }
}