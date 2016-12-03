using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserAuthenticationText : MonoBehaviour {

    public enum TextType
    {
        point,
        Descript,
        Clear
    }

    [SerializeField]
    private TextType _type = TextType.Descript;

    private Text _text = null;

    private float _time = 0.0f;
    private float _testTime = 0.5f;

    void Start()
    {
        GetComponent<Text>();
        if(_type == TextType.Descript)
        {
            _text.text = "ユーザー認証中";
        }
        else if(_type == TextType.Clear)
        {
            _text.text = "確認しました。";
        }
        else if(_type == TextType.point)
        {
            _text.text = "";
        }
    }

    void Update()
    {
        if (_type != TextType.point) return;
        _time += Time.deltaTime;
        if(_time > _testTime)
        {
            _testTime += 0.5f;
            _text.text += ".";
        }
        if(_testTime > 2.0f)
        {
            _testTime = 1.0f;
            _text.text = "";

        }
    }


}
