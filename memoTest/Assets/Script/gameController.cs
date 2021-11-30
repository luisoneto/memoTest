using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    private int puntos;
    public AudioSource cardSlide2;
    public AudioSource cardSlide;
    public AudioSource popSound;
    public int cartasAcertadas;
    public List<GameObject> cartasClones = new List<GameObject>();
    public List<int> idsCartas = new List<int>(2);
    public List<GameObject> cartasElegidas = new List<GameObject>();
    public List<GameObject> cartas = new List<GameObject>();
    public List<Vector3> CardsVectors = new List<Vector3>();
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);   
    public int cartasRotadas = 0;

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


        // habia un if(!rotatedcards) antes de este if , pongo esto para no olvidarme.

        if (Input.GetMouseButtonDown(0))
        {
            ClickOnCard();
        }


        if (cartasRotadas == 2)
        {
            bool correctAnswer = false;
            if (idsCartas[0] == idsCartas[1])
            {
                correctAnswer = true;
                ChangeCardsState();
                PointsLogic(correctAnswer);
                CheckGameState();
            }

            else
            {
                Debug.Log("Estas equivocadisimo.");
                cartasRotadas = 0;
                PointsLogic(correctAnswer);
                RotateBothCards();
                Invoke("reproducirCartaSlide2", 1.35f);
                ChangeCardsStateToZero();
            }

            //for (int i = 0; i < idsCartas.Count; ++i)
            //{
            //    idsCartas[i] = 0;
            //}
        }

        
       
    }

    void ClickOnCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            CardRotate cartaElegida = hit.collider.GetComponent<CardRotate>();
            // si el collider es null que no haga nada, si no tira error.

            if(ExistTwoRotatedCards())
            {
                return;
            }

            if (cartaElegida != null && cartaElegida.state == 0)
            {
                StartCoroutine(RotateCard(hidePosition, showPosition, 0.5f, hit.transform.gameObject, cartaElegida));
                idsCartas[cartasRotadas] = cartaElegida.id;
                cartasElegidas.Add(cartaElegida.transform.gameObject);
                //delay a cartasRotadas++ para que haya un tiempo de espera cuando tenes las cartas correctas.
                Invoke("reproducirCartaSlide", 0.35f);
                Invoke("sumarCarta", 0.3f);
                cartaElegida.state = 2;
            }


        }
    }

    void RotateBothCards()
    {
        StartCoroutine(RotateOverTime(showPosition, hidePosition, 0.5f, cartasElegidas[0]));
        StartCoroutine(RotateOverTime2(showPosition, hidePosition, 0.5f, cartasElegidas[1]));
    }

    void PointsLogic(bool answer)

    {
        if(answer)
        {
            puntos = puntos + 3;
            var pointText = GameObject.Find("text_points").GetComponent<TMP_Text>();
            pointText.enabled = true;
            Vector3 OriginalPosition = pointText.transform.localPosition;
            cartasRotadas = 0;
            StartCoroutine(upText(pointText.transform.localPosition, new Vector3(pointText.transform.localPosition.x, 60, 0), 0.5f, pointText));
            StartCoroutine(DisappearOverTime(pointText.transform.localScale, 1.0f, pointText, OriginalPosition));

        }
        else
        {
            puntos = puntos - 1;
            var pointText = GameObject.Find("text_lostpoint").GetComponent<TMP_Text>();
            pointText.enabled = true;
            Vector3 OriginalPosition = pointText.transform.localPosition;
            cartasRotadas = 0;
            StartCoroutine(upText(pointText.transform.localPosition, new Vector3(pointText.transform.localPosition.x, 60, 0), 0.5f, pointText));
            StartCoroutine(DisappearOverTime(pointText.transform.localScale, 1.0f, pointText, OriginalPosition));

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
        cartasAcertadas++;

        if (MainMenuController.Dificultad == 1 && cartasAcertadas == 4)
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

    void ChangeCardsStateToZero()
    {
        cartasElegidas[0].GetComponent<CardRotate>().state = 0;
        cartasElegidas[1].GetComponent<CardRotate>().state = 0;
        cartasElegidas.Clear();
    }

    void ChangeCardsState()
    {
        // Pongo las cartas correctas en estado 3.
        int count = 0;
        for(int i = 0; i < cartasClones.Count; i++)
        {
            if(cartasClones[i].GetComponent<CardRotate>().cardNumber == cartasElegidas[count].GetComponent<CardRotate>().cardNumber)
            {
                cartasClones[i].GetComponent<CardRotate>().state = 3;
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


    public IEnumerator RotateCard(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card, CardRotate carta)
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
                card.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.rotation = finalRotation;
    }

    bool ExistTwoRotatedCards()
    {
        int count = 0;
        for (int i = 0; i < cartasClones.Count; i++)
        {
            if (cartasClones[i].GetComponent<CardRotate>().state == 2)
            {
                count++;
            }
        }
            if (count == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }  
}
