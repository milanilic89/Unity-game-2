﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveSystem
{
    //public static void SavePlayer(Player player)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    string path = Application.persistentDataPath + "/player.fun";
    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    List<Level> levelsHistory = new List<Level>();

    //    //PlayerData data = new PlayerData(player.level, 1, 7, 0);        

    //    formatter.Serialize(stream, "Ta-da!");
    //    stream.Close();
    //}

    //public string LoadPlayer()
    //{
    //    string path = Application.persistentDataPath + "/player.fun";

    //    if (File.Exists(path))
    //    {
    //        BinaryFormatter formatter = new BinaryFormatter();
    //        FileStream stream = new FileStream(path, FileMode.Open);

    //        //String data = formatter.Deserialize(stream) as String;

    //        stream.Close();

    //        return null;
    //    }
    //    else
    //    {
    //        Debug.LogError("Save file not found in " + path);
    //        return null;
    //    }
    //}

}
