using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static int puntos = 0;
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
        string coinName;
        if(answer)
        {
            coinName = "GreenCoin";
            puntos = puntos + 3;
        }
        else
        {           
            coinName = "RedCoin";
            puntos = puntos - 1;
        }

        var coin = GameObject.Find(coinName).GetComponent<Image>();
        var parent = GameObject.Find("Canvas").GetComponent<RectTransform>();
        Vector3 OriginalPosition = coin.transform.localPosition;
        Image newCoin = Instantiate(coin, coin.transform.position,Quaternion.identity, parent );
        newCoin.enabled = true;
        StartCoroutine(upText(newCoin.transform.position, new Vector3(newCoin.transform.position.x, 200, 0), 1.5f, newCoin));
        StartCoroutine(DisappearOverTime(newCoin.transform.localScale, 1.0f, newCoin, OriginalPosition));   
    }


    IEnumerator upText(Vector3 originalPosition, Vector3 finalPosition, float duration, Image coin)
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

    IEnumerator DisappearOverTime(Vector3 originalScale, float duration, Image coin, Vector3 originalPosition)
    {

        yield return new WaitForSeconds(0.50f);
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
