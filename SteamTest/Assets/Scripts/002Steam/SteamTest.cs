using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//转为UTF8格式：
public class SteamTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!SteamManager.Initialized)
        {
            return;
        }
        string name = SteamFriends.GetPersonaName();
        Debug.Log(name);
        
    }
}
