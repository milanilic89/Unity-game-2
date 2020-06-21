using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// Restart level.
    /// </summary>
    public void btnRestart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    /// <summary>
    /// Exit app.
    /// </summary>
    public void applicationExit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Load run scene, Run
    /// </summary>
    public void LoadPlayScene()
    {
        GameObject map = GameObject.Find("Map");

        if (map == null)
            SceneManager.LoadScene("RunAndPlay", LoadSceneMode.Additive);
        else
        {
            GameObject startRunner = GameObject.Find("startRunner");

            if (startRunner != null)
                startRunner.GetComponent<Button>().onClick.Invoke();
        }
    }    
}
