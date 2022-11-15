using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Messaging
{
    public interface IMessageHandler
    {
        public bool CanRecieveMessage(string message);

        public void InvokeMessage(string message);
    }
}