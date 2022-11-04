using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public enum AttackType { Primary, Secondary, Both }
    public delegate void AttackResponse(AttackInfo info);
    public delegate void DamageHandler(Collider2D collision);

    public struct AttackInfo
    {
        public AttackType type;
    }
}