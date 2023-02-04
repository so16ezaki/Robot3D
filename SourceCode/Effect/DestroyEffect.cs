using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float destroyTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Particle")
        {
            //パーティクルの場合再生終了と同時に消す
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            Destroy(gameObject, particleSystem.main.duration);
        } 
        else
            Destroy(this.gameObject, destroyTime);
    }

    
}
