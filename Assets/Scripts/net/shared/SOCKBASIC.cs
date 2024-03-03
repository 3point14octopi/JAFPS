using System;
using System.Net;
using System.Net.Sockets;

namespace JAFPS_API
{
    public static class SocketHelp
    {
        public static bool HasMessage(Socket s, int timeToWait = 1)
        {
            return (s.Poll(timeToWait, SelectMode.SelectRead));
        }


        public static IPAddress GetLocalAddress()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList[1];
        }
    }
}