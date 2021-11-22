using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public AudioSource cardSlide2;
    public AudioSource cardSlide;
    public AudioSource popSound;
    public int terminoDeRotar = 0;
    public int cartasAcertadas;
    public float speed = 1.0f;
    //public int[] idsCartas = new int[2];
    public bool isRotated;
    public List<int> idsCartas = new List<int>(2);
    public List<GameObject> cartasElegidas = new List<GameObject>();
    public List<GameObject> cartas = new List<GameObject>();
    public List<Vector3> CardsVectors = new List<Vector3>();
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);   
    public int cartasRotadas = 0;


    // Start is called before the first frame update
    void Start()
    {
        RandomizeCardsPosition();
        CardsPosition();

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
                    StartCoroutine(cartaElegida.RotateOverTime(hidePosition, showPosition, 0.5f, hit.transform.gameObject));
                    idsCartas[cartasRotadas] = cartaElegida.id;
                    cartasElegidas.Add(cartaElegida.transform.gameObject);                   
                    //delay a cartasRotadas++ para que haya un tiempo de espera cuando tenes las cartas correctas.
                    Invoke("reproducirCartaSlide", 0.35f);
                    Invoke("sumarCarta", 0.5f);
                }
            }
        }

        if (cartasRotadas == 2)
        {
            if (idsCartas[0] == idsCartas[1])
            {
                // La dejo comentada, por si se usa después.
                //StartCoroutine(UpOverTime(cartasElegidas[0].transform.position, new Vector3(cartasElegidas[0].transform.position.x, 7.0f, cartasElegidas[0].transform.position.z), 1.0f, cartasElegidas[0]));
                //StartCoroutine(UpOverTime2(cartasElegidas[1].transform.position, new Vector3(cartasElegidas[1].transform.position.x, 7.0f, cartasElegidas[1].transform.position.z), 1.0f, cartasElegidas[1]));
                cartasRotadas = 0;
                cartasAcertadas++;
                cartasElegidas.Clear();
                CheckGameState();
            }
            else 
            {
                Debug.Log("Estas equivocadisimo.");
                cartasRotadas = 0;
                StartCoroutine(RotateOverTime(showPosition, hidePosition, 0.5f , cartasElegidas[0]));
                StartCoroutine(RotateOverTime2(showPosition, hidePosition, 0.5f, cartasElegidas[1]));
                Invoke("reproducirCartaSlide2", 1.35f);
                //Resetear la lista
                cartasElegidas.Clear();

            }

            for(int i = 0; i < idsCartas.Count; ++i)
            {
                idsCartas[i] = 0;
            }
        }
    }


    void CardsPosition()
    {



        // public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
        // For para instanciar en dificultad facil - En el eje x se separan por 2, en en el Z por 1.0f.
        // empiezan en el eje x en -3. /
        

        if (MainMenuController.Dificultad == 1)
        {
            int count = 0;
            for (int z = 0; z < 2; z++)
            {

                for (int a = 0; a < 4; a++)
                {
                    // guardar vectores en una lista para usarlos como referencia en donde instanciar los gameObects.
                    var carta = Instantiate(cartas[count], CardsVectors[z], showPosition);
                    CardsVectors[z] = CardsVectors[z] + new Vector3(2, 0, 0);
                    count++;
                }

            }
        }

        if (MainMenuController.Dificultad == 2)
        {
            int count = 0;
            for (int z = 0; z < 4 ; z++)
            {
                
                for (int a = 0; a < 4; a++)
                {
                    // guardar vectores en una lista para usarlos como referencia en donde instanciar los gameObects.
                    var carta = Instantiate(cartas[count], CardsVectors[z], showPosition);
                    CardsVectors[z] = CardsVectors[z] + new Vector3(2, 0, 0);
                    count++;
                    if(count == 8)
                    {
                        count = 0;
                    }
                }

             }
        }

        if (MainMenuController.Dificultad == 3)
        {
            int count = 0;
            for (int z = 0; z < 4; z++)
            {

                for (int a = 0; a < 4; a++)
                {
                    // guardar vectores en una lista para usarlos como referencia en donde instanciar los gameObects.
                    var carta = Instantiate(cartas[count], CardsVectors[z], showPosition);
                    CardsVectors[z] = CardsVectors[z] + new Vector3(2, 0, 0);
                    count++;
                    if (count == 8)
                    {
                        count = 0;
                    }
                }

            }
        }

    }
    void reproducirCartaSlide2()
    {
        cardSlide2.Play();
    }
    void reproducirCartaSlide()
    {
        cardSlide.Play();
    }
    void sumarCarta()
    {
        ++cartasRotadas;
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

    IEnumerator RotateOverTime(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
    {
        yield return new WaitForSeconds(1);
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                card.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.rotation = finalRotation;
    }

    IEnumerator RotateOverTime2(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
    {
        yield return new WaitForSeconds(1);
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                card.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.rotation = finalRotation;
    }

    //cambiar nombre de metodo , desaparece no se va para arriba.
    IEnumerator UpOverTime(Vector3 originalRotation, Vector3 finalRotation, float duration, GameObject card)
    {
        //yield return new WaitForSeconds(2);
        if (duration > 0f)
        {
            popSound.Play();
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.position = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                card.transform.localScale = Vector3.Lerp(card.transform.localScale, new Vector3(0,0,0), progress);
                yield return null;
            }
        }
        card.transform.position = finalRotation;
        Destroy(card.transform.gameObject);
        
        
    }

    IEnumerator UpOverTime2(Vector3 originalRotation, Vector3 finalRotation, float duration, GameObject card)
    {
        popSound.Play();
        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            card.transform.position = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                card.transform.localScale = Vector3.Slerp(card.transform.localScale, new Vector3(0, 0, 0), progress);
                //card.transform.position = Vector3.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.position = finalRotation;
        Destroy(card.transform.gameObject);
    }

    void CheckGameState()
    {
       if(MainMenuController.Dificultad == 1 && cartasAcertadas == 4)
        {
            Debug.Log("Ganaste perrin!");
        }

        if (MainMenuController.Dificultad == 2 && cartasAcertadas == 8)
        {
            Debug.Log("Ganaste perrin!");
        }

    }
}
