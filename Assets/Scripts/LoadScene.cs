using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int FirstLaunch = 0;

    void Start()
    {
        if (this.FirstLaunch == 0)
        {
            this.FirstLaunch = 1;

            //First launch
            PlayerPrefs.SetInt("FirstLaunch", 1);
            SceneManager.LoadScene("RunAndPlay", LoadSceneMode.Additive);
            SceneManager.LoadScene("GameSettings", LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
