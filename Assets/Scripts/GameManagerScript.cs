using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : Singleton
{
    public void DoLog(string log)
    {
        Debug.Log(log);
    }
}
