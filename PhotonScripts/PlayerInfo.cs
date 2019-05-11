using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {
    //플레이어 체력
    private int playerHP;
    private int initHP = 100;
    //PhotonView컴포넌트를 저장할 변수
    private PhotonView pv;

    public string objectName;
    //폭발프리팹
    public GameObject explosionObj;

    //플레이어 하위의 Canvas 객체를 연결할 변수
    public Canvas hudCanvas;
    //Filled타입의 Image UI항목을 연결할 변수
    public Image hpBar;
    //플레이어 표식
    public Text playerMarkText;

    [HideInInspector]
    public bool takeDamage = true;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        pv.ObservedComponents.Add(this);

        playerHP = initHP;

        //Filled 이미지 색상을 녹색으로 설정
        hpBar.color = Color.green;

        if (pv.isMine)
        {
            playerMarkText.text = "YOU";
        }
        else
        {
            playerMarkText.text = "ENEMY";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //피격시 PlayerDamaged함수를 RPC로 호출!!
        if(playerHP > 0 && other.tag == "Missile")
        {
            //플레이어의 데미지 처리
            PlayerDamaged(2);

            //현재 생명치 백분율 = (현재 생명치) / (초기 생명치)
            hpBar.fillAmount = (float)playerHP / (float)initHP;

            //생명 수치에 따라 Filled 이미지의 색상을 변경
            if (hpBar.fillAmount <= 0.4f)
            {
                hpBar.color = Color.red;
            }
            else if (hpBar.fillAmount <= 0.6f)
            {
                hpBar.color = Color.yellow;
            }

            //플레이어의 체력이 0이하가 되면 파괴되도록 처리
            if (playerHP <= 0)
            {
                // 죽으면 게임 오버 체크에 객체의 죽음을 알림
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameOverCheck>().GameOver(objectName);
                
                StartCoroutine("PlayerDestroy");
            }
        }
    }

    //플레이어가 데미지를 받을 때 체력 감소 시키는 함수
    public void PlayerDamaged(int damageValue)
    {
        if (takeDamage)
        {
            playerHP -= damageValue;

            if (playerHP <= 0)
                playerHP = 0;
        }
    }

    //플레이어 체력이 0이 되었을 때의 콜백 함수
    IEnumerator PlayerDestroy()
    {
        GameObject destroyObj = Instantiate(explosionObj, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        Destroy(destroyObj, 1.0f);

        //hud 비활성화
        hudCanvas.enabled = false;

        yield return new WaitForSeconds(0);
    }
}
