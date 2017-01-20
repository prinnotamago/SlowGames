using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextMessage : MonoBehaviour {

    private char[] _missionText;
    private string _message;

    private Text _text = null;

    private int _count = 0;

    [SerializeField]
    private float _textPopTime = 0.2f;
    private float _textMoveCount = 0.0f;

    /// <summary>
    /// テキストがすべて表示されたらtrueになる
    /// </summary>
    public bool isPopText
    {
        get; private set;
    }

    /// <summary>
    /// テキストを少しずつ表示して良いときにtrueにする
    /// </summary>
    public bool isMoveText
    {
        get; set;
    }

    void Start()
    {
        _text = GetComponent<Text>();
        _message = _text.text;
        _missionText = _text.text.ToCharArray();
        isPopText = false;
        isMoveText = false;
        _text.text = "";
    }

    void Update()
    {
        if (!isMoveText) return;
        TextCount(ref _missionText);
    }

    //文字を１文字ずつ出していく関数
    void TextCount(ref char[] text)
    {
        if (isPopText) return;
        if (_count > text.Length) //文字が最後まで表示されたらreturn
        {
            _count = text.Length;
            isPopText = true;
        }
        _textMoveCount += Time.deltaTime;
        if (_textMoveCount > _textPopTime) //一定時間超えたら１文字表示
        {
            _textMoveCount = 0.0f;
            if (_count < text.Length) _text.text += text[_count];
            _count++;
        }
    }

    public void Reset()
    {
        _count = 0;
        _text.text = _message;
        _missionText = _text.text.ToCharArray();
        _text.text = "";
    }
}
