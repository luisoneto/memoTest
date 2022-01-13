using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireworksController : MonoBehaviour
{
    public AudioClip Birth;
    public AudioClip Death;
    AudioSource audioSource;
    int _numberOfParticles = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        var intensity = GameObject.Find("ParticleSystemC").GetComponent<Intensity>();
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule yourEmissionModule;
        yourEmissionModule = ps.emission;
        // si intensity es 4 o 5 que la constante minima sea intensity - 2;
        yourEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minConstant(intensity.intensity), intensity.intensity - 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        var intensity = GameObject.Find("ParticleSystemC").GetComponent<Intensity>();
        if (gameController.gameState == 1 && intensity.intensity > 3 )
        {
            ParticleSystem ps = GetComponent<ParticleSystem>();
            var emission = ps.emission;
            emission.enabled = enabled;
        }
        var count = GetComponent<ParticleSystem>().particleCount;
        if (count < _numberOfParticles)
        { //particle has died
           audioSource.PlayOneShot(Birth, 0.1f);
        }
        if (count > _numberOfParticles)
        { //particle has been born
            audioSource.PlayOneShot(Death, 1.0f);
        }
        _numberOfParticles = count;
                 
    }

    float minConstant(int intensity)
    {
        float resultado = 0f;
        if (intensity > 3)
        {
            resultado = -2f;
        }
            
        return resultado;
    }

}
