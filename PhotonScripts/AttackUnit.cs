using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUnit : MonoBehaviour
{

    //유닛의 상태변수
    public enum UnitState
    {
        Idle = 100, //중립상태
        Red, //Player1에게 터치된 상태
        Blue, //Player2에게 터치된 상태
    };

    //유닛의 상태변수를 할당
    public UnitState unitState;
    //PhotonView 컴포넌트를 저장할 변수
    private PhotonView pv = null;
    //공격대상
    public GameObject targetObj = null;
    //발사 주기
    private float timeForFire = 0.0f;
    //발사체
    public GameObject missileObj = null;
    public GameObject[] missils;

    public float m_lifeTime = 7f;
    public float m_speed = 3f;
    public float m_searchRange = 20f;
    public int m_searchAngle = 360;
    public bool m_canLooseTarget = false;
    public bool m_shotPermission = true;
    public float m_guidanceIntensity = 4.0f;
    public Vector3 m_targetOffset = Vector3.zero;

    public Material[] color;

    public int state;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        state = 0;

        //유닛 상태 초기화
        unitState = UnitState.Idle;

        //데이터 전송 타입을 설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
    }

    private void Update()
    {
        if (pv.isMine && targetObj != null && missileObj != null && m_shotPermission)
        {
            timeForFire += Time.deltaTime;

            if (timeForFire >= 2.0f)
            {
                //로컬 단말기에서의 유닛 공격처리
                Fire();
                //원격 단말기에서의 유닛 공격처리
                pv.RPC("Fire", PhotonTargets.Others, null);

                timeForFire = 0.0f;
            }
        }
        else
        {
            timeForFire = 0.0f;
        }
    }

    //지정된 타겟을 공격하는 함수
    [PunRPC]
    void Fire()
    {
        GameObject newProjectile = Instantiate(missileObj, transform.position, missileObj.transform.rotation) as GameObject;

        BulletDetection bullet = newProjectile.GetComponent<BulletDetection>();

        if (unitState == UnitState.Red)
        {
            bullet.TargetTag = "BluePlayer";
        }
        else if (unitState == UnitState.Blue)
        {
            bullet.TargetTag = "RedPlayer";
            newProjectile.transform.eulerAngles = new Vector3(0,180,0);
        }

        bullet.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * m_speed, ForceMode.Impulse);

        bullet.m_lifeTime = m_lifeTime;
        bullet.m_searchRange = m_searchRange;
        bullet.m_searchAngle = m_searchAngle;
        bullet.m_canLooseTarget = m_canLooseTarget;
        bullet.m_guidanceIntensity = m_guidanceIntensity;
        bullet.m_targetOffset = m_targetOffset;

    }

    //플레이어로부터 터치입력을 받을 경우 플레이어에 맞춰 상태 변경하는 함수
    [PunRPC]
    void ChangeUnitStateByPlayerInput(string playerName)
    {
        switch (playerName)
        {
            case "RedPlayer":
                unitState = UnitState.Red;
                targetObj = GameObject.FindGameObjectWithTag("BluePlayer");
                missileObj = missils[0];
                GetComponent<Renderer>().material = color[0];
                break;
            case "BluePlayer":
                unitState = UnitState.Blue;
                targetObj = GameObject.FindGameObjectWithTag("RedPlayer");
                missileObj = missils[1];
                GetComponent<Renderer>().material = color[1];
                break;
        }
    }
}
