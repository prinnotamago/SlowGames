using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRotate : MonoBehaviour {

    [SerializeField]
    ParticleSystem[] _particle;

    [SerializeField]
    SwordEnemyMover _data;

	// Use this for initialization
	void Start () {
        switch (_data.data.enemyPattern)
        {
            case SlashSword.SlashPattern.ALL_RANGE:

                break;
            case SlashSword.SlashPattern.DOWN_UP:
                //_particle.startRotation = 0;
                //transform.Rotate(0, 90, 0);
                _particle[0].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.DOWNLEFT_UPRIGHT:
                //_particle.startRotation = 315;
                //transform.Rotate(0, 45, 0);
                _particle[1].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.LEFT_RIGHT:
                //_particle.startRotation = 270;
                //transform.Rotate(0, 0, 0);
                _particle[2].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.UPLEFT_DOWNRIGHT:
                //_particle.startRotation = 225;
                //transform.Rotate(0, 315, 0);
                _particle[3].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.UP_DOWN:
                //_particle.startRotation = 180;
                //transform.Rotate(0, 270, 0);
                _particle[4].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.UPRIGHT_DOWNLEFT:
                //_particle.startRotation = 135;
                //transform.Rotate(0, 225, 0);
                _particle[5].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.RIGHT_LEFT:
                //_particle.startRotation = 90;
                //transform.Rotate(0, 180, 0);
                _particle[6].gameObject.SetActive(true);
                break;
            case SlashSword.SlashPattern.DOWNRIGHT_UPLEFT:
                //_particle. = Vector3.right * 45;
                //transform.Rotate(0, 135, 0);
                _particle[7].gameObject.SetActive(true);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
