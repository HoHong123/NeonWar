using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoController : MonoBehaviour {

    private float timer = 0.0f;

	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if(timer >= 1.5f)
        {
            SceneManager.LoadScene(1);
            timer = 0;
        }
	}
}



