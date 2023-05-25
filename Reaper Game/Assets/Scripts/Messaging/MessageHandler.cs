using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Messaging
{
    public interface MessageHandler
    {
        public bool CanRecieveMessage<T>() where T : Message;

        public void InvokeMessage<T>(T message) where T: Message;
    }
}