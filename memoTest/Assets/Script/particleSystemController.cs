using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleSystemController : MonoBehaviour
{
    // puntos maximos 34
    public ParticleSystem Fireworks;
    public ParticleSystem Confetti;
    public GameObject Ballon;
    public int intensity = 0;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("BallonSpawn", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameController.gameState == 1)
        {
            gameController.gameState = 0;

            if(CanvasController.puntos < 10)
            {
                intensity = 1;
                Celebrations();
            }
            if (CanvasController.puntos > 10 && CanvasController.puntos < 15)
            {
                intensity = 2;
                Celebrations();
            }
            if (CanvasController.puntos > 15 && CanvasController.puntos < 20)
            {
                intensity = 3;
                Celebrations();
            }
            if (CanvasController.puntos > 20 && CanvasController.puntos < 25)
            {
                intensity = 4;
                Celebrations();
            }
            if (CanvasController.puntos > 30)
            {
                intensity = 5;
                Celebrations();
            }
        }
    }

    void Celebrations()
    {
        Instantiate(Fireworks, new Vector3(-9.0f, 0 , -3 ), Quaternion.identity);
        Instantiate(Fireworks, new Vector3(9.0f, 0, -3), Quaternion.identity);

    }

    void BallonSpawn()
    {
        Instantiate(Ballon, new Vector3(Random.Range(6,-6), 2, Random.Range(-6, -10)), Quaternion.identity);
        Invoke("BallonSpawn", 1.0f);
    }    
}
