using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// ��������������
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
        // �������� 8888 �˿�
        _server = new TcpListener(IPAddress.Any, 8888);
        _server.Start();
        Debug.Log("�������������ȴ��ͻ�������...");

        // �����̴߳���ͻ�������
        _serverThread = new Thread(ListenForClients);
        _serverThread.IsBackground = true;
        _serverThread.Start();
    }

    // �����ͻ�������
    private void ListenForClients()
    {
        while (_isRunning)
        {
            TcpClient client = _server.AcceptTcpClient();
            _clients.Add(client);
            Debug.Log($"�ͻ��� {client.Client.RemoteEndPoint} �����ӣ���ǰ��������{_clients.Count}");
            // �����̴߳���ͻ�������
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(client);
        }
    }

    // ����ͻ�������
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
                if (bytesRead == 0) break; // �ͻ��˶Ͽ�

                // ���տͻ������루��С��λ�ã�
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log($"�յ���Ϣ: {message}");

                // �㲥�����пͻ���
                Broadcast(message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"�ͻ����쳣�Ͽ�: {e.Message}");
        }
        finally
        {
            client.Close();
            _clients.Remove(client);
        }
    }

    // �㲥��Ϣ
    private void Broadcast(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        foreach (TcpClient client in _clients)
        {
            if (client.Connected)
            {
                Debug.Log("�㲥" + message);
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
