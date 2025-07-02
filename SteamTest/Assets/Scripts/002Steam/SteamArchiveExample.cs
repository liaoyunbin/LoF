using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//转为UTF8格式：
public class SteamArchiveExample : MonoBehaviour
{

    private const string a = "ACH_WIN_ONE_GAME";
    // Start is called before the first frame update
    void Update()
    {
        if(!SteamManager.Initialized)
        {
            return;
        }
        if(!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        //true:重设统计数据和成就  false：只重设统计数据
        SteamUserStats.ResetAllStats(true);

        //设置成就、储存成就
        SteamUserStats.SetAchievement(a);
        SteamUserStats.StoreStats();    
        
    }
}
