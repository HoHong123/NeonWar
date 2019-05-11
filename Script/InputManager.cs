using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    public bool isActive = false;

    private PhotonView pv = null;
    private Vector3 touchPosWorld;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        pv.synchronization = ViewSynchronization.UnreliableOnChange;
    }

    void Update()
    {
        if (pv.isMine && isActive)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            Touch[] touch = Input.touches;

            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if(touch[i].phase == TouchPhase.Began) //눌렀을 때만 발생
                        Shoot(touch[i].position);

                    break;
                }
            }

            if (Input.GetMouseButtonDown(0))
                Shoot(Input.mousePosition);
        }
    }

    void Shoot(Vector2 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1<<8))
        {
            if (hit.collider != null && hit.transform.gameObject.tag == "ATTACK_UNIT")
            {
                PhotonView attackUnitPV = hit.transform.gameObject.GetPhotonView();

                //터치한 AttakUnit오브젝트를 플레이어의 태그 정보에 따라 상태변화시킴
                attackUnitPV.RPC("ChangeUnitStateByPlayerInput", PhotonTargets.All, gameObject.tag);
                //if (gameObject.tag == "RedPlayer")
                //{
                //    attackUnitPV.RPC("ChangeUnitStateByPlayerInput", PhotonTargets.All, gameObject.tag);
                //}
                //else if (gameObject.tag == "BluePlayer")
                //{

                //}
            }
        }
    }
}