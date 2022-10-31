using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SoftCollider : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    [SerializeField] float pushForce = 1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<SoftCollider>()) //if the other object also has a soft collider
        {
            rigidbody?.AddForce((transform.position - collision.transform.position).normalized * pushForce);
        }
    }

#if UNITY_EDITOR
    private Collider2D enviroCollider;

    private void Reset()
    {
        if(!enviroCollider)
        {
            Transform enviroObject = transform.Find("Environment Collision");
            if (!enviroObject)
            {
                enviroObject = new GameObject("Environment Collision", typeof(CircleCollider2D)).transform;
                enviroObject.SetParent(transform);
                enviroObject.position = transform.position;
            }
            enviroCollider = enviroObject.GetComponent<Collider2D>();
            if (!enviroCollider)
                enviroCollider = enviroObject.gameObject.AddComponent(GetComponent<Collider2D>());
        }
        enviroCollider.gameObject.layer = 9;
    }
#endif

}
