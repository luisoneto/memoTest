using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotate : MonoBehaviour
{
    public bool isRotated;
    public float speed = 1.0f;
    public Quaternion rotation1 = Quaternion.Euler(0, 0, 0);
    public Quaternion rotation2 = Quaternion.Euler(0, 0, 180);
    public Quaternion rotationShowCard = Quaternion.Euler(0, 0, 180);
    public Quaternion rotationHideCard = Quaternion.Euler(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(ShowCards(rotationShowCard, rotationHideCard, 1f / speed));
    }
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)) && !isRotated)
        {
            print(this.transform.rotation.eulerAngles.x);
            StartCoroutine(RotateOverTime(rotation1, rotation2, 1f / speed));
        }
    }

    IEnumerator RotateOverTime(Quaternion originalRotation, Quaternion finalRotation, float duration)
    {
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
        isRotated = true;
    }

    IEnumerator ShowCards(Quaternion originalRotation, Quaternion finalRotation, float duration)
    {
        yield return new WaitForSeconds(5);

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
}
