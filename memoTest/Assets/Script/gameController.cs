using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public static int gameState = 0;
    public int gamestate2 = 0;
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

        CardsSpawn();

        RandomizeCardsPosition();

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

        var cartasDisponibles = cartas.ToList();
        int numeroCartaActual = 0;

        for (int cartasElegidas = 0; cartasElegidas < 8; cartasElegidas++)
        {
            var cartaElegida = cartasDisponibles[Random.Range(0, cartasDisponibles.Count)];
            cartasDisponibles.Remove(cartaElegida);

            agregarCarta(cartaElegida, numeroCartaActual);
            numeroCartaActual++;
            agregarCarta(cartaElegida, numeroCartaActual);
            numeroCartaActual++;
        }
    }

    void agregarCarta(GameObject cartaBase, int numeroCartaActual)
    {
        int fila = 0;
        if (numeroCartaActual >= 4 && numeroCartaActual < 8)
            fila = 1;
        if (numeroCartaActual >= 8 && numeroCartaActual < 12)
            fila = 2;
        if (numeroCartaActual >= 12)
            fila = 3;

        var carta = Instantiate(cartaBase, CardsVectors[fila], showPosition);
        cartasClones.Add(carta);
        carta.GetComponent<CardRotate>().cardNumber =  numeroCartaActual;
        CardsVectors[fila] = CardsVectors[fila] + new Vector3(2.5f, 0, 0);
    }

    void RandomizeCardsPosition()
    {
        for (int i = 0; i < 40; i++)
        {           
            int cardIndexOne = Random.Range(0, cartasClones.Count);
            int cardIndexTwo = Random.Range(0, cartasClones.Count);
            Vector3 cardOneposition = cartasClones[cardIndexOne].transform.position;
            cartasClones[cardIndexOne].transform.position = cartasClones[cardIndexTwo].transform.position;
            cartasClones[cardIndexTwo].transform.position = cardOneposition;
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
            cartasElegidas[0].GetComponent<CardRotate>().SetCardToFoundPosition();
            cartasElegidas[1].GetComponent<CardRotate>().SetCardToFoundPosition();
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
