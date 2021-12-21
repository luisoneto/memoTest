using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public static int gameState;
    public AudioSource cardSlide2;
    public AudioSource cardSlide;
    public AudioSource popSound;
    public List<GameObject> cartasClones = new List<GameObject>();
    public List<int> idsCartas = new List<int>(2);
    public List<GameObject> cartasElegidas = new List<GameObject>();
    public List<GameObject> cartas = new List<GameObject>();
    public List<Vector3> CardsVectors = new List<Vector3>();
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);

    void Start()
    {
        MainMenuController.Dificultad = 2;

        StartCoroutine(EnableCardsColliders());

        RandomizeCardsPosition();

        CardsSpawn();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickOnCard();
        }

        if (CardsWaitingForValidate())
        {
            ValidateCards();
        }
    }

    void ValidateCards()
    {
        var puntajeController = GameObject.Find("Canvas").GetComponent<CanvasController>();
        puntajeController.PointsLogic(CardsIdAreEquals());
        cartasElegidas.Clear();
        CheckGameState();
        return;
    }

    void ClickOnCard()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            CardRotate cartaElegida = hit.collider.GetComponent<CardRotate>();

            if (ExistTwoRotatedCards())
            {
                return;
            }

            if (cartaElegida != null && cartaElegida.cardState == 1)
            {
                cartaElegida.StartRotatingCard();
                cartasElegidas.Add(cartaElegida.gameObject);
            }
        }
    }

    void CardsSpawn()
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

  

    void CheckGameState()
    {
        int CorrectCards = 0;
        for (int i = 0; i < cartasClones.Count; i++)
        {
            if (cartasClones[i].GetComponent<CardRotate>().CardIsRotated())
            {
                CorrectCards++;
            }
            if(CorrectCards == cartasClones.Count)
            {
                Debug.Log("Ganaste");
                gameState = 1;              
            }
        }
        CorrectCards = 0;
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

    bool ExistTwoRotatedCards()
    {
        int count = 0;
        for (int i = 0; i < cartasClones.Count; i++)
        {
            if (cartasClones[i].GetComponent<CardRotate>().cardState == 3 || cartasClones[i].GetComponent<CardRotate>().cardState == 2)
            {
                count++;
            }
            if(cartasClones[i].GetComponent<CardRotate>().cardState == 4)
            {
                return true;
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

    public bool CardsWaitingForValidate()
    {
        int rotatedCards = 0;

        for(int i = 0; i < cartasElegidas.Count; i++)
        {
            if (cartasElegidas[i].GetComponent<CardRotate>().CardIsRotated())
            {
                rotatedCards++;
            }
            if(rotatedCards == 2)
            {
                return true;
            }               
        }
            return false;
    }

    bool CardsIdAreEquals()
    {
        if (cartasElegidas[0].GetComponent<CardRotate>().id == cartasElegidas[1].GetComponent<CardRotate>().id)
        {                     
            cartasElegidas[0].GetComponent<CardRotate>().EsAcertada(true);
            cartasElegidas[1].GetComponent<CardRotate>().EsAcertada(true);
            return true;
        }
        else
        {
            cartasElegidas[0].GetComponent<CardRotate>().ReturnCardToShowPosition();
            cartasElegidas[1].GetComponent<CardRotate>().ReturnCardToShowPosition();
            cartasElegidas[0].GetComponent<CardRotate>().EsAcertada(false);
            cartasElegidas[1].GetComponent<CardRotate>().EsAcertada(false);
            return false;
        }
    }
}
