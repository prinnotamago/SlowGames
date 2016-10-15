using UnityEngine;
using System.Collections;

public class SelectDirector : MonoBehaviour
{
    const int GUN_NUMBER = 0;
    const int FIST_NUMBER = 1;
    const int SWORD_NUMBER = 2;

    private bool _collision = false;

    [SerializeField, Tooltip("0:Gun, 1:Fist, 2:Sword")]
    private GameObject[] _candidateObj = null;

    private GameObject _selectObj = null;

    void Update()
    {
        if (!_collision) return;
        if (_selectObj == null) return;
        if(_selectObj == _candidateObj[GUN_NUMBER]) //Gun
        {
            SceneChange.ChangeScene(SceneName.Name.MainGame);
        }
        else if(_selectObj == _candidateObj[FIST_NUMBER]) //Fist
        {
            SceneChange.ChangeScene(SceneName.Name.FistGame);
        }
        else if(_selectObj == _candidateObj[SWORD_NUMBER]) //Sword
        {
            SceneChange.ChangeScene(SceneName.Name.SwordGame);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "UI")
        {
            _collision = true;
            _selectObj = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "UI")
        {
            _collision = false;
        }
    }
}
