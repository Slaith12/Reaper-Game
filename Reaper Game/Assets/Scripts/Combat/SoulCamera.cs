using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reaper.Enemy;

namespace Reaper.Combat
{
    //public class SoulCamera : Weapon
    //{
    //    //[SerializeField] Vector2 size = new Vector2(4, 3);
    //    //[SerializeField] float interval = 1;
    //    //private float attackCooldown;
    //    //public int flashes;
    //    //private bool canCapture;

    //    //private void Update()
    //    //{
    //    //    if (attackCooldown > 0)
    //    //        attackCooldown -= Time.deltaTime;
    //    //}

    //    //public override void Attack(Vector2 facing)
    //    //{
    //    //    if (attackCooldown > 0 || flashes <= 0)
    //    //        return;
    //    //    canCapture = true;
    //    //    MeleeHit.Create(0.5f, Capture, (Vector2)transform.position + facing, size, new List<string> { "Soul" }, rotation: facing.ToAngle());
    //    //    attackCooldown = interval;
    //    //    flashes--;
    //    //}

    //    //private void Capture(Collider2D collision)
    //    //{
    //    //    Soul soul = collision.GetComponent<Soul>();
    //    //    if(soul.state == EnemyInfo.STATE_UNMORPHED)
    //    //    {
    //    //        if (canCapture)
    //    //        {
    //    //            Destroy(soul.gameObject);
    //    //            canCapture = false;
    //    //        }
    //    //    }
    //    //}
    //}
}
