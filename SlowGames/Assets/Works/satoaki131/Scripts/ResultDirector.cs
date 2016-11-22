using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDirector : MonoBehaviour {

    [SerializeField]
    private Text _score = null;

    void Start()
    {
        _score.text = "スコア： " + ScoreManager.instance.getScore();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneChange.ChangeScene(SceneName.Name.Title);
        }
    }

}
