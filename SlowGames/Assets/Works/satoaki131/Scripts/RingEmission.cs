using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingEmission : MonoBehaviour {

    [SerializeField]
    private Material _mat = null;

    private Color _color = Color.white;

    void Awake()
    {
        _color = _mat.color;
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
            _mat.SetColor("_EmissionColor", _color);
            return _mat.color;
        }
    }
}
