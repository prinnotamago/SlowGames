using UnityEngine;
using System.Collections;

public class ParticleDestroy : MonoBehaviour {
    ParticleSystem _particle;

    void Start()
    {
        Init();
    }

    void Update()
    {
        Destroy();
    }

    void Init()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    void Destroy()
    {
        if (_particle)
        {
            if (!_particle.IsAlive())
            {
                Destroy(gameObject);
            }
        }
        else
        {
        }
    }
}
