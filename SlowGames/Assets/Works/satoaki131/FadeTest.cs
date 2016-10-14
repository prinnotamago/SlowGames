using UnityEngine;
using System.Collections;

public class FadeTest : MonoBehaviour {

    private FadeManager _manager = null;

    void Start()
    {
        _manager = FindObjectOfType<FadeManager>();
    }

    void Update()
    {
        if(Input.anyKeyDown)
        {
            StartCoroutine(_manager.FadeOut(2.0f, SceneName.test));
        }
    }
}
