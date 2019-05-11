using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonInput : MonoBehaviour {
    
    public void quit()
    {
        StartCoroutine("hold");
    }

    public void SceneChange(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator hold()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

}
