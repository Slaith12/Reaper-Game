using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Messaging
{
    public class NetCaptureMessage : Message
    {
        public NetCaptureMessage(Vector2 impactForce)
        {
            this.impactForce = impactForce;
        }

        public readonly Vector2 impactForce;
    }
}