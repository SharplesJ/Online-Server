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
        CLIENT_NAME,
        CLIENTS_GET
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

    [Serializable]
    public class ClientNamePacket : Packet
    {
        public string m_LocalName { get; set; }
        public ClientNamePacket(string localName)
        {
            m_LocalName = localName;
            m_PacketType = PacketType.CLIENT_NAME;
        }
    }

    [Serializable]
    public class PrivateNamePacket : Packet
    {
        public string m_Message { get; set; }
        public string m_target { get; set; }
        public string m_LocalName { get; set; }
        public PrivateNamePacket(string message, string target, string localName)
        {
            m_target = target;
            m_Message = message;
            m_LocalName = localName;
            m_PacketType = PacketType.PRIVATE_MESSAGE;
        }
    }
}