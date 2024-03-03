using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;

#if UNITY_EDITOR
#pragma warning disable CS8632
#endif

namespace JAFPS_API
{
    public class JAFPS_EVENTCODES
    {
        public const short ERROR_BREAK = 0;
        public const short DEFAULT_SERVERCOM = 1;
        public const short DEFAULT_CLIENTCOM = 2;
        public const short DEFAULT_DISCONNECTCONFIRM = 3;
        public const short DEFAULT_DISCONNECTREQUEST = 4;


        public const short EVENT_TRANSFORM = 5;
        public const short EVENT_ANIM = 6;
        public const short EVENT_OBJECT = 7;
    }

    public class ECHO_EVENTCODES
    {
        public const short ECHO = 1;
        public const short WELCOME = 2;
    }

    public class CTFORM_EVENTCODES
    {
        public const short MY_UPDATE = 1;//serv
        public const short REMOTE_UPDATE = 2;//cli
        public const short REQUEST_UPDATE = 3;
    }

    public class OBJANIM_EVENTCODES
    {
        public const short MY_UPDATE = 1;
        public const short REMOTE_UPDATE = 2;
        public const short REQUEST_UPDATE = 3;
        public const short PREFAB_UPDATE = 4;
    }

    public class REMOTEOBJ_EVENTCODES
    {
        public const short INIT_OBJECT = 1;
        public const short CONFIRM_INIT = 2;
        public const short NEW_OBJ = 3;
        public const short MY_UPDATE = 4;
        public const short REMOTE_UPDATE = 5;
        public const short PAUSE = 6;
        public const short RESUME = 7;
        public const short DESTROY = 8;
    }

    public static class JAFPS_CONVERSIONCODES
    {
        


        private static readonly Dictionary<string, short> conversionCodesByType = new Dictionary<string, short>
        {
            {"System.Int16",      1},
            {"System.Int32",      2},
            {"System.Single",     3},
            {"System.Double",     4},
            {"System.String",     5},
            {"System.Int16[]",    6},
            {"System.Int32[]",    7},
            {"System.Single[]",   8},
            {"System.Double[]",   9},
            {"System.String[]",  10}
        };
        private static readonly Dictionary<short, Type> typeByConversionCode = new Dictionary<short, Type>
        {
            {1,   typeof(System.Int16)},
            {2,   typeof(System.Int32)},
            {3,   typeof(System.Single)},
            {4,   typeof(System.Double)},
            {5,   typeof(System.String)},
            {6,   typeof(System.Int16[])},
            {7,   typeof(System.Int32[])},
            {8,   typeof(System.Single[])},
            {9,   typeof(System.Double[]) },
            {10,  typeof(System.String[]) }
        };

        public static short CodeForName(string name)
        {
            short ret = 0;
            conversionCodesByType.TryGetValue(name, out ret);
            return ret;
        }

        public static Type? GetTypeFromCode(short code)
        {
            Type? t = null;

            typeByConversionCode.TryGetValue(code, out t);
            return t;
        }
        
    }
}