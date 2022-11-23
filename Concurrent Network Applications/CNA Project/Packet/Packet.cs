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

namespace Packet
{
    enum PacketType
    {
        chatMessage,
        privateMessage,
        clientName
    }

    [Serializable]
    public abstract class Packet
    {
        public PacketType m_PacketType { get; protected set; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string m_Message { get; protected set; }
        public ChatMessagePacket(string message)
        {
            m_Message = message;
            m_PacketType = PacketType.chatMessage;
        }
    }
}