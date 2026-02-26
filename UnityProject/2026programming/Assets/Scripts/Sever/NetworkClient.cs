using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkClient2D : MonoBehaviour
{
    public string host = "127.0.0.1";
    public int port = 7777;
    public string playerName = "Player";

    public GameObject playerPrefab;

    TcpClient client;
    NetworkStream stream;
    CancellationTokenSource cts;

    ConcurrentQueue<Action> mainThread = new();

    private async void Start()
    {
        await Connect();
    }

    private async Task Connect()
    {
        cts = new CancellationTokenSource();
        client = new TcpClient();
        await client.ConnectAsync(host, port);
        stream = client.GetStream();
    }

    private void Update()
    {
        while (mainThread.TryDequeue(out var a))
            a?.Invoke();
    }
}