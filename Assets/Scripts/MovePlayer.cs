using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private bool move = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (move)
        //    movePlayer();
    }


    public void startMove()
    {
        this.move = true;
    }

    public void movePlayer()
    {
        print("player has been moved");

        GameObject[] player1 = GameObject.FindGameObjectsWithTag("player");

        GameObject player = player1[0]; // take first player

        float deltaTime = 0.5f;

        if (player != null)
        {
            player.transform.Translate(1, 0, 0, Space.World);
        }

    }

}
