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

        this.blockPath = new List<Transform>();
        this.movePath = new List<Transform>();

        if (blockPath != null)
            foreach (Transform t in blockPath)
                this.blockPath.Add(t);

        if (movePath != null)
            foreach (Transform t in movePath)
                this.movePath.Add(t);
    }
}
