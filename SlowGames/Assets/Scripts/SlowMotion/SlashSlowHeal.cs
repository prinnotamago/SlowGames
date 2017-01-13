using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSlowHeal : MonoBehaviour {

    [SerializeField]
    SlashSword[] _slashs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SlowMotion._instance.isSlow) { return; }
        bool isSlash = true;
		foreach(var slash in _slashs)
        {
            if(slash.IsAttack)
            {
                if(slash.pattern == SlashSword.SlashPattern.UPLEFT_DOWNRIGHT ||
                   slash.pattern == SlashSword.SlashPattern.UP_DOWN ||
                   slash.pattern == SlashSword.SlashPattern.UPRIGHT_DOWNLEFT)
                {

                }
                else
                {
                    isSlash = false;
                }
            }
            else
            {
                isSlash = false;
            }
        }

        if (isSlash)
        {
            //Debug.Log("回復");
            SlowMotion._instance.slowTime = SlowMotion._instance.slowTimeMax;
        }
	}
}
