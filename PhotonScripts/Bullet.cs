using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    
    public GameObject targetObj;
    public float rocketTurnSpeed = 50.0f;
    public float rocketSpeed = 20.0f;
    public float damping = 0.99f;

    [Header("생존 시간"), Space(5)]
    public float objectLifeTimerValue = 7;

    private Transform targetTrans;
    private float accle = 0;
    private float timerSinceLaunch_Contor = 0;

    private void Awake()
    {
        targetTrans = targetObj.transform;
        transform.TransformDirection(-targetTrans.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetObj.tag)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timerSinceLaunch_Contor += Time.deltaTime;

        if (targetObj != null)
        {

            if (timerSinceLaunch_Contor > 1)
            {
                if (transform.eulerAngles.y > 180)
                    damping -= (targetTrans.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime;
                else
                    damping += (targetTrans.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime;

                rocketTurnSpeed = damping;
            }

            Vector3 direction = (targetTrans.position - transform.position).normalized; // 끝점과 원점을 빼서 방향 벡터를 구함

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rocketTurnSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
        }

        if (timerSinceLaunch_Contor > objectLifeTimerValue && targetObj != null)
        {
            Destroy(transform.gameObject);
        }
    }
}
