using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Attack(Vector2 facing);
    }
}
