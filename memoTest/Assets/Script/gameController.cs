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
        MainMenuController.Dificultad = 2;

        StartCoroutine(EnableCardsColliders());

        RandomizeCardsPosition();

        CardsPosition();

        puntos = 10;
    }
    void Update()
    {
        var totalPoints = GameObject.Find("text_totalPoints").GetComponent<TMP_Text>();
        totalPoints.text = "Puntos: " + puntos;

        if (Input.GetMouseButtonDown(0))
        {
            ClickOnCard();
        }

        if (ExistTwoCardsInStateThree())
        {
            if (CardsIdAreEquals())
            {
                PointsLogic(true);
                CheckGameState();
            }
            else
            {
                PointsLogic(false);
            }
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

            if (ExistTwoRotatedCards())
            {
                return;
            }

            if (cartaElegida != null && cartaElegida.state == 1)
            {
                cartaElegida.state = 2;
            }
        }
    }

    void PointsLogic(bool answer)

    {
        if (answer)
        {
            puntos = puntos + 3;
            var pointText = GameObject.Find("text_points").GetComponent<TMP_Text>();
            pointText.enabled = true;
            Vector3 OriginalPosition = pointText.transform.localPosition;
            StartCoroutine(upText(pointText.transform.localPosition, new Vector3(pointText.transform.localPosition.x, 60, 0), 0.5f, pointText));
            StartCoroutine(DisappearOverTime(pointText.transform.localScale, 1.0f, pointText, OriginalPosition));

        }
        else
        {
            puntos = puntos - 1;
            var pointText = GameObject.Find("text_lostpoint").GetComponent<TMP_Text>();
            pointText.enabled = true;
            Vector3 OriginalPosition = pointText.transform.localPosition;
            StartCoroutine(upText(pointText.transform.localPosition, new Vector3(pointText.transform.localPosition.x, 60, 0), 0.5f, pointText));
            StartCoroutine(DisappearOverTime(pointText.transform.localScale, 1.0f, pointText, OriginalPosition));

        }


    }
    void CardsPosition()
    {





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
            for (int z = 0; z < 4; z++)
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
                    if (count == 8)
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
                text.transform.localPosition = Vector3.Lerp(text.transform.localPosition, new Vector3(text.transform.localPosition.x, 60, 0), progress);
                yield return null;
            }
        }
    }

    IEnumerator DisappearOverTime(Vector3 originalScale, float duration, TMP_Text text, Vector3 originalPosition)
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
            if (cartasClones[i].GetComponent<CardRotate>().state == 2 || cartasClones[i].GetComponent<CardRotate>().state == 4)
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

    bool ExistTwoCardsInStateThree()
    {
        // No funciona ! , Si no puedo solucionarlo voy a almacenar directamente el id de las cartas elegidas y despues compararlo.
        int count = 0;
        for (int i = 0; i < cartasClones.Count; i++)
        {
            if (cartasClones[i].GetComponent<CardRotate>().state == 3)
            {
                cartasElegidas[count] = cartasClones[i];
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

    bool CardsIdAreEquals()
    {
        if (cartasElegidas[0].GetComponent<CardRotate>().id == cartasElegidas[1].GetComponent<CardRotate>().id)
        {
            cartasElegidas[0].GetComponent<CardRotate>().state = 5;
            cartasElegidas[1].GetComponent<CardRotate>().state = 5;
            return true;
        }
        else
        {
            cartasElegidas[0].GetComponent<CardRotate>().state = 4;
            cartasElegidas[1].GetComponent<CardRotate>().state = 4;
            return false;
        }
    }
}
