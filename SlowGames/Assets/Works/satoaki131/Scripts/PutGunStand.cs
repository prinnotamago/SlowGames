using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutGunStand : MonoBehaviour {

    [SerializeField]
    private GameObject _gunObj = null;

    [SerializeField]
    private ResultChangeModel _changeModel = null;

    private bool _isPut = false;

    public bool isPutGun
    {
        get { return _isPut; }
    }

    void Start()
    {
        //_gunObj.transform.parent = transform; //自分の子供にする
        //_gunObj.gameObject.transform.position = transform.position;
        //_gunObj.gameObject.transform.rotation = transform.rotation;
        //_gunObj.gameObject.transform.Rotate(235, 0, 180);
    }

    void Update()
    {
        if(!_isPut)
        {
            //_gunObj.transform.parent = transform; //自分の子供にする
            //_gunObj.gameObject.transform.position = transform.position;
            //_gunObj.gameObject.transform.rotation = transform.rotation;
            //_gunObj.gameObject.transform.Rotate(235, 0, 180);
        }
    }

    /// <summary>
    /// Debug用です
    /// </summary>
    /// <param name="col"></param>
    public void Test(GameObject col)
    {
        AudioManager.instance.playSe(AudioName.SeName.GunGet);
        //_gunObj.transform.parent = null; //子供解除
        _gunObj.SetActive(true);
        var localPosition = col.transform.position;
        localPosition.y = 0.9f;
        _gunObj.transform.localPosition = localPosition;
        _gunObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        col.GetComponent<SphereCollider>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        var localRotation = col.transform.eulerAngles;
        localRotation.x = col.transform.position.x < 0 ? -90.0f : 90.0f;
        localRotation.y = col.transform.position.x < 0 ? 0.0f : 180.0f;
        _gunObj.transform.eulerAngles = localRotation;
        _isPut = true;
        _changeModel.isGunPut = true;


    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == TagName.Stand)
        {
            AudioManager.instance.playSe(AudioName.SeName.GunGet);
            //_gunObj.transform.parent = null; //子供解除
            _gunObj.SetActive(true);
            var localPosition = col.transform.position;
            localPosition.y = 0.9f;
            _gunObj.transform.localPosition = localPosition;
            _gunObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            col.GetComponent<SphereCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            var localRotation = col.transform.eulerAngles;
            localRotation.x = col.transform.position.x < 0 ? -90.0f : 90.0f;
            localRotation.y = col.transform.position.x < 0 ? 0.0f : 180.0f;
            _gunObj.transform.eulerAngles = localRotation; 
            _isPut = true;
            _changeModel.isGunPut = true;
            
        }
    }

}
