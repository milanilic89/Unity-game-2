using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int level;
    public List<Transform> blockPath;
    //public List<Transform> movePath;
    public List<string> movePath;
    public string algoName;


    public Level(int level, List<Transform> blockPath, List<string> movePath, string algoName)
    {
        this.level = level;

        this.blockPath = new List<Transform>();
        this.movePath = new List<string>();

        if (blockPath != null)
            foreach (Transform t in blockPath)
                this.blockPath.Add(t);

        if (movePath != null)
            foreach (string t in movePath)
                this.movePath.Add(t);

        this.algoName = algoName;
    }
}
