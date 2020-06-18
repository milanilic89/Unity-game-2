using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int FirstLaunch = 0;

    void Start()
    {
        print("first lunch");
        if (this.FirstLaunch == 0)
        {
            print("run lanch");

            this.FirstLaunch = 1;

            //First launch
            PlayerPrefs.SetInt("FirstLaunch", 1);
            SceneManager.LoadScene("RunAndPlay", LoadSceneMode.Additive);
            SceneManager.LoadScene("GameSettings", LoadSceneMode.Additive);

        }
        else
        {
            //Load scene_02 if its not the first launch
            //SceneManager.LoadScene("RunAndPlay");
            print("already loaded");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
