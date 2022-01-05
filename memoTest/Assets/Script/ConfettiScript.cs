using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiScript : MonoBehaviour
{
    public AudioSource Pop;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var em = ps.emission;
        em.enabled = true;
        em.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(2.0f, Random.Range(10, 60)) });
        GetComponent<Transform>().position = new Vector3(Random.Range(-4.5f, 4.5f), Random.Range(1f, 5), Random.Range(-3, 3));
        //ps.emission.SetBursts(new[] { new ParticleSystem.Burst(0f, Random.Range(10, 60)) } );
        //emission.burst.count = intensity x 100 
        Pop.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("DestroyConfetti", 3.0f);
    }

    void DestroyConfetti()
    {
        Destroy(gameObject);
    }
}
