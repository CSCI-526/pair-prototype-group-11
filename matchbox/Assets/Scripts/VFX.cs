using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = gameObject.GetComponent<ParticleSystem>();
        Destroy(gameObject,1.0f);
    }
}
