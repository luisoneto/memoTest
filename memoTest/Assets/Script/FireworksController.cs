using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworksController : MonoBehaviour
{
    public AudioSource onBirthSound;
    public AudioSource onDeathSound;
    int _numberOfParticles = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().position = new Vector3(Random.Range(-3, 3), Random.Range(0.5f, 1), -5);
        var intensity = GameObject.Find("ParticleSystemC").GetComponent<Intensity>();
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule yourEmissionModule;
        yourEmissionModule = ps.emission;
        // si intensity es 4 o 5 que la constante minima sea intensity - 2;
        yourEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(minConstant(intensity.intensity), intensity.intensity);
        Invoke("DestroyParticleSystem", 6.0f);
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

    float minConstant(int intensity)
    {
        float resultado = 0f;
        if (intensity > 3)
        {
            resultado = -2f;
        }
            
        return resultado;
    }

    void DestroyParticleSystem()
    {
        Destroy(gameObject);
    }





}
