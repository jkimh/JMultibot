using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Linq;
using System.Windows;

namespace JClientBot
{
    public enum PACKET_COMMAND
    {
        PACKET_CS_LOGIN = 0,
        PACKET_SC_LOGIN_ACK,
        PACKET_CS_LOGOUT,
        PACKET_CS_CHAT,
        PACKET_SC_CHAT,
        PACKET_CS_MOVE,
        PACKET_SC_MOVE,
        PACKET_SC_VIEW
    };

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PACKET_HEADER
    {
        [FieldOffset(0)] public PACKET_COMMAND command;
        [FieldOffset(4)] public uint size;
        public static byte[] Serialize(Object data)
        {
            try
            {
                int bufSize = Marshal.SizeOf(data);
                IntPtr unmanagedPointer = Marshal.AllocHGlobal(bufSize);
                Marshal.StructureToPtr(data, unmanagedPointer, false);
                byte[] managedArray = new byte[bufSize];
                Marshal.Copy(unmanagedPointer, managedArray, 0, bufSize);
                Marshal.FreeHGlobal(unmanagedPointer);
                
                /*
                var managedArray = new byte[((PACKET_HEADER)data).size];
                var gch = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
                var pBuffer = gch.AddrOfPinnedObject();
                Marshal.StructureToPtr(data, pBuffer, false);
                gch.Free();
                */
                return managedArray;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                return null;
            }
        }
        public static Object Deserialize(byte[] data, Type dataType)
        {
            int classSize = Marshal.SizeOf(dataType);
            if (classSize > data.Length)
                return null;

            IntPtr buffer = Marshal.AllocHGlobal(classSize);

            Marshal.Copy(data, 0, buffer, classSize);
            object objData = Marshal.PtrToStructure(buffer, dataType);
            Marshal.FreeHGlobal(buffer);
            return objData;
        }
    };
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_CS_LOGIN : PACKET_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(8)] public char[] commanderID = new char[32];  // 8 = sizeof(command) + sizeof(size)
    };
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_SC_LOGIN_ACK : PACKET_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(8)] public char[] commanderID = new char[32];
    };

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_CS_LOGOUT: PACKET_HEADER
    {
    };

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_CS_CHAT : PACKET_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        [FieldOffset(0)] public char[] chat = new char[128];
    };

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_SC_CHAT : PACKET_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        [FieldOffset(0)] public char[] chat = new char[128];
    };

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_CS_MOVE : PACKET_HEADER
    {
        [FieldOffset(0)] public Vector3 dest;
    };
    
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_SC_MOVE : PACKET_HEADER
    {
        [FieldOffset(0)] public Vector3 dest;
    };

    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public class PKS_SC_VIEW : PACKET_HEADER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(0)] public char[] commanderID1 = new char[32];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(32)] public char[] commanderID2 = new char[32];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(64)] public char[] commanderID3 = new char[32];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(96)] public char[] commanderID4 = new char[32];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        [FieldOffset(128)] public char[] commanderID5 = new char[32];
        [FieldOffset(160)] public Vector3 dest1;
        [FieldOffset(172)] public Vector3 dest2;
        [FieldOffset(184)] public Vector3 dest3;
        [FieldOffset(196)] public Vector3 dest4;
        [FieldOffset(208)] public Vector3 dest5;
    };
    
    
}
