using EscapeGame.Runtime.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//转为UTF8格式：
public class Boot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LogAnalyzer.Setup(true);
        UnityEngine.Debug.Log("Start");

    }
}
