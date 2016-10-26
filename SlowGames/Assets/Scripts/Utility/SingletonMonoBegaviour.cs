using UnityEngine;

/// <summary>
/// シングルトン
/// 継承先でAwakeを用いる場合はbase.Awake()を必ず記述してください
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBegaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance = null;

    public static T instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<T>();

            return _instance;
        }
    }

    virtual protected void Awake()
    {
        if(this != instance)
        {
            DestroyImmediate(this);
        }
    }

    virtual protected void Update()
    {

    }
}