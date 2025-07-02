using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// 创建服务器监听
/// </summary>
public class TCPServerNew : MonoBehaviour
{
    private TcpListener _server;
    private List<TcpClient> _clients = new List<TcpClient>();
    private Thread _serverThread;
    private bool _isRunning = true;
    private int m_instance = 0;

    public void Start()
    {
        // 监听本地 8888 端口
        _server = new TcpListener(IPAddress.Any, 8888);
        _server.Start();
        Debug.Log("服务器启动，等待客户端连接...");

        // 启动线程处理客户端连接
        _serverThread = new Thread(ListenForClients);
        _serverThread.IsBackground = true;
        _serverThread.Start();
    }

    // 监听客户端连接
    private void ListenForClients()
    {
        while (_isRunning)
        {
            TcpClient client = _server.AcceptTcpClient();
            _clients.Add(client);
            Debug.Log($"客户端 {client.Client.RemoteEndPoint} 已连接，当前连接数：{_clients.Count}");
            // 启动线程处理客户端数据
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    // 处理客户端数据
    private void HandleClient(object clientObj)
    {
        TcpClient client = (TcpClient)clientObj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        try
        {
            while (_isRunning)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break; // 客户端断开

                // 接收客户端输入（如小球位置）
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log($"收到消息: {message}");

                // 广播给所有客户端
                Broadcast(message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"客户端异常断开: {e.Message}");
        }
        finally
        {
            client.Close();
            _clients.Remove(client);
        }
    }

    // 广播消息
    private void Broadcast(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        foreach (TcpClient client in _clients)
        {
            if (client.Connected)
            {
                Debug.Log("广播" + message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
        }
    }

    public void OnDestroy()
    {
        _isRunning = false;
        _server.Stop();
        _serverThread?.Abort();
    }
}
