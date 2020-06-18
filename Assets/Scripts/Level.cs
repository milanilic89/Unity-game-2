using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int level;

    public List<Transform> blockPath;
    
    public List<Transform> movePath;


    public Level(int level, List<Transform> blockPath, List<Transform> movePath)
    {
        this.level = level;

        this.blockPath = blockPath;

        this.movePath = movePath;
    }
}
