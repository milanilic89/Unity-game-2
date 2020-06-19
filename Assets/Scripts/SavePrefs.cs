using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePrefs : MonoBehaviour
{

    private void Start()
    {
        PlayerPrefs.SetString("ReplayRun", "false");


        PlayerPrefs.SetInt("Score", 20);
        PlayerPrefs.SetString("Level", "test");

        PlayerPrefs.SetString("myString", "helpMe!");

    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(PlayerPrefs.HasKey("Score"))
            {
                Debug.Log("Player pref" + PlayerPrefs.GetInt("Score"));
            }
        }

        if (PlayerPrefs.HasKey("ReplayRun"))
        {
            //Debug.Log("ReplayRun = " + PlayerPrefs.GetString("ReplayRun"));
        }
    }

    //public void Set

}
