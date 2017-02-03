using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEmission : MonoBehaviour {

    [SerializeField]
    private Material _mat = null;

    private Color _color;

    void Awake()
    {
        _color = _mat.color;
        _mat.EnableKeyword("_EMISSION");
        _mat.SetColor("_EmissionColor", _color * 2);
    }

    public Color EmissionColor(bool isEmission)
    {
        if(!isEmission)
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
}
