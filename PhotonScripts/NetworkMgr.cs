using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkMgr : MonoBehaviour {
    [Header("각 플레이어의 위치")]
    //player1의 위치
    public Transform playerOneTr;
    //Player2의 위치
    public Transform playerTwoTr;

    //Player1의 카메라
    public GameObject ARDeviceRed;
    //Player2의 카메라
    public GameObject ARDeviceBlue;

    //접속한 플레이어의 수
    private int currPlayerCount = 0;

    //GameManager오브젝트를 저장할 변수
    public GameObject GameManager;

    //'상대방 대기 중'UI
    public Canvas waitCanvas;

    private void Awake()
    {
        //플레이어 생성하는 함수 호출
        CreatePlayer();
        //포톤 클라우드의 네트워크 메시지 수신을 다시 연결
        PhotonNetwork.isMessageQueueRunning = true;
        //룸에 입장한 접속자의 정보를 조회
        GetConnectPlayerCount();

        if(currPlayerCount != 2)
        {
            waitCanvas.enabled = true;
        }
        else
        {
            StartCoroutine("Wait");
        }
    }

    //네트워크 플레이어가 접속했을 때 호출되는 함수
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        //룸에 입장한 접속자의 정보를 조회
        GetConnectPlayerCount();

        //접속자 수가 2명일 경우 게임시작을 위해 GameManager의 gameStart변수를 true로 바꿔줌.
        if(currPlayerCount == 2)
        {
            StartCoroutine("Wait");
        }
    }

    //네트워크 플레이어가 룸을 나가거나 접속이 끊어졌을 때 호출되는 함수
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {

    }

    //룸 접속자 정보를 조회하는 함수
    void GetConnectPlayerCount()
    {
        //현재 입장한 룸 정보를 받아옴.
        Room currRoom = PhotonNetwork.room;

        //현재 접속자 수
        currPlayerCount = currRoom.PlayerCount;
    }

    //플레이어를 생성하는 함수
    void CreatePlayer()
    {
        //플레이어 오브젝트 생성 위치 변수 선언
        Vector3 creationPos = Vector3.zero;

        //플레이어1의 위치 설정
        if (PhotonNetwork.player.NickName == "Player1")
        {
            creationPos = playerOneTr.position;
        }
        //플레이어2의 위치 설정
        else
        {
            creationPos = playerTwoTr.position;
        }

        //플레이어 프리팹 생성
        //플레이어1 설정
        if (PhotonNetwork.player.NickName == "Player1")
        {
            PhotonNetwork.Instantiate("RedPlayer", creationPos, Quaternion.identity, 0);
            ARDeviceRed.SetActive(true);
        }
        //플레이어2 설정
        else
        {
            PhotonNetwork.Instantiate("BluePlayer", creationPos, Quaternion.identity, 0);
            ARDeviceBlue.SetActive(true);
        }
    }

    IEnumerator Wait()
    {
        waitCanvas.GetComponentInChildren<Text>().fontSize = 250;

        int i = 5;
        while(i > 0)
        {
            waitCanvas.GetComponentInChildren<Text>().text = (i).ToString();
            yield return new WaitForSeconds(1);
            i--;
        }

        GameManager.GetComponent<AudioSource>().Stop();
        GameManager.GetComponent<AudioSource>().clip = GameManager.GetComponent<GameOverCheck>().battleClip;
        GameManager.GetComponent<AudioSource>().Play();

        waitCanvas.enabled = false;


        GameObject red = GameObject.FindGameObjectWithTag("RedPlayer");
        if (red != null)
            red.GetComponent<InputManager>().isActive = true;


        GameObject blue = GameObject.FindGameObjectWithTag("BluePlayer");
        if (blue != null)
            blue.GetComponent<InputManager>().isActive = true;

    }
}
