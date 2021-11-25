using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    private bool rotatedCards;
    private int puntos;
    public AudioSource cardSlide2;
    public AudioSource cardSlide;
    public AudioSource popSound;
    public int terminoDeRotar = 0;
    public int cartasAcertadas;
    public float speed = 1.0f;
    //public int[] idsCartas = new int[2];
    public bool isRotated;
    public List<GameObject> cartasClones = new List<GameObject>();
    public List<int> idsCartas = new List<int>(2);
    public List<GameObject> cartasElegidas = new List<GameObject>();
    public List<GameObject> cartas = new List<GameObject>();
    public List<Vector3> CardsVectors = new List<Vector3>();
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);   
    public int cartasRotadas = 0;

    // Start is called before the first frame update
    // Bugs a arreglar : Me está dejando dar vuelta todas las cartas que quiero...
    void Start()
    {
        // EnableCardsColliders es para que cuando empieza la partida y te muestra las cartas, no te deje darlas vueltas cuando les hagas click entonces
        // les saco el collider un ratito para que no puedas interactuar con ellas.
        StartCoroutine(EnableCardsColliders());

        RandomizeCardsPosition();

        CardsPosition();

        puntos = 10;
    }
    void Update()
    {        
        //cantidad de puntos que tiene el jugador.
        var totalPoints = GameObject.Find("text_totalPoints").GetComponent<TMP_Text>();
        totalPoints.text = "Puntos: " + puntos;


        if (cartasRotadas == 2)
        {
            rotatedCards = true;

            if (idsCartas[0] == idsCartas[1])
            {
                puntos = puntos + 3;
                // desactivo el collider para que no le puedas hacer click de vuelta.
                Invoke("DisableCollider", 1.0f);
                cartasAcertadas++;
                CheckGameState();
                PointsGained();
                // Activo los colliders otra vez una vez que las animaciones de las cartas terminen.
                Invoke("ActiveColliders", 1.0f);
            }

            else
            {
                Debug.Log("Estas equivocadisimo.");
                cartasRotadas = 0;
                StartCoroutine(RotateOverTime(showPosition, hidePosition, 0.5f, cartasElegidas[0]));
                StartCoroutine(RotateOverTime2(showPosition, hidePosition, 0.5f, cartasElegidas[1]));
                Invoke("reproducirCartaSlide2", 1.35f);
                // Le devuelvo el collider, si son incorrectas podes volver a elegirlas.
                Invoke("ReturnCollider", 0.5f);

                rotatedCards = false;
            }

            for (int i = 0; i < idsCartas.Count; ++i)
            {
                idsCartas[i] = 0;
            }
        }


        if (!rotatedCards)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    CardRotate cartaElegida = hit.collider.GetComponent<CardRotate>();
                    // si el collider es null que no haga nada, si no tira error.
                    if(cartaElegida != null)
                    {
                        StartCoroutine(cartaElegida.RotateOverTime(hidePosition, showPosition, 0.5f, hit.transform.gameObject));
                        // Les saco el collider para que no las puedas dar vuelta otra vez
                        cartaElegida.GetComponent<Collider>().enabled = false;

                        idsCartas[cartasRotadas] = cartaElegida.id;
                        cartasElegidas.Add(cartaElegida.transform.gameObject);
                        //delay a cartasRotadas++ para que haya un tiempo de espera cuando tenes las cartas correctas.
                        Invoke("reproducirCartaSlide", 0.35f);
                        Invoke("sumarCarta", 0.3f);
                    }


                }
            }
        }
       
    }

    void PointsGained()
    {
        
        var pointText = GameObject.Find("text_points").GetComponent<TMP_Text>();
        pointText.enabled = true;
        Vector3 OriginalPosition = pointText.transform.localPosition;
        cartasRotadas = 0;
        StartCoroutine(upText(pointText.transform.localPosition, new Vector3(pointText.transform.localPosition.x, 60, 0), 0.5f, pointText));
        StartCoroutine(DisappearOverTime(pointText.transform.localScale,1.0f, pointText,OriginalPosition));
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
                    cartasClones.Add(carta);
                    cartasClones[count].GetComponent<CardRotate>().cardNumber = count;
                    CardsVectors[z] = CardsVectors[z] + new Vector3(2, 0, 0);
                    count++;
                }

            }
        }

        if (MainMenuController.Dificultad == 2)
        {
            int count = 0;
            int cardNumber = 0;
            for (int z = 0; z < 4 ; z++)
            {
                
                for (int a = 0; a < 4; a++)
                {
                    // guardar vectores en una lista para usarlos como referencia en donde instanciar los gameObects.
                    var carta = Instantiate(cartas[count], CardsVectors[z], showPosition);
                    cartasClones.Add(carta);
                    cartasClones[cardNumber].GetComponent<CardRotate>().cardNumber = cardNumber;
                    CardsVectors[z] = CardsVectors[z] + new Vector3(2, 0, 0);
                    count++;
                    cardNumber++;
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
    IEnumerator upText(Vector3 originalPosition, Vector3 finalPosition, float duration, TMP_Text text)
    {
        if (duration > 0f)
        {

            float startTime = Time.time;
            float endTime = startTime + duration;
            text.transform.localPosition = originalPosition;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                text.transform.localPosition = Vector3.Lerp(text.transform.localPosition, new Vector3(text.transform.localPosition.x,60,0), progress);
                yield return null;
            }
        }
    }

    IEnumerator DisappearOverTime(Vector3 originalScale,float duration, TMP_Text text, Vector3 originalPosition)
    {
        
        yield return new WaitForSeconds(0.40f);
        if (duration > 0f)
        {
            popSound.Play();
            float startTime = Time.time;
            float endTime = startTime + duration;
            //card.transform.position = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                text.transform.localScale = Vector3.Slerp(text.transform.localScale, new Vector3(0, 0, 0), progress);
                yield return null;
            }
        }

        text.enabled = false;
        text.transform.localPosition = originalPosition;
        text.transform.localScale = originalScale;
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

    IEnumerator EnableCardsColliders()
    {


        for (int carta = 0; carta < cartasClones.Count; carta++)
        {
            cartasClones[carta].GetComponent<Collider>().enabled = false;
        }

        yield return new WaitForSeconds(3);


        for (int carta = 0; carta < cartasClones.Count; carta++)
        {
            cartasClones[carta].GetComponent<Collider>().enabled = true;
        }
    }

    void ReturnCollider()
    {
        cartasElegidas[0].GetComponent<Collider>().enabled = true;
        cartasElegidas[1].GetComponent<Collider>().enabled = true;
        cartasElegidas.Clear();
    }

    void DisableCollider()
    {

        // La razón por la cual hago esto, es porque cuando las cartas se empiezan a repetir, con el metodo anterior
        // deshabilitaba el collider de todas las cartas con el id, entonces si habia 4 azules se deshabilitaban las otras dos
        // mas allá de que no estén seleccionadas, agregué una variable cardNumber que es unica y personal de cada carta
        // asi solamente le deshabilito el collider a las cartas que necesito.

        int count = 0;
        for(int i = 0; i < cartasClones.Count; i++)
        {
            if(cartasClones[i].GetComponent<CardRotate>().cardNumber == cartasElegidas[count].GetComponent<CardRotate>().cardNumber)
            {
                cartasClones[i].GetComponent<Collider>().enabled = false;
                count++;
                i = 0;
            }

            if(count == 2)
            {
                cartasElegidas.Clear();
                break;
            }
        }
    }

    void ActiveColliders()
    {
        rotatedCards = false;
    }
}
