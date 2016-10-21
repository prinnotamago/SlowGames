using UnityEngine;
using System.Collections;

public class TestResetScene : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneChange.ChangeScene(SceneName.Name.Select);
        }
	}
}
