using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotate : MonoBehaviour
{
    // 1 = oculta, 2= rotando, 3 = rotada esperando volver, 4 = volviendo , 5 = acertada
    public int state = 0;
    public int cardNumber;
    public int id;
    public float speed = 1.0f;
    public Quaternion rotation1 = Quaternion.Euler(0, 0, 0);
    public Quaternion rotation2 = Quaternion.Euler(0, 0, 180);
    public Quaternion showPosition = Quaternion.Euler(0, 0, 180);
    public Quaternion hidePosition = Quaternion.Euler(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowCards(showPosition, hidePosition, 1f / speed));
    }

    void Update()
    {
        if(state == 2)
        {
            StartCoroutine(RotateCard(hidePosition, showPosition, 0.5f, this.transform.gameObject));
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
    }

}

