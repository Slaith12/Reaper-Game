using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Combat
{
    [CreateAssetMenu(menuName = "Weapons/Restrainer")]
    public class Restrainer : Weapon
    {
        public override bool primaryHasHold => true;
        public override bool secondaryHasHold => false;
    }
}