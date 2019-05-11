using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour {

    public Transform target;
    public float rocketTurnSpeed = 50.0f;
    public float rocketSpeed = 45.0f;
    public float damping = 0.99f;

    [Header("생존 시간"), Space(5)]
    public float objectLifeTimerValue = 10;

    private float accle = 0;
    private float timerSinceLaunch_Contor = 0;

    // Update is called once per frame
    void Update()
    {
        timerSinceLaunch_Contor += Time.deltaTime;

        if (target != null)
        {
            
            if (timerSinceLaunch_Contor > 1)
            {
                if (transform.eulerAngles.y > 180)
                    damping -= (target.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime;
                else
                    damping += (target.eulerAngles.y - transform.eulerAngles.y) * Time.deltaTime;
                
                rocketTurnSpeed = damping;
            }

            Vector3 direction = (target.position - transform.position).normalized; // 끝점과 원점을 빼서 방향 벡터를 구함

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rocketTurnSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
        }

        if (timerSinceLaunch_Contor > objectLifeTimerValue)
        {
            Destroy(transform.gameObject);
        }
    }
}
