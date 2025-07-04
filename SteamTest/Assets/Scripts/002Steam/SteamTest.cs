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


        // 检测用户是否拥有AppID为480的应用（Steam测试示例）
        AppId_t targetAppID = (AppId_t)480;
        bool isOwned = SteamApps.BIsSubscribedApp(targetAppID);

        if (isOwned)
        {
            Debug.Log("用户已拥有此应用！");
        }
        else
        {
            Debug.Log("用户未购买，跳转商店...");
            SteamFriends.ActivateGameOverlayToStore(targetAppID, EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCart);
        }

        //SteamUser.BLoggedOn
        //用于检测玩家是否通过steam客户端登录并完成身份验证

        // 检查是否拥有游戏
        //if (!SteamUser.BLoggedOn() ||
        //    !SteamApps.BIsSubscribedApp(123456)) // 123456 替换为你的AppID
        //{
        //    Debug.Log("未购买游戏！");
        //    SteamAPI.Shutdown();
        //    Application.Quit();
        //}
        //. 解锁成就​​
        SteamUserStats.SetAchievement("ACH_WIN_10_GAMES"); // 成就ID需在Steamworks后台配置
        SteamUserStats.StoreStats(); // 保存成就状态


        //SteamUtils.BOverlayNeedsPresent += (isActive) => {
        //    if (!isActive && IsInternetAvailable())
        //    { // 自定义网络检测
        //        RetryFailedAchievements(); // 触发重试
        //    }
        //};

        // 检查云存储是否启用
        if (!SteamRemoteStorage.IsCloudEnabledForApp())
        {
            Debug.LogWarning("云存储未启用！");
        }

        string fileName = "savegame.dat";
        byte[] data = System.Text.Encoding.UTF8.GetBytes("存档数据");
        // 异步写入文件
        // 使用FileWriteAsync避免卡顿
        SteamAPICall_t writeCall = SteamRemoteStorage.FileWriteAsync(fileName, data, (uint)data.Length);
        CallResult<RemoteStorageFileWriteAsyncComplete_t> callResult = CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create(OnFileWriteAsyncComplete);
        callResult.Set(writeCall);

        // 回调处理
        void OnFileWriteAsyncComplete(RemoteStorageFileWriteAsyncComplete_t callback, bool failure)
        {
            if (failure || callback.m_eResult != EResult.k_EResultOK)
            {
                Debug.LogError($"写入失败: {callback.m_eResult}");
            }
            else
            {
                Debug.Log("异步写入成功！");
            }
        }


        //// ​​2.云存档存储​​
        //// 写入存档
        //SteamRemoteStorage.FileWrite("save.dat", System.Text.Encoding.UTF8.GetBytes("存档数据"),1);

        // 读取存档
        //byte[] data = SteamRemoteStorage.FileRead("save.dat");
        //string saveData = System.Text.Encoding.UTF8.GetString(data);

    }
}
