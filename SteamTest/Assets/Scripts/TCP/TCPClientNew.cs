using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Accessibility;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 客户端连接与数据收发
/// </summary>
public class TCPClientNew:MonoBehaviour
{
    private TcpClient _client;
    private NetworkStream _stream;
    private byte[] _buffer = new byte[1024];

    public GameObject remoteBall; // 远程同步的小球
    public GameObject localBall; // 本地控制的小球
    private Vector3 _localBalltargetPosition;
    private Vector3 _remoteBalltargetPosition;

    private bool _isRunning = true;
    private int instanceId = 1;


    public void Start()
    {
        localBall.SetActive(true);
        // 连接服务器
        _client = new TcpClient();
        _client.Connect("127.0.0.1", 8888);
        _stream = _client.GetStream();

        Thread clientThread = new Thread(ReceiveCallbackNew);
        clientThread.Start();
        // 启动异步接收数据
        // _stream.BeginRead(_buffer, 0, _buffer.Length, ReceiveCallback, null);
    }

    private void ReceiveCallbackNew()
    {
        while (_isRunning)
        {
            int bytesRead = _stream.Read(_buffer, 0, _buffer.Length); //备注：还不能一直读。会有问题
            if (bytesRead == 0) break; // 客户端断开

            // 接收客户端输入（如小球位置）
           
            string message = Encoding.UTF8.GetString(_buffer, 0, bytesRead);
            int uid = GetInstanceId(message);
            Vector3 pos = ParsePosition(message);
            if(uid != instanceId)
            {
                _remoteBalltargetPosition = pos;
            }else
            {
                _localBalltargetPosition = pos;
            }
        }
    }
    // 接收服务器广播
    private void ReceiveCallback(IAsyncResult result)
    {
        Debug.Log("lorna aaaa");
        int bytesRead = _stream.EndRead(result);
        if (bytesRead > 0)
        {
            _buffer = (byte[])result.AsyncState;
            string message = Encoding.UTF8.GetString(_buffer, 0, bytesRead);
            // 解析位置并更新小球
            Vector3 pos = ParsePosition(message);

            Debug.Log("lorna ReceiveCallback" + pos);
            _localBalltargetPosition = pos;
            // 继续监听
            _stream.BeginRead(_buffer, 0, _buffer.Length, ReceiveCallback, null);
        }
    }

    // 发送本地输入（例如键盘控制）
    public void SendInput(Vector3 position)
    {
        string message = $"{position.x},{position.y},{position.z},{instanceId},";
        Debug.Log(message);
        byte[] data = Encoding.UTF8.GetBytes(message);
        _stream.Write(data, 0, data.Length);
    }

    private Vector3 ParsePosition(string message)
    {
        string[] parts = message.Split(',');
        return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
    }
    private int GetInstanceId(string message)
    {
        Debug.Log("messaghe"+ message);
        string[] parts = message.Split(',');
        return int.Parse(parts[3]);
}

    public void OnDestroy()
    {
        _stream?.Close();
        _client?.Close();
    }

    void Update()
    {
        SendInput();
        aa();
    }

    private void SendInput()
    {
        Vector3 offset = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            offset += Vector3.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            offset += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            offset += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            offset += Vector3.left;
        }
        offset = offset * Time.deltaTime * 100;
        Vector3 newPos = localBall.transform.position + offset;
        SendInput(newPos);
    }

    private float _timer;
    private const float FrameInterval = 0.05f; // 20 FPS
    private void aa()
    {
        _timer += Time.deltaTime;
        if (_timer >= FrameInterval)
        {
            _timer = 0;
            // 执行逻辑帧更新
            
        }
        ExecuteGameFrame();
    }

    private void ExecuteGameFrame()
    {
        localBall.transform.position = Vector3.Lerp(localBall.transform.position, _localBalltargetPosition, 0.2f);
        remoteBall.transform.position = Vector3.Lerp(remoteBall.transform.position, _remoteBalltargetPosition, 0.2f);
    }
}
