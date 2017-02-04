using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEmission : MonoBehaviour
{

    public enum Emissivecolor
    {
        blue,
        yellow,
        red
    }

    [SerializeField]
    private Material _mat = null;

    private Color _color;

    /// <summary>
    /// 色の更新中かどうか
    /// コルーチンが走ってるときtrueになる
    /// </summary>
    public bool isChange
    {
        get; private set;
    }

    /// <summary>
    /// 現在のいろをenumで管理
    /// </summary>
    private Emissivecolor _emissiveColor = Emissivecolor.blue;
    public Emissivecolor emissiveColor
    {
        get { return _emissiveColor; }
    }

    public void setEmissiveColor(Emissivecolor color)
    {
        _emissiveColor = color;
    }

    /// <summary>
    /// EmissiveColorの情報から現在のいろを取り出す
    /// </summary>
    private Dictionary<Emissivecolor, Color> _selectColor = null;
    public Dictionary<Emissivecolor, Color> color
    {
        get { return _selectColor; }
    }

    void Awake()
    {
        _color = _mat.color;
        _mat.EnableKeyword("_EMISSION");
        _mat.SetColor("_EmissionColor", _color * 2);
        isChange = false;
    }

    void Start()
    {
        _selectColor = new Dictionary<Emissivecolor, Color>();
        _selectColor.Add(Emissivecolor.blue, new Color(0.0f, 148.0f / 255.0f, 1.0f));
        _selectColor.Add(Emissivecolor.yellow, Color.yellow);
        _selectColor.Add(Emissivecolor.red, Color.red);
    }

    public Color EmissionColor(bool isEmission)
    {
        if (!isEmission)
        {
            _mat.EnableKeyword("_EMISSION");
            _mat.SetColor("_EmissionColor", Color.black);
            return _mat.color;
        }
        else
        {
            _mat.EnableKeyword("_EMISSION");
            _mat.SetColor("_EmissionColor", _color * 2);
            return _mat.color;
        }
    }

    public void setBlueColor()
    {
        _mat.EnableKeyword("_EMISSION");
        _mat.SetColor("_EmissionColor", _color * 2);
    }

    //public void EmissionColorChange(Color color1, Color color2, float hp, float Maxhp)
    //{
    //    float r = (hp / Maxhp);
    //    float sin = r;
    //    float cos = Maxhp - r;
    //    Color color = (color1 * sin) + (color2 * cos);

    //    _mat.EnableKeyword("_EMISSION");
    //    _mat.SetColor("_EmissionColor", color * 2);
    //}

    public void ColorMoveChange(Color color1, Color color2)
    {
        if (isChange) return;
        StartCoroutine(ColorMoveChanger(color1, color2));
    }

    IEnumerator ColorMoveChanger(Color color1, Color color2)
    {
        isChange = true;
        var time = 0.0f;
        var END_TIME = 2.0f;
        while(time < END_TIME)
        {
            time += Time.unscaledDeltaTime;
            float r = (time / END_TIME);
            float sin = r;
            float cos = END_TIME - r;
            Color color = (color1 * sin) + (color2 * cos);

            _mat.EnableKeyword("_EMISSION");
            _mat.SetColor("_EmissionColor", color * 2);
            yield return null;
        }
        isChange = false;
        yield return null;
    }

}
