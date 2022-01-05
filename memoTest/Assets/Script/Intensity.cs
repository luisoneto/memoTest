using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intensity : MonoBehaviour
{
    public int intensity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.gameState == 1)
        {
            if (CanvasController.puntos <= 10)
            {
                intensity = 1;
            }
            if (CanvasController.puntos > 10 && CanvasController.puntos <= 15)
            {
                intensity = 2;
            }
            if (CanvasController.puntos > 15 && CanvasController.puntos <= 20)
            {
                intensity = 3;
            }
            if (CanvasController.puntos > 20 && CanvasController.puntos <= 25)
            {
                intensity = 4;
            }
            if (CanvasController.puntos > 25)
            {
                intensity = 5;
            }
            gameController.gameState = 0;
            GameObject.Find("ParticleSystemC").GetComponent<CelebrationStarter>().Celebrations();
            
        }
    }
}
