using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDirector : MonoBehaviour {

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ScoreManager.instance.Reset();
            SceneChange.ChangeScene(SceneName.Name.Title);
        }
    }

}
