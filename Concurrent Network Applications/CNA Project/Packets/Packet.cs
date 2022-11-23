using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Runtime.Serialization;

namespace Packets
{
    public enum PacketType
    {
        CHAT_MESSAGE,
        PRIVATE_MESSAGE,
        CLIENT_NAME
    }

    [Serializable]
    public abstract class Packet
    {
        public PacketType m_PacketType { get; set; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string m_Message { get; set; }
        public ChatMessagePacket(string message)
        {
            m_Message = message;
            m_PacketType = PacketType.CHAT_MESSAGE;
        }
    }
}