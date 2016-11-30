using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStartCountDown : MonoBehaviour {

    private Text _text = null;
    private Image _image = null;
    private Color _color = Color.black;

	void Start()
    {
        _text = GetComponent<Text>();
        _image = GetComponentInParent<Image>();
        _color = _image.color;
	}
	
	void Update()
    {
        _text.text = GameDirector.instance.displayTime.ToString();
        if(GameDirector.instance.displayTime == 0)
        {
            _text.text = "Start";
            StartCoroutine(DestroyText());
        }
	}

    private IEnumerator DestroyText()
    {
        var time = 0.0f;
        while (time < 2.0f)
        {
            time += Time.unscaledDeltaTime;
            _image.color = new Color(_color.r, _color.g, _color.b, Mathf.Lerp(1, 0, time / 2));
            yield return null;
        }
        _image.gameObject.SetActive(false);
        
    }
}
