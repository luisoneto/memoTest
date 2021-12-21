using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballonMovement : MonoBehaviour
{
    public List<Material> Materials = new List<Material>();
    public float BobAngle;
    public float BobSpeed;
    float time = 0;
    public float amount;

    void Start()
    {

        GetComponent<Renderer>().material = Materials[Random.Range(0, Materials.Count)];
        GetComponent<Transform>().localScale = new Vector3(1, Random.Range(0.5f, 1), 1);
        time = 0;
        BobAngle = 10;
        BobSpeed = 2;
        amount = 0.02f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(Mathf.Sin(time * BobSpeed) * BobAngle, new Vector3(0, 1, 0));
        transform.position += transform.forward * amount;
    }

    void OnCollisionEnter(Collision colission)
    {
        Vector3 dir = (colission.gameObject.transform.position - gameObject.transform.position).normalized;

        if (Mathf.Abs(dir.z) < 0.05f)
        {
            if (dir.x > 0)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.left * 5f);
            }
            else if (dir.x < 0)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.right * 5f);
            }

            if(dir.y == 1)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.back * 5f);
            }

               
            if(dir.y == -1)
            {
                GetComponent<Rigidbody>().AddForce(Vector3.forward * 5f);
            }

        }

    }
}
