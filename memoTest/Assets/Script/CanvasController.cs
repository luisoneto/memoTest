using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        totalPoints.text = "Puntos: " + puntos;
    }


    public void PointsLogic(bool answer)

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
            //popSound.Play();
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
}
