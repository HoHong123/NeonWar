using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleLauncher : MonoBehaviour {

    public float delayTime;
    public GameObject missle;
    public Transform target;

    private float originDelay;

    private void OnEnable()
    {
        originDelay = delayTime;
    }
    
    void Update ()
    {
        if (originDelay < 0)
        {
            (Instantiate(missle, transform.position, transform.rotation) as GameObject).GetComponent<HomingMissle>().target = target;
            originDelay = delayTime;
        }
        else
        {
            originDelay -= Time.deltaTime;
        }
    }
}
