using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCheck : MonoBehaviour {
    
    [SerializeField]
    private GameObject[] find;
    // 승리 후 게임이 종료되는걸 받지 않는 조건문
    private bool gameEnd = false;

    [Space(5)]
    public AudioClip battleClip;
    // 승패 알려주는 캔버스
    [Space(5)]
    public GameObject RedCanvas, BlueCanvas;
    // 종료 대기시간
    [Space(5),Header("게임 종료 후 대기시간")]
    public float waitTime;


    private void Awake()
    {
        find = GameObject.FindGameObjectsWithTag("ATTACK_UNIT");
    }

    public void GameOver(string name)
    {
        for (int i = 0; i < find.Length; ++i)
            find[i].GetComponent<AttackUnit>().enabled = false;

        if(name == "Red" && !gameEnd)
        {
            gameEnd = true; // 두 플레이어가 거의 동시에 죽어도 한쪽만 승리하게 만듬
            BlueCanvas.SetActive(true);

            // 블루 플레이어가 존재하면 더 데미지를 안 받음
            if(GameObject.FindGameObjectWithTag("BluePlayer") != null)
                GameObject.FindGameObjectWithTag("BluePlayer").GetComponent<PlayerInfo>().takeDamage = false;

        }else if(name == "Blue" && !gameEnd)
        {
            gameEnd = true;
            RedCanvas.SetActive(true);
            
            if (GameObject.FindGameObjectWithTag("RedPlayer") != null)
                GameObject.FindGameObjectWithTag("RedPlayer").GetComponent<PlayerInfo>().takeDamage = false;

        }

        StartCoroutine("wait");
    }

    IEnumerator wait()
    {

        yield return new WaitForSeconds(waitTime); // 대기

        //현재 룸을 빠져나감
        PhotonNetwork.LeaveRoom();
    }

    //룸에서 접속 종료됐을 때 호출되는 콜백 함수
    void OnLeftRoom()
    {
        // 메뉴로 이동
        SceneManager.LoadScene(0);
    }

}
