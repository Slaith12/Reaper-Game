using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Movement;
using Reaper.Combat;
using System;

namespace Reaper.Enemy
{
    [CreateAssetMenu(fileName = "New Contacter", menuName = "Enemies/Contacter")]
    public class ContacterInfo : EnemyInfo
    {
        public float swingRange = 2;
    }
}