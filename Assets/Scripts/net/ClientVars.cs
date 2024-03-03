using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using JAFPS_API;
using System.Threading;

class ClientRoot
{
    static Socket connection;
    static byte[] buffer = new byte[1024];
    static EndPoint remoteEP;

    public static bool isConnected = false;
    static bool receivedResponse = false;
    public static short serverID = 0;
    static Process localServerProcess = new Process();
    static JAFPS_API.Parser parser = new Parser();
    static Mutex processMutex = new Mutex();
    static Mutex sendMutex = new Mutex();









    public static void StartDebugServer(string concat)
    {
        ProcessStartInfo serverStartInfo = new ProcessStartInfo();

        string path = UnityEngine.Application.dataPath;
        path = path.Substring(0, path.Length - 6);
        path += "ServerBuild/Debug/JAFPS_TestServer.exe";

        
        string args = "LOCAL " + concat;

        

        serverStartInfo.UseShellExecute = true;
        serverStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
        serverStartInfo.FileName = path;
        serverStartInfo.Arguments = args;

        localServerProcess.StartInfo = serverStartInfo;
        localServerProcess.Start();
    }
    public static void StopDebugServer()
    {
        if (isConnected && !localServerProcess.HasExited)
        {
            connection.Close();
            localServerProcess.Kill();
            isConnected = false;
        }
    }



    public static void ConnectToOwnServer()
    {
        connection = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        remoteEP = new IPEndPoint(SocketHelp.GetLocalAddress(), 9669);

        buffer = parser.Convert(new object[] { JAFPS_EVENTCODES.DEFAULT_CLIENTCOM });
        connection.BeginSendTo(buffer, 0, buffer.Length, 0, remoteEP, new AsyncCallback(AsyncSend), connection);

        while (!receivedResponse)
        {
            if (SocketHelp.HasMessage(connection))
            {
                UnityEngine.Debug.Log("whee!");

                buffer = new byte[1024];
                connection.BeginReceiveFrom(buffer, 0, 1024, 0, ref remoteEP, new AsyncCallback(AsyncReceive), connection);
                receivedResponse = true;
            }
        }
    }
    static void FinalizeConnection(object[] data)
    {
        if (!isConnected)
        {
            isConnected = true;
            serverID = (short)data[1];
            byte[] buff = parser.Convert(new object[] { serverID, JAFPS_EVENTCODES.DEFAULT_CLIENTCOM });
            connection.BeginSendTo(buff, 0, buff.Length, 0, remoteEP, new AsyncCallback(AsyncSend), connection);
            UnityEngine.Debug.Log("connected!");
        }
    }


    public static void CheckSocket()
    {
        if (SocketHelp.HasMessage(connection))
        {
            buffer = new byte[1024];
            connection.BeginReceiveFrom(buffer, 0, 1024, 0, ref remoteEP, new AsyncCallback(AsyncReceive), connection);
        }
    }

    static void ProcessData(object[] data)
    {
        if (processMutex.WaitOne())
        {
            switch ((short)data[0])
            {
                case (JAFPS_EVENTCODES.DEFAULT_SERVERCOM):
                    FinalizeConnection(data);
                    break;
                case (JAFPS_EVENTCODES.DEFAULT_DISCONNECTREQUEST):
                    UnityEngine.Debug.Log("disconnect request received");
                    isConnected = false;
                    StopDebugServer();
                    DestroyMutexes();
                    break;
                case (JAFPS_EVENTCODES.EVENT_TRANSFORM):
                    UnityEngine.Debug.Log("received transform update request");
                    RemoteTForm_Manager.Instance.AddAction(data);
                    break;
                case (JAFPS_EVENTCODES.EVENT_ANIM):
                    UnityEngine.Debug.Log("received anim update request");
                    RemoteAState_Manager.Instance.AddAction(data);
                    break;
                case (JAFPS_EVENTCODES.EVENT_OBJECT):
                    UnityEngine.Debug.Log("received object update request");
                    RemoteBodyManager.Instance.AddAction(data);
                    break;
                default:
                    break;
            }

            processMutex.ReleaseMutex();
        }
    }


    public static void SendToServer(byte[] buff)
    {
        if (sendMutex.WaitOne())
        {

            connection.BeginSendTo(buff, 0, buff.Length, 0, remoteEP, new AsyncCallback(AsyncSend), connection);
            sendMutex.ReleaseMutex();
            
        }
    }


    private static void AsyncSend(IAsyncResult result)
    {
        Socket s = result.AsyncState as Socket;
        s?.EndSendTo(result);
        UnityEngine.Debug.Log("Sent");
    }

    private static void AsyncReceive(IAsyncResult result)
    {
        Socket s = result.AsyncState as Socket;
        int buffSize = s.EndReceiveFrom(result, ref remoteEP);

        if(buffSize > 0)
        {
            byte[] received = new byte[buffSize];
            Buffer.BlockCopy(buffer, 0, received, 0, buffSize);

            object[] data = parser.Deconvert(received);
            ProcessData(data);
        }
    }



    public static void DestroyMutexes()
    {
        processMutex.Dispose();
        sendMutex.Dispose();
    }
}