using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotate : MonoBehaviour
{
    // 1 = oculta, 2= rotando, 3 = rotada esperando volver, 4 = volviendo , 5 = acertada
    public bool Acertada;
    public int cardState = 0;
    public int cardNumber;
    public int id;
    public float speed = 1.0f;
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowCards(showPosition, hidePosition, 1f / speed));
    }

    void Update()
    {
        if(cardState == 3 && Acertada == true)
        {
            cardState = 5;
        }
    }
    IEnumerator ShowCards(Quaternion originalRotation, Quaternion finalRotation, float duration)
    {

        yield return new WaitForSeconds(2);

        if (duration > 0f)
        {
            float startTime = Time.time;
            float endTime = startTime + duration;
            this.transform.rotation = originalRotation;
            yield return null;
            while (Time.time < endTime)
            {
                float progress = (Time.time - startTime) / duration;
                // progress will equal 0 at startTime, 1 at endTime.
                this.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        this.transform.rotation = finalRotation;
        cardState = 1;
    }


    public void StartRotatingCard()
    {
        cardState = 2;
        StartCoroutine(RotateCard(hidePosition, showPosition, 0.5f, transform.gameObject));
    }

    public void ReturnCardToShowPosition()
    {
        cardState = 4;
        StartCoroutine(RotateOverTime(showPosition, hidePosition, 0.5f, transform.gameObject));
    }


    public IEnumerator RotateCard(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
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
        cardState = 3;
    }

    public IEnumerator RotateOverTime(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
    {
        yield return new WaitForSeconds(0.5f);
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
        cardState = 1;
    }



    // no encuentro un nombre para ese bool le puse ''a''
    public void EsAcertada(bool respuesta)
    {
        Acertada = respuesta;
    }


    public bool CardIsRotated()
    {
        if(this.transform.rotation == showPosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

