using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviour {
    //App의 버전 정보
    public string version = "v1.0";
    //게임씬이름 지정
    [SerializeField]
    private string gameSceneName;
    //플레이어 아이디
    private string userId = null;
    

    private void Awake()
    {
        if(!PhotonNetwork.connected)
        {
            //포톤 클라우드에 접속
            PhotonNetwork.ConnectUsingSettings(version);
        }
    }

    //포톤 클라우드에 정상적으로 접속한 후 로비에 입장하면 호출되는 콜백 함수
    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby!");
    }

    //룸에 입장하면 호출되는 콜백함수
    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");

        if(userId == null)
        {
            //접속자(클라이언트) 아이디 설정
            userId = "Player2";
            PhotonNetwork.player.NickName = userId;
        }             

        StartCoroutine(this.LoadGameScene());
    }
	
    public void OnClickJoinRandomRoom()
    {
        //무작위로 추출된 룸으로 입장
        PhotonNetwork.JoinRandomRoom();
    }

    //무작위 룸 접속에 실패한 경우 호출되는 콜백함수
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms!");

        //생성할 룸의 조건 설정
        RoomOptions roomOpt = new RoomOptions();
        roomOpt.IsOpen = true;
        roomOpt.IsVisible = true;
        roomOpt.MaxPlayers = 2;

        //최초 접속자(호스트) 아이디 설정
        userId = "Player1";
        PhotonNetwork.player.NickName = userId;

        //룸 생성
        PhotonNetwork.CreateRoom(null, roomOpt, TypedLobby.Default);
    }

    IEnumerator LoadGameScene()
    {
        //씬을 이동하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        PhotonNetwork.isMessageQueueRunning = false;
        //백그라운드로 씬 로딩
        AsyncOperation ao = SceneManager.LoadSceneAsync("Test1");
        yield return ao;
    }

    private void OnGUI()
    {
        //화면 좌측 상단에 접속 과정에 대한 로그를 출력
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}