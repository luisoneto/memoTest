using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public List<GameObject> cartas = new List<GameObject>();
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    Vector3 cardsPosition = new Vector3(-3.0f,0.5f,1.0f);
    Vector3 cardsPositionb = new Vector3(-3.0f, 0.5f, -1.0f);

    // Start is called before the first frame update
    void Start()
    {
        RandomizeCardsPosition();

        // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
        // For para instanciar en dificultad facil - En el eje x se separan por 2, en en el Z por 0.5f.

        for (int z = 0; z < cartas.Count; z++)
        {          
            if (z < 4)
            {
                var carta = Instantiate(cartas[z], cardsPosition, showPosition);
                cardsPosition = cardsPosition + new Vector3(2, 0, 0);
            }

            if(z >= 4)
            {
                var carta = Instantiate(cartas[z], cardsPositionb , showPosition);
                cardsPositionb = cardsPositionb + new Vector3(2, 0, 0);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RandomizeCardsPosition()
    {
        System.Random _random = new System.Random();
        GameObject myGO;

        int n = cartas.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = cartas[r];
            cartas[r] = cartas[i];
            cartas[i] = myGO;
        }
    }  

}
