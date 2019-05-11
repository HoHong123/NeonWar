using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour {

    public bool check = true;
    [SerializeField]
    private float time, speed = 0.5f;

    Vector3 pointA;
    Vector3 pointB;

    private void Start()
    {
        if (check)
        {
            pointA = transform.position + new Vector3(0, 2, 0);
            pointB = transform.position + new Vector3(0, -2, 0);
        } else
        {
            pointA = transform.position + new Vector3(0, -2, 0);
            pointB = transform.position + new Vector3(0, 2, 0);
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time * speed, time));
    }

}
