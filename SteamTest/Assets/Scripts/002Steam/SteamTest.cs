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
        if (SteamAPI.RestartAppIfNecessary(new AppId_t(111)))
        {
            Debug.Log("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");

            //Application.Quit();
            //return;
        }

        if (!SteamManager.Initialized)
        {
            return;
        }
        string userName = SteamFriends.GetPersonaName();
        Debug.Log("Steam用户名: " + userName); // 成功则输出用户名

        // 检查是否拥有游戏
        //if (!SteamUser.BLoggedOn() ||
        //    !SteamApps.BIsSubscribedApp(123456)) // 123456 替换为你的AppID
        //{
        //    Debug.Log("未购买游戏！");
        //    SteamAPI.Shutdown();
        //    Application.Quit();
        //}
        //. 解锁成就​​
        //SteamUserStats.SetAchievement("ACH_WIN_10_GAMES"); // 成就ID需在Steamworks后台配置
        //SteamUserStats.StoreStats(); // 保存成就状态

        //// ​​2.云存档存储​​
        //// 写入存档
        //SteamRemoteStorage.FileWrite("save.dat", System.Text.Encoding.UTF8.GetBytes("存档数据"),1);

        // 读取存档
        //byte[] data = SteamRemoteStorage.FileRead("save.dat");
        //string saveData = System.Text.Encoding.UTF8.GetString(data);

    }
}
