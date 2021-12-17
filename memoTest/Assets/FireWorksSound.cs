using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorksSound : MonoBehaviour
{
    public AudioSource onBirthSound;
    public AudioSource onDeathSound;
    int _numberOfParticles = 0;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        var count = GetComponent<ParticleSystem>().particleCount;
        if (count < _numberOfParticles)
        { //particle has died
            onDeathSound.Play();
        }
        if (count > _numberOfParticles)
        { //particle has been born
            onBirthSound.Play();
        }
        _numberOfParticles = count;
            
    }


}
