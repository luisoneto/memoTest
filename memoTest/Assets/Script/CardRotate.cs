using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRotate : MonoBehaviour
{
    public int id;
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
        //void OnMouseOver()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit, 100))
        //        {
        //            StartCoroutine(RotateOverTime(rotation1, rotation2, 1f / speed, hit.transform.gameObject));
        //        }
                
        //    }
        //}           
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

    public IEnumerator RotateOverTime(Quaternion originalRotation, Quaternion finalRotation, float duration, GameObject card)
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
                this.transform.rotation = Quaternion.Slerp(originalRotation, finalRotation, progress);
                yield return null;
            }
        }
        card.transform.rotation = finalRotation;
        isRotated = true;
    }
}

