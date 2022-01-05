using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiScript : MonoBehaviour
{
    public AudioSource Pop;
    // Start is called before the first frame update
    void Start()
    {
        //emission.burst.count = intensity x 100 
        Pop.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("DestroyConfetti", 1.0f);
    }

    void DestroyConfetti()
    {
        Destroy(gameObject);
    }
}
