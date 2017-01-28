using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerCounter : MonoBehaviour
{

    static  CustomerCounter _instance;
    [SerializeField]
    Canvas _canvas; 
    [SerializeField]
    Text _text;

    [SerializeField]
    bool _doCount;

    const string HIGH_SCORE_KEY = "highScore";

    public static CustomerCounter instance
    {
        get 
        {
            return _instance; 
        }

    }


    public void UpdateCount()
    {
        //あたいがかえってこなかったら0を返す.
        int score = GetCount();
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, score + 1);
        PlayerPrefs.Save();
    }

    public void DeleteCount()
    {
        //あたいがかえってこなかったら0を返す.
        int score = GetCount();
        PlayerPrefs.SetInt(HIGH_SCORE_KEY,0);
        PlayerPrefs.Save();
    }


    public int GetCount()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    void Awake()
    {
        _instance = this;

        if (_doCount)
        {
            UpdateCount();
        }
    }


    void Update()
    {
        //Update
        if (Input.GetKeyDown(KeyCode.U))
        {
            _text.text =  GetCount().ToString(); 
            UpdateCount();   
        }

        //Delete
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteCount();
        }

        //Show
        if (Input.GetKeyDown(KeyCode.S))
        {

            if (_canvas.enabled)
            {
                _canvas.enabled = false;
            }
            else
            {
                _canvas.enabled = true;
                _text.text =  GetCount().ToString(); 
            }
              
        }



    }



}
