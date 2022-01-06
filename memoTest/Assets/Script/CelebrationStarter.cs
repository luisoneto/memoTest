using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationStarter : MonoBehaviour
{
    // puntos maximos 34
    public ParticleSystem Fireworks;
    public ParticleSystem Confetti;
    public GameObject Ballon;
    // Start is called before the first frame update
    void Start()
    {
        //Celebrations();
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void Celebrations()
    {
        switch(GameObject.Find("ParticleSystemC").GetComponent<Intensity>().intensity)
            {
            case 1:
                BallonSpawn();
                break;
            case 2:
                ConfettiSpawn();
                break;
            case 3:
                ConfettiSpawn();
                BallonSpawn();
                break;
            case 4:
                FireWorksSpawn();
                BallonSpawn();
                ConfettiSpawn();
                break;
            case 5:
                FireWorksSpawn();
                BallonSpawn();
                ConfettiSpawn();
                break;
            }
    }

    void FireWorksSpawn()
    {
        Instantiate(Fireworks,transform.position , Quaternion.identity);
        Invoke("FireWorksSpawn", 1.0f);
    }
    void ConfettiSpawn()
    {
        _ = Instantiate(Confetti, transform.position, Quaternion.identity);
        Invoke("ConfettiSpawn", SetDelay());
    }

    void BallonSpawn()
    {
        Instantiate(Ballon, new Vector3(Random.Range(4, -4), 2, -5), Quaternion.identity);
        Invoke("BallonSpawn", SetDelay() - 0.5f);
    }

    float SetDelay()
    {
        switch (GameObject.Find("ParticleSystemC").GetComponent<Intensity>().intensity)
        {
            case 1:
                return 3.0f;
            case 2:
                return 3.0f;
            case 3:
                return 3.0f;
            case 4:
                return 2.0f;
            case 5:
                return 1.0f;
        }

        return 3.0f;

    }
}
