using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public float speed = 1.0f;
    int[] idsCartas = new int[2];
    public bool isRotated;
    public List<GameObject> cartasElegidas = new List<GameObject>();
    public List<GameObject> cartas = new List<GameObject>();
    public Quaternion showPosition = Quaternion.Euler(0, 0, -180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);
    private int cartasRotadas = 0;
    Vector3 cardsPosition = new Vector3(-3.0f, 0.5f, 1.0f);
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

            if (z >= 4)
            {
                var carta = Instantiate(cartas[z], cardsPositionb, showPosition);
                cardsPositionb = cardsPositionb + new Vector3(2, 0, 0);
            }

        }
    }

    // Update is called once per frame


    // 1. Que se de vuelta una carta, que mientras hay una carta dada vuelta , podes dar vuelta una carta más. Si las cartas tienen el mismo id entonces Desaparecen
    // si no vuelven a estar boca abajo
    void Update()
    {
        if (cartasRotadas < 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    CardRotate cartaElegida = hit.collider.GetComponent<CardRotate>();
                    StartCoroutine(cartaElegida.RotateOverTime(cartaElegida.rotation1, cartaElegida.rotation2, 1.0f, hit.transform.gameObject));
                    idsCartas[cartasRotadas] = cartaElegida.id;
                    cartasElegidas.Add(cartaElegida.transform.gameObject);
                    ++cartasRotadas;
                }
            }
        }

        if (cartasRotadas == 2)
        {
            if (idsCartas[0] == idsCartas[1])
            {
                // no funciona bien todavia
                StartCoroutine(UpOverTime(cartasElegidas[0].transform.position, new Vector3(cartasElegidas[0].transform.position.x, 1.0f * Time.deltaTime, cartasElegidas[0].transform.position.z), 3.0f, cartasElegidas[0]));
                StartCoroutine(UpOverTime(cartasElegidas[1].transform.position, new Vector3(cartasElegidas[1].transform.position.x, 1.0f * Time.deltaTime, cartasElegidas[0].transform.position.z), 3.0f, cartasElegidas[1]));
            }
            if(idsCartas[0] != idsCartas[1])
            {

                Debug.Log("Estas equivocadisimo.");
                cartasRotadas = 0;
                //no funciona bien todavia 
                StartCoroutine(RotateOverTime2(cartasElegidas[0].transform.rotation, hidePosition, 1.0f , cartasElegidas[0]));
                StartCoroutine(RotateOverTime2(showPosition, hidePosition, 1f  , cartasElegidas[1]));
            }


        }
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

    public IEnumerator RotateOverTime(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                this.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.rotation = finalRotation;
        isRotated = true;
    }

    public IEnumerator RotateOverTime2(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                this.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.rotation = finalRotation;
        isRotated = true;
    }
    public IEnumerator UpOverTime(Vector3 originalRotation, Vector3 finalRotation, float duration, GameObject card)
    {
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.position = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                this.transform.position = Vector3.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.position = finalRotation;
        isRotated = true;
    }
}
