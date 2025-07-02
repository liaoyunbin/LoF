using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Main:MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public GameObject server;
    public GameObject client;

    void Init()
    {
        if (server != null)
        {
            server.SetActive(true);
        }
        if (client != null)
        {
            client.SetActive(true); 
        }
    }

}
