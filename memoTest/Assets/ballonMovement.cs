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
        GetComponent<Renderer>().material = Materials[Random.Range(0 , 3)];
        time = 0;
        BobAngle = 10;
        BobSpeed = 2;
        amount = 0.01f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(Mathf.Sin(time * BobSpeed) * BobAngle, new Vector3(0, 1, 0));
        transform.position += transform.forward * amount;
    }

}
