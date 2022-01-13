using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static int puntos = 0;
    public GameObject BlueCoin;
    public GameObject RedCoin;
    // Start is called before the first frame update
    void Start()
    {
        puntos = 10;
    }

    // Update is called once per frame
    void Update()
    {
        var totalPoints = GameObject.Find("text_totalPoints").GetComponent<TMP_Text>();
        totalPoints.text = puntos.ToString();

    }


    public void PointsLogic(bool answer)

    {
        var card = GameObject.Find("gameController").GetComponent<gameController>();
        Vector3 OriginalPosition = BlueCoin.transform.position;
        if (answer)
        {
            puntos = puntos + 3;
            var coin = Instantiate(BlueCoin, card.cartasElegidas[1].transform.position , BlueCoin.transform.rotation);
            StartCoroutine(upText(card.cartasElegidas[1].transform.position + new Vector3(0, 1.0f, 0.0f), card.cartasElegidas[1].transform.position + new Vector3(0, 1.5f, 1.0f), 1.0f, coin));
            StartCoroutine(DisappearOverTime(coin.transform.localScale, 1.0f, coin, OriginalPosition));
        }
        else
        {
            if(puntos == 0)
            {
                puntos = 0;
            }
            else
            {
                puntos = puntos - 1;
                var coin = Instantiate(RedCoin, card.cartasElegidas[1].transform.position, BlueCoin.transform.rotation);
                StartCoroutine(upText(card.cartasElegidas[1].transform.position + new Vector3(0, 1.0f, 0.0f), card.cartasElegidas[1].transform.position + new Vector3(0, 1.5f, 1.0f), 1.0f, coin));
                StartCoroutine(DisappearOverTime(coin.transform.localScale, 1.0f, coin, OriginalPosition));
            }
        }
    }


    IEnumerator upText(Vector3 originalPosition, Vector3 finalPosition, float duration, GameObject coin)
    {

        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            coin.transform.position = originalPosition;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                coin.transform.position = Vector3.Lerp(coin.transform.position, finalPosition, progress);
                yield return null;
            }
        }
        coin.transform.position = finalPosition;
    }

    IEnumerator DisappearOverTime(Vector3 originalScale, float duration, GameObject coin, Vector3 originalPosition)
    {

        yield return new WaitForSeconds(0.75f);
        if (duration > 0f)
        {
            //popSound.Play();
            float startTime = Time.time;
            float endTime = startTime + duration;
            //card.transform.position = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                coin.transform.localScale = Vector3.Slerp(coin.transform.localScale, new Vector3(0, 0, 0), progress);
                yield return null;
            }
        }
        Destroy(coin.gameObject);
    }
}
