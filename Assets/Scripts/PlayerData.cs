using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData 
{

    public int level;

    public float[] position;

    public PlayerData(int _level, int x, int y, int z)
    {
        level = _level;

        position = new float[3];

        position[0] = x;
        position[1] = y;
        position[2] = z;

        //position[0] = player.transform.position.x;
        //position[1] = player.transform.position.y;
        //position[2] = player.transform.position.z;
    }


}
