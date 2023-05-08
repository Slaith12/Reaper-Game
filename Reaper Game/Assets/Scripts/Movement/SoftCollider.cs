using Reaper.Data;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SoftCollider : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private SoftColliderAttributes attributes;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        AttributeContainer container = GetComponent<AttributeContainer>();
        GetAttributes(container);
        container.OnAttributesChange += delegate { GetAttributes(container); };
    }

    private void GetAttributes(AttributeContainer container)
    {
        attributes = container.GetAttributes<SoftColliderAttributes>();
        if (attributes == null)
        {
            Debug.LogError($"Object {gameObject.name}'s attributes do not include attributes for the mover component. Using default attributes.");
            attributes = BasicAttributes.GetDefaultAttributes<SoftColliderAttributes>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<SoftCollider>()) //if the other object also has a soft collider
        {
            rigidbody?.AddForce((transform.position - collision.transform.position).normalized * attributes.pushForce);
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
